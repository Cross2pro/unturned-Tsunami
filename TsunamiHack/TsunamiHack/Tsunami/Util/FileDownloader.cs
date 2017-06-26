﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TsunamiHack.Tsunami.Types;
using Newtonsoft.Json;

namespace TsunamiHack.Tsunami.Util
{
    public class FileDownloader
    {
        private const string PremlistUrl = "https://pastebin.com/raw/KBRtvsdz";
        private const string BanListUrl = "https://pastebin.com/raw/AxXtzUVL";
        private const string BetaListUrl = "https://pastebin.com/raw/849dzxjn";
        private const string ControllerInfoUrl = "https://pastebin.com/raw/v3VCgGCE";

        public enum ListType { Premium, Ban, Beta}

        public static void DownloadList(out InfoList list, ListType type)
        {
            list = null;

            WebClient web = new WebClient();
            string raw = "";

            switch (type)
            {
                case ListType.Premium:
                    raw = web.DownloadString(PremlistUrl);
                    break;
                case ListType.Ban:
                    raw = web.DownloadString(BanListUrl);
                    break;
                case ListType.Beta:
                    raw = web.DownloadString(BetaListUrl);
                    break;
            }

            list = JsonConvert.DeserializeObject<InfoList>(raw);
        }

        public static void DownloadAll(out PremiumList premiumList, out BanList banList, out BetaList betaList ) 
        {
            WebClient web = new WebClient();

            var json = web.DownloadString(PremlistUrl);
            premiumList = JsonConvert.DeserializeObject<PremiumList>(json);

            json = web.DownloadString(BanListUrl);
            banList = JsonConvert.DeserializeObject<BanList>(json);

            json = web.DownloadString(BetaListUrl);
            betaList = JsonConvert.DeserializeObject<BetaList>(json);
        }

        public static void DownloadInfo(out HackController ctrl)
        {
            WebClient web = new WebClient();
            var raw = web.DownloadString(ControllerInfoUrl);
            ctrl = JsonConvert.DeserializeObject<HackController>(raw);
        }
    }
}
