using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TsunamiHack.Tsunami.Types;
using Newtonsoft.Json;
using TsunamiHack.Tsunami.Types.Lists;

namespace TsunamiHack.Tsunami.Util
{
    internal static  class WebAccess
    {
        private const string PremlistUrl = "https://pastebin.com/raw/HU0DXR3s";       //"https://pastebin.com/raw/KBRtvsdz"; 
        private const string BanListUrl = "https://pastebin.com/raw/vRq5a5FR";        //"https://pastebin.com/raw/AxXtzUVL";
        private const string BetaListUrl = "https://pastebin.com/raw/isaAstAt";       //"https://pastebin.com/raw/849dzxjn";
        private const string ControllerInfoUrl = "https://pastebin.com/raw/f8NyLp37"; //"https://pastebin.com/raw/v3VCgGCE";
        private const string EulaUrl = "https://pastebin.com/raw/c0FD93MP";

        private static bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) => true;
                
        public static void DownloadAll(out PremiumList premiumList, out BanList banList, out BetaList betaList)
        {
            ServicePointManager.ServerCertificateValidationCallback = Validator;
            var web = new WebClient();
            
            premiumList = JsonConvert.DeserializeObject<PremiumList>(web.DownloadString(PremlistUrl));
            banList = JsonConvert.DeserializeObject<BanList>(web.DownloadString(BanListUrl));
            betaList = JsonConvert.DeserializeObject<BetaList>(web.DownloadString(BetaListUrl));
        }

        public static void DownloadInfo(out HackController ctrl)
        {
            ServicePointManager.ServerCertificateValidationCallback = Validator;
            ctrl = JsonConvert.DeserializeObject<HackController>(new WebClient().DownloadString(ControllerInfoUrl));
        }

        public static void DownloadEula(out string eula)
        {
            ServicePointManager.ServerCertificateValidationCallback = Validator;
            eula = new WebClient().DownloadString(EulaUrl);
        }
    }
}
