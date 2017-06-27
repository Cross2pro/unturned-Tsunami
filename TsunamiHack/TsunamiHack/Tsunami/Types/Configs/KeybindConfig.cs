using System.Collections.Generic;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types.Configs
{
    public class KeybindConfig
    {
        public Dictionary<string, KeyCode> KeyDict;

        public KeybindConfig()
        {
            KeyDict = new Dictionary<string, KeyCode>();
        }

        public void AddBind(string name, KeyCode key)
        {

            if (KeyDict.ContainsKey(name) || KeyDict.ContainsValue(key))
            {
                return;
            }

            KeyDict.Add(name, key);
        }

        public KeyCode GetBind(string name)
        {

            if (KeyDict.ContainsKey(name))
            {
                KeyCode outvalue;
                if (KeyDict.TryGetValue(name, out outvalue))
                {
                    return outvalue;
                }

                return KeyCode.F1;
            }

            return KeyCode.F1;
        }

        public void RemoveBind(string name)
        {
            if (KeyDict.ContainsKey(name))
                KeyDict.Remove(name);
        }

        public Dictionary<string, KeyCode> GetKeyDict()
        { 
             return KeyDict;
        }
            
    }
}
