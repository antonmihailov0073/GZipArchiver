using System;
using System.Threading;

namespace VeeamTest.Services.Pool.Core
{
    public abstract class AThreadPoolWork : IThreadPoolWork, IWaitable
    {
        private readonly ManualResetEvent _completeEvent = new ManualResetEvent(false);

        private bool _isDisposed;
        private Exception _exception;


        public void Execute()
        {
            CheckDisposed();
            
            _completeEvent.Reset();
            try
            {
                ExecuteInternal();
            }
            catch (Exception exception)
            {
                _exception = exception;
            }
            finally
            {
                _completeEvent.Set();
            }
        }
        
        public void Wait()
        {
            CheckDisposed();
            
            _completeEvent.WaitOne();
            if (_exception != null)
            {
                throw _exception;
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            _completeEvent.Dispose();

            _isDisposed = true;
        }

        
        protected abstract void ExecuteInternal();


        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}