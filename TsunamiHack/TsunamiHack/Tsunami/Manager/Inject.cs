using System;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    static class Install
    {
        static void Inject()
        {
            GameObject _hook;
            TsunamiHack _instance;

            try
            {
                _hook = new GameObject("hookObj");
                _instance = _hook.AddComponent<TsunamiHack>();
                UnityEngine.Object.DontDestroyOnLoad(_instance);
            }
            catch (Exception e)
            {
                Debug.Log("Oh No! Crash Occured!");
                Debug.Log($"{DateTime.Now}:");
                Debug.Log(e.Message);
                Debug.Log("--End--");
            }
        }
    }
}
