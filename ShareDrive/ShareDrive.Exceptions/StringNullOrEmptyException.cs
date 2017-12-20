using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Exceptions
{
    public class StringNullOrEmptyException : Exception
    {
        public StringNullOrEmptyException(string message) : base (message) { }
    }
}
