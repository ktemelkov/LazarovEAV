using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;


namespace LazarovEAV.UI.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class MenuButtonProperties
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached("Icon", typeof(Path),
                                                                                            typeof(MenuButtonProperties), new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.RegisterAttached("IconMargin", typeof(Thickness),
                                                                                            typeof(MenuButtonProperties), new FrameworkPropertyMetadata(new Thickness(0, 0, 0, 0)));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.RegisterAttached("IconSize", typeof(double),
                                                                                            typeof(MenuButtonProperties), new FrameworkPropertyMetadata(30.0));

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("Text", typeof(string),
                                                                                            typeof(MenuButtonProperties), new FrameworkPropertyMetadata("A"));

        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.RegisterAttached("FontSize", typeof(double),
                                                                                            typeof(MenuButtonProperties), new FrameworkPropertyMetadata(12.0));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Path GetIcon(DependencyObject obj)
        {
            return (Path)obj.GetValue(IconProperty);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetIcon(DependencyObject obj, Path value)
        {
            obj.SetValue(IconProperty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetFontSize(DependencyObject obj)
        {
            return (double)obj.GetValue(FontSizeProperty);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetFontSize(DependencyObject obj, double value)
        {
            obj.SetValue(FontSizeProperty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Thickness GetIconMargin(DependencyObject obj)
        {
            return (Thickness)obj.GetValue(IconMarginProperty);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetIconMargin(DependencyObject obj, Thickness value)
        {
            obj.SetValue(IconMarginProperty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetIconSize(DependencyObject obj)
        {
            return (double)obj.GetValue(IconSizeProperty);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetIconSize(DependencyObject obj, double value)
        {
            obj.SetValue(IconSizeProperty, value);
        }
    }
}
