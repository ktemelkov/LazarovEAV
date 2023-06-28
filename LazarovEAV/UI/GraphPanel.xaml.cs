using LazarovEAV.Config;
using LazarovEAV.Model;
using LazarovEAV.Util.Util;
using LazarovEAV.ViewModel;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace LazarovEAV.UI
{

    /// <summary>
    /// Interaction logic for GraphPanel.xaml
    /// </summary>
    public partial class GraphPanel : UserControl, IDisposable
    {
        public static readonly DependencyProperty BodySideProperty =
                        DependencyProperty.Register("BodySide", typeof(PositionType), typeof(GraphPanel),
                        new FrameworkPropertyMetadata(PositionType.RIGHT));

        internal PositionType BodySide { get { return (PositionType)GetValue(BodySideProperty); } set { SetValue(BodySideProperty, value); } }

        
        public static readonly DependencyProperty SelectedSideProperty =
                        DependencyProperty.Register("SelectedSide", typeof(PositionType), typeof(GraphPanel),
                        new FrameworkPropertyMetadata(PositionType.UNDEFINED));

        internal PositionType SelectedSide { get { return (PositionType)GetValue(SelectedSideProperty); } set { SetValue(SelectedSideProperty, value); } }



        public static readonly DependencyProperty SelectedPointIndexProperty =
                        DependencyProperty.Register("SelectedPointIndex", typeof(int), typeof(GraphPanel),
                        new FrameworkPropertyMetadata(0));


        /// <summary>
        /// 
        /// </summary>
        internal int SelectedPointIndex
        {
            get { return (int)GetValue(SelectedPointIndexProperty); }
            set { SetValue(SelectedPointIndexProperty, value); }
        }



        /// <summary>
        /// 
        /// </summary>
        public GraphPanel()
        {
            InitializeComponent();

            this.plot.ActualModel.MouseDown += onPlotMouseDown;

            DependencyPropertyDescriptor
                .FromProperty(OxyPlot.Wpf.Series.ItemsSourceProperty, typeof(OxyPlot.Wpf.Series))
                .AddValueChanged(this.plot.Series[0], onUpdateSeries);
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.plot.ActualModel.MouseDown -= onPlotMouseDown;

            DependencyPropertyDescriptor
                .FromProperty(OxyPlot.Wpf.Series.ItemsSourceProperty, typeof(OxyPlot.Wpf.Series))
                .RemoveValueChanged(this.plot.Series[0], onUpdateSeries);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onUpdateSeries(object sender, EventArgs e)
        {
            updateAnnotations();
        }



        /// <summary>
        /// 
        /// </summary>
        private void updateAnnotations()
        {
            for (int i = this.plot.Annotations.Count - 1; i > 1; i--)
            {
                BindingOperations.ClearAllBindings(this.plot.Annotations[i]);
                this.plot.Annotations.RemoveAt(i);
            }

            if (this.plot.Series != null && this.plot.Series.Count > 0 && this.plot.Series[0].ItemsSource != null)
            {
                foreach (var item in this.plot.Series[0].ItemsSource)
                {
                    if (item is TestResultViewModel)
                    {
                        TestResultViewModel res = (TestResultViewModel)item;
                        
                        if (res.HasDeviation)
                        {
                            ArrowAnnotation ann = new ArrowAnnotation();
                            ann.StartPoint = new OxyPlot.DataPoint(res.MeridianPointIndex, res.ControlPoints[0].Value);
                            ann.EndPoint = new OxyPlot.DataPoint(res.MeridianPointIndex, res.ControlPoints[1].Value);

                            BindingOperations.SetBinding(ann, ArrowAnnotation.ColorProperty, new Binding("NormalDataColor") { Source = UiConfig.Instance });
                            BindingOperations.SetBinding(ann, ArrowAnnotation.StrokeThicknessProperty, new Binding("MarkerSize") { Source = this.plot.Series[0], Converter = new SizeConverter(), ConverterParameter = "1" });

                            ann.HeadWidth = 1.0;
                            ann.HeadLength = 2.0;

                            this.plot.Annotations.Add(ann);
                        }
                    }                    
                }
            }

            this.plot.InvalidatePlot(false);
        }


        /// <summary>
        /// 
        /// </summary>
        public Func<object, ScatterPoint> DataMappingFunc
        {
            get
            {
                return (item) =>
                {
                    return new ScatterPoint((double)((TestResultViewModel)item).MeridianPointIndex, ((TestResultViewModel)item).ResultValue);
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
                    int index = (int)item;

                    IEnumerable<MeridianPointViewModel> points = (IEnumerable<MeridianPointViewModel>)((OxyPlot.Wpf.CategoryAxis)this.plot.Axes[0]).ItemsSource;

                    if (points != null && index >= 0 && index < points.Count())
                    {
                        string label = points.ElementAt(index).Name;

                        if (label.Length > 0)
                        {
                            int p = label.IndexOf("-") + 1;

                            if (index != 0 && p > 0)
                                label = label.Substring(p);
                        }

                        return label;
                    }

                    return string.Empty;
                };
            }
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

                if (d > this.plot.Axes[0].Maximum + 0.5)
                    d = this.plot.Axes[0].Maximum + 0.5;
                else if (d < 0.0)
                    d = 0.0;

                this.SelectedPointIndex = (int)d;
                this.SelectedSide = this.BodySide;
                e.Handled = true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onPanelClick(object sender, MouseButtonEventArgs e)
        {
            this.SelectedSide = this.BodySide;
            e.Handled = true;
        }
    }
}

