using System;
using System.Collections.Generic;

namespace TsunamiHack.Tsunami.Types.Configs
{
    [Serializable]
    public class Settings
    {        
        public List<Setting> SettingList;

        public Dictionary<string,TsuColor> ColorList;
        
        public Settings()
        {
            SettingList = new List<Setting>();
            ColorList = new Dictionary<string, TsuColor>();
        }

        public void AddSetting(string name, object value)
        {
            if (SettingList.Exists(setting => setting.Name == name))
                return;
            
            SettingList.Add(new Setting(name, value));
        }
    }
}
