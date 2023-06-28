using LazarovEAV.Model;
using LazarovEAV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class PatientInfoViewModel : NotifyPropertyChangedImpl
    {
        private PatientInfo patient;

        public string Name { get { return this.patient.Name; } set { this.patient.Name = value; RaisePropertyChanged("Name"); } }
        public DateTime Birthdate { get { return this.patient.Birthdate; } set { this.patient.Birthdate = value; RaisePropertyChanged("Birthdate"); RaisePropertyChanged("Age"); } }
        public SexType Sex { get { return this.patient.Sex; } set { this.patient.Sex = value; RaisePropertyChanged("Sex"); } }
        public string Comment { get { return this.patient.Comment; } set { this.patient.Comment = value; RaisePropertyChanged("Comment"); } }
        public int Age 
        { 
            get 
            { 
                DateTime today = DateTime.Today;
                int age = today.Year - this.patient.Birthdate.Year;

                if (this.patient.Birthdate > today.AddYears(-age)) 
                    age--;

                return age;
            } 
            
            set { } 
        }

        internal PatientInfo Model { get { return this.patient;  } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pi"></param>
        public PatientInfoViewModel(PatientInfo pi) 
        {
            this.patient = pi;
        }


        /// <summary>
        /// 
        /// </summary>
        public PatientInfoViewModel() 
        {
            this.patient = new PatientInfo();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pivm"></param>
        public void CopyTo(PatientInfoViewModel pivm)
        {
            pivm.Name = this.Name;
            pivm.Birthdate = this.Birthdate;
            pivm.Sex = this.Sex;
            pivm.Comment = this.Comment;
        }
    }
}
