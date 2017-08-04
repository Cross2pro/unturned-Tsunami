using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using TsunamiHack.Tsunami.Types;

namespace JsonEditor
{
    public class Program
    {
        public enum Type
        {
            List,
            Hackinfo
        }

        static void Main(string[] args)
        {
//            //List Creation Variables
//
//            const string desktopDir = @"C:\Users\Zach\Desktop\";
//            const string outputFileName = @"output.txt";
//            const string inputFilePath = @"ControllerInfo.txt";
//            const Type generatingType = Type.Hackinfo;
//
//            /*-----Static Code-----*/
//
//            if (generatingType == Type.List)
//            {
//                string[] inputArray = null;
//                var outputList = new PubInfoList();
//
//                if (File.Exists(desktopDir + inputFilePath))
//                {
//                    inputArray = File.ReadAllLines(desktopDir + inputFilePath);
//                }
//
//                outputList.UserList = new List<string>();
//                
//                foreach (var str in inputArray)
//                {
//                    outputList.UserList.Add(str);
//                }
//
//                if (outputList.UserList.Count > 0)
//                {
//                    var jsonstring = JsonConvert.SerializeObject(outputList, Formatting.Indented);
//                    File.WriteAllText(desktopDir + outputFileName, jsonstring);
//                }
//            }
//            else if (generatingType == Type.Hackinfo)
//            {
//                string[] inputArray = null;
//                var output = new HackController();
//
//                if (File.Exists(desktopDir + inputFilePath))
//                {
//                    inputArray = File.ReadAllLines(desktopDir + inputFilePath);
//                }
//
//                output.Disabled = bool.Parse(inputArray[0]);
//                output.Reason = inputArray[1];
//                output.AuthorizedBy = inputArray[2];
//
//                var jsonstring = JsonConvert.SerializeObject(output, Formatting.Indented);
//                File.WriteAllText(desktopDir + outputFileName, jsonstring);
//            }
//

        }
    }


}
