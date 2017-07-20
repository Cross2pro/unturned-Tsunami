﻿using System;
using System.Runtime.InteropServices;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    internal class Blocker : MonoBehaviour
    {
        internal enum Type { Banned, Disabled}
        
        private HackController ctrl;

        private static bool BlockerEnabled;
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
    }
}