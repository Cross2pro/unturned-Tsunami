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
    internal class Main : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        private bool miniOpened;
        private Rect _windowRect;
        private Rect _miniRect;

        private bool _testButton;
        private bool _freeFlight;

        //TODO: make class for generating popup windows

        public void Start()
        {
            var size = new Vector2(200, 500);
            _windowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);
            size = new Vector2(200, 100);
            _miniRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Right, MenuTools.Vertical.Bottom, true, 5f);
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
                if (MenuOpened)
                {
                    _windowRect = GUI.Window(1, _windowRect, MenuFunct, "Main Menu");
                }

                if (miniOpened)
                {
                    _miniRect = GUI.Window(4, _miniRect, MiniMenuFunct, "Popup");
                }
                
            }
            
        }

        public void MenuFunct(int id)
        {
            _testButton = GUILayout.Toggle(_testButton, "Test button");
            _freeFlight = GUILayout.Toggle(_freeFlight, "Free Flight");

            if (GUILayout.Button("Popup Window"))
            {
//                var pos = new Vector2((float)(Screen.width - 205), (float)(Screen.height - 105));
//                var size = new Vector2(200,100);
//                _miniRect = new Rect(pos, size);
//                miniOpened = true;
//                _miniRect = GUI.Window(4, _miniRect, MiniMenuFunct, "popup");
                miniOpened = !miniOpened;
            }
            
            GUI.DragWindow();
        }

        public void MiniMenuFunct(int id)
        {
            GUILayout.Label("Example Content");
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
