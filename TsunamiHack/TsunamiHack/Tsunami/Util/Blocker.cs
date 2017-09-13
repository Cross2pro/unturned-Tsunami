using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    internal class Blocker : MonoBehaviour
    {
        internal enum Type { Banned, Disabled, OutOfDate, GameOutOfDate, EulaAgree}
        
        private HackController _ctrl;

        internal static bool BlockerEnabled;
        internal Rect WindowRect;
        internal bool Banned;
        internal static Type DisabledType;
        internal static Vector2 scrollpos;
        internal static bool EulaAgreed;
        
        private void Start()
        {
            var size = new Vector2(425,400);
            WindowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            
            _ctrl = WaveMaker.Controller;

            if (_ctrl.Disabled)
                BlockerEnabled = true;
            
            
            
        }

        private void OnGUI()
        {
            if (BlockerEnabled)
            {
                switch (DisabledType)
                {
                        case Type.Banned:
                            WindowRect = GUI.Window(WaveMaker.BannedId, WindowRect, BannedMenuFunct, "HACK DISABLED");
                            break;
                            
                        case Type.Disabled:
                            WindowRect = GUI.Window(WaveMaker.BannedId, WindowRect, DisabledMenuFunct, "HACK DISABLED");
                            break;
                        case Type.OutOfDate:
                            WindowRect = GUI.Window(WaveMaker.BannedId, WindowRect, OutOfDateMenuFunct, "OUT OF DATE");
                            break;
                        case Type.GameOutOfDate:
                            WindowRect = GUI.Window(WaveMaker.BannedId, WindowRect, GameOutOfDateMenuFunct, "YOU ARE USING A REPOSTED VERSION");
                            break;
                        case Type.EulaAgree:
                            WindowRect = GUI.Window(WaveMaker.BannedId, WindowRect, EulaAgreementFunct,
                                "You Must Agree to the Tsunami Hack Eula to use");
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
            GUILayout.Label($"Reason: {_ctrl.Reason}");
            GUILayout.Space(8f);
            GUILayout.Label($"This operation was made by {_ctrl.AuthorizedBy}");
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

        private void EulaAgreementFunct(int id)
        {
            PlayerPauseUI.active = true;
            PlayerUI.window.showCursor = true;
            
            GUILayout.Space(50f);
            GUILayout.Label("You Must Agree to our EULA to use Tsunami Hack");
            GUILayout.Space(8f);
            scrollpos = GUILayout.BeginScrollView(scrollpos, false, true);
            GUILayout.Label(WaveMaker.eula);
            GUILayout.EndScrollView();
            GUILayout.Space(10f);
            EulaAgreed = GUILayout.Toggle(EulaAgreed,"I AGREE TO TSUNAMI HACK EULA");
            GUILayout.Space(4f);
            if (GUILayout.Button("Continue"))
            {
                if (EulaAgreed)
                {
                    FileIo.AgreeEula();
                    BlockerEnabled = false;
                    PlayerPauseUI.active = false;
                    PlayerUI.window.showCursor = false;
                }
                
            }
        }
    }
}