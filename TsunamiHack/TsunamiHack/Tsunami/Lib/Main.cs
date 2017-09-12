using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Main
    {
        internal static Dictionary<Guid, GunAsset> backupsrecoil = new Dictionary<Guid, GunAsset>();
        internal static Dictionary<Guid, GunAsset> backupsspread = new Dictionary<Guid, GunAsset>();
        internal static Dictionary<Guid, GunAsset> backupsshake = new Dictionary<Guid, GunAsset>();
        internal static Dictionary<Guid, GunAsset> backupsdrop = new Dictionary<Guid, GunAsset>();

        internal static Menu.Main menu;
        
        public static void Start()
        {
            menu = WaveMaker.MenuMain;              
        }
        
        public static void Update()
        {
            CheckRecoil();
            CheckSpread();
            CheckShake();
        }

        public static void CheckRecoil()
        {
            if (menu.NoRecoil && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = (ItemGunAsset) Player.player.equipment.asset;
                var guid = asset.GUID;
                
                if(backupsrecoil.ContainsKey(guid) == false)
                    backupsrecoil.Add(guid, new GunAsset(asset));

                asset.recoilMax_x = 0f;
                asset.recoilMax_y = 0f;
                asset.recoilMin_x = 0f;
                asset.recoilMin_y = 0f;
            }
            else if (!menu.NoRecoil && Player.player.equipment.asset is ItemGunAsset)
            {

                var asset = Player.player.equipment.asset as ItemGunAsset;
                var assetguid = asset.GUID;

                if (backupsrecoil.ContainsKey(assetguid))
                {
                    var backup = backupsrecoil[assetguid];
                    
                    asset.recoilMax_x = backup.recoilmaxx;
                    asset.recoilMax_y = backup.recoilmaxy;
                    asset.recoilMin_x = backup.recoilminx;
                    asset.recoilMin_y = backup.recoilminy;

                    backupsrecoil.Remove(assetguid);
                }
            }
        }

        public static void CheckSpread()
        {
            if (menu.NoSpread && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = (ItemGunAsset) Player.player.equipment.asset;
                var guid = asset.GUID;
                
                if(backupsspread.ContainsKey(guid) == false)
                    backupsspread.Add(guid, new GunAsset(asset));

                asset.spreadAim = 0f;
                asset.spreadHip = 0f;
            }
            else if (!menu.NoSpread && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = Player.player.equipment.asset as ItemGunAsset;
                var assetguid = asset.GUID;

                if (backupsspread.ContainsKey(assetguid))
                {
                    var backup = backupsspread[assetguid];
                    
                    asset.spreadAim = backup.spreadaim;
                    asset.spreadHip = backup.spreadhip;
                    
                    
                    backupsspread.Remove(assetguid);
                }
            }
        }

        public static void CheckShake()
        {
            if (menu.NoShake && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = (ItemGunAsset) Player.player.equipment.asset;
                var guid = asset.GUID;
                
                if(backupsshake.ContainsKey(guid) == false)
                    backupsshake.Add(guid, new GunAsset(asset));

                asset.shakeMax_x = 0f;
                asset.shakeMax_y = 0f;
                asset.shakeMax_z = 0f;
                asset.shakeMin_x = 0f;
                asset.shakeMin_y = 0f;
                asset.shakeMin_z = 0f;
            }
            else if (!menu.NoShake && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = Player.player.equipment.asset as ItemGunAsset;
                var assetguid = asset.GUID;

                if (backupsshake.ContainsKey(assetguid))
                {
                    var backup = backupsshake[assetguid];

                    asset.shakeMax_x = backup.shakemaxx;
                    asset.shakeMax_y = backup.shakemaxy;
                    asset.shakeMax_z = backup.shakemaxz;
                    asset.shakeMin_x = backup.shakeminx;
                    asset.shakeMin_y = backup.shakeminy;
                    asset.shakeMin_z = backup.shakeminz;
                    
                    backupsshake.Remove(assetguid);
                }
            }
        }

        public static void CheckBallistics()
        {
            if (menu.NoDrop && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = (ItemGunAsset) Player.player.equipment.asset;
                var guid = asset.GUID;
                
                if(backupsdrop.ContainsKey(guid) == false)
                    backupsdrop.Add(guid, new GunAsset(asset));

                asset.ballisticForce = 999f;
                asset.ballisticDrop = 1E-05f;
            }
            else if (!menu.NoDrop && Player.player.equipment.asset is ItemGunAsset)
            {
                var asset = Player.player.equipment.asset as ItemGunAsset;
                var assetguid = asset.GUID;

                if (backupsdrop.ContainsKey(assetguid))
                {
                    var backup = backupsdrop[assetguid];
                    
                    asset.ballisticForce = backup.ballisticforce;
                    asset.ballisticDrop = backup.ballisticdrop;
 
                    backupsdrop.Remove(assetguid);
                }
            }
        }
        
        
    }
}