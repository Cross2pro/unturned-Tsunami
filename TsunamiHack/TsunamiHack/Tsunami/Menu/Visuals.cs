using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SDG.Unturned;
using TsunamiHack.Tsunami.Lib;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Util;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable InconsistentNaming

namespace TsunamiHack.Tsunami.Menu
{
    internal partial class Visuals : MonoBehaviour
    {

        public VisualsV2 VisualsLib;
        public VisualsPage CurrentPage;
        public IEnumerator CR_ESP;
        
        public enum VisualsPage
        {
            Player,
            Zombie,
            Item,
            Vehicle,
            Animal,
            Storages,
            Environment,
            Filter
        }
    
        public enum ColorOptions
        {
            aqua,
            black,
            blue,
            darkblue,
            magenta,
            green,
            lime,
            maroon,
            navy,
            olive,
            purple,
            red,
            silver,
            teal,
            white,
            yellow
        }

        public Rect MenuBar;
        public Rect CenterMenu;
        public Rect SettingsBar;




        public bool EnableEsp;
        public bool EnableHacksList;

        //Player Page

        public bool Player2DBoxes;
        public bool Player3DBoxes;
        public bool PlayerGlow;
        public bool PlayerSkeleton;
        public bool PlayerTracers;

        public bool PlayerName;
        public bool PlayerWeapon;
        public bool PlayerDistance;
        public bool AdminWarn;

        public ColorOptions FriendlyPlayerColor;
        public ColorOptions EnemyPlayerColor;
        public int FPlayerColorIndex;
        public int EPlayerColorIndex;
        public bool PlayerOverrideDistance;
        public int PlayerEspDistance;
        public bool PlayerInfDistance;

        //Zombie Page

        public bool ZombieBoxes;
        public bool ZombieGlow;
        public bool ZombieSkeleton;
        public bool ZombieTracers;

        public bool ZombieName;
        public bool ZombieType;
        public bool ZombieDistance;

        public ColorOptions ZombieColor;
        public int ZombieColorIndex;
        public bool ZombieOverrideDistance;
        public int ZombieEspDistance;
        public bool ZombieInfDistance;

        //Item Page

        public bool ItemGlow;

        public bool ItemName;
        public bool ItemDistance;
        public bool ItemFilter;

        public ColorOptions ItemColor;
        public int ItemColorIndex;
        public bool ItemOverrideDistance;
        public int ItemEspDistance;
        public bool ItemInfDistance;

        //Item Filter

        public bool FilterClothes;
        public bool FilterPacks;
        public bool FilterAmmo;
        public bool FilterGuns;
        public bool FilterAttach;
        public bool FilterFood;
        public bool FilterMed;
        public bool FilterWeapons;
        public bool FilterMisc;

        //Vehicle Page

        public bool VehicleGlow;

        public bool VehicleName;
        public bool VehicleFilterLocked;
        public bool VehicleGas;
        public bool VehicleHealth;
        public bool VehicleDistance;

        public ColorOptions VehicleColor;
        public int VehicleColorIndex;

        //Animal Page

        public bool AnimalGlow;

        public bool AnimalName;
        public bool AnimalDistance;

        public ColorOptions AnimalColor;
        public int AnimalColorIndex;

        //Storage Page

        public bool StorageGlow;

        public bool StorageName;
        public bool StorageDistance;
        public bool StorageFilterLocked;
        public bool StorageFilterInUse;

        public ColorOptions StorageColor;
        public int StorageColorIndex;

        //Environment Page

        public float Altitude;
        public NvType Nv;
        // Rain, Fog, Water, nv, getting high

        //Settings 

        public bool InfiniteDistance;
        public int EspDistance;
        public int UpdateRate;

        public bool ScaleText;
        public int CloseSize;
        public int FarSize;
        public int Dropoff;


        public void Start()
        {
            //Call Start of lib
            OnStart();
            
            //Set the first page
            CurrentPage = VisualsPage.Player;

            //Enable Hack List by default
            EnableHacksList = true;

            //Set Distances and update rates
            EspDistance = 200;
            UpdateRate = 10;

            //Set Distances for overrides
            PlayerEspDistance = 200;
            ZombieEspDistance = 200;
            ItemEspDistance = 200;

            //Set Text sizes for scale
            CloseSize = 7;
            FarSize = 5;
            Dropoff = 350;

            //Set Sizes and positions of menus
            var size = new Vector2(450, 500);
            CenterMenu = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);

