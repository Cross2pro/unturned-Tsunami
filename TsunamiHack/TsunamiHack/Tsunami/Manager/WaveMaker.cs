using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Types.Configs;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    internal class WaveMaker
    {
        //TODO: Add way to check the integrity of local files if they are deleted during run
        
        public static PremiumList Prem;
        public static BanList Ban;
        public static BetaList Beta;
        public static HackController Controller;
        public static FriendsList Friends;
        public static KeybindConfig Keybinds;
        public static Settings Settings;

        public static bool FirstTime;
        public static bool HackDisabled;

        public static Menu.Main MenuMain;
        public static Menu.Keybind MenuKeybind;
        public static PopupController PopupController;
        public static Blocker Blocker;

        public static readonly int MainId = 1; 
        public static readonly int VisualsId = 2;
        public static readonly int AimId = 3;
        public static readonly int KeybindId = 4;
        public static readonly int FriendId = 5;
        public static readonly int SettingsId = 6;

        public static readonly int FtPopupId = 11;
        public static readonly int InfoPopupId = 12;

        //TODO: add any more menu ids
        
        private GameObject _obj;
        private GameObject _blockerObj;

        public void Start()
        {
            if (FirstTime)
            {
                PopupController.EnableFirstTime = true;
            }
            
            if(Player.player)
            
            //check if on ban list
        }

        //TODO: Create blocker
        
        public void OnUpdate()
        {
            _blockerObj = new GameObject();
            Blocker = _blockerObj.AddComponent<Blocker>();
            UnityEngine.Object.DontDestroyOnLoad(Blocker);
            
            if (Provider.isConnected && !HackDisabled)
            {
                if (_obj == null)
                {
                    _obj = new GameObject();
                    MenuMain = _obj.AddComponent<Menu.Main>();
                    MenuKeybind = _obj.AddComponent<Menu.Keybind>();
                    PopupController = _obj.AddComponent<PopupController>();

                    UnityEngine.Object.DontDestroyOnLoad(MenuMain);
                    UnityEngine.Object.DontDestroyOnLoad(MenuKeybind);
                    UnityEngine.Object.DontDestroyOnLoad(PopupController);

                    //TODO: add other hack objects
 
                }
            }
            if (HackDisabled)
            {
                if (_obj != null)
                {
                    _obj = null;
                    MenuMain = null;
                    MenuKeybind = null;
                    PopupController = null;
                
                    UnityEngine.Object.Destroy(MenuMain);
                    UnityEngine.Object.Destroy(MenuKeybind);
                    UnityEngine.Object.Destroy(PopupController);

                }
            }
            
        }
    }
}
