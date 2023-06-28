using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;


namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class ResultDeviationToTextConverter : IValueConverter
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
            TestResultViewModel result = (TestResultViewModel)value;

            string ret = null;

            if (result != null && result.ControlPoints != null && result.ControlPoints.Count > 1)
            {
                int dev = (int)(result.ControlPoints[0].Value + 0.5) - (int)(result.ControlPoints[1].Value + 0.5);

                if (parameter != null)
                {
                    ret = String.Format("{0:0} / {1:0} / {2:0}", (int)(result.ControlPoints[0].Value + 0.5), (int)(result.ControlPoints[1].Value + 0.5), Math.Abs(dev));
                }
                else
                {
                    ret = String.Format("{0:0.0}", dev);
                }
            }

            return ret;
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



    /// <summary>
    /// 
    /// </summary>
    class ResultDeviationToColorConverter : IValueConverter
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
            TestResultViewModel result = (TestResultViewModel)value;

            if (result != null && result.ControlPoints != null && result.ControlPoints.Count > 1)
            {
                return parameter == null ? Colors.White : Color.FromArgb(0x20, 0x00, 0x00, 0x00);
            }


            return Colors.Transparent;
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
