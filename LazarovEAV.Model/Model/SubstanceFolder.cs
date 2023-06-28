using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SubstanceFolder
    {
        public long Id { get; set; }
        public String Name { get; set; }

        public long Parent_Id { get; set; }        
    }
}
