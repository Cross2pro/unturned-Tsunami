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
    }
}