using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Util;
using UnityEngine;
using UnityEngine.UI;

namespace TsunamiHack.Tsunami.Menu
{
     internal class Aim : MonoBehaviour, IMenuParent
    {
        internal enum TargetLimb
        {
            Head = 1,
            Chest = 2,
            Legs = 3
        };
        
        internal Rect BotRect;
        internal Rect LockRect;
        internal Rect TriggerRect;
        
        

        internal bool EnableAimbot;
        internal bool EnableAimlock;
        internal bool EnableTriggerbot;
        
        
        internal bool AimPlayers;//
        internal bool AimZombies;//
        internal bool AimAnimals;//
        internal bool AimVehicles;//
        internal float AimUpdateRate;//
        internal bool AimClosest;//
        internal bool AimManualChangeTarget;//
        internal float AimSpeed;//
        internal float AimDistance;//
        internal bool AimInfDistance;//
        internal bool AimUseGunDistance;//
        internal bool AimSilent;//
        internal bool AimWhitelistFriends; //
        internal bool AimWhitelistAdmins; //
        internal bool AimWhitelistPlayers;
        internal float AimFOV;//
        internal bool Aim360;//
        internal TargetLimb AimTargetLimb;//
        internal int Limb;//

        internal bool LockPlayers;//
        internal bool LockZombies;//
        internal bool LockAnimals;//
        internal bool LockVehicles;//
        
        internal float LockSensitivity;//
        
        internal float LockDistance; //
        internal bool LockGunRange;//
        
        internal bool LockWhiteListFriends;//
        internal bool LockWhitelistAdmins;//
        internal bool LockWhiteListPlayers;
        
        internal float LockUpdateRate;

        internal bool TriggerPlayers;//
        internal bool TriggerZombies;//
        internal bool TriggerAnimals;//
        internal bool TriggerVehicles;//
        internal bool TriggerWhiteListFriends;//
        internal bool TriggerWhiteListAdmins;//
        internal bool TriggerWhiteListPlayers;
        internal float TriggerDistance;//
        internal bool TriggerGunRange;//
        
        
        public void Start()
        {
            
            var size = new Vector2(200,700);
            BotRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            
            size = new Vector2(200,700);
            LockRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.LeftMid, MenuTools.Vertical.Center, false);
            LockRect.y = BotRect.y;
            LockRect.x += size.x;
            
            size = new Vector2(200,700);
            TriggerRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.RightMid, MenuTools.Vertical.Center, false);
            TriggerRect.y = BotRect.y;
            TriggerRect.x -= size.x;

            Limb = 1;
            AimTargetLimb = (TargetLimb)Limb;

            AimFOV = 180f;
            AimSpeed = 5f;
            AimDistance = 200f;
            AimUpdateRate = 5f;

