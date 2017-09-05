using System;

namespace TsunamiHack.Tsunami.Types
{
    public class UnableToMatchPlayerException : Exception
    {
        public UnableToMatchPlayerException()
        {
        }

        public UnableToMatchPlayerException(string message)
            : base(message)
        {
        }

        public UnableToMatchPlayerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}