namespace GameKit.Exception
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;

    public class GdkException : Exception
    {
        public GdkException() { }

        protected GdkException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context) { }

        public GdkException(string message) : base(message) { }

        public GdkException(string message, Exception innerException) : base(message, innerException) { }
    }
}