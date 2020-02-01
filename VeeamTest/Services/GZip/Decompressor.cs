using System;
using System.IO;
using System.IO.Compression;
using VeeamTest.Models;
using VeeamTest.Services.GZip.Core;

namespace VeeamTest.Services.GZip
{
    public class Decompressor : IGZip
    {
        public void Process(Block block)
        {
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(block.Data, 0, block.Data.Length);
                memoryStream.Position = 0;
                
                using (var compressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    var blockLength = BitConverter.ToInt32(block.Data, block.Data.Length - 4);
                    var buffer = new byte[blockLength];
                    compressStream.Read(buffer, 0, buffer.Length);
                    block.Data = buffer;
                }
            }
        }
    }
}