using System;
using VeeamTest.Collections;
using VeeamTest.Models;
using VeeamTest.Services.Archiver.Factories;
using VeeamTest.Services.Archiver.Factories.Core;
using VeeamTest.Services.Pool;
using VeeamTest.Services.Pool.Core;

namespace VeeamTest.Services.Archiver
{
    public class GZipArchiver
    {
        private readonly ArchiverSettings _settings;
        private readonly IArchiverFactory _factory;

        
        public GZipArchiver(ArchiverSettings settings)
        {
            _settings = settings;
            
            if (settings.Mode == ArchiverMode.Decompress)
            {
                _factory = new DecompressFactory(settings);
            }
            else
            {
                _factory = new CompressFactory(settings);
            }
        }


        public void Process()
        {
            var gZip = _factory.CreateGZip();
            using (var threadPool = new ThreadPool(Environment.ProcessorCount))
            using (var reader = _factory.CreateReader())
            using (var writer = _factory.CreateWriter())
            {
                while (reader.CanContinue)
                {
                    using (var waitables = new DisposableList<IWaitable<Block>>())
                    {
                        var readBytes = 0L;
                        while (readBytes < _settings.MemoryLimitation && reader.CanContinue)
                        {
                            var block = reader.ReadNext();
                            readBytes += block.Data.Length;
                            
                            var waitable = threadPool.ScheduleWork(() =>
                            {
                                gZip.Process(block);
                                return block;
                            });
                            waitables.Add(waitable);
                        }

                        foreach (var waitable in waitables)
                        {
                            waitable.Wait();
                        
                            writer.WriteNext(waitable.Result);
                        }
                    }
                }
            }
        }
    }
}