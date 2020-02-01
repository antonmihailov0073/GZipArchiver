using System;

namespace VeeamTest.Services.Pool.Core
{
    public interface IWaitable : IDisposable
    {
        void Wait();
    }

    public interface IWaitable<TResult> : IWaitable
    {
        TResult Result { get; }
    }
}