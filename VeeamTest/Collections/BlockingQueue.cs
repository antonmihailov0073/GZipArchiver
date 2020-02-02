using System;
using System.Threading;
using VeeamTest.Helpers.Strings;

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
                // trying to wait handle immediately
                var waitSuccessful = _dequeueSemaphore.Wait(0);
                if (!waitSuccessful)
                {
                    // when immediately wait failed, go for infinite with cancellation token to throw when enqueuing was completed
                    _dequeueSemaphore.Wait(Timeout.Infinite, _dequeueCancellationSource.Token);
                }
            }
            catch (OperationCanceledException)
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
            return new InvalidOperationException(StringsHelper.EnqueuingCompleted());
        }
    }
}