using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Main
    {
        internal static Dictionary<string, ItemGunAsset> Backups = new Dictionary<string, ItemGunAsset>();
        internal static FieldInfo spreadAim;
        internal static FieldInfo spreadHip;
        internal static FieldInfo ballisticForce;
        internal static FieldInfo ballisticDrop;

        internal static Menu.Main menu;
        
        public static void Start()
        {
            menu = WaveMaker.MenuMain;
            
            spreadAim = typeof(ItemGunAsset).GetField("_spreadAim", BindingFlags.Instance | BindingFlags.NonPublic);
            spreadHip = typeof(ItemGunAsset).GetField("_spreadHip", BindingFlags.Instance | BindingFlags.NonPublic);
            ballisticForce = typeof(ItemGunAsset).GetField("_ballisticForce", BindingFlags.Instance | BindingFlags.NonPublic);
            ballisticDrop = typeof(ItemGunAsset).GetField("_ballisticDrop", BindingFlags.Instance | BindingFlags.NonPublic);
        }
        
        public static void Update()
        {
            if (menu.NoRecoil && Player.player.equipment.asset is ItemGunAsset)
            {
                if(!Backups.ContainsKey(Player.player.equipment.asset.itemName))
                    Backups.Add(Player.player.equipment.asset.itemName, Player.player.equipment.asset as ItemGunAsset);
                
                var asset = Player.player.equipment.asset as ItemGunAsset;

                asset.recoilMax_x = 0f;
                asset.recoilMax_y = 0f;
                asset.recoilMin_x = 0f;
                asset.recoilMin_y = 0f;
            }
            if (!menu.NoRecoil && Backups.ContainsKey(Player.player.equipment.asset.itemName))
            {
                var asset = Player.player.equipment.asset as ItemGunAsset;
                var backupasset = Backups[Player.player.equipment.asset.itemName];

                asset.recoilMax_x = backupasset.recoilMax_x;
                asset.recoilMax_y = backupasset.recoilMax_y;
                asset.recoilMin_x = backupasset.recoilMin_x;
                asset.recoilMin_y = backupasset.recoilMin_y;
            }
            
            
        }
    }
}