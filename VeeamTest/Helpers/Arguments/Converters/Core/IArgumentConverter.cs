using System;

namespace VeeamTest.Helpers.Arguments.Converters.Core
{
    public interface IArgumentConverter
    {
        object Convert(string arg, Type destinationType);
    }
}