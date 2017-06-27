using System;
using UnityEngine;
using Newtonsoft.Json;
using TsunamiHack.Tsunami.Types;
using System.IO;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Types.Configs;

namespace TsunamiHack.Tsunami.Util
{
    class FileIo
    {
        //TODO: Implement a way to check if a file is empty and if so create a file instead.

        private static readonly string Path = Application.persistentDataPath;
        private static readonly string Directory = Path + @"\Tsunami";
        private static readonly string KeybindPath = Directory + @"\Keybinds.dat";
        private static readonly string InfoPath = Directory + @"\Info.dat";
        private static readonly string FriendsPath = Directory + @"\Friends.dat";
        private static readonly string SettingsPath = Directory + @"\Settings.dat";

        public static void CheckDirectory()
        {
            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }
        }

    #region Keybinds

        public static bool KeybindsExist()
        {
            return File.Exists(KeybindPath);
        }

        public static void LoadKeybinds(out KeybindConfig keybinds)
        {
            keybinds = null;

            var raw = File.ReadAllText(KeybindPath);
            keybinds = JsonConvert.DeserializeObject<KeybindConfig>(raw);
        }

        public static void CreateKeybinds(out KeybindConfig keybinds)
        {
            
            var output = new KeybindConfig();
            output.AddBind("Main Menu", KeyCode.F1);
            output.AddBind("Keybind Menu", KeyCode.F2);

            Logging.LogMsg("Keybinds instance created","Instance created, binds added");   //remove later

            SaveKeybinds(output);
            keybinds = output;
        }

        private static void SaveKeybinds(KeybindConfig config)
        {
            var json = JsonConvert.SerializeObject(config);

            if (!KeybindsExist())
            {
                File.Create(KeybindPath).Dispose();
            }

            File.WriteAllText(KeybindPath, json);

            Logging.LogMsg("Keybinds pasted and saved", "saved");  //remove later
        }

    #endregion

    #region Friends

        public static bool FriendsExist()
        {
            return File.Exists(FriendsPath);
        }

        public static void LoadFriends(out FriendsList fList)
        {
            fList = null;

            var raw = File.ReadAllText(FriendsPath);
            fList = JsonConvert.DeserializeObject<FriendsList>(raw);

        }

        public static void CreateFriends(out FriendsList fList)
        {
            fList = new FriendsList();
            SaveFriends(fList);
        }

        private static void SaveFriends(FriendsList friends)
        {
            var json = JsonConvert.SerializeObject(friends);

            if (!FriendsExist())
            {
                File.Create(FriendsPath).Dispose();
            }

            File.WriteAllText(FriendsPath, json);
        }

    #endregion

    #region Settings

        public static bool SettingsExist()
        {
            return File.Exists(SettingsPath);
        }

        public static void LoadSettings(out Settings settings )
        {
            var raw = File.ReadAllText(SettingsPath);
            settings = JsonConvert.DeserializeObject<Settings>(raw);
        }

        public static void CreateSettings(out Settings settings)
        {
            settings = new Settings();

            //TODO: Add default settings

            SaveSettings(settings);
        }

        private static void SaveSettings(Settings settings)
        {
            var raw = JsonConvert.SerializeObject(settings);

            if (!SettingsExist())
            {
                File.Create(SettingsPath).Dispose();
            }

            File.WriteAllText(SettingsPath, raw);
        }

    #endregion

    #region first time

        public static bool CheckIfFirstTime()
        {
            var result = File.Exists(InfoPath);

            if (!result)
            {
                File.Create(InfoPath);
            }

            return result;
        }

    #endregion  
    }
}
