using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MeridianInfo
    {
        public long Id { get; protected set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long SortKey { get; set; }
        public bool Visible { get; set; }

        public virtual ICollection<MeridianPoint> Points { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        public MeridianInfo()
        {
            this.Points = new List<MeridianPoint>();
        }
    }
}
