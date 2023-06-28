using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LazarovEAV.UI
{
    public class VisibilityCollection : ObservableCollection<Array> {}

    /// <summary>
    /// 
    /// </summary>
    class EditorModeToGridVisibilityConverter : IValueConverter
    {
        private VisibilityCollection res_map = new VisibilityCollection {
                new Visibility[] { Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Hidden },
                new Visibility[] { Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden },
                new Visibility[] { Visibility.Visible, Visibility.Visible, Visibility.Hidden, Visibility.Hidden },
                new Visibility[] { Visibility.Visible, Visibility.Hidden, Visibility.Visible, Visibility.Hidden },
                new Visibility[] { Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Visible },
                new Visibility[] { Visibility.Visible, Visibility.Hidden, Visibility.Hidden, Visibility.Visible },
            };

        public VisibilityCollection Map { get { return this.res_map; } set { this.res_map = value;  } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EditorMode mode = (EditorMode)value;
            int uiNum = 0;

            Int32.TryParse((string)parameter, out uiNum);

            return this.res_map[(int)mode].Cast<Visibility>().ElementAt(uiNum);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
