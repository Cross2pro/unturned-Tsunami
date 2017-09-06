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
        
        internal static Dictionary<int, string> StorageIds;
        internal static Dictionary<int, string> DoorIds;
        
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

        internal static Material BoxMaterial;
        
        public static void Start()
        {
            Menu = WaveMaker.MenuVisuals;
            
            var material = new Material(Shader.Find("Hidden/Internal-Colored"));
            material.hideFlags = (HideFlags) 61;
            BoxMaterial = material;
            BoxMaterial.SetInt("_SrcBlend", 5);
            BoxMaterial.SetInt("_DstBlend", 10);
            BoxMaterial.SetInt("_Cull", 0);
            BoxMaterial.SetInt("_ZWrite", 0);
            
            GenerateDicts();
        }
        
        public static void Update() 
        {
            //TODO: add update delay
            
            if (Provider.isConnected)
            {

                    if (Menu.GlowPlayers && Menu.EnableEsp)
                    {
                        Players = new List<SteamPlayer>();
                        Players = Provider.clients;

                        var friends = new List<SteamPlayer>();
                        var enemies = new List<SteamPlayer>();

                        foreach (var user in Players)
                        {
                            if (WaveMaker.Friends.Contains(user.playerID.steamID.m_SteamID))
                                friends.Add(user);
                            else
                                enemies.Add(user);
                        }

                        EnableGlowGeneric(GlowItem.GetListPlayer(friends), Menu.FriendlyPlayerGlow);
                        EnableGlowGeneric(GlowItem.GetListPlayer(enemies), Menu.EnemyPlayerGlow);
                    }
                    else
                    {
                        DisableGlowGeneric(GlowItem.GetListPlayer(Players));
                        Players = new List<SteamPlayer>();
                    }

                    if (Menu.GlowZombies && Menu.EnableEsp)
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

                    if (Menu.GlowVehicles && Menu.EnableEsp)
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


                    if (Menu.GlowItems && Menu.EnableEsp)
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

                    if (Menu.Animals && Menu.EnableEsp)
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

                    if (Menu.Storages && Menu.EnableEsp)
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

                    if (Menu.Bed && Menu.EnableEsp)
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

                    if (Menu.Doors && Menu.EnableEsp)
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

                    if (Menu.Flag && Menu.EnableEsp)
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

                    if (Menu.Sentries && Menu.EnableEsp)
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

                    if (Menu.Npc && Menu.EnableEsp)
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

        internal static void OnGUI()
        {
            CheckLabels();
            CheckBoxes();
        }


        public static void EnableGlowGeneric(List<GlowItem> list, Color glowColor)
        {
            foreach (var go in list)
            {
                if (Camera.main != null && go.GameObject != null)
                {
                    var mypos = Camera.main.gameObject.transform.position;
                    var targetpos = go.GameObject.transform.position;

                    if (Vector3.Distance(mypos, targetpos) <= Menu.Distance || Menu.InfDistance)
                    {
                        var highlighter = go.GameObject.GetComponent<Highlighter>() ??
                                          go.GameObject.AddComponent<Highlighter>();
                        
                        highlighter.ConstantParams(glowColor);
                        highlighter.OccluderOn();
                        highlighter.SeeThroughOn();
                        highlighter.ConstantOn();
                    }
                    else
                    {
                        if(go.GameObject.GetComponent<Highlighter>() != null)
                        {
                            var highlighter = go.GameObject.GetComponent<Highlighter>();
                            
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
                if(go.GameObject.GetComponent<Highlighter>() != null)
                    go.GameObject.GetComponent<Highlighter>().ConstantOffImmediate();
                    
            }
        }
        
        internal static void CheckBoxes()
        {
            if (Menu.PlayerBox)
            {
                
                foreach (var client in Provider.clients)
                {
                    if (client.player == Player.player) continue;
                    
                    var dist = Vector3.Distance(Camera.main.transform.position, client.player.transform.position);
                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        var scrnpt = Camera.main.WorldToScreenPoint(client.player.transform.position);
                        if (scrnpt.z >= 0)
                        {
                            scrnpt.y = Screen.height - scrnpt.y;
                            var color = WaveMaker.Friends.Contains(client.playerID.steamID.m_SteamID)
                                ? Menu.BoxPlayerFriendly
                                : Menu.BoxPlayerEnemy;
                            DrawBox(client.player.transform, scrnpt, color);
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
                        var dist = Vector3.Distance(Camera.main.transform.position, zombie.transform.position);
                        if (dist <= Menu.Distance || Menu.InfDistance)
                        {
                            var pos = Camera.main.WorldToScreenPoint(zombie.transform.position);

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
            if (Menu.EnableEsp)
            {
                if (Menu.PlayerName || Menu.PlayerWeapon || Menu.PlayerDistance)
                {
                    Players = Provider.clients;
                    UpdatePlayerLabels();
                }
                else
                    Players = null;
                    
                if (Menu.ZombieName || Menu.ZombieDistance || Menu.ZombieSpecialty)
                {
                    Zombies = new List<Zombie>();
                    foreach (var item in ZombieManager.regions)
                    {
                        foreach (var zo in item.zombies)
                        {
                            Zombies.Add(zo);
                        }
                    }
                    UpdateZombieLabels();
                }
    
                if (Menu.AnimalName || Menu.AnimalDistance)
                {
                    Animals = AnimalManager.animals;
                    UpdateAnimalLabels();
                }
    
                if (Menu.StorageType || Menu.StorageDistance)
                {
                    Storages = Object.FindObjectsOfType<InteractableStorage>().ToList(); 
                    UpdateStorageLabels();
                }
    
                if (Menu.VehicleName || Menu.VehicleDistance)
                {
                    Vehicles = VehicleManager.vehicles;
                    UpdateVehicleLabels();
                }
    
                if (Menu.ItemName || Menu.ItemDistance)
                {
                    Items = Object.FindObjectsOfType<InteractableItem>().ToList();
                    UpdateItemLabels();
                }
    
                if (Menu.NpcName || Menu.NpcDistance /*|| menu.NpcWeapon*/)
                {
                    Npcs = Object.FindObjectsOfType<InteractableObjectNPC>().ToList();
                    UpdateNpcLabels();
                }
    
                if (Menu.ForageType || Menu.ForageDistance)
                {
                    Forages = Object.FindObjectsOfType<InteractableForage>().ToList();
                    UpdateForageLabels();
                }
    
                if (Menu.BedType || Menu.BedDistance)
                {
                    Beds = Object.FindObjectsOfType<InteractableBed>().ToList();
                    UpdateBedLabels();
                }
    
                if (Menu.FlagType || Menu.FlagDistance)
                {
                    Claims = Object.FindObjectsOfType<InteractableClaim>().ToList();
                    UpdateFlagLabels();
                }
    
                if (Menu.SentryType || Menu.SentryWeapon || Menu.SentryState || Menu.SentryDistance)
                {
                    Sentries = Object.FindObjectsOfType<InteractableSentry>().ToList();
                    UpdateSentryLabels();
                }
    
                if (Menu.Admins)
                {
                    Players = Provider.clients;
                    UpdateAdminLabels();
                }    
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
                                
                                 
                                
                                GUI.Label(new Rect(scrnPt + new Vector3(0,4f,0), new Vector2(170,50)), $"<size={size}>{text}</size>" );        
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
            foreach (var flag in Claims)
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
                if (player.isAdmin && player.player != Player.player)
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
    }
}