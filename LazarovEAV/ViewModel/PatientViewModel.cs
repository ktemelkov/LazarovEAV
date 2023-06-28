using LazarovEAV.Model;
using LazarovEAV.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class PatientViewModel : NotifyPropertyChangedImpl
    {
        private PatientInfo patient;

        public long Id { get { return this.patient.Id; } }
        public string Name { get { return this.patient.Name; } set { this.patient.Name = value; RaisePropertyChanged("Name"); } }
        public DateTime Birthdate { get { return this.patient.Birthdate; } set { this.patient.Birthdate = value; RaisePropertyChanged("Birthdate"); } }
        public SexType Sex { get { return this.patient.Sex; } set { this.patient.Sex = value; RaisePropertyChanged("Sex"); } }
        public string Comment { get { return this.patient.Comment; } set { this.patient.Comment = value; RaisePropertyChanged("Comment"); } }

        private PatientSessionViewModel currentSession;
        public PatientSessionViewModel CurrentSession 
        { 
            get { return this.currentSession; }
            
            private set 
            { 
                object oldSession = this.currentSession;  
                this.currentSession = value; 
                
                SaveCurrentSession();
                RaisePropertyChanged("CurrentSession", oldSession, value); 
            } 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="patient"></param>
        public PatientViewModel(PatientInfo patient, DateTime visitDate)
        {
            this.patient = patient;

            initializeSession(visitDate);
        }


        /// <summary>
        /// 
        /// </summary>
        public void SaveCurrentSession()
        {
            if (this.currentSession == null || !this.currentSession.IsSessionActive || this.patient.Id == 1)
                return;

            if (this.currentSession.ResultsLeft.Count + this.currentSession.ResultsRight.Count <= 0)
                return;

            this.currentSession.Save();
        }


        /// <summary>
        /// 
        /// </summary>
        private void initializeSession(DateTime visitDate)
        {
            if (!DesignerProperties.GetIsInDesignMode(new Mock.MockDependencyObject()))
            {
                this.currentSession = new PatientSessionViewModel(this.patient.Id, visitDate);
            }
            else
            {
                var session = new PatientSession() {  };
                this.currentSession = new PatientSessionViewModel(this.patient.Id, DateTime.Now);
            }
        }
    }
}
