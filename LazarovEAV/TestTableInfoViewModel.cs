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
    class TestTableInfoViewModel : NotifyPropertyChangedImpl
    {
        private TestTableInfo model;
        public TestTableInfo Model { get { return this.model; } }

        private EffectiveSubstanceInfoViewModel substance;
        public EffectiveSubstanceInfoViewModel Substance 
        { 
            get { return this.substance; } 
        
            set 
            {
                this.model.Substance = value != null ? value.JSON : null;
                RaisePropertyChanged("Substance", this.substance, this.substance = value); 
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ttInfo"></param>
        public TestTableInfoViewModel(TestTableInfo ttInfo)
        {
            this.model = ttInfo;

            if (ttInfo.Substance != null)
                this.substance = new EffectiveSubstanceInfoViewModel(ttInfo.Substance);
        }
    }
}
