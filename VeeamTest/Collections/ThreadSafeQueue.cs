using System;
using System.Collections.Generic;
using VeeamTest.Threading;

namespace VeeamTest.Collections
{
    public class ThreadSafeQueue<TElement>
    {
        private readonly Queue<TElement> _internalQueue = new Queue<TElement>();
        private readonly SpinBlock _queueSync = new SpinBlock();

        
        public void Enqueue(TElement element)
        {
            WithSync(() => _internalQueue.Enqueue(element));
        }

        public TElement Dequeue()
        {
            return GetWithSync(() => _internalQueue.Dequeue());
        }
        

        private void WithSync(Action action)
        {
            _queueSync.Enter();

            try
            {
                action.Invoke();
            }
            finally
            {
                _queueSync.Exit();
            }
        }

        private TResult GetWithSync<TResult>(Func<TResult> getFunction)
        {
            var result = default(TResult);
            WithSync(() => result = getFunction.Invoke());
            return result; 
        }
    }
}