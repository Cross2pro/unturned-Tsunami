using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsunamiHack
{
    public class PremiumList 
    {
        private List<String> UserList;
         
        public PremiumList()
        {
            UserList = new List<string>();
        }

        public void addUser(string id)
        {
            if (id != null)
            {
                UserList.Add(id);
            }
        }
    }
}
