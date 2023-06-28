using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Util
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableHashSet<T> : ICollection<T>, INotifyCollectionChanged
    {
        private HashSet<T> _innerCollection;

        public event NotifyCollectionChangedEventHandler CollectionChanged;


        /// <summary>
        /// 
        /// </summary>
        public ObservableHashSet()
        {
            this._innerCollection = new HashSet<T>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public ObservableHashSet(IEnumerable<T> collection)
        {
            this._innerCollection = new HashSet<T>(collection);
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
            HashSet<T> oldCollection = this._innerCollection;
            this._innerCollection = new HashSet<T>();

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
            bool res = _innerCollection.Remove(item);

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
