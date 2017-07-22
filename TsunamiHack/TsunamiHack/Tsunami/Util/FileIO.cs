using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Types.Configs;

namespace TsunamiHack.Tsunami.Util
{
    internal class FileIo
    {
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

        public static void CheckEmpty()
        {
            var list = new List<String>()
            {
                KeybindPath,
                FriendsPath,
                SettingsPath
            };

            foreach (var path in list)
            {
                if (File.Exists(path))
                {
                    _reader = new StreamReader(path);
                    
                    var contents = _reader.ReadToEnd();

                    _reader.Dispose();
                    
                    if (contents.Length == 0)
                    {
                        File.Delete(path);
                    }
                    
                }
                 
            }
        }

    #region Keybinds

        public static bool KeybindsExist()
        {
            return File.Exists(KeybindPath);
        }

        [Obsolete]
        public static void LoadKeybinds(out KeybindConfig keybinds)
        {
            var raw = File.ReadAllText(KeybindPath);
            keybinds = JsonConvert.DeserializeObject<KeybindConfig>(raw);
        }

        public static void CreateKeybinds(out KeybindConfig keybinds)
        {
            
            keybinds = new KeybindConfig();
            keybinds.AddBind("main", KeyCode.F1);
            keybinds.AddBind("visuals", KeyCode.F2);
            keybinds.AddBind("keybinds", KeyCode.F4);
            keybinds.AddBind("aim", KeyCode.F3);
            keybinds.AddBind("changetarget", KeyCode.Backslash);

            SaveKeybinds(keybinds);// --------------------// --------------------
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
            var raw = File.ReadAllText(FriendsPath);
            fList = JsonConvert.DeserializeObject<FriendsList>(raw);

        }

        public static void CreateFriends(out FriendsList fList)
        {
            fList = new FriendsList();
            SaveFriends(fList);
        }

        [Obsolete]
        public static void SaveFriends(FriendsList friends)
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

            if (result)
            {
                var str = "";
                
                using (var reader = new StreamReader(InfoPath))
                {
                    str = reader.ReadToEnd();
                    reader.Close();
                }

                if (str != WaveMaker.Version)
                {
                                        
                    using (var writer = new StreamWriter(KeybindPath))
                    {
                        //TODO: Add all the other keybinds
                        var keybinds = new KeybindConfig();
                        keybinds.AddBind("main", KeyCode.F1);
                        keybinds.AddBind("visuals", KeyCode.F2);
                        keybinds.AddBind("aim", KeyCode.F3);
                        keybinds.AddBind("keybinds", KeyCode.F4);
                        keybinds.AddBind("changetarget", KeyCode.Backslash);


                        WaveMaker.Keybinds = keybinds;
                        var json = JsonConvert.SerializeObject(keybinds);
                        writer.WriteLine(json);
                        writer.Flush();
                        writer.Dispose();
                    }

                    using (var writer = new StreamWriter(SettingsPath))
                    {
                        //TODO: add all other settings
                        var settings = new Settings();

                        WaveMaker.Settings = settings;
                        var json = JsonConvert.SerializeObject(settings);
                        writer.WriteLine(json);
                        writer.Flush();
                        writer.Dispose();
                    }

                    using (var writer = new StreamWriter(FriendsPath))
                    {
                        var friends = new FriendsList();

                        WaveMaker.Friends = friends;
                        var json = JsonConvert.SerializeObject(friends);
                        writer.WriteLine(json);
                        writer.Flush();
                        writer.Dispose();
                    }

                    using (var writer = new StreamWriter(InfoPath))
                    {
                        writer.WriteLine(WaveMaker.Version);
                        writer.Flush();
                        writer.Dispose();
                    }
                }

            }
            else
            {
                File.WriteAllText(InfoPath, WaveMaker.Version);
            }

            return !result;
        }

    #endregion  
        
    
        
    #region  Stream

        public static void StreamLoadKeybinds(out KeybindConfig keybinds)
        {
            _reader = new StreamReader(KeybindPath);
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
