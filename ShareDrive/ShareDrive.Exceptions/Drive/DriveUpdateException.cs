using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Exceptions.Drive
{
    public class DriveUpdateException : Exception
    {
        public DriveUpdateException(string message) : base(message) { }
    }
}
