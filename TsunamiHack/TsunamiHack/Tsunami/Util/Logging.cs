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

        public static void LogMsg(string header, string msg, string footer = "END")
        {
            Debug.Log($"\n\n---{header}--- // {msg} (Occured @ {DateTime.Now}) // ---{footer}---\n\n");
        }
    }
}
