using LazarovEAV.Model;
using LazarovEAV.Util;
using LazarovEAV.ViewModel.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class ModeBaseViewModel : DependencyObject, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MeridiansProperty =
                        DependencyProperty.Register("Meridians", typeof(List<MeridianViewModel>), typeof(ModeBaseViewModel),
                        new PropertyMetadata(null, (o, arg) => { ((ModeBaseViewModel)o).onMeridiansChanged((List<MeridianViewModel>)arg.NewValue); }));

        internal List<MeridianViewModel> Meridians { get { return (List<MeridianViewModel>)GetValue(MeridiansProperty); } set { SetValue(MeridiansProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EavDeviceProperty =
                        DependencyProperty.Register("EavDevice", typeof(EavDeviceViewModel), typeof(ModeBaseViewModel),
                        new PropertyMetadata(null, (o, args) => { ((ModeBaseViewModel)o).onEavDeviceChanged((EavDeviceViewModel)args.OldValue, (EavDeviceViewModel)args.NewValue); }));

        internal EavDeviceViewModel EavDevice { get { return (EavDeviceViewModel)GetValue(EavDeviceProperty); } set { SetValue(EavDeviceProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ActivePatientProperty =
                        DependencyProperty.Register("ActivePatient", typeof(PatientViewModel), typeof(ModeBaseViewModel),
                        new PropertyMetadata(null, (o, args) => { ((ModeBaseViewModel)o).onActivePatientChanged((PatientViewModel)args.OldValue, (PatientViewModel)args.NewValue); }));

        internal PatientViewModel ActivePatient { get { return (PatientViewModel)GetValue(ActivePatientProperty); } set { SetValue(ActivePatientProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedSideProperty =
                        DependencyProperty.Register("SelectedSide", typeof(PositionType), typeof(ModeBaseViewModel),
                        new PropertyMetadata(PositionType.RIGHT, (o, args) => { ((ModeBaseViewModel)o).onSelectedSideChanged(); }));

        internal PositionType SelectedSide { get { return (PositionType)GetValue(SelectedSideProperty); } set { SetValue(SelectedSideProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedMeridianProperty =
                        DependencyProperty.Register("SelectedMeridian", typeof(MeridianViewModel), typeof(ModeBaseViewModel),
                        new PropertyMetadata(null, (o, args) => { ((ModeBaseViewModel)o).onSelectedMeridianChanged((MeridianViewModel)args.OldValue, (MeridianViewModel)args.NewValue); }));

        internal MeridianViewModel SelectedMeridian { get { return (MeridianViewModel)GetValue(SelectedMeridianProperty); } set { SetValue(SelectedMeridianProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty FitPointsProperty =
                        DependencyProperty.Register("FitPoints", typeof(bool), typeof(ModeBaseViewModel),
                        new PropertyMetadata(null));

        internal bool FitPoints { get { return (bool)GetValue(FitPointsProperty); } set { SetValue(FitPointsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StatusProperty =
                        DependencyProperty.Register("Status", typeof(Status), typeof(ModeBaseViewModel),
                        new PropertyMetadata(null));

        internal Status Status { get { return (Status)GetValue(StatusProperty); } set { SetValue(StatusProperty, value); } }


        private CommandDelegate toogleZoomCommand;
        public ICommand ToogleZoomCommand { get { return this.toogleZoomCommand; } }


        /// <summary>
        /// 
        /// </summary>
        public ModeBaseViewModel()
        {
            this.toogleZoomCommand = new CommandDelegate(new Action<object>(toggleZoom));

            if (DesignerProperties.GetIsInDesignMode(new Mock.MockDependencyObject()))
            {
                createMockData();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            EventUtils.detachEvents(this.ActivePatient, Patient_PropertyChanged);
            EventUtils.detachEvents(this.EavDevice, EavDeviceViewModel_PropertyChanged);
            EventUtils.detachEvents(this.SelectedMeridian, Meridian_PropertyChanged);

            if (this.ActivePatient != null)
                SessionUtils.detachSession(this.ActivePatient.CurrentSession, this.ResultsCollectionHandlers, this.ResultsPropertyHandlers);
        }


        /// <summary>
        /// 
        /// </summary>
        private NotifyCollectionChangedEventHandler[] ResultsCollectionHandlers 
        { 
            get 
            {
                return new NotifyCollectionChangedEventHandler[] { TestResultsLeft_CollectionChanged, TestResultsRight_CollectionChanged }; 
            } 
        }


        /// <summary>
        /// 
        /// </summary>
        private PropertyChangedEventHandler[] ResultsPropertyHandlers
        {
            get
            {
                return new PropertyChangedEventHandler[] { TestResultViewModel_Left_PropertyChanged, TestResultViewModel_Right_PropertyChanged };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void createMockData()
        {
            this.Meridians = new List<MeridianViewModel>() { 
                new MeridianViewModel(new MeridianInfo()
                {
                    Name = "T1",
                    Description = "Test Meridian with [b]long[/b] long long name blah blah blahblahblah blah blah blah blah  blahblahblah blah blah blah blah blah blahblahblahblah",
                    SortKey = 1,
                    Visible = true,
                }),

                new MeridianViewModel(new MeridianInfo()
                {
                    Name = "T2",
                    Description = "Test Meridian 2",
                    SortKey = 1,
                    Visible = true,
                }),
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void onMeridiansChanged(List<MeridianViewModel> list)
        {
            if (list != null && list.Count > 0)
                this.SelectedMeridian = list[0];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void toggleZoom(object obj)
        {
            this.FitPoints = !this.FitPoints;
        }


#region First Level Property Changed Handlers: ActivePatient, EavDevice, SelectedSide, SelectedMeridian

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void onActivePatientChanged(PatientViewModel oldValue, PatientViewModel newValue)
        {
            if (oldValue != null)
            {
                SessionUtils.detachSession(oldValue.CurrentSession, this.ResultsCollectionHandlers, this.ResultsPropertyHandlers);
                EventUtils.detachEvents(oldValue, Patient_PropertyChanged);
            }

            if (newValue != null)
            {
                EventUtils.attachEvents(newValue, Patient_PropertyChanged);
                SessionUtils.attachSession(newValue.CurrentSession, this.ResultsCollectionHandlers, this.ResultsPropertyHandlers);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eavDeviceViewModel1"></param>
        /// <param name="eavDeviceViewModel2"></param>
        protected virtual void onEavDeviceChanged(EavDeviceViewModel oldValue, EavDeviceViewModel newValue)
        {
            EventUtils.detachEvents(oldValue, EavDeviceViewModel_PropertyChanged);
            EventUtils.attachEvents(newValue, EavDeviceViewModel_PropertyChanged);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void onSelectedMeridianChanged(MeridianViewModel oldValue, MeridianViewModel newValue)
        {
            EventUtils.detachEvents(oldValue, Meridian_PropertyChanged);
            EventUtils.attachEvents(newValue, Meridian_PropertyChanged);
        }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void onSelectedSideChanged()
        {
        }

#endregion // First Level Property Changed Handlers


#region Second Level Property Changed Handlers: (LiveGraph, LiveSample, TestResults), (SelectedPoint), (CurrentSession)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void EavDeviceViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Properties:
            //--------------------
            // LiveSample
            // FilteredSample
            // LiveGraph
            // TestResults
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Meridian_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Properties:
            //--------------------
            // SelectedPoint
            // SelectedPointIndex
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Patient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Properties:
            //--------------------
            // CurrentSession

            if (e.PropertyName == "CurrentSession")
            {
                PropertyChangedEventArgs2 args = (PropertyChangedEventArgs2)e;

                SessionUtils.detachSession((PatientSessionViewModel)args.OldValue, this.ResultsCollectionHandlers, this.ResultsPropertyHandlers);
                SessionUtils.attachSession((PatientSessionViewModel)args.NewValue, this.ResultsCollectionHandlers, this.ResultsPropertyHandlers);
            }
        }

#endregion // Second Level Property Changed Handlers


#region Third Level Property Changed Handlers: TestResult Left/Right Collections

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TestResultsLeft_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            handleResultsCollectionChanged(sender, e, TestResultViewModel_Left_PropertyChanged);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TestResultsRight_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            handleResultsCollectionChanged(sender, e, TestResultViewModel_Right_PropertyChanged);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="handler"></param>
        private void handleResultsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, PropertyChangedEventHandler handler)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                SessionUtils.attachItems(e.NewItems, handler);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove 
                        || e.Action == NotifyCollectionChangedAction.Reset)
            {
                SessionUtils.detachItems(e.OldItems, handler);
            }
        }


#endregion //Third Level Property Changed Handlers


#region Fourth Level Property Changed Handlers: ResultData, ControlPoints, HasDeviation, ResultValue

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TestResultViewModel_Left_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Properties:
            // -------------------
            // ResultData
            // ControlPoints
            // HasDeviation
            // ResultValue
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TestResultViewModel_Right_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Properties:
            // -------------------
            // ResultData
            // ControlPoints
            // HasDeviation
            // ResultValue
        }

#endregion

    }
}
