﻿using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
using SDG.Unturned;
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

        public enum ColorChangeType
        {
            PlayerGlow = 1, ZombieGlow, ItemGlow, InteractableGlow, VehicleGlow, PlayerBox, ZombieBox
        }
        
        //TODO:Create File Saving for colors
        //TODO: Add item id selection
        
        //Selections

        internal bool PlayerName;
        internal bool PlayerWeapon;//
        internal bool PlayerDistance;

        internal bool ZombieName;
        internal bool ZombieDistance;//
        internal bool ZombieSpecialty;


        internal bool Items;
        internal bool ItemName;//
        internal bool ItemDistance;
        
        internal bool Vehicles;
        internal bool VehicleName;//
        internal bool VehicleDistance;

        internal bool Animals;
        internal bool AnimalName;//
        internal bool AnimalDistance;

        internal bool Storages;
        internal bool StorageType;//
        internal bool StorageDistance;

        internal bool Forages;
        internal bool ForageType;//
        internal bool ForageDistance;

        internal bool Bed;//
        internal bool BedDistance;

        internal bool Doors;
        internal bool DoorType;//
        internal bool DoorDistance;

        internal bool Traps;
        internal bool TrapType;//
        internal bool TrapDistance;

        internal bool Flag;
        internal bool FlagDistance;//

        internal bool Sentries;
        internal bool SentryDistance;//
        internal bool SentryWeapon;
        internal bool SentryState;

        internal bool Airdrop;
        internal bool AirdropDistance;//

        internal bool Npc;
        internal bool NpcDistance;
        internal bool NpcWeapon;//
        internal bool NpcName;

        internal bool Admins;//
        
        
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

        internal ColorChangeType Changing;
        internal int ChangeInt;

        internal bool ScaleText;
        internal float CloseSize;
        internal float FarSize;
        internal float Dropoff;

        internal bool PlayersOnMap;//
        
        internal Rect SelectionRect;
        internal Rect Selection2Rect;
        internal Rect TypeRect;
        internal Rect EnvRect;
        internal Rect SettingsRect;
        
        public void Start()
        {
            var size = new Vector2(203,815);
            SelectionRect =
                MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            SelectionRect.x -= 106;

            Selection2Rect =
                MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            Selection2Rect.x += 106;
            
            size = new Vector2(215,815);

            SettingsRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            SettingsRect.x = Selection2Rect.x + 203 + 5;
            
            size = new Vector2(200,398);
            TypeRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            TypeRect.y -= 206;
            TypeRect.x = SelectionRect.x - 207;

            EnvRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            EnvRect.y += 206;
            EnvRect.x = SelectionRect.x - 207;

            NvInt = 4;
            Nv = (NVType) NvInt;

            ChangeInt = 1;
            Changing = (ColorChangeType) ChangeInt;

            Distance = 200f;
            UpdateRate = 10f;

            CloseSize = 5f;
            FarSize = 3f;
            Dropoff = 500f
        }

        public void Update()
        {
        }

        public void OnGUI()
        {
            if (WaveMaker.MenuOpened == WaveMaker.VisualsId)
            {
                SelectionRect = GUI.Window(2004, SelectionRect, MenuFunct, "Selection");
                Selection2Rect = GUI.Window(2008, Selection2Rect, Menu2Funct, "Selection");
                TypeRect = GUI.Window(2005, TypeRect, TypeFunct, "ESP");
                EnvRect = GUI.Window(2006, EnvRect, EnvFunct, "Environment");
                SettingsRect = GUI.Window(2007, SettingsRect, SetFunct, "Visuals Settings");
            }
        }

        public void MenuFunct(int id)
        {
            
            GUILayout.Space(2f);
            GUILayout.Label("Player\n--------------------------------------");
            GUILayout.Space(2f);
            PlayerName = GUILayout.Toggle(PlayerName, " Show Player Name");
            PlayerWeapon = GUILayout.Toggle(PlayerWeapon, " Show Player Weapon");
            PlayerDistance = GUILayout.Toggle(PlayerDistance, " Show Player Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Zombie\n--------------------------------------");
            GUILayout.Space(2f);
            ZombieName = GUILayout.Toggle(ZombieName, " Show Zombie Name");
            ZombieSpecialty = GUILayout.Toggle(ZombieSpecialty, " Show Zombie Specialty");
            ZombieDistance = GUILayout.Toggle(ZombieDistance, " Show Zombie Distance");

            GUILayout.Space(2f);
            GUILayout.Label("NPCs\n--------------------------------------");
            GUILayout.Space(2f);
            Npc = GUILayout.Toggle(Npc, " Show NPCs");
            NpcName = GUILayout.Toggle(NpcName, " Show NPC Name");
            NpcWeapon = GUILayout.Toggle(NpcWeapon, " Show NPC Weapon");
            NpcDistance = GUILayout.Toggle(NpcDistance, "Show NPC Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Item\n--------------------------------------");
            GUILayout.Space(2f);
            Items = GUILayout.Toggle(Items, " Show Items");
            ItemName = GUILayout.Toggle(ItemName, " Show Item Name");
            ItemDistance = GUILayout.Toggle(ItemDistance, " Show Item Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Animals\n--------------------------------------");
            GUILayout.Space(2f);
            Animals = GUILayout.Toggle(Animals, " Show Animals");
            AnimalName = GUILayout.Toggle(AnimalName, " Show Animal Name");
            AnimalDistance = GUILayout.Toggle(AnimalDistance, " Show Animal Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Storages\n--------------------------------------");
            GUILayout.Space(2f);
            Storages = GUILayout.Toggle(Storages, " Show Storages");
            StorageType = GUILayout.Toggle(StorageType, " Show Storage Type");
            StorageDistance = GUILayout.Toggle(StorageDistance, " Show Storage Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Forages\n--------------------------------------");
            GUILayout.Space(2f);
            Forages = GUILayout.Toggle(Forages, " Show Forages");
            ForageType = GUILayout.Toggle(ForageType, " Show Forage Type");
            ForageDistance = GUILayout.Toggle(ForageDistance, " Show Forage Distance");

            
        }

        public void Menu2Funct(int id)
        {
            GUILayout.Space(2f);
            GUILayout.Label("Beds\n--------------------------------------");
            GUILayout.Space(2f);
            Bed = GUILayout.Toggle(Bed, " Show Beds");
            BedDistance = GUILayout.Toggle(BedDistance, " Show Bed Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Doors\n--------------------------------------");
            GUILayout.Space(2f);
            Doors = GUILayout.Toggle(Doors, " Show Doors");
            DoorType = GUILayout.Toggle(DoorType, " Show Door Type");
            DoorDistance = GUILayout.Toggle(DoorDistance, " Show Door Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Traps\n--------------------------------------");
            GUILayout.Space(2f);
            Traps = GUILayout.Toggle(Traps, " Show Traps");
            TrapType = GUILayout.Toggle(TrapType, " Show Trap Type");
            TrapDistance = GUILayout.Toggle(TrapDistance, " Show Trap Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Claim Flags\n--------------------------------------");
            GUILayout.Space(2f);
            Flag = GUILayout.Toggle(Flag, " Show Flags");
            FlagDistance = GUILayout.Toggle(FlagDistance, "Show Flag Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Sentries\n--------------------------------------");
            GUILayout.Space(2f);
            Sentries = GUILayout.Toggle(Sentries, " Show Sentries");
            SentryWeapon = GUILayout.Toggle(SentryWeapon, " Show Sentry Weapon");
            SentryState = GUILayout.Toggle(SentryState, " Show Sentry State");
            SentryDistance = GUILayout.Toggle(SentryDistance, " Show Sentry Distance");
            
            
            GUILayout.Space(2f);
            GUILayout.Label("Airdrops\n--------------------------------------");
            GUILayout.Space(2f);
            Airdrop = GUILayout.Toggle(Airdrop, " Show Airdrops");
            AirdropDistance = GUILayout.Toggle(AirdropDistance, " Show Airdrop Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Admins\n--------------------------------------");
            GUILayout.Space(2f);
            Admins = GUILayout.Toggle(Admins, " Show Admins");
        }

        public void TypeFunct(int id)
        {
            GUILayout.Space(2f);
            GUILayout.Label("Draw Boxes\n--------------------------------------");
            GUILayout.Space(2f);
            PlayerBox = GUILayout.Toggle(PlayerBox, " Show Player Boxes");
            ZombieBox = GUILayout.Toggle(ZombieBox, " Show Zombie Boxes");
            
            GUILayout.Space(2f);
            GUILayout.Label("Glow ESP\n--------------------------------------");
            GUILayout.Space(2f);
            GlowPlayers = GUILayout.Toggle(GlowPlayers, " Show Player Glow");
            GlowZombies = GUILayout.Toggle(GlowZombies, " Show Zombie Glow");
            GlowItems = GUILayout.Toggle(GlowItems, " Show Item Glow");
            GlowInteractables = GUILayout.Toggle(GlowInteractables, " Show Interactables Glow");
            GlowVehicles = GUILayout.Toggle(GlowVehicles, " Show Vehicle Glow");
            
            GUILayout.Space(2f);
            GUILayout.Label("Other Visuals\n--------------------------------------");
            GUILayout.Space(2f);
            Chams = GUILayout.Toggle(Chams, " Enable Chams");
            Tracers = GUILayout.Toggle(Tracers, " Enable Tracers");
        }

        public void EnvFunct(int id)
        {
            GUILayout.Space(2f);
            GUILayout.Label("Weather\n--------------------------------------");
            GUILayout.Space(2f);
            NoRain = GUILayout.Toggle(NoRain, " No Rain");
            NoSnow = GUILayout.Toggle(NoSnow, " No Snow");
            NoFog = GUILayout.Toggle(NoFog, " No Fog");
            NoWater = GUILayout.Toggle(NoWater, " No Water");
            GUILayout.Space(2f);
            GUILayout.Label("--------------------------------------");
            GUILayout.Space(2f);

            PlayersOnMap = GUILayout.Toggle(PlayersOnMap, " Show Players On Map");
            
            GUILayout.Space(2f);

            
            if (GUILayout.Button($"Night Vision : {Nv}"))
            {
                NvInt++;
                if (NvInt == 5)
                    NvInt = 1;
                Nv = (NVType) NvInt;
            }
            
        }

        public void SetFunct(int id)
        {
            GUILayout.Space(2f);
            GUILayout.Label("ESP Settings\n--------------------------------------");
            GUILayout.Space(2f);
            InfDistance = GUILayout.Toggle(InfDistance, " Infinite Distance");
            GUILayout.Space(1f);
            GUILayout.Label($"ESP Distance : {Distance}");
            Distance = GUILayout.HorizontalSlider((float) Math.Round(Distance, 0), 0f, 10000f);
            GUILayout.Space(1f);
            GUILayout.Label($"Update Rate : {UpdateRate}");
            UpdateRate = GUILayout.HorizontalSlider((float) Math.Round(UpdateRate, 0), 1f, 100f);
            
            GUILayout.Space(2f);
            GUILayout.Label("ESP Colors\n--------------------------------------");
            GUILayout.Space(2f);
            GUILayout.Label($"Player : R {PlayerGlow.r} G {PlayerGlow.g} B {PlayerGlow.b}");
           
            GUILayout.Space(3f);
            GUILayout.Label($"Zombie : R {ZombieGlow.r} G {ZombieGlow.g} B {ZombieGlow.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Item : R {ItemGlow.r} G {ItemGlow.g} B {ItemGlow.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Interactable : R {InteractableGlow.r} G {InteractableGlow.g} B {InteractableGlow.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Vehicle : R {VehicleGlow.r} G {VehicleGlow.g} B {VehicleGlow.b}");
            
            GUILayout.Space(5f);
            
            GUILayout.Label($"Player Box : R {BoxPlayer.r} G {BoxPlayer.g} B {BoxPlayer.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Zombie Box : R {BoxZombie.r} G {BoxZombie.g} B {BoxZombie.b}");
            
            GUILayout.Space(2f);
            GUILayout.Label("Edit Colors\n--------------------------------------");
            GUILayout.Space(2f);
            if(GUILayout.Button($"Editing : {Changing}"))
            {
                ChangeInt++;
                if (ChangeInt == 8)
                    ChangeInt = 1;
                Changing = (ColorChangeType) ChangeInt;
            }

            switch (ChangeInt)
            {
                    case 1:
                        GUILayout.Label($"R : {PlayerGlow.r} G : {PlayerGlow.g} B : {PlayerGlow.b}");
                        PlayerGlow.r = GUILayout.HorizontalSlider((float) Math.Round(PlayerGlow.r, 0), 0f, 255f);
                        PlayerGlow.g = GUILayout.HorizontalSlider((float) Math.Round(PlayerGlow.g, 0), 0f, 255f);
                        PlayerGlow.b = GUILayout.HorizontalSlider((float) Math.Round(PlayerGlow.b, 0), 0f, 255f);
                        break;
                    case 2:
                        GUILayout.Label($"R : {ZombieGlow.r} G : {ZombieGlow.r} B : {ZombieGlow.b}");
                        ZombieGlow.r = GUILayout.HorizontalSlider((float) Math.Round(ZombieGlow.r, 0), 0f, 255f);
                        ZombieGlow.g = GUILayout.HorizontalSlider((float) Math.Round(ZombieGlow.g, 0), 0f, 255f);
                        ZombieGlow.b = GUILayout.HorizontalSlider((float) Math.Round(ZombieGlow.b, 0), 0f, 255f);
                        break;
                    case 3:
                        GUILayout.Label($"R : {ItemGlow.r} G : {ItemGlow.r} B : {ItemGlow.b}");
                        ItemGlow.r = GUILayout.HorizontalSlider((float) Math.Round(ItemGlow.r, 0), 0f, 255f);
                        ItemGlow.g = GUILayout.HorizontalSlider((float) Math.Round(ItemGlow.g, 0), 0f, 255f);
                        ItemGlow.b = GUILayout.HorizontalSlider((float) Math.Round(ItemGlow.b, 0), 0f, 255f);
                        break;
                    case 4:
                        GUILayout.Label($"R : {InteractableGlow.r} G : {InteractableGlow.r} B : {InteractableGlow.b}");
                        InteractableGlow.r = GUILayout.HorizontalSlider((float) Math.Round(InteractableGlow.r, 0), 0f, 255f);
                        InteractableGlow.g = GUILayout.HorizontalSlider((float) Math.Round(InteractableGlow.g, 0), 0f, 255f);
                        InteractableGlow.b = GUILayout.HorizontalSlider((float) Math.Round(InteractableGlow.b, 0), 0f, 255f);
                        break;
                    case 5:
                        GUILayout.Label($"R : {VehicleGlow.r} G : {VehicleGlow.r} B : {VehicleGlow.b}");
                        VehicleGlow.r = GUILayout.HorizontalSlider((float) Math.Round(VehicleGlow.r, 0), 0f, 255f);
                        VehicleGlow.g = GUILayout.HorizontalSlider((float) Math.Round(VehicleGlow.g, 0), 0f, 255f);
                        VehicleGlow.b = GUILayout.HorizontalSlider((float) Math.Round(VehicleGlow.b, 0), 0f, 255f);
                        break;
                    case 6:
                        GUILayout.Label($"R : {BoxPlayer.r} G : {BoxPlayer.r} B : {BoxPlayer.b}");
                        BoxPlayer.r = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayer.r, 0), 0f, 255f);
                        BoxPlayer.g = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayer.g, 0), 0f, 255f);
                        BoxPlayer.b = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayer.b, 0), 0f, 255f);
                        break;
                    case 7:
                        GUILayout.Label($"R : {BoxZombie.r} G : {BoxZombie.r} B : {BoxZombie.b}");
                        BoxZombie.r = GUILayout.HorizontalSlider((float) Math.Round(BoxZombie.r, 0), 0f, 255f);
                        BoxZombie.g = GUILayout.HorizontalSlider((float) Math.Round(BoxZombie.g, 0), 0f, 255f);
                        BoxZombie.b = GUILayout.HorizontalSlider((float) Math.Round(BoxZombie.b, 0), 0f, 255f);
                        break;
            }

            GUILayout.Label("--------------------------------------");
            
            ScaleText = GUILayout.Toggle(ScaleText, " Scale ESP Text");
            GUILayout.Label($"Close Text Size : {CloseSize}");
            CloseSize = GUILayout.HorizontalSlider((float) Math.Round(CloseSize, 0), 1f, 12f);
            GUILayout.Label($"Far Text Size : {FarSize}");
            FarSize = GUILayout.HorizontalSlider((float) Math.Round(CloseSize, 0), 1f, 12f);
            GUILayout.Label($"Text Size Dropoff : {Dropoff}");
            Dropoff = GUILayout.HorizontalSlider((float) Math.Round(CloseSize, 0), 1f, 10000f);


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