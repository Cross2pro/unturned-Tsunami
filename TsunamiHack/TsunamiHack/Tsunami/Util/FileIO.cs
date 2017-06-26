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

        public static bool CreateKeybinds(out KeybindConfig keybinds)
        {
            var success = false;

            try
            {
                keybinds = new KeybindConfig();
                keybinds.AddBind("Main", KeyCode.F1);
                keybinds.AddBind("Keybinds", KeyCode.F2);

                //TODO: add other default keybinds

                SaveKeybinds(keybinds);
                success = true;
            }
            catch (Exception e)
            {

                keybinds = null;
            }

            return success;
        }

        private static void SaveKeybinds(KeybindConfig config)
        {
            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(KeybindPath, json);
        }

        #endregion

        #region Friends

        public static bool FriendsExist()
        {
            return File.Exists( FriendsPath );
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
            SaveSettings(settings);
        }

        private static void SaveSettings(Settings settings)
        {
            var raw = JsonConvert.SerializeObject(settings);
            File.WriteAllText(SettingsPath, raw);
        }

        #endregion

        #region first time

        public static bool CheckIfFirstTime()
        {
            if (File.Exists(InfoPath))
            {
                var time = File.ReadAllText(InfoPath);

                try
                {
                    if (DateTime.Parse(time) == DateTime.Now)
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    return true;
                }

                return false;
            }
            else
            {
                File.Create(InfoPath);
                File.WriteAllText(InfoPath, DateTime.Now.ToString());
                return true;
            }
        }

        #endregion  
    }
}
