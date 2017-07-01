using System.Collections.Generic;
using UnityEngine;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;

namespace TsunamiHack.Tsunami.Util
{
    public class PopupController : MonoBehaviour
    {
        //TODO: Add first time popup window
        
        private  List<Popup> MenuList;
        private  List<Popup> DynamicList;

        public  bool EnableFirstTime;
        
        private void Start()
        {
            EnableFirstTime = WaveMaker.FirstTime;
            MenuList = new List<Popup>();
            DynamicList = new List<Popup>();
            
            //TODO: add default popups

            var msg =$"Use {WaveMaker.Keybinds.GetBind("main")} to open the main menu ";
            var rect = MenuTools.GetRectAtLoc(new Vector2(200, 50), MenuTools.Horizontal.Right, MenuTools.Vertical.Bottom, true, 5f);
            var startPopup = new Popup(rect, "Tsunami Hack Info", msg, WaveMaker.FtPopupId)
            {
                CloseEnabled = false
            };
            AddPopup(startPopup);
        }

        private void Update()
        {
            //TODO: simplify dynamic popup checking
//            foreach (var pop in MenuList)
//            {
//                if(pop.IsMoving)
//                    DynamicList.Add(pop);
//            }
//
//            if (DynamicList.Count > 0)
//            {
                //TODO: add dynamic popup moving logic 
//            }
//            Logging.LogMsg("DEBUG", "About to check for first time");
            if (WaveMaker.FirstTime)
            {
//                Logging.LogMsg("DEBUG", "About to get popup and set opened to true");
                GetPopup(WaveMaker.FtPopupId).PopupOpened = true;
                WaveMaker.FirstTime = false;
            }
        }

        private void OnGUI()
        {
            if (Provider.isConnected)
            {
                foreach (var popup in MenuList)
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
            if (MenuList.Exists(p => p.Id == newPopup.Id) ||
                MenuList.Exists(p => p == newPopup)) return;
            
            MenuList.Add(newPopup);
        }

        public Popup GetPopup(int id)
        {
            if (MenuList.Exists(p => p.Id == id))
            {
                return MenuList.Find(popup => popup.Id == id);
            }

            return null;
        }

        public Popup GetPopup(string title)
        {
            if (MenuList.Exists(p => p.PopupTitle == title))
            {
                return MenuList.Find(popup => popup.PopupTitle == title);
            }

            return null;
        }

        public void DeactivatePopup(int id)
        {
            if (MenuList.Exists(p => p.Id == id))
            {
                MenuList.Remove(MenuList.Find(p => p.Id == id));
            }
        }
    }
}