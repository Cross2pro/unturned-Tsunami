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
        private static string _path = Application.persistentDataPath;
        private static readonly string _keybindPath = _path + @"\Keybinds.dat";
        private static string _infoPath = _path + @"\Info.dat";

        public static bool KeybindsExist()
        {
            return File.Exists(_keybindPath);
        }

        public static bool LoadKeybinds()
        {
            return true;
        }

        public static bool CreateKeybinds()
        {
            
        }

    }
}
