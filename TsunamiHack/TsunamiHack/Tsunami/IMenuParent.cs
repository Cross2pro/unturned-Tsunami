
namespace TsunamiHack.Tsunami
{
    internal interface IMenuParent
    {

        #region Unity Update Loop Members
        void Start();
        void Update();
        // ReSharper disable once InconsistentNaming
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
