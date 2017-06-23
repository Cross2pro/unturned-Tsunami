using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
