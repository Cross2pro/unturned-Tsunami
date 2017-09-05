using System;
using TsunamiHack.Tsunami.Manager;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types
{
    [Serializable]
    public class Keybind
    {
        public string Name { get; private set; }
        public KeyCode Key { get; private set; }

        public Keybind(string name, KeyCode key)
        {
            Name = name;
            Key = key;
        }

        public Keybind()
        {            
        }

        public bool SetKey(KeyCode key)
        {
            var dict = WaveMaker.Keybinds.GetKeyDict();

            if (dict.ContainsKey(Name) || dict.ContainsValue(key))
            {
                return false;
            
            }
            Key = key;
            return true;
           
        }
    }
}
