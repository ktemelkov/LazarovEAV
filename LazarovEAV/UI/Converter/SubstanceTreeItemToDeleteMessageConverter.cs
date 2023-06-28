using Aga.Controls.Tree;
using LazarovEAV.Model;
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
    class SubstanceTreeItemToDeleteMessageConverter : IValueConverter
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
            if (value == null)
                return null;
            

            if (value is TreeNode && ((TreeNode)value).Tag is SubstanceTreeItemViewModel
                        && ((SubstanceTreeItemViewModel)((TreeNode)value).Tag).Substance != null)
                return "Ще потвърдите ли изтриването на избрания препарат от каталога?";

            return "Ще потвърдите ли изтриването на избраната категория и прилежащите й препарати от каталога?";
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
            throw new InvalidOperationException();
        }
    }
}
