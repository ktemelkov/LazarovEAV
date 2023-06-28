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
    class EllipseSizeConverter : IValueConverter
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
            double initialSize = 12.0;

            if (parameter != null)
            {
                Double.TryParse((string)parameter, out initialSize);
            }

            if (value != null && value is TransformGroup)
            {
                TransformGroup t = (TransformGroup)value;

                foreach (var c in t.Children)
                {
                    if (c is ScaleTransform)
                    {
                        ScaleTransform s = (ScaleTransform)c;

                        return initialSize / s.ScaleX;
                    }
                }
            }

            return initialSize;
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
