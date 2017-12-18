using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Exceptions.Car
{
    public class CarCreateException : Exception
    {
        public CarCreateException(string message) : base(message) { }
    }
}
