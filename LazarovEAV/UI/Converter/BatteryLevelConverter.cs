using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;


namespace LazarovEAV.UI.Util
{
    /// <summary>
    /// 
    /// </summary>
    class BatteryLevelConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int lvl = (int)value;

            ResourceDictionary dict = new ResourceDictionary();
            Uri uri = new Uri("/LazarovEAV;component/Resources/user_icon.xaml", UriKind.Relative);
            dict.Source = uri;

            if (parameter != null)
                return (lvl < 25) ? dict["battery_empty"] : null;

            if (lvl < 25)
                return null;
            else if (lvl < 50)
                return dict["battery_1"];
            else if (lvl < 75)
                return dict["battery_2"];

            return dict["battery_full"];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
