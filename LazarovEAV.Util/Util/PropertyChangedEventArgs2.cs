using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyChangedEventArgs2 : PropertyChangedEventArgs 
    {
        private object oldValue;
        private object newValue;

        public object OldValue { get {return this.oldValue; } }
        public object NewValue { get { return this.newValue; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public PropertyChangedEventArgs2(string propertyName, object oldValue = null, object newValue = null)
            : base(propertyName)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }
}
