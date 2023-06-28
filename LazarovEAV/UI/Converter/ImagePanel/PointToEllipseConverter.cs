using LazarovEAV.UI.Util;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class PointToEllipseConverter : IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length < 6)
                throw new InvalidOperationException("Ivalid number of converter arguments.");

            double x = (double)values[0];
            double y = (double)values[1];
            double altx = (double)values[2];
            double alty = (double)values[3];
            ImagePanelUtil.PointLayer pl = ImagePanelUtil.CalcPointLayer((int)values[4], (int)values[5]);

            return pl == ImagePanelUtil.PointLayer.FRONT ? new Thickness(x, y, -x, -y) : new Thickness(altx, alty, -altx, -alty);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
