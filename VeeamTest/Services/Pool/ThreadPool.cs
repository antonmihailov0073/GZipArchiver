using System;
using VeeamTest.Collections;
using VeeamTest.Services.Pool.Core;

namespace VeeamTest.Services.Pool
{
    public class ThreadPool : IDisposable
    {
        private readonly BlockingQueue<IThreadPoolWork> _worksQueue = new BlockingQueue<IThreadPoolWork>();
        
        private bool _isDisposed;
        

        public ThreadPool(int threadsCount)
        {
            RunWorkers(threadsCount);
        }

        
        public IWaitable ScheduleWork(Action action)
        {
            CheckDisposed();
            
            var work = new ThreadPoolWork(action);
            _worksQueue.Enqueue(work);
            return work;
        }

        public IWaitable<TResult> ScheduleWork<TResult>(Func<TResult> function)
        {
            CheckDisposed();
            
            var work = new ThreadPoolWork<TResult>(function);
            _worksQueue.Enqueue(work);
            return work;
        }
        
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            _worksQueue.Dispose();
            
            _isDisposed = true;
        }
        

        private void RunWorkers(int threadsCount)
        {
            for (var i = 0; i < threadsCount; ++i)
            {
                var worker = new ThreadPoolWorker(_worksQueue);
                worker.Run();
            }
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