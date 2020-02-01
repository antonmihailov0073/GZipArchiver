using System;
using System.Diagnostics;

namespace VeeamTest.Helpers
{
    public static class Benchmark
    {
        public static TimeSpan Elapse(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}