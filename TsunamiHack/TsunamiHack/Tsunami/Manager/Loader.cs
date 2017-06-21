using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    static class Loader
    {
        public static void LoadAll()
        {
            LoadConfig();
            DownloadInfo();
            Debug.Log("Loaded All Items");
        }

        public static void LoadConfig()
        {
            
        }

        public static void DownloadInfo()
        {
            var premList = (PremiumList) DownloadableInfo.DownloadList(DownloadableInfo.InfoType.PremiumList, "https://pastebin.com/raw/KBRtvsdz");
        }

        
    }
}
