using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    class ImageTransformConverter : IMultiValueConverter
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
            if (value.Length < 7 || value[0] == null)
                return null;

            MeridianViewModel sm = (MeridianViewModel)value[0];
            MeridianPointViewModel s = (MeridianPointViewModel)value[1];
            bool fZoom = (bool)value[2];
            double actualWidth = (double)value[3];
            double actualHeight = (double)value[4];
            PositionType bs = (PositionType)value[5];
            PositionType ss = (PositionType)value[6];

            if (!fZoom || s == null || bs != ss)
                return null;

            int altImageIndex = 0xFFFF;

            if (s.ImageIndex == 0)
                altImageIndex = 1;
            else if (s.ImageIndex == 1)
                altImageIndex = 0;


            double left = 100000;
            double right = 0.0;
            double top = 100000;
            double bottom = 0.0;

            foreach (var p in sm.Points)
            {
                Point org = new Point(-1, -1);

                if (p.ImageIndex == s.ImageIndex)
                    org = new Point(p.X, p.Y);

                if (p.ImageIndex == altImageIndex)
                    org = new Point(p.AltX, p.AltY);

                if (org.X != -1)
                {
                    if (org.X < left)
                        left = org.X;

                    if (org.X > right)
                        right = org.X;

                    if (org.Y < top)
                        top = org.Y;

                    if (org.Y > bottom)
                        bottom = org.Y;
                }
            }

            left -= 35;
            right += 35;
            top -= 35;
            bottom += 35;

            double trWidth = right - left;
            double trHeight = bottom - top;
            double scale = 1.0;

            if (trWidth > trHeight)
                scale = actualWidth / trWidth;
            else
                scale = actualHeight / trHeight;

            TransformGroup transf = new TransformGroup();
            transf.Children.Add(new TranslateTransform(-(left+right)/2.0 + actualWidth/2.0, -(top+bottom)/2.0 + actualHeight/2.0));
            transf.Children.Add(new ScaleTransform(scale, scale));

            return transf;
        }


        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
