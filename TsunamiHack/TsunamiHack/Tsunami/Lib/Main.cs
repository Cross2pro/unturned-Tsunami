using System;
using System.Collections.Generic;
using System.Reflection;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Util;
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
        
        internal static Dictionary<Guid, GunAsset> backups = new Dictionary<Guid, GunAsset>();
        
        
        
        public static void Update()
        {
            CheckRecoil();
            CheckSpread();
        }


        public static void CheckRecoil()
        {
            if (menu.NoRecoil && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = (ItemGunAsset) Player.player.equipment.asset;
                var guid = asset.GUID;
                
                if(backups.ContainsKey(guid) == false)
                    backups.Add(guid, new GunAsset(asset));

                asset.recoilMax_x = 0f;
                asset.recoilMax_y = 0f;
                asset.recoilMin_x = 0f;
                asset.recoilMin_y = 0f;
            }
            else if (!menu.NoRecoil && Player.player.equipment.asset is ItemGunAsset)
            {

                var asset = Player.player.equipment.asset as ItemGunAsset;
                var assetguid = asset.GUID;

                if (backups.ContainsKey(assetguid))
                {
                    var backup = backups[assetguid];
                    
                    asset.recoilMax_x = backup.recoilmaxx;
                    asset.recoilMax_y = backup.recoilmaxy;
                    asset.recoilMin_x = backup.recoilminx;
                    asset.recoilMin_y = backup.recoilminy;

                    backups.Remove(assetguid);
                }
            }
        }

        public static void CheckSpread()
        {
            if (menu.NoSpread && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = (ItemGunAsset) Player.player.equipment.asset;
                var guid = asset.GUID;
                
                if(backups.)
            }
        }
    }
}