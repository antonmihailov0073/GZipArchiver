using VeeamTest.Models;
using VeeamTest.Services.Readers.Core;

namespace VeeamTest.Services.Readers
{
    public class BlockReader : AFileReader
    {
        private readonly int _blockSize;
        
        
        public BlockReader(string path, int blockSize)
            : base(path)
        {      
            _blockSize = blockSize;
        }
        
        
        protected override Block ReadNextInternal()
        {
            var bufferSize = CalculateBufferSize();
            var buffer = new byte[bufferSize];
            
            File.Read(buffer, 0, buffer.Length);
            
            return new Block(buffer);
        }
        
        
        private int CalculateBufferSize()
        {
            return File.Position + _blockSize <= File.Length
                ? _blockSize
                : (int) (File.Length - File.Position);
        }
    }
}