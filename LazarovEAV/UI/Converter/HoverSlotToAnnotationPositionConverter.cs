using LazarovEAV.Config;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class HoverSlotToAnnotationPositionConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value < 0)
                return new OxyPlot.DataPoint(-100, -100);

            return new OxyPlot.DataPoint((int)value*AppConfig.SUBSTANCE_TEST_SLOT_POSITIONS + double.Parse((string)parameter), 12.5);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
