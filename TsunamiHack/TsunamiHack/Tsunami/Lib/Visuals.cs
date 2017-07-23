using System;
using System.Collections.Generic;
using UnityEngine;
using SDG.Unturned;
using Object = UnityEngine.Object;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Visuals 
    {
        internal static Menu.Visuals menu;

        internal static Player[] Players;
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
            UpdateLists();
            Last = DateTime.Now;
        }

        internal static void Update()
        {
            UpdateLists();

            if ((DateTime.Now - Last).TotalMilliseconds >= menu.UpdateRate)
            {
                CheckGlows();

                Last = DateTime.Now;
            }
            
            
            
        }

        internal static void CheckGlows()
        {
            if(menu.GlowPlayers)
                UpdatePlayerGlow();
            else if(menu.GlowZombies)
                UpdateZombieGlow();
            else if(menu.GlowItems)
                UpdateItemGlow();
            else if(menu.GlowInteractables)
                UpdateInteractableGlow();
            else if (menu.GlowVehicles)
                UpdateVehicleGlow();
        }

        internal static void UpdatePlayerGlow()
        {
            var Cam = Camera.main;
            var MyPos = Player.player.transform.position;
            
            foreach (var user in Players)
            {
                var TargetPos = user.transform.position;
                if (Vector3.Distance(MyPos, TargetPos) <= menu.Distance || menu.InfDistance)
                {
                    
                }
            }
        }
        
        internal static void UpdateZombieGlow()
        {
            
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
            Players = Object.FindObjectsOfType<Player>();
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