using Aga.Controls.Tree;
using LazarovEAV.Model;
using LazarovEAV.Util;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    class SubstanceSelectionConverter : IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || !(values[1] is TreeList))
                return null;

            TreeList TreeControl = (TreeList)values[1];

            var model = (SubstanceTreeContentProvider)TreeControl.Model;
            var path = model.GetPathToItem((SubstanceTreeItemViewModel)values[0]);
            var nodes = TreeControl.Nodes;
            TreeNode node = null;
            
            foreach (var expandId in path)
            {
                node = nodes.FirstOrDefault(x => ((SubstanceTreeItemViewModel)x.Tag).Folder != null && ((SubstanceTreeItemViewModel)x.Tag).Id == expandId);

                if (node == null)
                    return null;

                node.IsExpanded = true;
                nodes = node.Nodes;
            }

            if (nodes != null)
            {
                var id = ((SubstanceTreeItemViewModel)values[0]).Id;
                node = nodes.FirstOrDefault(x => ((SubstanceTreeItemViewModel)x.Tag).Id == id);
            }

            return node;
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
            if (value == null)
                return new object[] { null, null };

            return new object[] { ((TreeNode)value).Tag, ((TreeNode)value).Tree };
        }
    }
}
