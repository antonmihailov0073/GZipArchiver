using System;
using VeeamTest.Helpers;
using VeeamTest.Helpers.Strings;
using VeeamTest.Services.Archiver;

namespace VeeamTest
{
    public static class Program
    {
        public static void Main(string[] arguments)
        {
            try
            {
                var settings = SettingsHelper.Get(arguments);
                var archiver = new GZipArchiver(settings);
                
                Console.WriteLine(StringsHelper.ProcessStarted());
                
                var elapsed = Benchmark.Elapse(() => archiver.Process());
                
                Console.WriteLine(StringsHelper.Processed(elapsed));
            }
            catch (Exception exception)
            {
                Console.WriteLine(StringsHelper.Exception(exception));
            }
        }
    }
}