using System;
using System.Runtime.InteropServices;
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

        internal static void Start()
        {
            lastLock = DateTime.Now;
            menu = WaveMaker.MenuAim;

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
            if (menu.EnableAimlock && Provider.isConnected)
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

//                Logging.LogMsg("DEBUG", "checking tag");

                if (hit.transform != null && menu.LockPlayers && hit.transform.tag == "Enemy")
                {

                    var player = PlayerTools.GetSteamPlayer(hit.transform.gameObject);

                    if(player != null)
                        Logging.LogMsg("FOUND", "FOUND PLAYER ");
                    
                    Logging.LogMsg("DEBUG", "checking if player != null");

//                    if (player != null)
//                    {
//                        Logging.LogMsg("DEBUG", "checking if player is in friends");
//
//                        if (WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID) && menu.LockWhiteListFriends)
//                            return;
//
//                        Logging.LogMsg("DEBUG", "checking if player is admin");
//
//                        if (player.isAdmin && menu.LockWhitelistAdmins)
//                            return;
//                        
//                    }
//                    else
//                    {
//                        Logging.LogMsg("DEBUG", "returning");
//
//                        return;
//                    }
//                    
//                    Logging.LogMsg("DEBUG", "locksense is true");

                    locksense = true;
                    
                    Logging.LogMsg("AIMLOCK", "DETECTED PLAYER");
                    
                }
                else if (hit.transform.CompareTag("Zombie") && menu.LockZombies)
                {
                    locksense = true;
                }
                else if (hit.transform.CompareTag("Vehicle") && menu.LockVehicles)
                {
                    locksense = true; 
                }
                else
                {
                    locksense = false;
                }

                if (locksense)
                {
                    look.sensitivity = defsense / menu.LockSensitivity;
                }
                else
                {
                    look.sensitivity = defsense;
                }
                
            }
        }

        
    }
}