using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Exceptions.Car
{
    public class CarDeleteException : Exception
    {
        public CarDeleteException(string message) : base(message) { }
    }
}
