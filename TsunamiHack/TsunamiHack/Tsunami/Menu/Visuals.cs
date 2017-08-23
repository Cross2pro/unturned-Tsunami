using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Configs;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    internal class Visuals : MonoBehaviour
    {
        internal bool MenuOpened { get; private set; }
        internal enum NVType
        {
            Military = 1, Civilian, HeadLamp, None
        }
        
        
        
        //TODO: Add item id selection
        
        //Selections

        internal bool Esp;

        internal bool EnableEsp;
        
        internal bool PlayerName;
        internal bool PlayerWeapon;//
        internal bool PlayerDistance;

        internal bool ZombieName;
        internal bool ZombieDistance;//
        internal bool ZombieSpecialty;

        internal bool ItemName;//
        internal bool ItemDistance;
        
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
        internal bool BedType;
        internal bool BedDistance;

        internal bool Doors;
        internal bool DoorType;//
        internal bool DoorDistance;

        internal bool Traps;
        internal bool TrapType;//
        internal bool TrapDistance;

        internal bool Flag;
        internal bool FlagType;
        internal bool FlagDistance;//

        internal bool Sentries;
        internal bool SentryType;
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
        internal float Altitude;
        
        //settings
        internal bool InfDistance;
        internal float Distance;
        internal float UpdateRate;

        internal Color EnemyPlayerGlow;
        internal Color FriendlyPlayerGlow;
        internal Color ZombieGlow;
        internal Color ItemGlow;
        internal Color InteractableGlow;
        internal Color VehicleGlow;

        internal Color BoxPlayerFriendly;
        internal Color BoxPlayerEnemy;
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

        internal Vector2 ScrollPos;
        internal Vector2 ScrollPos2;
        internal Vector2 ScrollPos3;
        
        public void Start()
        {
//            Lib.Visuals.Start(this);
            Lib.VisualsV2.Start();
            
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
            Dropoff = 500f;

            ScrollPos = new Vector2();

            EnemyPlayerGlow = WaveMaker.Settings.ColorList["enemyplayer"].Convert();
            FriendlyPlayerGlow = WaveMaker.Settings.ColorList["friendlyplayer"].Convert();
            ZombieGlow = WaveMaker.Settings.ColorList["zombie"].Convert();
            ItemGlow = WaveMaker.Settings.ColorList["item"].Convert();
            InteractableGlow = WaveMaker.Settings.ColorList["interactable"].Convert();
            VehicleGlow = WaveMaker.Settings.ColorList["vehicle"].Convert(); 

            BoxPlayerFriendly = WaveMaker.Settings.ColorList["friendlyplayerbox"].Convert();
            BoxPlayerEnemy = WaveMaker.Settings.ColorList["enemyplayerbox"].Convert();
            BoxZombie = WaveMaker.Settings.ColorList["zombiebox"].Convert();
            
        }

        public void UpdateColors()
        {
            EnemyPlayerGlow = WaveMaker.Settings.ColorList["enemyplayer"].Convert();
            FriendlyPlayerGlow = WaveMaker.Settings.ColorList["friendlyplayer"].Convert();
            ZombieGlow = WaveMaker.Settings.ColorList["zombie"].Convert();
            ItemGlow = WaveMaker.Settings.ColorList["item"].Convert();
            InteractableGlow = WaveMaker.Settings.ColorList["interactable"].Convert();
            VehicleGlow = WaveMaker.Settings.ColorList["vehicle"].Convert(); 

            BoxPlayerFriendly = WaveMaker.Settings.ColorList["friendlyplayerbox"].Convert();
            BoxPlayerEnemy = WaveMaker.Settings.ColorList["enemyplayerbox"].Convert();
            BoxZombie = WaveMaker.Settings.ColorList["zombiebox"].Convert();
        }
        
        public void Update()
        {
//            if(Provider.isConnected)
//                Lib.Visuals.Update();
                
            Lib.VisualsV2.Update();
        }

        public void OnGUI()
        {
            if (WaveMaker.MenuOpened == WaveMaker.VisualsId)
            {
                SelectionRect = GUI.Window(2004, SelectionRect, MenuFunct, "");
                Selection2Rect = GUI.Window(2008, Selection2Rect, Menu2Funct, "");
                TypeRect = GUI.Window(2005, TypeRect, TypeFunct, "Visuals");
                EnvRect = GUI.Window(2006, EnvRect, EnvFunct, "Environment Hacks");
                SettingsRect = GUI.Window(2007, SettingsRect, SetFunct, "Settings");
            }
            
//            if(Provider.isConnected)
//                Lib.Visuals.OnGUI();
//TODO: remove this comment once labels have been created in v2
        }

        public void MenuFunct(int id)
        {
            ScrollPos2 = GUILayout.BeginScrollView(ScrollPos2, false, true);
            
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
            GUILayout.Label("Item\n--------------------------------------");
            GUILayout.Space(2f);
            ItemName = GUILayout.Toggle(ItemName, " Show Item Name");
            ItemDistance = GUILayout.Toggle(ItemDistance, " Show Item Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Vehicles\n--------------------------------------");
            GUILayout.Space(2f);
            VehicleName = GUILayout.Toggle(VehicleName, " Show Vehicle Name");
            VehicleDistance = GUILayout.Toggle(VehicleDistance, " Show Vehicle Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Animals\n--------------------------------------");
            GUILayout.Space(2f);
            Animals = GUILayout.Toggle(Animals, " Show Animal Glow");
            AnimalName = GUILayout.Toggle(AnimalName, " Show Animal Name");
            AnimalDistance = GUILayout.Toggle(AnimalDistance, " Show Animal Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Forages\n--------------------------------------");
            GUILayout.Space(2f);
            Forages = GUILayout.Toggle(Forages, " Show Forage Glow");
//            ForageType = GUILayout.Toggle(ForageType, " Show Forage Type");                //FIX
//            ForageDistance = GUILayout.Toggle(ForageDistance, " Show Forage Distance");    //FIX

            GUILayout.Space(2f);
            GUILayout.Label("Airdrops\n--------------------------------------");
            GUILayout.Space(2f);
            Airdrop = GUILayout.Toggle(Airdrop, " Show Airdrop Glow");
            AirdropDistance = GUILayout.Toggle(AirdropDistance, " Show Airdrop Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("NPCs\n--------------------------------------");
            GUILayout.Space(2f);
            Npc = GUILayout.Toggle(Npc, " Show NPC Glow");
            NpcName = GUILayout.Toggle(NpcName, " Show NPC Name");
//            NpcWeapon = GUILayout.Toggle(NpcWeapon, " Show NPC Weapon");     FIX
            NpcDistance = GUILayout.Toggle(NpcDistance, "Show NPC Distance");
            
            GUILayout.EndScrollView();
        }

        public void Menu2Funct(int id)
        {
            GUILayout.Space(2f);
            GUILayout.Label("Beds\n--------------------------------------");
            GUILayout.Space(2f);
            Bed = GUILayout.Toggle(Bed, " Show Bed Glow");
            BedType = GUILayout.Toggle(BedType, " Show Bed Name");
            BedDistance = GUILayout.Toggle(BedDistance, " Show Bed Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Doors\n--------------------------------------");
            GUILayout.Space(2f);
            Doors = GUILayout.Toggle(Doors, " Show Door Glow");
            DoorType = GUILayout.Toggle(DoorType, " Show Door Type");
            DoorDistance = GUILayout.Toggle(DoorDistance, " Show Door Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Traps\n--------------------------------------");
            GUILayout.Space(2f);
            Traps = GUILayout.Toggle(Traps, " Show Trap Glow");
            TrapType = GUILayout.Toggle(TrapType, " Show Trap Type");
            TrapDistance = GUILayout.Toggle(TrapDistance, " Show Trap Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Claim Flags\n--------------------------------------");
            GUILayout.Space(2f);
            Flag = GUILayout.Toggle(Flag, " Show Flag Glow");
            FlagType = GUILayout.Toggle(FlagType, " Show Flag Type");
            FlagDistance = GUILayout.Toggle(FlagDistance, "Show Flag Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Sentries\n--------------------------------------");
            GUILayout.Space(2f);
            Sentries = GUILayout.Toggle(Sentries, " Show Sentrie Glow");
            SentryType = GUILayout.Toggle(SentryType, " Show Sentry Type");
//            SentryWeapon = GUILayout.Toggle(SentryWeapon, " Show Sentry Weapon"); //FIX
            SentryState = GUILayout.Toggle(SentryState, " Show Sentry State");
            SentryDistance = GUILayout.Toggle(SentryDistance, " Show Sentry Distance");
            
            GUILayout.Space(2f);
            GUILayout.Label("Storages\n--------------------------------------");
            GUILayout.Space(2f);
            Storages = GUILayout.Toggle(Storages, " Show Storage Glow");
            StorageType = GUILayout.Toggle(StorageType, " Show Storage Type");
            StorageDistance = GUILayout.Toggle(StorageDistance, " Show Storage Distance");
            
            
            GUILayout.Space(2f);
            GUILayout.Label("Admins\n--------------------------------------");
            GUILayout.Space(2f);
            Admins = GUILayout.Toggle(Admins, " Show Admins");
        }

        public void TypeFunct(int id)
        {
            GUILayout.Space(2f);

            EnableEsp = GUILayout.Toggle(EnableEsp, " Enable Esp");
            
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
//            GlowInteractables = GUILayout.Toggle(GlowInteractables, " Show Interactables Glow");
            GlowVehicles = GUILayout.Toggle(GlowVehicles, " Show Vehicle Glow");
            
            GUILayout.Space(2f);
            GUILayout.Label("Other Visuals\n--------------------------------------");
            GUILayout.Space(2f);
            Chams = GUILayout.Toggle(Chams, " Chams (Coming soon)");
            Tracers = GUILayout.Toggle(Tracers, " Tracers (Coming soon)");
        }

        public void EnvFunct(int id)
        {
            
            ScrollPos3 = GUILayout.BeginScrollView(ScrollPos3, false, true);
            GUILayout.Space(2f);
            GUILayout.Label("Weather\n--------------------------------------");
            GUILayout.Space(2f);

            if (GUILayout.Button("No Rain"))
            {
                LevelLighting.rainyness = ELightingRain.NONE;
            }

//            if (GUILayout.Button("No Snow"))
//            {
//                LevelLighting.snowLevel = 0f;
//                RenderSettings.fogDensity = 0f;
//                FieldInfo issnow = typeof(LevelLighting).GetField("isSnow", BindingFlags.Static | BindingFlags.NonPublic);
//                if (issnow != null)
//                {
//                    issnow.SetValue(null, false);
//                }
//                FieldInfo snowy = typeof(LevelLighting).GetField("snownyess", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//                if (snowy == null)
//                {
//                    return;
//                }
//                snowy.SetValue(null, 0f);
//            }

            if (GUILayout.Button("No Fog"))
            {
                RenderSettings.fog =(!RenderSettings.fog);
            }

            if (GUILayout.Button("No Water"))
            {
                if (Altitude == 0f)
                    Altitude = LevelLighting.seaLevel;
                
                LevelLighting.seaLevel = LevelLighting.seaLevel == 0f ? Altitude : 0f;
            }
            
            GUILayout.Space(2f);
            GUILayout.Label("--------------------------------------");
            GUILayout.Space(2f);

//            PlayersOnMap = GUILayout.Toggle(PlayersOnMap, " Show Players On Map");
            
            GUILayout.Space(4f);

            GUILayout.Label($"Night Vision: {Nv}");
            
            GUILayout.Space(2f);


            if (GUILayout.Button("Military"))
            {
                if(Nv != NVType.Military)
                    Nv = (NVType) 1;
                
                LevelLighting.vision = ELightingVision.MILITARY;
                LevelLighting.updateLighting();
                LevelLighting.updateLocal();
                PlayerLifeUI.updateGrayscale();
            }

            if (GUILayout.Button("Civilian"))
            {
                if(Nv != NVType.Civilian)
                    Nv = (NVType) 2;
                
                LevelLighting.vision = ELightingVision.CIVILIAN;
                LevelLighting.updateLighting();
                LevelLighting.updateLocal();
                PlayerLifeUI.updateGrayscale();
            }
            
            if (GUILayout.Button("None"))
            {
                if(Nv != NVType.None)
                    Nv = (NVType) 4;
                
                LevelLighting.vision = ELightingVision.NONE;
                LevelLighting.updateLighting();
                LevelLighting.updateLocal();
                PlayerLifeUI.updateGrayscale();
            }
            
            GUILayout.Space(10f);
            GUILayout.Label("Having too many categories selected at once will cause you to lag. For the best experience only select \"Show\" for those items you are specifically looking for");
            GUILayout.EndScrollView();
        }

        public void SetFunct(int id)
        {
            ScrollPos = GUILayout.BeginScrollView(ScrollPos, false, true);
            
            GUILayout.Space(2f);
            GUILayout.Label("ESP Settings\n--------------------------------------");
            GUILayout.Space(2f);
            InfDistance = GUILayout.Toggle(InfDistance, " Infinite Distance");
            GUILayout.Space(1f);
            GUILayout.Label($"ESP Distance : {Distance}");
            Distance = GUILayout.HorizontalSlider((float) Math.Round(Distance, 0), 0f, 1000f);
            GUILayout.Space(1f);
            GUILayout.Label($"Update Rate : {UpdateRate}");
            UpdateRate = GUILayout.HorizontalSlider((float) Math.Round(UpdateRate, 0), 1f, 100f);
            
            GUILayout.Space(2f);
            GUILayout.Label("ESP Colors\n--------------------------------------");
            GUILayout.Space(2f);
            GUILayout.Label($"Enemy Player :\n R {EnemyPlayerGlow.r} G {EnemyPlayerGlow.g} B {EnemyPlayerGlow.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Friendly Player :\n R {FriendlyPlayerGlow.r} G {FriendlyPlayerGlow.g} B {FriendlyPlayerGlow.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Zombie :\n R {ZombieGlow.r} G {ZombieGlow.g} B {ZombieGlow.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Item :\n R {ItemGlow.r} G {ItemGlow.g} B {ItemGlow.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Interactable :\n R {InteractableGlow.r} G {InteractableGlow.g} B {InteractableGlow.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Vehicle :\n R {VehicleGlow.r} G {VehicleGlow.g} B {VehicleGlow.b}");
            
            GUILayout.Space(5f);
            
            GUILayout.Label($"Friendly Player Box :\n R {BoxPlayerFriendly.r} G {BoxPlayerFriendly.g} B {BoxPlayerFriendly.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Enemy Player Box :\n R {BoxPlayerEnemy.r} G {BoxPlayerEnemy.g} B {BoxPlayerEnemy.b}");
            GUILayout.Space(3f);
            GUILayout.Label($"Zombie Box :\n R {BoxZombie.r} G {BoxZombie.g} B {BoxZombie.b}");
            
            GUILayout.Space(4f);
            GUILayout.Label("Edit Colors\n--------------------------------------");
            GUILayout.Space(2f);
            if(GUILayout.Button($"Editing : {Changing}"))
            {
                ChangeInt++;
                if (ChangeInt == 10)
                    ChangeInt = 1;
                Changing = (ColorChangeType) ChangeInt;
            }

            switch (ChangeInt)
            {
                    case 1:
                        GUILayout.Label($"R : {EnemyPlayerGlow.r} G : {EnemyPlayerGlow.g} B : {EnemyPlayerGlow.b}");
                        EnemyPlayerGlow.r = GUILayout.HorizontalSlider((float) Math.Round(EnemyPlayerGlow.r, 0), 0f, 255f);
                        EnemyPlayerGlow.g = GUILayout.HorizontalSlider((float) Math.Round(EnemyPlayerGlow.g, 0), 0f, 255f);
                        EnemyPlayerGlow.b = GUILayout.HorizontalSlider((float) Math.Round(EnemyPlayerGlow.b, 0), 0f, 255f);
                        break;
                    case 2:
                        GUILayout.Label($"R : {FriendlyPlayerGlow.r} G : {FriendlyPlayerGlow.g} B : {FriendlyPlayerGlow.b}");
                        FriendlyPlayerGlow.r = GUILayout.HorizontalSlider((float) Math.Round(FriendlyPlayerGlow.r, 0), 0f, 255f);
                        FriendlyPlayerGlow.g = GUILayout.HorizontalSlider((float) Math.Round(FriendlyPlayerGlow.g, 0), 0f, 255f);
                        FriendlyPlayerGlow.b = GUILayout.HorizontalSlider((float) Math.Round(FriendlyPlayerGlow.b, 0), 0f, 255f);
                        break;
                    case 3:
                        GUILayout.Label($"R : {ZombieGlow.r} G : {ZombieGlow.g} B : {ZombieGlow.b}");
                        ZombieGlow.r = GUILayout.HorizontalSlider((float) Math.Round(ZombieGlow.r, 0), 0f, 255f);
                        ZombieGlow.g = GUILayout.HorizontalSlider((float) Math.Round(ZombieGlow.g, 0), 0f, 255f);
                        ZombieGlow.b = GUILayout.HorizontalSlider((float) Math.Round(ZombieGlow.b, 0), 0f, 255f);
                        break;
                    case 4:
                        GUILayout.Label($"R : {ItemGlow.r} G : {ItemGlow.g} B : {ItemGlow.b}");
                        ItemGlow.r = GUILayout.HorizontalSlider((float) Math.Round(ItemGlow.r, 0), 0f, 255f);
                        ItemGlow.g = GUILayout.HorizontalSlider((float) Math.Round(ItemGlow.g, 0), 0f, 255f);
                        ItemGlow.b = GUILayout.HorizontalSlider((float) Math.Round(ItemGlow.b, 0), 0f, 255f);
                        break;
                    case 5:
                        GUILayout.Label($"R : {InteractableGlow.r} G : {InteractableGlow.g} B : {InteractableGlow.b}");
                        InteractableGlow.r = GUILayout.HorizontalSlider((float) Math.Round(InteractableGlow.r, 0), 0f, 255f);
                        InteractableGlow.g = GUILayout.HorizontalSlider((float) Math.Round(InteractableGlow.g, 0), 0f, 255f);
                        InteractableGlow.b = GUILayout.HorizontalSlider((float) Math.Round(InteractableGlow.b, 0), 0f, 255f);
                        break;
                    case 6:
                        GUILayout.Label($"R : {VehicleGlow.r} G : {VehicleGlow.g} B : {VehicleGlow.b}");
                        VehicleGlow.r = GUILayout.HorizontalSlider((float) Math.Round(VehicleGlow.r, 0), 0f, 255f);
                        VehicleGlow.g = GUILayout.HorizontalSlider((float) Math.Round(VehicleGlow.g, 0), 0f, 255f);
                        VehicleGlow.b = GUILayout.HorizontalSlider((float) Math.Round(VehicleGlow.b, 0), 0f, 255f);
                        break;
                    case 7:
                        GUILayout.Label($"R : {BoxPlayerFriendly.r} G : {BoxPlayerFriendly.g} B : {BoxPlayerFriendly.b}");
                        BoxPlayerFriendly.r = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayerFriendly.r, 0), 0f, 255f);
                        BoxPlayerFriendly.g = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayerFriendly.g, 0), 0f, 255f);
                        BoxPlayerFriendly.b = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayerFriendly.b, 0), 0f, 255f);
                        break;
                    case 8:
                        GUILayout.Label($"R : {BoxPlayerEnemy.r} G : {BoxPlayerEnemy.g} B : {BoxPlayerEnemy.b}");
                        BoxPlayerEnemy.r = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayerEnemy.r, 0), 0f, 255f);
                        BoxPlayerEnemy.g = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayerEnemy.g, 0), 0f, 255f);
                        BoxPlayerEnemy .b = GUILayout.HorizontalSlider((float) Math.Round(BoxPlayerEnemy.b, 0), 0f, 255f);
                        break;
                    case 9:
                        GUILayout.Label($"R : {BoxZombie.r} G : {BoxZombie.g} B : {BoxZombie.b}");
                        BoxZombie.r = GUILayout.HorizontalSlider((float) Math.Round(BoxZombie.r, 0), 0f, 255f);
                        BoxZombie.g = GUILayout.HorizontalSlider((float) Math.Round(BoxZombie.g, 0), 0f, 255f);
                        BoxZombie.b = GUILayout.HorizontalSlider((float) Math.Round(BoxZombie.b, 0), 0f, 255f);
                        break;
            }
            
            GUILayout.Space(2f);
            
            if(GUILayout.Button("Save Colors"))
            {
                WaveMaker.Settings.ColorList["enemyplayer"] = new TsuColor(EnemyPlayerGlow);
                WaveMaker.Settings.ColorList["friendlyplayer"] = new TsuColor(FriendlyPlayerGlow);
                WaveMaker.Settings.ColorList["zombie"] = new TsuColor(ZombieGlow);
                WaveMaker.Settings.ColorList["item"] = new TsuColor(ItemGlow);
                WaveMaker.Settings.ColorList["interactable"] = new TsuColor(InteractableGlow);
                WaveMaker.Settings.ColorList["vehicle"] = new TsuColor(VehicleGlow);
                
                WaveMaker.Settings.ColorList["friendlyplayerbox"] = new TsuColor(BoxPlayerFriendly);
                WaveMaker.Settings.ColorList["enemyplayerbox"] = new TsuColor(BoxPlayerEnemy);
                WaveMaker.Settings.ColorList["zombiebox"] = new TsuColor(BoxZombie);


                FileIo.SaveColors(WaveMaker.Settings);
            }
            GUILayout.Space(2f);
            if (GUILayout.Button("Reset Colors"))
            {
                WaveMaker.Settings.ColorList["enemyplayer"] = new TsuColor(new Color(255,45,45));
                WaveMaker.Settings.ColorList["friendlyplayer"] = new TsuColor(new Color(150,255,255));
                WaveMaker.Settings.ColorList["zombie"] = new TsuColor(new Color(50,150,0));
                WaveMaker.Settings.ColorList["item"] = new TsuColor(new Color(230,230,40));
                WaveMaker.Settings.ColorList["interactable"] = new TsuColor(new Color(255,180,0));
                WaveMaker.Settings.ColorList["vehicle"] = new TsuColor(new Color(255,0,230));
                
                WaveMaker.Settings.ColorList["friendlyplayerbox"] = new TsuColor(new Color(150,255,255));
                WaveMaker.Settings.ColorList["enemyplayerbox"] = new TsuColor(new Color(255,45,45));
                WaveMaker.Settings.ColorList["zombiebox"] = new TsuColor(new Color(50, 150, 0));
                
                UpdateColors();
                FileIo.SaveColors(WaveMaker.Settings);
            }

            GUILayout.Label("--------------------------------------");
            
            ScaleText = GUILayout.Toggle(ScaleText, " Scale ESP Text");
            GUILayout.Label($"Close Text Size : {CloseSize}");
            CloseSize = GUILayout.HorizontalSlider((float) Math.Round(CloseSize, 0), 1f, 12f);
            GUILayout.Label($"Far Text Size : {FarSize}");
            FarSize = GUILayout.HorizontalSlider((float) Math.Round(FarSize, 0), 1f, 12f);
            GUILayout.Label($"Text Size Dropoff : {Dropoff}");
            Dropoff = GUILayout.HorizontalSlider((float) Math.Round(Dropoff, 0), 1f, 10000f);
            
            GUILayout.EndScrollView();
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