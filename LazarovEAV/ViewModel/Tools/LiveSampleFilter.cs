using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace LazarovEAV.ViewModel
{
    delegate void LiveSampleCallback(double sample);

    /// <summary>
    /// 
    /// </summary>
    class LiveSampleFilter
    {
        public event LiveSampleCallback OnLiveSample;

        private int queueSize = 30;
        private int timerInterval = 10;
        private int fadeoutTime = 2000;
        private List<double> sampleQueue = new List<double>();
        private DispatcherTimer queueTimer;

        private int lastSampleTimestamp = 0;


        /// <summary>
        /// 
        /// </summary>
        public LiveSampleFilter()
        {
            this.queueTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(this.timerInterval), DispatcherPriority.Normal, onQueueTimer, Application.Current.Dispatcher);
        }


        /// <summary>
        /// 
        /// </summary>
        public void reset()
        {
            this.lastSampleTimestamp = Environment.TickCount;
            this.sampleQueue.Clear();

            for (int i = 0; i < this.queueSize; i++)
                this.sampleQueue.Add(0.0);

            if (!this.queueTimer.IsEnabled)
                this.queueTimer.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        public void feed(double sample)
        {
            calculateSample(sample);

            this.lastSampleTimestamp = Environment.TickCount;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        private double calculateSample(double sample)
        {
            if (this.sampleQueue.Count <= 0)
                return 0.0;

            while (this.sampleQueue.Count >= this.queueSize)
                this.sampleQueue.RemoveAt(0);

            this.sampleQueue.Add(sample);

            double sum = 0.0;

            for (int i = 0; i < this.sampleQueue.Count; i++)
            {
                sum += this.sampleQueue[i];
            }

            return sum / this.sampleQueue.Count;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onQueueTimer(object sender, EventArgs e)
        {
            if (Environment.TickCount - this.lastSampleTimestamp > this.fadeoutTime)
                this.queueTimer.Stop();

            double sample = 0.0;

            if (Environment.TickCount - this.lastSampleTimestamp < 100 && this.sampleQueue.Count > 0)
                sample = this.sampleQueue[this.sampleQueue.Count - 1];

            sample = calculateSample(sample);
            fireNewSample(sample);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        private void fireNewSample(double sample)
        {
            if (this.OnLiveSample != null)
                this.OnLiveSample(sample);
        }
    }
}
