using Aga.Controls.Tree;
using LazarovEAV.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class SubstanceSelectorViewModel : DependencyObject, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TreeContentProviderProperty =
                        DependencyProperty.Register("TreeContentProvider", typeof(ITreeModel), typeof(SubstanceSelectorViewModel),
                        new FrameworkPropertyMetadata(null));

        public ITreeModel TreeContentProvider { get { return (ITreeModel)GetValue(TreeContentProviderProperty); } set { SetValue(TreeContentProviderProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SubstanceNameProperty =
                        DependencyProperty.Register("SubstanceName", typeof(string), typeof(SubstanceSelectorViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { ((SubstanceSelectorViewModel)o).onSubstanceNameChanged((string)e.NewValue); }));

        public string SubstanceName { get { return (string)GetValue(SubstanceNameProperty); } set { SetValue(SubstanceNameProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SubstanceDescriptionProperty =
                        DependencyProperty.Register("SubstanceDescription", typeof(string), typeof(SubstanceSelectorViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { }));

        public string SubstanceDescription { get { return (string)GetValue(SubstanceDescriptionProperty); } set { SetValue(SubstanceDescriptionProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SubstanceSuggestionsProperty =
                        DependencyProperty.Register("SubstanceSuggestions", typeof(IEnumerable<EffectiveSubstanceInfoViewModel>), typeof(SubstanceSelectorViewModel),
                        new FrameworkPropertyMetadata(new List<EffectiveSubstanceInfoViewModel>(), (o, e) => { }));

        public IEnumerable<EffectiveSubstanceInfoViewModel> SubstanceSuggestions { get { return (IEnumerable<EffectiveSubstanceInfoViewModel>)GetValue(SubstanceSuggestionsProperty); } set { SetValue(SubstanceSuggestionsProperty, value); } }



        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedSubstanceTypeProperty =
                        DependencyProperty.Register("SelectedSubstanceType", typeof(SubstanceType), typeof(SubstanceSelectorViewModel),
                        new FrameworkPropertyMetadata(SubstanceType.OTHER));

        public SubstanceType SelectedSubstanceType { get { return (SubstanceType)GetValue(SelectedSubstanceTypeProperty); } set { SetValue(SelectedSubstanceTypeProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ActivePatientProperty =
                        DependencyProperty.Register("ActivePatient", typeof(PatientViewModel), typeof(SubstanceSelectorViewModel),
                        new PropertyMetadata(null, (o, args) => { ((SubstanceSelectorViewModel)o).updateCache(); }));

        internal PatientViewModel ActivePatient { get { return (PatientViewModel)GetValue(ActivePatientProperty); } set { SetValue(ActivePatientProperty, value); } }

        private EntityManager itemManager;


        /// <summary>
        /// 
        /// </summary>
        public SubstanceSelectorViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new Mock.MockDependencyObject()))
            {
                this.TreeContentProvider = new SubstanceTreeContentProvider(this.itemManager = new EntityManager());
            }
            else
            {
                this.TreeContentProvider = new SubstanceTreeMockProvider();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.TreeContentProvider = null;
            this.itemManager.Dispose();
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newValue"></param>
        private void onSubstanceNameChanged(string newValue)
        {
            this.TreeContentProvider = new SubstanceTreeContentProvider(this.itemManager, newValue);

            EffectiveSubstanceInfoViewModel suggestion = this.SubstanceSuggestions.FirstOrDefault(s => s.Name == newValue);

            SubstanceType st = SubstanceType.OTHER;

            if (suggestion != null)
            {
                st = suggestion.Type;
            }

            this.SelectedSubstanceType = st;
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateCache()
        {
            if (this.ActivePatient != null && this.ActivePatient.CurrentSession != null)
            {
                var left = this.ActivePatient.CurrentSession.ResultsLeft.Where(r => r.Setup != null && !r.Setup.StartsWith("["));
                var right = this.ActivePatient.CurrentSession.ResultsRight.Where(r => r.Setup != null && !r.Setup.StartsWith("["));

                this.SubstanceSuggestions = left.Concat(right).Select(r => new EffectiveSubstanceInfoViewModel(r.Setup));

                if (this.ActivePatient.CurrentSession.TestTableList != null)
                {
                    this.SubstanceSuggestions = this.SubstanceSuggestions.Concat(this.ActivePatient.CurrentSession.TestTableList
                                            .SelectMany(x => x.Positions).Where(p => p.Substance != null).Select(s => s.Substance));
                }

                this.SubstanceSuggestions = this.SubstanceSuggestions.GroupBy(o => o.Name).Select(g => g.First());
            }
            else
            {
                this.SubstanceSuggestions = new List<EffectiveSubstanceInfoViewModel>();
            }
        }
    }
}
