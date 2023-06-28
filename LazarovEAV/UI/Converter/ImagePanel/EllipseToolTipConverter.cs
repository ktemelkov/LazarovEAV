using LazarovEAV.Config;
using LazarovEAV.UI.Util;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class EllipseToolTipConverter : IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.Length < 2)
                return null;

            MeridianPointViewModel point = (MeridianPointViewModel)value[0];
            PositionType side = (PositionType)value[1];

            return point.Name + (point.AltName != null && point.AltName.Length > 0 ? " " + point.AltName : "") + "\n"
                            + (side == PositionType.LEFT ? point.DescriptionLeft : point.DescriptionRight);
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
