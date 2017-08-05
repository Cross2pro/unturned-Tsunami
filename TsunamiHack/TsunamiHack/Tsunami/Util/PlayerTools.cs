using System.Linq;
using SDG.Unturned;
using UnityEngine;

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
            foreach (var user in Provider.clients)
            {
                if (user.player.transform == trans)
                    return user;
            }

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
    }
}