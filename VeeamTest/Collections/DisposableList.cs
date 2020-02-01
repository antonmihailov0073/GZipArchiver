using System;
using System.Collections;
using System.Collections.Generic;

namespace VeeamTest.Collections
{
    public class DisposableList<TElement> : IList<TElement>, IDisposable
        where TElement : IDisposable
    {
        private readonly List<TElement> _internalList;

        private bool _isDisposed;
        
        
        public DisposableList()
        {
            _internalList = new List<TElement>();
        }


        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            foreach (var element in _internalList)
            {
                element.Dispose();
            }
            
            _internalList.Clear();
            _isDisposed = true;
        }
        

        #region IList
        
        public TElement this[int index]
        {
            get
            {
                CheckDisposed();
                return _internalList[index];
            }
            set
            {
                CheckDisposed();
                _internalList[index] = value;
            }
        }

        public int Count
        {
            get
            {
                CheckDisposed();
                return _internalList.Count;
            }
        }

        bool ICollection<TElement>.IsReadOnly
        {
            get
            {
                CheckDisposed();
                return ((ICollection<TElement>)_internalList).IsReadOnly;
            }
        }
        
        
        public IEnumerator<TElement> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        
        public void Add(TElement item)
        {
            CheckDisposed();
            _internalList.Add(item);
        }

        public void Clear()
        {
            CheckDisposed();
            _internalList.Clear();
        }

        public bool Contains(TElement item)
        {
            CheckDisposed();
            return _internalList.Contains(item);
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            CheckDisposed();
            _internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(TElement item)
        {
            CheckDisposed();
            return _internalList.Remove(item);
        }
        
        public int IndexOf(TElement item)
        {
            CheckDisposed();
            return _internalList.IndexOf(item);
        }

        public void Insert(int index, TElement item)
        {
            CheckDisposed();
            _internalList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            CheckDisposed();
            _internalList.RemoveAt(index);
        }

        #endregion


        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}