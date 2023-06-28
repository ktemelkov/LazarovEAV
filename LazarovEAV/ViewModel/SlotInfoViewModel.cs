using LazarovEAV.Model;
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
    class SlotInfoViewModel : NotifyPropertyChangedImpl
    {
        private SlotInfo model;
        public SlotInfo Model { get { return this.model; } }

        private int selectedPosition = 0;
        public int SelectedPosition { get { return this.selectedPosition; } set { RaisePropertyChanged("SelectedPosition", this.selectedPosition, this.selectedPosition = value); } }

        private PositionType selectedSide = PositionType.RIGHT;
        public PositionType SelectedSide { 
            get { return this.selectedSide; } 
            set {
                this.Model.BodySide = (BodySideType)value;
                RaisePropertyChanged("SelectedSide", this.selectedSide, this.selectedSide = value); 
            } 
        }
        
        private MeridianViewModel selectedMeridian;
        public MeridianViewModel SelectedMeridian { get { return this.selectedMeridian; } set { RaisePropertyChanged("SelectedMeridian", this.selectedMeridian, this.selectedMeridian = value); } }

        private MeridianPointViewModel selectedPoint;
        public MeridianPointViewModel SelectedPoint { 
            get { return this.selectedPoint; } 
            set {
                if (value != null)
                    this.model.MeridianPoint_Id = value.Id;
                else
                    this.model.MeridianPoint_Id = null;

                RaisePropertyChanged("SelectedPoint", this.selectedPoint, this.selectedPoint = value); 
            } 
        }

        private List<SlotPositionViewModel> positionData = null;
        public List<SlotPositionViewModel> PositionData { get { return this.positionData; } set { RaisePropertyChanged("PositionData", this.positionData, this.positionData = value); } }


        /// <summary>
        /// 
        /// </summary>
        public SlotInfoViewModel(SlotInfo model)
        {
            this.model = model;
//            this.selectedSide = (PositionType)model.BodySide;
//            this.selectedPoint = model.MeridianPoint_Id;
        }
    }
}
