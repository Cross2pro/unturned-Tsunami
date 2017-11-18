using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HighlightingSystem;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Util;
using UnityEngine;
using Object = UnityEngine.Object;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ArrangeRedundantParentheses
// ReSharper disable InconsistentNaming

//TODO: 3d boxes
//TODO: Tracers


// ReSharper disable once CheckNamespace
namespace TsunamiHack.Tsunami.Menu
{
    internal partial class Visuals
    {

        public IEnumerator CR_ZombieGlow;
        public IEnumerator CR_PlayerGlow;
        public IEnumerator CR_ItemGlow;
        public IEnumerator CR_StorageGlow;
        public IEnumerator CR_AnimalGlow;
        
        public Camera Main;
        
        public List<Zombie> Zombielist;
        public List<Zombie> ZombieSkeletonlist;
        public List<Zombie> ZombieBoxlist;
        public List<Zombie> ZombieLabellist;
        public List<SteamPlayer> Playerlist;
        public List<InteractableItem> Itemlist;
        public List<InteractableItem> ItemLabelList;
        public List<Animal> Animallist;
        public List<InteractableStorage> Storagelist;
        public List<InteractableStorage> StorageLabellist;
        public List<InteractableVehicle> Vehiclelist;

        public static Dictionary<int, string> StorageIds;
        
        public DateTime UpLastUpdate;
        
        public Material DrawingMaterial;

        public bool forcestorageoff;
        
        public void OnStart()
        {
            Main = Main;
            UpLastUpdate = DateTime.Now;

            Zombielist = new List<Zombie>();
            ZombieSkeletonlist = new List<Zombie>();
            ZombieBoxlist = new List<Zombie>();
            ZombieLabellist = new List<Zombie>();
            Playerlist = new List<SteamPlayer>();
            Itemlist = new List<InteractableItem>();
            ItemLabelList = new List<InteractableItem>();
            Animallist = new List<Animal>();
            Storagelist = new List<InteractableStorage>();
            StorageLabellist = new List<InteractableStorage>();
            Vehiclelist = new List<InteractableVehicle>();

            
            var material = new Material(Shader.Find("Hidden/Internal-Colored"));
            material.hideFlags = (HideFlags) 61;
            DrawingMaterial = material;
            DrawingMaterial.SetInt("_SrcBlend", 5);
            DrawingMaterial.SetInt("_DstBlend", 10);
            DrawingMaterial.SetInt("_Cull", 0);
            DrawingMaterial.SetInt("_ZWrite", 0);
            
            GenerateDicts();
            
            StartCoroutine(UpdateZombieSkeletonList());
            StartCoroutine(UpdateZombieBoxList());
            StartCoroutine(UpdateZombieLabelList());
            StartCoroutine(UpdateItemLabelList());
            StartCoroutine(UpdateStorageLabelList());
        }
        
        public void OnUpdate()
        {
            Main = Camera.main;
                
            if (Event.current.type == EventType.Repaint && (DateTime.Now - UpLastUpdate).TotalMilliseconds >= UpdateRate && Provider.isConnected) 
            {
                CheckGlow();
                UpLastUpdate = DateTime.Now;
            }
            
        }
        
        public void OnGUIUpdate()
        {
            Main = Camera.main;
            
            
            if (Event.current.type == EventType.Repaint &&  Provider.isConnected)
            {
                CheckSkeleton();
                CheckBoxes();
                CheckLabels();
                
            }
        }

        #region Glow
        
