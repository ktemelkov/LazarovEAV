using LazarovEAV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LazarovEAV.ViewModel.Mock
{
    /// <summary>
    /// 
    /// </summary>
    class MainViewModel
    {
        private Status status = new Status();
        public Status Status { get { return this.status; } }

        private Console console = new Console();
        public Console Console { get { return this.console; } }

        private PatientViewModel selectedPatient = new PatientViewModel(new PatientInfo() { Name = "Mock Patient 1", Comment = "Comment lines for Mock Patient ...", Sex = SexType.MALE, Birthdate = new DateTime(1980, 5, 15) }, DateTime.Now);
        public PatientViewModel SelectedPatient { get { return this.selectedPatient; } }
    }
}
