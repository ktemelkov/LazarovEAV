using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class MeterGaugeMetrics
    {
        public const double ANGLE0 = 30;
        public const double ANGLE100 = 150;

        public double CX { get; private set; }
        public double CY { get; private set; }
        public Point Center { get; private set; }

        public double MajorTickSize { get; private set; }
        public double MinorTickSize { get; private set; }

        public double OuterRadius { get; private set; }
        public double InnerRadius { get; private set; }
        public double WindowRadius { get; private set; }

        public double MajorFontSize { get; private set; }
        public double MinorFontSize { get; private set; }

        public double TitleFontSize { get; private set; }
        public double TitleX { get; private set; }
        public double TitleY { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        public MeterGaugeMetrics(double cx, double cy)
        {
            if (cx < 10)
                cx = 10;

            if (cy < 10)
                cy = 10;

            if (cx / 316 >= cy / 184)
            {
                this.CX = 316 * cy / 184;
                this.CY = cy;
            }
            else
            {
                this.CX = cx;
                this.CY = 184 * cx / 316;
            }

            this.Center = new Point(this.CX / 2, this.CY);//this.CX * 3 / 4);

            this.MajorTickSize = this.CX * 10 / 316;
            this.MinorTickSize = this.CX * 6 / 316;

            this.OuterRadius = this.CX * 10 / 22;
            this.InnerRadius = this.CX * 10.5 / 24;
            this.WindowRadius = this.CX * 10.5 / 30;

            this.MajorFontSize = this.CX * 12 / 316;
            this.MinorFontSize = this.CX * 10 / 316;

            this.TitleFontSize = this.MajorFontSize*2;
            this.TitleX = this.CX / 2;
            this.TitleY = this.CY * 2 / 3;
        }
    }
}
