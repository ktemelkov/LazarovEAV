using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel.Util
{
    /// <summary>
    /// 
    /// </summary>
    static class SessionUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public static void attachSession(PatientSessionViewModel session, NotifyCollectionChangedEventHandler[] collectionHandlers, PropertyChangedEventHandler[] propertyHandlers)
        {
            if (session == null)
                return;

            if (session.ResultsLeft != null)
            {
                session.ResultsLeft.CollectionChanged += collectionHandlers[0];
                attachItems(session.ResultsLeft.ToList(), propertyHandlers[0]);
            }

            if (session.ResultsRight != null)
            {
                session.ResultsRight.CollectionChanged += collectionHandlers[1];
                attachItems(session.ResultsRight.ToList(), propertyHandlers[1]);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public static void detachSession(PatientSessionViewModel session, NotifyCollectionChangedEventHandler[] collectionHandlers, PropertyChangedEventHandler[] propertyHandlers)
        {
            if (session == null)
                return;

            if (session.ResultsLeft != null)
            {
                session.ResultsLeft.CollectionChanged -= collectionHandlers[0];
                detachItems(session.ResultsLeft.ToList(), propertyHandlers[0]);
            }

            if (session.ResultsRight != null)
            {
                session.ResultsRight.CollectionChanged -= collectionHandlers[1];
                detachItems(session.ResultsRight.ToList(), propertyHandlers[1]);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="handler"></param>
        public static void attachItems(IList list, PropertyChangedEventHandler handler)
        {
            foreach (var item in list)
                ((INotifyPropertyChanged)item).PropertyChanged += handler;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="handler"></param>
        public static void detachItems(IList list, PropertyChangedEventHandler handler)
        {
            foreach (var item in list)
                ((INotifyPropertyChanged)item).PropertyChanged -= handler;
        }

    }
}
