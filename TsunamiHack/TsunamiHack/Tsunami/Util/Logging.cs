using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace TsunamiHack.Tsunami.Util
{
    class Logging
    {        
        public static void Exception(Exception e)
        {
            Debug.Log($"\n\n\n--- Exception occured @ {DateTime.Now} ---\n{e}\n--- END ---\n\n\n");
        }

        public static void LogMsg(string header, string msg, string footer = "END")
        {
            Debug.Log($"\n\n--- {header} ---\n{msg} (Occured @ {DateTime.Now})\n--- {footer} ---\n\n");
        }

        public static void LogList(string header, List<string> list, string footer = "END")
        {
            var msg = "";

            if (list.Count > 0)
            {
                foreach (var obj in list)
                {
                    msg += $"\n{obj}";
                }
            }
            else
            {
                msg = "\nNo Items in Collection";
            }
            
            Debug.Log($"\n\n--- {header} ---{msg}\n--- {footer} ---\n\n");
        }
    }
}
