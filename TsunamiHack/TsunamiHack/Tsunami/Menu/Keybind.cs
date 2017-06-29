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
    internal class Keybind : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        private Rect _windowRect;

        public static bool KeybindsChanged;

        //TODO:implement loaded keybind config

        public void Start()
        {
            var size = new Vector2(200,300);
            _windowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.RightMid,MenuTools.Vertical.Center, false);
        }

        public void Update()
        {
            Lib.Keybind.Check();

            if (Provider.isConnected)
            {
                if (Input.GetKeyUp(KeyCode.F1))
                {
                    WaveMaker.MenuMain.ToggleMenuStatus();
                }
                if (Input.GetKeyUp(KeyCode.F2))
                {
                    ToggleMenuStatus();
                }
            }

//            PlayerPauseUI.active = MenuOpened;
//            PlayerUI.window.showCursor = MenuOpened;
        }

        public void OnGUI()
        {
            if (Provider.isConnected)
            {
                if (MenuOpened)
                {
                    _windowRect = GUI.Window(2, _windowRect, MenuFunct, "Keybind Menu");
                }
            }
        }

        public void MenuFunct(int id)
        {
            GUILayout.Label($"Main Menu : {WaveMaker.Keybinds.GetBind("main").ToString()}");
            GUILayout.Space(2f);
            GUILayout.Label($"Visuals Menu : {WaveMaker.Keybinds.GetBind("visuals").ToString()}");
            GUILayout.Space(2f);
            GUILayout.Label($"Keybind Menu : {WaveMaker.Keybinds.GetBind("keybinds").ToString()}");
            GUILayout.Space(2f);
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
