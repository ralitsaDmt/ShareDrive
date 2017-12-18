using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Exceptions.Car
{
    public class CarEditException : Exception
    {
        public CarEditException(string message) : base(message) { }
    }
}
