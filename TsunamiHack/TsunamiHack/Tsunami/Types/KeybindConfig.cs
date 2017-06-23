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
        private Dictionary<string, KeyCode> KeyList;

        public void addBind(string name, KeyCode key)
        {
            if (KeyList.ContainsKey(name) || KeyList.ContainsValue(key))
            {
                return;
            }

            KeyList.Add(name,key);
        }
    }
}
