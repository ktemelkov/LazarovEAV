using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMainModel
    {
        public virtual IEnumerable<MeridianInfo> Meridians { get; private set; }
        public virtual ObservableCollection<PathogenInfo> Pathogens { get; private set; }
        public virtual ObservableCollection<CureInfo> Cures { get; private set; }
        public virtual ObservableCollection<PatientInfo> Patients { get; private set; }
    }
}
