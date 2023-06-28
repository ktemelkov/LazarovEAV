using LazarovEAV.Model;
using LazarovEAV.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class SelectPatientBoxViewModel : DependencyObject
    {
        /// <summary>
        /// 
        /// </summary>
        public class LastSessionInfo : NotifyPropertyChangedImpl
        {
            private long patient_Id;
            public long Patient_Id { get { return this.patient_Id; } set { RaisePropertyChanged("Patient_Id", this.patient_Id, this.patient_Id = value); } }
            
            private DateTime visitDate;
            public DateTime VisitDate { get { return this.visitDate; } set { RaisePropertyChanged("VisitDate", this.visitDate, this.visitDate = value); } }
            
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
 	             return (int)this.patient_Id;
            }
        }


        public static readonly DependencyProperty ActivePatientProperty =
                        DependencyProperty.Register("ActivePatient", typeof(PatientViewModel), typeof(SelectPatientBoxViewModel),
                        new PropertyMetadata(null, (o, e) => { ((SelectPatientBoxViewModel)o).onActivePatientChanged((PatientViewModel)e.NewValue); }));

        internal PatientViewModel ActivePatient { get { return (PatientViewModel)GetValue(ActivePatientProperty); } set { SetValue(ActivePatientProperty, value); } }


        public static readonly DependencyProperty PatientListProperty =
                        DependencyProperty.Register("PatientList", typeof(ObservableCollection<PatientInfoViewModel>), typeof(SelectPatientBoxViewModel),
                        new FrameworkPropertyMetadata(null));

        internal ObservableCollection<PatientInfoViewModel> PatientList { get { return (ObservableCollection<PatientInfoViewModel>)GetValue(PatientListProperty); } set { SetValue(PatientListProperty, value); } }


        public static readonly DependencyProperty SelectedPatientProperty =
                        DependencyProperty.Register("SelectedPatient", typeof(PatientInfoViewModel), typeof(SelectPatientBoxViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { ((SelectPatientBoxViewModel)o).onSelectedPatientChanged((PatientInfoViewModel)e.NewValue); }));

        internal PatientInfoViewModel SelectedPatient { get { return (PatientInfoViewModel)GetValue(SelectedPatientProperty); } set { SetValue(SelectedPatientProperty, value); } }


        public static readonly DependencyProperty LastSessionProperty =
                DependencyProperty.Register("LastSession", typeof(LastSessionInfo), typeof(SelectPatientBoxViewModel),
                new FrameworkPropertyMetadata(null));

        internal LastSessionInfo LastSession { get { return (LastSessionInfo)GetValue(LastSessionProperty); } set { SetValue(LastSessionProperty, value); } }


        public static readonly DependencyProperty PatientSessionsListProperty =
                DependencyProperty.Register("PatientSessionsList", typeof(List<DateTime>), typeof(SelectPatientBoxViewModel),
                new FrameworkPropertyMetadata(null));

        internal List<DateTime> PatientSessionsList { get { return (List<DateTime>)GetValue(PatientSessionsListProperty); } set { SetValue(PatientSessionsListProperty, value); } }
        

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EditorModeProperty =
                        DependencyProperty.Register("EditorMode", typeof(EditorMode), typeof(SelectPatientBoxViewModel),
                        new FrameworkPropertyMetadata(EditorMode.LIST, (o, e) => { }));

        public EditorMode EditorMode { get { return (EditorMode)GetValue(EditorModeProperty); } set { SetValue(EditorModeProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EditedPatientProperty =
                        DependencyProperty.Register("EditedPatient", typeof(PatientInfoViewModel), typeof(SelectPatientBoxViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { }));

        public PatientInfoViewModel EditedPatient { get { return (PatientInfoViewModel)GetValue(EditedPatientProperty); } set { SetValue(EditedPatientProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ErrorMessageProperty =
                        DependencyProperty.Register("ErrorMessage", typeof(string), typeof(SelectPatientBoxViewModel),
                        new FrameworkPropertyMetadata("", (o, e) => { }));

        public string ErrorMessage { get { return (string)GetValue(ErrorMessageProperty); } set { SetValue(ErrorMessageProperty, value); } }


        private CommandDelegate createPatientCommand;
        public ICommand CreatePatientCommand { get { return this.createPatientCommand; } }

        private CommandDelegate applyCommand;
        public ICommand ApplyCommand { get { return this.applyCommand; } }

        private CommandDelegate cancelCommand;
        public ICommand CancelCommand { get { return this.cancelCommand; } }

        private DispatcherTimer errorMessageTimer;


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectPatientCommandProperty =
                        DependencyProperty.Register("SelectPatientCommand", typeof(ICommand), typeof(SelectPatientBoxViewModel),
                        new PropertyMetadata(null));

        internal ICommand SelectPatientCommand { get { return (ICommand)GetValue(SelectPatientCommandProperty); } set { SetValue(SelectPatientCommandProperty, value); } }


        private HashSet<LastSessionInfo> sessionCache;


        /// <summary>
        /// 
        /// </summary>
        public SelectPatientBoxViewModel()
        {
            this.SelectPatientCommand = new CommandDelegate(new Action<object>(onApplySelectedPatient));
            this.createPatientCommand = new CommandDelegate(new Action<object>(createPatient));
            this.applyCommand = new CommandDelegate(new Action<object>(apply));
            this.cancelCommand = new CommandDelegate(new Action<object>(cancel));

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                using (PatientManager patientManager = new PatientManager())
                {
                    CultureInfo culture = new CultureInfo("bg-BG");

                    this.PatientList = new ObservableCollection<PatientInfoViewModel>((from p in patientManager.loadPatients() select new PatientInfoViewModel(p)).OrderBy(x => x.Model.Name, StringComparer.Create(culture, true)));

                    if (this.PatientList.Count <= 0)
                    {
                        PatientInfo anonPatient = new PatientInfo() { Name = "Анонимен пациент", Birthdate = new DateTime(2001, 01, 01), Sex = SexType.MALE, Comment = "" };
                        patientManager.addItem(anonPatient);
                        patientManager.saveChanges();

                        this.PatientList.Add(new PatientInfoViewModel(anonPatient));
                    }

                    this.sessionCache = new HashSet<LastSessionInfo>(from s in patientManager.loadLastSessions() where s.VisitDate.Date == DateTime.Now.Date
                                                                     select new LastSessionInfo() { Patient_Id = s.Patient_Id, VisitDate = s.VisitDate });

                    this.SelectedPatient = this.PatientList[0];
                }
            }
            else
            {
                // some test data for the designer
                this.PatientList = new ObservableCollection<PatientInfoViewModel>()
                {
                    new PatientInfoViewModel(new PatientInfo(){ Name="Patient 1", Birthdate = new DateTime(1990, 1, 1), Comment="Some comment goes here ...", Sex=SexType.MALE }),
                    new PatientInfoViewModel(new PatientInfo(){ Name="Patient 2", Birthdate = new DateTime(1977, 9, 6), Comment="Some very long comment goes here \r\n long long long \r\n very very long ...", Sex=SexType.FEMALE }),
                    new PatientInfoViewModel(new PatientInfo(){ Name="Patient 3", Birthdate = new DateTime(1950, 10, 7), Comment="Some more comment goes here ...", Sex=SexType.MALE })
                };

                this.SelectedPatient = this.PatientList[0];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientInfoViewModel"></param>
        private void onSelectedPatientChanged(PatientInfoViewModel patientInfoViewModel)
        {
            if (patientInfoViewModel == null || this.sessionCache == null)
            {
                this.LastSession = null;
                this.PatientSessionsList = null;
            }
            else
            {
                LastSessionInfo lastSession;
                List<DateTime> sessionList = null;

                if (patientInfoViewModel.Model.Id == 0 
                        ||  null == (lastSession = this.sessionCache.FirstOrDefault(x => x.Patient_Id == patientInfoViewModel.Model.Id)))
                {
                    lastSession = new LastSessionInfo() { Patient_Id = patientInfoViewModel.Model.Id, VisitDate = DateTime.Now };
                    this.sessionCache.Add(lastSession);
                }


                if (patientInfoViewModel.Model.Id > 1)
                {
                    using (SessionManager db = new SessionManager())
                    {
                        sessionList = (from s in db.listByPatientId(patientInfoViewModel.Model.Id) select s.VisitDate).ToList();
                    }
                }
               
                this.LastSession = lastSession;
                this.PatientSessionsList = sessionList;
            }
        }

    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void onApplySelectedPatient(object obj)
        {
            if (this.SelectedPatient != null && this.LastSession != null)
            {
                this.ActivePatient = new PatientViewModel(this.SelectedPatient.Model, this.LastSession.VisitDate);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void cancel(object obj)
        {
            backToMainMode();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void apply(object obj)
        {
            if (this.EditedPatient.Name == null || this.EditedPatient.Name.Length <= 0)
            {
                this.ErrorMessage = "Моля въведете име на пациент!";

                if (this.errorMessageTimer != null)
                {
                    if (this.errorMessageTimer.IsEnabled)
                        this.errorMessageTimer.Stop();

                    this.errorMessageTimer.Start();
                }
                else if (this.EditorMode != EditorMode.ERROR)
                {
                    EditorMode prevEditorMode = this.EditorMode;
                    this.EditorMode = EditorMode.ERROR;

                    this.errorMessageTimer = new DispatcherTimer(TimeSpan.FromSeconds(3), DispatcherPriority.Normal,
                        (s, a) =>
                        {
                            this.EditorMode = prevEditorMode;

                            this.errorMessageTimer.Stop();
                            this.errorMessageTimer = null;
                        }, Dispatcher.CurrentDispatcher) { IsEnabled = true };
                }
            }
            else
            {
                if (this.EditorMode == EditorMode.CREATE)
                {
                    this.PatientList.Add(this.EditedPatient);
                    this.SelectedPatient = this.EditedPatient;

                    using (PatientManager patientManager = new PatientManager())
                    {
                        patientManager.addPatient(this.SelectedPatient.Model);
                    }
                }

                backToMainMode();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void backToMainMode()
        {
            this.EditedPatient = null;
            this.ErrorMessage = "";
            this.EditorMode = EditorMode.LIST;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void createPatient(object obj)
        {
            this.EditorMode = EditorMode.CREATE;
            this.EditedPatient = new PatientInfoViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientViewModel"></param>
        private void onActivePatientChanged(PatientViewModel newValue)
        {
            if (newValue != null)
            {
                PatientInfoViewModel p = this.PatientList.FirstOrDefault(x => x.Model.Id == newValue.Id);

                if (p != null)
                {
                    this.SelectedPatient = p;
                }
            }
        }
    }
}
