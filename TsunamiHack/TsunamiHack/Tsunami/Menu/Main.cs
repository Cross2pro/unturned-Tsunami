using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    internal class Main : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        private Rect _windowRect;

        private bool _testButton;
        private bool _freeFlight;

        public void Start()
        {
            var size = new Vector2(200, 500);
            _windowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
        }

        public void Update()
        {
            Player.player.look.isOrbiting = _freeFlight;

//            PlayerPauseUI.active = MenuOpened;
//            PlayerUI.window.showCursor = MenuOpened;

//            if (miniOpened && _miniRect.x != Screen.height - 105)
//            {
//                var curry = _miniRect.y;
//                var newpos = curry - 2;
//
//                Logging.LogMsg("POS", $"Rect y is {curry}");
//                Logging.LogMsg("POS", $"new y is {newpos}");
//                    
//                _miniRect.y = newpos;
//                
//            }
        }

        public void OnGUI()
        {
            if (Provider.isConnected)
            {
                if (WaveMaker.MenuOpened ==  WaveMaker.MainId)
                {
                    _windowRect = GUI.Window(1, _windowRect, MenuFunct, "Main Menu");
                }

                
            }
            
        }

        public void MenuFunct(int id)
        {
            _testButton = GUILayout.Toggle(_testButton, "Test button");
            _freeFlight = GUILayout.Toggle(_freeFlight, "Free Flight");

            
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
