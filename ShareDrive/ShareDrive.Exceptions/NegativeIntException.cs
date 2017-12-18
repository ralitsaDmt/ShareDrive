using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Exceptions
{
    public class NegativeIntException : Exception
    {
        public NegativeIntException(string message) : base(message) { }
    }
}
