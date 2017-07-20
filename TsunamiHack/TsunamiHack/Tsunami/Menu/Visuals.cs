using System;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    internal class Visuals : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        public enum NVType
        {
            Military = 1, Civilian, HeadLamp, None
        }
        
        //TODO: Add item id selection
        
        //Selections

        internal bool PlayerName;
        internal bool PlayerWeapon;//
        internal bool PlayerDistance;

        internal bool ZombieName;
        internal bool ZombieDistance;//
        internal bool ZombieSpecialty;


        internal bool Items;
        internal bool ItemName;
        internal bool ItemDistance;
        
        internal bool Vehicles;
        internal bool VehicleName;
        internal bool VehicleDistance;

        internal bool Animals;
        internal bool AnimalName;
        internal bool AnimalDistance;

        internal bool Storages;
        internal bool StorageType;
        internal bool StorageDistance;

        internal bool Forages;
        internal bool ForageType;
        internal bool ForageDistance;

        internal bool Bed;
        internal bool BedDistance;

        internal bool Doors;
        internal bool DoorType;
        internal bool DoorDistance;

        internal bool Traps;
        internal bool TrapType;
        internal bool TrapDistance;

        internal bool Flag;
        internal bool FlagDistance;

        internal bool Sentries;
        internal bool SentrieDistance;
        internal bool SentrieWeapon;
        internal bool SentrieState;

        internal bool Airdrop;
        internal bool AirdropDistance;

        internal bool Npc;
        internal bool NpcDistance;
        internal bool NpcWeapon;
        internal bool NpcName;

        internal bool Admins;
        
        
        //Type
        internal bool PlayerBox;//
        internal bool ZombieBox;

        internal bool GlowPlayers;
        internal bool GlowZombies;
        internal bool GlowItems;
        internal bool GlowInteractables;
        internal bool GlowVehicles;

        internal bool Chams;
        internal bool Tracers;
        //Env

        internal bool NoRain;//
        internal bool NoSnow;
        internal bool NoFog;
        internal bool NoWater;
        internal NVType Nv;
        internal int NvInt;
        
        //settings
        internal bool InfDistance;
        internal float Distance;
        internal float UpdateRate;

        internal Color PlayerGlow;
        internal Color ZombieGlow;
        internal Color ItemGlow;
        internal Color InteractableGlow;
        internal Color VehicleGlow;

        internal Color BoxPlayer;
        internal Color BoxZombie;

        internal bool ScaleText;
        internal float CloseSize;
        internal float FarSize;
        internal float Dropoff;

        internal bool PlayersOnMap;//
        
        internal Rect SelectionRect;
        internal Rect TypeRect;
        internal Rect EnvRect;
        internal Rect SettingsRect;
        
        public void Start()
        {
            var size = new Vector2(200,700);
            SelectionRect =
                MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            
            SettingsRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.RightMid, MenuTools.Vertical.Center, false);

            
            size = new Vector2(200,300);
            TypeRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.LeftMid, MenuTools.Vertical.Center, false);
            TypeRect.y += 200;

            EnvRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.LeftMid, MenuTools.Vertical.Center, false);
            EnvRect.y -= 400;
            
        }

        public void Update()
        {
        }

        public void OnGUI()
        {
            if (WaveMaker.MenuOpened == WaveMaker.VisualsId)
            {
                SelectionRect = GUI.Window(2004, SelectionRect, MenuFunct, "Selection");
                TypeRect = GUI.Window(2005, TypeRect, TypeFunct, "ESP");
                EnvRect = GUI.Window(2006, EnvRect, EnvFunct, "Environment");
                SettingsRect = GUI.Window(2007, SettingsRect, SetFunct, "Visuals Settings");
            }
        }

        public void MenuFunct(int id)
        {
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nPlayer\n----------------------------------------");
            GUILayout.Space(2f);
            PlayerName = GUILayout.Toggle(PlayerName, " Show Player Name");
            PlayerWeapon = GUILayout.Toggle(PlayerWeapon, " Show Player Weapon");
            PlayerDistance = GUILayout.Toggle(PlayerDistance, " Show Player Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nZombie\n----------------------------------------");
            GUILayout.Space(2f);
            ZombieName = GUILayout.Toggle(ZombieName, " Show Zombie Name");
            ZombieSpecialty = GUILayout.Toggle(ZombieSpecialty, " Show Zombie Specialty");
            ZombieDistance = GUILayout.Toggle(ZombieDistance, " Show Zombie Distance");

            GUILayout.Space(2f);
            GUILayout.Label("----------------------------------------\nItem\n----------------------------------------");
            GUILayout.Space(2f);
            
        }

        public void TypeFunct(int id)
        {
            PlayerBox = GUILayout.Toggle(PlayerBox, " Show Player Box");
        }

        public void EnvFunct(int id)
        {
            NoRain = GUILayout.Toggle(NoRain, " No Rain");
        }

        public void SetFunct(int id)
        {
            PlayersOnMap = GUILayout.Toggle(PlayersOnMap, " Show Players on Map");
        }
        
        
        public void SetMenuStatus(bool setting)
        {
            MenuOpened = setting;
        }

        public void ToggleMenuStatus()
        {
            MenuOpened = !MenuOpened;
        }

        public bool GetMenuStatus()
        {
            return MenuOpened;
        }
    }
}