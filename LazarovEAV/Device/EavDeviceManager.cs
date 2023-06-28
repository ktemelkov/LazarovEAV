using System;
using System.Collections.Generic;
using System.Threading;


namespace LazarovEAV.Device
{

    /// <summary>
    /// 
    /// </summary>
    class EavDeviceManager
    {
        private IEavDevice deviceInstance;
        private DeviceLogCallback loggerCallback;

        /// <summary>
        /// 
        /// </summary>
        public EavDeviceManager(DeviceLogCallback logCB)
        {
            this.loggerCallback = logCB;
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
                List<EavDeviceInfo> devices = BiocheckDevice.getDevices();

                if (devices.Count <= 0)
                {
                    devices = BioballanceDevice.getDevices();

                    if (devices.Count <= 0)
                    {
                        devices = ArduinoDevice.getDevices();
                    }
                }

                if (context != null)
                {
                    ((SynchronizationContext)context).Post((s) => { callback((List<EavDeviceInfo>)s); }, devices);
                }
                else
                {
                    callback(devices);
                }
            }, SynchronizationContext.Current);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="cb"></param>
        public void connect(EavDeviceInfo dev, DeviceConnectCallback connectCB, DeviceDisconnectCallback disconnectCB, DeviceDataCallback dataCB)
        {
            if (dev.DeviceType == DEVICE_TYPE.ARDUINO)
            {
                this.deviceInstance = new ArduinoDevice(dev);
            } 
            else if (dev.DeviceType == DEVICE_TYPE.BIOBALLANCE)
            {
                this.deviceInstance = new BioballanceDevice(dev);
            }
            else if (dev.DeviceType == DEVICE_TYPE.BIOCHECK)
            {
                this.deviceInstance = new BiocheckDevice(dev, this.loggerCallback);
            }
            else
            {
                this.deviceInstance = null;
            }

            if (this.deviceInstance != null)
            {
                this.deviceInstance.connect(connectCB, disconnectCB, dataCB);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void disconnect()
        {
            if (this.deviceInstance != null)
            {
                this.deviceInstance.disconnect();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void send(string data)
        {
            if (this.deviceInstance != null)
            {
                this.deviceInstance.send(data);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this.deviceInstance != null ? this.deviceInstance.IsConnected : false;
            }
        }
    }
}
