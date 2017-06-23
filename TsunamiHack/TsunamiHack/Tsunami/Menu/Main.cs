using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TsunamiHack;

namespace TsunamiHack.Tsunami.Menu
{
    public class Main : MonoBehaviour, IMenuParent
    {
        private bool menuOpened { get; set; }

        private Rect WindowRect;


        public void Start()
        {
            WindowRect = new Rect( ((Screen.width / 2) - 100) , ((Screen.height /2) + 225), 200, 450);
        }

        public void Update()
        {
        }

        public void OnGUI()
        {

        }

        public void MenuFunct(int id)
        {
        }

        #region Interface Members

        public void setMenuStatus(bool setting)
        {
            menuOpened = setting;
        }

        public void toggleMenuStatus()
        {
            menuOpened = !menuOpened;
        }

        public bool getMenuStatus()
        {
            return menuOpened;
        }
        #endregion
    }
}
