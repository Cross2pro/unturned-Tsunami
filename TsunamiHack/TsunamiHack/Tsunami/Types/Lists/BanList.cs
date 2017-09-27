using System.Collections.Generic;

namespace TsunamiHack.Tsunami.Types.Lists
{
    public class BanList
    {
        public List<string> UserList;

        internal string GetUserByIndex(int index)
        {
            return index > UserList.Count ? null : UserList[index];
        }

        internal string GetUserByName(string name)
        {
            if (!UserList.Contains(name)) return null;
            var index = UserList.IndexOf(name);
            return UserList[index];
        }

        internal List<string> GetFullList()
        {
            return UserList;
        }

        internal bool Contains(string item)
        {
            return UserList.Contains(item);
        }
    }
}
