using TsunamiHack.Tsunami.Manager;

namespace TsunamiHack.Tsunami.Types
{
    internal class HackController
    {
        public bool Disabled;
        public string Reason;
        public string AuthorizedBy;
        public string Version;
        public ulong Dev;

        public void BanOverride(string reason)
        {
            WaveMaker.HackDisabled = true;
            Disabled = true;
            Reason = reason;
            AuthorizedBy = "AutoBan";
        }
        
    }
}
