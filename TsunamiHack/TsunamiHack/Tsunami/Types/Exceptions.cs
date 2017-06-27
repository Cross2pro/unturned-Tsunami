using System;
using System.Runtime.Serialization;

namespace TsunamiHack.Tsunami.Types
{
    [Serializable]
    public class UnableToLoadException : Exception
    {


        public UnableToLoadException()
        {
        }

        public UnableToLoadException(string message) : base(message)
        {
        }

        public UnableToLoadException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnableToLoadException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
