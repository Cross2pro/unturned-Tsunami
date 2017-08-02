using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using HighlightingSystem;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Util;
using Object = UnityEngine.Object;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Visuals 
    {
        //TODO: fix positioning of labels
        //TODO: Finish all other labels
        //TODO: maybe add other boxes
        //TODO: create a maximum zombies to show value
        //TODO: change mypos to camera pos
        //TODO: change menu sizes to a proportion and add scrollbars into all menus
        
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

        internal static float altitiude;
        
        internal static Color EnemyPlayerGlow;
        internal static Color FriendlyPlayerGlow;
        internal static Color ZombieGlow;
        internal static Color ItemGlow;
        internal static Color InteractableGlow;
        internal static Color VehicleGlow;

        internal static Color BoxPlayerFriendly;
        internal static Color BoxPlayerEnemy;
        internal static Color BoxZombie;

        internal static Dictionary<int, string> StorageIds;
        internal static Dictionary<int, string> DoorIds;
        
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

            GenerateDicts();
            
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
            UpdateLists();  
            UpdateColors();
            
            if ((DateTime.Now - Last).TotalMilliseconds >= menu.UpdateRate)
            {
  
                CheckGlows();     
//                UpdateWeather();
                
                Last = DateTime.Now;

            }
            
            
            
        }

        internal static void OnGUI()
        {
            CheckLabels();
            CheckBoxes();
        }

        //----------------------Labels/boxes---------------------------------------------------------------------
        
        internal static void CheckBoxes()
        {
            if (menu.PlayerBox)
            {
                foreach (var player in Players)
                {
                    if (!player.player.life.isDead && player.player != Player.player)
                    {
                        var mypos = Player.player.transform.position;
                        var targetpos = player.player.transform.position;
                        var dist = Vector3.Distance(mypos, targetpos);

                        if (dist <= menu.Distance || menu.InfDistance)
                        {
                            var pos = Camera.main.WorldToScreenPoint(targetpos);

                            if (pos.z >= 0)
                            {
                                pos.y = Screen.height - pos.y;
                                
                                var color = WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID) ? menu.BoxPlayerFriendly : menu.BoxPlayerEnemy;
                                
                                DrawBox(player.player.transform, pos, color);
                            }
                        }
                    }
                }           
            }

            if (menu.ZombieBox)
            {
                foreach (var zombie in Zombies)
                {
                    if (!zombie.isDead)
                    {
                        var mypos = Player.player.transform.position;
                        var targetpos = zombie.transform.position;
                        var dist = Vector3.Distance(mypos, targetpos);

                        if (dist <= menu.Distance || menu.InfDistance)
                        {
                            var pos = zombie.transform.position;
                            pos = Camera.main.WorldToScreenPoint(pos);

                            if (pos.z >= 0)
                            {
                                pos.y = Screen.height - pos.y;

                                DrawBox(zombie.transform, pos, menu.BoxZombie);
                            }
                        }         
                    }   
                }
            }
        }
        
        internal static void CheckLabels()
        {
             
            if (menu.PlayerName || menu.PlayerWeapon || menu.PlayerDistance)
            {
                UpdatePlayerLabels();
            }
                
            if (menu.ZombieName || menu.ZombieDistance || menu.ZombieSpecialty)
            {
                UpdateZombieLabels();
            }

            if (menu.AnimalName || menu.AnimalDistance)
            {
                UpdateAnimalLabels();
            }

            if (menu.StorageType || menu.StorageDistance)
            {
                UpdateStorageLabels();
            }

            if (menu.VehicleName || menu.VehicleDistance)
            {
                UpdateVehicleLabels();
            }

            if (menu.ItemName || menu.ItemDistance)
            {
                UpdateItemLabels();
            }

            if (menu.NpcName || menu.NpcDistance /*|| menu.NpcWeapon*/)
            {
                UpdateNpcLabels();
            }

            if (menu.ForageType || menu.ForageDistance)
            {
                UpdateForageLabels();
            }

            if (menu.BedType || menu.BedDistance)
            {
                UpdateBedLabels();
            }

            if (menu.FlagType || menu.FlagDistance)
            {
                UpdateFlagLabels();
            }

            if (menu.SentryType || menu.SentryWeapon || menu.SentryState || menu.SentryDistance)
            {
                UpdateSentryLabels();
            }

            if (menu.Admins)
            {
                UpdateAdminLabels();
            }
            
            //Add airdrops and traps once glow is fixed and labels are created
        }

        internal static void UpdatePlayerLabels()
        {
            foreach (var player in Players)
                {
                                        
                    if (!player.player.life.isDead && player.player != Player.player)
                    {
                        var myPos = Player.player.transform.position;
                        var targetpos = player.player.transform.position;
                        var dist = Vector3.Distance(myPos, targetpos);

                        if (dist <= menu.Distance || menu.InfDistance)
                        {
                            targetpos += new Vector3(0f,3f,0f);
                            var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                            if (scrnpt.z >= 0)
                            {
                                scrnpt.y = Screen.height - scrnpt.y;
                                var text = "";

                                if (menu.PlayerName)
                                {
                                    text += $"{player.playerID.nickName}";
                                }

                                if (menu.PlayerWeapon)
                                {
                                    if (text.Length > 0)
                                        text += $"\nWeapon: {player.player.equipment.asset.name}";
                                    else
                                        text += $"Weapon: {player.player.equipment.asset.name}";
                                }

                                if (menu.PlayerDistance)
                                {
                                    if (text.Length > 0)
                                        text += $"\nDistance: {Math.Round(dist, 0)}";
                                    else
                                        text += $"Distance: {Math.Round(dist, 0)}";
                                }
                                
                                
                                float size;

                                if (menu.ScaleText)
                                    size = dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize;
                                else
                                    size = 10f;

                                var color = WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID)
                                    ? menu.FriendlyPlayerGlow
                                    : menu.EnemyPlayerGlow;
                            
                                GUI.Label(new Rect(scrnpt + new Vector3(0,6f,0), new Vector2(170,70)), $"<color={color}><size={size}>{text}</size></color>" );
                            }
      
                        }
                    }
                }
        }

        internal static void UpdateZombieLabels()
        {
            foreach (var zombie in Zombies)
                {
                    if (zombie.isDead == false)
                    {
                        var myPos = Player.player.transform.position;
                        var targetPos = zombie.transform.position;
                        var dist = Vector3.Distance(myPos, targetPos);
    
                        if (dist <= menu.Distance || menu.InfDistance)
                        {
                            targetPos += new Vector3(0f,3f, 0f);
                            var scrnPt = Camera.main.WorldToScreenPoint(targetPos);
    
                            if (scrnPt.z >= 0)
                            {
    
                                scrnPt.y = (float)(Screen.height - scrnPt.y);
    
                                var text = "";
    
                                if (menu.ZombieName)
                                {
                                    text += $"{zombie.gameObject.name}";
                                }
    
                                if (menu.ZombieDistance)
                                {
                                    if (text.Length > 0)
                                    {
                                        text += $"\nDistance: {Math.Round(dist, 0)}";
                                    }
                                    else
                                    {
                                        text += $"Distance: {Math.Round(dist, 0)}";
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

                                float size;
                                
                                if (menu.ScaleText)
                                {
                                    size = dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize;
                                }
                                else
                                {                                    
                                    size = 10;
                                }
                                
                                 
                                
                                GUI.Label(new Rect(scrnPt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={ColorUtility.ToHtmlStringRGBA(menu.BoxZombie)}><size={size}>{text}</size></color>" );        
                            }
                        }
                    }
                      
                }
        }

        internal static void UpdateAnimalLabels()
        {
            foreach (var animal in Animals)
            {
                if (!animal.isDead)
                {
                    var mypos = Player.player.transform.position;
                    var targetpos = animal.transform.position;
                    var dist = Vector3.Distance(mypos, targetpos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        targetpos += new Vector3(0f,3f,0f);
                        var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                        if (scrnpt.z >= 0f)
                        {
                            scrnpt.y = Screen.height - scrnpt.y;

                            var text = "";

                            if (menu.AnimalName)
                            {
                                text += $"{animal.asset.animalName}";
                            }

                            if (menu.AnimalDistance)
                            {
                                if (text.Length > 0)
                                    text += $"\nDistance: {Math.Round(dist, 0)}";
                                else
                                    text += $"Distance: {Math.Round(dist, 0)}"; 
                            }

                            var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;
                            
                            GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={menu.InteractableGlow}><size={size}>{text}</size></color>" );
                        }
                    }
                }
            }
        }

        internal static void UpdateStorageLabels()
        {
            foreach (var storage in Storages)
            {
                var mypos = Player.player.transform.position;
                var targetpos = storage.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f,1.5f,0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.StorageType)
                        {
                            text += $"Storage: {StorageIds[int.Parse(storage.name)]}";
                        }

                        if (menu.StorageDistance)
                        {
                            text += text.Length > 0
                                ? $"\nDistance: {Math.Round(dist, 0)}"
                                : $"Distance: {Math.Round(dist, 0)}";
                        }

                        
                        var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;
                            
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={menu.InteractableGlow}><size={size}>{text}</size></color>" );
                        
                    }

                }
                
            }
        }

        internal static void UpdateVehicleLabels()
        {
            foreach (var vehicle in Vehicles)
            {
                var mypos = Player.player.transform.position;
                var targetpos = vehicle.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f,1.5f,0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.VehicleName)
                        {
                            var name = vehicle.asset.name.Replace("_", " ");

                            if (name.Contains("Tractor"))
                                name = "Tractor";

                            if (name.Contains("Police"))
                                name = "Police Car";
                            
                            text += $"Vehicle: {name}";
                        }

                        if (menu.VehicleDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";
                            
                            text += $"Distance: {Math.Round(dist,0)}";
                        }

                        var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;
                        
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={menu.InteractableGlow}><size={size}>{text}</size></color>" );
                    }
                }
            }
        }

        internal static void UpdateItemLabels()
        {
            foreach (var item in Items)
            {
                var mypos = Player.player.transform.position;
                var targetpos = item.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f,1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.ItemName)
                        {
                            text += $"Item: {item.asset.itemName}";
                        }

                        if (menu.ItemDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                            
                        }
                        
                        var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;
                        
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={menu.InteractableGlow}><size={size}>{text}</size></color>" );
                    }
                }
            }
        }

        internal static void UpdateAirdropLabels() //FIX ONCE GLOW IS FIXED
        {
            
        }

        internal static void UpdateTrapLabels() //FIX ONCE GLOW IS FIXED
        {
            
        }

        internal static void UpdateNpcLabels()
        {
            foreach (var npc in Npcs)
            {
                var mypos = Player.player.transform.position;
                var targetpos = npc.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f,1.5f,0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.NpcName)
                        {
                            text += $"NPC: {npc.npcAsset.name}";
                        }

//                        if (menu.NpcWeapon)     FIND WAY TO DETERMINE PRIMARY WEAPON
//                        {
//                            if (text.Length > 0)
//                                text += "\n";
//                            
//                            text += $"Weapon: {npc.}"
//                        }

                        if (menu.NpcDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }
                        
                    }
                }
            }
        }

        internal static void UpdateForageLabels()
        {
            foreach (var forage in Forages)
            {
                var mypos = Player.player.transform.position;
                var targetpos = forage.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.ForageType)
                        {
                            text += "Forage";
                        }

                        if (menu.ForageDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }
                        
                        var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;
                        
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={ColorUtility.ToHtmlStringRGBA(menu.InteractableGlow)}><size={size}>{text}</size></color>" );
                    }
                }

            }
        }

        //Possibly add owner of bed?
        internal static void UpdateBedLabels()
        {
            foreach (var bed in Beds)
            {
                var mypos = Player.player.transform.position;
                var targetpos = bed.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.BedType)
                        {
                            text += "Bed";
                        }

                        if (menu.BedDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }
                        
                        var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;
                        
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={ColorUtility.ToHtmlStringRGBA(menu.InteractableGlow)}><size={size}>{text}</size></color>" );
                    }
                }

            }
        }

        internal static void UpdateDoorLabels()
        {
            foreach (var door in Doors)
            {
                var mypos = Player.player.transform.position;
                var targetpos = door.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.DoorType)
                        {
                            text += $"Door: {DoorIds[int.Parse(door.name)]}";
                        }

                        if (menu.BedDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }

                        var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color={ColorUtility.ToHtmlStringRGBA(menu.InteractableGlow)}><size={size}>{text}</size></color>");
                    }
                }
            }
        }

        internal static void UpdateFlagLabels()
        {
            foreach (var flag in Flags)
            {
                var mypos = Player.player.transform.position;
                var targetpos = flag.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.FlagType)
                        {
                            text += "Claim Flag";
                        }

                        if (menu.FlagDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }

                        var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color={ColorUtility.ToHtmlStringRGBA(menu.InteractableGlow)}><size={size}>{text}</size></color>");
                    }
                }
            }
        }

        internal static void UpdateSentryLabels()
        {
            foreach (var sentry in Sentries)
            {
                var mypos = Player.player.transform.position;
                var targetpos = sentry.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= menu.Distance || menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (menu.SentryType)
                        {
                            text += "Sentry";
                        }

//                        if (menu.SentryWeapon)        FIX NULLCHECK
//                        {
//                            if (text.Length > 0)
//                                text += "\n";
//
//                            if(sentry.displayItem.id != null) 
//                               text += $"Weapon: {sentry.displayItem.id}";
//                        }

                        if (menu.SentryState)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            var state = sentry.enabled ? "On" : "Off";
                            text += $"State: {state}";
                        }
                        
                        if (menu.SentryDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }

                        var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 70)),
                            $"<color={ColorUtility.ToHtmlStringRGBA(menu.InteractableGlow)}><size={size}>{text}</size></color>");
                    }
                }
            }
        }

        internal static void UpdateAdminLabels()
        {
            foreach (var player in Players)
            {
                if (player.isAdmin)
                {
                    var mypos = Player.player.transform.position;
                    var targetpos = player.player.transform.position;
                    var dist = Vector3.Distance(mypos, targetpos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        targetpos += new Vector3(0f, 3f, 0f);
                        var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                        if (scrnpt.z >= 0f)
                        {
                            scrnpt.y = Screen.height - scrnpt.y;
                            var text = "";

                            if (menu.Admins)
                            {
                                text += "ADMIN";
                            }

                            var size = menu.ScaleText ? dist <= menu.Dropoff ? menu.CloseSize : menu.FarSize : 10f;

                            GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                                $"<color=#FF0000><size={size}>{text}</size></color>");
                        }
                    }
                }  
            }
        }
        
        //---------------Glows------------------------------------------------------------------------------------
        
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
            if (Players.Length > 1)
            {
                
                foreach (var player in Players)
                {
                
                    var myPos = Player.player.transform.position;
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
            
        }
        
        internal static void UpdateZombieGlow()
        {
            
            foreach (var zombie in Zombies)
            {
                var myPos = Player.player.transform.position;
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

            foreach (var vehicle in Vehicles)
            {
                Logging.LogMsg("DEBUG", "My Pos");
                var myPos = Player.player.transform.position;
                Logging.LogMsg("DEBUG", "Target Pos");
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

            foreach (var item in Items)
            {
                var myPos = Camera.main.transform.position;
                var targetpos = item.transform.position;

                if (menu.EnableEsp && menu.GlowItems)
                {
                    var dist = Vector3.Distance(myPos, targetpos);

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
            
            UpdateAnimalGlow();
            UpdateStorageGlow();
            UpdateForageGlow();
            UpdateBedGlow();
            UpdateDoorsGlow();
//            UpdateTrapsGlow();
            UpdateFlagsGlow();
//            UpdateAirdropGlow();
            UpdateSentryGlow();
        }

        internal static void UpdateAnimalGlow()
        {

            foreach (var animal in Animals)
            {
                
                if (menu.EnableEsp && menu.GlowInteractables && menu.Animals)
                {
                    var myPos = Player.player.transform.position;
                    var targetPos = animal.transform.position;
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = animal.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null) 
                            highlighter = animal.gameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(menu.InteractableGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = animal.gameObject.GetComponent<Highlighter>();
                    
                        if(highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = animal.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }
            }
        }

        internal static void UpdateStorageGlow()
        {

            foreach (var storage in Storages)
            {
                var myPos = Player.player.transform.position;
                var targetPos = storage.transform.position;
                var dist = Vector3.Distance(myPos, targetPos);

                if (menu.EnableEsp && menu.GlowInteractables && menu.Storages)
                {
                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = storage.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = storage.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(menu.InteractableGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = storage.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = storage.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }

            }
        }

        internal static void UpdateForageGlow()
        {

            foreach (var forage in Forages)
            {
                var myPos = Player.player.transform.position;
                var targetpos = forage.transform.position;
                var dist = Vector3.Distance(myPos, targetpos);

                if (menu.EnableEsp && menu.GlowInteractables && menu.Forages)
                {
                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = forage.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = forage.gameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(menu.InteractableGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = forage.gameObject.GetComponent<Highlighter>();
                        
                        if(highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = forage.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }
            }
        }

        internal static void UpdateBedGlow()
        {

            foreach (var bed in Beds)
            {
                var myPos = Player.player.transform.position;
                
                var targetPos = bed.transform.position;
                var dist = Vector3.Distance(myPos, targetPos);

                if (menu.EnableEsp && menu.GlowInteractables && menu.Bed)
                {
                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = bed.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = bed.gameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(menu.InteractableGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = bed.gameObject.GetComponent<Highlighter>();
                        
                        if(highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = bed.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }

            }
        }

        internal static void UpdateDoorsGlow()
        {

            foreach (var door in Doors)
            {
                var mypos = Player.player.transform.position;
                var targetpos = door.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (menu.EnableEsp && menu.GlowInteractables && menu.Doors)
                {
                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = door.gameObject.GetComponent<Highlighter>();
                        
                        if(highlighter == null)
                            highlighter = door.gameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(menu.InteractableGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = door.gameObject.GetComponent<Highlighter>();
                        
                        if(highlighter != null)
                            highlighter.ConstantOffImmediate();
                            
                    }
                }
                else
                {
                    var highlighter = door.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }
            }
        }

        //Fix traps
        internal static void UpdateTrapsGlow()
        {

            foreach (var trap in Traps)
            {
                var mypos = Player.player.transform.position;
                var targetpos = trap.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (menu.EnableEsp && menu.GlowInteractables && menu.Traps)
                {
                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highligter = trap.gameObject.GetComponent<Highlighter>();

                        if (highligter == null)
                            highligter = trap.gameObject.AddComponent<Highlighter>();
                        
                        highligter.ConstantParams(menu.InteractableGlow);
                        highligter.OccluderOn();
                        highligter.SeeThroughOn();
                        highligter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = trap.gameObject.GetComponent<Highlighter>();
                        
                        if(highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = trap.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }
            }
        }

        internal static void UpdateFlagsGlow()
        {

            foreach (var flag in Flags)
            {
                var mypos = Player.player.transform.position;
                var targetpos = flag.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (menu.EnableEsp && menu.GlowInteractables && menu.Flag)
                {
                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = flag.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = flag.gameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(menu.InteractableGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = flag.gameObject.GetComponent<Highlighter>();
                        
                        if(highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = flag.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }
            }
        }

        internal static void UpdateSentryGlow()
        {

            foreach (var sentry in Sentries)
            {
                var mypos = Player.player.transform.position;
                var targetpos = sentry.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (menu.EnableEsp && menu.GlowInteractables && menu.Sentries)
                {
                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = sentry.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = sentry.gameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(menu.InteractableGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = sentry.gameObject.GetComponent<Highlighter>();
                        
                        if(highlighter != null)
                            highlighter.ConstantOffImmediate();
                        
                    }
                }
                else
                {
                    var highlighter = sentry.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }
            }
        }

        //Fix Airdrops
        internal static void UpdateAirdropGlow()
        {   

            foreach (var airdrop in Storages)
            {
                if (airdrop.name == "1374")
                {
                    var mypos = Player.player.transform.position;
                    var targetpos = airdrop.transform.position;
                    var dist = Vector3.Distance(mypos, targetpos);

                    if (menu.EnableEsp && menu.GlowInteractables && menu.Airdrop)
                    {
                        if (dist <= menu.Distance || menu.InfDistance)
                        {
                            var highlighter = airdrop.gameObject.GetComponent<Highlighter>();

                            if (highlighter == null)
                                highlighter = airdrop.gameObject.AddComponent<Highlighter>();
                        
                            highlighter.ConstantParams(menu.InteractableGlow);
                            highlighter.OccluderOn();
                            highlighter.SeeThroughOn();
                            highlighter.ConstantOn();
                        }
                        else
                        {
                            var highlighter = airdrop.gameObject.GetComponent<Highlighter>();

                            if (highlighter != null)
                                highlighter.ConstantOffImmediate();
                        }
                    }
                    else
                    {
                        var highlighter = airdrop.gameObject.GetComponent<Highlighter>();
                    
                        if(highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                
            }
        }

        internal static void UpdateNpcGlow()
        {

            foreach (var npc in Npcs)
            {
                var mypos = Player.player.transform.position;
                var targetpos = npc.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (menu.EnableEsp && menu.GlowInteractables && menu.Npc)
                {
                    if (dist <= menu.Distance || menu.InfDistance)
                    {
                        var highlighter = npc.gameObject.GetComponent<Highlighter>();

                        if (highlighter == null)
                            highlighter = npc.gameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(menu.InteractableGlow);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        var highlighter = npc.gameObject.GetComponent<Highlighter>();

                        if (highlighter != null)
                            highlighter.ConstantOffImmediate();
                    }
                }
                else
                {
                    var highlighter = npc.gameObject.GetComponent<Highlighter>();
                    
                    if(highlighter != null)
                        highlighter.ConstantOffImmediate();
                }
            }
        }
        
        //------------------Env-------------------------------------------------------------------------------

        
        
        //------------------Misc----------------------------------------------------------------------------------
        
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
        
        internal static void DrawBox(Transform target, Vector3 position, Color color)
        {
            var  targetPos = GetTargetVector(target, "Skull");
            var  scrnPt = Camera.main.WorldToScreenPoint(targetPos);
            scrnPt.y = Screen.height - scrnPt.y;
            
            var  dist = Math.Abs(position.y - scrnPt.y);
            var  hlfDist = dist / 2f;
            var  xVal = position.x - hlfDist / 2f;
            var  yVal = position.y - dist;
            
            GL.PushMatrix();
            GL.Begin(1);
            boxMaterial.SetPass(0);
            GL.Color(color);
            
            GL.Vertex3(xVal, yVal, 0f);
            GL.Vertex3(xVal, yVal + dist, 0f);
            GL.Vertex3(xVal, yVal, 0f);
            GL.Vertex3(xVal + hlfDist, yVal, 0f);
            GL.Vertex3(xVal + hlfDist, yVal, 0f);
            GL.Vertex3(xVal + hlfDist, yVal + dist, 0f);
            GL.Vertex3(xVal, yVal + dist, 0f);
            GL.Vertex3(xVal + hlfDist, yVal + dist, 0f);
            
            GL.End();
            GL.PopMatrix();
        }

        internal static void DrawTracers()
        {
            
        }

        internal static void GenerateDicts()
        {
            StorageIds = new Dictionary<int, string>();
            
            StorageIds.Add(367, "Birch Crate");
            StorageIds.Add(366, "Maple Crate");
            StorageIds.Add(368, "Pine Crate");
            
            StorageIds.Add(328, "Locker");
            
            StorageIds.Add(1246, "Birch Counter");
            StorageIds.Add(1245, "Maple Counter");
            StorageIds.Add(1247, "Pine Counter");
            StorageIds.Add(1248, "Metal Counter");
            
            StorageIds.Add(1279, "Birch Wardrobe");
            StorageIds.Add(1278, "Maple Wardrobe");
            StorageIds.Add(1280, "Pine Wardrobe");
            StorageIds.Add(1281, "Metal Wardrobe");
            
            StorageIds.Add(1206, "Birch Trophy Case");
            StorageIds.Add(1205, "Maple Trophy Case");
            StorageIds.Add(1207, "Pine Trophy Case");
            StorageIds.Add(1221, "Metal Trophy Case");
            
            StorageIds.Add(1203, "Birch Rifle Rack");
            StorageIds.Add(1202, "Maple Rifle Rack");
            StorageIds.Add(1204, "Pine Rifle Rack");
            StorageIds.Add(1220, "Metal Rifle Rack");
            
            StorageIds.Add(1283, "Cooler");
            StorageIds.Add(1249, "Fridge");
            
            
            DoorIds = new Dictionary<int, string>();
            
            DoorIds.Add(282,"Birch Door");
            DoorIds.Add(281,"Maple Door");
            DoorIds.Add(283,"Pine Door");
            DoorIds.Add(378,"Metal Door");
            
            DoorIds.Add(284, "Jail Door");
            DoorIds.Add(286, "Vault Door");
            
            DoorIds.Add(1236, "Birch Double Doors");
            DoorIds.Add(1235, "Maple Double Doors");
            DoorIds.Add(1237, "Pine Double Doors");
            DoorIds.Add(1238, "Metal Double Doors");
            
            DoorIds.Add(1330, "Birch Hatch");
            DoorIds.Add(1331, "Pine Hatch");
            DoorIds.Add(1332, "Metal Hatch");
            DoorIds.Add(1329, "Maple Hatch");

        }
    }
}