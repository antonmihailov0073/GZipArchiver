using System;
using System.Threading;

namespace VeeamTest.Collections
{
    public class BlockingQueue<TElement> : IDisposable
    {
        private readonly ThreadSafeQueue<TElement> _internalQueue = new ThreadSafeQueue<TElement>();
        private readonly SemaphoreSlim _dequeueSemaphore = new SemaphoreSlim(0);
        private readonly CancellationTokenSource _dequeueCancellationSource = new CancellationTokenSource();

        private bool _isDisposed;


        public bool IsEnqueuingCompleted
        {
            get
            {
                CheckDisposed();
                
                return _dequeueCancellationSource.IsCancellationRequested;
            }
        }

        public bool IsCompleted
        {
            get
            {
                CheckDisposed();
                
                return IsEnqueuingCompleted && _dequeueSemaphore.CurrentCount == 0;
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
                throw EnqueuingCompletedException();
            }
            
            try
            {
                _dequeueSemaphore.Wait(_dequeueCancellationSource.Token);
            }
            catch (OperationCanceledException) // it's mean that enqueuing was completed while we were waiting
            {
                throw EnqueuingCompletedException();
            }
            
            return _internalQueue.Dequeue();
        }

        public void CompleteEnqueuing()
        {
            CheckDisposed();
            
            _dequeueCancellationSource.Cancel();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            _dequeueSemaphore.Dispose();
            _dequeueCancellationSource.Dispose();
            
            _isDisposed = true;
        }


        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        
        private static Exception EnqueuingCompletedException()
        {
            return new InvalidOperationException("Enqueuing was completed");
        }
    }
}