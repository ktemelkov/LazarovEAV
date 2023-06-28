using Aga.Controls.Tree;
using LazarovEAV.Model;
using LazarovEAV.Util.Util;
using LazarovEAV.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for SubstanceSelector.xaml
    /// </summary>
    public partial class SubstanceSelector : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public SubstanceSelector()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        public string Potency
        {
            get
            {
                return (SubstanceType)this.substanceType.SelectedItem == SubstanceType.HOMEOPATHIC ? this.substancePotencyCombo.Text : this.substancePotency.Text;
            }

            set
            {
                if ((SubstanceType)this.substanceType.SelectedItem == SubstanceType.HOMEOPATHIC)
                    this.substancePotencyCombo.Text = value;
                else
                    this.substancePotency.Text = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void substanceListItem_Click(object sender, RoutedEventArgs e)
        {
            applySubstance();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void substancesTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            applySubstance();
        }


        /// <summary>
        /// 
        /// </summary>
        private void applySubstance()
        {
            object obj = this.substancesTree.SelectedItem;

            if (obj != null && obj is TreeNode && ((TreeNode)obj).Tag is SubstanceTreeItemViewModel)
            {
                SubstanceTreeItemViewModel substanceVM = (SubstanceTreeItemViewModel)((TreeNode)obj).Tag;

                if (substanceVM.Substance != null)
                {
                    SubstanceInfo substance = substanceVM.Substance;

                    this.substanceName.Text = substance.Name;
                    this.substanceDescription.Text = substance.Description;
                    this.substanceType.SelectedIndex = (int)substance.Type;
                    this.substancePotency.Text = "";
                    this.substancePotencyCombo.SelectedIndex = 0;

                    this.substanceListButton.IsChecked = false;
                    this.expandTree(this.substancesTree.Nodes);
                }
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
        private void comboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.substanceName.Text))
            {
                this.expandTree(this.substancesTree.Nodes);
            }
            else
            {
                this.collapseTree(this.substancesTree.Nodes);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void substanceName_DropDownClosed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.substanceName.Text))
            {
                this.expandTree(this.substancesTree.Nodes);

                if (this.substanceType.SelectedIndex == 1)
                {
                    this.substancePotencyCombo.SelectedIndex = 0;
                }
            }
            else
            {
                this.collapseTree(this.substancesTree.Nodes);
            }
        }
    }
}
