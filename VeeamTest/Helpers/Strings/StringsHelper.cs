using System;

namespace VeeamTest.Helpers.Strings
{
    public static class StringsHelper
    {
        public static string ProcessStarted()
        {
            return StringsValues.ProcessStarted;
        }
        
        public static string Exception(Exception exception)
        {
            return StringsValues.ExceptionPrefix + exception.Message;
        }

        public static string Processed(TimeSpan elapsed)
        {
            return StringsValues.ProcessedPrefix + elapsed;
        }

        public static string MissingRequiredArguments()
        {
            return StringsValues.MissingRequiredArguments;
        }

        public static string InvalidConverter()
        {
            return StringsValues.InvalidConverter;
        }

        public static string EmptyExtraFieldFlag()
        {
            return StringsValues.EmptyExtraFieldFlag;
        }

        public static string InvalidExtraFieldLength()
        {
            return StringsValues.InvalidExtraFieldLength;
        }

        public static string UnsupportedCompressedFile()
        {
            return StringsValues.UnsupportedCompressedFile;
        }

        public static string EnqueuingCompleted()
        {
            return StringsValues.EnqueuingCompleted;
        }
    }
}