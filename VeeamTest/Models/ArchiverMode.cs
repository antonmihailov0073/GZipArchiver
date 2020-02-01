using System.Runtime.Serialization;

namespace VeeamTest.Models
{
    public enum ArchiverMode
    {
        [EnumMember(Value = "compress")]
        Compress,
        [EnumMember(Value = "decompress")]
        Decompress
    }
}