using System;
using System.Runtime.InteropServices;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    internal class Blocker : MonoBehaviour
    {
        internal enum Type { Banned, Disabled, OutOfDate, GameOutOfDate}
        
        private HackController ctrl;

        internal static bool BlockerEnabled;
        internal Rect windowRect;
        internal bool Banned;
        internal static Type DisabledType;
        
        private void Start()
        {
            var size = new Vector2(425,400);
            windowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            
            ctrl = WaveMaker.Controller;

            if (ctrl.Disabled)
                BlockerEnabled = true;
            
            
            
        }

        private void Update()
        {
            
        }

        private void OnGUI()
        {
            if (BlockerEnabled)
            {
                switch (DisabledType)
                {
                        case Type.Banned:
                            windowRect = GUI.Window(WaveMaker.BannedId, windowRect, BannedMenuFunct, "HACK DISABLED");
                            break;
                            
                        case Type.Disabled:
                            windowRect = GUI.Window(WaveMaker.BannedId, windowRect, DisabledMenuFunct, "HACK DISABLED");
                            break;
                        case Type.OutOfDate:
                            windowRect = GUI.Window(WaveMaker.BannedId, windowRect, OutOfDateMenuFunct, "OUT OF DATE");
                            break;
                        case Type.GameOutOfDate:
                            windowRect = GUI.Window(WaveMaker.BannedId, windowRect, GameOutOfDateMenuFunct, "YOU ARE USING A REPOSTED VERSION");
                            break;
                }
            }
        }

        private void DisabledMenuFunct(int id)
        {
            
            PlayerPauseUI.active = true;
            PlayerUI.window.showCursor = true;
           
            GUILayout.Space(50f);
            GUILayout.Label($"TsunamiHack has been globally disabled by its creator.");
            GUILayout.Space(8f);
            GUILayout.Label($"Reason: {ctrl.Reason}");
            GUILayout.Space(8f);
            GUILayout.Label($"This operation was made by {ctrl.AuthorizedBy}");
            GUILayout.Space(100f);
            GUILayout.Label("To remove the hack and play normally, click \"EXIT GAME\" and \"Verify Game Files\" through steam");
            GUILayout.Space(20f);
            if(GUILayout.Button("EXIT GAME"))
            {
                Application.Quit();
            }
        }

        private void BannedMenuFunct(int id)
        {
            PlayerPauseUI.active = true;
            PlayerUI.window.showCursor = true;
            
            GUILayout.Space(50f);
            GUILayout.Label($"You have been globally banned from using Tsunami Hack");
            GUILayout.Space(8f);
            GUILayout.Label($"This operation was made by AutoBanner");
            GUILayout.Space(100f);
            GUILayout.Label("To remove the hack and play normally, click \"EXIT GAME\" and \"Verify Game Files\" through steam");
            GUILayout.Space(20f);
            if(GUILayout.Button("EXIT GAME"))
            {
                Application.Quit();
            }
        }

        private void OutOfDateMenuFunct(int id)
        {
            PlayerPauseUI.active = true;
            PlayerUI.window.showCursor = true;
            
            GUILayout.Space(50f);
            GUILayout.Label($"Your hack is out of date!");
            GUILayout.Space(8f);
            GUILayout.Label($"You are running hack version {WaveMaker.Version} and there is a new version of {WaveMaker.Controller.Version}");
            GUILayout.Label($"For concerns of bug fixes and/or new features we are restricting this version. Please download and install the latest version to continue using TsunamiHack");
            GUILayout.Space(50f);
            GUILayout.Label("To remove the hack and play normally, click \"EXIT GAME\" and \"Verify Game Files\" through steam");
            GUILayout.Label("You can get the latest game version by visiting the link below");
            if(GUILayout.Button("Join Discord and get latest version"))
            {
                System.Diagnostics.Process.Start("https://discord.gg/cW8Mjdf");
            }
            
            GUILayout.Space(20f);
            if(GUILayout.Button("EXIT GAME"))
            {
                Application.Quit();
            }
            
        }

        private void GameOutOfDateMenuFunct(int id)
        {
            PlayerPauseUI.active = true;
            PlayerUI.window.showCursor = true;
            
            GUILayout.Space(50f);
            GUILayout.Label($"You got this DLL from someone posing as its creator!");
            GUILayout.Space(8f);
            GUILayout.Label($"You are running hack for game version {WaveMaker.GameVersion} and there is a new version for {Provider.APP_VERSION}");
            GUILayout.Label($"Download the latest version directly from Tidal");
            GUILayout.Space(50f);
            GUILayout.Label("To remove the hack and play normally, click \"EXIT GAME\" and \"Verify Game Files\" through steam");
            GUILayout.Label("You can get the latest game version by visiting the link below");
            if(GUILayout.Button("Join Discord and get latest version"))
            {
                System.Diagnostics.Process.Start("https://discord.gg/cW8Mjdf");
            }
            
            GUILayout.Space(20f);
            if(GUILayout.Button("EXIT GAME"))
            {
                Application.Quit();
            }
        }
        
        
    }
}