            LockSensitivity = 5f;
            LockDistance = 200f;
            LockUpdateRate = 5f;

        }

        
        public void Update()
        {
            
        }

        public void OnGUI()
        {
            if (WaveMaker.MenuOpened == WaveMaker.AimId)
            {
                BotRect = GUILayout.Window(2001, BotRect, MenuFunct, "Aimbot");
                LockRect = GUILayout.Window(2002, LockRect, LockMenuFunct, "AimLock");
                TriggerRect = GUILayout.Window(2003, TriggerRect, TriggerMenuFunct, "Triggerbot");
            }
        }
        
        public void MenuFunct(int id)
        {
            GUILayout.Space(2f);
            EnableAimbot = GUILayout.Toggle(EnableAimbot, " Enable Aimbot");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nAiming Selection\n----------------------------------------");
            GUILayout.Space(2f);
            AimPlayers = GUILayout.Toggle(AimPlayers, " Aim at Players");
            AimZombies = GUILayout.Toggle(AimZombies, " Aim at Zombies");
            AimAnimals = GUILayout.Toggle(AimAnimals, " Aim at Animals");
            AimVehicles = GUILayout.Toggle(AimVehicles, " Aim at Vehicles");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nTarget Selection\n----------------------------------------");
            GUILayout.Space(2f);
            AimClosest = GUILayout.Toggle(AimClosest, " Aim Closest Target");
            AimManualChangeTarget = GUILayout.Toggle(AimManualChangeTarget, " Manually Change Target");
            if (GUILayout.Button($"Change Target : {WaveMaker.Keybinds.GetBind("changetarget")}"))
            {
                WaveMaker.MenuKeybind.Changing = true;
                WaveMaker.MenuKeybind.focus = "changetarget"; 
            }
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nWhitelist Settings\n----------------------------------------");
            GUILayout.Space(2f);
            AimWhitelistFriends = GUILayout.Toggle(AimWhitelistFriends, " Whitelist steam Friends");
            AimWhitelistAdmins = GUILayout.Toggle(AimWhitelistAdmins, " Whitelist server Admins");
            AimWhitelistPlayers = GUILayout.Toggle(AimWhitelistPlayers, " Whitelist Friends");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nAimbot Settings\n----------------------------------------");
            GUILayout.Space(2f);
            AimSilent = GUILayout.Toggle(AimSilent, " Enable Silent Aim");
            GUILayout.Space(2f);
            Aim360 = GUILayout.Toggle(Aim360, " 360 Aim FOV");
            GUILayout.Label($"Aim FOV : {AimFOV}");
            AimFOV = GUILayout.HorizontalSlider((float) Math.Round(AimFOV, 0), 10f, 360f);
            GUILayout.Space(2f);
            GUILayout.Label($"Aim Speed : {AimSpeed}");
            AimSpeed = GUILayout.HorizontalSlider((float) Math.Round(AimSpeed, 0), 1f, 10f);
            GUILayout.Space(2f);     
            if (GUILayout.Button($"Target Limb : {AimTargetLimb}"))
            {
                Limb++;
                if (Limb == 4)
                    Limb = 1;
                
                AimTargetLimb = (TargetLimb) Limb;                
            }
            GUILayout.Space(2f);
            AimInfDistance = GUILayout.Toggle(AimInfDistance, " Infinite Aim Distance");
            GUILayout.Label($"Aim Distance: {AimDistance}");
            AimDistance = GUILayout.HorizontalSlider((float) Math.Round(AimDistance, 0), 0f, 1000f);
            GUILayout.Space(2f);
            AimUseGunDistance = GUILayout.Toggle(AimUseGunDistance, " Use Gun Range");
            GUILayout.Space(2f);
            GUILayout.Label($"Aim Update Rate: {AimUpdateRate}");
            AimUpdateRate = GUILayout.HorizontalSlider((float) Math.Round(AimUpdateRate, 0), 0f, 100f);
        }

        public void LockMenuFunct(int id)
        {
            GUILayout.Space(2f);
            EnableAimlock = GUILayout.Toggle(EnableAimlock, "Enable Aimlock");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nLock Selection\n----------------------------------------");
            GUILayout.Space(2f);
            LockPlayers = GUILayout.Toggle(LockPlayers, " Lock Players");
            LockZombies = GUILayout.Toggle(LockZombies, " Lock Zombies");
            LockAnimals = GUILayout.Toggle(LockAnimals, " Lock Animals");
            LockVehicles = GUILayout.Toggle(LockVehicles, " Lock Vehicles");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nWhitelist Settings\n----------------------------------------");
            GUILayout.Space(2f);
            LockWhiteListFriends = GUILayout.Toggle(LockWhiteListFriends, " Whitelist steam Friends");
            LockWhitelistAdmins = GUILayout.Toggle(LockWhitelistAdmins, " Whitelist server Admins");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nAimbot Settings\n----------------------------------------");
            GUILayout.Space(2f);
            
            GUILayout.Label($"Lock Sensitivity : {LockSensitivity}");
            LockSensitivity = GUILayout.HorizontalSlider((float) Math.Round(LockSensitivity, 0), 1f, 10f);
            
            GUILayout.Space(2f);

            LockGunRange = GUILayout.Toggle(LockGunRange, " Use Gun Range");
            
            GUILayout.Label($"Lock Distance : {LockDistance}");
            LockDistance = GUILayout.HorizontalSlider((float) Math.Round(LockDistance, 0), 1f, 1000f);
            
            GUILayout.Space(2f);
            
            GUILayout.Label($"Lock Update Rate: {LockUpdateRate}");
            LockUpdateRate = GUILayout.HorizontalSlider((float) Math.Round(LockUpdateRate, 0), 0f, 100f);
            
        }

        public void TriggerMenuFunct(int id)
        {
            GUILayout.Space(2f);
            EnableTriggerbot = GUILayout.Toggle(EnableTriggerbot, "Enable Triggerbot");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nTrigger Selection\n----------------------------------------");
            GUILayout.Space(2f);
            TriggerPlayers = GUILayout.Toggle(TriggerPlayers, " Trigger Players");
            TriggerZombies = GUILayout.Toggle(TriggerZombies, " Trigger Zombies");
            TriggerAnimals = GUILayout.Toggle(TriggerAnimals, " Trigger Animals");
            TriggerVehicles = GUILayout.Toggle(TriggerVehicles, " Trigger Vehicles");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nWhitelist Settings\n----------------------------------------");
            GUILayout.Space(2f);
            TriggerWhiteListFriends = GUILayout.Toggle(TriggerWhiteListFriends, " Trigger steam Friends");
            TriggerWhiteListAdmins = GUILayout.Toggle(TriggerWhiteListAdmins, " Trigger server Admins");
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nTrigger Settings\n----------------------------------------");
            GUILayout.Space(2f);

            TriggerGunRange = GUILayout.Toggle(TriggerGunRange, "Use Gun Range");
            
            GUILayout.Label($"Trigger Distance : {TriggerDistance}");
            TriggerDistance = GUILayout.HorizontalSlider((float) Math.Round(TriggerDistance, 0), 1f, 1000f);
        }
        
        
        #region 
        public void SetMenuStatus(bool setting)
        {
        }

        public void ToggleMenuStatus()
        {
        }

        public bool GetMenuStatus()
        {
            return false;
        }
        
        #endregion
    }
}