            size = new Vector2(200, 500);
            MenuBar = new Rect(CenterMenu.x - 210, CenterMenu.y, size.x, size.y);
            SettingsBar = new Rect(CenterMenu.x + 460, CenterMenu.y, size.x, size.y);

            //set starting color indexs for colors
            FPlayerColorIndex = 0;
            EPlayerColorIndex = 15;
            ZombieColorIndex = 6;
            ItemColorIndex = 2;
            VehicleColorIndex = 14;
            AnimalColorIndex = 19;
            StorageColorIndex = 5;
        }

        public void Update()
        {
             OnUpdate();
            
        }

        public void OnGUI()
        {
            if (Provider.isConnected && WaveMaker.MenuOpened == WaveMaker.VisualsId)
            {
                MenuBar = GUI.Window(2001, MenuBar, MenuBarFunct, "Visuals");
                CenterMenu = GUI.Window(2002, CenterMenu, CenterMenuFunct, $"{CurrentPage}");
                SettingsBar = GUI.Window(2003, SettingsBar, SettingsBarFunct, "Settings");
            }
            
            OnGUIUpdate();
        }

        private void MenuBarFunct(int id)
        {
            GUILayout.Space(2f);
            EnableEsp = GUILayout.Toggle(EnableEsp, " Enable Esp");
            GUILayout.Space(1f);
            EnableHacksList = GUILayout.Toggle(EnableHacksList, " Enable Hack List");

            GUILayout.Space(4f);
            GUILayout.Label(" Categories\n --------------------------------------");
            GUILayout.Space(2f);

            if (GUILayout.Button(" Player"))
                CurrentPage = VisualsPage.Player;
            GUILayout.Space(1f);

            if (GUILayout.Button(" Zombie"))
                CurrentPage = VisualsPage.Zombie;
            GUILayout.Space(1f);

            if (GUILayout.Button(" Item"))
                CurrentPage = VisualsPage.Item;
            GUILayout.Space(1f);

            if (GUILayout.Button(" Item Filter"))
                CurrentPage = VisualsPage.Filter;
            GUILayout.Space(1f);

            if (GUILayout.Button(" Vehicle"))
                CurrentPage = VisualsPage.Vehicle;
            GUILayout.Space(1f);

            if (GUILayout.Button(" Animal"))
                CurrentPage = VisualsPage.Animal;
            GUILayout.Space(1f);

            if (GUILayout.Button(" Storages"))
                CurrentPage = VisualsPage.Storages;
            GUILayout.Space(1f);

            if (GUILayout.Button(" Environment"))
                CurrentPage = VisualsPage.Environment;
            GUILayout.Space(1f);

        }

        private void CenterMenuFunct(int id)
        {
            switch (CurrentPage)
            {
                case VisualsPage.Player:
                    PlayerPage();
                    break;
                case VisualsPage.Zombie:
                    ZombiePage();
                    break;
                case VisualsPage.Item:
                    ItemPage();
                    break;
                case VisualsPage.Filter:
                    ItemFilterPage();
                    break;
                case VisualsPage.Vehicle:
                    VehiclePage();
                    break;
                case VisualsPage.Animal:
                    AnimalPage(id);
                    break;
                case VisualsPage.Storages:
                    StoragePage(id);
                    break;
                case VisualsPage.Environment:
                    EnvironmentPage(id);
                    break;
                
            }
        }

        private void SettingsBarFunct(int id)
        {
            GUILayout.Space(2f);
            GUILayout.Label($" Esp Distance: {EspDistance}");
            EspDistance = (int) GUILayout.HorizontalSlider(EspDistance, 50f, 10000f);

            InfiniteDistance = GUILayout.Toggle(InfiniteDistance, " Infinite Esp Distance");

            GUILayout.Space(2f);
            GUILayout.Label($" Update Rate : {UpdateRate} ms");
            UpdateRate = (int) GUILayout.HorizontalSlider(UpdateRate, 1f, 15f);

            GUILayout.Space(3f);
            GUILayout.Label(" Text Scale\n --------------------------------------");

            GUILayout.Space(3f);
            ScaleText = GUILayout.Toggle(ScaleText, " Scale Esp Text");

            GUILayout.Space(1f);
            GUILayout.Label($" Close Size: {CloseSize}");
            CloseSize = (int) GUILayout.HorizontalSlider(CloseSize, 1f, 24f);

            GUILayout.Space(1f);
            GUILayout.Label($" Far Size: {FarSize}");
            FarSize = (int) GUILayout.HorizontalSlider(FarSize, 1f, 24f);

            GUILayout.Space(1f);
            GUILayout.Label($" Dropoff: {Dropoff}");
            Dropoff = (int) GUILayout.HorizontalSlider(Dropoff, 50f, 10000f);
        }

