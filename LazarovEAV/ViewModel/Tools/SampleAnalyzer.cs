using LazarovEAV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class SampleAnalyzer
    {
        public double Average { get; private set; }

        public DataPoint StartPoint { get; private set; }
        public DataPoint EndPoint { get; private set; }

        public List<DataPoint> Data { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public SampleAnalyzer(List<DataPoint> data, int radix, int step)
        {
            this.Data = data;
            this.Average = 0.0;

            if (data != null)
            {
                doDataAnalysis(data);   
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void doDataAnalysis(List<DataPoint> data)
        {
            if (data.Count < 12)
                return;

            int wndLen = data.Count < 42 ? data.Count : 42;

            int start = 0;
            int end = -1;

            double[] startI = new double[wndLen - 2]; // calculate here first derivative of the 24 samples at the beginning of data
            double[] endI = new double[wndLen - 2]; // calculate here first derivative of the 24 samples at the end of data

            // calculate startI
            for (int i = 0; i < startI.Length; i++)
            {
                startI[i] = data[i + 2].Value - data[i].Value;
            }


            // calculate endI
            for (int i = 0, j = data.Count - 1; i < endI.Length; i++, j--)
            {
                endI[i] = data[j].Value - data[j - 2].Value;
            }


            // search startI
            int si = 0;

            for (; si < startI.Length; si++)
                if (startI[si] > 0.0) break;


            // search startI
            for (; si < startI.Length; si++)
            {
                if (startI[si] <= 0.0)                         // search for first negative value of the first derivative
                {
                    break;
                }
            }

            start = si;

            // search endI
            for (int i = 0; i < endI.Length; i++)
            {
                if (endI[i] >= -0.4)                         // search for first negative or zero value of the first derivative
                {
                    end = data.Count - i - 1;
                    break;
                }
            }


            double sum = 0.0;

            for (int i = start; i <= end; i++)
            {
                sum += data[i].Value;
            }

            if (end - start > 0)
            {
                this.Average = sum / (end - start);
                this.StartPoint = data[start];
                this.EndPoint = data[end];
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void doDataAnalysis2(List<DataPoint> data)
        {
            if (data.Count < 12)
                return;

            int wndLen = data.Count < 42 ? data.Count : 42;

            int start = 0;
            int end = -1;

            double[] startI = new double[wndLen - 2]; // calculate here first derivative of the 24 samples at the beginning of data
            double[] endI = new double[wndLen - 2]; // calculate here first derivative of the 24 samples at the end of data

            double maxStart = 0.0;
            int maxStartIdx = 0;


            // calculate startI
            for (int i = 0; i < startI.Length; i++)
            {
                double delta = data[i + 2].Value - data[i].Value;

                startI[i] = delta;

                if (maxStart < delta)
                {
                    maxStart = delta;
                    maxStartIdx = i;
                }
            }


            // calculate endI
            for (int i = 0, j = data.Count - 1; i < endI.Length; i++, j--)
            {
                endI[i] = data[j].Value - data[j - 2].Value;
            }


            // search startI
            for (int i = maxStartIdx; i < startI.Length; i++)
            {
                if (startI[i] <= 0.0)                         // search for first negative value of the first derivative
                {
                    start = i;
                    break;
                }
            }


            // search endI
            for (int i=0; i < endI.Length; i++)
            {
                if (endI[i] >= -0.4)                         // search for first negative or zero value of the first derivative
                {
                    end = data.Count - i - 1;
                    break;
                }
            }


            double sum = 0.0;

            for (int i = start; i <= end; i++)
            {
                sum += data[i].Value;
            }

            if (end - start > 0)
            {
                this.Average = sum / (end - start);
                this.StartPoint = data[start];
                this.EndPoint = data[end];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void doDataAnalysis_old(List<DataPoint> data)
        {
            int radix = 1;
            int step = 1;


            this.Data = data;
            this.Average = 0.0;

            if (data != null && data.Count >= 40)
            {
                double sum = 0.0;
                int start = 0;
                int end = -1;

                for (int i = radix; i < 25 - radix; i += step)
                {
                    double delta = Math.Abs(data[i + radix].Value - data[i - radix].Value);

                    if (delta <= 0.3)
                    {
                        start = i;
                        break;
                    }
                }

                for (int i = data.Count - radix - 1; i >= radix; i -= step)
                {
                    double delta = Math.Abs(data[i + radix].Value - data[i - radix].Value);

                    if (delta <= 0.3)
                    {
                        end = i;
                        break;
                    }
                }

                for (int i = start; i <= end; i++)
                {
                    sum += data[i].Value;
                }

                if (end - start > 0)
                {
                    this.Average = sum / (end - start);
                    this.StartPoint = data[start];
                    this.EndPoint = data[end];
                }
            }
        }
    }
}
