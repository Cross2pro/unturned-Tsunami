using System;
using System.Runtime.InteropServices;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    public class Blocker : MonoBehaviour
    {
        public enum Type { Banned, Disabled}
        
        private HackController ctrl;

        private static bool BlockerEnabled;
        public Rect windowRect;
        public bool Banned;
        public static Type DisabledType;
        
        private void Start()
        {
            var size = new Vector2(500,700);
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

                            
                }
            }
        }

        private void DisabledMenuFunct(int id)
        {
            var style = new GUIStyle();
            
            style.alignment = TextAnchor.UpperCenter;
            
            PlayerPauseUI.active = true;
            PlayerUI.window.showCursor = true;
            
            GUILayout.Label($"TsunamiHack has been globally disabled by its creator.\n\nReason: {ctrl.Reason}\n\nThis Operation was made by {ctrl.AuthorizedBy}\n\n\nTo remove the hack and play normally, click \"EXIT GAME\" and \"Verify Game Files\" through steam");
//            GUILayout.Space(8f);
//            GUILayout.Label($"Reason: {ctrl.Reason}");
//            GUILayout.Space(8f);
//            GUILayout.Label($"This operation was made by {ctrl.AuthorizedBy}");
//            GUILayout.Space(20f);
//            GUILayout.Label("To remove the hack and play normally, click \"EXIT GAME\" and \"Verify Game Files\" through steam");
            GUILayout.Space(400f);
            if(GUILayout.Button("EXIT GAME"))
            {
                Application.Quit();
            }
        }

        private void BannedMenuFunct(int id)
        {
            PlayerPauseUI.active = true;
            PlayerUI.window.showCursor = true;
            
            GUILayout.Label($"You have been globally banned from using Tsunami Hack");
            GUILayout.Space(8f);
            GUILayout.Label($"This operation was made by AutoBanner");
            GUILayout.Space(20f);
            GUILayout.Label("To remove the hack and play normally, click \"EXIT GAME\" and \"Verify Game Files\" through steam");
            GUILayout.Space(400f);
            if(GUILayout.Button("EXIT GAME"))
            {
                Application.Quit();
            }
        }
    }
}