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
    public class DecompressFactory : IArchiverFactory
    {
        private readonly ArchiverSettings _settings;

        
        public DecompressFactory(ArchiverSettings settings)
        {
            _settings = settings;
        }


        public IFileReader CreateReader()
        {
            return new CompressedBlockReader(_settings.Path);
        }

        public IGZip CreateGZip()
        {
            return new Decompressor();
        }

        public IFileWriter CreateWriter()
        {
            return new BlockWriter(_settings.DestinationPath);
        }
    }
}