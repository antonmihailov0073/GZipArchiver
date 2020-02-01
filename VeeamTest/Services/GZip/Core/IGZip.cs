using VeeamTest.Models;

namespace VeeamTest.Services.GZip.Core
{
    public interface IGZip
    {
        void Process(Block block);
    }
}