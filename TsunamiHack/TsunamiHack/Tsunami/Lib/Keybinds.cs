using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    class Keybinds : MonoBehaviour
    {
        private void Start()
        {
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                Manager.WaveMaker.main.menuOpened = !Manager.WaveMaker.main.menuOpened;
            }
        }
    }
}
