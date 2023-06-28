using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PatientEditor.xaml
    /// </summary>
    public partial class PatientEditor : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public PatientEditor()
        {
            InitializeComponent();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(this.listView.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Editor_Unloaded(object sender, RoutedEventArgs e)
        {
            var disp = this.DataContext as IDisposable;

            if (disp != null)
                disp.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Patient_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Parent.SetValue(VisibilityProperty, Visibility.Collapsed);
        }
    }
}
