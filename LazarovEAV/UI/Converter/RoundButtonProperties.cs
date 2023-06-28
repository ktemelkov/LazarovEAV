using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace LazarovEAV.UI.Util
{
    public static class RoundButtonProperties
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(object), typeof(RoundButtonProperties), new FrameworkPropertyMetadata(null));

        public static void SetIcon(DependencyObject obj, object value) { obj.SetValue(IconProperty, value); }
        public static object GetIcon(DependencyObject obj) { return (Path)obj.GetValue(IconProperty); }




        public static string GetTextIcon(DependencyObject obj)
        {
            return (string)obj.GetValue(TextIconProperty);
        }

        public static void SetTextIcon(DependencyObject obj, string value)
        {
            obj.SetValue(TextIconProperty, value);
        }

        public static readonly DependencyProperty TextIconProperty =
            DependencyProperty.RegisterAttached(
                "TextIcon",
                typeof(string),
                typeof(RoundButtonProperties),
                new FrameworkPropertyMetadata("A"));


        public static double GetFontSize(DependencyObject obj)
        {
            return (double)obj.GetValue(FontSizeProperty);
        }

        public static void SetFontSize(DependencyObject obj, double value)
        {
            obj.SetValue(FontSizeProperty, value);
        }

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached(
                "FontSize",
                typeof(double),
                typeof(RoundButtonProperties),
                new FrameworkPropertyMetadata(12.0));


        public static Thickness GetIconMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(IconMarginProperty);
        }

        public static void SetIconMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(IconMarginProperty, value);
        }

        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.RegisterAttached(
                "IconMargin",
                typeof(Thickness),
                typeof(RoundButtonProperties),
                new FrameworkPropertyMetadata(new Thickness(0,0,0,0)));


        public static double GetIconSize(DependencyObject obj)
        {
            return (double)obj.GetValue(IconSizeProperty);
        }

        public static void SetIconSize(DependencyObject obj, double value)
        {
            obj.SetValue(IconSizeProperty, value);
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.RegisterAttached(
                "IconSize",
                typeof(double),
                typeof(RoundButtonProperties),
                new FrameworkPropertyMetadata(30.0));
    
    }
}
