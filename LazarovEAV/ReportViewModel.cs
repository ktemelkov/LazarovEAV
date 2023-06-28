using LazarovEAV.Util;
using LazarovEAV.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class ReportViewModel : DependencyObject, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ActivePatientProperty =
                        DependencyProperty.Register("ActivePatient", typeof(PatientViewModel), typeof(ReportViewModel),
                        new PropertyMetadata(null, (o, args) => { ((ReportViewModel)o).onActivePatientChanged((PatientViewModel)args.OldValue, (PatientViewModel)args.NewValue); }));

        internal PatientViewModel ActivePatient { get { return (PatientViewModel)GetValue(ActivePatientProperty); } set { SetValue(ActivePatientProperty, value); } }



        /// <summary>
        /// 
        /// </summary>
        public ReportViewModel()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            EventUtils.detachEvents(this.ActivePatient, Patient_PropertyChanged);

            if (this.ActivePatient != null)
                SessionUtils.detachSession(this.ActivePatient.CurrentSession, this.CollectionHandlers, this.PropertyHandlers);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void onActivePatientChanged(PatientViewModel oldValue, PatientViewModel newValue)
        {
            if (oldValue != null)
            {
                SessionUtils.detachSession(oldValue.CurrentSession, this.CollectionHandlers, this.PropertyHandlers);
                EventUtils.detachEvents(oldValue, Patient_PropertyChanged);
            }

            if (newValue != null)
            {
                EventUtils.attachEvents(newValue, Patient_PropertyChanged);
                SessionUtils.attachSession(newValue.CurrentSession, this.CollectionHandlers, this.PropertyHandlers);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Patient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Properties:
            //--------------------
            // CurrentSession

            if (e.PropertyName == "CurrentSession")
            {
                PropertyChangedEventArgs2 args = (PropertyChangedEventArgs2)e;

                SessionUtils.detachSession((PatientSessionViewModel)args.OldValue, this.CollectionHandlers, this.PropertyHandlers);
                SessionUtils.attachSession((PatientSessionViewModel)args.NewValue, this.CollectionHandlers, this.PropertyHandlers);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private NotifyCollectionChangedEventHandler[] CollectionHandlers
        {
            get
            {
                return new NotifyCollectionChangedEventHandler[] { TestResultsLeft_CollectionChanged, TestResultsRight_CollectionChanged };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private PropertyChangedEventHandler[] PropertyHandlers
        {
            get
            {
                return new PropertyChangedEventHandler[] { TestResultViewModel_Left_PropertyChanged, TestResultViewModel_Right_PropertyChanged };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TestResultsLeft_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            handleResultsCollectionChanged(sender, e, TestResultViewModel_Left_PropertyChanged);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TestResultsRight_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            handleResultsCollectionChanged(sender, e, TestResultViewModel_Right_PropertyChanged);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="handler"></param>
        private void handleResultsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, PropertyChangedEventHandler handler)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                SessionUtils.attachItems(e.NewItems, handler);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove
                        || e.Action == NotifyCollectionChangedAction.Reset)
            {
                SessionUtils.detachItems(e.OldItems, handler);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TestResultViewModel_Left_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Properties:
            // -------------------
            // ResultData
            // ControlPoints
            // HasDeviation
            // ResultValue
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TestResultViewModel_Right_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Properties:
            // -------------------
            // ResultData
            // ControlPoints
            // HasDeviation
            // ResultValue
        }

    }
}
