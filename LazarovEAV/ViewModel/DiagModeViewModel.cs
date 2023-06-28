using LazarovEAV.Model;
using LazarovEAV.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;


namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class DiagModeViewModel : ModeBaseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LeftResultsProperty =
                        DependencyProperty.Register("LeftResults", typeof(ObservableCollection<TestResultViewModel>), typeof(DiagModeViewModel),
                        new PropertyMetadata(null));

        internal ObservableCollection<TestResultViewModel> LeftResults { get { return (ObservableCollection<TestResultViewModel>)GetValue(LeftResultsProperty); } set { SetValue(LeftResultsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RightResultsProperty =
                        DependencyProperty.Register("RightResults", typeof(ObservableCollection<TestResultViewModel>), typeof(DiagModeViewModel),
                        new PropertyMetadata(null));

        internal ObservableCollection<TestResultViewModel> RightResults { get { return (ObservableCollection<TestResultViewModel>)GetValue(RightResultsProperty); } set { SetValue(RightResultsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty PointResultsProperty =
                        DependencyProperty.Register("PointResults", typeof(TestResultViewModel), typeof(DiagModeViewModel),
                        new PropertyMetadata(null));

        internal TestResultViewModel PointResults { get { return (TestResultViewModel)GetValue(PointResultsProperty); } set { SetValue(PointResultsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ActualPointResultsProperty =
                        DependencyProperty.Register("ActualPointResults", typeof(TestResultViewModel), typeof(DiagModeViewModel),
                        new PropertyMetadata(null));

        internal TestResultViewModel ActualPointResults { get { return (TestResultViewModel)GetValue(ActualPointResultsProperty); } set { SetValue(ActualPointResultsProperty, value); } }


        private ILookup<long, MeridianPointViewModel> pointIdLookup;
        private DispatcherTimer pointGraphUpdateTimer;


        /// <summary>
        /// 
        /// </summary>
        public DiagModeViewModel()
        {
            this.pointGraphUpdateTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Normal, onPointGraphTimer, Dispatcher.CurrentDispatcher);
            this.pointGraphUpdateTimer.IsEnabled = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void onSelectedMeridianChanged(MeridianViewModel oldValue, MeridianViewModel newValue)
        {
            base.onSelectedMeridianChanged(oldValue, newValue);

            this.pointIdLookup = (newValue != null && newValue.Points != null) ? newValue.Points.ToLookup(x => x.Id) : null;

            updateAll();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void onActivePatientChanged(PatientViewModel oldValue, PatientViewModel newValue)
        {
            base.onActivePatientChanged(oldValue, newValue);
            updateAll();
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void onSelectedSideChanged()
        {
            base.onSelectedSideChanged();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void EavDeviceViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.EavDeviceViewModel_PropertyChanged(sender, e);

            if (e.PropertyName == "TestResults")
            {
                if (this.ActivePatient != null && this.ActivePatient.CurrentSession != null
                    && this.ActivePatient.CurrentSession.IsSessionActive
                    && this.SelectedMeridian != null && this.SelectedMeridian.SelectedPoint != null)
                {
                    ObservableHashSet<TestResultViewModel> results = this.SelectedSide == PositionType.LEFT ? this.ActivePatient.CurrentSession.ResultsLeft
                                                                                                            : this.ActivePatient.CurrentSession.ResultsRight;

                    if (results != null)
                    {
                        long pointId = this.SelectedMeridian.SelectedPoint.Id;

                        IEnumerable<TestResultViewModel> existingRes = from r in results
                                                                       where r.MeridianPointId == pointId && r.Type == ResultType.BASIC_TEST
                                                                       select r;

                        TestResultViewModel newRes = null;

                        if (existingRes.Count() > 0)
                        {
                            if (this.EavDevice.TestResults == null)
                            {
                                results.Remove(existingRes.ElementAt(0));
                            }
                            else
                            {
                                newRes = existingRes.ElementAt(0);
                                newRes.ResultData = this.EavDevice.TestResults;
                            }
                        }
                        else if (this.EavDevice.TestResults != null)
                        {
                            TestResult resInfo = this.SelectedSide == PositionType.LEFT ? (TestResult)new TestResultLeft() : (TestResult)new TestResultRight();

                            resInfo.Type = ResultType.BASIC_TEST;
                            resInfo.MeridianPoint_Id = pointId;
                            resInfo.Session_Id = this.ActivePatient.CurrentSession.Id;

                            newRes = new TestResultViewModel(resInfo);
                            newRes.ResultData = this.EavDevice.TestResults; // set the data before adding the item to the collection

                            results.Add(newRes);
                        }


                        this.PointResults = newRes;
                        this.ActualPointResults = newRes;

                        if (this.pointGraphUpdateTimer.IsEnabled)
                            this.pointGraphUpdateTimer.Stop();
                    }
                }
            }
            else if (e.PropertyName == "LiveSample")
            {
                if (this.ActivePatient != null && this.ActivePatient.CurrentSession != null
                    && this.ActivePatient.CurrentSession.IsSessionActive)
                {
                    if (!this.pointGraphUpdateTimer.IsEnabled)
                        this.pointGraphUpdateTimer.Start();
                }
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Meridian_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPoint")
            {
                updatePointGraphViewModel();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Patient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.Patient_PropertyChanged(sender, e);

            if (e.PropertyName == "CurrentSession")
                updateAll();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TestResultsLeft_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.TestResultsLeft_CollectionChanged(sender, e);

            if (this.SelectedMeridian == null || this.SelectedMeridian.Points == null)
                return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (this.LeftResults == null)
                    this.LeftResults = new ObservableCollection<TestResultViewModel>();

                foreach (var it in e.NewItems)
                {
                    TestResultViewModel item = (TestResultViewModel)it;

                    if (item.Type == ResultType.BASIC_TEST && this.pointIdLookup.Contains(item.MeridianPointId))
                        this.LeftResults.Add(item.AttachPointIndex(this.SelectedMeridian.Points.FindIndex(x => x.Id == item.MeridianPointId)));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove
                        || e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (this.LeftResults != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (this.LeftResults.Contains((TestResultViewModel)item))
                            this.LeftResults.Remove((TestResultViewModel)item);
                    }
                }
            }


            //
            // Force OxyPlot refresh
            //
            ObservableCollection<TestResultViewModel> backup = this.LeftResults;

            this.LeftResults = null;
            this.LeftResults = backup;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TestResultsRight_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.TestResultsRight_CollectionChanged(sender, e);

            if (this.SelectedMeridian == null || this.SelectedMeridian.Points == null)
                return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (this.RightResults == null)
                    this.RightResults = new ObservableCollection<TestResultViewModel>();

                foreach (var it in e.NewItems)
                {
                    TestResultViewModel item = (TestResultViewModel)it;
                    
                    if (item.Type == ResultType.BASIC_TEST && this.pointIdLookup.Contains(item.MeridianPointId))
                        this.RightResults.Add(item.AttachPointIndex(this.SelectedMeridian.Points.FindIndex(x => x.Id == item.MeridianPointId)));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove
                        || e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (this.RightResults != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        if (this.RightResults.Contains((TestResultViewModel)item))
                            this.RightResults.Remove((TestResultViewModel)item);
                    }
                }
            }


            //
            // Force OxyPlot refresh
            //
            ObservableCollection<TestResultViewModel> backup = this.RightResults;

            this.RightResults = null;
            this.RightResults = backup;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TestResultViewModel_Left_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ResultData")
            {
                //
                // Force OxyPlot refresh
                //
                ObservableCollection<TestResultViewModel> backup = this.LeftResults;

                this.LeftResults = null;
                this.LeftResults = backup;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TestResultViewModel_Right_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ResultData")
            {
                //
                // Force OxyPlot refresh
                //
                ObservableCollection<TestResultViewModel> backup = this.RightResults;

                this.RightResults = null;
                this.RightResults = backup;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateAll()
        {
            updateLeftGraphViewModel();
            updateRightGraphViewModel();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateLeftGraphViewModel()
        {
            if (this.ActivePatient != null && this.ActivePatient.CurrentSession != null 
                && this.ActivePatient.CurrentSession.ResultsLeft != null
                && this.SelectedMeridian != null
                && this.SelectedMeridian.Points != null)
            {
                IEnumerable<TestResultViewModel> filteredRes = from t in this.ActivePatient.CurrentSession.ResultsLeft
                                                               where t.Type == ResultType.BASIC_TEST && this.pointIdLookup.Contains(t.MeridianPointId)
                                                               select t.AttachPointIndex(this.SelectedMeridian.Points.FindIndex(x => x.Id == t.MeridianPointId));

                this.LeftResults = new ObservableCollection<TestResultViewModel>(filteredRes);
            }
            else
            {
                this.LeftResults = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateRightGraphViewModel()
        {
            if (this.ActivePatient != null && this.ActivePatient.CurrentSession != null
                && this.ActivePatient.CurrentSession.ResultsRight != null
                && this.SelectedMeridian != null
                && this.SelectedMeridian.Points != null)
            {
                IEnumerable<TestResultViewModel> filteredRes = from t in this.ActivePatient.CurrentSession.ResultsRight
                                                               where t.Type == ResultType.BASIC_TEST && this.pointIdLookup.Contains(t.MeridianPointId)
                                                               select t.AttachPointIndex(this.SelectedMeridian.Points.FindIndex(x => x.Id == t.MeridianPointId));

                this.RightResults = new ObservableCollection<TestResultViewModel>(filteredRes);
            }
            else
            {
                this.RightResults = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void updatePointGraphViewModel()
        {
            TestResultViewModel newRes = null;

            if (this.ActivePatient != null && this.ActivePatient.CurrentSession != null
                && this.SelectedMeridian != null
                && this.SelectedMeridian.SelectedPoint != null)
            {
                ObservableCollection<TestResultViewModel> results = this.SelectedSide == PositionType.LEFT ? this.LeftResults
                                                                                                        : this.RightResults;

                if (results != null)
                {
                    long pointId = this.SelectedMeridian.SelectedPoint.Id;
                    newRes = results.FirstOrDefault(x => x.MeridianPointId == pointId);
                }
            }

            this.PointResults = newRes;
            this.ActualPointResults = newRes;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onPointGraphTimer(object sender, EventArgs e)
        {
            if (this.ActivePatient != null && this.ActivePatient.CurrentSession != null
                    && this.SelectedMeridian != null && this.SelectedMeridian.SelectedPoint != null)
            {
                long pointId = this.SelectedMeridian.SelectedPoint.Id;
                TestResult resInfo = this.SelectedSide == PositionType.LEFT ? (TestResult)new TestResultLeft() : (TestResult)new TestResultRight();

                resInfo.Type = ResultType.BASIC_TEST;
                resInfo.MeridianPoint_Id = pointId;
                resInfo.Session_Id = this.ActivePatient.CurrentSession.Id;

                TestResultViewModel newRes = new TestResultViewModel(resInfo);
                newRes.ResultData = this.EavDevice.LiveGraph.ToList();

                this.PointResults = newRes;
                this.ActualPointResults = null;
            }

            this.pointGraphUpdateTimer.Stop();
        }
    }
}
