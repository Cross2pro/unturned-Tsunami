using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsunamiHack.Tsunami
{

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.GenericParameter | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Struct, AllowMultiple = true)]
    class Identifier : Attribute
    {
        private string _info;

        public string Info { get{ return _info; } }

        public Identifier(string description)
        {
            _info = description;
        }

    }
}
