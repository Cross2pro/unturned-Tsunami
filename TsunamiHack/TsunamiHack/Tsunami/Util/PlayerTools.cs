using System;
using System.Linq;
using SDG.Unturned;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TsunamiHack.Tsunami.Util
{
    internal class PlayerTools
    {
        public static SteamPlayer GetSteamPlayer(Player player)
        {
            foreach (var user in Provider.clients)
            {
                if (user.player == player)
                    return user;
            }

            return null;
        }

        public static SteamPlayer GetSteamPlayer(Transform trans)
        {
            Logging.LogMsg("DEBUG", "foreaching in steamplayer");

            foreach (var user in Provider.clients)
            {
                Logging.LogMsg("DEBUG", "checking if transform is equal");

                if (user.player.transform == trans)
                    return user;
            }

            Logging.LogMsg("DEBUG", "returning null");

            return null;
        }
        
        public static SteamPlayer GetSteamPlayer(GameObject trans)
        {
            Logging.LogMsg("DEBUG", "foreaching in steamplayer");

            foreach (var user in Provider.clients)
            {
                Logging.LogMsg("DEBUG", "checking if transform is equal");

                if (user.player.transform.gameObject == trans)
                    return user;
            }

            Logging.LogMsg("DEBUG", "returning null");

            return null;
        }

        public static Zombie GetZombie(Transform trans)
        {
            var zoms = Object.FindObjectsOfType<Zombie>();
            
            foreach (var zombie in zoms )
            {
                if (zombie.transform == trans)
                    return zombie;
            }

            return null;
        }


        public static SteamPlayer GetPlayer(RaycastHit hit)
        {
            var list = Object.FindObjectsOfType<Player>();

            foreach (var ply in list)
            {
                if (ply.transform.gameObject == hit.transform.gameObject)
                {
                    return Array.Find(Provider.clients.ToArray(),
                        client => client.player.transform.gameObject == ply.transform.gameObject);
                }        
            }
            
            return null;
        }
    }
}