using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class HackManager : MonoBehaviour
    {
        public static Menu.Main main;
        public static Menu.Keybind keybind;


        private GameObject obj;

        public void OnUpdate()
        {
            if (Provider.isConnected)
            {
                if (obj == null)
                {
                    obj = new GameObject();
                    main = obj.AddComponent<Menu.Main>();
                    keybind = obj.AddComponent<Menu.Keybind>();

                    UnityEngine.Object.DontDestroyOnLoad(main);
                    UnityEngine.Object.DontDestroyOnLoad(keybind);
                }
            }
        }
    }
}
