using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LazarovEAV.UI
{
    public static class ImagePanelProperties
    {
        public static readonly DependencyProperty ActualMeridianProperty =
            DependencyProperty.RegisterAttached("ActualMeridian", typeof(object), typeof(ImagePanelProperties), new FrameworkPropertyMetadata(null));

        public static object GetActualMeridian(DependencyObject obj) { return obj.GetValue(ActualMeridianProperty); }
        public static void SetActualMeridian(DependencyObject obj, object value) { obj.SetValue(ActualMeridianProperty, value); }


        public static readonly DependencyProperty ActualPointProperty =
            DependencyProperty.RegisterAttached("ActualPoint", typeof(object), typeof(ImagePanelProperties), new FrameworkPropertyMetadata(null));

        public static object GetActualPoint(DependencyObject obj) { return obj.GetValue(ActualPointProperty); }
        public static void SetActualPoint(DependencyObject obj, object value) { obj.SetValue(ActualPointProperty, value); }

    }
}
