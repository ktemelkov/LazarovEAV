using LazarovEAV.Config;
using LazarovEAV.Model;
using LazarovEAV.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class PatientSessionViewModel : NotifyPropertyChangedImpl
    {
        private PatientSession session;

        public PatientSession Model { get { return this.session; } private set { } }

        public long Id { get { return this.session.Id;  } }
        
        public DateTime VisitDate { 
            get { return this.session.VisitDate; } 
            set { 
                this.session.VisitDate = value;
                this.isSessionActive = this.session.VisitDate.Date.Equals(DateTime.Now.Date); 

                RaisePropertyChanged("VisitDate"); 
                RaisePropertyChanged("IsSessionActive"); 
            } 
        }

        public string Comment { get { return this.session.Comment; } set { this.session.Comment = value; RaisePropertyChanged("Comment"); } }


        private bool isSessionActive = false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSessionActive
        {
            get { return this.isSessionActive; }
        }


        public ObservableHashSet<TestResultViewModel> ResultsLeft { get; protected set; }
        public ObservableHashSet<TestResultViewModel> ResultsRight { get; protected set; }


        private List<TestTableViewModel> testTableList;
        public List<TestTableViewModel> TestTableList { get { return this.testTableList; } protected set { RaisePropertyChanged("TestTableList", this.testTableList, this.testTableList = value); } }

        private List<List<SlotInfoViewModel>> slotList;
        public List<List<SlotInfoViewModel>> SlotList { get { return this.slotList; } protected set { RaisePropertyChanged("SlotList", this.slotList, this.slotList = value); } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sess"></param>
        public PatientSessionViewModel(long patientId, DateTime visitDate)
        {
            this.testTableList = new List<TestTableViewModel>();

            for (int i = 0; i < AppConfig.SUBSTANCE_TEST_PAGES; i++)
                this.testTableList.Add(new TestTableViewModel(((char)(65 + i)).ToString()) { Positions = new ObservableCollection<TestTableInfoViewModel>() });


            // create test tables
            for (int j = 0; j < this.testTableList.Count; j++)
            {
                for (int i = 0; i < AppConfig.TEST_TABLE_POSITIONS; i++)
                    this.testTableList[j].Positions.Add(new TestTableInfoViewModel(new TestTableInfo() { TableNo = j, Position = i }));
            }

            using (var sm = new SessionManager())
            {
                this.session = sm.getByVisitDate(patientId, visitDate);

                if (this.session == null)
                {
                    this.session = new PatientSession() { Patient_Id = patientId, VisitDate = visitDate };

                    this.ResultsLeft = new ObservableHashSet<TestResultViewModel>();
                    this.ResultsRight = new ObservableHashSet<TestResultViewModel>();

                    // create slots
                    createSlots();
                }
                else
                {
                    this.ResultsLeft = new ObservableHashSet<TestResultViewModel>(from r in this.session.ResultsLeft select new TestResultViewModel(r));
                    this.ResultsRight = new ObservableHashSet<TestResultViewModel>(from r in this.session.ResultsRight select new TestResultViewModel(r));
                
                    // load slots
                    IEnumerable<SlotInfo> slotInfoList = sm.getItemsByFilter<SlotInfo>((x) => x.Session_Id == this.session.Id).OrderBy(o => o.Id);
                    loadSlots(slotInfoList);

                    // load test tables
                    List<TestTableInfo> ttInfoList = sm.getItemsByFilter<TestTableInfo>((x) => x.Session_Id == this.session.Id).OrderBy(o => o.Id).ToList();

                    for (int j=0; j < ttInfoList.Count; j++)
                    {
                        int tableNo = (int)ttInfoList[j].TableNo;
                        int posNo = (int)ttInfoList[j].Position;

                        if (tableNo >= 0 && tableNo < this.testTableList.Count
                                && posNo >= 0 && posNo < AppConfig.TEST_TABLE_POSITIONS)
                        {
                            this.testTableList[tableNo].Positions[posNo] = new TestTableInfoViewModel(ttInfoList[j]);
                        }
                    }
                }
            }

            this.isSessionActive = this.session.VisitDate.Date.Equals(DateTime.Now.Date); 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotInfoList"></param>
        private void loadSlots(IEnumerable<SlotInfo> slotInfoList)
        {
            this.slotList = new List<List<SlotInfoViewModel>>();

            var singleSubstSlots = (from s in slotInfoList where s.Setup == null select s).OrderBy(o => o.Id).ToList();
            var mixSlots = (from s in slotInfoList where s.Setup != null select s).OrderBy(o => o.Id).ToList();

            for (int j = 0; j < AppConfig.SUBSTANCE_TEST_PAGES; j++)
            {
                this.slotList.Add(new List<SlotInfoViewModel>());

                for (int i = 0; i < AppConfig.SUBSTANCE_TEST_SLOTS; i++)
                {
                    int idx = j * AppConfig.SUBSTANCE_TEST_SLOTS + i;

                    this.slotList[j].Add(new SlotInfoViewModel(idx < singleSubstSlots.Count ? singleSubstSlots[idx] : new SlotInfo()));
                }

                if (j < mixSlots.Count)
                {
                    SlotInfo mixSlot = mixSlots[j];

                    SlotInfoViewModel mixSlotVM = new SlotInfoViewModel(mixSlot);
                    mixSlotVM.PositionData = new List<SlotPositionViewModel>();

                    List<SlotPositionViewModel> posData = JsonConvert.DeserializeObject<List<SlotPositionViewModel>>(mixSlot.Setup);

                    for (int i = 0; i < AppConfig.MIX_TEST_POSITIONS; i++)
                        mixSlotVM.PositionData.Add(i < posData.Count ? posData[i] : new SlotPositionViewModel());

                    this.slotList[j].Add(mixSlotVM);
                }
                else
                {
                    SlotInfoViewModel newMixSlotVM = new SlotInfoViewModel(new SlotInfo());
                    newMixSlotVM.PositionData = new List<SlotPositionViewModel>();

                    for (int i = 0; i < AppConfig.MIX_TEST_POSITIONS; i++)
                        newMixSlotVM.PositionData.Add(new SlotPositionViewModel());

                    this.slotList[j].Add(newMixSlotVM);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void createSlots()
        {
            this.slotList = new List<List<SlotInfoViewModel>>();

            for (int j = 0; j < AppConfig.SUBSTANCE_TEST_PAGES; j++)
            {
                this.slotList.Add(new List<SlotInfoViewModel>());

                for (int i = 0; i < AppConfig.SUBSTANCE_TEST_SLOTS + 1; i++)
                    this.slotList[j].Add(new SlotInfoViewModel(new SlotInfo() { Session_Id = this.session.Id }));

                int mixTestSlotIndex = AppConfig.SUBSTANCE_TEST_SLOTS;

                this.slotList[j][mixTestSlotIndex].PositionData = new List<SlotPositionViewModel>();

                for (int i = 0; i < AppConfig.MIX_TEST_POSITIONS; i++)
                    this.slotList[j][mixTestSlotIndex].PositionData.Add(new SlotPositionViewModel());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            // prepare results
            this.Model.ResultsLeft.Clear();
            this.Model.ResultsRight.Clear();

            foreach (var r in this.ResultsLeft)
                this.Model.ResultsLeft.Add((TestResultLeft)r.Model);

            foreach (var r in this.ResultsRight)
                this.Model.ResultsRight.Add((TestResultRight)r.Model);


            // prepare slots
            var slots = this.slotList.SelectMany(x => x.Select(s => s.Model));

            foreach (var slist in this.slotList)
            {
                foreach(var s in slist)
                {
                    if (s.PositionData != null)
                        s.Model.Setup = JsonConvert.SerializeObject(s.PositionData);

                    if (s.SelectedPoint != null)
                    {
                        s.Model.MeridianPoint_Id = s.SelectedPoint.Id;
                        s.Model.BodySide = (BodySideType)s.SelectedSide;
                    }
                }
            }


            // prepare test table data
            var testTable = this.testTableList.SelectMany<TestTableViewModel, TestTableInfo>((x) => x.Positions.Select(p => p.Model));

            // save all
            using (var sm = new SessionManager())
            {
                sm.SaveSession(this.session, true);

                // update session id in case of newly created session
                foreach (var s in slots) s.Session_Id = this.session.Id;
                foreach (var t in testTable) t.Session_Id = this.session.Id;

                sm.SaveSlots(slots);
                sm.SaveTestTable(testTable, true);                
            }
         }
    }
}
