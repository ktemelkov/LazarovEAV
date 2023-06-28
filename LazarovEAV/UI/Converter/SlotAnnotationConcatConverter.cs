using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class SlotAnnotationConcatConverter : IMultiValueConverter
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
            if (values.Length == 3)
            {
                string m = values[0] == DependencyProperty.UnsetValue ? null : (string)values[0];
                string p = values[1] == DependencyProperty.UnsetValue ? null : (string)values[1];
                PositionType s = values[2] == DependencyProperty.UnsetValue ? PositionType.RIGHT : (PositionType)values[2];

                return m != null ? (m + " \u00B7 " + p + (s == PositionType.RIGHT ? " [R]" : " [L]")) : null;
            }

            if (values.Length == 2)
            {
                string p = values[0] == DependencyProperty.UnsetValue ? null : (string)values[0];
                PositionType s = values[1] == DependencyProperty.UnsetValue ? PositionType.RIGHT : (PositionType)values[1];

                return p != null ? (p + (s == PositionType.RIGHT ? " [R]" : " [L]")) : null;
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
        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
