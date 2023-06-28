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
    class TestTablePositionToIndexConverter : IMultiValueConverter
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
                return null;

            if (values[1] == null || !(values[1] is TestTableInfoViewModel))
                return null;

            Collection<TestTableInfoViewModel> coll = (Collection<TestTableInfoViewModel>)values[0];
            TestTableInfoViewModel item = (TestTableInfoViewModel)values[1];
            
            return (coll.IndexOf(item) + 1).ToString() + ".";
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
