using FTD2XX_NET;
using LazarovEAV.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using static FTD2XX_NET.FTDI;

namespace LazarovEAV.Device
{
    enum BIOCHECK_STATE
    {
        IDLE = 0,
        SAMPLING = 1
    }


    /// <summary>
    /// 
    /// </summary>
    class BiocheckDeviceInfo : EavDeviceInfo
    {
        public BiocheckDeviceInfo(uint id, string descr, string serialNumber)
        {
            this.Description = descr;
            this.SerialNumber = serialNumber;
            this.ID = id;
        }

        public DEVICE_TYPE DeviceType => DEVICE_TYPE.BIOCHECK;

        public string Description { get; private set; }
        public string SerialNumber { get; private set; }
        public uint ID { get; private set; }
    }


    /// <summary>
    /// 
    /// </summary>
    class BiocheckDevice : IEavDevice
    {
        private readonly FTDI ftdiApi = new FTDI();
        private readonly BiocheckDeviceInfo devInfo;
        private readonly DeviceLogCallback loggerCallback;

        public bool IsConnected
        {
            get
            {
                lock (this)
                    return this.ftdiApi.IsOpen;
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
        public BiocheckDevice(EavDeviceInfo dev, DeviceLogCallback logCB)
        {
            if (dev.DeviceType != DEVICE_TYPE.BIOCHECK)
            {
                throw new Exception("Invalid EavDeviceInfo configuration object for Biocheck device");
            }

            this.devInfo = (BiocheckDeviceInfo)dev;
            this.loggerCallback = logCB;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<EavDeviceInfo> getDevices()
        {
            List<EavDeviceInfo> deviceList = new List<EavDeviceInfo>();
            FTDI ftdiApi = new FTDI();

            uint devcount = 0;
            FT_STATUS res = ftdiApi.GetNumberOfDevices(ref devcount);

            if (res == FT_STATUS.FT_OK && devcount > 0)
            {
                FT_DEVICE_INFO_NODE[] devices = new FT_DEVICE_INFO_NODE[devcount];
                res = ftdiApi.GetDeviceList(devices);

                if (res == FT_STATUS.FT_OK)
                {
                    for (int i = 0; i < devcount; i++)
                    {
                        if (devices[i].Description == AppConfig.BIOCHECK_DEVICE_NAME)
                        {
                            deviceList.Add(new BiocheckDeviceInfo(devices[i].ID, devices[i].Description, devices[i].SerialNumber));
                        }
                    }
                }
            }

            return deviceList;
        }

        public void connect(DeviceConnectCallback connectCB, DeviceDisconnectCallback disconnectCB, DeviceDataCallback dataCB)
        {
            ThreadPool.QueueUserWorkItem((context) =>
            {
                FT_STATUS res = FT_STATUS.FT_OTHER_ERROR;

                lock (this)
                {
                    if (!this.ftdiApi.IsOpen)
                    {
                        if (FT_STATUS.FT_OK == (res = this.ftdiApi.OpenBySerialNumber(this.devInfo.SerialNumber))
                            && FT_STATUS.FT_OK == (res = this.ftdiApi.SetBaudRate(19200))
                            && FT_STATUS.FT_OK == (res = this.ftdiApi.SetDataCharacteristics(FT_DATA_BITS.FT_BITS_8, FT_STOP_BITS.FT_STOP_BITS_1, FT_PARITY.FT_PARITY_NONE))
                            && FT_STATUS.FT_OK == (res = this.ftdiApi.SetFlowControl(FT_FLOW_CONTROL.FT_FLOW_NONE, 0, 0)))
                        {
                            startDataMonitor(disconnectCB, dataCB, (SynchronizationContext)context);
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
                        connectCB((int)res);
                    }
                }
            }, SynchronizationContext.Current);
        }

        public void disconnect()
        {
            ThreadPool.QueueUserWorkItem((context) =>
            {
                lock (this)
                {
                    if (this.ftdiApi.IsOpen)
                        this.ftdiApi.Close();
                }
            }, SynchronizationContext.Current);
        }

        public void send(string data)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="disconnectCB"></param>
        /// <param name="dataCB"></param>
        /// <param name="context"></param>
        private void startDataMonitor(DeviceDisconnectCallback disconnectCB, DeviceDataCallback dataCB, SynchronizationContext context)
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                long fContinue = 1;
                long lastValue = 0;

                uint written = 0;
                this.ftdiApi.Write(new byte[] { 0x02, 0x06, 0x40, 0x46, 0x03 }, 5, ref written);

                var dataThread = new Thread(() =>
                {
                    uint rxActual = 0;
                    byte[] rxBuffer = new byte[256];
                    byte[] packet = new byte[7];
                    int packetPos = 0;

                    while (pollDeviceForData(ref rxBuffer, 256, ref rxActual))
                    {                        
                        int pos = 0;
                        long value = -1;

                        while (pos < rxActual)
                        {
                            while (packetPos == 0 && rxBuffer[pos] != 0x02 && ++pos < rxActual);
                            while (packetPos < 7 && pos < rxActual) packet[packetPos++] = rxBuffer[pos++];

                            if (packetPos == 7)
                            {
                                packetPos = 0;

                                if (packet[6] != 0x03)
                                {
                                    pos -= 6;
                                    if (pos < 0) pos = 0;
                                }
                                else if (packet[3] != 0xFF && packet[4] != 0xFF)
                                {
                                    //
                                    // we have new measurement packet here
                                    //
                                    value = (packet[3] | packet[4] << 8) & 0x3FF;
                                }
                            }
                        }

                        if (value > -1)
                        {
                            Interlocked.Exchange(ref lastValue, value);
                        }

                        Thread.Sleep(rxActual == 0 ? 10 : 0);
                    }

                    Interlocked.Exchange(ref fContinue, 0);
                });

                dataThread.Priority = ThreadPriority.Highest;
                dataThread.Start();

                BIOCHECK_STATE biocheckState = BIOCHECK_STATE.IDLE;
                int lastPacketTicks = Environment.TickCount;

                while (Interlocked.Read(ref fContinue) != 0)
                {
                    int ticks = Environment.TickCount;

                    if (ticks - lastPacketTicks >= 24)
                    {
                        var err = ticks - lastPacketTicks - 24;
                        lastPacketTicks = ticks - err;

                        var lastRead = Interlocked.Read(ref lastValue);

                        if (biocheckState == BIOCHECK_STATE.IDLE && lastRead != 0)
                        {
                            //
                            // start sampling
                            //
                            biocheckState = BIOCHECK_STATE.SAMPLING;
                            DeviceUtil.callDataCallback(lastRead, this.devInfo.DeviceType, dataCB, context);
                        } 
                        else if (biocheckState == BIOCHECK_STATE.SAMPLING)
                        {
                            DeviceUtil.callDataCallback(lastRead, this.devInfo.DeviceType, dataCB, context);

//                            DeviceUtil.callLogCallback(sampleLine, this.loggerCallback, context);

                            if (lastRead == 0)
                                biocheckState = BIOCHECK_STATE.IDLE;
                        }
                    }

                    Thread.Sleep(10);
                }

                DeviceUtil.callDisconnectCallback(disconnectCB, context);
            }, null);
        }

        private bool pollDeviceForData(ref byte[] rxBuffer, uint size, ref uint rxBytesRead)
        {
            rxBytesRead = 0;
            uint rxBytesAvailable = 0;

            lock (this)
            {
                if (!this.ftdiApi.IsOpen)
                {
                    return false;
                }
                else
                {
                    if (FT_STATUS.FT_OK != this.ftdiApi.GetRxBytesAvailable(ref rxBytesAvailable))
                    {
                        this.ftdiApi.Close();
                        return false;
                    }
                    else if (rxBytesAvailable > 0)
                    {
                        if (FT_STATUS.FT_OK != this.ftdiApi.Read(rxBuffer, rxBytesAvailable < size ? rxBytesAvailable : size, ref rxBytesRead))
                        {
                            this.ftdiApi.Close();
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
