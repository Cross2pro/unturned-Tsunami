using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class Quake : MonoBehaviour
    {
        WaveMaker manager = new WaveMaker();

        public void Start()
        {
            //Add all hack components
            //Call loader

            //Loader.LoadAll();
        }

        public void Update()
        {
            //Call Updates

            manager.OnUpdate();
        }

        public void OnGUI()
        {
            //nothing
        }

    }
}
