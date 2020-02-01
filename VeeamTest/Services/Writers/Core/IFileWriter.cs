using System;
using VeeamTest.Models;

namespace VeeamTest.Services.Writers.Core
{
    public interface IFileWriter : IDisposable
    {
        void WriteNext(Block block);
    }
}