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
    public class SlotInfo
    {
        public long Id { get; protected set; }

        //
        // JSON: list of slot positions with selected test table items for tests with more than one substances
        //
        // Example: TODO
        //
        public String Setup { get; set; }

        public BodySideType BodySide { get; set; }
        public long? MeridianPoint_Id { get; set; }
        public long? Session_Id { get; set; }
    }
}
