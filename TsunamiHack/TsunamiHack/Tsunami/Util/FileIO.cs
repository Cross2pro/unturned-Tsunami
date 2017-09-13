using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.IO;
using SDG.Unturned;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types;
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
        private static readonly string EulaPath = Directory + @"\Agreement.dat";

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

        public static void LoadSettings(out Settings settings )
        {
            var raw = File.ReadAllText(SettingsPath);
            settings = JsonConvert.DeserializeObject<Settings>(raw);
        }

        public static void CreateSettings(out Settings settings)
        {
            settings = new Settings();

            settings.ColorList.Add("enemyplayer", new TsuColor(new Color(255,45,45)));
            settings.ColorList.Add("friendlyplayer", new TsuColor(new Color(150,255,255)));
            settings.ColorList.Add("zombie",new TsuColor(new Color(50,150,0)));
            settings.ColorList.Add("item", new TsuColor(new Color(230,230,40)));
            settings.ColorList.Add("interactable", new TsuColor(new Color(255,180,0)));
            settings.ColorList.Add("vehicle", new TsuColor(new Color(255,0,230)));
            settings.ColorList.Add("friendlyplayerbox", new TsuColor(new Color(150,255,255)));
            settings.ColorList.Add("enemyplayerbox", new TsuColor(new Color(255,45,45)));
            settings.ColorList.Add("zombiebox", new TsuColor(new Color(50,150,0)));
          
            using (var writer = new StreamWriter(SettingsPath))
            {   
                var json = JsonConvert.SerializeObject(settings);
                writer.WriteLine(json);
                writer.Flush();
                writer.Dispose();
            }
        }

        
        // ReSharper disable once UnusedMember.Local
        private static void SaveSettings(Settings settings)
        {
            var raw = JsonConvert.SerializeObject(settings);

            if (!SettingsExist())
            {
                File.Create(SettingsPath).Dispose();
            }

            File.WriteAllText(SettingsPath, raw);
        }

        public static void SaveColors(Settings settings)
        {
            using (var writer = new StreamWriter(SettingsPath))
            {   
                var json = JsonConvert.SerializeObject(settings);
                writer.WriteLine(json);
                writer.Flush();
                writer.Dispose();
            }
        }
        
    #endregion

    #region first time

        public static bool CheckIfFirstTime()
        {
            var result = File.Exists(InfoPath);

            if (result)
            {
                string str;
                
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
                        keybinds.AddBind("toggleaimbot", KeyCode.F5);


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

                        settings.ColorList.Add("enemyplayer", new TsuColor(new Color(255,45,45)));
                        settings.ColorList.Add("friendlyplayer", new TsuColor(new Color(150,255,255)));
                        settings.ColorList.Add("zombie", new TsuColor(new Color(50,150,0)));
                        settings.ColorList.Add("item", new TsuColor(new Color(230,230,40)));
                        settings.ColorList.Add("interactable", new TsuColor(new Color(255,180,0)));
                        settings.ColorList.Add("vehicle", new TsuColor(new Color(255,0,230)));
                        settings.ColorList.Add("friendlyplayerbox", new TsuColor(new Color(150,255,255)));
                        settings.ColorList.Add("enemyplayerbox", new TsuColor(new Color(255,45,45)));
                        settings.ColorList.Add("zombiebox", new TsuColor(new Color(50,150,0)));
                        
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

        public static void CheckIfAgreed()
        {
            var res = File.Exists(EulaPath);

            if (!res)
            {
                WaveMaker.ShowEula = true;
                
            }
                
        }

        public static void AgreeEula()
        {
            using (var writer = new StreamWriter(EulaPath))
            {
                writer.WriteLine("Agreed");
                writer.Flush();
                writer.Dispose();
            }

            WaveMaker.ShowEula = false;
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
