using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Windows;


namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MeridianPoint
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AltName { get; set; }
        public bool IsControlPoint { get; set; }
        public string DescriptionLeft { get; set; }
        public string DescriptionRight { get; set; }
        public int ImageIndex { get; set; }
        public int SubImageIndex { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double AltX { get; set; }
        public double AltY { get; set; }
        public long SortKey { get; set; }

        public virtual MeridianInfo Meridian { get; set; }
    }
}
