using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Types;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class WaveMaker
    {
        public static KeybindConfig Keybinds;

        private bool firstTime;

        public static Menu.Main main;
        public static Menu.Keybind keybind;


        private GameObject obj;


        public WaveMaker()
        {

        }

        public void Start()
        {
            //check if its first time
            //check if on ban list
        }


        public void OnUpdate()
        {
            if (Provider.isConnected)       //add all hack objects
            {
                if (obj == null)
                {
                    obj = new GameObject();
                    main = obj.AddComponent<Menu.Main>();
                    keybind = obj.AddComponent<Menu.Keybind>();

                    UnityEngine.Object.DontDestroyOnLoad(main);
                    UnityEngine.Object.DontDestroyOnLoad(keybind);
                }
            }
        }


    }
}
