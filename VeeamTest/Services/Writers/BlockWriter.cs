using VeeamTest.Models;
using VeeamTest.Services.Writers.Core;

namespace VeeamTest.Services.Writers
{
    public class BlockWriter : AFileWriter
    {
        public BlockWriter(string path)
            : base(path)
        {
        }


        protected override void WriteNextInternal(Block block)
        {
            File.Write(block.Data, 0, block.Data.Length);
        }
    }
}