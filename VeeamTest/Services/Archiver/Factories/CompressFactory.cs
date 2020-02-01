using VeeamTest.Models;
using VeeamTest.Services.Archiver.Factories.Core;
using VeeamTest.Services.GZip;
using VeeamTest.Services.GZip.Core;
using VeeamTest.Services.Readers;
using VeeamTest.Services.Readers.Core;
using VeeamTest.Services.Writers;
using VeeamTest.Services.Writers.Core;

namespace VeeamTest.Services.Archiver.Factories
{
    public class CompressFactory : IArchiverFactory
    {
        private readonly ArchiverSettings _settings;

        
        public CompressFactory(ArchiverSettings settings)
        {
            _settings = settings;
        }


        public IFileReader CreateReader()
        {
            return new BlockReader(_settings.Path, _settings.BlockSize);
        }

        public IGZip CreateGZip()
        {
            return new Compressor();
        }

        public IFileWriter CreateWriter()
        {
            return new CompressedBlockWriter(_settings.DestinationPath);
        }
    }
}