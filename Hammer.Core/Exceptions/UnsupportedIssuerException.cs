using System;
using System.Runtime.Serialization;

namespace Hammer.Core
{
    [Serializable]
    public class UnsupportedIssuerException : Exception
    {
        public UnsupportedIssuerException()
        {
        }

        public UnsupportedIssuerException(string message) : base(message)
        {
        }

        public UnsupportedIssuerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsupportedIssuerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}