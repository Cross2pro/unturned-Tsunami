using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class Quake : MonoBehaviour
    {
        public WaveMaker WM;

        private void Start()
        {
            WM.Start();
        }

        private void Update()
        {
            WM.Start();
        }

        private void OnGUI()
        {
            WM.OnGUI();
        }
    }
}
