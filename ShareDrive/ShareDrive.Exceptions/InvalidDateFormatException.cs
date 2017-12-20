using System;

namespace ShareDrive.Exceptions
{
    public class InvalidDateFormatException : FormatException
    {
        public InvalidDateFormatException(string message) : base(message) { }
    }
}
