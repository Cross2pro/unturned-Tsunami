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
        private Menu.Main main;
        private GameObject obj;

        public void OnUpdate()
        {
            if (Provider.isConnected)
            {
                if (obj == null)
                {
                    obj = new GameObject();
                    main = obj.AddComponent<Menu.Main>();

                    UnityEngine.Object.DontDestroyOnLoad(main);
                }
            }
        }
    }
}
