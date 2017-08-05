using System;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Aim
    {
        private static DateTime lastLock;
        private static Menu.Aim menu;

        private static float defsense;

        internal static void Start(Menu.Aim menu)
        {
            lastLock = DateTime.Now;
            Aim.menu = menu;

            defsense = Player.player.look.sensitivity;
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
                var look = Player.player.look;
                var mypos = Camera.main.transform.position;
                var range = menu.LockDistance;
                
                var locksense = false;

                if (menu.LockGunRange && Player.player.equipment.asset is ItemWeaponAsset)
                {
                    range = ((ItemWeaponAsset) Player.player.equipment.asset).range;
                }
                
                RaycastHit hit;
                Physics.Raycast(mypos, look.aim.forward, out hit, range, RayMasks.DAMAGE_CLIENT);

                /*if (hit.transform.CompareTag("Enemy") && menu.LockPlayers)
                {
                    var player = PlayerTools.GetSteamPlayer(hit.transform);

                    if (WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID) && menu.LockWhiteListFriends)
                        return;
                    
                    if (player.isAdmin && menu.LockWhitelistAdmins)
                        return;
                    
                    if(!player.player.life.isDead)
                    
                    locksense = true;
                }
                else */if (hit.transform.CompareTag("Zombie") && menu.LockZombies)
                {
                    if(!PlayerTools.GetZombie(hit.transform).isDead)
                        locksense = true;
                }
/*                else if (hit.transform.CompareTag("Vehicle") && menu.LockVehicles)
                {
                    
                }*/

                if (locksense)
                {
                    var newsense = defsense / menu.LockSensitivity / Time.deltaTime;
                }
                else
                {
                    look.sensitivity = defsense;
                }
                
            }
        }
    }
}