        public void CheckGlow()
        {
            if (ZombieGlow && EnableEsp)
            {
                if (CR_ZombieGlow == null)
                {
                    var dist = ZombieOverrideDistance ? ZombieInfDistance ? 10000 : ZombieEspDistance : EspDistance;

                    CR_ZombieGlow = UpdateZGlowEsp(dist, InterpColor(ZombieColor));
                    StartCoroutine(CR_ZombieGlow);
                }
            }
            else
            {
                if (CR_ZombieGlow != null)
                {
                    StopCoroutine(CR_ZombieGlow);
                    CR_ZombieGlow = null;

                    StartCoroutine(ScrubGlowEsp(Zombielist));
                }
            }

//            Logging.Log($"Current Vehglow: {VehicleGlow}");
//            Logging.Log($"Current Enable: {EnableEsp}");
//            Logging.Log($"Count: {Vehiclelist.Count}");
//            
//            if (VehicleGlow && EnableEsp)
//            {
//                Logging.Log("A");
//                if (CR_VehicleGlow == null)
//                {
//                    Logging.Log("b");
//                    CR_VehicleGlow = UpdateVGlowEsp(EspDistance, InterpColor(VehicleColor));
//                    Logging.Log("c");
//                    StartCoroutine(CR_VehicleGlow);
//                }
//            }
//            else
//            {
//                Logging.Log("d");
//                if (CR_VehicleGlow != null)
//                {
//                    Logging.Log("e");
//                    StopCoroutine(CR_VehicleGlow);
//                    Logging.Log("f");
//                    CR_VehicleGlow = null;
//
//                    Logging.Log("g");
//                    StartCoroutine(ScrubGlowEsp(Vehiclelist));
//                }
//            }


            if (PlayerGlow && EnableEsp)
            {
                if (CR_PlayerGlow == null)
                {
                    var dist = PlayerOverrideDistance ? PlayerInfDistance ? 10000 : PlayerEspDistance : EspDistance;

                    CR_PlayerGlow = UpdatePGlowEsp(dist);
                    StartCoroutine(CR_PlayerGlow);
                }
            }
            else
            {
                if (CR_PlayerGlow != null)
                {
                    StopCoroutine(CR_PlayerGlow);
                    CR_PlayerGlow = null;

                    StartCoroutine(ScrubGlowEsp(Playerlist));
                }
            }


            if (ItemGlow && EnableEsp)
            {
                if (CR_ItemGlow == null)
                {
                    var dist = ItemOverrideDistance ? PlayerInfDistance ? 10000 : ItemEspDistance : EspDistance;

                    CR_ItemGlow = UpdateIGlowEsp(dist, InterpColor(ItemColor));
                    StartCoroutine(CR_ItemGlow);
                }
            }
            else
            {
                if (CR_ItemGlow != null)
                {
                    StopCoroutine(CR_ItemGlow);
                    CR_ItemGlow = null;

                    StartCoroutine(ScrubGlowEsp(Itemlist));
                }
            }

            if (AnimalGlow && EnableEsp)
            {
                if (CR_AnimalGlow == null)
                {
                    CR_AnimalGlow = UpdateAGlowEsp(EspDistance, InterpColor(AnimalColor));
                    StartCoroutine(CR_AnimalGlow);
                }
            }
            else
            {
                if (CR_AnimalGlow != null)
                {
                    StopCoroutine(CR_AnimalGlow);
                    CR_AnimalGlow = null;

                    StartCoroutine(ScrubGlowEsp(Animallist));
                }
            }

            if (StorageGlow && EnableEsp)
            {
                if (CR_StorageGlow == null)
                {
                    CR_StorageGlow = UpdateSGlowEsp(EspDistance, InterpColor(StorageColor));
                    StartCoroutine(CR_StorageGlow);
                }
            }
            else
            {
                if (CR_StorageGlow != null)
                {
                    StopCoroutine(CR_StorageGlow);
                    CR_StorageGlow = null;

                    StartCoroutine(ScrubGlowEsp(Storagelist));
                }
            }
        }

        public IEnumerator UpdateZGlowEsp(int distance, Color glowcolor)
        {
            Zombielist.Clear();
            foreach (var region in ZombieManager.regions)
            {                
                foreach (var zombie in region.zombies)
                {
                    Zombielist.Add(zombie);
                }
            }    
            
            
            foreach (var obj in Zombielist.ToList())
            {
                if (Vector3.Distance(Main.transform.position, obj.transform.position) <= distance)
                {
                    var highlighter = obj.gameObject.GetComponent<Highlighter>() ?? obj.gameObject.AddComponent<Highlighter>();
                    
                    highlighter.ConstantParams(glowcolor);
                    highlighter.OccluderOn();
                    highlighter.SeeThroughOn();
                    highlighter.ConstantOnImmediate();
                }
                else
                    obj.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();

                yield return null;
            }

            CR_ZombieGlow = null;
        }
        
        public IEnumerator UpdatePGlowEsp(int distance)
        {
            Playerlist = Provider.clients;
            
            foreach (var steamPlayer in Playerlist)
            {
                if (Vector3.Distance(Main.transform.position, steamPlayer.player.transform.position) <= distance && steamPlayer.player != Player.player)
                {
                    var highlighter = steamPlayer.player.gameObject.GetComponent<Highlighter>() ?? steamPlayer.player.gameObject.AddComponent<Highlighter>();
        
                    var glowcolor = WaveMaker.Friends.IsFriend(steamPlayer.playerID.steamID.m_SteamID) ? InterpColor(FriendlyPlayerColor) : InterpColor(EnemyPlayerColor);
                    
                    highlighter.ConstantParams(glowcolor);
                    highlighter.OccluderOn();
                    highlighter.SeeThroughOn();
                    highlighter.ConstantOnImmediate();
                }
                else
                    steamPlayer.player.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
        
                yield return null;
            }

            CR_PlayerGlow = null;
        }

