using LazarovEAV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class SlotResultViewModel : NotifyPropertyChangedImpl
    {
        private TestResultViewModel testResult;
        public TestResultViewModel TestResult { get { return this.testResult; } set { RaisePropertyChanged("TestResult", this.testResult, this.testResult = value); } }

        private int slotNumber;
        public int SlotNumber { get { return this.slotNumber; } set { RaisePropertyChanged("SlotNumber", this.slotNumber, this.slotNumber = value); } }

        private int slotPosition;
        public int SlotPosition { get { return this.slotPosition; } set { RaisePropertyChanged("SlotPosition", this.slotPosition, this.slotPosition = value); } }


        /// <summary>
        /// 
        /// </summary>
        public SlotResultViewModel()
        {

        }
    }
}
