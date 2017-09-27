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

        public static bool ShowEula = true;
        public static string eula;
        
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

        public static readonly string Version = "1.1.2";
        public static readonly string GameVersion = "3.20.6.0";

        private string messagetoanyone =
                "If you are reading this, youve obviously had to use some modified tool to open it. If you are really that desperate " +
                "to get my source code, just fucking ask me im going to open source it anyway. Dont use this to make money, however, or" +
                "we will be having some legal proceedings.";
        
        private GameObject _obj;
        private GameObject _blockerObj;

        public void Start()
        {            
            
            
            
            if (ShowEula)
            {
                
                WebAccess.DownloadEula(out eula);
                Blocker.BlockerEnabled = true;
                Blocker.DisabledType = Blocker.Type.EulaAgree;
            }
            
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

            Logging.Log($"CURRENT VERSION NUMBER: {Controller.Version}");
            Logging.Log($"CURRENT DLL VERSION NUMBER: {Version}");

            if (Controller.Version != Version)
            {
                Logging.Log("Setting Disabled Type");
                Blocker.DisabledType = Blocker.Type.OutOfDate;
                Logging.Log("Enabling Blocker");
                Blocker.BlockerEnabled = true;
                Logging.Log("setting hackdisabed to true");
                HackDisabled = true;
                Logging.Log("Setting controller disabled to true");
                Controller.Disabled = true;
            }
            else if (GameVersion != Provider.APP_VERSION)
            {
                Blocker.DisabledType = Blocker.Type.GameOutOfDate;
                Blocker.BlockerEnabled = true;
                HackDisabled = true;
                Controller.Disabled = true;
            }
            else if (Ban.Contains(LocalSteamId.ToString()))
            {
                Logging.Log("YOU HAVE BEEN BANNED, CONTACT TIDAL ON STEAM TO DISPUTE WWW.STEAMCOMMUNITY.COM/ID/TIDALL", "FATAL ERROR, BANNED");
                Controller.BanOverride("YOU HAVE BEEN GLOBALLY BANNED FROM USING TSUNAMI HACK, VERIFY GAME FILES TO UNINSTALL");
                Blocker.DisabledType = Blocker.Type.Disabled;
            }
            else
            {
                Blocker.DisabledType = Blocker.Type.Disabled;
            }
            
            //Whitelist me from all disabling
//            if (PlayerTools.GetSteamPlayer(Player.player).playerID.steamID.m_SteamID == Controller.Dev)
//            {
//                HackDisabled = false;
//                Controller.Disabled = false;
//                Blocker.BlockerEnabled = false;
//            }
                 
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
