using System;

namespace ShareDrive.Exceptions
{
    public class NullObjectException : Exception
    {
        public NullObjectException(string message) : base(message)
        {
        }
    }
}
