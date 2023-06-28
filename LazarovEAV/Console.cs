using LazarovEAV.Model;
using LazarovEAV.Util;
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
    class Console : NotifyPropertyChangedImpl
    {
        private ObservableCollection<string> output = new ObservableCollection<string>();
        public ObservableCollection<string> Output { get { return output; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void print(string message)
        {
            if (this.output.Count > 500)
                this.output.RemoveAt(0);

            string msg = DateTime.Now.ToString("[HH:mm:ss.fff] ");

            this.output.Add(msg + message);
        }
    }
}
