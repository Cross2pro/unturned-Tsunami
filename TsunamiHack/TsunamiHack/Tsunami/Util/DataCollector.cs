using System.Net;
using System.Management;

namespace TsunamiHack.Tsunami.Util
{   
    internal class DataCollector
    {
        //TODO: Add a hwid creating system for banning
        
        private const string IpCheckUrl = "https://api.ipify.org/";
        
        public static string GetIp()
        {
            ServicePointManager.ServerCertificateValidationCallback = WebAccess.Validator;
            var web = new WebClient();
            var ip = web.DownloadString(IpCheckUrl);
            return ip;
        }

        public static string GetCpuId()
        {
            var info = "";
            var mc = new ManagementClass("win32_processor");
            var moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if (info == "")
                {
                    info = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return info;
        }
    }
}