using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TestResult
    {
        public long Id { get; protected set; }

        public ResultType Type { get; set; }
        public string Data { get; set; }

        /// <summary>
        /// 
        /// JSON string with substance data including: Name, Type, Quantity
        /// This string must be exactly the same as in TestTableInfo for correct result filtering
        /// 
        /// </summary>
        public string Setup { get; set; } 

        public long? MeridianPoint_Id { get; set; }
        public long? Session_Id { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public class TestResultLeft : TestResult
    {
    }


    /// <summary>
    /// 
    /// </summary>
    public class TestResultRight : TestResult
    {
    }
}
