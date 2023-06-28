using LazarovEAV.Config;
using LazarovEAV.Model;
using LazarovEAV.Util;
using LazarovEAV.ViewModel.Util;
using Newtonsoft.Json;
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
    class SubstanceTestViewModel : ModeBaseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TestTableProperty =
                        DependencyProperty.Register("TestTable", typeof(TestTableViewModel), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null, (o, arg) => { ((SubstanceTestViewModel)o).onTestTableChanged((TestTableViewModel)arg.OldValue, (TestTableViewModel)arg.NewValue); }));

        internal TestTableViewModel TestTable { get { return (TestTableViewModel)GetValue(TestTableProperty); } set { SetValue(TestTableProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TestTableListProperty =
                        DependencyProperty.Register("TestTableList", typeof(List<TestTableViewModel>), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null, (o, arg) => { ((SubstanceTestViewModel)o).TestTable = arg.NewValue != null ? ((List<TestTableViewModel>)arg.NewValue).FirstOrDefault() : null; }));

        internal List<TestTableViewModel> TestTableList { get { return (List<TestTableViewModel>)GetValue(TestTableListProperty); } set { SetValue(TestTableListProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedTableIndexProperty =
                        DependencyProperty.Register("SelectedTableIndex", typeof(int), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(-1, (o, e) => { }));

        internal int SelectedTableIndex { get { return (int)GetValue(SelectedTableIndexProperty); } set { SetValue(SelectedTableIndexProperty, value); } }



        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty PointResultsProperty =
                        DependencyProperty.Register("PointResults", typeof(TestResultViewModel), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null));

        internal TestResultViewModel PointResults { get { return (TestResultViewModel)GetValue(PointResultsProperty); } set { SetValue(PointResultsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ActualPointResultsProperty =
                        DependencyProperty.Register("ActualPointResults", typeof(TestResultViewModel), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null));

        internal TestResultViewModel ActualPointResults { get { return (TestResultViewModel)GetValue(ActualPointResultsProperty); } set { SetValue(ActualPointResultsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SlotsProperty =
                        DependencyProperty.Register("Slots", typeof(List<SlotInfoViewModel>), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null, (o, arg) => { ((SubstanceTestViewModel)o).onSlotsChanged((List<SlotInfoViewModel>)arg.NewValue); } ));

        public List<SlotInfoViewModel> Slots { get { return (List<SlotInfoViewModel>)GetValue(SlotsProperty); } set { SetValue(SlotsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SlotListProperty =
                        DependencyProperty.Register("SlotList", typeof(List<List<SlotInfoViewModel>>), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null, (o, arg) => {  }));

        public List<List<SlotInfoViewModel>> SlotList { get { return (List<List<SlotInfoViewModel>>)GetValue(SlotListProperty); } set { SetValue(SlotListProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedSlotProperty =
                        DependencyProperty.Register("SelectedSlot", typeof(SlotInfoViewModel), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null, (o, arg) => { ((SubstanceTestViewModel)o).onSelectedSlotChanged((SlotInfoViewModel)arg.OldValue, (SlotInfoViewModel)arg.NewValue); }));

        internal SlotInfoViewModel SelectedSlot { get { return (SlotInfoViewModel)GetValue(SelectedSlotProperty); } set { SetValue(SelectedSlotProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedSlotIndexProperty =
                        DependencyProperty.Register("SelectedSlotIndex", typeof(int), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(0, (o,e) => { ((SubstanceTestViewModel)o).updateLinearPosition(); }));

        internal int SelectedSlotIndex { get { return (int)GetValue(SelectedSlotIndexProperty); } set { SetValue(SelectedSlotIndexProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectSlotByIndexCommandProperty =
                        DependencyProperty.Register("SelectSlotByIndexCommand", typeof(ICommand), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null));

        internal ICommand SelectSlotByIndexCommand { get { return (ICommand)GetValue(SelectSlotByIndexCommandProperty); } set { SetValue(SelectSlotByIndexCommandProperty, value); } }
        

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectSlotPositionCommandProperty =
                        DependencyProperty.Register("SelectSlotPositionCommand", typeof(ICommand), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null));

        internal ICommand SelectSlotPositionCommand { get { return (ICommand)GetValue(SelectSlotPositionCommandProperty); } set { SetValue(SelectSlotPositionCommandProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectTableCommandProperty =
                        DependencyProperty.Register("SelectTableCommand", typeof(ICommand), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null));

        internal ICommand SelectTableCommand { get { return (ICommand)GetValue(SelectTableCommandProperty); } set { SetValue(SelectTableCommandProperty, value); } }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty UpdateMultiTestSlotsCommandProperty =
                        DependencyProperty.Register("UpdateMultiTestSlotsCommand", typeof(ICommand), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null));

        internal ICommand UpdateMultiTestSlotsCommand { get { return (ICommand)GetValue(UpdateMultiTestSlotsCommandProperty); } set { SetValue(UpdateMultiTestSlotsCommandProperty, value); } }

        

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RemoveSubstanceCommandProperty =
                        DependencyProperty.Register("RemoveSubstanceCommand", typeof(ICommand), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null));

        internal ICommand RemoveSubstanceCommand { get { return (ICommand)GetValue(RemoveSubstanceCommandProperty); } set { SetValue(RemoveSubstanceCommandProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty EditSubstanceCommandProperty =
                        DependencyProperty.Register("EditSubstanceCommand", typeof(ICommand), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(null));

        internal ICommand EditSubstanceCommand { get { return (ICommand)GetValue(EditSubstanceCommandProperty); } set { SetValue(EditSubstanceCommandProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedLinearPositionProperty =
                        DependencyProperty.Register("SelectedLinearPosition", typeof(int), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(-1));

        internal int SelectedLinearPosition { get { return (int)GetValue(SelectedLinearPositionProperty); } set { SetValue(SelectedLinearPositionProperty, value); } }




        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SlotResultsProperty =
                        DependencyProperty.Register("SlotResults", typeof(List<SlotResultViewModel>), typeof(SubstanceTestViewModel),
                        new PropertyMetadata(new List<SlotResultViewModel>()));

        internal List<SlotResultViewModel> SlotResults { get { return (List<SlotResultViewModel>)GetValue(SlotResultsProperty); } set { SetValue(SlotResultsProperty, value); } }        


        private DispatcherTimer pointGraphUpdateTimer = null;
        private DispatcherTimer slotsGraphUpdateTimer = null;


        /// <summary>
        /// 
        /// </summary>
        public SubstanceTestViewModel()
        {
            this.SelectSlotByIndexCommand = new CommandDelegateBase<int>(new Action<int>(selectSlotByIndex));
            this.SelectSlotPositionCommand = new CommandDelegateBase<int>(new Action<int>(selectSlotPosition));
            this.UpdateMultiTestSlotsCommand = new CommandDelegate(new Action<object>(updateMultiTestSlots));
            this.RemoveSubstanceCommand = new CommandDelegate(new Action<object>(removeSubstance));
            this.EditSubstanceCommand = new CommandDelegate(new Action<object>(editSubstance));
            
            this.SelectTableCommand = new CommandDelegate(new Action<object>(selectTable));
        }


        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            EventUtils.detachEvents(this.SelectedSlot, SlotInfoViewModel_PropertyChanged);
            
            bindToSessionData(null);
        }


        /// <summary>
        /// 
        /// </summary>
        private void onSlotsChanged(List<SlotInfoViewModel> newSlots)
        {
            int idx = (newSlots != null && this.SelectedSlotIndex < newSlots.Count) ? this.SelectedSlotIndex : -1;
            
            this.SelectedSlot = idx >= 0 ? newSlots[idx] : null;
        }


        /// <summary>
        /// 
        /// </summary>
        private void onTestTableChanged(TestTableViewModel oldTable, TestTableViewModel newTable)
        {
            if (oldTable != null && this.TestTableList != null)
            {
                var tableIndex = this.TestTableList.IndexOf(oldTable);

                if (this.SlotList != null)
                {
                    this.SlotList[tableIndex][5].SelectedPosition = 0;
                }
            }

            if (newTable != null && this.TestTableList != null)
            {
                this.SelectedTableIndex = this.TestTableList.IndexOf(newTable);
                this.Slots = this.SlotList != null ? this.SlotList[this.SelectedTableIndex] : null;
            }
            else
            {
                this.SelectedTableIndex = -1;
                this.Slots = null;
            }

            updateSlotsGraphViewModel();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void onActivePatientChanged(PatientViewModel oldValue, PatientViewModel newValue)
        {
            base.onActivePatientChanged(oldValue, newValue);

            bindToSessionData(newValue != null ? newValue.CurrentSession : null);

            updateSlotsGraphViewModel();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected override void onSelectedMeridianChanged(MeridianViewModel oldValue, MeridianViewModel newValue)
        {
            base.onSelectedMeridianChanged(oldValue, newValue);

            if (this.SelectedSlot != null && newValue != null)
            {
                this.SelectedSlot.SelectedMeridian = newValue;
                this.SelectedSlot.SelectedPoint = newValue != null ? newValue.SelectedPoint : null;
            }

            updateSelectedSlotResults();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void onSelectedSideChanged()
        {
            base.onSelectedSideChanged();

            if (this.SelectedSlot != null)
                this.SelectedSlot.SelectedSide = this.SelectedSide;

            updateSelectedSlotResults();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldSlot"></param>
        /// <param name="slotInfo"></param>
        private void onSelectedSlotChanged(SlotInfoViewModel oldSlot, SlotInfoViewModel slotInfo)
        {
            EventUtils.detachEvents(oldSlot, SlotInfoViewModel_PropertyChanged);

            if (slotInfo != null)
            {
                if (slotInfo.SelectedMeridian == null)
                {
                    slotInfo.SelectedMeridian = this.SelectedMeridian;
                    slotInfo.SelectedPoint = this.SelectedMeridian != null ? this.SelectedMeridian.SelectedPoint : null;
                    slotInfo.SelectedSide = this.SelectedSide;

                    updateSelectedSlotResults();
                }
                else
                {
                    slotInfo.SelectedMeridian.SelectedPoint = slotInfo.SelectedPoint;
                    this.SelectedMeridian = slotInfo.SelectedMeridian;
                    this.SelectedSide = slotInfo.SelectedSide;
                }
            }

            if (slotInfo != null && this.Slots != null)
            {
                this.SelectedSlotIndex = this.Slots.IndexOf(slotInfo);
            }
            else
            {
                this.SelectedSlotIndex = -1;
            }

            EventUtils.attachEvents(slotInfo, SlotInfoViewModel_PropertyChanged);

            updatePointGraphViewModel();
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
            {
                bindToSessionData(this.ActivePatient.CurrentSession);

                updateSlotsGraphViewModel();
                updatePointGraphViewModel();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        private void bindToSessionData(object session)
        {
            BindingOperations.ClearBinding(this, TestTableProperty);
            BindingOperations.ClearBinding(this, SlotListProperty);
            BindingOperations.ClearBinding(this, TestTableListProperty);

            if (session != null)
            {
                BindingOperations.SetBinding(this, TestTableListProperty, new Binding("TestTableList") { Source = session, Mode = BindingMode.OneWay });
                BindingOperations.SetBinding(this, SlotListProperty, new Binding("SlotList") { Source = session, Mode = BindingMode.OneWay });

                var pointLookup = this.Meridians.SelectMany(m => m.Points).ToLookup(x => x.Id);

                foreach (var s in this.SlotList.SelectMany(x => x))
                {
                    if (s.Model.MeridianPoint_Id.HasValue)
                    {                        
                        MeridianPointViewModel p = pointLookup[s.Model.MeridianPoint_Id.Value].FirstOrDefault();

                        if (p != null)
                            s.SelectedMeridian = p.Meridian;

                        s.SelectedPoint = p;
                    }

                    s.SelectedSide = (PositionType)s.Model.BodySide;
                }

                this.Slots = (this.SlotList != null && this.SelectedTableIndex >= 0 && this.SelectedTableIndex < this.SlotList.Count) 
                                ? this.SlotList[this.SelectedTableIndex] : null;
            }
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
                    && this.SelectedMeridian != null && this.SelectedMeridian.SelectedPoint != null
                    && this.SelectedSlot != null)
                {
                    ObservableHashSet<TestResultViewModel> results = this.SelectedSide == PositionType.LEFT ? this.ActivePatient.CurrentSession.ResultsLeft
                                                                                                            : this.ActivePatient.CurrentSession.ResultsRight;

                    if (results != null)
                    {
                        long pointId = this.SelectedMeridian.SelectedPoint.Id;
                        ResultType currentResultType = this.SelectedSlot.SelectedPosition == 0 ? ResultType.BASIC_TEST : ResultType.SUBSTANCE_TEST;
                        string testSetup = buildTestSetup();

                        IEnumerable<TestResultViewModel> existingRes = from r in results
                                                                       where (r.MeridianPointId == pointId && r.Type == currentResultType && ((r.Setup == null && testSetup == null) || r.Setup.CompareTo(testSetup) == 0))
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

                            resInfo.Type = currentResultType;
                            resInfo.MeridianPoint_Id = pointId;
                            resInfo.Session_Id = this.ActivePatient.CurrentSession.Id;
                            resInfo.Setup = testSetup;

                            newRes = new TestResultViewModel(resInfo);
                            newRes.ResultData = this.EavDevice.TestResults; // set the data before adding the item to the collection

                            if (currentResultType == ResultType.BASIC_TEST || testSetup != null)
                                results.Add(newRes);
                            else
                                addTemporarySlotResult(newRes, this.Slots.IndexOf(this.SelectedSlot), this.SelectedSlot.SelectedPosition); // temp result
                        }

                        updatePointGraphViewModel();
                    }
                }
            }
            else if (e.PropertyName == "LiveSample")
            {
                if (this.ActivePatient != null && this.ActivePatient.CurrentSession != null
                    && this.ActivePatient.CurrentSession.IsSessionActive)
                {
                    updatePointGraphViewModel(true);
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
            base.Meridian_PropertyChanged(sender, e);

            if (e.PropertyName == "SelectedPoint")
            {
                if (this.SelectedSlot != null)
                    this.SelectedSlot.SelectedPoint = this.SelectedMeridian.SelectedPoint;

                updateSelectedSlotResults();
                updatePointGraphViewModel();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TestResultsLeft_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.TestResultsLeft_CollectionChanged(sender, e);

            updateSlotsGraphViewModel();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TestResultsRight_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.TestResultsRight_CollectionChanged(sender, e);

            updateSlotsGraphViewModel();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TestResultViewModel_Left_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ResultData")
                forceSlotResultsUpdate();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TestResultViewModel_Right_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ResultData")
                forceSlotResultsUpdate();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlotInfoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPosition")
            {
                updateLinearPosition();
                updateSelectedSlotResults();
                updatePointGraphViewModel();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateLinearPosition()
        {
            this.SelectedLinearPosition = this.SelectedSlotIndex >= 0 && this.SelectedSlot.SelectedPosition >= 0
                ? this.SelectedSlotIndex * (int)AppConfig.SUBSTANCE_TEST_SLOT_POSITIONS + this.SelectedSlot.SelectedPosition
                : -1;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newRes"></param>
        private void addTemporarySlotResult(TestResultViewModel newRes, int slotNumber, int slotPosition)
        {
            SlotResultViewModel res = this.SlotResults.Find((x) => x.SlotNumber == slotNumber && x.SlotPosition == slotPosition);

            if (res != null)
            {
                res.TestResult = newRes;
            }
            else
            {
                this.SlotResults.Add(new SlotResultViewModel() { SlotPosition = slotPosition, SlotNumber = slotNumber, TestResult = newRes });
            }

            forceSlotResultsUpdate();
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateSlotsGraphViewModel()
        {
            if (this.slotsGraphUpdateTimer != null && this.slotsGraphUpdateTimer.IsEnabled)
            {
                this.slotsGraphUpdateTimer.Stop();
            }


            (this.slotsGraphUpdateTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(10), DispatcherPriority.Normal,
                (sender, e) =>
                {
                    if (this.Slots != null)
                    {
                        IEnumerable<TestResultViewModel> filteredResL = filterTestResults(PositionType.LEFT);
                        IEnumerable<TestResultViewModel> filteredResR = filterTestResults(PositionType.RIGHT);

                        for (int i = 0; i < this.Slots.Count; i++)
                        {
                            updateSlotResults(i, this.Slots[i].SelectedSide == PositionType.RIGHT ? filteredResR : filteredResL);
                        }

                        forceSlotResultsUpdate();
                    }

                    this.slotsGraphUpdateTimer.Stop();
                },
                Dispatcher.CurrentDispatcher)).Start();
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateSelectedSlotResults()
        {
            if (this.SelectedSlot != null && this.Slots != null)
            {
                updateSlotResults(this.Slots.IndexOf(this.SelectedSlot), filterTestResults(this.SelectedSlot.SelectedSide));
                
                forceSlotResultsUpdate();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotNumber"></param>
        /// <param name="filteredResults"></param>
        private void updateSlotResults(int slotNumber, IEnumerable<TestResultViewModel> filteredResults)
        {
            if (this.SlotResults != null && slotNumber >= 0)
            {
                this.SlotResults.RemoveAll(x => x.SlotNumber == slotNumber);

                if (filteredResults == null)
                    return;


                SlotInfoViewModel slot = this.Slots[slotNumber];
            
                if (slot.SelectedPoint != null)
                {
                    foreach (TestResultViewModel baseRes in filteredResults.Where(x => x.MeridianPointId == slot.SelectedPoint.Id))
                    {
                        if (baseRes != null)
                        {
                            if (baseRes.Type == ResultType.SUBSTANCE_TEST)
                            {
                                if (slot.PositionData != null)
                                {
                                    //
                                    // multi substance test slot
                                    //

                                    for (int i=0; i < slot.PositionData.Count; i ++)
                                    {
                                        string setup = buildMultiTestSetup(slot.PositionData[i]);

                                        if (setup == baseRes.Setup)
                                        {
                                            this.SlotResults.Add(new SlotResultViewModel() { SlotNumber = slotNumber, SlotPosition = i + 1, TestResult = baseRes });
                                        }
                                    }
                                }
                                else
                                {
                                    var testTable = this.TestTableList[this.SelectedTableIndex];

                                    foreach (TestTableInfoViewModel ti in testTable.Positions.Where(x => x.Substance != null && x.Substance.JSON == baseRes.Setup))
                                    {
                                        int slotPosition = testTable.Positions.IndexOf(ti) + 1;
                                        this.SlotResults.Add(new SlotResultViewModel() { SlotNumber = slotNumber, SlotPosition = slotPosition, TestResult = baseRes });
                                    }
                                }
                            }
                            else
                                this.SlotResults.Add(new SlotResultViewModel() { SlotNumber = slotNumber, SlotPosition = 0, TestResult = baseRes });

                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fUseLiveData"></param>
        private void updatePointGraphViewModel(bool fUseLiveData = false)
        {
            if (fUseLiveData)
            {
                if (this.pointGraphUpdateTimer == null)
                {
                    (this.pointGraphUpdateTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Normal,
                        (sender, e) =>
                        {
                            this.PointResults = new TestResultViewModel(null) { ResultData = this.EavDevice.LiveGraph.ToList() };
                            this.ActualPointResults = null;

                            this.pointGraphUpdateTimer.Stop();
                        },
                        Dispatcher.CurrentDispatcher)).Start();
                }
                else if (!this.pointGraphUpdateTimer.IsEnabled)
                {
                    this.pointGraphUpdateTimer.Start();
                }
            }
            else
            {
                if (this.pointGraphUpdateTimer != null && this.pointGraphUpdateTimer.IsEnabled)
                {
                    this.pointGraphUpdateTimer.Stop();
                }

                (this.pointGraphUpdateTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(50), DispatcherPriority.Normal,
                    (sender, e) =>
                    {
                        SlotResultViewModel slotRes;
                        
                        this.PointResults = (this.SelectedSlot != null && null != (slotRes = this.SlotResults.Find(x => x.SlotNumber == this.SelectedSlotIndex && x.SlotPosition == this.SelectedSlot.SelectedPosition)))
                                                ? slotRes.TestResult : null;
                        this.ActualPointResults = this.PointResults;

                        this.pointGraphUpdateTimer.Stop();
                        this.pointGraphUpdateTimer = null;
                    },
                    Dispatcher.CurrentDispatcher)).Start();
            }            
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        private void selectTable(object tbl)
        {
            int selectedPos = (this.SelectedSlot != null) ? this.SelectedSlot.SelectedPosition : -1;

            if (tbl is TestTableViewModel)
            {
                this.TestTable = (TestTableViewModel)tbl;
            }
            else if (tbl is int)
            {
                int idx = (int)tbl;

                if (this.TestTableList == null || idx < 0 || idx >= this.TestTableList.Count)
                    this.TestTable = null;
                else
                    this.TestTable = this.TestTableList[idx];
            }
            else
            {
                throw new InvalidOperationException("Invalid SelectTableCommand argument type.");
            }

            if (this.SelectedSlot != null)
                this.SelectedSlot.SelectedPosition = selectedPos;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        private void selectSlotByIndex(int idx)
        {
            if (this.Slots == null || idx < 0 || idx >= this.Slots.Count)
                this.SelectedSlot = null;
            else
                this.SelectedSlot = this.Slots[idx];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        private void selectSlotPosition(int pos)
        {
            if (this.SelectedSlot != null)
                this.SelectedSlot.SelectedPosition = pos;
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateMultiTestSlots(object param)
        {
            if (this.Slots != null)
            {
                IEnumerable<TestResultViewModel> filteredResL = filterTestResults(PositionType.LEFT);
                IEnumerable<TestResultViewModel> filteredResR = filterTestResults(PositionType.RIGHT);

                foreach (var slot in this.Slots.FindAll(x => x.PositionData != null))
                {
                    updateSlotResults(this.Slots.IndexOf(slot), slot.SelectedSide == PositionType.RIGHT ? filteredResR : filteredResL);
                }

                forceSlotResultsUpdate();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void removeSubstance(object obj)
        {
            int testTablePosition = -1;

            if (obj != null)
            {
                testTablePosition = (int)obj;       
            }
            else if (this.SelectedSlot != null 
                    && this.SelectedSlotIndex < AppConfig.SUBSTANCE_TEST_SLOTS)
            {
                testTablePosition = this.SelectedSlot.SelectedPosition - 1;
            }

            if (this.TestTable != null
                && testTablePosition >= 0
                && testTablePosition < this.TestTable.Positions.Count)
            {
                this.TestTable.Positions[testTablePosition].Substance = null;
            }

            updateSlotsGraphViewModel();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void editSubstance(object obj)
        {
            updateSlotsGraphViewModel();
            updatePointGraphViewModel();
        }


        /// <summary>
        /// 
        /// </summary>
        private void forceSlotResultsUpdate()
        {
            List<SlotResultViewModel> temp = this.SlotResults;
            this.SlotResults = null;
            this.SlotResults = temp;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        private IEnumerable<TestResultViewModel> filterTestResults(PositionType side)
        {
            if (this.ActivePatient == null || this.ActivePatient.CurrentSession == null)
                return null;


            HashSet<long> pointIdLookup = new HashSet<long>(from v in this.Slots where (v.SelectedPoint != null && v.SelectedSide == side) select v.SelectedPoint.Id);
            ObservableHashSet<TestResultViewModel> results = side == PositionType.RIGHT ? this.ActivePatient.CurrentSession.ResultsRight : this.ActivePatient.CurrentSession.ResultsLeft;

            IEnumerable<TestResultViewModel> filteredRes = from t in results
                                                            where (pointIdLookup.Contains(t.MeridianPointId) && (t.Type == ResultType.BASIC_TEST || t.Type == ResultType.SUBSTANCE_TEST))
                                                            select t;

            return filteredRes;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string buildTestSetup()
        {
            if (this.SelectedSlot.SelectedPosition <= 0)
                return null;

            if (this.SelectedSlot.PositionData != null)
            {
                return buildMultiTestSetup(this.SelectedSlot.PositionData[this.SelectedSlot.SelectedPosition - 1]);
            }

            EffectiveSubstanceInfoViewModel si = this.TestTable.Positions[this.SelectedSlot.SelectedPosition - 1].Substance;

            return si != null ? si.JSON : null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotPos"></param>
        /// <returns></returns>
        private string buildMultiTestSetup(SlotPositionViewModel slotPos)
        {
            List<EffectiveSubstanceInfo> setup = new List<EffectiveSubstanceInfo>();

            foreach (var s in slotPos.SelectedSubstances.OrderBy(x => x))
            {
                var testTable = this.TestTableList[this.SelectedTableIndex];

                if (testTable.Positions[s].Substance != null)
                    setup.Add(testTable.Positions[s].Substance.Model);
            }

            return setup.Count > 0 ? JsonConvert.SerializeObject(setup) : null;
        }
    }
}
