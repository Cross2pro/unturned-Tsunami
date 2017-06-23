using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class WaveMaker
    {
        private GameObject obj;

        public static Menu.Main main;
        public static Lib.Keybinds keybind;

        public List<UnityEngine.Object> ComponentList;

        public void Start()
        {
            
        }

        public void Update()
        {
            if (Provider.isConnected)
            {
                if (obj == null)
                {
                    obj = new GameObject();
                    main = obj.AddComponent<Menu.Main>();
                    keybind = obj.AddComponent<Lib.Keybinds>();

                    UnityEngine.Object.DontDestroyOnLoad(main);
                    UnityEngine.Object.DontDestroyOnLoad(keybind);
                }
            }
        }

        public void OnGUI()
        {
            
        }

    }
}
