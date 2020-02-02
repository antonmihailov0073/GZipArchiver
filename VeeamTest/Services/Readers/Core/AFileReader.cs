using System;
using System.IO;
using VeeamTest.Models;

namespace VeeamTest.Services.Readers.Core
{
    public abstract class AFileReader : IFileReader
    {
        private bool _isDisposed;
        
        
        protected AFileReader(string path)
        {
            File = CreateStream(path);
        }
        
        
        protected FileStream File { get; }
        
        
        public bool CanContinue
        {
            get
            {
                CheckDisposed();
                
                return File.Position != File.Length;
            }
        }


        public Block ReadNext()
        {
            CheckDisposed();
            CheckContinue();
            
            return ReadNextInternal();
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


        protected abstract Block ReadNextInternal();
        

        private void CheckContinue()
        {
            if (!CanContinue)
            {
                throw new InvalidOperationException();
            }
        }
        
        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        
        
        private static FileStream CreateStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}