        #region pages

        private void PlayerPage()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Box\n --------------------------------------");
            Player2DBoxes = GUILayout.Toggle(Player2DBoxes, " 2D Boxes");
            Player3DBoxes = GUILayout.Toggle(Player3DBoxes, " 3D Boxes");
            
            GUILayout.Label(" Glow\n --------------------------------------");
            PlayerGlow = GUILayout.Toggle(PlayerGlow, " Glow ESP");

            GUILayout.Space(2f);
            GUILayout.Label(" Labels\n --------------------------------------");
            PlayerName = GUILayout.Toggle(PlayerName, " Show Name");
            PlayerWeapon = GUILayout.Toggle(PlayerWeapon, " Show Weapon");
            PlayerDistance = GUILayout.Toggle(PlayerDistance, " Show Distance");

            GUILayout.Space(2f);
            GUILayout.Label(" Other\n --------------------------------------");
            PlayerSkeleton = GUILayout.Toggle(PlayerSkeleton, " Skeleton ESP");
            PlayerTracers = GUILayout.Toggle(PlayerTracers, " Tracers (Beta Only)");
            AdminWarn = GUILayout.Toggle(AdminWarn, " Show Admin Warning");
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Player Colors\n --------------------------------------");
            GUILayout.Space(2f);
            if (GUILayout.Button($" Friendly: {FriendlyPlayerColor.ToString().First().ToString().ToUpper() + FriendlyPlayerColor.ToString().Substring(1)}"))
            {
                FPlayerColorIndex++;
                if (FPlayerColorIndex > 16)
                    FPlayerColorIndex = 0;
                FriendlyPlayerColor = (ColorOptions) FPlayerColorIndex;
            }
            if (GUILayout.Button($" Enemy: {EnemyPlayerColor.ToString().First().ToString().ToUpper() + EnemyPlayerColor.ToString().Substring(1)}"))
            {
                EPlayerColorIndex++;
                if (EPlayerColorIndex > 16)
                    EPlayerColorIndex = 0;
                EnemyPlayerColor = (ColorOptions) EPlayerColorIndex;
            }

            GUILayout.Label(" Override Dist\n --------------------------------------");
            GUILayout.Space(2f);
            PlayerOverrideDistance = GUILayout.Toggle(PlayerOverrideDistance, " Enable Override");
            GUILayout.Space(1f);
            PlayerInfDistance = GUILayout.Toggle(PlayerInfDistance, " Infinite Player Dist");
            GUILayout.Label($" Player Esp Distance: {PlayerEspDistance}");
            PlayerEspDistance = (int) GUILayout.HorizontalSlider(PlayerEspDistance, 50f, 10000f);
            GUILayout.EndVertical();


            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        private void ZombiePage()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Box\n --------------------------------------");
            ZombieBoxes = GUILayout.Toggle(ZombieBoxes, " 2D Boxes");
            
            
            GUILayout.Label(" Glow\n --------------------------------------");
            ZombieGlow = GUILayout.Toggle(ZombieGlow, " Glow ESP");

            GUILayout.Space(2f);
            GUILayout.Label(" Labels\n --------------------------------------");
            ZombieName = GUILayout.Toggle(ZombieName, " Show Name");
            ZombieType = GUILayout.Toggle(ZombieType, " Show Type");
            ZombieDistance = GUILayout.Toggle(ZombieDistance, " Show Distance");

            GUILayout.Space(2f);
            GUILayout.Label(" Other\n --------------------------------------");
            ZombieSkeleton = GUILayout.Toggle(ZombieSkeleton, " Skeleton ESP");
            ZombieTracers = GUILayout.Toggle(ZombieTracers, " Tracers (Beta Only)");
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Zombie Colors\n --------------------------------------");
            GUILayout.Space(2f);
            if (GUILayout.Button($" Zombies: {ZombieColor.ToString().First().ToString().ToUpper() + ZombieColor.ToString().Substring(1)}"))
            {
                ZombieColorIndex++;
                if (ZombieColorIndex > 19)
                    ZombieColorIndex = 0;
                ZombieColor = (ColorOptions) ZombieColorIndex;
            }


