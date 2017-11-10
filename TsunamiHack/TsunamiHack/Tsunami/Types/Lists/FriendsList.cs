using System;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using TsunamiHack.Tsunami.Util;

namespace TsunamiHack.Tsunami.Types.Lists
{
    public class FriendsList
    {
        public List<Friend> Userlist;

        public FriendsList()
        {
            Userlist = new List<Friend>();
        }

        public void AddFriend(Friend newFriend)
        {
            Userlist.Add(newFriend);
        }

        public void RemoveFriend(Friend oldFriend)
        {
            if (Userlist.Any(friend => friend == oldFriend))
                Userlist.Remove(oldFriend);
        }

        public bool IsFriend(ulong steamid)
        {
            return Userlist.Any(friend => friend.SteamId == steamid);
        }

        public void SaveFriends()
        {
            FileIo.SaveFriends(this);
        }
    }
}
