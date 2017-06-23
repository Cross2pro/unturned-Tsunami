using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    public static class Hook
    {
        public static void Cast()
        {
            GameObject Hook;
            Quake inst;

            try
            {
                Hook = new GameObject("HookLineAndSinker");
                inst = Hook.AddComponent<Quake>();
                UnityEngine.Object.DontDestroyOnLoad(inst);
            }
            catch(Exception e)
            {
                Util.Logging.Exception(e);
            }

            if (UnityEngine.GameObject.Find("HookLineAndSinker"))
                Util.Logging.logMsg("Sucess", "Hook injected");
            else
                Util.Logging.logMsg("Failed", "Hook Unable to be Injected");
        }
    }
}
