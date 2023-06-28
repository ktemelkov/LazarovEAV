using LazarovEAV.Device;
using LazarovEAV.Model;
using LazarovEAV.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class EavDeviceViewModel : NotifyPropertyChangedImpl, IDisposable
    {
        private ProtocolAdapter protocolAdapter;
        private LiveSampleFilter sampleFilter;

        private DataPoint liveSample = new DataPoint(0.0, 0.0);
        public DataPoint LiveSample { get { return this.liveSample; } set { Set(ref this.liveSample, value, "LiveSample"); } }

        private double filteredSample = 0.0;
        public double FilteredSample { get { return this.filteredSample; } set { Set(ref this.filteredSample, value, "FilteredSample"); } }

        private ObservableCollection<DataPoint> liveGraph;
        public ObservableCollection<DataPoint> LiveGraph { get { return this.liveGraph; } set { Set(ref this.liveGraph, value, "LiveGraph"); } }

        private List<DataPoint> testResults;
        public List<DataPoint> TestResults { get { return this.testResults; } set { this.testResults = value; RaisePropertyChanged("TestResults"); } }

        private DispatcherTimer liveSequenceWatchdog;

        private ICommand startTestSequenceCommand;
        public ICommand StartTestSequenceCommand { get { return this.startTestSequenceCommand;  } }

        private DispatcherTimer testSequenceTimer = null;


        /// <summary>
        /// 
        /// </summary>
        public EavDeviceViewModel(ProtocolAdapter deviceAdapter)
        {
            this.protocolAdapter = deviceAdapter;

            this.protocolAdapter.StartSequence += onStartSequence;
            this.protocolAdapter.EndSequence += onEndSequence;
            this.protocolAdapter.NewSample += onNewSample;
            this.protocolAdapter.HardwareVersion += onHardwareVersion;
            this.protocolAdapter.BatteryLevel += onBatteryLevel;

            this.sampleFilter = new LiveSampleFilter();
            this.sampleFilter.OnLiveSample += onLiveSample;

            this.liveSequenceWatchdog = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Normal, onWatchdogTimer, Dispatcher.CurrentDispatcher);
            this.liveSequenceWatchdog.IsEnabled = false;

            this.startTestSequenceCommand = new CommandDelegate(new Action<object>(startTestSequence));
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.liveSequenceWatchdog.Stop();

            this.protocolAdapter.StartSequence -= onStartSequence;
            this.protocolAdapter.EndSequence -= onEndSequence;
            this.protocolAdapter.NewSample -= onNewSample;
            this.protocolAdapter.HardwareVersion -= onHardwareVersion;
            this.protocolAdapter.BatteryLevel -= onBatteryLevel;

            this.sampleFilter.OnLiveSample -= onLiveSample;
        }


        /// <summary>
        /// 
        /// </summary>
        private void onStartSequence()
        {
            this.sampleFilter.reset();
            this.LiveGraph = new ObservableCollection<DataPoint>();

            this.liveSequenceWatchdog.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        private void onEndSequence()
        {
            this.liveSequenceWatchdog.Stop();

            if (this.LiveGraph.Count > 1 && this.LiveGraph[this.LiveGraph.Count - 1].Time > 700)
            {

                List<DataPoint> data = this.LiveGraph.ToList();

                SampleAnalyzer sa = new SampleAnalyzer(data, 1, 1);

                if (sa.StartPoint != null && sa.EndPoint != null)
                    this.TestResults = data;
            }
            else
            {
                this.TestResults = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        private void onNewSample(double timeOffset, double sample)
        {
            this.sampleFilter.feed(sample);

            DataPoint dataP = new DataPoint(timeOffset, sample);
            
            this.LiveSample = dataP;
            this.liveGraph.Add(dataP);

            this.liveSequenceWatchdog.Stop();
            this.liveSequenceWatchdog.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ver"></param>
        private void onHardwareVersion(string ver)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="volts"></param>
        private void onBatteryLevel(int percent, double volts)
        {
        }


        /// <summary>
        /// Callback that gets the output of the Sample Filter
        /// </summary>
        /// <param name="sample"></param>
        private void onLiveSample(double sample)
        {
            this.FilteredSample = sample;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onWatchdogTimer(object sender, EventArgs e)
        {
            this.protocolAdapter.ResetState();
            onEndSequence();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void startTestSequence(object obj)
        {
            if (obj == null || !(obj is ArrayList))
                throw new ArgumentException();

            if (this.testSequenceTimer != null)
                return;

            int state = 0;
            int index = -1;
            ArrayList data = (ArrayList)obj;

            (this.testSequenceTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(16), DispatcherPriority.Normal, 
                (o, e) => 
                {
                    switch (state)
                    {
                        case 0:
                            this.onStartSequence();
                            state = 1;
                            break;
                        case 1:
                            if (++index < data.Count)
                                this.onNewSample(((DataPoint)data[index]).Time, ((DataPoint)data[index]).Value);
                            else
                                state = 2;
                            break;
                        case 2:
                            this.onEndSequence();
                            this.testSequenceTimer.Stop();
                            this.testSequenceTimer = null;
                            break;
                    }
                 
                }, 
                Dispatcher.CurrentDispatcher)).Start();
        }
    }
}
