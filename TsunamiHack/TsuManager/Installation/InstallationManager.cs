using System.Deployment.Internal;

namespace TsuManager.Installation
{
    internal class InstallationManager
    {
        internal MainWindow MWindow;

        internal InstallSettings Settings;
        internal InstallEula Eula;
        internal InstallStatus Status;
        
        public InstallationManager(MainWindow parent)
        {
            MWindow = parent;
            
            Settings = new InstallSettings(this, parent);
            Eula = new InstallEula(this, parent);
            Status = new InstallStatus(this, parent);
            
        }
        
        
        
    }
}