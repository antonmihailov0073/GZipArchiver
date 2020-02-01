using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using VeeamTest.Helpers.Arguments.Converters.Core;

namespace VeeamTest.Helpers.Arguments.Converters
{
    public class EnumMemberValueConverter : IArgumentConverter
    {
        public object Convert(string arg, Type destinationType)
        {
            var field = destinationType
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => new { Field = f, Attribute = f.GetCustomAttribute<EnumMemberAttribute>() })
                .Where(o => o.Attribute != null && o.Attribute.Value == arg)
                .Select(o => o.Field)
                .FirstOrDefault();
            return field?.GetRawConstantValue() ?? 0;
        }
    }
}