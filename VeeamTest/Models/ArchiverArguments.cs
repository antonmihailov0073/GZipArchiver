using VeeamTest.Helpers.Arguments;
using VeeamTest.Helpers.Arguments.Converters;

namespace VeeamTest.Models
{
    public class ArchiverArguments
    {
        [Argument(Order = 1, IsRequired = true, Converter = typeof(EnumMemberValueConverter))]
        public ArchiverMode Mode { get; private set; }
        
        [Argument(Order = 2, IsRequired = true)]
        public string Path { get; private set; }
        
        [Argument(Order = 3, IsRequired = true)]
        public string DestinationPath { get; private set; }
        
        [Argument(Order = 4)]
        public int? BlockSize { get; private set; }
        
        [Argument(Order = 5)]
        public int? MemoryLimitation { get; private set; }
    }
}