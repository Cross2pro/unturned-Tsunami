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
            try
            {
                var hook = new GameObject();
                Quake instance = hook.AddComponent<Quake>();
                UnityEngine.Object.DontDestroyOnLoad(instance);
            }
            catch (Exception e)
            {
                Util.Logging.Exception(e);

                if (1 == 1)
                {
                    
                }
            }
        }
    }
}
