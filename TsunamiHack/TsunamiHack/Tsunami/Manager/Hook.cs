using System;
using SDG.Unturned;
using UnityEngine;
using System.Reflection;
using TsunamiHack.Tsunami.Util;

// ReSharper disable PossibleNullReferenceException

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
                
                Logging.Log("Tsunami Sucessfully Loaded", "SUCESS");
            }
            catch (Exception e)
            {
                Util.Logging.Exception(e);
            }
        }
        
    }
}
