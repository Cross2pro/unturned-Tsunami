using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JsonEditor
{
    class PubInfoList
    {
        public List<string> UserList;

        public virtual string GetUserByIndex( int index )
        {
            if (index > UserList.Count)
                return null;
            return UserList[index];
        }

        public virtual string GetUserByName( string name )
        {
            if (!UserList.Contains( name ))
                return null;
            var index = UserList.IndexOf( name );
            return UserList[index];
        }

        public virtual List<string> GetFullList()
        {
            return UserList;
        }
    }
}
