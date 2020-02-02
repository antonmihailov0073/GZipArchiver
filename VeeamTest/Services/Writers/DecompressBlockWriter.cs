using System;
using VeeamTest.Models;
using VeeamTest.Services.Writers.Core;

namespace VeeamTest.Services.Writers
{
    public class CompressedBlockWriter : AFileWriter
    {
        private const int HeaderLength = 10;
        
        
        public CompressedBlockWriter(string path)
            : base(path)
        {
        }

        
        protected override void WriteNextInternal(Block block)
        {
            // magic numbers and compression mode
            File.Write(block.Data, 0, 3);
                
            // extra field bit
            File.Write(new[] { (byte) GZipFlags.ExtraFieldPresent });
                
            // other required bits
            File.Write(block.Data, 3, 6);

            var blockLengthWithoutHeader = block.Data.Length - HeaderLength;
            var blockLengthBytes = BitConverter.GetBytes(blockLengthWithoutHeader);
            
            // extra field length
            File.Write(new byte[] { sizeof(int), 0 });
                
            // extra field with block size inside
            File.Write(blockLengthBytes);
                
            // compressed data
            File.Write(block.Data, HeaderLength, blockLengthWithoutHeader);
        }
    }
}