using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    class Main : MonoBehaviour
    {
        public bool menuOpened = false;
        private Rect windowRect = new Rect(20,20,100,300);

        private bool bool1;

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        private void OnGUI()
        {
                windowRect = GUI.Window(1, windowRect, new GUI.WindowFunction(WindowFunct), "Main Menu");
        }

        private void WindowFunct(int id)
        {
            bool1 = GUILayout.Toggle(bool1, "Test Button", new GUILayoutOption[0]);
            GUI.DragWindow();
        }
    }
}
