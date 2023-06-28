using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TestTableInfo
    {
        public long Id { get; protected set; }

        public long TableNo { get; set; }
        public long Position { get; set; }

        /// <summary>
        /// 
        /// JSON String with Substance data: Name, Type, Quantity
        /// This string will be used to set the TestResult:Setup field
        /// 
        /// </summary>
        public string Substance { get; set; } 
        public long? Session_Id { get; set; }
    }
}
