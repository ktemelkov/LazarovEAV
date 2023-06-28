using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SubstanceInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubstanceType Type { get; set; }

        public long Folder_Id { get; set; }
    }
}


