using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    public static class Hook
    {
        public static void Cast()
        {
            try
            {
                var hook = new GameObject();
                var instance = hook.AddComponent<Quake>();
                UnityEngine.Object.DontDestroyOnLoad(instance);
            }
            catch (Exception e)
            {
                Util.Logging.Exception(e);
            }
        }
    }
}
