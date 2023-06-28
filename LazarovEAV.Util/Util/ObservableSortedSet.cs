using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LazarovEAV.Util
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableSortedSet<T> : ICollection<T>, INotifyCollectionChanged
    {
        private SortedSet<T> _innerCollection;

        public event NotifyCollectionChangedEventHandler CollectionChanged;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public ObservableSortedSet(IComparer<T> comparer)
        {
            this._innerCollection = new SortedSet<T>(comparer);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comparer"></param>
        public ObservableSortedSet(IEnumerable<T> collection, IComparer<T> comparer)
        {
            this._innerCollection = new SortedSet<T>(collection, comparer);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this._innerCollection.GetEnumerator();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            this._innerCollection.Add(item);

            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }


        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            SortedSet<T> oldCollection = this._innerCollection;
            this._innerCollection = new SortedSet<T>();

            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, oldCollection.ToList()));

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return this._innerCollection.Contains(item);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this._innerCollection.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            bool res = this._innerCollection.Remove(item);

            if (this.CollectionChanged != null)
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return this._innerCollection.Count; }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return ((ICollection<T>)this._innerCollection).IsReadOnly; }
        }
    }
}
