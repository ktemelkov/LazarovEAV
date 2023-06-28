using FTD2XX_NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazarovEAV.Device
{
    /// <summary>
    /// 
    /// </summary>
    class EavDeviceManagerOld
    {
        private FTDI ftdiApi = new FTDI();
       

        /// <summary>
        /// 
        /// </summary>
        public EavDeviceManagerOld() 
        { 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public void listDevices(DeviceListCallback callback)
        {
            if (callback == null)
                return;

            ThreadPool.QueueUserWorkItem((context) =>
            {
                List<EavDeviceInfo> deviceList = new List<EavDeviceInfo>();

                uint devcount = 0;
                FTDI.FT_STATUS res = this.ftdiApi.GetNumberOfDevices(ref devcount);

                if (res == FTDI.FT_STATUS.FT_OK && devcount > 0)
                {
                    FTDI.FT_DEVICE_INFO_NODE[] devices = new FTDI.FT_DEVICE_INFO_NODE[devcount];

                    res = this.ftdiApi.GetDeviceList(devices);

                    if (res == FTDI.FT_STATUS.FT_OK)
                    {
                        for (int i = 0; i < devcount; i++)
                        {
                            deviceList.Add(new EavDeviceInfo(devices[i].ID, devices[i].Description, devices[i].SerialNumber));
                        }
                    }
                }

                if (context != null)
                {
                    ((SynchronizationContext)context).Post((s) => { callback((List<EavDeviceInfo>)s); }, deviceList);
                }
                else
                {
                    callback(deviceList);
                }
            }, SynchronizationContext.Current);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="cb"></param>
        public void connect(string serial, DeviceConnectCallback connectCB, DeviceDisconnectCallback disconnectCB, DeviceDataCallback dataCB)
        {
            ThreadPool.QueueUserWorkItem((context) =>
            {
                FTDI.FT_STATUS res = FTDI.FT_STATUS.FT_OK;
                    
                lock (this)
                {
                    if (!this.ftdiApi.IsOpen)
                    {
                        res = this.ftdiApi.OpenBySerialNumber(serial);

                        if (FTDI.FT_STATUS.FT_OK == res)
                            startDataMonitor(disconnectCB, dataCB, (SynchronizationContext)context);
                    }
                }

                if (connectCB != null)
                {
                    if (context != null)
                    {
                        ((SynchronizationContext)context).Post((r) => { connectCB(FTDI.FT_STATUS.FT_OK == (FTDI.FT_STATUS)r); }, res);
                    }
                    else
                    {
                        connectCB(FTDI.FT_STATUS.FT_OK == res);
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
                    if (this.ftdiApi.IsOpen)
                        this.ftdiApi.Close();
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
                            callDisconnectCallback(disconnectCB, context);
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
                            callDataCallback(buffer, dataCB, context);
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="dataCB"></param>
        /// <param name="context"></param>
        private void callDataCallback(string buffer, DeviceDataCallback dataCB, SynchronizationContext context)
        {
            if (dataCB != null)
            {
                if (context != null)
                {
                    ((SynchronizationContext)context).Post((b) => { dataCB((string)b); }, buffer);
                }
                else
                {
                    dataCB(buffer);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="disconnectCB"></param>
        /// <param name="context"></param>
        private void callDisconnectCallback(DeviceDisconnectCallback disconnectCB, SynchronizationContext context)
        {
            if (disconnectCB != null)
            {
                if (context != null)
                {
                    ((SynchronizationContext)context).Post((r) => { disconnectCB(); }, null);
                }
                else
                {
                    disconnectCB();
                }
            }
        }
    }
}
