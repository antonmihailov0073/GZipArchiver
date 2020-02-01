using System;
using System.Threading;

namespace VeeamTest.Collections
{
    public class BlockingQueue<TElement> : IDisposable
    {
        private readonly ThreadSafeQueue<TElement> _internalQueue = new ThreadSafeQueue<TElement>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);

        private bool _isDisposed;
        

        public void Enqueue(TElement element)
        {
            CheckDisposed();
            
            _internalQueue.Enqueue(element);
            _semaphore.Release();
        }

        public TElement Dequeue()
        {
            CheckDisposed();
            
            _semaphore.Wait();
            return _internalQueue.Dequeue();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            _semaphore.Dispose();
            
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