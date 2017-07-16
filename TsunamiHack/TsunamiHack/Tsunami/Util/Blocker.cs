using System;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    public class Blocker : MonoBehaviour
    {
        private HackController ctrl;

        private static bool BlockerEnabled;
        
        
        private void Start()
        {
            ctrl = WaveMaker.Controller;

            switch (ctrl.Disabled)
            {
                    case true:
                        BlockerEnabled = true;
                        
            }
        }

        private void Update()
        {
            
        }

        private void OnGUI()
        {
            
        }
    }
}