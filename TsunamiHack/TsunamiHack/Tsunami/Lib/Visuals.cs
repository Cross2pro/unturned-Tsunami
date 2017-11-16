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

// ReSharper disable once CheckNamespace
namespace TsunamiHack.Tsunami.Menu
{
    internal partial class Visuals : MonoBehaviour
    {

        public IEnumerator CR_ZombieGlow;
        public IEnumerator CR_PlayerGlow;
        public IEnumerator CR_ItemGlow;
        public IEnumerator CR_VehicleGlow;
        public IEnumerator CR_StorageGlow;
        public IEnumerator CR_AnimalGlow;

        public Camera Main;
        
        public List<Zombie> Zombielist;
        public List<SteamPlayer> Playerlist;
        public List<InteractableItem> Itemlist;

        public DateTime LastUpdate;
        public bool ForceUpdate;
        
        public void OnStart()
        {
            Main = Camera.main;
            LastUpdate = DateTime.Now;

            Zombielist = new List<Zombie>();
            Playerlist = new List<SteamPlayer>();
            Itemlist = new List<InteractableItem>();
        }
        
        public void OnUpdate()
        {
            Main = Camera.main;
                
            if (Event.current.type == EventType.Repaint && (DateTime.Now - LastUpdate).TotalMilliseconds >= UpdateRate && Provider.isConnected) 
            {
                CheckGlow();
            }

        }
        
        public void OnGUIUpdate()
        {
            
        }

        public void  CheckGlow()
        {
            if (ZombieGlow && EnableEsp)
            {
                Logging.Log("Inside zom");
                if (CR_ZombieGlow == null)
                {
                    Logging.Log("A");
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
            
            Logging.Log("6");
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
            
        }

        public IEnumerator UpdateIGlowEsp(int distance, Color glowColor)
        {
            Itemlist.Clear();
            Itemlist = Object.FindObjectsOfType<InteractableItem>().ToList();

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
        }
        
        public IEnumerator ScrubGlowEsp(List<Zombie> list)
        {
            foreach (var obj in list)
            {
                obj.GetComponent<Highlighter>()?.ConstantOffImmediate();
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
                obj.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
        public IEnumerator ScrubGlowEsp(List<InteractableVehicle> list)
        {
            foreach (var obj in list)
            {
                obj.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
        public IEnumerator ScrubGlowEsp(List<Animal> list)
        {
            foreach (var obj in list)
            {
                obj.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
        public IEnumerator ScrubGlowEsp(List<InteractableStorage> list)
        {
            foreach (var obj in list)
            {
                obj.GetComponent<Highlighter>()?.ConstantOffImmediate();
                yield return null;
            }
        }
        
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
                    case ColorOptions.darkblue:
                        return new Color(0, 0, 160, 255);
                    case ColorOptions.magenta:
                        return new Color(255, 0, 255, 255);
                    case ColorOptions.green:
                        return new Color(0, 128, 0, 255);
                    case ColorOptions.lime:
                        return new Color(0, 255, 0, 255);
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
                    case ColorOptions.silver:
                        return new Color(192, 192, 192, 255);
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