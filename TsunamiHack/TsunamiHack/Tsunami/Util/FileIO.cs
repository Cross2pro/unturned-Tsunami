using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using TsunamiHack.Tsunami.Types;
using System.IO;

namespace TsunamiHack.Tsunami.Util
{
    class FileIO
    {
        private static readonly string _path = Application.persistentDataPath;
        private static readonly string _keybindPath = _path + @"\Keybinds.dat";
        private static string _infoPath = _path + @"\Info.dat";

        public static bool KeybindsExist()
        {
            return File.Exists(_keybindPath);
        }

        public static bool LoadKeybinds(out KeybindConfig keybinds)
        {
            var succeeded = false;
            string raw = null;
            keybinds = null;

            if (File.Exists(_keybindPath))
                raw = File.ReadAllText(_keybindPath);
            else
                return false;

            keybinds = JsonConvert.DeserializeObject<KeybindConfig>(raw);
            succeeded = true;

            return succeeded;
        }

        public static bool CreateKeybinds(out KeybindConfig keybinds)
        {
            var success = false;

            try
            {
                keybinds = new KeybindConfig();
                keybinds.AddBind("Main", KeyCode.F1);
                keybinds.AddBind("Keybinds", KeyCode.F2);

                //TODO: add other keybinds

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
            File.WriteAllText(_keybindPath, json);
        }


    }
}
