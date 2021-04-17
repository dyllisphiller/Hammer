using System;
using System.Runtime.Serialization;

namespace Hammer.Core
{
    [Serializable]
    public class LicenseDatabaseUpdatingException : Exception
    {
        public LicenseDatabaseUpdatingException()
        {
        }

        public LicenseDatabaseUpdatingException(string message) : base(message)
        {
        }

        public LicenseDatabaseUpdatingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LicenseDatabaseUpdatingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
