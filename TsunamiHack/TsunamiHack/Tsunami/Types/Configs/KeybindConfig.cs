using System;
using System.Collections.Generic;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types.Configs
{
    [Serializable]
    public class KeybindConfig
    {
        public Dictionary<string, KeyCode> KeyDict;

        public KeybindConfig()
        {
            KeyDict = new Dictionary<string, KeyCode>();
        }

    #region returns

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

        public Dictionary<string, KeyCode> GetKeyDict()
        { 
             return KeyDict;
        }

        public List<string> GetNameList()
        {
            var list = new List<string>();
            foreach (var key in KeyDict.Keys)
            {
                list.Add(key);
            }
            return list;
        }

        public List<KeyCode> GetCodeList()
        {
            var list = new List<KeyCode>();
            foreach (var value in KeyDict.Values)
            {
                list.Add(value);
            }
            return list;
        }

        public bool BindExists(string name)
        {
            return KeyDict.ContainsKey(name);
        }

        public bool BindExists(KeyCode key)
        {
            return KeyDict.ContainsValue(key);
        }
        
    #endregion
        
    #region manipulation
        
        public void AddBind(string name, KeyCode key)
        {

            if (KeyDict.ContainsKey(name) || KeyDict.ContainsValue(key))
            {
                return;
            }

            KeyDict.Add(name, key);
        }
        
        public void RemoveBind(string name)
        {
            if (KeyDict.ContainsKey(name))
                KeyDict.Remove(name);
        }

        public bool ChangeBind(string name, KeyCode key)
        {
            if (KeyDict.ContainsKey(name))
            {
                KeyDict[name] = key;
                return true;
            }

            return false;
        }

        public void SaveBinds()
        {
            Util.FileIo.SaveKeybinds(this);
        }

    #endregion


        
    }
}
