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
        //TODO: Change all file io to streams

        private static readonly string Path = Application.persistentDataPath;
        private static readonly string Directory = Path + @"\Tsunami";
        private static readonly string KeybindPath = Directory + @"\Keybinds.dat";
        private static readonly string InfoPath = Directory + @"\Info.dat";
        private static readonly string FriendsPath = Directory + @"\Friends.dat";
        private static readonly string SettingsPath = Directory + @"\Settings.dat";

        private static StreamReader _reader;
        private static StreamWriter _writer;

        public static void CheckDirectory()
        {
            if (!System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }
        }

        public static void CheckEmpty(string path)
        {

        }

    #region Keybinds

        public static bool KeybindsExist()
        {
            return File.Exists(KeybindPath);
        }

        [Obsolete]
        public static void LoadKeybinds(out KeybindConfig keybinds)
        {
            keybinds = null;

            var raw = File.ReadAllText(KeybindPath);
            keybinds = JsonConvert.DeserializeObject<KeybindConfig>(raw);
        }

        public static void CreateKeybinds(out KeybindConfig keybinds)
        {
            
            keybinds = new KeybindConfig();
            keybinds.AddBind("Main Menu", KeyCode.F1);
            keybinds.AddBind("Keybind Menu", KeyCode.F2);

            SaveKeybinds(keybinds);
        }

        [Obsolete]
        private static void SaveKeybinds(KeybindConfig config)
        {
            var json = JsonConvert.SerializeObject(config);

            if (!KeybindsExist())
            {
                File.Create(KeybindPath).Dispose();
            }

            File.WriteAllText(KeybindPath, json);
        }

    #endregion

    #region Friends

        public static bool FriendsExist()
        {
            return File.Exists(FriendsPath);
        }

        [Obsolete]
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

        [Obsolete]
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

        [Obsolete]
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

        [Obsolete]
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
        
    #region  Stream

        public static void StreamLoadKeybinds(out KeybindConfig keybinds)
        {
            _reader = new StreamReader(KeybindPath);
            keybinds = null;
            var json = _reader.ReadToEnd();
            _reader.Dispose();
            keybinds = JsonConvert.DeserializeObject<KeybindConfig>(json);
        }

        public static void StreamSaveKeybinds( KeybindConfig config)
        {
            var json = JsonConvert.SerializeObject(config);
            _writer = new StreamWriter(KeybindPath);

            if (!KeybindsExist())
            {
                File.Create(KeybindPath).Dispose();
            }
            
            _writer.Write(json);
            _writer.Dispose();
        }

        public static void StreamLoadFriends(out FriendsList friends)
        {
            _reader = new StreamReader(FriendsPath);
            friends = null;
            var json = _reader.ReadToEnd();
            _reader.Dispose();
            friends = JsonConvert.DeserializeObject<FriendsList>(json);
        }

        public static void StreamSaveFriends(FriendsList list)
        {
            _writer = new StreamWriter(FriendsPath);
            var json = JsonConvert.SerializeObject(list);
            _writer.Write(json);
            _writer.Dispose();
        }

        public static void StreamLoadSettings(out Settings list)
        {
            _reader = new StreamReader(SettingsPath);
            var json = _reader.ReadToEnd();
            _reader.Dispose();
            list = null;
            list = JsonConvert.DeserializeObject<Settings>(json);
        }

        public static void StreamSaveSettings(Settings settings)
        {
            _writer = new StreamWriter(SettingsPath);
            var json = JsonConvert.SerializeObject(settings);
            _writer.Write(json);
            _writer.Dispose();
        }
        
    #endregion
    
    }
}
