using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
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
            if (button2)
            {
                Player.player.look.isOrbiting = true;
            }
            else
            {
                Player.player.look.isOrbiting = false;
            }
        }

        private void OnGUI()
        {
            if (menuOpened)
            {
                windowRect = GUI.Window(1, windowRect, new GUI.WindowFunction(mainWindowFucnt), "Main Window");
            }
            
        }

        private void mainWindowFucnt(int id)
        {
            button1 = GUILayout.Toggle(button1, " Button 1", new GUILayoutOption[0]);
            button2 = GUILayout.Toggle(button2, " Free Flight", new GUILayoutOption[0]);
            GUI.DragWindow();
        }
    }
}
