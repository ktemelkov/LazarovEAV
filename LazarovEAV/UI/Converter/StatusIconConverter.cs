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
    class StatusIconConverter : IValueConverter
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
            StatusIconType t = (StatusIconType)value;

            ResourceDictionary dict = new ResourceDictionary();
            Uri uri = new Uri("/LazarovEAV;component/Resources/user_icon.xaml", UriKind.Relative);
            dict.Source = uri;

            switch (t)
            {
                case StatusIconType.OK:
                    return null;
                case StatusIconType.ALERT:
                    return dict["appbar_alert"];
                case StatusIconType.ERROR:
                    return dict["appbar_error"];
            }

            return null;
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
