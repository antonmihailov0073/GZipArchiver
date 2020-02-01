using System;
using VeeamTest.Helpers.Arguments.Converters.Core;

namespace VeeamTest.Helpers.Arguments.Converters
{
    public class DefaultConverter : IArgumentConverter
    {
        public object Convert(string arg, Type destinationType)
        {
            return System.Convert.ChangeType(arg, destinationType);
        }
    }
}