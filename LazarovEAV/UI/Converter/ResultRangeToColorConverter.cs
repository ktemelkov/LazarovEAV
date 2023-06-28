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
    class ResultRangeToColorConverter : IMultiValueConverter
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

            int range = Math.Abs((int)((double)values[0] + 0.5) - (int)((double)values[1] + 0.5));

            Color c;

            if (range <= 5)
                c = (Color)values[2];
            else if (range <= 10)
                c = (Color)values[3];
            else if (range <= 20)
                c = (Color)values[4];
            else
                c = (Color)values[5];

            return new SolidColorBrush(c);
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
