using ShareDrive.Exceptions;
using System;

namespace ShareDrive.Core
{
    public static class Require
    {
        public static void ThatObjectIsNotNull(object obj)
        {
            if (obj == null)
            {
                throw new NullObjectException("The given object is null.");
            }
        }

        public static void ThatIntIsPositive(int number)
        {
            if (number <= 0)
            {
                throw new NegativeIntException("The given number is not positive.");
            }
        }
    }
}
