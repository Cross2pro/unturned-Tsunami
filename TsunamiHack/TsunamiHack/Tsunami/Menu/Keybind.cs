using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    class Keybind : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        private Rect _windowRect;

        private KeyCode mainMenu;
        private KeyCode keybindMenu;

        public void Start()
        {
            var size = new Vector2(200,300);
            _windowRect = Util.MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.RightMid,MenuTools.Vertical.Center, false);
        }

        public void Update()
        {
            if (Provider.isConnected)
            {
                if (Input.GetKeyUp(KeyCode.F1))
                {
                    WaveMaker.menuMain.toggleMenuStatus();
                }

                if (Input.GetKeyUp(KeyCode.F2))
                {
                    toggleMenuStatus();
                }
                
            }

        }

        public void OnGUI()
        {
            if (Provider.isConnected)
            {
                if (MenuOpened)
                {
                    _windowRect = GUI.Window(2, _windowRect, new GUI.WindowFunction(MenuFunct), "Keybind Menu");
                }
            }
        }

        public void MenuFunct(int id)
        {
            GUILayout.Button($"Main Menu : {mainMenu.ToString()}");
            GUILayout.Button($"KeybindMenu : {keybindMenu.ToString()}");
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
