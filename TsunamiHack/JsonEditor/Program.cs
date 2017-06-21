using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using TsunamiHack;

namespace JsonEditor
{
    public class Program
    {

        static void Main(string[] args)
        {
            string outputFileName = "output.txt";
            string inputFilePath = "input.txt";

            /*-----Static Code-----*/

            string[] inputArray = null;
            InfoList outputList = new InfoList();

            if (File.Exists(inputFilePath))
            {
                inputArray = File.ReadAllLines(inputFilePath);
            }

            foreach (var str in inputArray)
            {
                outputList.list.Add(str);
            }

            if (outputList.list.Count > 0)
            {
                var jsonstring = JsonConvert.SerializeObject(outputList, Formatting.Indented);
                File.WriteAllText(outputFileName, jsonstring);
            }
        }
    }


    public class MenuCommands
    {
        public static void Start(CurrItem inst)
        {
            Console.WriteLine("1 - load json, 2 - create new");
            var res = Console.ReadLine();

             if (res == "1")
            {
                loadJson(inst);
            }
            else if (res == "2")
             {
                 createNewJson(inst);
             }
             else
             {
                 Start(inst);
             }

            if (inst.deserializedList == null)
            {
                inst.deserializedList = new InfoList();
            }
            else
            {
                Console.WriteLine("Loaded Json Users:");
                foreach (var str in inst.deserializedList.list)
                {
                    Console.WriteLine(str);
                }
            }

            giveOptions(inst);

        }

        static bool loadJson(CurrItem inst)
        {
            Console.WriteLine("Input file name");
            var path = Console.ReadLine();

            if (!File.Exists(path))
            {
                Console.WriteLine("Does not exist!");
                loadJson(inst);
            }
            else
            {
                inst.jsonPath = path;
                inst.fileJSON = File.ReadAllText(path);
                inst.deserializedList = JsonConvert.DeserializeObject<InfoList>(inst.fileJSON);
                return true;
            }
            return false;
        }

        static void createNewJson(CurrItem inst)
        {
            Console.WriteLine("input new file name");
            var path = $@"{Console.ReadLine()}";

            inst.jsonPath = path;
        }

        static void giveOptions(CurrItem inst)
        {
            Console.WriteLine("1 - add item, 2 - remove item, 3 - save, 4 - exit, 5 - view list");
            var res = Console.ReadLine();

            if (res == "1")
            {
                additem(inst);
            }
            else if (res == "2")
            {
                remItem(inst);
            }
            else if (res == "3")
            {
                save(inst);
            }
            else if (res == "4")
            {
                exit(inst);
            }
            else if (res == "5")
            {
                viewList(inst);
            }
            else
            {
                Console.WriteLine("Invalid option");
                giveOptions(inst);
            }
        }

        static void additem(CurrItem inst)
        {
            Console.WriteLine("input item to add");
            var input = Console.ReadLine();
            if (input != null)
            {
                inst.deserializedList.addItem(input);
            }
            giveOptions(inst);
        }

        static void remItem(CurrItem inst)
        {
            Console.WriteLine("input the item you want deleted");
            var input = Console.ReadLine();
            var list = inst.deserializedList.list;

            foreach (var str in list)
            {
                if (str.Contains(input))
                {
                    var index = list.IndexOf(str);
                    list.RemoveAt(index);
                    Console.WriteLine("Removed");
                    inst.deserializedList.list = list;
                    giveOptions(inst);
                }
            }
            inst.deserializedList.list = list;
            giveOptions(inst);
        }

        static void save(CurrItem inst)
        {
            if (inst.deserializedList.list.Count > 0)
            {
                
                
            }
            giveOptions(inst);
        }

        static void exit(CurrItem inst)
        {
            save(inst);
            Environment.Exit(0);
        }

        static void viewList(CurrItem inst)
        {
            foreach (var item in inst.deserializedList.list)
            {
                Console.WriteLine(item);
            }
            giveOptions(inst);
        }

    }
}
