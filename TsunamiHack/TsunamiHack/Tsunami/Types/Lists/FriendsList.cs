using System;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using Steamworks;
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
            var index = Userlist.IndexOf(oldFriend); 
            
            if(index != -1)
                Userlist.RemoveAt(index);
        }

        public bool IsFriend(ulong steamid)
        {
            foreach (var user in Userlist)
            {
                if (user.SteamId == steamid)
                    return true;
            }

            return false;
        }

        public void SaveFriends()
        {
            FileIo.SaveFriends(this);
        }
    }
}
