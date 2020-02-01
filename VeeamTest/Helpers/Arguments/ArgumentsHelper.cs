using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VeeamTest.Helpers.Arguments.Converters;
using VeeamTest.Helpers.Arguments.Converters.Core;
using VeeamTest.Helpers.Strings;

namespace VeeamTest.Helpers.Arguments
{
    public static class ArgumentsHelper
    {
        public static TModel Parse<TModel>(string[] arguments)
            where TModel : new()
        {
            var model = new TModel();
            foreach (var property in EnumerateArgumentProperties(model.GetType()))
            {
                property.SetValue(model, arguments);
            }
            return model;
        }
        
        private static IEnumerable<ArgumentProperty> EnumerateArgumentProperties(Type modelType)
        {
            return modelType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => new { Attribute = p.GetCustomAttribute<ArgumentAttribute>(), Property = p })
                .Where(o => o.Property.CanWrite && o.Attribute != null)
                .Select(o => new ArgumentProperty(o.Property, o.Attribute));
        }


        private class ArgumentProperty
        {
            private readonly PropertyInfo _property;
            private readonly ArgumentAttribute _attribute;
            
            
            public ArgumentProperty(PropertyInfo property, ArgumentAttribute attribute)
            {
                _property = property;
                _attribute = attribute;
            }
            

            public void SetValue(object model, string[] arguments)
            {
                var argument = GetArgumentValue(arguments);
                if (argument == null)
                {
                    return;
                }
            
                var converter = GetConverter();
                var value = converter.Convert(argument, _property.PropertyType);
            
                _property.SetValue(model, value);
            }

            
            private string GetArgumentValue(string[] arguments)
            {
                if (_attribute.Order <= arguments.Length)
                {
                    return arguments[_attribute.Order - 1];
                }
            
                if (_attribute.IsRequired)
                {
                    throw new InvalidOperationException(StringsHelper.MissingRequiredArguments());
                }
            
                return null;
            }

            private IArgumentConverter GetConverter()
            {
                var converterType = _attribute.Converter;
                
                var isCorrectInterface = typeof(IArgumentConverter).IsAssignableFrom(converterType);
                var hasEmptyConstructor = converterType.GetConstructor(Type.EmptyTypes) != null;
                
                if (isCorrectInterface && hasEmptyConstructor)
                {
                    return (IArgumentConverter) Activator.CreateInstance(converterType);
                }
                
                throw new InvalidOperationException(StringsHelper.InvalidConverter());
            }
        }
    }
}