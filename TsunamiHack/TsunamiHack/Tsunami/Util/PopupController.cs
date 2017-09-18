using System.Collections.Generic;
using UnityEngine;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;

namespace TsunamiHack.Tsunami.Util
{
    internal class PopupController : MonoBehaviour
    {        
    
        //TODO: Add a way to deallocate unused windows using "Inuse" Popup var
        
        private  List<Popup> _menuList;
        private  List<Popup> _dynamicList;

        public static bool EnableFirstTime;
        
        private void Start()
        {
            EnableFirstTime = WaveMaker.FirstTime;
            _menuList = new List<Popup>();
            _dynamicList = new List<Popup>();
            
            //TODO: add default popups

            var msg =$"Use {WaveMaker.Keybinds.GetBind("main")} to open the main menu ";
            var rect = MenuTools.GetRectAtLoc(new Vector2(200, 50), MenuTools.Horizontal.Right, MenuTools.Vertical.Bottom, true, 5f);
            var startPopup = new Popup(rect, "Tsunami Hack Info", msg, WaveMaker.FtPopupId)
            {
                CloseAble = false
            };
            AddPopup(startPopup);
        }

        private void Update()
        {
            //TODO: simplify dynamic popup checking
            
            //TODO: add dynamic popup moving logic 
            
            if (WaveMaker.FirstTime && Provider.isConnected)
            {
                GetPopup(WaveMaker.FtPopupId).PopupOpened = true;
                WaveMaker.FirstTime = false;
            }
        }

        private void OnGUI()
        {
            if (Provider.isConnected)
            {
                foreach (var popup in _menuList)
                {
                    if (popup.PopupOpened)
                    {
                        popup.PopupRect = GUI.Window(popup.Id, popup.PopupRect, popup.PopupFunct, popup.PopupTitle);
                    }
                }
            }
        }

        public void AddPopup(Popup newPopup)
        {
            if (_menuList.Exists(p => p.Id == newPopup.Id) ||
                _menuList.Exists(p => p == newPopup)) return;
            
            _menuList.Add(newPopup);
        }

        public Popup GetPopup(int id)
        {
            if (_menuList.Exists(p => p.Id == id))
            {
                return _menuList.Find(popup => popup.Id == id);
            }

            return null;
        }

        public Popup GetPopup(string title)
        {
            if (_menuList.Exists(p => p.PopupTitle == title))
            {
                return _menuList.Find(popup => popup.PopupTitle == title);
            }

            return null;
        }

        public void DeactivatePopup(int id)
        {
            if (_menuList.Exists(p => p.Id == id))
            {
                _menuList.Remove(_menuList.Find(p => p.Id == id));
            }
        }
    }
}