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

        public void setMenuStatus(bool setting)
        {
            menuOpened = setting;
        }

        public void toggleMenuStatus()
        {
            menuOpened = !menuOpened;
        }

        public void Start()
        {
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
    }
}
