using SDG.Unturned;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Types.Configs;
using TsunamiHack.Tsunami.Util;
using UnityEngine;
using UnityEngine.UI;

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
        public static bool SoftDisable;

        public static int MenuOpened;

        public static Menu.Main MenuMain;
        public static Menu.Keybind MenuKeybind;
        public static Menu.Visuals MenuVisuals;
        public static Menu.Aim MenuAim;
        public static PopupController PopupController;
        public static Blocker Blocker;
        public static ShowEnabled Enabler;
        public static readonly int MainId = 1; 
        public static readonly int VisualsId = 2;
        public static readonly int AimId = 3;
        public static readonly int KeybindId = 4;
        public static readonly int FriendId = 5;
        public static readonly int SettingsId = 6;

        public static readonly int BannedId = 11;
        public static readonly int FtPopupId = 12;

        public const string Version = "2.0";
        public const string GameVersion = "3.21.3.1";
        public const string OmittedServer = "90111985339994117";

        private GameObject _obj;
        private GameObject _blockerObj;

        public void Start()
        {
            //Check if they are of any particular rank
            if (Provider.client.m_SteamID == Controller.Dev)
            {
                isDev = true;
                isBeta = true;
                isPremium = true;
            }
            
            if (Prem.Contain(Provider.client.m_SteamID.ToString()))
                isPremium = true;
            
            if (Beta.Contain(Provider.client.m_SteamID.ToString()))
                isBeta = true;
            
            //Show the EULA
            if (ShowEula)
            {
                WebAccess.DownloadEula(out eula);
                Blocker.BlockerEnabled = true;
                Blocker.DisabledType = Blocker.Type.EulaAgree;
            }
            
            //Set blocker
            if (Controller.Version != Version)
            {
                Blocker.SetDisabled(Blocker.Type.OutOfDate);
            }
            else if (Provider.APP_VERSION != GameVersion)
            {
                Blocker.SetDisabled(Blocker.Type.GameOutOfDate);
            }
            else if (Ban.Contains(Provider.client.m_SteamID.ToString()))
            {
                Controller.BanOverride("YOU HAVE BEEN GLOBALLY BANNED FROM USING TSUNAMI HACK, VERIFY GAME FILES TO UNINSTALL");
                Blocker.DisabledType = Blocker.Type.Disabled;
            }
            else
            {
                Blocker.DisabledType = Blocker.Type.Disabled;
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

            if (Provider.isConnected && Provider.server.m_SteamID.ToString() == OmittedServer && !isDev && !isPremium)
            {
                if(HackDisabled != true && !Blocker.DontShowBlocker)
                    Blocker.SetDisabled(Blocker.Type.OmittedServer);
            }
            
            if (Provider.isConnected && Provider.server.m_SteamID.ToString() != OmittedServer && Blocker.DontShowBlocker)
            {
                Blocker.DisabledType = Blocker.Type.Disabled;
                Blocker.BlockerEnabled = false;
                HackDisabled = false;
                Controller.Disabled = false;
                Blocker.DontShowBlocker = false;
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
                    Enabler = _obj.AddComponent<ShowEnabled>();
                    

                    Object.DontDestroyOnLoad(MenuMain);
                    Object.DontDestroyOnLoad(MenuKeybind);
                    Object.DontDestroyOnLoad(PopupController);
                    Object.DontDestroyOnLoad(MenuVisuals);
                    Object.DontDestroyOnLoad(MenuAim);
                    Object.DontDestroyOnLoad(Enabler);

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
                    Object.Destroy(Enabler);
                    
                    _obj = null;
                    MenuMain = null;
                    MenuKeybind = null;
                    PopupController = null;
                    MenuVisuals = null;
                    MenuAim = null;
                    Enabler = null;

                }
            }

            
        }
    }
}