            GUILayout.Label(" Override Dist\n --------------------------------------");
            GUILayout.Space(2f);
            ZombieOverrideDistance = GUILayout.Toggle(ZombieOverrideDistance, " Enable Override");
            GUILayout.Space(1f);
            ZombieInfDistance = GUILayout.Toggle(ZombieInfDistance, " Infinite Zombie Dist");
            GUILayout.Label($" Zombie Esp Distance: {ZombieEspDistance}");
            ZombieEspDistance = (int) GUILayout.HorizontalSlider(ZombieEspDistance, 50f, 10000f);
            GUILayout.EndVertical();


            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        private void ItemPage()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Glow\n --------------------------------------");
            ItemGlow = GUILayout.Toggle(ItemGlow, " Glow ESP");

            GUILayout.Space(2f);
            GUILayout.Label(" Labels\n --------------------------------------");
            ItemName = GUILayout.Toggle(ItemName, " Show Name");
            ItemDistance = GUILayout.Toggle(ItemDistance, " Show Distance");

            GUILayout.Space(2f);
            GUILayout.Label(" Other\n --------------------------------------");
            ItemFilter = GUILayout.Toggle(ItemFilter, " Enable Item Filter");

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Item Colors\n --------------------------------------");
            GUILayout.Space(2f);
            if (GUILayout.Button($" Items: {ItemColor.ToString().First().ToString().ToUpper() + ItemColor.ToString().Substring(1)}"))
            {
                ItemColorIndex++;
                if (ItemColorIndex > 19)
                    ItemColorIndex = 0;
                ItemColor = (ColorOptions) ItemColorIndex;
            }


            GUILayout.Label(" Override Dist\n --------------------------------------");
            GUILayout.Space(2f);
            ItemOverrideDistance = GUILayout.Toggle(ItemOverrideDistance, " Enable Override");
            GUILayout.Space(1f);
            ItemInfDistance = GUILayout.Toggle(ItemInfDistance, " Infinite Item Dist");
            GUILayout.Label($" Item Esp Distance: {ItemEspDistance}");
            ItemEspDistance = (int) GUILayout.HorizontalSlider(ItemEspDistance, 50f, 10000f);
            GUILayout.EndVertical();


            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        private void ItemFilterPage()
        {
            GUILayout.Space(2f);
            ItemFilter = GUILayout.Toggle(ItemFilter, " Enable Item Filter");
            GUILayout.Space(2f);
            GUILayout.Label(" Filter Options\n --------------------------------------");
            GUILayout.Space(2f);
            FilterClothes = GUILayout.Toggle(FilterClothes, " Filter Clothes");
            FilterPacks = GUILayout.Toggle(FilterPacks, " Filter Backpacks");
            FilterAmmo = GUILayout.Toggle(FilterAmmo, " Filter Ammunition");
            FilterGuns = GUILayout.Toggle(FilterGuns, " Filter Guns");
            FilterAttach = GUILayout.Toggle(FilterAttach, " Filter Attachments");
            FilterFood = GUILayout.Toggle(FilterFood, " Filter Food");
            FilterMed = GUILayout.Toggle(FilterMed, " Filter Medical");
            FilterWeapons = GUILayout.Toggle(FilterWeapons, " Filter Weapons");
            FilterMisc = GUILayout.Toggle(FilterMisc, " Filter Misc");
        }

