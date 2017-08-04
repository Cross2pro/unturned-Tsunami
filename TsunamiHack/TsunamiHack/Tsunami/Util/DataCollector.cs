using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace TsunamiHack.Tsunami.Util
{   
    internal class DataCollector
    {
        //TODO: Add a hwid creating system for banning
        
        private const string IpCheckUrl = "https://api.ipify.org/";

        public static string GetHWID()
        {
            return GetHash("CPU>>" + GetCpuId() + "Bios>>" + GetBiosId() + "Hdd>>" + GetHddId() + "Board>>" + GetBiosId());
        }
        
        public static string GetIp()
        {
            ServicePointManager.ServerCertificateValidationCallback = WebAccess.Validator;
            var web = new WebClient();
            var ip = web.DownloadString(IpCheckUrl);
            return ip;
        }

        public static string GetCpuId()
        {   
            var result = "";
            var mc = new System.Management.ManagementClass("Win32_Processor");
            var moc = mc.GetInstances();
            foreach (System.Management.ManagementBaseObject mo in moc)
            {
                if (result != "") continue;
                try
                {
                    result = mo["ProcessorId"].ToString();
                    break;
                }
                catch
                {
                }
            }
            return result;
        }

        public static string GetBiosId()
        {
            var idstr = "";
            
            var info = "";
            var mc = new ManagementClass("Win32_BIOS");
            var moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if (info == "")
                {
                    info = mo.Properties["Manufacturer"].Value.ToString();
                    break;
                }
            }

            idstr += info;

            info = "";
            mc = new ManagementClass("Win32_BIOS");
            moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if (info == "")
                {
                    info = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                }
            }

            idstr += info;

            info = "";
            mc = new ManagementClass("Win32_BIOS");
            moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if (info == "")
                {
                    info = mo.Properties["IdentificationCode"].Value.ToString();
                    break;
                }
            }

            idstr += info;
            return idstr;
        }

        public static string GetHddId()
        {
            var idstr = "";

            var info = "";
            var mc = new ManagementClass("Win32_DiskDrive");
            var moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if (info == "")
                {
                    info = mo.Properties["Model"].Value.ToString();
                    break;
                }
            }

            idstr += info;

            info = "";
            mc = new ManagementClass("Win32_DiskDrive");
            moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if (info == "")
                {
                    info = mo.Properties["Signature"].Value.ToString();
                    break;
                }
            }

            idstr += info;
            return idstr;
        }

        public static string GetBoardId()
        {
            var idstr = "";

            var info = "";
            var mc = new ManagementClass("Win32_BaseBoard");
            var moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if (info == "")
                {
                    info = mo.Properties["Model"].Value.ToString();
                    break;
                }
            }

            idstr += info;

            info = "";
            mc = new ManagementClass("Win32_BaseBoard");
            moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if (info == "")
                {
                    info = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                }
            }

            idstr += info;
            return idstr;
        }
        
        public static string GetHash(string s)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            byte[] bt = Encoding.ASCII.GetBytes(s);
            return GetHexString(sec.ComputeHash(bt));
        }
        
        public static string GetHexString(IList<byte> bt)
        {
            var s = string.Empty;
            for (var i = 0; i < bt.Count; i++)
            {
                byte b = bt[i];
                int n = b;
                int n1 = n & 15;
                int n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + 'A')).ToString();
                else
                    s += n2.ToString();
                if (n1 > 9)
                    s += ((char)(n1 - 10 + 'A')).ToString();
                else
                    s += n1.ToString();
                if ((i + 1) != bt.Count && (i + 1) % 2 == 0) s += "-";
            }
            return s;
        }
    }
}





















