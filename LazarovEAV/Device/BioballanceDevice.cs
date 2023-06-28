using FTD2XX_NET;
using LazarovEAV.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LazarovEAV.Device
{
    /// <summary>
    /// 
    /// </summary>
    class BioballanceDeviceInfo : EavDeviceInfo
    {
        public BioballanceDeviceInfo(uint id, string descr, string serialNumber)
        {
            this.Description = descr;
            this.SerialNumber = serialNumber;
            this.ID = id;
        }

        public DEVICE_TYPE DeviceType => DEVICE_TYPE.BIOBALLANCE;

        public string Description { get; private set; }
        public string SerialNumber { get; private set; }
        public uint ID { get; private set; }
    }


    class BioballanceDevice : IEavDevice
    {
        private FTDI ftdiApi = new FTDI();
        private readonly BioballanceDeviceInfo devInfo;


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
        public BioballanceDevice(EavDeviceInfo dev)
        {
            if (dev.DeviceType != DEVICE_TYPE.BIOBALLANCE)
            {
                throw new Exception("Invalid EavDeviceInfo configuration object for Arduino based device");
            }

            this.devInfo = (BioballanceDeviceInfo)dev;
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
            FTDI.FT_STATUS res = ftdiApi.GetNumberOfDevices(ref devcount);

            if (res == FTDI.FT_STATUS.FT_OK && devcount > 0)
            {
                FTDI.FT_DEVICE_INFO_NODE[] devices = new FTDI.FT_DEVICE_INFO_NODE[devcount];
                res = ftdiApi.GetDeviceList(devices);

                if (res == FTDI.FT_STATUS.FT_OK)
                {
                    for (int i = 0; i < devcount; i++)
                    {
                        if (devices[i].Description.StartsWith(AppConfig.EAV_DEVICE_NAME))
                        {
                            deviceList.Add(new BioballanceDeviceInfo(devices[i].ID, devices[i].Description, devices[i].SerialNumber));
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
                FTDI.FT_STATUS res = FTDI.FT_STATUS.FT_OK;

                lock (this)
                {
                    if (!this.ftdiApi.IsOpen)
                    {
                        res = this.ftdiApi.OpenBySerialNumber(this.devInfo.SerialNumber);

                        if (FTDI.FT_STATUS.FT_OK == res)
                            startDataMonitor(disconnectCB, dataCB, (SynchronizationContext)context);
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
            if (data == null || data.Length <= 0)
                return;

            byte[] buffer = Encoding.ASCII.GetBytes(data);

            lock (this)
            {
                if (this.ftdiApi.IsOpen)
                {
                    uint written = 0;
                    this.ftdiApi.Write(buffer, data.Length, ref written);
                }
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
                        if (!this.ftdiApi.IsOpen)
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
            uint rxBytes = 0;

            if (FTDI.FT_STATUS.FT_OK != this.ftdiApi.GetRxBytesAvailable(ref rxBytes))
            {
                this.ftdiApi.Close();
            }
            else if (rxBytes > 0)
            {
                byte[] temp = new byte[rxBytes];
                uint numRead = 0;

                if (FTDI.FT_STATUS.FT_OK == this.ftdiApi.Read(temp, rxBytes, ref numRead))
                {
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
            }
        }
    }
}
