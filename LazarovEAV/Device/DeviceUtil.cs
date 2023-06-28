using System.Threading;

namespace LazarovEAV.Device
{
    class DataCallbackArgs
    {
        public object Buffer { get; set;  }
        public DEVICE_TYPE DeviceType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    class DeviceUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="dataCB"></param>
        /// <param name="context"></param>
        public static void callDataCallback(object buffer, DEVICE_TYPE devType, DeviceDataCallback dataCB, SynchronizationContext context)
        {
            if (dataCB != null)
            {
                if (context != null)
                {
                    DataCallbackArgs state = new DataCallbackArgs { Buffer = buffer, DeviceType = devType };
                    ((SynchronizationContext)context).Post((s) => { dataCB(((DataCallbackArgs)s).Buffer, ((DataCallbackArgs)s).DeviceType); }, state);
                }
                else
                {
                    dataCB(buffer, devType);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="disconnectCB"></param>
        /// <param name="context"></param>
        public static void callDisconnectCallback(DeviceDisconnectCallback disconnectCB, SynchronizationContext context)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="dataCB"></param>
        /// <param name="context"></param>
        public static void callLogCallback(string message, DeviceLogCallback logCB, SynchronizationContext context)
        {
            if (logCB != null)
            {
                if (context != null)
                {
                    ((SynchronizationContext)context).Post((b) => { logCB((string)b); }, message);
                }
                else
                {
                    logCB(message);
                }
            }
        }

    }
}
