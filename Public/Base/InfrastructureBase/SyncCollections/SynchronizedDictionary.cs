using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace InfrastructureBase.SyncCollections
{
    /// <summary>
    /// Dictionary that uses ReaderWriterLockSlim to syncronize all read and writes to the underlying Dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class SynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {

        Dictionary<TKey, TValue> _dictionary;

        [NonSerialized]
        ReaderWriterLockSlim _lock;

        public SynchronizedDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }
        public SynchronizedDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }
        public SynchronizedDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }
        public SynchronizedDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }
        public SynchronizedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }
        public SynchronizedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }
        protected ReaderWriterLockSlim Lock
        {
            get
            {
                if (_lock == null)
                {
                    Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);
                }
                return _lock;
            }
        }
        public void EnterReadLock()
        {
            Lock.EnterReadLock();
        }
        public void ExitReadLock()
        {
            Lock.ExitReadLock();
        }
        public void EnterWriteLock()
        {
            Lock.EnterWriteLock();
        }
        public void ExitWriteLock()
        {
            Lock.ExitWriteLock();
        }
        public void EnterUpgradeableReadLock()
        {
            Lock.EnterUpgradeableReadLock();
        }
        public void ExitUpgradeableReadLock()
        {
            Lock.ExitUpgradeableReadLock();
        }
        protected Dictionary<TKey, TValue> Dictionary
        {
            get
            {
                return _dictionary;
            }
        }
        public TValue GetAdd(TKey key, Func<TValue> addfunction)
        {

            if (addfunction == null)
                throw new ArgumentNullException("Func<TValue> addfunction");

            try
            {
                EnterUpgradeableReadLock();

                if (!_dictionary.ContainsKey(key))
                {
                    try
                    {
                        EnterWriteLock();

                        TValue value = addfunction();

                        _dictionary.Add(key, value);
                        return value;
                    }
                    finally
                    {
                        ExitWriteLock();
                    }
                }
                else
                {
                    return _dictionary[key];
                }
            }
            finally
            {
                ExitUpgradeableReadLock();
            }
        }
        public void Add(TKey key, TValue value)
        {
            try
            {
                EnterWriteLock();
                _dictionary.Add(key, value);
            }
            finally
            {
                ExitWriteLock();
            }
        }
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            try
            {
                EnterWriteLock();
                _dictionary.Add(item.Key, item.Value);
            }
            finally
            {
                ExitWriteLock();
            }
        }
        public bool Add(TKey key, TValue value, bool throwOnNotFound)
        {
            try
            {
                EnterUpgradeableReadLock();
                if (!_dictionary.ContainsKey(key))
                {
                    try
                    {
                        EnterWriteLock();
                        _dictionary.Add(key, value);
                        return true;
                    }
                    finally
                    {
                        ExitWriteLock();
                    }
                }
                else
                {
                    if (throwOnNotFound)
                        throw new ArgumentNullException();
                    else
                        return false;
                }
            }
            finally
            {
                ExitUpgradeableReadLock();
            }
        }
        public bool ContainsKey(TKey key)
        {
            try
            {
                EnterReadLock();
                return _dictionary.ContainsKey(key);
            }
            finally
            {
                ExitReadLock();
            }
        }
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            try
            {
                EnterReadLock();
                return _dictionary.Contains(item);
            }
            finally
            {
                ExitReadLock();
            }
        }
        public TKey[] KeysToArray()
        {
            try
            {
                EnterReadLock();
                return _dictionary.Keys.ToArray();
            }
            finally
            {
                ExitReadLock();
            }
        }
        public ICollection<TKey> Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }
        public bool Remove(TKey key)
        {
            try
            {
                EnterWriteLock();
                return _dictionary.Remove(key);
            }
            finally
            {
                ExitWriteLock();
            }
        }
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            try
            {
                EnterWriteLock();
                return _dictionary.Remove(item.Key);
            }
            finally
            {
                ExitWriteLock();
            }
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            try
            {
                EnterReadLock();
                return _dictionary.TryGetValue(key, out value);
            }
            finally
            {
                ExitReadLock();
            }
        }
        public TValue[] ValuesToArray()
        {
            try
            {
                EnterReadLock();
                return _dictionary.Values.ToArray();
            }
            finally
            {
                ExitReadLock();
            }
        }
        public ICollection<TValue> Values
        {
            get
            {
                return _dictionary.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                try
                {
                    EnterReadLock();
                    return _dictionary[key];
                }
                finally
                {
                    ExitReadLock();
                }
            }
            set
            {
                try
                {
                    EnterWriteLock();
                    _dictionary[key] = value;
                }
                finally
                {
                    ExitWriteLock();
                }
            }
        }
        public void Clear()
        {
            try
            {
                EnterWriteLock();
                _dictionary.Clear();
            }
            finally
            {
                ExitWriteLock();
            }
        }
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            try
            {
                EnterReadLock();

                for (int i = 0; i < _dictionary.Count; i++)
                {
                    array.SetValue(_dictionary.ElementAt(i), arrayIndex);
                }
            }
            finally
            {
                ExitReadLock();
            }
        }
        public int Count
        {
            get
            {
                try
                {
                    EnterReadLock();
                    return _dictionary.Count;
                }
                finally
                {
                    ExitReadLock();
                }
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            EnterReadLock();
            try
            {
                return _dictionary.GetEnumerator();
            }
            finally
            {
                ExitReadLock();
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected bool IsDisposed { get; set; }
        public virtual void Dispose()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().Name);
            try
            {
                this.Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (disposing)
                    {
                        if (_lock != null)
                        {
                            _lock.Dispose();
                            _lock = null;
                        }
                    }
                }
            }
            finally
            {
                this.IsDisposed = true;
            }
        }
        //Only add Finalizer in you need to dispose of resources with out call Dispose() directly
        ~SynchronizedDictionary()
        {
            Dispose(!IsDisposed);
        }
    }
}
