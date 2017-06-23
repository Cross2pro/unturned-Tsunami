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

            Rebrand("Tsunami Hack By Tidal", "Tidal");

            //Loader.LoadAll();
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


        private static void Rebrand(string app, string author, string version = null)
        {
            typeof(Provider).GetField("APP_NAME", BindingFlags.Public | BindingFlags.Static).SetValue(null, app);
            typeof(Provider).GetField("APP_AUTHOR", BindingFlags.Public | BindingFlags.Static).SetValue(null, author);

            if (version != null)
            {
                typeof(Provider).GetField("APP_VERSION", BindingFlags.Public | BindingFlags.Static).SetValue(null, version);

            }
        }
    }
}
