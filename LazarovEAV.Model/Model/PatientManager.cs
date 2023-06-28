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
    public class PatientManager : EntityManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PatientInfo> loadPatients() 
        {
            return base.loadItems<PatientInfo>();
        }


        /// <summary>
        /// 
        /// </summary>
        public void deletePatient(PatientInfo patient)
        {
            foreach (var s in patient.Sessions)
            {
                base.deleteItemsByFilter<TestResultLeft>(x => x.Session_Id == s.Id);
                base.deleteItemsByFilter<TestResultRight>(x => x.Session_Id == s.Id);
                base.deleteItemsByFilter<TestTableInfo>(x => x.Session_Id == s.Id);
                base.deleteItemsByFilter<SlotInfo>(x => x.Session_Id == s.Id);
            }

            base.saveChanges();

            base.deleteItemsByFilter<PatientSession>(x => x.Patient_Id == patient.Id);

            base.saveChanges();

            base.deleteItem(patient);
            base.saveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        public void addPatient(PatientInfo patient)
        {
            base.addItem(patient);
            base.saveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PatientSession> loadLastSessions()
        {
            string query =  "SELECT tt.* " +
                            "FROM PatientSession tt " +
                            "INNER JOIN " +
                                "(SELECT Patient_Id, MAX(Id) AS MaxId " +
                                "FROM PatientSession " +
                                "GROUP BY Patient_Id) groupedtt " +
                            "ON tt.Patient_Id = groupedtt.Patient_Id " +
                            "AND tt.Id = groupedtt.MaxId";

            return base.queryItems<PatientSession>(query);
        }
    }
}
