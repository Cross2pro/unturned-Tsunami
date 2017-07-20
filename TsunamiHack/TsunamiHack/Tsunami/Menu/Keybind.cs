using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Configs;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    internal class Keybind : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        private Rect _windowRect;
        public bool Changing;
        public string focus;
        public bool KeybindsChanged;


        private KeyCode _MainKey;
        private KeyCode _VisualsKey;
        private KeyCode _KeybindKey;
        private KeyCode _AimKey;
        private KeyCode _ChangeTargetKey;
        
        public void Start()
        {
            var size = new Vector2(200,300);
            _windowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);

            _MainKey = WaveMaker.Keybinds.GetBind("main");
            _VisualsKey = WaveMaker.Keybinds.GetBind("visuals");
            _KeybindKey = WaveMaker.Keybinds.GetBind("keybinds");
            _AimKey = WaveMaker.Keybinds.GetBind("aim");
            _ChangeTargetKey = WaveMaker.Keybinds.GetBind("changetarget");


        }

        public void Update()
        {
            //creates popup rect
            var rect = MenuTools.GetRectAtLoc(new Vector2(180, 75), MenuTools.Horizontal.Right,
                MenuTools.Vertical.Bottom, true, 5f);
            var popup = new Popup(rect, 1000, "Change Error", "Key is already in use");
            WaveMaker.PopupController.AddPopup(popup);
            // --------------------
            
            if (Event.current.type == EventType.KeyDown)
            {
                if (Changing)
                {
                    if (WaveMaker.Keybinds.BindExists(Event.current.keyCode))
                    {
                        Changing = false;
                        WaveMaker.PopupController.GetPopup(1000).PopupOpened = true;
                    }
                    else
                    {
                        WaveMaker.Keybinds.ChangeBind(focus, Event.current.keyCode);
                        Changing = false;
                        KeybindsChanged = true;
                        WaveMaker.Keybinds.SaveBinds();
                    }
                }
                else
                {
                    var pressed = Event.current.keyCode;

                    if (pressed == _MainKey)
                    {
                        var ftpopup = WaveMaker.PopupController.GetPopup(WaveMaker.FtPopupId);
                        
                        if (ftpopup.PopupOpened)
                        {
                            ftpopup.PopupOpened = false;
                        }
                        
                        UseMenu(WaveMaker.MainId);

                        if (WaveMaker.FirstTime && WaveMaker.PopupController.GetPopup(WaveMaker.FtPopupId).PopupOpened)
                        {
                            WaveMaker.PopupController.GetPopup(WaveMaker.FtPopupId).PopupOpened = false;
                            WaveMaker.FirstTime = false;
                        }
                    }
                    else if (pressed == _VisualsKey)
                        UseMenu(WaveMaker.VisualsId);
                    else if (pressed == _KeybindKey)
                        UseMenu(WaveMaker.KeybindId);
                    else if (pressed == _AimKey)
                        UseMenu(WaveMaker.AimId);
                        
                    //TODO: add other menus
                    //TODO: add change key logic

                    if (pressed == KeyCode.Escape && WaveMaker.MenuOpened != 0)
                    {
                        UseMenu(WaveMaker.MenuOpened);
                    }
                }
                
            }
            
            CheckChange();
        }

        private void UseMenu(int id)
        {
            if (WaveMaker.MenuOpened == id)
            {
                WaveMaker.MenuOpened = 0;
                PlayerPauseUI.active = false;
                PlayerUI.window.showCursor = false;
            }
            else
            {
                WaveMaker.MenuOpened = id;
                PlayerPauseUI.active = true;
                PlayerUI.window.showCursor = false;
            }
            
        }

        private void CheckChange()
        {
            if (KeybindsChanged)
            {
                _MainKey = WaveMaker.Keybinds.GetBind("main");
                _VisualsKey = WaveMaker.Keybinds.GetBind("visuals");
                _KeybindKey = WaveMaker.Keybinds.GetBind("keybinds");
                _AimKey = WaveMaker.Keybinds.GetBind("aim");
                _ChangeTargetKey = WaveMaker.Keybinds.GetBind("changetarget");

                
                KeybindsChanged = false;
            }
        }
        
        public void OnGUI()
        {
            if (Provider.isConnected)
            {
                if (WaveMaker.MenuOpened == WaveMaker.KeybindId)
                {
                    _windowRect = GUI.Window(2, _windowRect, MenuFunct, "Keybind Menu");
                }
            }

        }
        
        

        public void MenuFunct(int id)
        {

            if (GUILayout.Button($"Main Menu : {WaveMaker.Keybinds.GetBind("main")}"))
            {
                Changing = true;
                focus = "main";
            }
            GUILayout.Space(2f);
            
            if (GUILayout.Button($"Visuals Menu : {WaveMaker.Keybinds.GetBind("visuals")}"))
            {
                Changing = true;
                focus = "visuals";
            }
            GUILayout.Space(2f);
            
            if (GUILayout.Button($"Aim Menu : {WaveMaker.Keybinds.GetBind("aim")}"))
            {
                Changing = true;
                focus = "aim";
            }
            GUILayout.Space(2f);
            
            if (GUILayout.Button($"Keybinds Menu : {WaveMaker.Keybinds.GetBind("keybinds")}"))
            {
                Changing = true;
                focus = "keybinds";
            }
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
