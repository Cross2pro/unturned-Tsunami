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
        
        internal static Color EnemyPlayerGlow;
        internal static Color FriendlyPlayerGlow;
        internal static Color ZombieGlow;
        internal static Color ItemGlow;
        internal static Color InteractableGlow;
        internal static Color VehicleGlow;

        internal static Color BoxPlayerFriendly;
        internal static Color BoxPlayerEnemy;
        internal static Color BoxZombie;
        
        internal static Material boxMaterial;
        
        internal static void Start(Menu.Visuals parent)
        {
            var material = new Material(Shader.Find("Hidden/Internal-Colored"));
            material.hideFlags = (HideFlags) 61;
            boxMaterial = material;
            boxMaterial.SetInt("_SrcBlend", 5);
            boxMaterial.SetInt("_DstBlend", 10);
            boxMaterial.SetInt("_Cull", 0);
            boxMaterial.SetInt("_ZWrite", 0);

            
            
            menu = WaveMaker.MenuVisuals;
            Last = DateTime.Now;
            
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

        internal static void Update()
        {
            if ((DateTime.Now - Last).TotalMilliseconds >= menu.UpdateRate)
            {
                if (menu.EnableEsp)
                {
                    UpdateLists();
                    UpdateColors();
                    Last = DateTime.Now;
                }

                
                
                CheckGlows();
                CheckLabels();

            }
            
            
            
        }

        internal static void CheckLabels()
        {
            var myPos = Player.player.transform.position;
            
            if (menu.PlayerName || menu.PlayerWeapon || menu.PlayerDistance)
            {
                
            }

            if (menu.ZombieName || menu.ZombieDistance || menu.ZombieSpecialty || menu.ZombieBox)
            {
                foreach (var zombie in Zombies)
                {
                    var targetPos = zombie.transform.position;
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var scrnPt = Camera.main.WorldToScreenPoint(targetPos);

                        if (scrnPt.z >= 0)
                        {

                            scrnPt.y = Screen.height - scrnPt.y;

                            var text = "";

                            if (menu.ZombieName)
                            {
                                text += $"{zombie.gameObject.name}";
                            }

                            if (menu.ZombieDistance)
                            {
                                if (text.Length > 0)
                                {
                                    text += $"\nDistance: {dist}";
                                }
                                else
                                {
                                    text += $"Distance: {dist}";
                                }
                            }

                            if (menu.ZombieSpecialty)
                            {
                                var str = "";

                                if (text.Length > 0)
                                    str += "\n";

                                switch (zombie.speciality)
                                {
                                        case EZombieSpeciality.NONE:
                                            str += "Specialty: None";
                                            break;
                                        case EZombieSpeciality.NORMAL:
                                            str += "Specialty: None";
                                            break;
                                        case EZombieSpeciality.MEGA:
                                            str += "Specialty: Mega";
                                            break;
                                        case EZombieSpeciality.CRAWLER:
                                            str += "Specialty: Crawler";
                                            break;
                                        case EZombieSpeciality.SPRINTER:
                                            str += "Specialty: Sprinter";
                                            break;
                                        case EZombieSpeciality.FLANKER_FRIENDLY:
                                            str += "Specialty: Friendly Flanker";
                                            break;
                                        case EZombieSpeciality.FLANKER_STALK:
                                            str += "Specialty: Stalking Flanker";
                                            break;
                                        case EZombieSpeciality.BURNER:
                                            str += "Specialty: Burner";
                                            break;
                                        case EZombieSpeciality.ACID:
                                            str += "Specialty: Acid";
                                            break;
                                        case EZombieSpeciality.BOSS_ELECTRIC:
                                            str += "Specialty: Electric (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_WIND:
                                            str += "Specialty: Wind (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_FIRE:
                                            str += "Specialty: Fire (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_ALL:
                                            str += "Specialty: All (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_MAGMA:
                                            str += "Specialty: Magma (Boss)";
                                            break;
                                }

                                text += str;
                            }
                        
                        }
                    }
                }
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

            foreach (var player in Players)
            {
                var targetPos = player.player.transform.position;

                if (menu.EnableEsp && menu.GlowPlayers)
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = player.player.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = player.player.gameObject.AddComponent<Highlighter>();

                        var color = WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID)
                            ? menu.FriendlyPlayerGlow
                            : menu.EnemyPlayerGlow;
                        
                        highlighter.ConstantParams(color);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = player.player.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = player.player.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
        }
        
        internal static void UpdateZombieGlow()
        {
            var myPos = Player.player.transform.position;

            foreach (var zombie in Zombies)
            {
                var zomPos = zombie.transform.position;

                if (menu.EnableEsp && menu.GlowZombies)
                {
                    var dist = Vector3.Distance(myPos, zomPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = zombie.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = zombie.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.ZombieGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = zombie.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = zombie.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
        }
        
        internal static void UpdateVehicleGlow()
        {
            var myPos = Player.player.transform.position;

            foreach (var vehicle in Vehicles)
            {
                var zomPos = vehicle.transform.position;

                if (menu.EnableEsp && menu.GlowVehicles)
                {
                    var dist = Vector3.Distance(myPos, zomPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = vehicle.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = vehicle.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.VehicleGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = vehicle.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = vehicle.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
        }
        
        internal static void UpdateItemGlow()
        {
            var myPos = Player.player.transform.position;

            foreach (var item in Items)
            {
                var zomPos = item.transform.position;

                if (menu.EnableEsp && menu.GlowItems)
                {
                    var dist = Vector3.Distance(myPos, zomPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = item.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = item.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.ItemGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = item.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = item.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
        }
        
        internal static void UpdateInteractableGlow()
        {
            var myPos = Player.player.transform.position;

            foreach (var interact in Animals) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Animals) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Storages) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Storages) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Forages) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Forages) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Beds) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Bed) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Doors) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Doors) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Traps) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Traps) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Flags) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Flag) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Sentries) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Sentries) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {   
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Airdrops) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Airdrop) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
            
            foreach (var interact in Npcs) //---
            {
                var targetPos = interact.transform.position;

                if (menu.EnableEsp && menu.GlowInteractables && menu.Npc) //--
                {
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = interact.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow); 
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = interact.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = interact.gameObject.GetComponent<Highlighter>();

                    if (highlighter != null)
                    {
                        highlighter.ConstantOffImmediate();
                    }
                }
            }
        }
        
        internal static void UpdateColors()
        {
            EnemyPlayerGlow = menu.EnemyPlayerGlow;
            FriendlyPlayerGlow = menu.FriendlyPlayerGlow;
            ZombieGlow = menu.ZombieGlow;
            ItemGlow = menu.ItemGlow;
            InteractableGlow = menu.InteractableGlow;
            VehicleGlow = menu.VehicleGlow;

            BoxPlayerFriendly = menu.BoxPlayerFriendly;
            BoxPlayerEnemy = menu.BoxPlayerEnemy;
            BoxZombie = menu.BoxZombie;
        }
        
        internal static void UpdateLists()
        {
            if (menu.EnableEsp) //DONT FOGET THIS VAR
            {
                if (menu.GlowPlayers || menu.PlayerName || menu.PlayerWeapon || menu.PlayerDistance)
                    Players = Provider.clients.ToArray();
                if (menu.GlowZombies || menu.ZombieName || menu.ZombieDistance || menu.ZombieSpecialty)
                    Zombies = Object.FindObjectsOfType<Zombie>();
                if (menu.GlowItems || menu.ItemName || menu.ItemDistance)
                    Items = Object.FindObjectsOfType<InteractableItem>();
                if (menu.GlowVehicles || menu.VehicleName || menu.VehicleDistance)
                    Vehicles = Object.FindObjectsOfType<InteractableVehicle>();
                if (menu.GlowInteractables && menu.Animals || menu.AnimalName || menu.AnimalDistance)
                    Animals = Object.FindObjectsOfType<Animal>();
                if (menu.GlowInteractables && menu.Forages || menu.ForageType || menu.ForageDistance)
                    Forages = Object.FindObjectsOfType<InteractableForage>();
                if (menu.GlowInteractables && menu.Storages || menu.StorageType || menu.StorageDistance)
                    Storages = Object.FindObjectsOfType<InteractableStorage>();
                if (menu.GlowInteractables && menu.Bed || menu.BedDistance)
                    Beds = Object.FindObjectsOfType<InteractableBed>();
                if (menu.GlowInteractables && menu.Doors || menu.DoorType || menu.DoorDistance)
                    Doors = Object.FindObjectsOfType<InteractableDoor>();
                if (menu.GlowInteractables && menu.Traps || menu.TrapType || menu.TrapDistance)
                    Traps = Object.FindObjectsOfType<InteractableTrap>();
                if (menu.GlowInteractables && menu.Flag || menu.FlagDistance)
                    Flags = Object.FindObjectsOfType<InteractableClaim>();
                if (menu.GlowInteractables && menu.Sentries || menu.SentryState || menu.SentryWeapon ||
                    menu.SentryDistance)
                    Sentries = Object.FindObjectsOfType<InteractableSentry>();
                if (menu.GlowInteractables && menu.Airdrop || menu.AirdropDistance)
                    Airdrops = Object.FindObjectsOfType<Carepackage>();
                if (menu.GlowInteractables && menu.Npc || menu.NpcName || menu.NpcWeapon || menu.NpcDistance)
                    Npcs = Object.FindObjectsOfType<InteractableObjectNPC>();
            }
            else
            {
                Players = null;
                Zombies = null;
                Items = null;
                Vehicles = null;
                Animals = null;
                Storages = null;
                Forages = null;
                Beds = null;
                Doors = null;
                Traps = null;
                Flags = null;
                Sentries = null;
                Airdrops = null;
                Npcs =  null;
            }
            
        }

        internal static Vector3 GetTargetVector(Transform target, string objName)
        {
            Transform[] componentsInChildren = target.transform.GetComponentsInChildren<Transform>();
            Vector3 result = Vector3.zero;

            if (componentsInChildren != null)
            {
                Transform[] array = componentsInChildren;
                for (int i = 0; i < array.Length; i++)
                {
                    Transform transform = array[i];
                    if (transform.name.Trim() == objName)
                    {
                        result = transform.position + new Vector3(0f, 0.4f, 0f);
                        break;
                    }
                }
            }
            return result;
        }
        
        internal void DrawBox(Transform target, Vector3 position, Color color)
        {
            Vector3 targetPos = GetTargetVector(target, "Skull");
            Vector3 ScrnPt = Camera.main.WorldToScreenPoint(targetPos);
            ScrnPt.y = (float)Screen.height - ScrnPt.y;
            
            float dist = System.Math.Abs(position.y - ScrnPt.y);
            float HlfDist = dist / 2f;
            float XVal = position.x - HlfDist / 2f;
            float YVal = position.y - dist;
            
            GL.PushMatrix();
            GL.Begin(1);
            boxMaterial.SetPass(0);
            GL.Color(color);
            
            GL.Vertex3(XVal, YVal, 0f);
            GL.Vertex3(XVal, YVal + dist, 0f);
            GL.Vertex3(XVal, YVal, 0f);
            GL.Vertex3(XVal + HlfDist, YVal, 0f);
            GL.Vertex3(XVal + HlfDist, YVal, 0f);
            GL.Vertex3(XVal + HlfDist, YVal + dist, 0f);
            GL.Vertex3(XVal, YVal + dist, 0f);
            GL.Vertex3(XVal + HlfDist, YVal + dist, 0f);
            
            GL.End();
            GL.PopMatrix();
        }
    }
}