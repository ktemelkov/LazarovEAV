using LazarovEAV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class MeridianPointViewModel
    {
        private MeridianPoint point;
        private MeridianViewModel meridian;

        public MeridianPoint Model { get { return this.point; } private set { } }

        public long Id { get { return this.point.Id; } private set { } }
        public string Name { get { return this.point.Name; } private set { } }
        public string AltName { get { return this.point.AltName != null ? "(" + this.point.AltName + ")" : ""; } private set { } }
        public bool IsControlPoint { get { return this.point.IsControlPoint; } private set { } }
        public string DescriptionLeft { get { return this.point.DescriptionLeft; } private set { } }
        public string DescriptionRight { get { return this.point.DescriptionRight != null ? this.point.DescriptionRight : this.point.DescriptionLeft; } private set { } }
        public int ImageIndex { get { return this.point.ImageIndex; } private set { } }
        public int SubImageIndex { get { return this.point.SubImageIndex; } private set { } }
        public double X { get { return this.point.X; } private set { } }
        public double Y { get { return this.point.Y; } private set { } }
        public double AltX { get { return this.point.AltX; } private set { } }
        public double AltY { get { return this.point.AltY; } private set { } }

        public bool IsDirty { get; set; }

        public string ToolTip 
        { 
            get 
            { 
                return this.point.DescriptionRight != null 
                        ? "Отляво: " + this.point.DescriptionLeft + "\r\nОтдясно: " + this.point.DescriptionRight
                        : this.point.DescriptionLeft;
            }

            private set { }
        }

        public MeridianViewModel Meridian { get { return this.meridian; } private set { } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        public MeridianPointViewModel(MeridianPoint pt, MeridianViewModel m)
        {
            this.point = pt;
            this.meridian = m;
        }
    }
}
