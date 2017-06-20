using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TsunamiHack;

namespace JsonEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            PremiumList list = new PremiumList();
            list.addUser("463521");
            list.addUser("896413");

            var output = JsonConvert.SerializeObject(list);
            File.WriteAllText("OutputList.txt", output);
        }
    }
}
