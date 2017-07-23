using System.Collections.Generic;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types.Configs
{
    public class Settings
    {
        //TODO: add way to check the type of a setting, or restrict thereof
        
        public List<Setting> SettingList;

        public Dictionary<string,Color> ColorList;
        
        public Settings()
        {
            SettingList = new List<Setting>();
            ColorList = new Dictionary<string, Color>();
        }

        public void addSetting(string name, object value)
        {
            if (SettingList.Exists(setting => setting.Name == name))
                return;
            
            SettingList.Add(new Setting(name, value));
        }
    }
}
