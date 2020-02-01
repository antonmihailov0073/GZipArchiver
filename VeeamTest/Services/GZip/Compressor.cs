using System.IO;
using System.IO.Compression;
using VeeamTest.Models;
using VeeamTest.Services.GZip.Core;

namespace VeeamTest.Services.GZip
{
    public class Compressor : IGZip
    {
        public void Process(Block block)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var compressStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    compressStream.Write(block.Data, 0, block.Data.Length);
                }

                block.Data = memoryStream.ToArray();
            }
        }
    }
}