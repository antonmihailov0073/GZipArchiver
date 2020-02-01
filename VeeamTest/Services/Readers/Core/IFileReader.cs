using System;
using VeeamTest.Models;

namespace VeeamTest.Services.Readers.Core
{
    public interface IFileReader : IDisposable
    {
        bool CanContinue { get; }
        
        
        Block ReadNext();
    }
}