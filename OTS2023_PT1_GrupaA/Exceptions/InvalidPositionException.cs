

using System;

namespace OTS2026_PT1_GrupaB.Exceptions
{
    public class InvalidPositionException: Exception
    {
        public InvalidPositionException()
        {

        }

        public InvalidPositionException(string message) : base(message)
        {

        }
    }
}
