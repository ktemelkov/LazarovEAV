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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public static readonly DependencyProperty HasActivePatientProperty =
                        DependencyProperty.Register("HasActivePatient", typeof(bool), typeof(MainMenu),
                        new PropertyMetadata(false));

        internal bool HasActivePatient { get { return (bool)GetValue(HasActivePatientProperty); } set { SetValue(HasActivePatientProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public MainMenu()
        {
            InitializeComponent();
        }
    }
}
