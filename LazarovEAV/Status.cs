using LazarovEAV.Model;
using LazarovEAV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    enum StatusIconType
    {
        OK = 0,
        ALERT = 1,
        ERROR = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    class Status : NotifyPropertyChangedImpl
    {
        private string message = "Status message text goes here ...";
        public string Message { get { return this.message; } set { Set(ref this.message, value, "Message"); } }

        private string batteryVoltage = "4.45V";
        public string BatteryVoltage { get { return this.batteryVoltage; } set { Set(ref this.batteryVoltage, value, "BatteryVoltage"); } }

        private int batteryLevel = 100;
        public int BatteryLevel { get { return this.batteryLevel; } set { Set(ref this.batteryLevel, value, "BatteryLevel"); } }

        private StatusIconType icon = StatusIconType.ERROR;
        public StatusIconType Icon { get { return this.icon; } set { Set(ref this.icon, value, "Icon"); } }
    }
}
