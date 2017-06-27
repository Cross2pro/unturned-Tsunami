using System.Collections.Generic;

namespace TsunamiHack.Tsunami.Types.Configs
{
    public class Settings
    {
        //TODO: add way to check the type of a setting, or restrict thereof
        
        public List<Setting> SettingList;

        public Settings()
        {
            SettingList = new List<Setting>();
        }
    }
}
