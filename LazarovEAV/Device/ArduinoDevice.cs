using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;
using LazarovEAV.Config;
using System.Text;

namespace LazarovEAV.Device
{
    /// <summary>
    /// 
    /// </summary>
    class ArduinoDeviceInfo : EavDeviceInfo
    {
        public DEVICE_TYPE DeviceType => DEVICE_TYPE.ARDUINO;

        public string Description { get; }
        public string PortName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="description"></param>
        public ArduinoDeviceInfo(string port, string description)
        {
            this.Description = description;
            this.PortName = port;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    class ArduinoDevice : IEavDevice
    {
        private readonly SerialPort serialPort = new SerialPort();
        private readonly ArduinoDeviceInfo devInfo;

        /// <summary>
        /// 
        /// </summary>
        public ArduinoDevice(EavDeviceInfo dev)
        {
            if (dev.DeviceType != DEVICE_TYPE.ARDUINO)
            {
                throw new Exception("Invalid EavDeviceInfo configuration object for Arduino based device");
            }

            this.serialPort.ErrorReceived += (object sender, SerialErrorReceivedEventArgs e) =>
            {
                try { this.serialPort.Close(); } catch (Exception) { }
            };

            this.devInfo = (ArduinoDeviceInfo)dev;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<EavDeviceInfo> getDevices()
        {
            var vidPids = new List<string[]>() {
                        new string[]{ "2341", "0043" },
                        new string[]{ "1A86", "7523" }
                    };

            List<EavDeviceInfo> ports;
            List<string> matchedDevices = searchRegistry(vidPids);
            ports = SerialPort.GetPortNames().ToList()
                                            .FindAll(s => matchedDevices.Contains(s) && isArduinoBasedDevice(s))
                                            .ConvertAll<EavDeviceInfo>(port => new ArduinoDeviceInfo(port, AppConfig.ARDUINO_DEVICE_NAME));

            return ports;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectCB"></param>
        /// <param name="disconnectCB"></param>
        /// <param name="dataCB"></param>
        public void connect(DeviceConnectCallback connectCB, DeviceDisconnectCallback disconnectCB, DeviceDataCallback dataCB)
        {
            string portName = this.devInfo.PortName;

            ThreadPool.QueueUserWorkItem((context) =>
            {
                int res = -1;

                lock (this)
                {
                    if (this.serialPort.PortName != portName && this.serialPort.IsOpen)
                    {
                        this.serialPort.Close();
                        Thread.Sleep(150);
                    }

                    this.serialPort.PortName = portName;
                    this.serialPort.BaudRate = 9600;
                    this.serialPort.Parity = Parity.None;
                    this.serialPort.StopBits = StopBits.One;
                    this.serialPort.NewLine = "\r\n";

                    if (!this.serialPort.IsOpen)
                    {
                        try
                        {
                            this.serialPort.Open();
                            this.startDataMonitor(disconnectCB, dataCB, (SynchronizationContext)context);

                            res = 0;
                        }
                        catch (Exception)
                        {
                            res = -2;
                        }
                    }
                }

                if (connectCB != null)
                {
                    if (context != null)
                    {
                        ((SynchronizationContext)context).Post((r) => { connectCB((int)r); }, res);
                    }
                    else
                    {
                        connectCB(res);
                    }
                }
            }, SynchronizationContext.Current);
        }


        /// <summary>
        /// 
        /// </summary>
        public void disconnect()
        {
            ThreadPool.QueueUserWorkItem((context) =>
            {
                lock (this)
                {
                    if (this.serialPort.IsOpen)
                    {
                        this.serialPort.Close();
                        Thread.Sleep(150);
                    }
                }
            }, SynchronizationContext.Current);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void send(string data)
        {
            if (data == null || data.Length <= 0)
                return;

            byte[] buffer = Encoding.ASCII.GetBytes(data);

            lock (this)
            {
                if (this.serialPort.IsOpen)
                {
                    this.serialPort.Write(buffer, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected
        {
            get
            {
                lock (this)
                    return this.serialPort.IsOpen;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DEVICE_TYPE DeviceType 
        { 
            get
            {
                return this.devInfo.DeviceType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disconnectCB"></param>
        /// <param name="dataCB"></param>
        private void startDataMonitor(DeviceDisconnectCallback disconnectCB, DeviceDataCallback dataCB, SynchronizationContext context)
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                bool fContinue = true;
                string buffer = "";

                while (fContinue)
                {
                    lock (this)
                    {
                        if (!this.serialPort.IsOpen)
                        {
                            DeviceUtil.callDisconnectCallback(disconnectCB, context);
                            fContinue = false;
                        }
                        else
                        {
                            pollDeviceForData(ref buffer, dataCB, context);
                        }
                    }

                    Thread.Sleep(10);
                }
            }, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="dataCB"></param>
        /// <param name="context"></param>
        private void pollDeviceForData(ref string buffer, DeviceDataCallback dataCB, SynchronizationContext context)
        {
            int rxBytes = this.serialPort.BytesToRead;

            if (rxBytes > 0)
            {
                byte[] temp = new byte[rxBytes];

                try
                {
                    int numRead = this.serialPort.Read(temp, 0, rxBytes);
                    for (int i = 0; i < numRead; i++)
                    {
                        if (temp[i] == '\n')
                        {
                            DeviceUtil.callDataCallback(buffer, this.devInfo.DeviceType, dataCB, context);
                            buffer = "";
                        }
                        else if (temp[i] != '\r')
                        {
                            buffer += (char)temp[i];
                        }
                    }
                }
                catch (Exception)
                {
                    try { this.serialPort.Close(); } catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comPortName"></param>
        /// <returns></returns>
        private static bool isArduinoBasedDevice(string comPortName)
        {
            using (var port = new SerialPort(comPortName, 9600, Parity.None, 8))
            {
                try
                {
                    port.Open();
                    port.ReadTimeout = 150;
                    port.NewLine = "\r\n";
                }
                catch (Exception)
                {
                    return false;
                }

                try
                {
                    port.WriteLine("HVer?");
                    string line = "";
                    int retries = 3;

                    while (line.Length <= 0 && retries-- > 0)
                    {
                        line = port.ReadLine();
                        Thread.Sleep(10);
                    }

                    try { port.Close(); } catch (Exception) { }
                    return line.StartsWith("HVer:");
                }
                catch (Exception)
                {
                    try { port.Close(); } catch (Exception) { }
                    return false;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vidPids"></param>
        /// <returns></returns>
        private static List<string> searchRegistry(List<string[]> vidPids)
        {
            Regex _rx = new Regex(String.Format("^({0})",
                                      String.Join("|", vidPids.ConvertAll<string>(elem => String.Format("VID_{0}.PID_{1}", elem[0], elem.Length > 1 ? elem[1] : "")))),
                                      RegexOptions.IgnoreCase);

            List<string> comports = new List<string>();

            using (RegistryKey rk2 = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum"))
                foreach (String s3 in rk2.GetSubKeyNames())
                    using (RegistryKey rk3 = rk2.OpenSubKey(s3))
                        foreach (String s in rk3.GetSubKeyNames())
                        {
                            if (!_rx.Match(s).Success)
                                continue;

                            using (RegistryKey rk4 = rk3.OpenSubKey(s))
                                foreach (String s2 in rk4.GetSubKeyNames())
                                {
                                    using (RegistryKey rk5 = rk4.OpenSubKey(s2))
                                        try
                                        {
                                            using (RegistryKey rk6 = rk5.OpenSubKey("Device Parameters"))
                                                comports.Add((string)rk6.GetValue("PortName"));
                                        }
                                        catch (Exception)
                                        { }
                                }
                        }

            return comports;
        }

    }
}
