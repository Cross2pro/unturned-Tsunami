using System;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Aim
    {
        private static DateTime lastLock;
        private static Menu.Aim menu;

        internal static void Start(Menu.Aim menu)
        {
            lastLock = DateTime.Now;
            Aim.menu = menu;
        }
        
        internal static void Update()
        {
            if ((DateTime.Now - lastLock).TotalMilliseconds >= menu.LockUpdateRate)
            {
                UpdateLock();
                lastLock = DateTime.Now;
            }
        }
        
        
        
        //------------------Lock------------------------

        internal static void UpdateLock()
        {
            if (menu.EnableAimlock)
            {
                if (menu.LockPlayers)
                {
                    foreach (var client in Provider.clients)
                    {
                        var mypos = Camera.main.transform.position;
                        var targetpos = client.player.transform.position;
                        var dist = Vector3.Distance(mypos, targetpos);
                        var range = menu.LockDistance;
                        
                        if (menu.LockGunRange && client.player.equipment.asset is ItemWeaponAsset)
                        {
                            range = ((ItemWeaponAsset) client.player.equipment.asset).range;
                        }

                        if (dist <= range)
                        {
                            if (WaveMaker.Friends.Contains(client.playerID.steamID.m_SteamID) &&  menu.LockWhiteListFriends)
                                return;

                            if (client.isAdmin && menu.LockWhitelistAdmins)
                                return;

                            
                        }
                    }
                }

                if (menu.LockZombies)
                {
                    
                }

                if (menu.LockAnimals)
                {
                    
                }

                if (menu.LockVehicles)
                {
                    
                }
            }
        }
    }
}