using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class Hook : MonoBehaviour
    {
        public void Cast()
        {
            GameObject Hook;
            Quake inst;

            try
            {
                Hook = new GameObject("HookLineAndSinker");
                inst = Hook.AddComponent<Quake>();
                UnityEngine.Object.DontDestroyOnLoad(inst);
            }
            catch (Exception e)
            {
                Debug.Log($"Crash Occured @ {DateTime.Now}");
                Debug.Log(e);
                Debug.Log("---End---");
            }

            if (UnityEngine.GameObject.Find("HookLineAndSinker") != null)
            {
                Debug.Log($"Hook Sucessful @ {DateTime.Now}");
            }
        }
    }
}
