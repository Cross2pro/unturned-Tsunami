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
        //TODO: change window sizes to more accurately change between devices
        //TODO: add shutoff for all hacks at once

        public static bool isDev;
        public static bool isPremium;
        public static bool isBeta;
        
        public static PremiumList Prem;
        public static BanList Ban;
        public static BetaList Beta;
        public static HackController Controller;
        public static FriendsList Friends;
        public static KeybindConfig Keybinds;
        public static Settings Settings;

        public static bool FirstTime;
        public static bool HackDisabled;

        public static int MenuOpened;

        public static Menu.Main MenuMain;
        public static Menu.Keybind MenuKeybind;
        public static Menu.Visuals MenuVisuals;
        public static Menu.Aim MenuAim;
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

        public static readonly string Version = "1.0";
        public static readonly string GameVersion = "3.20.4.0";
        
        private GameObject _obj;
        private GameObject _blockerObj;

        public void Start()
        {
            
            //Checking if player is dev
            LocalSteamId = Provider.client.m_SteamID;

            if (LocalSteamId == Controller.Dev || LocalSteamId == ulong.Parse("76561198308025096"))
            {
                isDev = true;
                isBeta = true;
                isPremium = true;
            }

            if (Prem.Contain(LocalSteamId.ToString()))
                isPremium = true;

            if (Beta.Contain(LocalSteamId.ToString()))
                isBeta = true;
                

            //Checking if first time
            if (FirstTime)
            {
                PopupController.EnableFirstTime = true;
            }

            //Checking blocker
            if (Ban.Contains(LocalSteamId.ToString()))
            {
                Logging.Log("BANNED",
                    "You have been banned! If you are reading this you are one smart cookie! Contact Tidal on steam to dispute (www.steamcommunity.com/id/tidall");
                Controller.BanOverride(
                    "You have been globally banned from using TsunamiHack! Verify game files to uninstall"); //change this when i make installer
                Blocker.DisabledType = Blocker.Type.Banned;
            }
            else if (Version != Controller.Version)
            {
                Blocker.DisabledType = Blocker.Type.OutOfDate;
                Blocker.BlockerEnabled = true;
                HackDisabled = true;
                Controller.Disabled = true;
            }
            else if (GameVersion != Provider.APP_VERSION)
            {
                Blocker.DisabledType = Blocker.Type.GameOutOfDate;
                Blocker.BlockerEnabled = true;
                HackDisabled = true;
                Controller.Disabled = true;
            } 
            else
                Blocker.DisabledType = Blocker.Type.Disabled;

            
            //Whitelist me from all disabling
            if (PlayerTools.GetSteamPlayer(Player.player).playerID.steamID.m_SteamID == Controller.Dev)
            {
                HackDisabled = false;
                Controller.Disabled = false;
                Blocker.BlockerEnabled = false;
            }
                 
        }

        public void OnUpdate()
        {
            
            if (_blockerObj == null)
            {
                _blockerObj = new GameObject();
                Blocker = _blockerObj.AddComponent<Blocker>();
                Object.DontDestroyOnLoad(Blocker);
            }

            if (Provider.isConnected && !HackDisabled)
            {
                if (_obj == null)
                {
                    _obj = new GameObject();
                    MenuMain = _obj.AddComponent<Menu.Main>();
                    MenuKeybind = _obj.AddComponent<Menu.Keybind>();
                    MenuVisuals = _obj.AddComponent<Menu.Visuals>();
                    PopupController = _obj.AddComponent<PopupController>();
                    MenuAim = _obj.AddComponent<Menu.Aim>();

                    Object.DontDestroyOnLoad(MenuMain);
                    Object.DontDestroyOnLoad(MenuKeybind);
                    Object.DontDestroyOnLoad(PopupController);
                    Object.DontDestroyOnLoad(MenuVisuals);
                    Object.DontDestroyOnLoad(MenuAim);

                    //TODO: add other hack objects
                }
            }

            if (HackDisabled || Provider.isConnected == false )
            {
                if (_obj != null)
                {
                    Object.Destroy(MenuMain);
                    Object.Destroy(MenuKeybind);
                    Object.Destroy(PopupController);
                    Object.Destroy(MenuVisuals);
                    Object.Destroy(MenuAim);
                    
                    _obj = null;
                    MenuMain = null;
                    MenuKeybind = null;
                    PopupController = null;
                    MenuVisuals = null;
                    MenuAim = null;
                
                }
            }
            
        }
    }
}
