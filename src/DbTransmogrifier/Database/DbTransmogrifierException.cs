using System;

namespace DbTransmogrifier.Database
{
    public class DbTransmogrifierException : Exception
    {
        public DbTransmogrifierException(string message)
            : this(message, null)
        {
        }

        public DbTransmogrifierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}