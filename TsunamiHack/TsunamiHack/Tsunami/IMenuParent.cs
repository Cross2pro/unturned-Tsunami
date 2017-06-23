using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsunamiHack.Tsunami
{
    interface IMenuParent
    {

        #region Unity Update Loop Members
        void Start();
        void Update();
        void OnGUI();
        void MenuFunct(int id);
        #endregion

        #region Menu Members
        void setMenuStatus(bool setting);
        void toggleMenuStatus();
        bool getMenuStatus();
        #endregion
    }
}
