using System;
using System.Collections.Generic;

namespace TsunamiHack.Tsunami.Types.Lists
{
    public class FriendsList
    {
        public List<Friend> Userlist;

        public FriendsList()
        {
            Userlist = new List<Friend>();
        }

        public void RemoveFriend(ulong id)
        {
           if(Array.Exists(Userlist.ToArray(), friend => friend.SteamId == id ))
           {

               foreach (var friend in Userlist)
               {
                   if (friend.SteamId == id)
                   {
                       Userlist.Remove(friend);
                   }
               }
           }
        }

        public void RemoveFriend(string name)
        {
            if (Array.Exists(Userlist.ToArray(), friend => friend.Name == name))
            {
                foreach (var friend in Userlist)
                {
                    if (friend.Name == name)
                    {
                        Userlist.Remove(friend);
                    }
                }
            }
        }
    }
}
