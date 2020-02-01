using System;
using VeeamTest.Helpers.Strings;
using VeeamTest.Models;
using VeeamTest.Services.Readers.Core;

namespace VeeamTest.Services.Readers
{
    public class CompressedBlockReader : AFileReader
    {
        private const int FullHeaderLength = 16;
        
        
        public CompressedBlockReader(string path)
            : base(path)
        {
        }
        

        protected override Block ReadNextInternal()
        {
            try
            {
                var bytes = new byte[FullHeaderLength];
                File.Read(bytes, 0, bytes.Length);
                
                // check extra field bit
                if ((bytes[3] & 4) == 0)
                {
                    throw new InvalidOperationException(StringsHelper.EmptyExtraFieldFlag());
                }

                // check extra field length
                var extraFieldLength = BitConverter.ToInt16(new ReadOnlySpan<byte>(bytes, 10, 2));
                if (extraFieldLength != sizeof(int))
                {
                    throw new InvalidOperationException(StringsHelper.InvalidExtraFieldLength());
                }
                
                // get block size from extra field
                var blockLength = BitConverter.ToInt32(new ReadOnlySpan<byte>(bytes, 12, extraFieldLength));

                var oldBytesLength = bytes.Length;
                Array.Resize(ref bytes, bytes.Length + blockLength);
                File.Read(bytes, oldBytesLength, blockLength);
                
                return new Block(bytes);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(StringsHelper.UnsupportedCompressedFile(), e);
            }
        }
    }
}