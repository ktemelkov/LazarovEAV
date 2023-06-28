using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LazarovEAV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private Mutex myMutex;

        /// <summary>
        /// 
        /// </summary>
        public App()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.myMutex.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool aIsNewInstance = false;

            myMutex = new Mutex(true, "LazarovEAV.Application", out aIsNewInstance);  

            if (!aIsNewInstance)
            {
                App.Current.Shutdown();
                return;
            }
        }
    }
}
