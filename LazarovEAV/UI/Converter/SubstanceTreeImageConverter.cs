﻿using LazarovEAV.Model;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;


namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SubstanceTreeImageConverter : IValueConverter
    {   
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
            int idx = 0;

            if (parameter != null)
            {
                Int32.TryParse((string)parameter, out idx);
            }

            int resIdx = 0;

            if (value is SubstanceTreeItemViewModel && ((SubstanceTreeItemViewModel)value).Substance != null)
                resIdx = 1;
            else
                resIdx = 0;

            return idx == resIdx ? Visibility.Visible : Visibility.Collapsed;
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
            throw new NotImplementedException();
        }
    }
}
