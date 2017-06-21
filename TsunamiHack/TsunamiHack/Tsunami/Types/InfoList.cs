using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Framework.Debug;

namespace TsunamiHack
{
    public class InfoList
    {
        public List<string> list;

        public InfoList()
        {
            list = new List<string>();
        }

        public void addItem(string item)
        {
            if (item != null)
            {
                list.Add(item);
            }
        }
    }
}
