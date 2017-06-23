using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types
{
    public class KeybindConfig
    {
        //private List<Keybind> KeyList;
        private Dictionary<string, KeyCode> KeyDict;

        public void addBind(string name, KeyCode key)
        {

            if (KeyDict.ContainsKey(name) || KeyDict.ContainsValue(key))
            {
                return;
            }

            KeyDict.Add(name, key);
        }

        public Tuple<string, KeyCode> getBind(string name)
        {

            if (KeyDict.ContainsKey(name))
            {
                KeyCode outvalue;
                if (KeyDict.TryGetValue(name, out outvalue))
                {
                    return new Tuple<string, KeyCode>(name, outvalue);
                }

                return null;
            }

            return null;
        }

        public Dictionary<string, KeyCode> getKeyDict()
        { 
            return KeyDict;
        }
            
    }
}
