using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Boo.Lang.Runtime;
using Mono.Posix;
using SDG.Unturned;
using UnityEngine;
using TsunamiHack.Tsunami.Util;

namespace TsunamiHack.Tsunami.Manager
{
    class Manager : MonoBehaviour
    {
        private readonly bool isPremium;
        private readonly bool isBanned;  

        #region Menus
        private GameObject obj;
        public static Menu.Main main;
        public static Lib.Keybinds keybinds;

        #endregion

        private List<UnityEngine.Object> list;

        public void Start()
        {
            Rebrand();
            //check for lists, check ban
        }

        public void Update()
        {
            if (Provider.isConnected)
            {
                if (obj == null)
                {
                    obj = new GameObject();
                    list.Add(main = obj.AddComponent<Menu.Main>());

                    foreach (var obj in list)
                    {
                        UnityEngine.Object.DontDestroyOnLoad(obj);
                    }

                }
            }
        }


        private bool Rebrand()
        {
            bool failed = false;

            try
            {
                typeof(Provider).GetField("APP_NAME", BindingFlags.Static | BindingFlags.Public).SetValue(null, "Tsunami Hack by Tidal");
                typeof(Provider).GetField("APP_AUTHOR", BindingFlags.Static | BindingFlags.Public).SetValue(null, "Tidal");
            }
            catch (System.NullReferenceException e)
            {
                Logging.Exception(e);
                failed = true;
            }

            if (failed)
                return !failed;
            
            return true;
        }
    }
}
