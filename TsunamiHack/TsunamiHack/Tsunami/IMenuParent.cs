using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsunamiHack.Tsunami
{
    interface IMenuParent
    {

        void setMenuStatus(bool setting);
        void toggleMenuStatus();

        void Start();
        void Update();
        void OnGUI();
        void MenuFunct(int id);
    }
}