        public IEnumerator UpdateIGlowEsp(int distance, Color glowColor)
        {
            Itemlist.Clear();
            Itemlist = Object.FindObjectsOfType<InteractableItem>().ToList();

            yield return null;
            
            var filteredList = new List<InteractableItem>();
            
            if (ItemFilter)
            {
                foreach (var item in Itemlist)
                {
                    if( (FilterClothes && isOfType(1, item)) || (FilterAmmo && isOfType(2, item)) || (FilterGuns && isOfType(3, item)) || (FilterAttach && isOfType(4, item)) || (FilterFood && isOfType(5, item)) || (FilterMed && isOfType(6, item)) || (FilterWeapons && isOfType(7, item)) || (FilterMisc && isOfType(8, item) || (FilterPacks && isOfType(9, item))))
                        filteredList.Add(item);
                }

                var inverse = Itemlist.Except(filteredList).ToList();
                if(inverse.Count > 0)
                    StartCoroutine(ScrubGlowEsp(inverse));
            }
            else
            {
                filteredList = Itemlist;
            }
            
            yield return null;

            foreach (var item in filteredList)
            {
                if (Vector3.Distance(Main.transform.position, item.transform.position) <= distance)
                {
                    var highligter = item.gameObject.GetComponent<Highlighter>() ?? item.gameObject.AddComponent<Highlighter>();
                    
                    highligter.ConstantParams(glowColor);
                    highligter.OccluderOn();
                    highligter.SeeThroughOn();
                    highligter.ConstantOnImmediate();
                }
                else
                    item.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();

                yield return null;
            }

            CR_ItemGlow = null;
        }

        public IEnumerator UpdateAGlowEsp(int distance, Color glowColor)
        {
            Animallist.Clear();
            Animallist = AnimalManager.animals;

            foreach (var animal in Animallist)
            {
                if (Vector3.Distance(Main.transform.position, animal.transform.position) <= distance)
                {
                    var highligter = animal.gameObject.GetComponent<Highlighter>() ?? animal.gameObject.AddComponent<Highlighter>();
                    
                    highligter.ConstantParams(glowColor);
                    highligter.OccluderOn();
                    highligter.SeeThroughOn();
                    highligter.ConstantOnImmediate();
                }
                else
                    animal.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
                
                yield return null;
            }

            CR_AnimalGlow = null;
        }
            
        public IEnumerator UpdateVGlowEsp(int distance, Color glowColor)
        {
            Vehiclelist.Clear();
            Vehiclelist = Object.FindObjectsOfType<InteractableVehicle>().ToList();

            foreach (var Vehicle in Vehiclelist)
            {
                if (Vector3.Distance(Main.transform.position, Vehicle.transform.position) <= distance)
                {
                    var highligter = Vehicle.gameObject.GetComponent<Highlighter>() ?? Vehicle.gameObject.AddComponent<Highlighter>();
                
                    highligter.ConstantParams(glowColor);
                    highligter.OccluderOn();
                    highligter.SeeThroughOn();
                    highligter.ConstantOnImmediate();
                }
                else
                    Vehicle.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
            
                yield return null;
            }

//            CR_VehicleGlow = null;
        }

        public IEnumerator UpdateSGlowEsp(int distance, Color glowColor)
        {
            Storagelist.Clear();
            Storagelist = Object.FindObjectsOfType<InteractableStorage>().ToList();

            yield return null;

            foreach (var storage in Storagelist)
            {
                if (storage.items.getItemCount() == 0 && StorageFilterInUse)
                    forcestorageoff = true;
                if (storage.isOpen && StorageFilterLocked)
                    forcestorageoff = true;
                
                if (Vector3.Distance(Main.transform.position, storage.transform.position) <= distance && !forcestorageoff)
                {
                    var highligter = storage.gameObject.GetComponent<Highlighter>() ?? storage.gameObject.AddComponent<Highlighter>();
                    
                    highligter.ConstantParams(glowColor);
                    highligter.OccluderOn();
                    highligter.SeeThroughOn();
                    highligter.ConstantOnImmediate();
                }
                else if (forcestorageoff)
                {
                    storage.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
                    forcestorageoff = false;
                }
                else
                    storage.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
                
                yield return null;
            }

            CR_StorageGlow = null;
        }
        
