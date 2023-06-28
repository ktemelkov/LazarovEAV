using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LazarovEAV.UI.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class BBCodeFormatter
    {
        public static readonly DependencyProperty FormattedTextProperty = DependencyProperty.RegisterAttached(
                            "FormattedText", 
                            typeof(string), 
                            typeof(BBCodeFormatter), 
                            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure, FormattedTextPropertyChanged));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBlock"></param>
        /// <param name="value"></param>
        public static void SetFormattedText(DependencyObject textBlock, string value)
        {
            textBlock.SetValue(FormattedTextProperty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBlock"></param>
        /// <returns></returns>
        public static string GetFormattedText(DependencyObject textBlock)
        {
            return (string)textBlock.GetValue(FormattedTextProperty);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void FormattedTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as TextBlock;

            if (textBlock == null)
                return;

            textBlock.Inlines.Clear();
            textBlock.Inlines.Add(BBCodeToInlines.Convert((string)e.NewValue ?? string.Empty));
        }
    }
}
