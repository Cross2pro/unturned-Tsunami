using System;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using Object = UnityEngine.Object;

namespace TsunamiHack.Tsunami.Lib
{
    internal class Visuals 
    {
        //TODO: fix positioning of labels
        //TODO: maybe add other boxes
        //TODO: create a maximum zombies to show value
        //TODO: change menu sizes to a proportion and add scrollbars into all menus
        //TODO: Fix admin label positioning 
        
        internal static Menu.Visuals Menu;

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

        internal static float Altitiude;
        
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
        
        internal static Material BoxMaterial;
        
        internal static void Start(Menu.Visuals parent)
        {
            var material = new Material(Shader.Find("Hidden/Internal-Colored"));
            material.hideFlags = (HideFlags) 61;
            BoxMaterial = material;
            BoxMaterial.SetInt("_SrcBlend", 5);
            BoxMaterial.SetInt("_DstBlend", 10);
            BoxMaterial.SetInt("_Cull", 0);
            BoxMaterial.SetInt("_ZWrite", 0);

            GenerateDicts();
            
            Menu = WaveMaker.MenuVisuals;
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
            
            if ((DateTime.Now - Last).TotalMilliseconds >= Menu.UpdateRate)
            {
                CheckGlows();                     
                Last = DateTime.Now;
            }      
        }

        // ReSharper disable once InconsistentNaming
        internal static void OnGUI()
        {
            CheckLabels();
            CheckBoxes();
        }

        //----------------------Labels/boxes---------------------------------------------------------------------
        
        internal static void CheckBoxes()
        {
            if (Menu.PlayerBox)
            {
                foreach (var player in Players)
                {
                    if (!player.player.life.isDead && player.player != Player.player)
                    {
                        var mypos = Camera.main.transform.position;
                        var targetpos = player.player.transform.position;
                        var dist = Vector3.Distance(mypos, targetpos);

                        if (dist <= Menu.Distance || Menu.InfDistance)
                        {
                            var pos = Camera.main.WorldToScreenPoint(targetpos);

                            if (pos.z >= 0)
                            {
                                pos.y = Screen.height - pos.y;
                                
                                var color = WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID) ? Menu.BoxPlayerFriendly : Menu.BoxPlayerEnemy;
                                
                                DrawBox(player.player.transform, pos, color);
                            }
                        }
                    }
                }           
            }

            if (Menu.ZombieBox)
            {
                foreach (var zombie in Zombies)
                {
                    if (!zombie.isDead)
                    {
                        var mypos = Camera.main.transform.position;
                        var targetpos = zombie.transform.position;
                        var dist = Vector3.Distance(mypos, targetpos);

                        if (dist <= Menu.Distance || Menu.InfDistance)
                        {
                            var pos = zombie.transform.position;
                            pos = Camera.main.WorldToScreenPoint(pos);

                            if (pos.z >= 0)
                            {
                                pos.y = Screen.height - pos.y;

                                DrawBox(zombie.transform, pos, Menu.BoxZombie);
                            }
                        }         
                    }   
                }
            }
        }

