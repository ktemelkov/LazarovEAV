using Aga.Controls.Tree;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for SubstanceEditor.xaml
    /// </summary>
    public partial class SubstanceEditor : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public SubstanceEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Editor_Unloaded(object sender, RoutedEventArgs e)
        {
            var disp = this.DataContext as IDisposable;

            if (disp != null)
                disp.Dispose();
        }


        private void filterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.filterText.Text))
            {
                this.expandTree(this.treeList.Nodes);
            }
            else
            {
                this.collapseTree(this.treeList.Nodes);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        private void expandTree(ReadOnlyCollection<TreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.IsExpandable)
                {
                    node.IsExpanded = true;
                    this.expandTree(node.Nodes);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        private void collapseTree(ReadOnlyCollection<TreeNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.IsExpandable)
                {
                    node.IsExpanded = false;
                    this.collapseTree(node.Nodes);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.filterText.Text = null;
        }

        private void treeList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(this.treeList, e.GetPosition(this.treeList));

            Control controlUnderMouse = this.GetParentOfType<ListBoxItem>(hitTestResult.VisualHit);

            if (controlUnderMouse == null)
                this.treeList.SelectedItem = null;

        }


        private T GetParentOfType<T>(DependencyObject element) where T : DependencyObject
        {
            Type type = typeof(T);

            if (element == null)
                return null;

            DependencyObject parent = VisualTreeHelper.GetParent(element);

            if (parent == null && ((FrameworkElement)element).Parent is DependencyObject)
                parent = ((FrameworkElement)element).Parent;

            if (parent == null)
                return null;
            else if (parent.GetType() == type || parent.GetType().IsSubclassOf(type))
                return parent as T;

            return this.GetParentOfType<T>(parent);
        }
    }
}
