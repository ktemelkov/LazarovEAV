using LazarovEAV.Model;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
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
    class SubstanceTypeEnumConverter : IValueConverter
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
            if (value == null || !(value is SubstanceType))
                return null;

            SubstanceType t = (SubstanceType)value;
            
            switch (t)
            {
                case SubstanceType.HOMEOPATHIC:
                    return "Хомеопатия";
                case SubstanceType.PARASITE:
                    return "Паразити";
                case SubstanceType.BACTERIA:
                    return "Бактерии";
                case SubstanceType.VIRUS:
                    return "Вируси";
                case SubstanceType.FUNGUS:
                    return "Гъбички";
                case SubstanceType.OTHER:
                    return "Други";
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
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
