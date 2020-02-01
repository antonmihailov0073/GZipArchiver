using System.Threading;
using VeeamTest.Collections;
using VeeamTest.Services.Pool.Core;

namespace VeeamTest.Services.Pool
{
    public class ThreadPoolWorker
    {
        private readonly BlockingQueue<IThreadPoolWork> _worksQueue;
        private readonly Thread _internalThread;
            

        public ThreadPoolWorker(BlockingQueue<IThreadPoolWork> worksQueue)
        {
            _worksQueue = worksQueue;
            _internalThread = CreateThread();
        }


        public void Run()
        {
            _internalThread.Start();
        }
            
            
        private void Process()
        {
            while (true)
            {
                IThreadPoolWork work;
                try
                {
                    work = _worksQueue.Dequeue();
                }
                catch
                {
                    break;
                }

                work.Execute();
            }
        }

        private Thread CreateThread()
        {
            return new Thread(Process)
            {
                IsBackground = true
            };
        }
    }
}