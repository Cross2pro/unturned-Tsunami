﻿using System;
using System.Collections.Generic;
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

        internal void RemoveFriend(string name)
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

        internal void AddFriend(Friend user)
        {
            if (Array.Exists(Userlist.ToArray(), friend => friend.SteamId == user.SteamId))
            {
                return;
            }
            
            Userlist.Add(user);
        }

        public void AddFriend(string name, ulong id)
        {
            if (Array.Exists(Userlist.ToArray(), friend => friend.SteamId == id))
            {
                return;
            }
            
            var fr = new Friend();
            fr.Name = name;
            fr.SteamId = id;
            
            Userlist.Add(fr);
        }

        public bool IsFriend(SteamPlayer client)
        {
            return Contains(client.playerID.steamID.m_SteamID);
        }
        
        public bool Contains(ulong id)
        {
            return Array.Exists(Userlist.ToArray(), user => user.SteamId == id);
        }

        public void SaveFriends()
        {
            FileIo.SaveFriends(this);
        }
    }
}
