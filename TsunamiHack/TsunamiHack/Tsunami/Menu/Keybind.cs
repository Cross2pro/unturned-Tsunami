using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static bool Changing;
        public static string focus;
        public static bool KeybindsChanged;

        //TODO:implement loaded keybind config

        public void Start()
        {
            var size = new Vector2(200,300);
            _windowRect = MenuTools.GetRectAtLoc(size, MenuTools.Horizontal.RightMid,MenuTools.Vertical.Center, false);
        }

        public void Update()
        {
            if (Changing)
            {
                if (Event.current.type == EventType.KeyDown)
                {
                    var pressed = Event.current.keyCode;

                    if (WaveMaker.Keybinds.BindExists(pressed))
                    {
                        Changing = false;
                        
                        WaveMaker.PopupController.AddPopup(new Popup(MenuTools.GetRectAtLoc(new Vector2(100,200), MenuTools.Horizontal.Right, MenuTools.Vertical.Bottom, true, 5f), 1000, "Keychange Error", "A bind with that key already exists"));

                        WaveMaker.PopupController.GetPopup(1000).PopupOpened = true;

                        
                    }
                    else
                    {
                        WaveMaker.Keybinds.ChangeBind(focus, pressed);
                    }
                    
                    
                     
                }
            }
            
            Lib.Keybind.Check();

            if (Provider.isConnected)
            {
                if (Input.GetKeyUp(KeyCode.F1))
                {
                    WaveMaker.MenuMain.ToggleMenuStatus();

                    if (WaveMaker.PopupController.GetPopup(WaveMaker.FtPopupId).PopupOpened)
                    {
                        WaveMaker.PopupController.GetPopup(WaveMaker.FtPopupId).PopupOpened = false;
                    }
                }
                if (Input.GetKeyUp(KeyCode.F2))
                {
                    ToggleMenuStatus();
                }
            }

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
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Main Menu :");
            GUILayout.Space(1f);
            if (GUILayout.Button($"{WaveMaker.Keybinds.GetBind("main")}"))
            {
                Changing = true;
                focus = "main";
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Visuals Menu :");
            GUILayout.Space(1f);
            if (GUILayout.Button($"{WaveMaker.Keybinds.GetBind("visuals")}"))
            {
                Changing = true;
                focus = "visuals";
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Keybind Menu :");
            GUILayout.Space(1f);
            if (GUILayout.Button($"{WaveMaker.Keybinds.GetBind("keybinds")}"))
            {
                Changing = true;
                focus = "keybinds";
            }
            GUILayout.EndHorizontal();
                
            GUILayout.EndVertical();
            
//            GUILayout.Label($"Main Menu : {WaveMaker.Keybinds.GetBind("main").ToString()}");
//            GUILayout.Space(2f);
//            GUILayout.Label($"Visuals Menu : {WaveMaker.Keybinds.GetBind("visuals").ToString()}");
//            GUILayout.Space(2f);
//            GUILayout.Label($"Keybind Menu : {WaveMaker.Keybinds.GetBind("keybinds").ToString()}");
//            GUILayout.Space(2f);
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
