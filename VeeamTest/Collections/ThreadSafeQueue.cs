using System;
using System.Collections.Generic;
using VeeamTest.Threading;

namespace VeeamTest.Collections
{
    public class ThreadSafeQueue<TElement>
    {
        private readonly Queue<TElement> _internalQueue = new Queue<TElement>();
        private readonly SpinBlock _sync = new SpinBlock();


        public int Count => _sync.GetWithSync(() => _internalQueue.Count);
        
        
        public void Enqueue(TElement element)
        {
            _sync.WithSync(() => _internalQueue.Enqueue(element));
        }

        public TElement Dequeue()
        {
            return _sync.GetWithSync(() => _internalQueue.Dequeue());
        }
    }
}