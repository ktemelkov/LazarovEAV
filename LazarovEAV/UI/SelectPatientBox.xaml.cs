using LazarovEAV.Util.Util;
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

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for SelectPatientBox.xaml
    /// </summary>
    public partial class SelectPatientBox : UserControl
    {
        public static readonly DependencyProperty ActivePageProperty =
                                                      DependencyProperty.Register("ActivePage", typeof(UiPageType), typeof(SelectPatientBox),
                                                      new PropertyMetadata(UiPageType.None));

        internal UiPageType ActivePage { get { return (UiPageType)GetValue(ActivePageProperty); } set { SetValue(ActivePageProperty, value); this.ActiveOverlay = UiOverlayType.None; } }


        public static readonly DependencyProperty ActiveOverlayProperty =
                                                      DependencyProperty.Register("ActiveOverlay", typeof(UiOverlayType), typeof(SelectPatientBox),
                                                      new PropertyMetadata(UiOverlayType.None));

        internal UiOverlayType ActiveOverlay { get { return (UiOverlayType)GetValue(ActiveOverlayProperty); } set { SetValue(ActiveOverlayProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public SelectPatientBox()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            ICommand cmd = (ICommand)DependencyObjectUtil.GetValueByName(this.DataContext, "SelectPatientCommand");

            if (cmd != null)
                cmd.Execute(null);

            this.ActivePage = UiPageType.Default;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonShowHistory_Click(object sender, RoutedEventArgs e)
        {
            this.historyCombo.SelectedIndex = -1;
            this.historyCombo.IsDropDownOpen = true;

            this.historyCombo.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void historyCombo_DropDownClosed(object sender, EventArgs e)
        {
            this.historyCombo.Visibility = Visibility.Hidden;

            if (this.historyCombo.SelectedIndex >= 0)
            {
                this.sessionTime.Value = (DateTime)this.historyCombo.SelectedItem;
                this.sessionTime.IsReadOnly = true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetNow_Click(object sender, RoutedEventArgs e)
        {
            this.sessionTime.Value = DateTime.Now;
            this.sessionTime.IsReadOnly = false;
        }
    }
}
