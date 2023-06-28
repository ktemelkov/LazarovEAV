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
    [JsonObject]
    class SlotPositionViewModel : NotifyPropertyChangedImpl
    {
        [JsonProperty(PropertyName="Substances")]
        private ObservableCollection<int> selectedSubstances = new ObservableCollection<int>();
        
        [JsonIgnore]
        public ObservableCollection<int> SelectedSubstances { get { return this.selectedSubstances; } set { RaisePropertyChanged("SelectedSubstances", this.selectedSubstances, this.selectedSubstances = value); } }
    }
}
