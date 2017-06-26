using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsunamiHack.Tsunami.Types.Lists
{
    public class FriendsList
    {
        public List<Friend> Userlist;

        public void RemoveFriend(ulong Id)
        {
           if(Array.Exists(Userlist.ToArray(), friend => friend.steamId == Id ))
           {

               foreach (var friend in Userlist)
               {
                   if (friend.steamId == Id)
                   {
                       Userlist.Remove(friend);
                   }
               }
           }
        }

        public void RemoveFriend(string Name)
        {
            if (Array.Exists(Userlist.ToArray(), friend => friend.Name == Name))
            {
                foreach (var friend in Userlist)
                {
                    if (friend.Name == Name)
                    {
                        Userlist.Remove(friend);
                    }
                }
            }
        }
    }
}
