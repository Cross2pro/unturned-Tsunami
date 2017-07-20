using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Keybind
    {
        private static List<KeyCode> _keyList;
        private static KeyCode _mainKey;
        private static KeyCode _visualsKey;
        private static KeyCode _keybindKey;

        public void Start()
        {
            _mainKey = WaveMaker.Keybinds.GetBind("main");
            _visualsKey = WaveMaker.Keybinds.GetBind("visuals");
            _keybindKey = WaveMaker.Keybinds.GetBind("keybinds"); 
        }
        
        private static void UpdateVars()
        {
            
//            if (Menu.Keybind.KeybindsChanged)
//            {
//                
//            }
        }

        public static void Check()
        {
            UpdateVars();

           
        }

        public static void ChangeKeybind(string name, KeyCode newKey)
        {
            
        }
    }
}
