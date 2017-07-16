using System.Diagnostics;
using TsunamiHack.Tsunami.Manager;

namespace TsunamiHack.Tsunami.Types
{
    public class HackController
    {
        public bool Disabled;
        public string Reason;
        public string AuthorizedBy;

        public void BanOverride(string reason)
        {
            WaveMaker.HackDisabled = true;
            Disabled = true;
            Reason = reason;
            AuthorizedBy = "AutoBan";
        }
        
    }
}