        internal static void CheckLabels()
        {
             
            if (Menu.PlayerName || Menu.PlayerWeapon || Menu.PlayerDistance)
            {
                UpdatePlayerLabels();
            }
                
            if (Menu.ZombieName || Menu.ZombieDistance || Menu.ZombieSpecialty)
            {
                UpdateZombieLabels();
            }

            if (Menu.AnimalName || Menu.AnimalDistance)
            {
                UpdateAnimalLabels();
            }

            if (Menu.StorageType || Menu.StorageDistance)
            {
                UpdateStorageLabels();
            }

            if (Menu.VehicleName || Menu.VehicleDistance)
            {
                UpdateVehicleLabels();
            }

            if (Menu.ItemName || Menu.ItemDistance)
            {
                UpdateItemLabels();
            }

            if (Menu.NpcName || Menu.NpcDistance /*|| menu.NpcWeapon*/)
            {
                UpdateNpcLabels();
            }

            if (Menu.ForageType || Menu.ForageDistance)
            {
                UpdateForageLabels();
            }

            if (Menu.BedType || Menu.BedDistance)
            {
                UpdateBedLabels();
            }

            if (Menu.FlagType || Menu.FlagDistance)
            {
                UpdateFlagLabels();
            }

            if (Menu.SentryType || Menu.SentryWeapon || Menu.SentryState || Menu.SentryDistance)
            {
                UpdateSentryLabels();
            }

            if (Menu.Admins)
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
                    var myPos = Camera.main.transform.position;
                    var targetpos = player.player.transform.position;
                    var dist = Vector3.Distance(myPos, targetpos);

                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        targetpos += new Vector3(0f,3f,0f);
                        var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                        if (scrnpt.z >= 0)
                        {
                            scrnpt.y = Screen.height - scrnpt.y;
                            var text = "";

                            if (Menu.PlayerName)
                            {
                                text += $"{player.playerID.nickName}";
                            }

                            if (Menu.PlayerWeapon)
                            {
                                if (text.Length > 0)
                                    text += "\n";
                                
                                if (player.player.equipment.asset != null)
                                {
                                    if (player.player.equipment.asset.type == EItemType.GUN || player.player.equipment.asset.type == EItemType.MELEE)
                                        text += $"Weapon: {player.player.equipment.asset.name}";
                                    else
                                        text += $"Weapon: None";
                                }
                                else
                                    text += "Weapon: None";
                                 
                            }

                            if (Menu.PlayerDistance)
                            {
                                if (text.Length > 0)
                                    text += $"\nDistance: {Math.Round(dist, 0)}";
                                else
                                    text += $"Distance: {Math.Round(dist, 0)}";
                            }
                            
                            
                            float size;

                            if (Menu.ScaleText)
                                size = dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize;
                            else
                                size = 10f;

                            var color = WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID)
                                ? Menu.FriendlyPlayerGlow
                                : Menu.EnemyPlayerGlow;
                        
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
                        var myPos = Camera.main.transform.position;
                        var targetPos = zombie.transform.position;
                        var dist = Vector3.Distance(myPos, targetPos);
    
                        if (dist <= Menu.Distance || Menu.InfDistance)
                        {
                            targetPos += new Vector3(0f,3f, 0f);
                            var scrnPt = Camera.main.WorldToScreenPoint(targetPos);
    
                            if (scrnPt.z >= 0)
                            {
    
                                scrnPt.y = Screen.height - scrnPt.y;
    
                                var text = "";
    
                                if (Menu.ZombieName)
                                {
                                    text += $"{zombie.gameObject.name}";
                                }
    
                                if (Menu.ZombieDistance)
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
    
                                if (Menu.ZombieSpecialty)
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
                                
                                if (Menu.ScaleText)
                                {
                                    size = dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize;
                                }
                                else
                                {                                    
                                    size = 10;
                                }
                                
                                 
                                
                                GUI.Label(new Rect(scrnPt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={ColorUtility.ToHtmlStringRGBA(Menu.BoxZombie)}><size={size}>{text}</size></color>" );        
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
                    var mypos = Camera.main.transform.position;
                    var targetpos = animal.transform.position;
                    var dist = Vector3.Distance(mypos, targetpos);

                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        targetpos += new Vector3(0f,3f,0f);
                        var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                        if (scrnpt.z >= 0f)
                        {
                            scrnpt.y = Screen.height - scrnpt.y;

                            var text = "";

                            if (Menu.AnimalName)
                            {
                                text += $"{animal.asset.animalName}";
                            }

                            if (Menu.AnimalDistance)
                            {
                                if (text.Length > 0)
                                    text += $"\nDistance: {Math.Round(dist, 0)}";
                                else
                                    text += $"Distance: {Math.Round(dist, 0)}"; 
                            }

                            var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;
                            
                            GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={Menu.InteractableGlow}><size={size}>{text}</size></color>" );
                        }
                    }
                }
            }
        }

        internal static void UpdateStorageLabels()
        {
            foreach (var storage in Storages)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = storage.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f,1.5f,0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.StorageType)
                        {
                            text += $"Storage: {StorageIds[int.Parse(storage.name)]}";
                        }

                        if (Menu.StorageDistance)
                        {
                            text += text.Length > 0
                                ? $"\nDistance: {Math.Round(dist, 0)}"
                                : $"Distance: {Math.Round(dist, 0)}";
                        }

                        
                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;
                            
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={Menu.InteractableGlow}><size={size}>{text}</size></color>" );
                        
                    }

                }
                
            }
        }

        internal static void UpdateVehicleLabels()
        {
            foreach (var vehicle in Vehicles)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = vehicle.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f,1.5f,0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.VehicleName)
                        {
                            var name = vehicle.asset.name.Replace("_", " ");

                            if (name.Contains("Tractor"))
                                name = "Tractor";

                            if (name.Contains("Police"))
                                name = "Police Car";
                            
                            text += $"Vehicle: {name}";
                        }

                        if (Menu.VehicleDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";
                            
                            text += $"Distance: {Math.Round(dist,0)}";
                        }

                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;
                        
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={Menu.InteractableGlow}><size={size}>{text}</size></color>" );
                    }
                }
            }
        }

        internal static void UpdateItemLabels()
        {
            foreach (var item in Items)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = item.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f,1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.ItemName)
                        {
                            text += $"Item: {item.asset.itemName}";
                        }

                        if (Menu.ItemDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                            
                        }
                        
                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;
                        
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={Menu.InteractableGlow}><size={size}>{text}</size></color>" );
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
                var mypos = Camera.main.transform.position;
                var targetpos = npc.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f,1.5f,0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.NpcName)
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

                        if (Menu.NpcDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }

                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;
                        
                        GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={Menu.InteractableGlow}><size={size}>{text}</size></color>" );

                    }
                }
            }
        }

        internal static void UpdateForageLabels()
        {
            foreach (var forage in Forages)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = forage.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.ForageType)
                        {
                            text += "Forage";
                        }

                        if (Menu.ForageDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }
                        
                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;
                        
                        if(text.Length > 0)
                            GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={ColorUtility.ToHtmlStringRGBA(Menu.InteractableGlow)}><size={size}>{text}</size></color>" );
                    }
                }

            }
        }

        //Possibly add owner of bed?
        internal static void UpdateBedLabels()
        {
            foreach (var bed in Beds)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = bed.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.BedType)
                        {
                            text += "Bed";
                        }

                        if (Menu.BedDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }
                        
                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;
                        
                        if(text.Length > 0)
                            GUI.Label(new Rect(scrnpt + new Vector3(0,4f,0), new Vector2(170,50)), $"<color={ColorUtility.ToHtmlStringRGBA(Menu.InteractableGlow)}><size={size}>{text}</size></color>" );
                    }
                }

            }
        }

        internal static void UpdateDoorLabels()
        {
            foreach (var door in Doors)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = door.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.DoorType)
                        {
                            text += $"Door: {DoorIds[int.Parse(door.name)]}";
                        }

                        if (Menu.BedDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }

                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;

                        if(text.Length > 0)
                            GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color={ColorUtility.ToHtmlStringRGBA(Menu.InteractableGlow)}><size={size}>{text}</size></color>");
                    }
                }
            }
        }

        internal static void UpdateFlagLabels()
        {
            foreach (var flag in Flags)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = flag.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.FlagType)
                        {
                            text += "Claim Flag";
                        }

                        if (Menu.FlagDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }

                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;

                        if(text.Length > 0)
                            GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color={ColorUtility.ToHtmlStringRGBA(Menu.InteractableGlow)}><size={size}>{text}</size></color>");
                    }
                }
            }
        }

        internal static void UpdateSentryLabels()
        {
            foreach (var sentry in Sentries)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = sentry.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.SentryType)
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

                        if (Menu.SentryState)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            var state = sentry.enabled ? "On" : "Off";
                            text += $"State: {state}";
                        }
                        
                        if (Menu.SentryDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"Distance: {Math.Round(dist, 0)}";
                        }

                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;

                        if(text.Length > 0)
                            GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 70)),
                            $"<color={ColorUtility.ToHtmlStringRGBA(Menu.InteractableGlow)}><size={size}>{text}</size></color>");
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
                    var mypos = Camera.main.transform.position;
                    var targetpos = player.player.transform.position;
                    var dist = Vector3.Distance(mypos, targetpos);

                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        targetpos += new Vector3(0f, 3f, 0f);
                        var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                        if (scrnpt.z >= 0f)
                        {
                            scrnpt.y = Screen.height - scrnpt.y;
                            var text = "";

                            if (Menu.Admins)
                            {
                                text += "ADMIN";
                            }

                            var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;

                            if(text.Length > 0)
                                GUI.Label(new Rect(scrnpt - new Vector3(0, 5.3f, 0), new Vector2(170, 50)),
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

                    var myPos = Camera.main.transform.position;
                    var targetPos = player.player.transform.position;

                    if (Menu.EnableEsp && Menu.GlowPlayers)
                    {
                        var dist = Vector3.Distance(myPos, targetPos);

                        if (dist <= Menu.Distance || Menu.InfDistance)
                        {
                            var highlighter = player.player.gameObject.GetComponent<Highlighter>() ?? player.player.gameObject.AddComponent<Highlighter>();

                            var color = WaveMaker.Friends.Contains(player.playerID.steamID.m_SteamID)
                                ? Menu.FriendlyPlayerGlow
                                : Menu.EnemyPlayerGlow;
                        
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
            if(Zombies.Length > 0)
                 foreach (var zombie in Zombies)
                 {
                     var myPos = Camera.main.transform.position;
                     var zomPos = zombie.transform.position;

                     if (Menu.EnableEsp && Menu.GlowZombies)
                     {
                         var dist = Vector3.Distance(myPos, zomPos);

                         if (dist <= Menu.Distance || Menu.InfDistance)
                         {
                             var highlighter = zombie.gameObject.GetComponent<Highlighter>() ?? zombie.gameObject.AddComponent<Highlighter>();

                             highlighter.ConstantParams(Menu.ZombieGlow);
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
                var myPos = Camera.main.transform.position;
                var zomPos = vehicle.transform.position;

                if (Menu.EnableEsp && Menu.GlowVehicles)
                {
                    var dist = Vector3.Distance(myPos, zomPos);

                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = vehicle.gameObject.GetComponent<Highlighter>() ?? vehicle.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.VehicleGlow);
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

                if (Menu.EnableEsp && Menu.GlowItems)
                {
                    var dist = Vector3.Distance(myPos, targetpos);

                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = item.gameObject.GetComponent<Highlighter>() ?? item.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.ItemGlow);
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
                
                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Animals)
                {
                    var myPos = Camera.main.transform.position;
                    var targetPos = animal.transform.position;
                    var dist = Vector3.Distance(myPos, targetPos);

                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = animal.gameObject.GetComponent<Highlighter>() ?? animal.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.InteractableGlow);
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
                var myPos = Camera.main.transform.position;
                var targetPos = storage.transform.position;
                var dist = Vector3.Distance(myPos, targetPos);

                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Storages)
                {
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = storage.gameObject.GetComponent<Highlighter>() ?? storage.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.InteractableGlow);
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
                var myPos = Camera.main.transform.position;
                var targetpos = forage.transform.position;
                var dist = Vector3.Distance(myPos, targetpos);

                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Forages)
                {
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = forage.gameObject.GetComponent<Highlighter>() ?? forage.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.InteractableGlow);
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
                var myPos = Camera.main.transform.position;
                
                var targetPos = bed.transform.position;
                var dist = Vector3.Distance(myPos, targetPos);

                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Bed)
                {
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = bed.gameObject.GetComponent<Highlighter>() ?? bed.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.InteractableGlow);
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
                var mypos = Camera.main.transform.position;
                var targetpos = door.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Doors)
                {
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = door.gameObject.GetComponent<Highlighter>() ?? door.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.InteractableGlow);
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
                var mypos = Camera.main.transform.position;
                var targetpos = trap.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Traps)
                {
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highligter = trap.gameObject.GetComponent<Highlighter>() ?? trap.gameObject.AddComponent<Highlighter>();

                        highligter.ConstantParams(Menu.InteractableGlow);
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
                var mypos = Camera.main.transform.position;
                var targetpos = flag.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Flag)
                {
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = flag.gameObject.GetComponent<Highlighter>() ?? flag.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.InteractableGlow);
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
                var mypos = Camera.main.transform.position;
                var targetpos = sentry.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Sentries)
                {
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = sentry.gameObject.GetComponent<Highlighter>() ?? sentry.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.InteractableGlow);
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
                    var mypos = Camera.main.transform.position;
                    var targetpos = airdrop.transform.position;
                    var dist = Vector3.Distance(mypos, targetpos);

                    if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Airdrop)
                    {
                        if (dist <= Menu.Distance || Menu.InfDistance)
                        {
                            var highlighter = airdrop.gameObject.GetComponent<Highlighter>() ?? airdrop.gameObject.AddComponent<Highlighter>();

                            highlighter.ConstantParams(Menu.InteractableGlow);
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
                var mypos = Camera.main.transform.position;
                var targetpos = npc.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (Menu.EnableEsp && Menu.GlowInteractables && Menu.Npc)
                {
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = npc.gameObject.GetComponent<Highlighter>() ?? npc.gameObject.AddComponent<Highlighter>();

                        highlighter.ConstantParams(Menu.InteractableGlow);
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
            EnemyPlayerGlow = Menu.EnemyPlayerGlow;
            FriendlyPlayerGlow = Menu.FriendlyPlayerGlow;
            ZombieGlow = Menu.ZombieGlow;
            ItemGlow = Menu.ItemGlow;
            InteractableGlow = Menu.InteractableGlow;
            VehicleGlow = Menu.VehicleGlow;

            BoxPlayerFriendly = Menu.BoxPlayerFriendly;
            BoxPlayerEnemy = Menu.BoxPlayerEnemy;
            BoxZombie = Menu.BoxZombie;
        }
        
        internal static void UpdateLists()
        {

                if (Menu.GlowPlayers || Menu.PlayerName || Menu.PlayerWeapon || Menu.PlayerDistance)
                    Players = Provider.clients.ToArray();
                if (Menu.GlowZombies || Menu.ZombieName || Menu.ZombieDistance || Menu.ZombieSpecialty)
                    Zombies = Object.FindObjectsOfType<Zombie>();
                if (Menu.GlowItems || Menu.ItemName || Menu.ItemDistance)
                    Items = Object.FindObjectsOfType<InteractableItem>();
                if (Menu.GlowVehicles || Menu.VehicleName || Menu.VehicleDistance)
                    Vehicles = Object.FindObjectsOfType<InteractableVehicle>();
                if (Menu.GlowInteractables && Menu.Animals || Menu.AnimalName || Menu.AnimalDistance)
                    Animals = Object.FindObjectsOfType<Animal>();
                if (Menu.GlowInteractables && Menu.Forages || Menu.ForageType || Menu.ForageDistance)
                    Forages = Object.FindObjectsOfType<InteractableForage>();
                if (Menu.GlowInteractables && Menu.Storages || Menu.StorageType || Menu.StorageDistance)
                    Storages = Object.FindObjectsOfType<InteractableStorage>();
                if (Menu.GlowInteractables && Menu.Bed || Menu.BedDistance)
                    Beds = Object.FindObjectsOfType<InteractableBed>();
                if (Menu.GlowInteractables && Menu.Doors || Menu.DoorType || Menu.DoorDistance)
                    Doors = Object.FindObjectsOfType<InteractableDoor>();
                if (Menu.GlowInteractables && Menu.Traps || Menu.TrapType || Menu.TrapDistance)
                    Traps = Object.FindObjectsOfType<InteractableTrap>();
                if (Menu.GlowInteractables && Menu.Flag || Menu.FlagDistance)
                    Flags = Object.FindObjectsOfType<InteractableClaim>();
                if (Menu.GlowInteractables && Menu.Sentries || Menu.SentryState || Menu.SentryWeapon ||
                    Menu.SentryDistance)
                    Sentries = Object.FindObjectsOfType<InteractableSentry>();
                if (Menu.GlowInteractables && Menu.Airdrop || Menu.AirdropDistance)
                    Airdrops = Object.FindObjectsOfType<Carepackage>();
                if (Menu.GlowInteractables && Menu.Npc || Menu.NpcName || Menu.NpcWeapon || Menu.NpcDistance)
                    Npcs = Object.FindObjectsOfType<InteractableObjectNPC>();
            
        }

        internal static Vector3 GetTargetVector(Transform target, string objName)
        {
            var componentsInChildren = target.transform.GetComponentsInChildren<Transform>();
            var result = Vector3.zero;

            if (componentsInChildren != null)
            {
                var array = componentsInChildren;
                foreach (var transform in array)
                {
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
            BoxMaterial.SetPass(0);
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
            StorageIds = new Dictionary<int, string>
            {
                {367, "Birch Crate"},
                {366, "Maple Crate"},
                {368, "Pine Crate"},
                {328, "Locker"},
                {1246, "Birch Counter"},
                {1245, "Maple Counter"},
                {1247, "Pine Counter"},
                {1248, "Metal Counter"},
                {1279, "Birch Wardrobe"},
                {1278, "Maple Wardrobe"},
                {1280, "Pine Wardrobe"},
                {1281, "Metal Wardrobe"},
                {1206, "Birch Trophy Case"},
                {1205, "Maple Trophy Case"},
                {1207, "Pine Trophy Case"},
                {1221, "Metal Trophy Case"},
                {1203, "Birch Rifle Rack"},
                {1202, "Maple Rifle Rack"},
                {1204, "Pine Rifle Rack"},
                {1220, "Metal Rifle Rack"},
                {1283, "Cooler"},
                {1249, "Fridge"}
            };

            DoorIds = new Dictionary<int, string>
            {
                {282, "Birch Door"},
                {281, "Maple Door"},
                {283, "Pine Door"},
                {378, "Metal Door"},
                {284, "Jail Door"},
                {286, "Vault Door"},
                {1236, "Birch Double Doors"},
                {1235, "Maple Double Doors"},
                {1237, "Pine Double Doors"},
                {1238, "Metal Double Doors"},
                {1330, "Birch Hatch"},
                {1331, "Pine Hatch"},
                {1332, "Metal Hatch"},
                {1329, "Maple Hatch"}
            };

        }
    }
}