using LazarovEAV.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class SlotAxisConverter : IValueConverter
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
            double offs = -0.5001;

            if (parameter != null)
            {
                if (parameter is double)
                    offs += (double)parameter;
                else if (parameter is string)
                    double.TryParse((string)parameter, NumberStyles.Float, new CultureInfo("en-US"), out offs);
            }

            return (double)(int)value*(AppConfig.TEST_TABLE_POSITIONS + 1) + offs;
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
