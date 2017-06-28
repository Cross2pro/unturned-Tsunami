using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsunamiHack.Tsunami
{
    internal interface IMenuParent
    {

        #region Unity Update Loop Members
        void Start();
        void Update();
        void OnGUI();
        void MenuFunct(int id);
        #endregion

        #region Menu Members
        void SetMenuStatus(bool setting);
        void ToggleMenuStatus();
        bool GetMenuStatus();
        #endregion
    }
}
