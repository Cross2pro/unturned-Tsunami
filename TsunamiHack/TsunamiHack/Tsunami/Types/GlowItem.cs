using System.Collections.Generic;
using UnityEngine;
using SDG.Unturned;

namespace TsunamiHack.Tsunami.Types
{
    internal class GlowItem
    {
      
        internal GameObject gameObject;

        internal GlowItem(GameObject obj)
        {
            gameObject = obj;
        }
        
        
        public static List<GlowItem> GetListZombie(List<Zombie> list)
        {
            List<GlowItem> rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListPlayer(List<SteamPlayer> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                if(item.player != null && item.player != Player.player)
                    rlist.Add(new GlowItem(item.player.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListVehicle(List<InteractableVehicle> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListItem(List<InteractableItem> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListAnimal(List<Animal> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListStorage(List<InteractableStorage> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListBed(List<InteractableBed> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListDoor(List<InteractableDoor> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListFlag(List<InteractableClaim> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListSentry(List<InteractableSentry> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
        
        public static List<GlowItem> GetListNpc(List<InteractableObjectNPC> list)
        {
            var rlist = new List<GlowItem>();

            foreach (var item in list)
            {
                rlist.Add(new GlowItem(item.gameObject));
            }

            return rlist;
        }
    }
    
    
}