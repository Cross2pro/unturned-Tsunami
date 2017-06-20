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
            var list = new InfoList();
            list.addItem("654984");
            list.addItem("543654");

            var outp = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText("Output2.txt", outp);
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }

    class 
}
