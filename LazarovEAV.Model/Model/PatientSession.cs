using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class PatientSession
    {
        public long Id { get; protected set; }
        public DateTime VisitDate { get; set; }
        public string Comment { get; set; }

        public long Patient_Id { get; set; }


        [ForeignKey("Session_Id")]
        public virtual List<TestResultLeft> ResultsLeft { get; protected set; }

        [ForeignKey("Session_Id")]
        public virtual List<TestResultRight> ResultsRight { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        public PatientSession()
        {
            this.ResultsLeft = new List<TestResultLeft>();
            this.ResultsRight = new List<TestResultRight>();
        }
    }
}
