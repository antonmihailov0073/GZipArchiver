using System;
using System.IO;
using VeeamTest.Models;

namespace VeeamTest.Services.Writers.Core
{
    public abstract class AFileWriter : IFileWriter
    {
        private bool _isDisposed;


        protected AFileWriter(string path)
        {
            File = CreateStream(path);
        }
            
            
        protected FileStream File { get;}


        public void WriteNext(Block block)
        {
            CheckDisposed();
            
            WriteNextInternal(block);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            File.Dispose();

            _isDisposed = true;
        }


        protected abstract void WriteNextInternal(Block block);
            
        
        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        

        private static FileStream CreateStream(string path)
        {
            return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
        }
    }
}