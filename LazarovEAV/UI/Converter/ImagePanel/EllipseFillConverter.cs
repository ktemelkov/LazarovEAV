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
    class EllipseFillConverter : IMultiValueConverter
    {
        SolidColorBrush selectedFill = new SolidColorBrush();
        SolidColorBrush normalFill = new SolidColorBrush();
        SolidColorBrush controlFill = new SolidColorBrush();
        SolidColorBrush altSideFill = new SolidColorBrush();


        /// <summary>
        /// 
        /// </summary>
        public EllipseFillConverter()
        {
            BindingOperations.SetBinding(this.selectedFill, SolidColorBrush.ColorProperty, new Binding("ImageSelectedPointColor") { Source = UiConfig.Instance });
            BindingOperations.SetBinding(this.normalFill, SolidColorBrush.ColorProperty, new Binding("ImageNormalPointColor") { Source = UiConfig.Instance });
            BindingOperations.SetBinding(this.controlFill, SolidColorBrush.ColorProperty, new Binding("ImageControlPointColor") { Source = UiConfig.Instance });
            BindingOperations.SetBinding(this.altSideFill, SolidColorBrush.ColorProperty, new Binding("ImageHiddenPointColor") { Source = UiConfig.Instance });
        }


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

            MeridianPointViewModel p = (MeridianPointViewModel)value[0];
            MeridianPointViewModel s = (MeridianPointViewModel)value[1];

            if (p == s)
                return this.selectedFill;

            ImagePanelUtil.PointLayer pl = ImagePanelUtil.CalcPointLayer(p.ImageIndex, s.ImageIndex);

            if (pl == ImagePanelUtil.PointLayer.BEHIND)
                return this.altSideFill;

            if (p.IsControlPoint)
                return this.controlFill;

            if (pl == ImagePanelUtil.PointLayer.FRONT)
                return this.normalFill;

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
