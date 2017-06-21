using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    class TsunamiHack : MonoBehaviour
    {
        HackManager manager = new HackManager();

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
