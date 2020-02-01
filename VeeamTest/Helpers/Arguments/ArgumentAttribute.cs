using System;
using VeeamTest.Helpers.Arguments.Converters;

namespace VeeamTest.Helpers.Arguments
{
    public class ArgumentAttribute : Attribute
    {
        public int Order { get; set; }

        public Type Converter { get; set; } = typeof(DefaultConverter);
        
        public bool IsRequired { get; set; }
    }
}