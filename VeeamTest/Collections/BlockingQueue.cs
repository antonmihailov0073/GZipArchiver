using System;
using System.Threading;
using VeeamTest.Helpers.Strings;
using VeeamTest.Threading;

namespace VeeamTest.Collections
{
    public class BlockingQueue<TElement> : IDisposable
    {
        private readonly ThreadSafeQueue<TElement> _internalQueue = new ThreadSafeQueue<TElement>();
        private readonly SemaphoreSlim _dequeueSemaphore = new SemaphoreSlim(0);
        private readonly SpinBlock _enqueuingCompletedSync = new SpinBlock();

        private bool _isDisposed;
        private bool _isEnqueuingCompleted;


        public bool IsCompleted
        {
            get
            {
                CheckDisposed();
                
                return _enqueuingCompletedSync.GetWithSync(() => _isEnqueuingCompleted) 
                       && _internalQueue.Count == 0;;
            }
        }
        

        public void Enqueue(TElement element)
        {
            CheckDisposed();

            _internalQueue.Enqueue(element);
            _dequeueSemaphore.Release();
        }

        public TElement Dequeue()
        {
            CheckDisposed();

            if (IsCompleted)
            {
                throw new InvalidOperationException(StringsHelper.EnqueuingCompleted());
            }
            
            _dequeueSemaphore.Wait();
            return _internalQueue.Dequeue();
        }

        public void CompleteEnqueuing()
        {
            CheckDisposed();
            
            _enqueuingCompletedSync.WithSync(() => _isEnqueuingCompleted = true);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            _dequeueSemaphore.Dispose();
            
            _isDisposed = true;
        }


        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}