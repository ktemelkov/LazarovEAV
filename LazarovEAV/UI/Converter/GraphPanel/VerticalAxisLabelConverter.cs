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
    class VerticalAxisLabelConverter : IValueConverter
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
            PositionType side = (PositionType)value;

            return (side == PositionType.LEFT) ? LeftAxisLabelFormatterFunc : RightAxisLabelFormatterFunc;
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


        /// <summary>
        /// 
        /// </summary>
        public Func<double, string> RightAxisLabelFormatterFunc
        {
            get
            {
                return (item) =>
                {
                    double d = (double)item;
                    string res = String.Format("{0,0:N0}", (double)item);

                    if (res.Length == 2)
                        res = " " + res;
                    else if (res.Length == 1)
                        res = "  " + res;

                    return res;
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Func<double, string> LeftAxisLabelFormatterFunc
        {
            get
            {
                return (item) =>
                {
                    double d = (double)item;
                    string res = String.Format("{0,0:N0}", (double)item);

                    if (res.Length == 2)
                        res = res + " ";
                    else if (res.Length == 1)
                        res = res + "  ";

                    return res;
                };
            }
        }
    }
}

