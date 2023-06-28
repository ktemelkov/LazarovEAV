using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Device
{
    enum DEVICE_TYPE
    {
        BIOCHECK = 1,
        BIOBALLANCE = 2,
        ARDUINO = 3
    }


    interface EavDeviceInfo
    {
        DEVICE_TYPE DeviceType { get; }
        string Description { get; }
    }

    interface IEavDevice
    {
        void connect(DeviceConnectCallback connectCB, DeviceDisconnectCallback disconnectCB, DeviceDataCallback dataCB);

        void disconnect();

        void send(string data);

        bool IsConnected { get; }

        DEVICE_TYPE DeviceType { get; }
    }


    delegate void DeviceListCallback(List<EavDeviceInfo> devices);
    delegate void DeviceConnectCallback(int errorCode);
    delegate void DeviceDisconnectCallback();
    delegate void DeviceDataCallback(object data, DEVICE_TYPE devType);
    delegate void DeviceLogCallback(string message);
}
