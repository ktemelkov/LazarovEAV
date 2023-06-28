using LazarovEAV.Config;
using LazarovEAV.Model;
using LazarovEAV.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class TestTableViewModel : NotifyPropertyChangedImpl
    {
        private ObservableCollection<TestTableInfoViewModel> positions;
        public ObservableCollection<TestTableInfoViewModel> Positions { get { return this.positions; } set { this.positions = value; RaisePropertyChanged("Posistions"); } }

        private string name;
        public string Name { get { return this.name; } }


        /// <summary>
        /// 
        /// </summary>
        public TestTableViewModel(string name = null)
        {
            this.name = name;

            if (DesignerProperties.GetIsInDesignMode(new Mock.MockDependencyObject()))
            {
                this.positions = new ObservableCollection<TestTableInfoViewModel>()
                {
                    new TestTableInfoViewModel(new TestTableInfo() { Substance = null, Session_Id = 0 }){ Substance = new EffectiveSubstanceInfoViewModel(new EffectiveSubstanceInfo() { Name = "Лекарство А", Type = SubstanceType.HOMEOPATHIC, Description = "Описание на лекарство А", Quantity = "9CH" }) },
                    new TestTableInfoViewModel(new TestTableInfo() { Substance = null, Session_Id = 0 }){ Substance = new EffectiveSubstanceInfoViewModel(new EffectiveSubstanceInfo() { Name = "Лекарство Б", Type = SubstanceType.HOMEOPATHIC, Description = "Описание на лекарство Б", Quantity = "9CH" }) },
                    new TestTableInfoViewModel(new TestTableInfo() { Substance = null, Session_Id = 0 }){ Substance = new EffectiveSubstanceInfoViewModel(new EffectiveSubstanceInfo() { Name = "Лекарство В", Type = SubstanceType.HOMEOPATHIC, Description = "Описание на лекарство В", Quantity = "9CH" }) },
                    new TestTableInfoViewModel(new TestTableInfo() { Substance = null, Session_Id = 0 }){ Substance = new EffectiveSubstanceInfoViewModel(new EffectiveSubstanceInfo() { Name = "Лекарство Г", Type = SubstanceType.HOMEOPATHIC, Description = "Описание на лекарство Г", Quantity = "9CH" }) },
                    new TestTableInfoViewModel(new TestTableInfo() { Substance = null, Session_Id = 0 }){ Substance = new EffectiveSubstanceInfoViewModel(new EffectiveSubstanceInfo() { Name = "Лекарство Д", Type = SubstanceType.HOMEOPATHIC, Description = "Описание на лекарство Д", Quantity = "9CH" }) },
                    new TestTableInfoViewModel(new TestTableInfo() { Substance = null, Session_Id = 0 }){ Substance = new EffectiveSubstanceInfoViewModel(new EffectiveSubstanceInfo() { Name = "Лекарство Е", Type = SubstanceType.HOMEOPATHIC, Description = "Описание на лекарство Е", Quantity = "9CH" }) },
                    new TestTableInfoViewModel(new TestTableInfo() { Substance = null, Session_Id = 0 }){ Substance = new EffectiveSubstanceInfoViewModel(new EffectiveSubstanceInfo() { Name = "Лекарство Ж", Type = SubstanceType.HOMEOPATHIC, Description = "Описание на лекарство Ж", Quantity = "9CH" }) },
                    new TestTableInfoViewModel(new TestTableInfo() { Substance = null, Session_Id = 0 }){ Substance = new EffectiveSubstanceInfoViewModel(new EffectiveSubstanceInfo() { Name = "Лекарство З", Type = SubstanceType.HOMEOPATHIC, Description = "Описание на лекарство З", Quantity = "9CH" }) },
                };                
            }
        }
    }
}
