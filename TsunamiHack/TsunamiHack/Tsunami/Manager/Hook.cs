using System;
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
