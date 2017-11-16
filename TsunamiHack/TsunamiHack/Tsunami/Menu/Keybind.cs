using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Util;
using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    internal class Keybind : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        private Rect _windowRect;
        public bool Changing;
        public string Focus;
        public bool KeybindsChanged;
        public static Texture _cursorTexture;


        private KeyCode _mainKey;
        private KeyCode _visualsKey;
        private KeyCode _keybindKey;
        private KeyCode _aimKey;
        private KeyCode _changeTargetKey;
        private KeyCode _aimbotToggleKey;
                
        public void Start()
        {
            var size = new Vector2(200,300);
            _windowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.Center, MenuTools.Vertical.Center, false);

            _mainKey = WaveMaker.Keybinds.GetBind("main");
            _visualsKey = WaveMaker.Keybinds.GetBind("visuals");
            _keybindKey = WaveMaker.Keybinds.GetBind("keybinds");
            _aimKey = WaveMaker.Keybinds.GetBind("aim");
            _changeTargetKey = WaveMaker.Keybinds.GetBind("changetarget");
            _aimbotToggleKey = WaveMaker.Keybinds.GetBind("toggleaimbot");

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
                        WaveMaker.Keybinds.ChangeBind(Focus, Event.current.keyCode);
                        Changing = false;
                        KeybindsChanged = true;
                        WaveMaker.Keybinds.SaveBinds();
                    }
                }
                else
                {
                    var pressed = Event.current.keyCode;

                    if (pressed == _mainKey)
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
                    else if (pressed == _visualsKey)
                        UseMenu(WaveMaker.VisualsId);
                    else if (pressed == _keybindKey)
                        UseMenu(WaveMaker.KeybindId);
                    else if (pressed == _aimKey)
                        UseMenu(WaveMaker.AimId);
                    else if (pressed == _aimbotToggleKey)
                        WaveMaker.MenuAim.EnableAimbot = !WaveMaker.MenuAim.EnableAimbot;
                        
                    //TODO: add other menus

                    if (pressed == KeyCode.Escape && WaveMaker.MenuOpened != 0)
                    {
                        UseMenu(WaveMaker.MenuOpened);
                    }
                }
                
            }
            
            CheckChange();
        }

        internal void UseMenu(int id)
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
                PlayerUI.window.showCursor = true;
            }
            
        }

        private void CheckChange()
        {
            if (KeybindsChanged)
            {
                _mainKey = WaveMaker.Keybinds.GetBind("main");
                _visualsKey = WaveMaker.Keybinds.GetBind("visuals");
                _keybindKey = WaveMaker.Keybinds.GetBind("keybinds");
                _aimKey = WaveMaker.Keybinds.GetBind("aim");
                _changeTargetKey = WaveMaker.Keybinds.GetBind("changetarget");
                _aimbotToggleKey = WaveMaker.Keybinds.GetBind("toggleaimbot");
                
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

            if (WaveMaker.MenuOpened != 0)
            {
                if (_cursorTexture == null)
                    _cursorTexture = Resources.Load("UI/Cursor") as Texture;
                
                PlayerUI.window.showCursor = true;
                var _cursor = new Rect();
                _cursor.x = Input.mousePosition.x;
                _cursor.y = Screen.height - Input.mousePosition.y;
                GUI.depth = 0;
                GUI.DrawTexture(_cursor, _cursorTexture);
            }

        }
        
        

        public void MenuFunct(int id)
        {

            var maintext = Changing && Focus == "main" ? "??" : WaveMaker.Keybinds.GetBind("main").ToString();
            if (GUILayout.Button($"Main Menu : {maintext}"))
            {
                Changing = true;
                Focus = "main";
            }
            GUILayout.Space(2f);

            var visualstext = Changing && Focus == "visuals" ? "??" : WaveMaker.Keybinds.GetBind("visuals").ToString();
            if (GUILayout.Button($"Visuals Menu : {visualstext}"))
            {
                Changing = true;
                Focus = "visuals";
            }
            GUILayout.Space(2f);

            var aimtext = Changing && Focus == "aim" ? "??" : WaveMaker.Keybinds.GetBind("aim").ToString();
            if (GUILayout.Button($"Aim Menu : {aimtext}"))
            {
                Changing = true;
                Focus = "aim";
            }
            GUILayout.Space(2f);

            var keybindtext = Changing && Focus == "keybinds" ? "??" : WaveMaker.Keybinds.GetBind("keybinds").ToString();
            if (GUILayout.Button($"Keybinds Menu : {keybindtext}"))
            {
                Changing = true;
                Focus = "keybinds";
            }
            GUILayout.Space(2f);
            
            var aimtoggletext  = Changing && Focus == "Aimbot Toggle" ? "??" : WaveMaker.Keybinds.GetBind("toggleaimbot").ToString();
            if (GUILayout.Button($"Toggle Aimbot : {aimtoggletext}"))
            {
                Changing = true;
                Focus = "toggleaimbot";
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
