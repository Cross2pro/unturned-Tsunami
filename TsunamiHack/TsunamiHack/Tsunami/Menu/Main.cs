using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    class Main : MonoBehaviour
    {
        private Rect windowRect;
        public bool menuOpened;

        private bool button1;
        private bool button2;

        private void Start()
        {
            windowRect = new Rect(((Screen.width / 2) - 100), ((Screen.height /2) - 250), 200, 500 );
        }

        private void Update()
        {
            
        }

        private void OnGUI()
        {
            if (menuOpened)
            {
                windowRect = GUI.Window(1, windowRect, new GUI.WindowFunction(mainWindowFucnt), "Main Window", new GUIStyle(GUIStyle.none));
            }
            
        }

        private void mainWindowFucnt(int id)
        {
            button1 = GUILayout.Toggle(button1, " Button 1", new GUILayoutOption[0]);
            button2 = GUILayout.Toggle(button2, " Button 2", new GUILayoutOption[0]);
        }
    }
}
