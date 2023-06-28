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
    public class SubstanceQuantity
    {
        public long Id { get; set; }
        public long SubstabceType { get; set; }
        public QuantityType Type { get; set; }
        public string Unit { get; set; }
    }
}
