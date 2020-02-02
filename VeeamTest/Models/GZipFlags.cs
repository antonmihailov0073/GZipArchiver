using System;

namespace VeeamTest.Models
{
    [Flags]
    public enum GZipFlags : byte
    {
        AsciiText = 1,
        ContinuationOfGzipFilePartNumber = 1 << 1,
        ExtraFieldPresent = 1 << 2,
        OriginalFileNamePresent = 1 << 3,
        FileCommentPresent = 1 << 4,
        FileIsEncryptedHeaderPresent = 1 << 5
    }
}