        public IEnumerator ScrubGlowEsp(List<Zombie> list)
        {
            foreach (var obj in list)
            {
                obj.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
        public IEnumerator ScrubGlowEsp(List<InteractableVehicle> list)
        {
            foreach (var veh in list)
            {
                veh.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
        public IEnumerator ScrubGlowEsp(List<SteamPlayer> list)
        {
            foreach (var obj in list)
            {
                obj.player.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
        public IEnumerator ScrubGlowEsp(List<InteractableItem> list)
        {
            foreach (var obj in list)
            {
                obj.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
              
        public IEnumerator ScrubGlowEsp(List<Animal> list)
        {
            foreach (var obj in list)
            {
                obj.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
        public IEnumerator ScrubGlowEsp(List<InteractableStorage> list)
        {
            foreach (var obj in list)
            {
                obj.gameObject.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
        #endregion
        
        #region Labels

        public IEnumerator UpdateZombieLabelList()
        {
            while (!WaveMaker.HackDisabled)
            {
                if ( ZombieName || ZombieDistance || ZombieType && EnableEsp)
                {
                    var updatelist = new List<Zombie>();
                    foreach (var region in ZombieManager.regions)
                    {
                        foreach (var zombie in region.zombies)
                        {
                            updatelist.Add(zombie);
                        }
                    }

                    ZombieLabellist = updatelist;
                    yield return new WaitForSeconds(5f);
                }
                
                yield return null;
            }
        }

        public IEnumerator UpdateItemLabelList()
        {
            while (!WaveMaker.HackDisabled)
            {
                if (ItemName || ItemDistance && EnableEsp)
                {
                    ItemLabelList = Object.FindObjectsOfType<InteractableItem>().ToList();
                    yield return new WaitForSeconds(5f);
                }
                
                yield return null;
            }
        }
        
        public IEnumerator UpdateStorageLabelList()
        {
            while (!WaveMaker.HackDisabled)
            {
                if (StorageName || StorageDistance && EnableEsp)
                {
                    StorageLabellist = Object.FindObjectsOfType<InteractableStorage>().ToList();
                    yield return new WaitForSeconds(5f);
                }
                
                yield return null;
            }
        }
        
        public void CheckLabels()
        {
            if (EnableEsp)
            {
                if (PlayerName || PlayerDistance || PlayerWeapon || AdminWarn)
                {
                    UpdatePlayerLabels();
                }

                if (ZombieName || ZombieDistance || ZombieType)
                {
                    UpdateZombieLabels();
                }

                if (ItemName || ItemDistance)
                {
                    UpdateItemLabels();
                }

                if (VehicleName || VehicleGas || VehicleHealth || VehicleDistance)
                {
                    UpdateVehicleLabels();
                }

                if (AnimalName || AnimalDistance)
                {
                    UpdateAnimalLabels();
                }

                if (StorageName || StorageDistance)
                {
                    UpdateStorageLabels();
                }
            }
        }

        public void UpdatePlayerLabels()
        {
            foreach (var player in Provider.clients)
            {

                if (!player.player.life.isDead && player.player != Player.player)
                {
                    var dist = PlayerOverrideDistance ? PlayerInfDistance ? 10000 : PlayerEspDistance : EspDistance;

                    if (Vector3.Distance(Main.transform.position, player.player.transform.position) <= dist || InfiniteDistance)
                    {
                        var targetpos = player.player.transform.position;
                        targetpos += new Vector3(0f, 3f, 0f);
                        var scrnpt = Main.WorldToScreenPoint(targetpos);

                        if (scrnpt.z >= 0)
                        {
                            scrnpt.y = Screen.height - scrnpt.y;
                            var text = "";

                            if (PlayerName)
                            {
                                text += $"{player.playerID.nickName}";
                            }

                            if (PlayerWeapon)
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

                            if (PlayerDistance)
                            {
                                if (text.Length > 0)
                                    text += "\n";

                                text += $"[{Math.Round(Vector3.Distance(Main.transform.position, player.player.transform.position), 0)}]";
                            }


                            float size;

                            if (ScaleText)
                                size = Vector3.Distance(Main.transform.position, player.player.transform.position) <= Dropoff ? CloseSize : FarSize;
                            else
                                size = 10f;

                            var color = WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID) ? InterpColor(FriendlyPlayerColor) : InterpColor(EnemyPlayerColor);
                            var friend = WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID);

                            GUI.Label(new Rect(scrnpt + new Vector3(0, 6f, 0), new Vector2(170, 70)), $"<color=#{ColorToHex(color)}><size={size}>{text}</size></color>");
                        }

                    }
                }
            }
        }

        public void UpdateZombieLabels()
        {
            foreach (var zombie in ZombieLabellist)
            {
                if (zombie.isDead == false)
                {

                    var dist = ZombieOverrideDistance ? ZombieInfDistance ? 10000 : ZombieEspDistance : EspDistance;

                    if (Vector3.Distance(Main.transform.position, zombie.transform.position) <= dist || InfiniteDistance)
                    {
                        var targetPos = zombie.transform.position;
                        targetPos += new Vector3(0f, 3f, 0f);
                        var scrnPt = Main.WorldToScreenPoint(targetPos);

                        if (scrnPt.z >= 0)
                        {

                            scrnPt.y = Screen.height - scrnPt.y;

                            var text = "";

                            if (ZombieName)
                            {
                                text += $"{zombie.gameObject.name}";
                            }

                            if (ZombieDistance)
                            {
                                if (text.Length > 0)
                                    text += "\n";

                                text += $"[{Math.Round(Vector3.Distance(Main.transform.position, zombie.transform.position), 0)}]";

                            }

                            if (ZombieType)
                            {
                                var str = "";

                                if (ZombieDistance)
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

                            if (ScaleText)
                            {
                                size = Vector3.Distance(Main.transform.position, zombie.transform.position) <= Dropoff ? CloseSize : FarSize;
                            }
                            else
                            {
                                size = 10;
                            }



                            GUI.Label(new Rect(scrnPt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                                $"<color=#{ColorToHex(InterpColor(ZombieColor))}><size={size}>{text}</size></color>");
                        }
                    }
                }

            }
        }

        public void UpdateItemLabels()
        {
            var filteredList = new List<InteractableItem>();
            
            if (ItemFilter)
            {
                foreach (var item in ItemLabelList)
                {
                    if( (FilterClothes && isOfType(1, item)) || (FilterAmmo && isOfType(2, item)) || (FilterGuns && isOfType(3, item)) || (FilterAttach && isOfType(4, item)) || (FilterFood && isOfType(5, item)) || (FilterMed && isOfType(6, item)) || (FilterWeapons && isOfType(7, item)) || (FilterMisc && isOfType(8, item) || (FilterPacks && isOfType(9, item))))
                        filteredList.Add(item);
                }
            }
            else
            {
                filteredList = ItemLabelList;
            }
            
            foreach (var item in filteredList)
            {                
                var dist = ItemOverrideDistance ? ItemInfDistance ? 10000 : ItemEspDistance : EspDistance;

                if (Vector3.Distance(Main.transform.position, item.transform.position) <= dist|| InfiniteDistance)
                {
                    var targetpos = item.transform.position;
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (ItemName)
                        {
                            text += $"{item.asset.itemName}";
                        }

                        if (ItemDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"[{Math.Round(Vector3.Distance(Main.transform.position, item.transform.position), 0)}]";

                        }

                        var size = ScaleText ? Vector3.Distance(Main.transform.position, item.transform.position) <= Dropoff ? CloseSize : FarSize : 10f;

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color=#{ColorToHex(InterpColor(ItemColor))}><size={size}>{text}</size></color>");
                    }
                }
            }
        }

        public void UpdateVehicleLabels()
        {
            foreach (var vehicle in VehicleManager.vehicles)
            {
               
                if (Vector3.Distance(Main.transform.position, vehicle.transform.position) <= EspDistance || InfiniteDistance)
                {
                    var targetpos = vehicle.transform.position;
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (VehicleName)
                        {
                            var name = vehicle.asset.name.Replace("_", " ");

                            if (name.Contains("Tractor"))
                                name = "Tractor";

                            if (name.Contains("Police"))
                                name = "Police Car";

                            text += $"{name}";
                        }

                        if (VehicleHealth)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"{Math.Round((decimal) vehicle.health, 0)} / {Math.Round((decimal) vehicle.asset.healthMax, 0)} Health";
                        }

                        if (VehicleGas)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text +=  $"{Math.Round((decimal) vehicle.fuel, 0)} / {Math.Round((decimal) vehicle.asset.fuelMax, 0)} Fuel";
                        }
                        
                        if (VehicleDistance)
                        {
                            if (text.Length > 0)
                                text += "\n";

                            text += $"[{Math.Round(Vector3.Distance(Main.transform.position, vehicle.transform.position), 0)}]";
                        }

                        var size = ScaleText ? Vector3.Distance(Main.transform.position, vehicle.transform.position) <= Dropoff ? CloseSize : FarSize : 10f;

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color=#{ColorToHex(InterpColor(VehicleColor))}><size={size}>{text}</size></color>");
                    }
                }
            }
        }

        public void UpdateAnimalLabels()
        {
            foreach (var animal in AnimalManager.animals)
            {
                if (!animal.isDead)
                {

                    if (Vector3.Distance(Main.transform.position, animal.transform.position) <= EspDistance || InfiniteDistance)
                    {
                        var targetpos = animal.transform.position;
                        targetpos += new Vector3(0f, 3f, 0f);
                        var scrnpt = Main.WorldToScreenPoint(targetpos);

                        if (scrnpt.z >= 0f)
                        {
                            scrnpt.y = Screen.height - scrnpt.y;

                            var text = "";

                            if (AnimalName)
                            {
                                text += $"{animal.asset.animalName}";
                            }

                            if (AnimalDistance)
                            {
                                if (text.Length > 0)
                                    text += $"\nDistance: {Math.Round(Vector3.Distance(Main.transform.position, animal.transform.position), 0)}";
                                else
                                    text += $"Distance: {Math.Round(Vector3.Distance(Main.transform.position, animal.transform.position), 0)}";
                            }

                            var size = ScaleText ? Vector3.Distance(Main.transform.position, animal.transform.position) <= Dropoff ? CloseSize : FarSize : 10f;

                            GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                                $"<color=#{ColorToHex(InterpColor(AnimalColor))}><size={size}>{text}</size></color>");
                        }
                    }
                }
            }
        }

        public void UpdateStorageLabels()
        {
            foreach (var storage in StorageLabellist)
            {

                if (storage.items.getItemCount() == 0 && StorageFilterInUse) break;
                if (storage.isOpen && StorageFilterLocked) break;
                
                if (Vector3.Distance(Main.transform.position, storage.transform.position) <= EspDistance || InfiniteDistance)
                {
                    var targetpos = storage.transform.position;
                    targetpos += new Vector3(0f, 1.5f, 0f);
                    var scrnpt = Main.WorldToScreenPoint(targetpos);

                    if (scrnpt.z >= 0)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var text = "";

                        if (StorageName)
                        {
                            text += $"Storage: {StorageIds[int.Parse(storage.name)]}";
                        }

                        if (StorageDistance)
                        {
                            text += text.Length > 0
                                ? $"\nDistance: {Math.Round(Vector3.Distance(Main.transform.position, storage.transform.position), 0)}"
                                : $"Distance: {Math.Round(Vector3.Distance(Main.transform.position, storage.transform.position), 0)}";
                        }


                        var size = ScaleText ? Vector3.Distance(Main.transform.position, storage.transform.position) <= Dropoff ? CloseSize : FarSize : 10f;

                        GUI.Label(new Rect(scrnpt + new Vector3(0, 4f, 0), new Vector2(170, 50)),
                            $"<color=#{ColorToHex(InterpColor(StorageColor))}><size={size}>{text}</size></color>");

                    }

                }

            }
        }
        
        public string ColorToHex(Color input)
        {
            Color32 color32 = input;
            string hex = color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");
            return hex;
        }
        
        #endregion
        
        #region skeleton

        public void CheckSkeleton()
        {
            if (ZombieSkeleton && EnableEsp)
                UpdateZombieSkeleton();
            if (PlayerSkeleton && EnableEsp)
                UpdatePlayerSkeleton();
        }

        public void UpdatePlayerSkeleton()
        {
            foreach (var player in Provider.clients)
            {
                var dist = PlayerOverrideDistance ? PlayerInfDistance ? 10000 : PlayerEspDistance : EspDistance;

                if (Vector3.Distance(Main.transform.position, player.player.transform.position) <= dist && player.player != Player.player)
                {
                    var mainlist = player.player.GetComponentsInChildren<Transform>().ToList();
                    foreach (var item in mainlist)
                    {
                        if (item.name.Trim() == "Skeleton")
                        {
                            var list = item.GetComponentsInChildren<Transform>();
                            var Bones = new List<BonePair>();

                            var sortedlist = new Transform[30];
                            
                            foreach (var co in list)
                            {
                                switch (co.name.Trim())
                                {
                                    case "Skull":
                                        sortedlist[1] = co;
                                        break;

                                    case "Spine":
                                        sortedlist[2] = co;
                                        break;

                                    case "Left_Shoulder":
                                        sortedlist[4] = co;
                                        break;

                                    case "Right_Shoulder":
                                        sortedlist[3] = co;
                                        break;

                                    case "Left_Arm":
                                        sortedlist[7] = co;
                                        break;

                                    case "Right_Arm":
                                        sortedlist[5] = co;
                                        break;

                                    case "Left_Hand":
                                        sortedlist[8] = co;
                                        break;

                                    case "Right_Hand":
                                        sortedlist[6] = co;
                                        break;

                                    case "Left_Leg":
                                        sortedlist[9] = co;
                                        break;

                                    case "Right_Leg":
                                        sortedlist[11] = co;
                                        break;

                                    case "Left_Foot":
                                        sortedlist[10] = co;
                                        break;

                                    case "Right_Foot":
                                        sortedlist[12] = co;
                                        break;
                                }
                            }
                            
                            
                            Bones.Add(BonePair.CreatePair(sortedlist[1], sortedlist[2]));    //skull to spine
                            Bones.Add(BonePair.CreatePair(sortedlist[1], sortedlist[3]));    //skull to r shoulder
                            Bones.Add(BonePair.CreatePair(sortedlist[1], sortedlist[4]));    //skull to l shoulder
                            Bones.Add(BonePair.CreatePair(sortedlist[3], sortedlist[5]));    //r shoulder to r arm
                            Bones.Add(BonePair.CreatePair(sortedlist[5], sortedlist[6]));    //r arm to r hand
                            Bones.Add(BonePair.CreatePair(sortedlist[4], sortedlist[7]));    //l shoulder to l arm
                            Bones.Add(BonePair.CreatePair(sortedlist[7], sortedlist[8]));    //l arm to l hand
                            Bones.Add(BonePair.CreatePair(sortedlist[2], sortedlist[9]));     //spine to l leg
                            Bones.Add(BonePair.CreatePair(sortedlist[9], sortedlist[10]));      //l leg to l foot  
                            Bones.Add(BonePair.CreatePair(sortedlist[2], sortedlist[11]));     //spine to r leg
                            Bones.Add(BonePair.CreatePair(sortedlist[11], sortedlist[12]));      //r leg to r foot

                            var color = WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID) ? InterpColor(FriendlyPlayerColor) : InterpColor(EnemyPlayerColor);
                            
                            DrawSkeleton(Bones, color, sortedlist[1]);
                        }
                    }
                }
                
            }
        }

        public IEnumerator UpdateZombieSkeletonList()
        {
            while (!WaveMaker.HackDisabled)
            {
                if (ZombieSkeleton && EnableEsp)
                {
                    var updatelist = new List<Zombie>();
                    foreach (var region in ZombieManager.regions)
                    {
                        foreach (var zombie in region.zombies)
                        {
                            updatelist.Add(zombie);
                        }
                    }

                    ZombieSkeletonlist = updatelist;
                    yield return new WaitForSeconds(5f);    
                }

                yield return null;
            }
        }
        
        public void UpdateZombieSkeleton()
        {

            foreach (var zombie in ZombieSkeletonlist)
            {
                var dist = ZombieOverrideDistance ? ZombieInfDistance ? 10000 : ZombieEspDistance : EspDistance;
                
                if (Vector3.Distance(Main.transform.position, zombie.transform.position) <= dist && !zombie.isDead)
                {
                    var mainlist = zombie.GetComponentsInChildren<Transform>().ToList();
                    foreach (var item in mainlist)
                    {
                        if (item.name.Trim() == "Skeleton")
                        {
                            var list = item.GetComponentsInChildren<Transform>();
                            var Bones = new List<BonePair>();
                        
                            Bones.Add(BonePair.CreatePair(list[17], list[7]));
                            Bones.Add(BonePair.CreatePair(list[17], list[12]));
                            Bones.Add(BonePair.CreatePair(list[17], list[8]));
                            Bones.Add(BonePair.CreatePair(list[12], list[13]));
                            Bones.Add(BonePair.CreatePair(list[13], list[14]));
                            Bones.Add(BonePair.CreatePair(list[8], list[9]));
                            Bones.Add(BonePair.CreatePair(list[9], list[10]));
                            Bones.Add(BonePair.CreatePair(list[7], list[2]));
                            Bones.Add(BonePair.CreatePair(list[2], list[3]));
                            Bones.Add(BonePair.CreatePair(list[7], list[5]));
                            Bones.Add(BonePair.CreatePair(list[5], list[6]));
                        
                            DrawSkeleton(Bones, InterpColor(ZombieColor), list[17]);
                        }
                    }
                }
                
            }
        }
        
        public void DrawSkeleton(List<BonePair> bones, Color color, Transform skull)
        {
            
            var visible = Main.WorldToViewportPoint(skull.position);
            if (visible.z <= 0f || visible.x <= 0f || visible.x >= 1f || visible.y <= 0f || visible.y >= 1f) return;
            
            var skullelevatedpos = Main.WorldToScreenPoint(new Vector3(skull.position.x, skull.position.y + (float) 0.25, skull.position.z));
            skullelevatedpos.y = Screen.height - skullelevatedpos.y;

            var skullpos = Main.WorldToScreenPoint(skull.transform.position);
            skullpos.y = Screen.height - skullpos.y;
                
            GL.PushMatrix();
            GL.Begin(1);
            DrawingMaterial.SetPass(0);
            GL.Color(color);

            foreach (var combo in bones)
            {
                GL.Vertex3(Pos(combo.Item1).x, Pos(combo.Item1).y, 0f);
                GL.Vertex3(Pos(combo.Item2).x, Pos(combo.Item2).y, 0f);
            }
            
            GL.Vertex3(skullpos.x, skullpos.y, 0f);
            GL.Vertex3(skullelevatedpos.x, skullelevatedpos.y, 0f);
            
            GL.End();
            GL.PopMatrix();
        }
        
        public Vector3 Pos(Transform input)
        {
            var scrnpt = Main.WorldToScreenPoint(input.position);
            scrnpt.y = Screen.height - scrnpt.y;
            return scrnpt;
        }

        
        #endregion

        #region Boxes

        public IEnumerator UpdateZombieBoxList()
        {
            while (!WaveMaker.HackDisabled)
            {
                if (ZombieBoxes && EnableEsp)
                {
                    var updatelist = new List<Zombie>();
                    foreach (var region in ZombieManager.regions)
                    {
                        foreach (var zombie in region.zombies)
                        {
                            updatelist.Add(zombie);
                        }
                    }

                    ZombieBoxlist = updatelist;
                    yield return new WaitForSeconds(5f);
                }
                
                yield return null;
            }
        }
        
        public void CheckBoxes()
        {
            if (Player2DBoxes && EnableEsp)
            {
                Update2DPlayerBoxes();
            }
            
            if (Player3DBoxes && EnableEsp)
            {
                Update3DPlayerBoxes();
            }

            if (ZombieBoxes && EnableEsp)
            {
                UpdateZombieBoxes();
            }
        }

        public void Update2DPlayerBoxes()
        {
            foreach (var player in Provider.clients)
            {
                var dist = PlayerOverrideDistance ? PlayerInfDistance ? 10000 : PlayerEspDistance : EspDistance;

                if (Vector3.Distance(Main.transform.position, player.player.transform.position) <= dist && Player.player != player.player)
                {
                    var scrnpt = Main.WorldToScreenPoint(player.player.transform.position);
                    if (scrnpt.z >= 0)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        var color = WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID) ? InterpColor(FriendlyPlayerColor) : InterpColor(EnemyPlayerColor);
                        Draw2DBox(player.player.transform, scrnpt, color);
                    }
                }
            }
        }

        public void Update3DPlayerBoxes()
        {
//            foreach (var player in Provider.clients)
//            {
//                var dist = PlayerOverrideDistance ? PlayerInfDistance ? 10000 : PlayerEspDistance : EspDistance;
//
//                if (Vector3.Distance(Main.transform.position, player.player.transform.position) <= dist && Player.player != player.player && !Player2DBoxes)
//                {
//                    var scrnpt = Main.WorldToScreenPoint(player.player.transform.position);
//                    if (scrnpt.z >= 0)
//                    {
//                        scrnpt.y = Screen.height - scrnpt.y;
//                        var color = WaveMaker.Friends.IsFriend(player.playerID.steamID.m_SteamID) ? InterpColor(FriendlyPlayerColor) : InterpColor(EnemyPlayerColor);
//                        Draw3DBox(player.player.transform, scrnpt, color);
//                    }
//                }
//            }
        }

        public void UpdateZombieBoxes()
        {
            
            foreach (var zombie in ZombieBoxlist)
            {
                var dist = ZombieOverrideDistance ? ZombieInfDistance ? 10000 : ZombieEspDistance : EspDistance;

                if (Vector3.Distance(Main.transform.position, zombie.transform.position) <= dist)
                {
                    var scrnpt = Main.WorldToScreenPoint(zombie.transform.position);
                    if (scrnpt.z >= 0f)
                    {
                        scrnpt.y = Screen.height - scrnpt.y;
                        Draw2DBox(zombie.transform, scrnpt, InterpColor(ZombieColor));
                    }
                }
            }
        }
        
        public void Draw2DBox(Transform target, Vector3 position, Color color)
        {
            var targetPos = GetTargetVector(target, "Skull");
            var scrnPt = Main.WorldToScreenPoint(targetPos);
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

        public void Draw3DBox(Transform target, Vector3 position, Color color)
        {
//            var skullpos = GetTargetVector(target, "Skull");
//            var dist = Math.Abs(position.y - skullpos.y);
//            var fright = new Vector3(position.x += dist, position.y += dist, position.z += dist );
//            var fleft = new Vector3(position.x -= dist, position.y += dist, position.z += dist);
//            var rright = new Vector3(position.x += dist, position.y += dist, position.z -= dist);
//            var rleft = new Vector3(position.x - dist, position.y += dist, position.z -= dist);
//            
//            GL.PushMatrix();
//            GL.Begin(1);
//            DrawingMaterial.SetPass(0);
//            GL.Color(color);
//            
//            GL.Vertex3(Pos(fright).x, Pos(fright).y, 0f );
//            GL.Vertex3(Pos(fleft).x, Pos(fleft).y, 0f);
//            
//            GL.End();
//            GL.PopMatrix();
        }
        
        public Vector3 Pos(Vector3 input)
        {
            var scrnpt = Main.WorldToScreenPoint(input);
            scrnpt.y = Screen.height - scrnpt.y;
            return scrnpt;
        }
        
        public Vector3 GetTargetVector(Transform target, string objName)
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
        
        #endregion
        
        public static Color InterpColor(ColorOptions input)
        {
            switch (input)
            {
                    case ColorOptions.aqua:
                        return new Color(0, 255, 255, 255);
                    case ColorOptions.black:
                        return new Color(0, 0, 0, 255);
                    case ColorOptions.blue:
                        return new Color(0, 0, 255, 255);
                    case ColorOptions.magenta:
                        return new Color(255, 0, 255, 255);
                    case ColorOptions.green:
                        return new Color(0, 128, 0, 255);
                    case ColorOptions.maroon:
                        return new Color(128, 0, 0, 255);
                    case ColorOptions.navy:
                        return new Color(0, 0, 128, 255);
                    case ColorOptions.olive:
                        return new Color(128, 128, 0, 255);
                    case ColorOptions.purple:
                        return new Color(128, 0, 128, 255);
                    case ColorOptions.red:
                        return new Color(255, 0, 0, 255);
                    case ColorOptions.teal:
                        return new Color(0, 128, 128, 255);
                    case ColorOptions.white:
                        return new Color(255, 255, 255, 255);
                    case ColorOptions.yellow:
                        return new Color(255, 255, 0, 255);
                    default:
                        goto case ColorOptions.white;
            }
        }
        
        public static bool isOfType(int type, InteractableItem input)
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
                if (input.asset.type == EItemType.MAGAZINE || input.asset.type == EItemType.REFILL || input.asset.type == EItemType.SUPPLY || input.asset.type == EItemType.BOX)
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
        }

















    }
}