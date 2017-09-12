using System;
using SDG.Unturned;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.Linq;

namespace TsunamiHack.Tsunami.Util
{
    internal class PlayerTools
    {

        public static InteractableVehicle GetVehicle(Transform trans)
        {
            foreach (var veh in VehicleManager.vehicles)
            {
                if (veh.gameObject.transform == trans)
                    return veh;

                return null;
            }

            return null;
        }
        
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
            Logging.Log("DEBUG", "foreaching in steamplayer");

            foreach (var user in Provider.clients)
            {
                Logging.Log("DEBUG", "checking if transform is equal");

                if (user.player.transform == trans)
                    return user;
            }

            Logging.Log("DEBUG", "returning null");

            return null;
        }
        
        public static SteamPlayer GetSteamPlayer(GameObject trans)
        {
            Logging.Log("DEBUG", "foreaching in steamplayer");

            foreach (var user in Provider.clients)
            {
                Logging.Log("DEBUG", "checking if transform is equal");

                if (user.player.transform.gameObject == trans)
                    return user;
            }

            Logging.Log("DEBUG", "returning null");

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