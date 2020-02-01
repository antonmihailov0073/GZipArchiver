using System;
using VeeamTest.Services.Pool.Core;

namespace VeeamTest.Services.Pool
{
    public class ThreadPoolWork : AThreadPoolWork
    {
        private readonly Action _action;


        public ThreadPoolWork(Action action)
        {
            _action = action;
        }
        
        
        protected override void ExecuteInternal()
        {
            _action.Invoke();
        }
    }

    public class ThreadPoolWork<TResult> : AThreadPoolWork, IWaitable<TResult>
    {
        private readonly Func<TResult> _function;

        
        public ThreadPoolWork(Func<TResult> function)
        {
            _function = function;
        }

        
        public TResult Result { get; private set; }
        

        protected override void ExecuteInternal()
        {
            Result = _function.Invoke();
        }
    }
}