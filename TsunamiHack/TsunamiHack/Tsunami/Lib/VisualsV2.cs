using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using HighlightingSystem;
using UnityEngine;
using SDG.Unturned;
using Steamworks;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Util;
using Object = UnityEngine.Object;
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace TsunamiHack.Tsunami.Lib
{
    internal class VisualsV2
    {
        internal struct ESPObject
        {
            public GameObject value;
            public ulong steamID;

            internal ESPObject(ref GameObject go)
            {
                value = go;
                steamID = 0L;
            }

            internal ESPObject(ref GameObject go, ulong id)
            {
                value = new GameObject();
                steamID = id;
            }
        }

        internal static Menu.Visuals Menu;
        internal static float updateInterval;
        internal static DateTime updateLastUpdate;
        internal static DateTime guiLastUpdate;

        internal static float listupdateInterval = 5000f;

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

        internal static Material DrawingMaterial;

//        internal static Camera Camera.main;

        public static void Start()
        {
            Menu = WaveMaker.MenuVisuals;

            var material = new Material(Shader.Find("Hidden/Internal-Colored"));
            material.hideFlags = (HideFlags) 61;
            DrawingMaterial = material;
            DrawingMaterial.SetInt("_SrcBlend", 5);
            DrawingMaterial.SetInt("_DstBlend", 10);
            DrawingMaterial.SetInt("_Cull", 0);
            DrawingMaterial.SetInt("_ZWrite", 0);

            GenerateDicts();
            updateLastUpdate = DateTime.Now;
            guiLastUpdate = DateTime.Now;
            zomskeleint = 55;
        }

        public static void Update()
        {
            if (Event.current.type == EventType.repaint && (DateTime.Now - updateLastUpdate).TotalMilliseconds >= Menu.UpdateRate)
            {
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
                            if (WaveMaker.Friends.IsFriend(user.playerID.steamID.m_SteamID))
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

                    //Type:
                    /*
                    1- clothing
                    2- ammo
                    3- guns
                    4- attachments
                    5- food
                    6- medical
                    7- weapons
                    8- misc
                    9- backpack
                    */

                    if (Menu.GlowItems && Menu.EnableEsp)
                    {
                        Items = new List<InteractableItem>();
                        Items = Object.FindObjectsOfType<InteractableItem>().ToList();

                        var filterList = new List<InteractableItem>();
                        
                        foreach (var item in Items)
                        {
                            if( (Menu.FilterClothing && isOfType(1, item)) || (Menu.FilterAmmo && isOfType(2, item)) || (Menu.FilterGuns && isOfType(3, item)) || (Menu.FilterAttach && isOfType(4, item)) || (Menu.FilterFood && isOfType(5, item)) || (Menu.FilterMedical && isOfType(6, item)) || (Menu.FilterWeapons && isOfType(7, item)) || (Menu.FilterMisc && isOfType(8, item) || (Menu.FilterBackpacks && isOfType(9, item))))
                                filterList.Add(item);
                        }
                        
                        EnableGlowGeneric(GlowItem.GetListItem(filterList), Menu.ItemGlow);
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

        }

        internal static void OnGUI()
        {


            
            if ((DateTime.Now - guiLastUpdate).TotalMilliseconds >= Menu.UpdateRate)
            {
                if (Provider.isConnected && Event.current.type == EventType.repaint)
                {
                    CheckLabels();
                    CheckBoxes();

                }
            }

        }

        #region other esp

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
                        if (go.GameObject.GetComponent<Highlighter>() != null)
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
                if (go.GameObject.GetComponent<Highlighter>() != null)
                    go.GameObject.GetComponent<Highlighter>().ConstantOffImmediate();

            }
        }

        internal static void CheckBoxes()
        {
            if (Menu.EnableEsp && Provider.isConnected)
            {
                if (Menu.PlayerBox && Menu.EnableEsp)
                {

                    foreach (var client in Provider.clients)
                    {
                        if (client.player == Player.player) continue;
                        if (client.player.life.isDead) continue;

                        var dist = Vector3.Distance(Camera.main.transform.position, client.player.transform.position);
                        if (dist <= Menu.Distance || Menu.InfDistance)
                        {
                            var scrnpt = Camera.main.WorldToScreenPoint(client.player.transform.position);
                            if (scrnpt.z >= 0)
                            {
                                scrnpt.y = Screen.height - scrnpt.y;
                                var color = WaveMaker.Friends.IsFriend(client.playerID.steamID.m_SteamID)
                                    ? Menu.BoxPlayerFriendly
                                    : Menu.BoxPlayerEnemy;
                                DrawBox(client.player.transform, scrnpt, color);


                            }
                        }

                    }
                }
    
                Logging.Log("3");
                if (Menu.ZombieBox && Menu.EnableEsp)
                {
                    Logging.Log("2");
                    foreach (var zombie in Zombies)
                    {
                        Logging.Log("1");
                        if (!zombie.isDead)
                        {
                            Logging.Log("A");
                            var dist = Vector3.Distance(Camera.main.transform.position, zombie.transform.position);
                            Logging.Log("B");
                            if (dist <= Menu.Distance || Menu.InfDistance)
                            {
                                Logging.Log("C");
                                var pos = Camera.main.WorldToScreenPoint(zombie.transform.position);

                                Logging.Log("D");
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
                    
                    var filterList = new List<InteractableItem>();
                        
                    foreach (var item in Items)
                    {
                        if( (Menu.FilterClothing && isOfType(1, item)) || (Menu.FilterAmmo && isOfType(2, item)) || (Menu.FilterGuns && isOfType(3, item)) || (Menu.FilterAttach && isOfType(4, item)) || (Menu.FilterFood && isOfType(5, item)) || (Menu.FilterMedical && isOfType(6, item)) || (Menu.FilterWeapons && isOfType(7, item)) || (Menu.FilterMisc && isOfType(8, item) || (Menu.FilterBackpacks && isOfType(9, item)) ))
                        filterList.Add(item);
                    }
                    
                    UpdateItemLabels(filterList);
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
                    Logging.Log("Camera use 1");
                    var myPos = Camera.main.transform.position;
                    var targetpos = player.player.transform.position;
                    var dist = Vector3.Distance(myPos, targetpos);

                    if (dist <= Menu.Distance || Menu.InfDistance)
                    {
                        targetpos += new Vector3(0f, 3f, 0f);
                        Logging.Log("Camera use 2");
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
                                if (player.player.equipment.asset != null)
                                {
                                    if (text.Length > 0 && player.player.equipment.asset.type == EItemType.GUN ||
                                        player.player.equipment.asset.type == EItemType.MELEE)
                                        text += "\n";

                                    if (player.player.equipment.asset.type == EItemType.GUN ||
                                        player.player.equipment.asset.type == EItemType.MELEE)
                                        text += player.player.equipment.asset.name.Replace("_", " ");
                                }
                            }

                            if (Menu.PlayerDistance)
                            {
                                if (text.Length > 0)
                                    text += "\n";

                                text += $"[{Math.Round(dist, 0)}]";
                            }


                            float size;

                            if (Menu.ScaleText)
                                size = dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize;
                            else
                                size = 10f;

                            var color = WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID)
                                ? Menu.FriendlyPlayerGlow
                                : Menu.EnemyPlayerGlow;

                            var friend = WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID);

                            if (friend)
                                GUI.Label(new Rect(scrnpt + new Vector3(0, 6f, 0), new Vector2(170, 70)),
                                    $"<color=#00ffff><size={size}>{text}</size></color>");
                            else
                                GUI.Label(new Rect(scrnpt + new Vector3(0, 6f, 0), new Vector2(170, 70)),
                                    $"<color=#ff0000><size={size}>{text}</size></color>");
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
                        targetPos += new Vector3(0f, 3f, 0f);
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
                                    text += "\n";

                                text += $"[{Math.Round(dist, 0)}]";

                            }

                            if (Menu.ZombieSpecialty)
                            {
                                var str = "";

                                if (Menu.ZombieDistance)
                                {
                                    str = " || ";

                                    switch (zombie.speciality)
                                    {
                                        case EZombieSpeciality.NONE:
                                            str += "Normal";
                                            break;
                                        case EZombieSpeciality.NORMAL:
                                            str += "Normal";
                                            break;
                                        case EZombieSpeciality.MEGA:
                                            str += "Mega";
                                            break;
                                        case EZombieSpeciality.CRAWLER:
                                            str += "Crawler";
                                            break;
                                        case EZombieSpeciality.SPRINTER:
                                            str += "Sprinter";
                                            break;
                                        case EZombieSpeciality.FLANKER_FRIENDLY:
                                            str += "Friendly Flanker";
                                            break;
                                        case EZombieSpeciality.FLANKER_STALK:
                                            str += "Stalking Flanker";
                                            break;
                                        case EZombieSpeciality.BURNER:
                                            str += "Burner";
                                            break;
                                        case EZombieSpeciality.ACID:
                                            str += "Acid";
                                            break;
                                        case EZombieSpeciality.BOSS_ELECTRIC:
                                            str += "Electric (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_WIND:
                                            str += "Wind (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_FIRE:
                                            str += "Fire (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_ALL:
                                            str += "All (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_MAGMA:
                                            str += "Magma (Boss)";
                                            break;
                                    }
                                }
                                else
                                {
                                    text += "\n";

                                    switch (zombie.speciality)
                                    {
                                        case EZombieSpeciality.NONE:
                                            str += "Normal";
                                            break;
                                        case EZombieSpeciality.NORMAL:
                                            str += "Normal";
                                            break;
                                        case EZombieSpeciality.MEGA:
                                            str += "Mega";
                                            break;
                                        case EZombieSpeciality.CRAWLER:
                                            str += "Crawler";
                                            break;
                                        case EZombieSpeciality.SPRINTER:
                                            str += "Sprinter";
                                            break;
                                        case EZombieSpeciality.FLANKER_FRIENDLY:
                                            str += "Friendly Flanker";
                                            break;
                                        case EZombieSpeciality.FLANKER_STALK:
                                            str += "Stalking Flanker";
                                            break;
                                        case EZombieSpeciality.BURNER:
                                            str += "Burner";
                                            break;
                                        case EZombieSpeciality.ACID:
                                            str += "Acid";
                                            break;
                                        case EZombieSpeciality.BOSS_ELECTRIC:
                                            str += "Electric (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_WIND:
                                            str += "Wind (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_FIRE:
                                            str += "Fire (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_ALL:
                                            str += "All (Boss)";
                                            break;
                                        case EZombieSpeciality.BOSS_MAGMA:
                                            str += "Magma (Boss)";
                                            break;
                                    }
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



                            GUI.Label(new Rect(scrnPt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                                $"<color=#00ff00><size={size}>{text}</size></color>");
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
                        targetpos += new Vector3(0f, 3f, 0f);
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

                            GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                                $"<color={Menu.InteractableGlow}><size={size}>{text}</size></color>");
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
                    targetpos += new Vector3(0f, 1.5f, 0f);
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

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color=#ffff00><size={size}>{text}</size></color>");

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
                    targetpos += new Vector3(0f, 1.5f, 0f);
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

                            text += $"{name}";
                        }

                        if (Menu.VehicleDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"[{Math.Round(dist, 0)}]";
                        }

                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color=#ff00ff><size={size}>{text}</size></color>");
                    }
                }
            }
        }

        internal static void UpdateItemLabels(List<InteractableItem> inputlist)
        {
            foreach (var item in inputlist)
            {
                var mypos = Camera.main.transform.position;
                var targetpos = item.transform.position;
                var dist = Vector3.Distance(mypos, targetpos);

                if (dist <= Menu.Distance || Menu.InfDistance)
                {
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Camera.main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (Menu.ItemName)
                        {
                            text += $"{item.asset.itemName}";
                        }

                        if (Menu.ItemDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"[{Math.Round(dist, 0)}]";

                        }

                        var size = Menu.ScaleText ? dist <= Menu.Dropoff ? Menu.CloseSize : Menu.FarSize : 10f;

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color=#ffff00><size={size}>{text}</size></color>");
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
                    targetpos += new Vector3(0f, 1.5f, 0f);
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

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color={Menu.InteractableGlow}><size={size}>{text}</size></color>");

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

                        if (text.Length > 0)
                            GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                                $"<color={ColorUtility.ToHtmlStringRGBA(Menu.InteractableGlow)}><size={size}>{text}</size></color>");
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

                        if (text.Length > 0)
                            GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                                $"<color={ColorUtility.ToHtmlStringRGBA(Menu.InteractableGlow)}><size={size}>{text}</size></color>");
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

                        if (text.Length > 0)
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

                        if (text.Length > 0)
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

                        if (text.Length > 0)
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

                            if (text.Length > 0)
                                GUI.Label(new Rect(scrnpt - new Vector3(0, 5.3f, 0), new Vector2(170, 50)),
                                    $"<color=#FF0000><size={size}>{text}</size></color>");
                        }
                    }
                }
            }
        }

        #endregion


        internal static void CheckTracers()
        {

            if (Menu.Tracers)
            {
                var zoms = new List<Zombie>();
                foreach (var region in ZombieManager.regions)
                {
                    foreach (var zombie in region.zombies)
                    {
                        zoms.Add(zombie);
                    }
                }

                foreach (var zombie in zoms)
                {
                    if (Vector3.Distance(zombie.transform.position, Camera.main.transform.position) >= 150) continue;

                    var centerpt = new Vector2(Screen.width / 2, Screen.height / 2);
                    Vector3 spinepos = Vector3.zero;
                    Vector3 skullpos = Vector3.zero;

                    var components = zombie.transform.GetComponentsInChildren<Transform>();
                    foreach (var comp in components)
                    {
                        if (comp.name.Trim() == "Spine")
                            spinepos = comp.position;
//		                if (comp.name.Trim() == "Skull")
//		                    skullpos = comp.position;
                    }

//		            var vp = Camera.main.WorldToViewportPoint(skullpos);
//		            if (vp.z <= 0f || vp.x <= 0f || vp.x >= 1f || vp.y <= 0f || vp.y >= 1f) continue;

//		            spinepos.y = skullpos.y - spinepos.y;

                    var spinescreenpos = Camera.main.WorldToScreenPoint(spinepos);
                    spinescreenpos.y = Screen.height - spinescreenpos.y;

                    GL.PushMatrix();
                    GL.Begin(1);
                    DrawingMaterial.SetPass(0);
                    GL.Color(Color.red);

                    GL.Vertex3(centerpt.x, centerpt.y, 0f);
                    GL.Vertex3(spinescreenpos.x, spinescreenpos.y, 0f);

                    GL.End();
                    GL.PopMatrix();
                }
            }
        }


        private static int zomskeleint;

        internal static void CheckSkeleton()
        {

            if (Menu.EnablePlayerSkeleton && (DateTime.Now - guiLastUpdate).TotalMilliseconds > 200)
            {

                foreach (var player in Provider.clients)
                {
                    if (!player.player.life.isDead)
                    {
                        if (Vector3.Distance(Camera.main.transform.position, player.player.transform.position) <=
                            Menu.Distance)
                        {
                            var color = WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID)
                                ? new Color(0, 255, 255)
                                : new Color(255, 0, 0);

                            DrawSkeleton(player.player.transform, color);
                        }
                    }
                }

            }

            if (Menu.EnableZombieSkeleton && (DateTime.Now - guiLastUpdate).TotalMilliseconds > 200)
            {
                if (zomskeleint > 50)
                {
                    Zombies = new List<Zombie>();
                    foreach (var region in ZombieManager.regions)
                    {
                        foreach (var zombie in region.zombies)
                        {
                            Zombies.Add(zombie);
                        }
                    }
                    zomskeleint = 0;
                }
                else
                    zomskeleint++;

                foreach (var zombie in Zombies)
                {
                    if (!zombie.isDead)
                    {
                        if (Vector3.Distance(Camera.main.transform.position, zombie.transform.position) <=
                            Menu.Distance)
                        {
                            DrawSkeleton(zombie.transform, new Color(0, 255, 0));
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
            var targetPos = GetTargetVector(target, "Skull");
            var scrnPt = Camera.main.WorldToScreenPoint(targetPos);
            scrnPt.y = Screen.height - scrnPt.y;

            var dist = Math.Abs(position.y - scrnPt.y);
            var hlfDist = dist / 2f;
            var xVal = position.x - hlfDist / 2f;
            var yVal = position.y - dist;

            GL.PushMatrix();
            GL.Begin(1);
            DrawingMaterial.SetPass(0);
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

        internal static void DrawSkeleton(Transform input, Color color)
        {
            //Insure that people who arent on the visible screen arent shown
            var visible = Camera.main.WorldToViewportPoint(input.position);
            if (visible.z <= 0f || visible.x <= 0f || visible.x >= 1f || visible.y <= 0f || visible.y >= 1f) return;

            //Set up all the transforms
            Transform skull = null;
            Transform spine = null;

            Transform left_shoulder = null;
            Transform right_shoulder = null;

            Transform left_arm = null;
            Transform right_arm = null;

            Transform left_hand = null;
            Transform right_hand = null;

            Transform left_leg = null;
            Transform right_leg = null;

            Transform left_foot = null;
            Transform right_foot = null;

            //Get components and assign each to their respective variables
            var components = input.GetComponentsInChildren<Transform>();
            foreach (var co in components)
            {
                switch (co.name.Trim())
                {
                    case "Skull":
                        skull = co;
                        break;

                    case "Spine":
                        spine = co;
                        break;

                    case "Left_Shoulder":
                        left_shoulder = co;
                        break;

                    case "Right_Shoulder":
                        right_shoulder = co;
                        break;

                    case "Left_Arm":
                        left_arm = co;
                        break;

                    case "Right_Arm":
                        right_arm = co;
                        break;

                    case "Left_Hand":
                        left_hand = co;
                        break;

                    case "Right_Hand":
                        right_hand = co;
                        break;

                    case "Left_Leg":
                        left_leg = co;
                        break;

                    case "Right_Leg":
                        right_leg = co;
                        break;

                    case "Left_Foot":
                        left_foot = co;
                        break;

                    case "Right_Foot":
                        right_foot = co;
                        break;
                }
            }


            //Setup the screen positions
            var skullpos = Camera.main.WorldToScreenPoint(skull.position);
            skullpos.y = Screen.height - skullpos.y;


            var skullelevatedpos =
                Camera.main.WorldToScreenPoint(new Vector3(skull.position.x, skull.position.y + (float) 0.25,
                    skull.position.z));
            skullelevatedpos.y = Screen.height - skullelevatedpos.y;

            var spinepos = Camera.main.WorldToScreenPoint(spine.position);
            spinepos.y = Screen.height - spinepos.y;

            var rshoulderpos = Camera.main.WorldToScreenPoint(right_shoulder.position);
            rshoulderpos.y = Screen.height - rshoulderpos.y;

            var lshoulderpos = Camera.main.WorldToScreenPoint(left_shoulder.position);
            lshoulderpos.y = Screen.height - lshoulderpos.y;

            var rarmpos = Camera.main.WorldToScreenPoint(right_arm.position);
            rarmpos.y = Screen.height - rarmpos.y;

            var rhandpos = Camera.main.WorldToScreenPoint(right_hand.position);
            rhandpos.y = Screen.height - rhandpos.y;

            var lhandpos = Camera.main.WorldToScreenPoint(left_hand.position);
            lhandpos.y = Screen.height - lhandpos.y;

            var larmpos = Camera.main.WorldToScreenPoint(left_arm.position);
            larmpos.y = Screen.height - larmpos.y;


            var llegpos = Camera.main.WorldToScreenPoint(left_leg.position);
            llegpos.y = Screen.height - llegpos.y;


            var rlegpos = Camera.main.WorldToScreenPoint(right_leg.position);
            rlegpos.y = Screen.height - rlegpos.y;

            var rfootpos = Camera.main.WorldToScreenPoint(right_foot.position);
            rfootpos.y = Screen.height - rfootpos.y;

            var lfootpos = Camera.main.WorldToScreenPoint(left_foot.position);
            lfootpos.y = Screen.height - lfootpos.y;

            //Draw it
            GL.PushMatrix();
            GL.Begin(1);
            DrawingMaterial.SetPass(0);
            GL.Color(color);

            //skull to spine
            GL.Vertex3(skullpos.x, skullpos.y, 0f);
            GL.Vertex3(spinepos.x, spinepos.y, 0f);

            //skull to right shoulder
            GL.Vertex3(skullpos.x, skullpos.y, 0f);
            GL.Vertex3(rshoulderpos.x, rshoulderpos.y, 0f);

            //skull to left shoulder
            GL.Vertex3(skullpos.x, skullpos.y, 0f);
            GL.Vertex3(lshoulderpos.x, lshoulderpos.y, 0f);

            //right shoulder to right arm
            GL.Vertex3(rshoulderpos.x, rshoulderpos.y, 0f);
            GL.Vertex3(rarmpos.x, rarmpos.y, 0f);

            //right arm to right hand
            GL.Vertex3(rarmpos.x, rarmpos.y, 0f);
            GL.Vertex3(rhandpos.x, rhandpos.y, 0f);

            //left shoulder to left arm
            GL.Vertex3(lshoulderpos.x, lshoulderpos.y, 0f);
            GL.Vertex3(larmpos.x, larmpos.y, 0f);

            //left arm to left hand
            GL.Vertex3(larmpos.x, larmpos.y, 0f);
            GL.Vertex3(lhandpos.x, lhandpos.y, 0f);

            //spine to left leg
            GL.Vertex3(spinepos.x, spinepos.y, 0f);
            GL.Vertex3(llegpos.x, llegpos.y, 0f);

            //left leg to left foot
            GL.Vertex3(llegpos.x, llegpos.y, 0f);
            GL.Vertex3(lfootpos.x, lfootpos.y, 0f);

            //spine to right leg
            GL.Vertex3(spinepos.x, spinepos.y, 0f);
            GL.Vertex3(rlegpos.x, rlegpos.y, 0f);

            //right leg to right foot
            GL.Vertex3(rlegpos.x, rlegpos.y, 0f);
            GL.Vertex3(rfootpos.x, rfootpos.y, 0f);

            //skull to elevated skull 
            GL.Vertex3(skullpos.x, skullpos.y, 0f);
            GL.Vertex3(skullelevatedpos.x, skullelevatedpos.y, 0f);

            GL.End();
            GL.PopMatrix();
        }

        //Type:
        /*
        1- clothing
        2- ammo
        3- guns
        4- attachments
        5- food
        6- medical
        7- weapons
        8- misc
        9- backpack
        */
        
        public static  bool isOfType(int type, InteractableItem input)
        {
            if (type == 1)
            {
                if (input.asset.type == EItemType.HAT || input.asset.type == EItemType.PANTS ||
                    input.asset.type == EItemType.SHIRT || input.asset.type == EItemType.MASK || input.asset.type == EItemType.VEST ||
                    input.asset.type == EItemType.GLASSES)
                {
                    return true;
                }
            }
            else if (type == 2)
            {
                if (input.asset.type == EItemType.MAGAZINE || input.asset.type == EItemType.REFILL)
                {
                    return true;
                }
            }
            else if (type == 3)
            {
                if (input.asset.type == EItemType.GUN)
                {
                    return true;
                }
            }
            else if (type == 4)
            {
                if (input.asset.type == EItemType.SIGHT || input.asset.type == EItemType.TACTICAL ||
                    input.asset.type == EItemType.GRIP || input.asset.type == EItemType.BARREL || input.asset.type == EItemType.OPTIC)
                {
                    return true;
                }
            }
            else if (type == 5)
            {
                if (input.asset.type == EItemType.FOOD || input.asset.type == EItemType.WATER)
                {
                    return true;
                }
            }
            else if (type == 6)
            {
                if (input.asset.type == EItemType.MEDICAL)
                    return true;
            }
            else if (type == 7)
            {
                if (input.asset.type == EItemType.MELEE || input.asset.type == EItemType.THROWABLE ||
                    input.asset.type == EItemType.DETONATOR || input.asset.type == EItemType.CHARGE ||
                    input.asset.type == EItemType.TRAP)
                {
                    return true;
                }
            }
            else if (type == 9)
            {
                if (input.asset.type == EItemType.BACKPACK)
                    return true;
            }
            else if (type == 8)
                return true;

            return false;
        }

    }
}