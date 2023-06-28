using LazarovEAV.Model;
using LazarovEAV.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class TestResultViewModel : NotifyPropertyChangedImpl
    {
        private int calcStep = 1;
        private int calcRadix = 1;

        private TestResult result;
        private List<DataPoint> resultDataCache;

        public TestResult Model { get {
                if (this.result != null && this.resultDataCache != null)
                {
                    this.result.Data = saveData(this.resultDataCache);
                }

                return this.result; 
            }
        }

        public long MeridianPointId { get { return this.result != null ? this.result.MeridianPoint_Id.GetValueOrDefault() : 0; } }
        public ResultType Type { get { return this.result != null ? this.result.Type : ResultType.BASIC_TEST; } }

        public string Setup { get { return this.result != null ? this.result.Setup : null; } set { if (this.result != null) this.result.Setup = value; RaisePropertyChanged("Setup"); } }

        private double resultValue = 0.0;
        public double ResultValue { get { return this.resultValue; } set { this.resultValue = value; RaisePropertyChanged("ResultValue"); } }

        private bool hasDeviation = false;
        public bool HasDeviation { get { return this.hasDeviation; } set { this.hasDeviation = value; RaisePropertyChanged("HasDeviation"); } }

        private List<DataPoint> controlPoints;
        public List<DataPoint> ControlPoints { get { return this.controlPoints; } set { this.controlPoints = value; RaisePropertyChanged("ControlPoints"); } }


        private long meridianPointIndex = 0;
        public long MeridianPointIndex { get { return this.meridianPointIndex; } } // valid only if the ViewModel has set it by calling AttachPointIndex, while filtering the results for the View


        public List<DataPoint> ResultData 
        { 
            get 
            {
                return this.resultDataCache;                    
            } 
            
            set
            {
                this.resultDataCache = value;

                calcValues();

                RaisePropertyChanged("ResultData");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        public TestResultViewModel(TestResult res, int radix = 1, int step = 1)
        {            
            this.result = res;
            this.calcStep = step;
            this.calcRadix = radix;

            if (res != null && res.Data != null)
            {
                loadData();
                calcValues();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)this.MeridianPointId << 24 + (int)this.Type << 16 + (this.Setup != null ? this.Setup.GetHashCode() : 0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public TestResultViewModel AttachPointIndex(long index)
        {
            this.meridianPointIndex = index;
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        private void calcValues()
        {
            SampleAnalyzer sa = new SampleAnalyzer(this.resultDataCache, this.calcRadix, this.calcStep);
            this.ControlPoints = (sa.StartPoint != null && sa.EndPoint != null) ? new List<DataPoint>() { sa.StartPoint, sa.EndPoint } : null;

            if (this.ControlPoints != null && Math.Abs(this.ControlPoints[1].Value - this.ControlPoints[0].Value) > 5.0)
            {
                this.ResultValue = this.ControlPoints[0].Value;
                this.HasDeviation = true;
            }
            else
            {
                this.ResultValue = sa.Average;
                this.HasDeviation = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void loadData()
        {
            if (this.resultDataCache == null)
            {
                if (this.result.Data != null && this.result.Data.StartsWith("[{"))
                {
                    this.resultDataCache = JsonConvert.DeserializeObject<List<DataPoint>>(this.result.Data);
                }
                else 
                {
                    this.resultDataCache = JsonConvert.DeserializeObject<List<DataPoint>>(Decompress(this.result.Data));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private string saveData(List<DataPoint> value)
        {
            return Compress(JsonConvert.SerializeObject(value));
//            return JsonConvert.SerializeObject(value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Compress(string json)
        {
            byte[] data = Encoding.ASCII.GetBytes(json);

            using (MemoryStream output = new MemoryStream())
            {
                using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
                {
                    dstream.Write(data, 0, data.Length);
                }

                return System.Convert.ToBase64String(output.ToArray());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Decompress(string base64data)
        {
            byte[] data = System.Convert.FromBase64String(base64data);

            using (MemoryStream input = new MemoryStream(data))
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
                    {
                        dstream.CopyTo(output);
                    }
                 
                    return Encoding.ASCII.GetString(output.ToArray());
                }
            }
        }
    }
}

