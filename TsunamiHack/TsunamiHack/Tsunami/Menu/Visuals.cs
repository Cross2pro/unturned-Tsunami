using UnityEngine;

namespace TsunamiHack.Tsunami.Menu
{
    public class Visuals : MonoBehaviour, IMenuParent
    {
        public bool MenuOpened { get; private set; }
        
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
    }
}