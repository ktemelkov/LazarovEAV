using LazarovEAV.Device;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    delegate void StartSequenceCallback();
    delegate void EndSequenceCallback();
    delegate void BaterryLevelCallback(int percent, double volts);
    delegate void HardwareVersionCallback(string ver);
    delegate void NewSampleCallback(double timeOffset, double sample);


    /// <summary>
    /// 
    /// </summary>
    class ProtocolAdapter
    {
        public event StartSequenceCallback StartSequence;
        public event EndSequenceCallback EndSequence;
        public event BaterryLevelCallback BatteryLevel;
        public event HardwareVersionCallback HardwareVersion;
        public event NewSampleCallback NewSample;

        private EavDeviceManager device;

        private int state = STATE_IDLE;

        private const int STATE_IDLE = 0;
        private const int STATE_SAMPLE = 1;
        private const int STATE_BATT = 2;

        private double startTime2 = 0;


        /// <summary>
        /// 
        /// </summary>
        public ProtocolAdapter(EavDeviceManager device)
        {
            this.device = device;
        }


        /// <summary>
        /// 
        /// </summary>
        public void requestVersion()
        {
            if (state == 0)
                this.device.send("HVer?\r\n");
        }


        /// <summary>
        /// 
        /// </summary>
        public void requestBatteryLevel()
        {
            if (this.state == STATE_IDLE)
            {
                this.state = STATE_BATT;
                this.device.send("Batt?\r\n");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void ResetState()
        {
            this.state = STATE_IDLE;
        }


        /// <summary>
        /// 
        /// </summary>
        public void feedData(object rawData, DEVICE_TYPE devType)
        {
            if (devType == DEVICE_TYPE.BIOCHECK)
            {
                parseBiocheckData((long)rawData);
            }
            else
            {
                parseBioballanceData((string)rawData);
            }
        }

        private void parseBiocheckData(long rawData)
        {
            float sample = 100 * rawData / 1018.0f;

            if (sample > 100.0f)
                sample = 100.0f;

            if (state == STATE_IDLE)
            {
                this.state = STATE_SAMPLE;
                this.startTime2 = 0.0;

                this.StartSequence?.Invoke();
                this.NewSample?.Invoke(1.0, 0.0);
            }

            if (this.state == STATE_SAMPLE)
            {
                double timeOffset = this.startTime2 += 24;
                this.NewSample?.Invoke(timeOffset, sample);

                if (rawData == 0)
                {
                    this.state = STATE_IDLE;
                }
            }
        }


        private void parseBioballanceData(string rawData)
        {
            var data = ((string)rawData).ToUpper();

            if (data.StartsWith("VAL:"))
            {
                if (this.state == STATE_SAMPLE)
                {
                    double sample = 0.0;
                    double timeOffset = this.startTime2 += 24;

                    if (Double.TryParse(data.Substring(4), NumberStyles.Float, new CultureInfo("en-US"), out sample) && this.NewSample != null)
                        this.NewSample(timeOffset, sample);
                }
            }
            else if (data == "BEG")
            {
                if (state == STATE_IDLE)
                {
                    this.state = STATE_SAMPLE;
                    this.startTime2 = 0.0;

                    this.StartSequence?.Invoke();
//                    this.NewSample?.Invoke(1.0, 0.0);
                }
            }
            else if (data == "END")
            {
                if (state == STATE_SAMPLE)
                {
//                    this.NewSample?.Invoke(this.startTime2 += 24, 0.0);
                    this.EndSequence?.Invoke();
                }
                    
                state = STATE_IDLE;
            }
            else if (data.StartsWith("HVER:"))
            {
                string ver = data.Substring(5);

                if (this.HardwareVersion != null)
                    this.HardwareVersion(ver);
            }
            else if (data.StartsWith("BAT:"))
            {
                Regex x = new Regex(@"(\d+)\%\s*\(\s*([\d\.]+)[Vv]\s*\)");
                Match m = x.Match(data.Substring(4), 0);

                if (m.Success && m.Groups.Count == 3 && this.BatteryLevel != null)
                {
                    int percents = 0;
                    double volts = 0.0;

                    Int32.TryParse(m.Groups[1].Value, out percents);
                    Double.TryParse(m.Groups[2].Value, out volts);

                    this.BatteryLevel(percents, volts);
                }
            }
        }
    }
}
