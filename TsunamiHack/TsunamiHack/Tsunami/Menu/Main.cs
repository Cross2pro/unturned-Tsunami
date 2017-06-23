using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TsunamiHack;
using TsunamiHack.Tsunami.Util;

namespace TsunamiHack.Tsunami.Menu
{
    public class Main : MonoBehaviour, IMenuParent
    {
        private bool menuOpened { get; set; }

        private Rect WindowRect;
        private Vector2 WindowSize;

        private bool button1;

        public void Start()
        {
            WindowSize = new Vector2(200, 450);
            WindowRect = MenuTools.getRectAtLoc(WindowSize, MenuTools.HorizontalLoc.Center, MenuTools.VerticalLoc.Center, false);
        }

        public void Update()
        {
        }

        public void OnGUI()
        {
            if (menuOpened)
            {
                WindowRect = GUILayout.Window(1, WindowRect, new GUI.WindowFunction(MenuFunct), "Main Menu",new GUILayoutOption[0]);
            }
        }

        public void MenuFunct(int id)
        {
            button1 = GUILayout.Toggle(button1, "Button 1 test");
            GUI.DragWindow();
        }

        #region Interface Members

        public void setMenuStatus(bool setting)
        {
            menuOpened = setting;
        }

        public void toggleMenuStatus()
        {
            menuOpened = !menuOpened;
        }

        public bool getMenuStatus()
        {
            return menuOpened;
        }
        #endregion
    }
}
