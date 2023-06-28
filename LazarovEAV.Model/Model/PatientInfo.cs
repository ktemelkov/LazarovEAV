using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class PatientInfo
    {
        public long Id { get; protected set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public SexType Sex { get; set; }
        public string Comment { get; set; }

        [ForeignKey("Patient_Id")]
        public virtual List<PatientSession> Sessions { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        public PatientInfo()
        {
            this.Birthdate = new DateTime(2000, 1, 1);
            this.Sessions = new List<PatientSession>();
        }
    }
}
