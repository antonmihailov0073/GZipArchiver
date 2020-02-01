using System;
using VeeamTest.Helpers.Arguments;
using VeeamTest.Models;

namespace VeeamTest.Helpers
{
    public static class SettingsHelper
    {
        private const int DefaultBlockSize = 1 * 1024 * 1024; // 1 MB
        private const double MemoryLimitationCoefficient = 0.15; // using to calculate memory limitation as total available memory * this coefficient
        
        
        public static ArchiverSettings Get(string[] arguments)
        {
            var parsedArguments = ArgumentsHelper.Parse<ArchiverArguments>(arguments);
            return MapSettings(parsedArguments);
        }

        
        private static ArchiverSettings MapSettings(ArchiverArguments arguments)
        {
            var blockSize = arguments.BlockSize ?? DefaultBlockSize;
            var memoryLimitation = arguments.MemoryLimitation ?? CalculateMemoryLimitation();
            
            return new ArchiverSettings(arguments.Mode, arguments.Path, arguments.DestinationPath, blockSize, memoryLimitation);
        }
        
        private static long CalculateMemoryLimitation()
        {
            return (long) Math.Ceiling(GC.GetGCMemoryInfo().TotalAvailableMemoryBytes * MemoryLimitationCoefficient);
        }
    }
}