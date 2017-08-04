using System;

namespace TsunamiHack.Tsunami.Types
{
    [Serializable]
    public class Friend
    {
        public string Name;
        public ulong SteamId;

        public Friend(){}

        public Friend(string name, ulong id)
        {
            Name = name;
            SteamId = id;
        }
    }
}
