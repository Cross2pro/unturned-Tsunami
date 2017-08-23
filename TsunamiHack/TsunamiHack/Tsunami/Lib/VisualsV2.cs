using System;
using System.Collections.Generic;
using System.Linq;
using HighlightingSystem;
using UnityEngine;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using Object = UnityEngine.Object;

namespace TsunamiHack.Tsunami.Lib
{
    internal class VisualsV2
    {
        internal static Menu.Visuals Menu;
        internal static float updateInterval;
        internal static DateTime LastUpdate;
        
        
        internal static List<SteamPlayer> Players;
        internal static List<Zombie> Zombies;
        internal static List<InteractableVehicle> Vehicles;
        internal static List<InteractableItem> Items;
        internal static List<Animal> Animals;
        internal static List<InteractableStorage> Storages;
        internal static List<InteractableForage> Forages;
        internal static List<InteractableBed> Beds;
        internal static List<InteractableDoor> Doors;
        internal static List<InteractableTrap> Traps;
        internal static List<InteractableClaim> Claims;
        internal static List<InteractableSentry> Sentries;
        internal static List<InteractableObjectNPC> Npcs;

        public static void Start()
        {
            Menu = WaveMaker.MenuVisuals;
            
        }

        public static void Update() 
        {
            //TODO: add update delay
            
            if (Provider.isConnected)
            {

                  //TODO: Fix null reference found in getlistplayer in this block
                
//                if (Menu.GlowPlayers)
//                {
//                    Players = new List<SteamPlayer>();
//                    Players = Provider.clients;
//
//                    var friends = new List<SteamPlayer>();
//                    var enemies = new List<SteamPlayer>();
//
//                    foreach (var user in Players)
//                    {
//                        if(WaveMaker.Friends.Contains(user.playerID.steamID.m_SteamID))
//                            friends.Add(user);
//                        else
//                            enemies.Add(user);
//                    }
//                    
//                    EnableGlowGeneric(GlowItem.GetListPlayer(friends), Menu.FriendlyPlayerGlow);
//                    EnableGlowGeneric(GlowItem.GetListPlayer(enemies), Menu.EnemyPlayerGlow);
//                }
//                else
//                {
//                    DisableGlowGeneric(GlowItem.GetListPlayer(Players));
//                    Players = new List<SteamPlayer>();
//                }
                
                if (Menu.GlowZombies)
                {
                    Zombies = new List<Zombie>();
                    foreach (var region in ZombieManager.regions)
                    {
                        foreach (var list in region.zombies)
                        {
                            Zombies.Add(list);
                        }
                    }

                    EnableGlowGeneric(GlowItem.GetListZombie(Zombies), Menu.ZombieGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListZombie(Zombies));
                    Zombies = new List<Zombie>();
                }

                if (Menu.GlowVehicles)
                {
                    Vehicles = new List<InteractableVehicle>();
                    Vehicles = VehicleManager.vehicles;
                    
                    EnableGlowGeneric(GlowItem.GetListVehicle(Vehicles), Menu.VehicleGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListVehicle(Vehicles));
                    Vehicles = new List<InteractableVehicle>();
                }


                if (Menu.GlowItems)
                {
                    Items = new List<InteractableItem>();
                    Items = Object.FindObjectsOfType<InteractableItem>().ToList();
                    
                    EnableGlowGeneric(GlowItem.GetListItem(Items), Menu.ItemGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListItem(Items));
                    Items = new List<InteractableItem>();
                }

                if (Menu.Animals)
                {
                    Animals = new List<Animal>();
                    Animals = AnimalManager.animals;

                    EnableGlowGeneric(GlowItem.GetListAnimal(Animals), Menu.InteractableGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListAnimal(Animals));
                    Animals = new List<Animal>();
                }

                if (Menu.Storages)
                {
                    Storages = new List<InteractableStorage>();
                    Storages = Object.FindObjectsOfType<InteractableStorage>().ToList();

                    EnableGlowGeneric(GlowItem.GetListStorage(Storages), Menu.InteractableGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListStorage(Storages));
                    Storages = new List<InteractableStorage>();
                }
                
                if (Menu.Bed)
                {
                    Beds = new List<InteractableBed>();
                    Beds = Object.FindObjectsOfType<InteractableBed>().ToList();

                    EnableGlowGeneric(GlowItem.GetListBed(Beds), Menu.InteractableGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListBed(Beds));
                    Beds = new List<InteractableBed>();
                }
                
                if (Menu.Doors)
                {
                    Doors = new List<InteractableDoor>();
                    Doors = Object.FindObjectsOfType<InteractableDoor>().ToList();

                    EnableGlowGeneric(GlowItem.GetListDoor(Doors), Menu.InteractableGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListDoor(Doors));
                    Doors = new List<InteractableDoor>();
                }
                
                if (Menu.Flag)
                {
                    Claims = new List<InteractableClaim>();
                    Claims = Object.FindObjectsOfType<InteractableClaim>().ToList();

                    EnableGlowGeneric(GlowItem.GetListFlag(Claims), Menu.InteractableGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListFlag(Claims));
                    Claims = new List<InteractableClaim>();
                }
                
                if (Menu.Sentries)
                {
                    Sentries = new List<InteractableSentry>();
                    Sentries = Object.FindObjectsOfType<InteractableSentry>().ToList();

                    EnableGlowGeneric(GlowItem.GetListSentry(Sentries), Menu.InteractableGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListSentry(Sentries));
                    Sentries = new List<InteractableSentry>();
                }
                
                if (Menu.Npc)
                {
                    Npcs = new List<InteractableObjectNPC>();
                    Npcs = Object.FindObjectsOfType<InteractableObjectNPC>().ToList();

                    EnableGlowGeneric(GlowItem.GetListNpc(Npcs), Menu.InteractableGlow);
                }
                else
                {
                    DisableGlowGeneric(GlowItem.GetListNpc(Npcs));
                    Npcs = new List<InteractableObjectNPC>();
                }
                
                
                
            }
           

        }



        public static void EnableGlowGeneric(List<GlowItem> list, Color glowColor)
        {
            foreach (var go in list)
            {
                if (Camera.main != null && go.gameObject != null)
                {
                    var mypos = Camera.main.gameObject.transform.position;
                    var targetpos = go.gameObject.transform.position;

                    if (Vector3.Distance(mypos, targetpos) <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = go.gameObject.GetComponent<Highlighter>() ??
                                          go.gameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(glowColor);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        if(go.gameObject.GetComponent<Highlighter>() != null)
                        {
                            var highlighter = go.gameObject.GetComponent<Highlighter>();
                            
                            highlighter.ConstantOffImmediate();
                        }
                    }
                }
            }
        }

        public static void DisableGlowGeneric(List<GlowItem> list)
        {
            foreach (var go in list)
            {
                if(go.gameObject.GetComponent<Highlighter>() != null)
                    go.gameObject.GetComponent<Highlighter>().ConstantOffImmediate();
                    
            }
        }


    }
}