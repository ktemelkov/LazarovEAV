using LazarovEAV.Config;
using LazarovEAV.ViewModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for LiveViewPanel.xaml
    /// </summary>
    public partial class LiveViewPanel : UserControl
    {
        public static readonly DependencyProperty LiveSampleProperty =
                                                      DependencyProperty.Register("LiveSample", typeof(Model.DataPoint), typeof(LiveViewPanel),
                                                      new PropertyMetadata(null, (o, arg) => { ((LiveViewPanel)o).onNewSample((Model.DataPoint)arg.NewValue); }));

        internal Model.DataPoint LiveSample { get { return (Model.DataPoint)GetValue(LiveSampleProperty); } set { SetValue(LiveSampleProperty, value); } }


        public static readonly DependencyProperty FilteredSampleProperty =
                                                      DependencyProperty.Register("FilteredSample", typeof(double), typeof(LiveViewPanel),
                                                      new PropertyMetadata(0.0));

        internal double FilteredSample { get { return (double)GetValue(FilteredSampleProperty); } set { SetValue(FilteredSampleProperty, value); } }


        public static readonly DependencyProperty MeridiansProperty =
                        DependencyProperty.Register("Meridians", typeof(List<MeridianViewModel>), typeof(LiveViewPanel),
                        new PropertyMetadata(null));

        internal List<MeridianViewModel> Meridians { get { return (List<MeridianViewModel>)GetValue(MeridiansProperty); } set { SetValue(MeridiansProperty, value); } }


        public static readonly DependencyProperty SelectedMeridianProperty =
                        DependencyProperty.Register("SelectedMeridian", typeof(MeridianViewModel), typeof(LiveViewPanel),
                        new PropertyMetadata(null));

        internal MeridianViewModel SelectedMeridian { get { return (MeridianViewModel)GetValue(SelectedMeridianProperty); } set { SetValue(SelectedMeridianProperty, value); } }



        public static readonly DependencyProperty SelectedSideProperty =
                        DependencyProperty.Register("SelectedSide", typeof(PositionType), typeof(LiveViewPanel),
                        new PropertyMetadata(PositionType.RIGHT));

        internal PositionType SelectedSide { get { return (PositionType)GetValue(SelectedSideProperty); } set { SetValue(SelectedSideProperty, value); } }


        public static readonly DependencyProperty StatusProperty =
                        DependencyProperty.Register("Status", typeof(Status), typeof(LiveViewPanel),
                        new PropertyMetadata(null));

        internal Status Status { get { return (Status)GetValue(StatusProperty); } set { SetValue(StatusProperty, value); } }

        
        private static List<List<ScatterPoint>> sLiveSamples = new List<List<ScatterPoint>>();
        
        public static readonly DependencyProperty LiveSamplesProperty =
                        DependencyProperty.Register("LiveSamples", typeof(List<List<ScatterPoint>>), typeof(LiveViewPanel),
                        new PropertyMetadata(sLiveSamples));

        internal List<List<ScatterPoint>> LiveSamples { get { return (List<List<ScatterPoint>>)GetValue(LiveSamplesProperty); } set { SetValue(LiveSamplesProperty, value); } }

        public static readonly DependencyProperty TestResultsProperty =
                        DependencyProperty.Register("TestResults", typeof(List<Model.DataPoint>), typeof(LiveViewPanel),
                        new PropertyMetadata(null, (o, arg) => { ((LiveViewPanel)o).onTestResultsChanged((List<Model.DataPoint>)arg.NewValue); }));

        internal List<Model.DataPoint> TestResults { get { return (List<Model.DataPoint>)GetValue(TestResultsProperty); } set { SetValue(TestResultsProperty, value); } }


        public static readonly DependencyProperty ControlPointsProperty =
                        DependencyProperty.Register("ControlPoints", typeof(List<Model.DataPoint>), typeof(LiveViewPanel),
                        new PropertyMetadata(null));

        internal List<Model.DataPoint> ControlPoints { get { return (List<Model.DataPoint>)GetValue(ControlPointsProperty); } set { SetValue(ControlPointsProperty, value); } }
 

        private DispatcherTimer updateTimer;
        private static int lastSampleTimestamp = Environment.TickCount;
        private static int liveSampleSlot = 0;
        private static double activeTimeOffset = 0.0;
        private static double nextTimeOffset = 0.0;
        private static Model.DataPoint lastLiveSample = new Model.DataPoint(-100.0, 0.0);


        static LiveViewPanel()
        {
            for (int i = 0; i < 10; i++)
                sLiveSamples.Add(new List<ScatterPoint>());
        }


        /// <summary>
        /// 
        /// </summary>
        public LiveViewPanel()
        {
            InitializeComponent();

            this.updateTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(70), DispatcherPriority.DataBind, onTimer, this.Dispatcher);
            this.updateTimer.IsEnabled = false;

        }


        /// <summary>
        /// 
        /// </summary>
        private void onNewSample(Model.DataPoint sample)
        {
            if (sample == null)
                return;
                
            LiveViewPanel.lastSampleTimestamp = Environment.TickCount;

            if (!this.updateTimer.IsEnabled)
                this.updateTimer.Start();

            if (LiveViewPanel.lastLiveSample.Time > sample.Time)
                newLiveSequence();

            addLiveSample(sample);
        }


        /// <summary>
        /// 
        /// </summary>
        public void addLiveSample(Model.DataPoint sample)
        {
            double timeOffset = sample.Time + LiveViewPanel.activeTimeOffset;
            LiveViewPanel.nextTimeOffset = timeOffset;


            if (sample.Time - LiveViewPanel.lastLiveSample.Time > 100 || sample.Value - LiveViewPanel.lastLiveSample.Value > 2.5 || sample.Value - LiveViewPanel.lastLiveSample.Value < -2.5)
            {
                this.LiveSamples[LiveViewPanel.liveSampleSlot].Add(new ScatterPoint(timeOffset, sample.Value));

                LiveViewPanel.lastLiveSample = sample;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void newLiveSequence()
        {
            LiveViewPanel.liveSampleSlot = (LiveViewPanel.liveSampleSlot + 1) % this.LiveSamples.Count;
            this.LiveSamples[LiveViewPanel.liveSampleSlot].Clear();

            LiveViewPanel.activeTimeOffset = LiveViewPanel.nextTimeOffset + 2000 - (LiveViewPanel.nextTimeOffset % 1000);
            LiveViewPanel.lastLiveSample = new Model.DataPoint(-100.0, 0.0);

            this.ControlPoints = null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void onTestResultsChanged(List<Model.DataPoint> list)
        {
            if (list == null)
                return;

            SampleAnalyzer sa = new SampleAnalyzer(list, 1, 1);

            this.ControlPoints = new List<Model.DataPoint>() { sa.StartPoint, sa.EndPoint };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onTimer(object sender, EventArgs e)
        {
            if (Environment.TickCount - LiveViewPanel.lastSampleTimestamp > 1500)
                this.updateTimer.Stop();

            OxyPlot.Axes.Axis xAxis = this.plot.Axes[0].InternalAxis;

            double d = xAxis.DataMaximum;

            if (d > xAxis.ActualMinimum + (xAxis.ActualMaximum - xAxis.ActualMinimum) * 19 / 20)
            {
                double delta = d - ((xAxis.ActualMinimum + (xAxis.ActualMaximum - xAxis.ActualMinimum) * 18 / 20));
                double panStep = xAxis.Transform(-delta + xAxis.Offset);

                xAxis.Pan(panStep);
            }

            this.plot.InvalidatePlot();
        }
    }
}
