using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace LazarovEAV.UI.Util
{
    class BBCodeToFlowDocumentConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Span root = BBCodeToInlines.Convert((string)value);

            if (parameter != null)
            {
                string[] pp = ((string)parameter).Split(new[] { '|' });

                if (pp.Length > 1)
                {
                    double fsize = 14;
                    Double.TryParse(pp[1], out fsize);
                    root.FontSize = fsize;
                    root.FontFamily = new FontFamily(pp[1]);
                }
                else if (pp.Length > 0)
                {
                    root.FontFamily = new FontFamily(pp[0]);
                }
            }
            else
            {
                root.FontSize = 14;
                root.FontFamily = new FontFamily("Segoe UI");
            }

            return new FlowDocument(new Paragraph(root) { TextAlignment = TextAlignment.Left }) { PagePadding = new Thickness(0) };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
