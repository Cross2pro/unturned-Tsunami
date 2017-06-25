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

        private bool _firstTime;

        public static Menu.Main Main;
        public static Menu.Keybind Keybind;


        private GameObject _obj;


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
            if (Provider.isConnected)
            {
                if (_obj == null)
                {
                    _obj = new GameObject();
                    Main = _obj.AddComponent<Menu.Main>();
                    Keybind = _obj.AddComponent<Menu.Keybind>();

                    UnityEngine.Object.DontDestroyOnLoad(Main);
                    UnityEngine.Object.DontDestroyOnLoad(Keybind);

                    //TODO: add other hack objects
                }
            }
        }


    }
}
