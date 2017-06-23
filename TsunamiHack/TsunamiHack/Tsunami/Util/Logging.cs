using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    class Logging
    {
        public static void Exception(Exception e)
        {
            Debug.Log($"Exception occured @ {DateTime.Now}");
            Debug.Log(e);
            Debug.Log("---End---");
        }
    }
}
