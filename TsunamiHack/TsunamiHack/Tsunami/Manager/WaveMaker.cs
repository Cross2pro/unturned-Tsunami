using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Types.Configs;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class WaveMaker
    {
        public static PremiumList Prem;
        public static BanList Ban;
        public static BetaList Beta;
        public static HackController Controller;
        public static FriendsList Friends;
        public static KeybindConfig Keybinds;
        public static Settings Settings;

        public static bool FirstTime;
        public static bool HackDisabled;

        public static Menu.Main menuMain;
        public static Menu.Keybind menuKeybind;


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
                    menuMain = _obj.AddComponent<Menu.Main>();
                    menuKeybind = _obj.AddComponent<Menu.Keybind>();

                    UnityEngine.Object.DontDestroyOnLoad(menuMain);
                    UnityEngine.Object.DontDestroyOnLoad(menuKeybind);

                    //TODO: add other hack objects
                }
            }
        }


    }
}
