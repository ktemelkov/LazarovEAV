using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionManager : EntityManager
    {

        /// <summary>
        /// 
        /// </summary>
        public SessionManager()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public IEnumerable<PatientSession> listByPatientId(long id)
        {
            if (this.CTX == null)
                throw new Exception("Database context not initialized!");

            var qres = CTX.Set<PatientSession>().SqlQuery("SELECT * FROM PatientSession WHERE Patient_Id=" + id.ToString()).AsNoTracking().AsEnumerable<PatientSession>();
            
            return qres;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        public PatientSession lastByPatientId(long id)
        {
            if (this.CTX == null)
                throw new Exception("Database context not initialized!");

            this.CTX.Configuration.LazyLoadingEnabled = false;
            this.CTX.Configuration.ProxyCreationEnabled = false;

            long? maxId = this.CTX.Set<PatientSession>().Where((x) => x.Patient_Id == id).Max<PatientSession, long?>(x => x.Id);

            if (!maxId.HasValue)
                return null;

            try
            {
                var res = this.CTX.Set<PatientSession>().Where(x => x.Id == maxId)
                                                        .Include("ResultsLeft")
                                                        .Include("ResultsRight").FirstOrDefault();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }                       
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PatientSession getBySessionId(long id)
        {
            if (this.CTX == null)
                throw new Exception("Database context not initialized!");

            try
            {
                var res = this.CTX.Set<PatientSession>().Where(x => x.Id == id)
                                                        .Include("ResultsLeft")
                                                        .Include("ResultsRight").FirstOrDefault();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="visitTime"></param>
        /// <returns></returns>
        public PatientSession getByVisitDate(long patientId, DateTime visitDate)
        {
            if (this.CTX == null)
                throw new Exception("Database context not initialized!");

            try
            {
                var res = this.CTX.Set<PatientSession>().Where(x => x.Patient_Id == patientId && x.VisitDate == visitDate)
                                                        .Include("ResultsLeft")
                                                        .Include("ResultsRight").FirstOrDefault();
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void SaveSession(PatientSession s, bool fSave = false)
        {
            if (this.CTX == null)
                throw new Exception("Database context not initialized!");

            if (s.Id == 0)
            {
                this.CTX.Entry(s).State = EntityState.Added;
            }
            else
            {
                foreach (var r in s.ResultsLeft)                
                {
                    if (r.Id == 0)
                        this.CTX.Entry(r).State = EntityState.Added;
                    else
                    {
                        this.CTX.Set<TestResultLeft>().Attach(r);
                        this.CTX.Entry(r).State = EntityState.Modified;
                    }
                }

                foreach (var r in s.ResultsRight)
                {
                    if (r.Id == 0)
                        this.CTX.Entry(r).State = EntityState.Added;
                    else
                    {
                        this.CTX.Set<TestResultRight>().Attach(r);
                        this.CTX.Entry(r).State = EntityState.Modified;
                    }
                }

                this.CTX.Set<PatientSession>().Attach(s);
                this.CTX.Entry(s).State = EntityState.Unchanged;
            }

            
            try
            {
                if (fSave)
                    this.CTX.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void CleanSession(PatientSession s, bool fSave = false) 
        {
            if (this.CTX == null)
                throw new Exception("Database context not initialized!");

            if (s.Id == 0)
                return;

            foreach (var r in s.ResultsLeft)
                this.CTX.Entry(r).State = EntityState.Deleted;

            foreach (var r in s.ResultsRight)
                this.CTX.Entry(r).State = EntityState.Deleted;

            s.ResultsLeft.Clear();
            s.ResultsRight.Clear();

            this.CTX.Entry(s).State = EntityState.Deleted;

            if (fSave)
                this.CTX.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="slots"></param>
        public void SaveSlots(IEnumerable<SlotInfo> slots, bool fSave = false)
        {
            if (this.CTX == null)
                throw new Exception("Database context not initialized!");

            foreach (var s in slots)
            {
                if (s.Id == 0)
                    this.CTX.Entry(s).State = EntityState.Added;
                else
                {
                    this.CTX.Set<SlotInfo>().Attach(s);
                    this.CTX.Entry(s).State = EntityState.Modified;
                }
            }

            if (fSave)
                this.CTX.SaveChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="testTable"></param>
        public void SaveTestTable(IEnumerable<TestTableInfo> testTable, bool fSave = false)
        {
            if (this.CTX == null)
                throw new Exception("Database context not initialized!");

            foreach (var t in testTable)
            {
                if (t.Id == 0)
                    this.CTX.Entry(t).State = EntityState.Added;
                else
                {
                    this.CTX.Set<TestTableInfo>().Attach(t);
                    this.CTX.Entry(t).State = EntityState.Modified;
                }
            }

            if (fSave)
                this.CTX.SaveChanges();
        }
    }
}
