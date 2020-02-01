using VeeamTest.Services.GZip.Core;
using VeeamTest.Services.Readers.Core;
using VeeamTest.Services.Writers.Core;

namespace VeeamTest.Services.Archiver.Factories.Core
{
    public interface IArchiverFactory
    {
        IFileReader CreateReader();

        IGZip CreateGZip();

        IFileWriter CreateWriter();
    }
}