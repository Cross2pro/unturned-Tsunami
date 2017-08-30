using System;
using System.Runtime.InteropServices;
using Pathfinding;
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

                var range = menu.LockDistance;
                if (menu.LockGunRange && Player.player.equipment.asset is ItemGunAsset)
                {
                    range = ((ItemGunAsset) Player.player.equipment.asset).range;
                }

                RaycastHit hit;
                Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range);

                if (hit.transform != null && menu.LockPlayers && hit.transform.CompareTag("Enemy"))
                {
                    if (menu.LockWhiteListFriends)
                        if (!isPlayerFriend(hit))
                            Player.player.look.sensitivity = defsense;

                    Player.player.look.sensitivity = defsense / menu.LockSensitivity;
                }
                else
                    Player.player.look.sensitivity = defsense;
                
            }
            
            
            
        }

        internal static bool isPlayerFriend(RaycastHit rch)
        {
//            try
//            {
                if (rch.transform != null && rch.transform.CompareTag("Enemy"))
                {
                    var list = Provider.clients;

                    foreach (var client in list)
                    {
                        if (client.player.transform.gameObject == rch.transform.gameObject)
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;
//            } catch(Exception){}
            

        }

        
    }
}