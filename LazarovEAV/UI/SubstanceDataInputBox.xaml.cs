using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for PatientDataInputBox.xaml
    /// </summary>
    public partial class SubstanceDataInputBox : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public SubstanceDataInputBox()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fieldsContainer_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.tbName != null)
            {
                DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(25) };

                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    this.tbName.CaretIndex = this.tbName.Text.Length;
                };

                timer.Start();
            }
        }
    }
}
