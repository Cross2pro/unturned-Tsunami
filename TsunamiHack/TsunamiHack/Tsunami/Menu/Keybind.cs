using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    class Keybind : MonoBehaviour
    {
        private void Start()
        {
            
        }

        private void Update()
        {
            if (Provider.isConnected)
            {
                if (Input.GetKeyUp(KeyCode.F1))
                {
                    HackManager.main.menuOpened = !HackManager.main.menuOpened;
                }
            }
        }

        private void OnGUI()
        {
            
        }
    }
}
