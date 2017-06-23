using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class Quake : MonoBehaviour
    {
        WaveMaker WM = new WaveMaker();

        public void Start()
        {
            //Add all hack components
            //Call loader

            Loader.LoadAll();
            WM.Start();
        }

        public void Update()
        {
            //Call Updates

            WM.OnUpdate();
        }

        public void OnGUI()
        {
            //nothing
        }
    }
}
