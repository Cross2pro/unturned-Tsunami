using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private Rect _windowRect;

        private bool _testButton;



        public void Start()
        {
            var size = new Vector2(200, 500);
            _windowRect = Util.MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
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
                    _windowRect = GUI.Window(1, _windowRect, new GUI.WindowFunction(MenuFunct), "Main Menu");
                }
            }
        }

        public void MenuFunct(int id)
        {
            _testButton = GUILayout.Toggle(_testButton, "Test button", new GUILayoutOption[0]);
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
