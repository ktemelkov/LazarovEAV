using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;


namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class BodySideToOpacityConverter : IMultiValueConverter
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
            if (values.Length < 2)
                throw new InvalidOperationException("Ivalid number of converter arguments.");

            PositionType pt1 = (PositionType)values[0];
            PositionType pt2 = (PositionType)values[1];
            
            double inactiveOpacity = 0.5;

            if (values.Length > 2)
                inactiveOpacity = (double)values[2];

            return pt1 == pt2 ? 1.0 : inactiveOpacity;
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
