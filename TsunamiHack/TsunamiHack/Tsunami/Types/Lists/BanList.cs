using System.Collections.Generic;

namespace TsunamiHack.Tsunami.Types.Lists
{
    internal class BanList
    {
        public List<string> UserList;

        public string GetUserByIndex(int index)
        {
            return index > UserList.Count ? null : UserList[index];
        }

        public string GetUserByName(string name)
        {
            if (!UserList.Contains(name)) return null;
            var index = UserList.IndexOf(name);
            return UserList[index];
        }

        public List<string> GetFullList()
        {
            return UserList;
        }

        public bool Contains(string item)
        {
            return UserList.Contains(item);
        }
    }
}
