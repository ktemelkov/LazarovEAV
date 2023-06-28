using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel.Util
{
    /// <summary>
    /// 
    /// </summary>
    static class EventUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="handler"></param>
        public static void attachEvents(INotifyPropertyChanged obj, PropertyChangedEventHandler handler)
        {
            if (obj != null)
                obj.PropertyChanged += handler;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="handler"></param>
        public static void detachEvents(INotifyPropertyChanged obj, PropertyChangedEventHandler handler)
        {
            if (obj != null)
                obj.PropertyChanged -= handler;
        }
    }
}