        private void VehiclePage()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);

            GUILayout.BeginVertical();
            GUILayout.Space(2f);   
            GUILayout.Label(" Glow\n --------------------------------------");
            VehicleGlow = GUILayout.Toggle(VehicleGlow, " Glow ESP");

            GUILayout.Space(2f);
            GUILayout.Label(" Labels\n --------------------------------------");
            VehicleName = GUILayout.Toggle(VehicleName, " Show Name");
            VehicleDistance = GUILayout.Toggle(VehicleDistance, " Show Distance");
            VehicleGas = GUILayout.Toggle(VehicleGas, " Show Gas");
            VehicleHealth = GUILayout.Toggle(VehicleHealth, " Show Health");

            GUILayout.Space(2f);
            GUILayout.Label(" Other\n --------------------------------------");
            VehicleFilterLocked = GUILayout.Toggle(VehicleFilterLocked, " Show Only Unlocked");

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Vehicle Colors\n --------------------------------------");
            GUILayout.Space(2f);
            if (GUILayout.Button($" Vehicles: {VehicleColor.ToString().First().ToString().ToUpper() + VehicleColor.ToString().Substring(1)}"))
            {
                VehicleColorIndex++;
                if (VehicleColorIndex > 19)
                    VehicleColorIndex = 0;
                VehicleColor = (ColorOptions) VehicleColorIndex;
            }
            GUILayout.EndVertical();


            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        private void AnimalPage(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);

            GUILayout.BeginVertical();
            GUILayout.Space(2f);   
            GUILayout.Label(" Glow\n --------------------------------------");
            AnimalGlow = GUILayout.Toggle(AnimalGlow, " Glow ESP");

            GUILayout.Space(2f);
            GUILayout.Label(" Labels\n --------------------------------------");
            AnimalName = GUILayout.Toggle(AnimalName, " Show Name");
            AnimalDistance = GUILayout.Toggle(AnimalDistance, " Show Distance");

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Animal Colors\n --------------------------------------");
            GUILayout.Space(2f);
            if (GUILayout.Button($" Animals: {AnimalColor.ToString().First().ToString().ToUpper() + AnimalColor.ToString().Substring(1)}"))
            {
                AnimalColorIndex++;
                if (AnimalColorIndex > 19)
                    AnimalColorIndex = 0;
                AnimalColor = (ColorOptions) AnimalColorIndex;
            }
            GUILayout.EndVertical();


            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        private void StoragePage(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(3f);

            GUILayout.BeginVertical();
            GUILayout.Space(2f);   
            GUILayout.Label(" Glow\n --------------------------------------");
            StorageGlow = GUILayout.Toggle(StorageGlow, " Glow ESP");

            GUILayout.Space(2f);
            GUILayout.Label(" Labels\n --------------------------------------");
            StorageName = GUILayout.Toggle(StorageName, " Show Name");
            StorageDistance = GUILayout.Toggle(StorageDistance, " Show Distance");

            GUILayout.Space(2f);
            GUILayout.Label(" Other\n --------------------------------------");
            StorageFilterLocked = GUILayout.Toggle(StorageFilterLocked, " Show Only Unlocked");
            StorageFilterInUse = GUILayout.Toggle(StorageFilterInUse, " Show Only With Items");

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Space(2f);
            GUILayout.Label(" Storage Colors\n --------------------------------------");
            GUILayout.Space(2f);
            if (GUILayout.Button($" Storages: {StorageColor.ToString().First().ToString().ToUpper() + StorageColor.ToString().Substring(1)}"))
            {
                StorageColorIndex++;
                if (StorageColorIndex > 19)
                    StorageColorIndex = 0;
                StorageColor = (ColorOptions) StorageColorIndex;
            }
            GUILayout.EndVertical();


            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        private void EnvironmentPage(int id)
        {
            GUILayout.Space(2f);
            GUILayout.Label("Weather\n--------------------------------------");
            GUILayout.Space(2f);

            if (GUILayout.Button("No Rain"))
            {
                LevelLighting.rainyness = ELightingRain.NONE;
            }

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

            
            GUILayout.Space(4f);

            GUILayout.Label($"Night Vision: {Nv}");
            
            GUILayout.Space(2f);


            if (GUILayout.Button("Military"))
            {
                Nv = NvType.Military;
                
                LevelLighting.vision = ELightingVision.MILITARY;
                LevelLighting.updateLighting();
                LevelLighting.updateLocal();
                PlayerLifeUI.updateGrayscale();
            }

            if (GUILayout.Button("Civilian"))
            {
                Nv = NvType.Civilian;
                
                LevelLighting.vision = ELightingVision.CIVILIAN;
                LevelLighting.updateLighting();
                LevelLighting.updateLocal();
                PlayerLifeUI.updateGrayscale();
            }
            
            if (GUILayout.Button("None"))
            {
                Nv = NvType.None;
                
                LevelLighting.vision = ELightingVision.NONE;
                LevelLighting.updateLighting();
                LevelLighting.updateLocal();
                PlayerLifeUI.updateGrayscale();
            }
            
            GUILayout.Space(10f);
            GUILayout.Label(" Get High\n --------------------------------------");
            GUILayout.Space(2f);
            
            if (GUILayout.Button("Get High (10 sec)"))
            {
                Player.player.life.askView(10);
            }
            if (GUILayout.Button("Get High (30 Sec)"))
            {
                Player.player.life.askView(30);
            }
            if (GUILayout.Button("Get High (1 min)"))
            {
                Player.player.life.askView(60);
            }
            if (GUILayout.Button("Get High (5 mins"))
            {
                Player.player.life.askView(255);
            }
            
        }
        
        #endregion
    }
}
