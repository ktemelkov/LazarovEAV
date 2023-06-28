using LazarovEAV.Config;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class SubstanceToolTipConverter : IValueConverter
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
            if (!(value is EffectiveSubstanceInfoViewModel))
                return null;

            EffectiveSubstanceInfoViewModel info = (EffectiveSubstanceInfoViewModel)value;
            return info.Name
                    + (info.Description != null && info.Description.Length > 0 ? "\r\n" + info.Description : "")
                    + (info.Quantity != null && info.Quantity.Length > 0 ? "\r\n" + info.Quantity : "");
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
