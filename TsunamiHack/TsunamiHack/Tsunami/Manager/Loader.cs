using SDG.Unturned;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Util;

namespace TsunamiHack.Tsunami.Manager
{
    internal class Loader
    {
        //TODO:Check what happens when offline
        
        
        public static void LoadAll()
        {
            try
            {
                FileIo.CheckDirectory();
                FileIo.CheckEmpty();
                
                LoadDownloads();

                WaveMaker.FirstTime = FileIo.CheckIfFirstTime();

                LoadConfigs();
                LoadKeybinds();
            }
            catch (UnableToLoadException e)
            {
                Logging.Exception(e);
                WaveMaker.HackDisabled = true;
            }
        }

        private static void LoadConfigs()
        {
            // --- Friends ---

            if (FileIo.FriendsExist())
            {
                FileIo.StreamLoadFriends(out WaveMaker.Friends);

                if (WaveMaker.Friends == null)
                {
                    Logging.LogMsg("Internal Error", "Unable to load friends file");
                    throw new UnableToLoadException("Unable to load friends file");
                }
            }
            else 
            {
                FileIo.CreateFriends( out WaveMaker.Friends );

                if (WaveMaker.Friends == null)
                {
                    Logging.LogMsg( "Internal Error", "Unable to create friends file" );
                    throw new UnableToLoadException( "Unable to create friends file" );
                }
            }
/**/
            // --- Settings ---

            if (FileIo.SettingsExist())
            {
                FileIo.StreamLoadSettings(out WaveMaker.Settings);

                if (WaveMaker.Settings == null)
                {
                    Logging.LogMsg("Internal Error", "Unable to load settings file");
                    throw new UnableToLoadException("Unable to load settings file");
                }
            }
            else
            {
                FileIo.CreateSettings(out WaveMaker.Settings);

                if (WaveMaker.Settings == null)
                {
                    Logging.LogMsg("Internal Error", "Unable to create settings file");
                    throw new UnableToLoadException("Unable to create settings file");
                }
            }


        }

        private static void LoadKeybinds()
        {
            if (FileIo.KeybindsExist())
            {
                FileIo.StreamLoadKeybinds(out WaveMaker.Keybinds);

                if (WaveMaker.Keybinds == null)
                {
                    Logging.LogMsg("Internal Error", "Unable to load keybinds file");
                    throw new UnableToLoadException("Unable to load keybinds file");
                }
            }
            else
            {
                FileIo.CreateKeybinds(out WaveMaker.Keybinds);

                if (WaveMaker.Keybinds == null)
                {
                    Logging.LogMsg("Internal Error", "Unable to create keybinds file");
                    throw new UnableToLoadException("Unable to create keybinds file");
                }
            }
        }

        private static void LoadDownloads()
        {
//            WebAccess.DownloadAll(out WaveMaker.Prem, out WaveMaker.Ban, out WaveMaker.Beta);
            Db.GetAll(out WaveMaker.Prem, out WaveMaker.Ban, out WaveMaker.Beta);
//            WebAccess.DownloadInfo(out WaveMaker.Controller);
            Db.GetController(out WaveMaker.Controller);
            
            WaveMaker.HackDisabled = WaveMaker.Controller.Disabled;
        }

    }
}
