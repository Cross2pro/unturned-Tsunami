using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TsunamiHack.Tsunami.Types;
using Newtonsoft.Json;
using SDG.Provider.Services;
using TsunamiHack.Tsunami.Manager;
using TsunamiHack.Tsunami.Types.Lists;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    internal static  class WebAccess
    {
        private const string PremlistUrl = "https://pastebin.com/raw/KBRtvsdz";
        private const string BanListUrl = "https://pastebin.com/raw/AxXtzUVL";
        private const string BetaListUrl = "https://pastebin.com/raw/849dzxjn";
        private const string ControllerInfoUrl = "https://pastebin.com/raw/v3VCgGCE";

        public enum ListType { Premium, Ban, Beta}

        public static bool Validator(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true;
        }

        [Obsolete]
        public static void DownloadList(out InfoList list, ListType type)
        {
            list = null;

            var web = new WebClient();
            var raw = "";

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
                
        public static void DownloadAll(out PremiumList premiumList, out BanList banList, out BetaList betaList)
        {
            ServicePointManager.ServerCertificateValidationCallback = Validator;
            var web = new WebClient();
            var json = "";

            json = web.DownloadString(PremlistUrl);
            premiumList = JsonConvert.DeserializeObject<PremiumList>(json);

            json = web.DownloadString(BanListUrl);
            banList = JsonConvert.DeserializeObject<BanList>(json);

            json = web.DownloadString(BetaListUrl);
            betaList = JsonConvert.DeserializeObject<BetaList>(json);

        }

        public static void DownloadInfo(out HackController ctrl)
        {
            ServicePointManager.ServerCertificateValidationCallback = Validator;
            var web = new WebClient();
            var raw = web.DownloadString(ControllerInfoUrl);
            ctrl = JsonConvert.DeserializeObject<HackController>(raw);
        }
    }
}
