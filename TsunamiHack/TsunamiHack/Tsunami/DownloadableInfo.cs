using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TsunamiHack.Tsunami
{
    static class DownloadableInfo
    {
        public enum InfoType { PremiumList = 1, BanList = 2, BetaList = 3, HackInfo = 4 }

        public static InfoList DownloadList(InfoType t, string url)
        {
            WebClient client = new WebClient();
            var list = client.DownloadString(url);

            /*switch (t)
            {
                case InfoType.PremiumList:
                    return JsonConvert.DeserializeObject<PremiumList>(list);
                case InfoType.BanList:
                    return JsonConvert.DeserializeObject<BanList>(list);
                case InfoType.BetaList:
                    return JsonConvert.DeserializeObject<BetaList>(list);
                case InfoType.HackInfo:
                    throw new ArgumentException("HackInfo Does not return List!");
                default:
                    return null;
            }*/

            return null;
        }
    }
}
