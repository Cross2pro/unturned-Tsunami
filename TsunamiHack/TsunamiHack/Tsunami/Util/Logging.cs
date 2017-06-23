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
            Debug.Log($"---Exception occured @ {DateTime.Now}---");
            Debug.Log(e);
            Debug.Log("---End---");
        }

        public static void logMsg(string header, string msg, string footer = "END")
        {
            Debug.Log($"---{header}---");
            Debug.Log($"{msg} occured @ {DateTime.Now}");
            Debug.Log($"---{footer}---");

        }
    }
}
