using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
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
        //TODO: Change popup controller to be able to be instantiated outside of game runtime
        
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

        public static readonly int BannedId = 11;
        public static readonly int FtPopupId = 12;
        public static readonly int InfoPopupId = 13;

        public static ulong LocalSteamId;

        public static readonly string Version = "3.20.0.0";
        
        private GameObject _obj;
        private GameObject _blockerObj;
            
        public void Start()
        {
            
            LocalSteamId = Provider.client.m_SteamID;
            
            if (FirstTime)
            {
                PopupController.EnableFirstTime = true;
            }

            if (Ban.Contains(LocalSteamId.ToString()))
            {
                Logging.LogMsg("BANNED",
                    "You have been banned! If you are reading this you are one smart cookie! Contact Tidal on steam to dispute (www.steamcommunity.com/id/tidall");
                Controller.BanOverride(
                    "You have been globally banned from using TsunamiHack! Verify game files to uninstall");
                Util.Blocker.DisabledType = Blocker.Type.Banned;
            }
            else
                Util.Blocker.DisabledType = Blocker.Type.Disabled;
        }
        
        public void OnUpdate()
        {
            if (_blockerObj == null)
            {
                _blockerObj = new GameObject();
                Blocker = _blockerObj.AddComponent<Blocker>();
                UnityEngine.Object.DontDestroyOnLoad(Blocker);
            }
            
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
