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
            _windowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
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
                    _windowRect = GUI.Window(1, _windowRect, MenuFunct, "Main Menu");
                }
            }
        }

        public void MenuFunct(int id)
        {
            _testButton = GUILayout.Toggle(_testButton, "Test button");
            GUI.DragWindow();
        }



        #region Interface Members

        public void SetMenuStatus(bool setting)
        {
            MenuOpened = setting;
        }

        public void ToggleMenuStatus()
        {
            MenuOpened = !MenuOpened;
        }

        public bool GetMenuStatus()
        {
            return MenuOpened;
        }
        #endregion
    }
}
