using System;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Menu;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    internal class ShowEnabled : MonoBehaviour
    {
        public static Menu.Aim AimRef;
        public static Menu.Visuals VisRef;
        public static Menu.Main MainRef;

        public static string OutputString;
        
        public void Start()
        {
            AimRef = WaveMaker.MenuAim;
            VisRef = WaveMaker.MenuVisuals;
            MainRef = WaveMaker.MenuMain;
            
        }

        public void Update()
        {
            OutputString = string.Empty;

            if (AimRef.EnableAimbot || AimRef.EnableAimlock || AimRef.EnableTriggerbot)
            {
                OutputString += "<b><color=#00ffffff>Aim:</color></b>\n";

                if (AimRef.EnableAimbot)
                {
                    var ignwalls = AimRef.AimIgnoreWalls ? "Ignore" : "Recognize";
                    var fov = !AimRef.Aim360 ? AimRef.AimFov.ToString() : "360";
                    
                    var aimingon = "";
                    if (AimRef.AimPlayers)
                        aimingon += "P";
                    if (AimRef.AimZombies)
                    {
                        if(aimingon.Length > 0)
                            aimingon += ",";

                        aimingon += "Z";
                    }
                    if (!AimRef.AimPlayers && !AimRef.AimZombies)
                        aimingon = "None";
                            
                    OutputString += $"<b><color=#00ffffff>Aimbot</color>\n  <color=#c0c0c0ff>[{AimRef.AimTargetLimb}]\n  [{ignwalls} Walls]\n  [Speed {AimRef.AimSpeed}]\n  [Fov {fov}]\n  [Aiming {aimingon}]</color></b>";
                }

                if (AimRef.EnableAimlock)
                {
                    var aimingon = "";
                    if (AimRef.LockPlayers)
                    {
                        aimingon += "P";
                    }    
                    if (AimRef.LockZombies)
                    {
                        if(aimingon.Length > 0)
                            aimingon += ",";

                        aimingon += "Z";
                    }
                    if (aimingon.Length < 1)
                        aimingon = "None";

                    OutputString += $"<b>\n<color=#00ffffff>Aimlock</color>\n  <color=#c0c0c0ff>[Sens {AimRef.LockSensitivity}]\n  [Dist {AimRef.LockDistance}]\n  [Aiming {aimingon}]</color></b>";
                }

                if (AimRef.EnableTriggerbot)
                {
                    var aimingon = "";
                    if (AimRef.TriggerPlayers)
                        aimingon += "P";
                    if (AimRef.TriggerZombies)
                    {
                        if (aimingon.Length > 0)
                            aimingon += ",";

                        aimingon += "Z";
                    }
                    if (aimingon.Length < 1)
                    {
                        aimingon = "None";
                    }
                    
                    OutputString += $"<b>\n<color=#00ffffff>Triggerbot</color>\n  <color=#c0c0c0ff>[Aiming {aimingon}]</color></b>";
                }
                
                
                
            }
            
        }

        public void OnGUI()
        {
            if (Event.current.type == EventType.Repaint && WaveMaker.MenuVisuals.EnableHacksList)
            {
                var rect = new Rect(Screen.width - 150, 50, 200, 700);
                GUI.Label(rect, OutputString);    
            }
            
        }
        
    }
}