using LazarovEAV.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LazarovEAV.UI.Util
{
    public class SexToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SexType? valueToConvert = value as SexType?;

            if (valueToConvert != null)
            {
                if (valueToConvert == SexType.FEMALE)
                    return "Женски";
                else
                    return "Мъжки";
            }

            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var valor = value as string;

                if (!string.IsNullOrEmpty(valor) && valor == "Женски")
                    return SexType.FEMALE;

                return SexType.MALE;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
