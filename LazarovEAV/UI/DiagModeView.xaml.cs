using LazarovEAV.Model;
using LazarovEAV.Util.Util;
using LazarovEAV.ViewModel;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for ClientLayout.xaml
    /// </summary>
    public partial class DiagModeView : UserControl, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public DiagModeView()
        {
            try
            {
                InitializeComponent();

                this.pointGraph.ActualModel.MouseDown += onPlotMouseDown;

                DependencyPropertyDescriptor
                    .FromProperty(OxyPlot.Wpf.Series.ItemsSourceProperty, typeof(OxyPlot.Wpf.Series))
                    .AddValueChanged(this.pointGraph.Series[0], onUpdateSeries);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.pointGraph.ActualModel.MouseDown -= onPlotMouseDown;

            DependencyPropertyDescriptor
                .FromProperty(OxyPlot.Wpf.Series.ItemsSourceProperty, typeof(OxyPlot.Wpf.Series))
                .RemoveValueChanged(this.pointGraph.Series[0], onUpdateSeries);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onPlotMouseDown(object sender, OxyMouseDownEventArgs e)
        {
            if (e.ChangedButton == OxyMouseButton.Right)
            {
                IEnumerable points = this.pointGraph.Series[0].ItemsSource;

                foreach (var p in points)
                {
                    Model.DataPoint point = (Model.DataPoint)p;

                    Debug.WriteLine(point.Time + "\t" + point.Value);                    
                }

                e.Handled = true;
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
                    return new ScatterPoint((double)((LazarovEAV.Model.DataPoint)item).Time, ((LazarovEAV.Model.DataPoint)item).Value);
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
                    return new OxyPlot.DataPoint((double)((LazarovEAV.Model.DataPoint)item).Time, ((LazarovEAV.Model.DataPoint)item).Value);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onUpdateSeries(object sender, EventArgs e)
        {
            // updateAnnotations();
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateAnnotations()
        {
            for (int i = this.pointGraph.Annotations.Count - 1; i > 1; i--)
            {
                BindingOperations.ClearAllBindings(this.pointGraph.Annotations[i]);
                this.pointGraph.Annotations.RemoveAt(i);
            }

            if (this.pointGraph.Series != null && this.pointGraph.Series.Count > 1 && this.pointGraph.Series[1].ItemsSource != null)
            {
                foreach (var item in this.pointGraph.Series[1].ItemsSource)
                {
                }
            }

            this.pointGraph.InvalidatePlot(false);
        }
    }
}
