using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class NotifyPropertyChangedImpl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool Set<T>(ref T field, T value, string propertyName, object oldValue = null, object newValue = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) 
            { 
                return false; 
            }

            field = value;

            RaisePropertyChanged(propertyName, oldValue, newValue);
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void RaisePropertyChanged(string propertyName, object oldValue = null, object newValue = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null) 
                handler(this, new PropertyChangedEventArgs2(propertyName, oldValue, newValue));
        }
    }
}
