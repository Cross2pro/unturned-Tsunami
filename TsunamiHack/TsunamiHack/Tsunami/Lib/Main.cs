using System;
using System.Collections.Generic;
using System.Reflection;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Main
    {
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
        
        internal static List<ItemGunAsset> backuplist = new List<ItemGunAsset>(); 
        
        public static void Update()
        {

            if (menu.NoRecoil && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = Player.player.equipment.asset as ItemGunAsset;

                if (!backuplist.Contains(asset))
                {
                    backuplist.Add(asset);
                }

                asset.recoilMax_x = 0f;
                asset.recoilMax_y = 0f;
                asset.recoilMin_x = 0f;
                asset.recoilMin_y = 0f;
            }
            else
            {
                if (Player.player.equipment.asset is ItemGunAsset)
                {
                    if(Array.Exists(backuplist.ToArray(), asset => asset.item == (Player.player.equipment.asset as ItemGunAsset).item))
                    {
                        var gun = Array.Find(backuplist.ToArray(),
                            asset => asset.item == (Player.player.equipment.asset as ItemGunAsset).item);

                        if (gun != null)
                        {
                            var current = Player.player.equipment.asset as ItemGunAsset;

                            current.recoilMax_x = gun.recoilMax_x;
                            current.recoilMax_y = gun.recoilMax_y;
                            current.recoilMin_x = gun.recoilMin_x;
                            current.recoilMin_y = gun.recoilMin_y;
                        }
                    }
                }
                
            }
            
        }
    }
}