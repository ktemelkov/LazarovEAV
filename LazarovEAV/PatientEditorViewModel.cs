using LazarovEAV.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    class PatientEditorViewModel : DependencyObject, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ActivePatientProperty =
                        DependencyProperty.Register("ActivePatient", typeof(PatientViewModel), typeof(PatientEditorViewModel),
                        new PropertyMetadata(null));

        internal PatientViewModel ActivePatient { get { return (PatientViewModel)GetValue(ActivePatientProperty); } set { SetValue(ActivePatientProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EditorModeProperty =
                        DependencyProperty.Register("EditorMode", typeof(EditorMode), typeof(PatientEditorViewModel),
                        new FrameworkPropertyMetadata(EditorMode.LIST, (o, e) => { }));
        
        public EditorMode EditorMode { get { return (EditorMode)GetValue(EditorModeProperty); } set { SetValue(EditorModeProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty PatientListProperty =
                        DependencyProperty.Register("PatientList", typeof(ObservableCollection<PatientInfoViewModel>), typeof(PatientEditorViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { }));

        public ObservableCollection<PatientInfoViewModel> PatientList { get { return (ObservableCollection<PatientInfoViewModel>)GetValue(PatientListProperty); } set { SetValue(PatientListProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedPatientProperty =
                        DependencyProperty.Register("SelectedPatient", typeof(PatientInfoViewModel), typeof(PatientEditorViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { }));

        public PatientInfoViewModel SelectedPatient { get { return (PatientInfoViewModel)GetValue(SelectedPatientProperty); } set { SetValue(SelectedPatientProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EditedPatientProperty =
                        DependencyProperty.Register("EditedPatient", typeof(PatientInfoViewModel), typeof(PatientEditorViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { }));

        public PatientInfoViewModel EditedPatient { get { return (PatientInfoViewModel)GetValue(EditedPatientProperty); } set { SetValue(EditedPatientProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ErrorMessageProperty =
                        DependencyProperty.Register("ErrorMessage", typeof(string), typeof(PatientEditorViewModel),
                        new FrameworkPropertyMetadata("", (o, e) => { }));

        public string ErrorMessage { get { return (string)GetValue(ErrorMessageProperty); } set { SetValue(ErrorMessageProperty, value); } }


        private CommandDelegate editPatientCommand;
        public ICommand EditPatientCommand { get { return this.editPatientCommand; } }

        private CommandDelegate createPatientCommand;
        public ICommand CreatePatientCommand { get { return this.createPatientCommand; } }

        private CommandDelegate applyCommand;
        public ICommand ApplyCommand { get { return this.applyCommand; } }

        private CommandDelegate cancelCommand;
        public ICommand CancelCommand { get { return this.cancelCommand; } }

        private CommandDelegate deletePatientCommand;
        public ICommand DeletePatientCommand { get { return this.deletePatientCommand; } }

        private CommandDelegate confirmDeletePatientCommand;
        public ICommand ConfirmDeletePatientCommand { get { return this.confirmDeletePatientCommand; } }

        private PatientManager patientManager;

        private DispatcherTimer errorMessageTimer;

        /// <summary>
        /// 
        /// </summary>
        public PatientEditorViewModel()
        {
            this.editPatientCommand = new CommandDelegate(new Action<object>(editPatient));
            this.createPatientCommand = new CommandDelegate(new Action<object>(createPatient));
            this.applyCommand = new CommandDelegate(new Action<object>(apply));
            this.cancelCommand = new CommandDelegate(new Action<object>(cancel));
            this.deletePatientCommand = new CommandDelegate(new Action<object>(deletePatient));
            this.confirmDeletePatientCommand = new CommandDelegate(new Action<object>(confirmDeletePatient));
            

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.patientManager = new PatientManager();
                this.PatientList = new ObservableCollection<PatientInfoViewModel>(from p in this.patientManager.loadPatients() select new PatientInfoViewModel(p));
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

                this.SelectedPatient = this.PatientList[1];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.patientManager != null)
                this.patientManager.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void confirmDeletePatient(object obj)
        {
            if (this.ActivePatient != null && this.SelectedPatient != null && this.SelectedPatient.Model != null 
                    && this.ActivePatient.Id == this.SelectedPatient.Model.Id)
            {
                this.ActivePatient = null;
            }

            // TODO: delete sessions and session data
            this.patientManager.deletePatient(this.SelectedPatient.Model);
            this.PatientList.Remove(this.SelectedPatient);
            
            this.EditorMode = EditorMode.LIST;
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

                    this.patientManager.addPatient(this.SelectedPatient.Model);
                }
                else if (this.EditorMode == EditorMode.EDIT)
                {
                    this.EditedPatient.CopyTo(this.SelectedPatient);
                    this.patientManager.saveChanges();
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
        /// <param name="obj"></param>
        private void deletePatient(object obj)
        {
            this.EditedPatient = this.SelectedPatient;
            this.EditorMode = EditorMode.CONFIRM_DELETE;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void editPatient(object obj)
        {
            this.EditorMode = EditorMode.EDIT;
            this.EditedPatient = new PatientInfoViewModel();
            this.SelectedPatient.CopyTo(this.EditedPatient);
        }
    }
}
