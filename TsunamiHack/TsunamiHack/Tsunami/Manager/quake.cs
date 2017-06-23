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
        Manager man = new Manager();

        private void Start()
        {
            man.Start();
        }

        private void Update()
        {
            man.Update();
        }

        private void OnGUI()
        {
        }
    }
}
