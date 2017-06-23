using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Lib
{
    class Keybinds : MonoBehaviour, ILibParent
    {
        public void Start()
        {
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                Manager.WaveMaker.main.menuOpened = !Manager.WaveMaker.main.menuOpened;
            }
        }
    }
}
