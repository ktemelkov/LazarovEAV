using LazarovEAV.Config;
using LazarovEAV.Util;
using LazarovEAV.Util.Util;
using LazarovEAV.ViewModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for SubstanceTestView.xaml
    /// </summary>
    public partial class SubstanceTestView : UserControl, IDisposable
    {
        private INotifyPropertyChanged mixTestsSlot = null;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsEditingProperty =
                        DependencyProperty.Register("IsEditing", typeof(bool), typeof(SubstanceTestView),
                        new PropertyMetadata(false));

        internal bool IsEditing { get { return (bool)GetValue(IsEditingProperty); } set { SetValue(IsEditingProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BaseResultsProperty =
                        DependencyProperty.Register("BaseResults", typeof(List<SlotResultViewModel>), typeof(SubstanceTestView),
                        new PropertyMetadata(new List<SlotResultViewModel>()));

        internal List<SlotResultViewModel> BaseResults { get { return (List<SlotResultViewModel>)GetValue(BaseResultsProperty); } set { SetValue(BaseResultsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedResultsProperty =
                        DependencyProperty.Register("SelectedResults", typeof(List<SlotResultViewModel>), typeof(SubstanceTestView),
                        new PropertyMetadata(new List<SlotResultViewModel>()));

        internal List<SlotResultViewModel> SelectedResults { get { return (List<SlotResultViewModel>)GetValue(SelectedResultsProperty); } set { SetValue(SelectedResultsProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverSlotProperty =
                        DependencyProperty.Register("HoverSlot", typeof(int), typeof(SubstanceTestView),
                        new PropertyMetadata(-1));

        internal int HoverSlot { get { return (int)GetValue(HoverSlotProperty); } set { SetValue(HoverSlotProperty, value); } }



        private object selectedTestTableItem;
        private DispatcherTimer updateTimer = new DispatcherTimer();
        private SizeConverter sizeConverter = new SizeConverter();

        /// <summary>
        /// 
        /// </summary>
        public SubstanceTestView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                throw e;
            }

            CategoryAxis a = (CategoryAxis)this.plot.Axes[0];
            a.Labels.Clear();

            for (int j = 0; j < AppConfig.SUBSTANCE_TEST_SLOTS; j++ )
            {
                a.Labels.Add("Б");

                for (int i = 1; i <= AppConfig.TEST_TABLE_POSITIONS; i++)
                    a.Labels.Add(i.ToString());
            }

            a.Labels.Add("Б");

            for (int i = 0; i < AppConfig.MIX_TEST_POSITIONS; i++)
            {
                a.Labels.Add("V" + (i + 1).ToString());
            }


            this.plot.ActualModel.MouseDown += onPlotMouseDown;
            this.plot.ActualModel.MouseMove += onPlotMouseMove;
            this.plot.ActualModel.MouseLeave += onPlotMouseLeave;

            DependencyPropertyDescriptor
                .FromProperty(OxyPlot.Wpf.Series.ItemsSourceProperty, typeof(OxyPlot.Wpf.Series))
                .AddValueChanged(this.plot.Series[0], onUpdateSeries);


            this.updateTimer.Interval = TimeSpan.FromMilliseconds(15);

            this.updateTimer.Tick += new EventHandler((o,e) => {
                this.updateTimer.Stop();
                updateAnnotations();
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onPlotMouseLeave(object sender, OxyMouseEventArgs e)
        {
            this.HoverSlot = -1;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onPlotMouseMove(object sender, OxyMouseEventArgs e)
        {
            double x = 0.5 + this.plot.Axes[0].InternalAxis.InverseTransform(e.Position.X);
            double y = 0.5 + this.plot.Axes[0].InternalAxis.InverseTransform(e.Position.Y);

            if (x > this.plot.Axes[0].Maximum + 0.4999 || x < 0.0
                || y > this.plot.Axes[1].Maximum || y < 0.0)
            {
                this.HoverSlot = -1;
            }
            else
            {
                this.HoverSlot = (int)x / (int)AppConfig.SUBSTANCE_TEST_SLOT_POSITIONS;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.DataContext != null && this.DataContext is DependencyObject)
            {
                TypeDescriptor.GetProperties(this.DataContext)["Slots"].RemoveValueChanged(this.DataContext, onUpdateSlots);
            }

            DependencyPropertyDescriptor
                .FromProperty(OxyPlot.Wpf.Series.ItemsSourceProperty, typeof(OxyPlot.Wpf.Series))
                .RemoveValueChanged(this.plot.Series[0], onUpdateSeries);
         
            this.plot.ActualModel.MouseDown -= onPlotMouseDown;

            if (this.mixTestsSlot != null)
                this.mixTestsSlot.PropertyChanged -= SlotInfoViewModel_PropertyChanged;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onPlotMouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (e.ChangedButton == OxyMouseButton.Left)
            {
                double d = 0.5 + this.plot.Axes[0].InternalAxis.InverseTransform(e.Position.X);

                if (d > this.plot.Axes[0].Maximum + 0.4999)
                    d = this.plot.Axes[0].Maximum + 0.4999;
                else if (d < 0.0)
                    d = 0.0;

                ICommand cmd = (ICommand)DependencyObjectUtil.GetValueByName(this.DataContext, "SelectSlotByIndexCommand");

                if (cmd != null)
                    cmd.Execute((int)d / (int)AppConfig.SUBSTANCE_TEST_SLOT_POSITIONS);


                cmd = (ICommand)DependencyObjectUtil.GetValueByName(this.DataContext, "SelectSlotPositionCommand");

                if (cmd != null)
                    cmd.Execute((int)d % (int)AppConfig.SUBSTANCE_TEST_SLOT_POSITIONS);

                e.Handled = true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Func<double, string> RightAxisLabelFormatterFunc
        {
            get
            {
                return (item) =>
                {
                    double d = (double)item;
                    string res = String.Format("{0,0:N0}", (double)item);

                    if (res.Length == 2)
                        res = " " + res;
                    else if (res.Length == 1)
                        res = "  " + res;

                    return res;
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Func<double, String> LabelFormatterFunc
        {
            get
            {
                return (item) =>
                {
                    return String.Format("{0:n0} s", item / 1000);
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Func<object, ScatterPoint> DataMappingFuncControl
        {
            get
            {
                return (item) =>
                {
                    return new ScatterPoint((double)((Model.DataPoint)item).Time, ((Model.DataPoint)item).Value);
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Func<object, OxyPlot.DataPoint> DataMappingFuncLine
        {
            get
            {
                return (item) =>
                {
                    return new OxyPlot.DataPoint((double)((Model.DataPoint)item).Time, ((Model.DataPoint)item).Value);
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Func<object, ScatterPoint> DataMappingFuncSlotResults
        {
            get
            {
                return (item) =>
                {
                    SlotResultViewModel result = (SlotResultViewModel)item;
                    
                    return new ScatterPoint((double)(result.SlotNumber * (AppConfig.TEST_TABLE_POSITIONS + 1) + result.SlotPosition), result.TestResult.ResultValue);
                };
            }
        }

        public OxyPlot.DataPoint[] AnnotationPosition
        {
            get {
                return new OxyPlot.DataPoint[] 
                {
                    new OxyPlot.DataPoint((AppConfig.TEST_TABLE_POSITIONS + 1)/2, 2),
                    new OxyPlot.DataPoint(3*(AppConfig.TEST_TABLE_POSITIONS + 1)/2, 2),
                    new OxyPlot.DataPoint(5*(AppConfig.TEST_TABLE_POSITIONS + 1)/2, 2),
                    new OxyPlot.DataPoint(7*(AppConfig.TEST_TABLE_POSITIONS + 1)/2, 2),
                    new OxyPlot.DataPoint(9*(AppConfig.TEST_TABLE_POSITIONS + 1)/2, 2),
                    new OxyPlot.DataPoint(5*(AppConfig.TEST_TABLE_POSITIONS + 1) + (AppConfig.MIX_TEST_POSITIONS+1)/2 - 0.5, 9.5),
                    new OxyPlot.DataPoint(5*(AppConfig.TEST_TABLE_POSITIONS + 1) + (AppConfig.MIX_TEST_POSITIONS+1)/2 - 0.5, 2)
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.mixTestsSlot != null)
                this.mixTestsSlot.PropertyChanged -= SlotInfoViewModel_PropertyChanged;

            this.mixTestsSlot = null;

            if (e.NewValue != null && e.NewValue is DependencyObject)
            {
                TypeDescriptor.GetProperties(e.NewValue)["Slots"].AddValueChanged(e.NewValue, onUpdateSlots);

                attachMixTestSlotEvents((DependencyObject)e.NewValue);
            }

            updateTestTableSelection();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dc"></param>
        private void attachMixTestSlotEvents(DependencyObject dc)
        {
            DependencyProperty dp;
            object slotList;
            PropertyInfo indexer;
            object slotInfo;

            if (null != (dp = DependencyObjectUtil.GetDependencyPropertyByName(dc, "SlotsProperty"))
                && null != (slotList = dc.GetValue(dp))
                && null != (indexer = slotList.GetType().GetProperty("Item"))
                && null != (slotInfo = indexer.GetValue(slotList, new object[] { AppConfig.SUBSTANCE_TEST_SLOTS }))
                && slotInfo is INotifyPropertyChanged)
            {
                this.mixTestsSlot = (INotifyPropertyChanged)slotInfo;
                this.mixTestsSlot.PropertyChanged += SlotInfoViewModel_PropertyChanged;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onUpdateSlots(object sender, EventArgs e)
        {
            if (this.mixTestsSlot != null)
                this.mixTestsSlot.PropertyChanged -= SlotInfoViewModel_PropertyChanged;

            this.mixTestsSlot = null;

            if (this.DataContext != null && this.DataContext is DependencyObject)
            {
                attachMixTestSlotEvents((DependencyObject)this.DataContext);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlotInfoViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPosition")
            {
                updateTestTableSelection();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateTestTableSelection()
        {
            if (this.mixTestsTable == null)
                return;

            this.mixTestsTable.SelectionChanged -= mixTestsTable_SelectionChanged;

            this.mixTestsTable.UnselectAll();

            Collection<int> selectedIndexes = getSelectedIndexes();

            if (selectedIndexes != null)
            {
                foreach (int idx in selectedIndexes)
                {
                    this.mixTestsTable.SelectedItems.Add(this.mixTestsTable.Items[idx]);
                }
            }

            this.mixTestsTable.SelectionChanged += mixTestsTable_SelectionChanged;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Collection<int> getSelectedIndexes()
        {
            int position;
            PropertyInfo posProp;
            PropertyInfo dataProp;
            object positionData;
            PropertyInfo indexer;
            object slotPosViewModel;
            PropertyInfo selSubstProp;
            object selectedIndexes;

            if (null != this.mixTestsSlot
                && null != (posProp = this.mixTestsSlot.GetType().GetProperty("SelectedPosition"))
                && 0 < (position = (int)posProp.GetValue(this.mixTestsSlot))
                && null != (dataProp = this.mixTestsSlot.GetType().GetProperty("PositionData"))
                && null != (positionData = dataProp.GetValue(this.mixTestsSlot))
                && null != (indexer = positionData.GetType().GetProperty("Item"))
                && null != (slotPosViewModel = indexer.GetValue(positionData, new object[] { position - 1 }))
                && null != (selSubstProp = slotPosViewModel.GetType().GetProperty("SelectedSubstances"))
                && null != (selectedIndexes = selSubstProp.GetValue(slotPosViewModel))
                && selectedIndexes is Collection<int>)
            {
                return (Collection<int>)selectedIndexes;
            }

            return null;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int getSelectedLinearPosition()
        {
            if (this.DataContext != null && this.DataContext is DependencyObject)
            {
                var dc = (DependencyObject)this.DataContext;

                DependencyProperty dp;
                object selAsObject;

                if (null != (dp = DependencyObjectUtil.GetDependencyPropertyByName(dc, "SelectedLinearPositionProperty"))
                    && null != (selAsObject = dc.GetValue(dp))
                    && selAsObject is int)
                {
                    return (int)selAsObject;
                }
            }

            return -1;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mixTestsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Collection<int> selectedIndexes = getSelectedIndexes();

            if (selectedIndexes != null)
            {
                selectedIndexes.Clear();

                foreach (var item in this.mixTestsTable.SelectedItems)
                {
                    selectedIndexes.Add(this.mixTestsTable.Items.IndexOf(item));
                }
            }
            else
            {
                this.mixTestsTable.SelectionChanged -= mixTestsTable_SelectionChanged;

                this.mixTestsTable.UnselectAll();

                this.mixTestsTable.SelectionChanged += mixTestsTable_SelectionChanged;
            }


            ICommand cmd = (ICommand)DependencyObjectUtil.GetValueByName(this.DataContext, "UpdateMultiTestSlotsCommand");

            if (cmd != null)
                cmd.Execute(null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onUpdateSeries(object sender, EventArgs e)
        {
            if (this.updateTimer.IsEnabled)
                this.updateTimer.Stop();

            this.updateTimer.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateAnnotations()
        {
            int minAnnotations = 3 + AppConfig.SUBSTANCE_TEST_SLOTS + AppConfig.MIX_TEST_SLOTS*2; // 3 -> EAV Window + Slot Selection + Position Selection + Slot Labels 

            for (int i = this.plot.Annotations.Count - 1; i >= minAnnotations; i--)
            {
                BindingOperations.ClearAllBindings(this.plot.Annotations[i]);
                this.plot.Annotations.RemoveAt(i);
            }

            this.BaseResults.Clear();
            this.SelectedResults.Clear();

            int slotPositions = AppConfig.TEST_TABLE_POSITIONS + 1;
            int selectedPos = this.getSelectedLinearPosition();
            int mixSlotsStart = AppConfig.SUBSTANCE_TEST_SLOTS * slotPositions;

            if (this.plot.Series != null && this.plot.Series.Count > 0 && this.plot.Series[0].ItemsSource != null)
            {
                foreach (var item in this.plot.Series[0].ItemsSource)
                {
                    if (item is SlotResultViewModel)
                    {
                        SlotResultViewModel res = (SlotResultViewModel)item;

                        bool isSelected = (res.SlotPosition == (selectedPos % slotPositions)
                                && ((selectedPos % slotPositions) == 0
                                    || (selectedPos <= mixSlotsStart && res.SlotNumber < AppConfig.SUBSTANCE_TEST_SLOTS)
                                    || (selectedPos > mixSlotsStart && res.SlotNumber >= AppConfig.SUBSTANCE_TEST_SLOTS))) ;

                        if (isSelected)
                            this.SelectedResults.Add(res);
                        else if (res.SlotPosition == 0)
                            this.BaseResults.Add(res);

                        if (res.TestResult.HasDeviation)
                        {
                            ArrowAnnotation ann = new ArrowAnnotation();
                            ScatterPoint pt = DataMappingFuncSlotResults(res);

                            ann.StartPoint = new OxyPlot.DataPoint(pt.X, res.TestResult.ControlPoints[0].Value);
                            ann.EndPoint = new OxyPlot.DataPoint(pt.X, res.TestResult.ControlPoints[1].Value);

                            if (isSelected)
                                BindingOperations.SetBinding(ann, ArrowAnnotation.ColorProperty, new Binding("SelectedDataColor") { Source = UiConfig.Instance });
                            else if (res.SlotPosition == 0)
                                BindingOperations.SetBinding(ann, ArrowAnnotation.ColorProperty, new Binding("BaseDataColor") { Source = UiConfig.Instance });
                            else
                                BindingOperations.SetBinding(ann, ArrowAnnotation.ColorProperty, new Binding("NormalDataColor") { Source = UiConfig.Instance });

                            BindingOperations.SetBinding(ann, ArrowAnnotation.StrokeThicknessProperty, new Binding("MarkerSize") { Source = this.plot.Series[0], Converter = this.sizeConverter, ConverterParameter = "1" });

                            ann.HeadWidth = 1.0;
                            ann.HeadLength = 2.0;

                            this.plot.Annotations.Add(ann);
                        }
                    }
                }                
            }

            this.plot.InvalidatePlot(true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectSubstance_Click(object sender, RoutedEventArgs e)
        {
            this.IsEditing = true;
            
            SubstanceSelector selector = new SubstanceSelector();

            selector.closeButton.Click += substanceSelectorClose_Click;
            selector.applyButton.Click += substanceSelectorApply_Click;
            
            if (this.testTable1.SelectedItem != null && this.testTable1.SelectedItem is TestTableInfoViewModel)
            {
                TestTableInfoViewModel model = (TestTableInfoViewModel)this.testTable1.SelectedItem;
                this.selectedTestTableItem = model;

                if (model.Substance != null)
                {
                    selector.substanceName.Text = model.Substance.Name;
                    selector.substanceType.SelectedIndex = (int)model.Substance.Type;
                    selector.Potency = model.Substance.Quantity;
                    selector.substanceDescription.Text = model.Substance.Description;
                }
            }

            bindSubstanceSelector(selector);

            this.overlayContent.Content = selector;
            this.overlay.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// 
        /// </summary>
        private void applySelectedSubstance()
        {
            if (this.overlayContent.Content != null)
            {
                SubstanceSelector selector = (SubstanceSelector)this.overlayContent.Content;

                EffectiveSubstanceInfoViewModel substance = new EffectiveSubstanceInfoViewModel(selector.substanceName.Text, selector.substanceDescription.Text, selector.substanceType.SelectedIndex, selector.Potency);

                if (this.selectedTestTableItem != null && this.selectedTestTableItem is TestTableInfoViewModel)
                {
                    TestTableInfoViewModel model = (TestTableInfoViewModel)this.selectedTestTableItem;
                    model.Substance = substance;


                    if (this.DataContext != null && this.DataContext is DependencyObject)
                    {
                        DependencyObject dc = (DependencyObject)this.DataContext;

                        ICommand cmd = (ICommand)DependencyObjectUtil.GetValueByName(dc, "EditSubstanceCommand");

                        if (cmd != null)
                            cmd.Execute(null);
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void closeSubstanceSelector()
        {
            if (this.overlayContent.Content != null)
            {
                object dc = ((UserControl)this.overlayContent.Content).DataContext;

                if (dc != null && dc != this.DataContext)
                {
                    if (dc is DependencyObject)
                        BindingOperations.ClearAllBindings((DependencyObject)dc);

                    if (dc is IDisposable)
                        ((IDisposable)dc).Dispose();
                }

                if (this.overlayContent.Content is DependencyObject)
                    BindingOperations.ClearAllBindings((DependencyObject)this.overlayContent.Content);

                if (this.overlayContent.Content is IDisposable)
                    ((IDisposable)this.overlayContent.Content).Dispose();

                SubstanceSelector selector = (SubstanceSelector)this.overlayContent.Content;
                selector.closeButton.Click -= substanceSelectorClose_Click;
                selector.applyButton.Click -= substanceSelectorApply_Click;

                this.overlayContent.Content = null;
                this.overlay.Visibility = Visibility.Hidden;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void substanceSelectorClose_Click(object sender, RoutedEventArgs e)
        {
            this.IsEditing = false;
            
            closeSubstanceSelector();

            this.selectedTestTableItem = null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void substanceSelectorApply_Click(object sender, RoutedEventArgs e)
        {
            this.IsEditing = false;

            applySelectedSubstance();
            closeSubstanceSelector();

            this.selectedTestTableItem = null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectSubstanceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement))
                return;

            object listItem = ((FrameworkElement)sender).Tag;

            if (listItem == null)
                return;

            this.IsEditing = true;

            SubstanceSelector selector = new SubstanceSelector();

            selector.closeButton.Click += substanceSelectorClose_Click;
            selector.applyButton.Click += substanceSelectorApply_Click;

            if (listItem is TestTableInfoViewModel)
            {
                TestTableInfoViewModel model = (TestTableInfoViewModel)listItem;
                this.selectedTestTableItem = model;

                if (model.Substance != null)
                {
                    selector.substanceName.Text = model.Substance.Name;
                    selector.substanceType.SelectedIndex = (int)model.Substance.Type;
                    selector.Potency = model.Substance.Quantity;
                }
            }

            bindSubstanceSelector(selector);

            this.overlayContent.Content = selector;
            this.overlay.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        private void bindSubstanceSelector(SubstanceSelector selector)
        {
            if (selector.DataContext != null && selector.DataContext is DependencyObject)
            {
                DependencyObject dc = (DependencyObject)selector.DataContext;

                DependencyProperty dp;

                if (null != (dp = DependencyObjectUtil.GetDependencyPropertyByName(dc, "ActivePatientProperty")))
                {
                    BindingOperations.SetBinding(dc, dp, new Binding("ActivePatient") { Source = this.DataContext });
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteSubstanceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is FrameworkElement))
                return;

            object listItem = ((FrameworkElement)sender).Tag;

            if (listItem == null)
                return;

            if (this.DataContext != null && this.DataContext is DependencyObject)
            {
                DependencyObject dc = (DependencyObject)this.DataContext;

                ICommand cmd = (ICommand)DependencyObjectUtil.GetValueByName(dc, "RemoveSubstanceCommand");

                if (cmd != null)
                    cmd.Execute(this.testTable1.Items.IndexOf(listItem));
            }
        }
    }
}
