using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="v"></param>
        public DataPoint(double t, double v)
        {
            this.Time = t;
            this.Value = v;
        }


        /// <summary>
        /// 
        /// </summary>
        public DataPoint()
        {
        }


        public double Time { get; set; }
        public double Value { get; set; }
    }
}
