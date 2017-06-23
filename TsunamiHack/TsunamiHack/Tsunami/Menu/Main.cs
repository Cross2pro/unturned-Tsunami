using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    class Main : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        private Rect windowRect;

        private bool testButton;

        public void Start()
        {
            var size = new Vector2(200, 500);
            windowRect = Util.MenuTools.getRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
        }

        public void Update()
        {
        }

        public void OnGUI()
        {
            if (Provider.isConnected)
            {
                if (MenuOpened)
                {
                    windowRect = GUI.Window(1, windowRect, new GUI.WindowFunction(MenuFunct), "Main Menu");
                }
            }
        }

        public void MenuFunct(int id)
        {
            testButton = GUILayout.Toggle(testButton, "Test button", new GUILayoutOption[0]);
            GUI.DragWindow();
        }

        #region Interface Members

        public void setMenuStatus(bool setting)
        {
            MenuOpened = setting;
        }

        public void toggleMenuStatus()
        {
            MenuOpened = !MenuOpened;
        }

        public bool getMenuStatus()
        {
            return MenuOpened;
        }
        #endregion
    }
}
