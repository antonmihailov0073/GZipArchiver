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
        // it's the optimal thread pool size based on my research
        private static readonly int _threadPoolSize = Environment.ProcessorCount * 2;
        
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
            
            using (var threadPool = new ThreadPool(_threadPoolSize))
            using (var reader = _factory.CreateReader())
            using (var writer = _factory.CreateWriter())
            {
                while (reader.CanContinue)
                {
                    using (var compressQueue = new BlockingQueue<IWaitable<Block>>())
                    {
                        // schedule a write work
                        var writeWork = threadPool.ScheduleWork(() =>
                        {
                            while (!compressQueue.IsCompleted)
                            {
                                IWaitable<Block> work;
                                try
                                {
                                    work = compressQueue.Dequeue();
                                }
                                catch 
                                {
                                    break;
                                }

                                try
                                {
                                    work.Wait();

                                    writer.WriteNext(work.Result);
                                }
                                finally
                                {
                                    work.Dispose();
                                }
                            }
                        });
                        
                        // read blocks until the memory limitation
                        var readBytes = 0L;
                        while (readBytes < _settings.MemoryLimitation && reader.CanContinue)
                        {
                            var block = reader.ReadNext();
                            readBytes += block.Data.Length;
                            
                            // schedule a compress work
                            var work = threadPool.ScheduleWork(() =>
                            {
                                gZip.Process(block);
                                return block;
                            });
                            compressQueue.Enqueue(work);
                        }
                    
                        compressQueue.CompleteEnqueuing();
                    
                        // wait for write work to complete before continue
                        writeWork.Wait();
                    }
                }
            }
        }
    }
}