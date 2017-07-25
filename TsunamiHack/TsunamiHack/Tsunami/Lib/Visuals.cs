using System;
using System.Collections.Generic;
using System.Globalization;
using HighlightingSystem;
using UnityEngine;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Util;
using Object = UnityEngine.Object;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Visuals 
    {
        internal static Menu.Visuals menu;

        internal static SteamPlayer[] Players;
        internal static Zombie[] Zombies;
        internal static InteractableItem[] Items;
        internal static InteractableVehicle[] Vehicles;
        internal static Animal[] Animals;
        internal static InteractableStorage[] Storages;
        internal static InteractableForage[] Forages;
        internal static InteractableBed[] Beds;
        internal static InteractableDoor[] Doors;
        internal static InteractableTrap[] Traps;
        internal static InteractableClaim[] Flags;
        internal static InteractableSentry[] Sentries;
        internal static Carepackage[] Airdrops;
        internal static InteractableObjectNPC[] Npcs;

        internal static DateTime Last;
        
        
        internal static void Start(Menu.Visuals parent)
        {
            menu = WaveMaker.MenuVisuals;
            UpdateLists();
            Last = DateTime.Now;
        }

        internal static void Update()
        {
//            UpdateLists();

            if ((DateTime.Now - Last).TotalMilliseconds >= menu.UpdateRate)
            {
                CheckGlows();

                Last = DateTime.Now;
            }
            
            
            
        }

        internal static void CheckGlows()
        {
            UpdatePlayerGlow();
            UpdateZombieGlow();
            UpdateItemGlow();
            UpdateInteractableGlow();
            UpdateVehicleGlow();
                
        }

        internal static void UpdatePlayerGlow()
        {
            var myPos = Player.player.transform.position;
            
            foreach (var user in Players)
            {
                if (user.player.transform != Player.player.transform)
                {
                
                    var targetPos = user.player.transform.position;

                    if (Vector3.Distance(myPos, targetPos) <= menu.Distance || menu.InfDistance)
                    {

                        var espColor = WaveMaker.Friends.Contains(user.playerID.steamID.m_SteamID)
                            ? menu.FriendlyPlayerGlow
                            : menu.EnemyPlayerGlow;

                        var highl = user.player.gameObject.GetComponent<Highlighter>();

                        if (!menu.GlowPlayers && highl != null)
                        {
                            highl.ConstantOffImmediate();
                            return;
                        }

                        if (menu.GlowPlayers)
                        {
                            if (highl == null)
                            {
                                highl = user.player.gameObject.AddComponent<Highlighter>();
                            }

                            highl.ConstantParams(espColor);
                            highl.OccluderOn();
                            highl.SeeThroughOn();
                            highl.ConstantOn();
                        }

                    }
                    else
                    {
                        var highl = user.player.gameObject.GetComponent<Highlighter>();
                        
                        if(highl != null)
                            highl.ConstantOffImmediate();
                    }
                }
            }
        }
        
        internal static void UpdateZombieGlow()
        {
            
            
            var myPos = Player.player.transform.position;

            foreach (var zombie in Zombies)
            {
                var targetPos = zombie.transform.position;
                var dist = Vector3.Distance(myPos, targetPos);

                Logging.LogMsg("Debug", $"{dist}/{menu.Distance}");

                if (dist <= menu.Distance)
                {


                    var espColor = menu.ZombieGlow;

                    var highl = zombie.gameObject.GetComponent<Highlighter>();

                    if (!menu.GlowZombies && menu.Esp && highl != null)
                    {
                        highl.ConstantOffImmediate();
                    }
                    else if (menu.GlowZombies && menu.Esp)
                    {
                        if (highl == null)
                        {
                            highl = zombie.gameObject.AddComponent<Highlighter>();
                        }

                        highl.ConstantParams(espColor);
                        highl.OccluderOn();
                        highl.SeeThroughOn();
                        highl.ConstantOn();
                    }
                }
                else
                {
                    var highl = zombie.gameObject.GetComponent<Highlighter>();

                    if (highl != null)
                    {
                        highl.ConstantOffImmediate();
                    }
                }
                    
                    
                }
                
            }
        
        internal static void UpdateItemGlow()
        {
            
        }
        
        internal static void UpdateInteractableGlow()
        {
            
        }
        
        internal static void UpdateVehicleGlow()
        {
            
        }
        
        internal static void UpdateLists()
        {
            Players = Provider.clients.ToArray();
            Zombies = Object.FindObjectsOfType<Zombie>();
            Items = Object.FindObjectsOfType<InteractableItem>();
            Vehicles = Object.FindObjectsOfType<InteractableVehicle>();
            Animals = Object.FindObjectsOfType<Animal>();
            Storages = Object.FindObjectsOfType<InteractableStorage>();
            Forages = Object.FindObjectsOfType<InteractableForage>();
            Beds = Object.FindObjectsOfType<InteractableBed>();
            Doors = Object.FindObjectsOfType<InteractableDoor>();
            Traps = Object.FindObjectsOfType<InteractableTrap>();
            Flags = Object.FindObjectsOfType<InteractableClaim>();
            Sentries = Object.FindObjectsOfType<InteractableSentry>();
            Airdrops = Object.FindObjectsOfType<Carepackage>();
            Npcs = Object.FindObjectsOfType<InteractableObjectNPC>();
        }
        
        
    }
}