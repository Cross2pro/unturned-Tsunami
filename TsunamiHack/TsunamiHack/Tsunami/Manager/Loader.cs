using SDG.Unturned;
using TsunamiHack.Tsunami.Types;
using TsunamiHack.Tsunami.Types.Lists;
using TsunamiHack.Tsunami.Util;

namespace TsunamiHack.Tsunami.Manager
{
    class Loader
    {
        
        public static void LoadAll()
        {
            try
            {
                Util.FileIo.CheckDirectory();

                WaveMaker.FirstTime = Util.FileIo.CheckIfFirstTime();

                LoadConfigs();
                LoadKeybinds();
                LoadDownloads();
            }
            catch (UnableToLoadException e)
            {
                Util.Logging.Exception(e);
                WaveMaker.HackDisabled = true;
            }
        }

        private static void LoadConfigs()
        {
            // --- Friends ---

            if (Util.FileIo.FriendsExist())
            {
                Util.FileIo.LoadFriends(out WaveMaker.Friends);

                if (WaveMaker.Friends == null)
                {
                    Util.Logging.LogMsg("Internal Error", "Unable to load friends file");
                    throw new UnableToLoadException("Unable to load friends file");
                }
            }
            else 
            {
                Util.FileIo.CreateFriends( out WaveMaker.Friends );

                if (WaveMaker.Friends == null)
                {
                    Util.Logging.LogMsg( "Internal Error", "Unable to create friends file" );
                    throw new UnableToLoadException( "Unable to create friends file" );
                }
            }

            // --- Settings ---

            if (Util.FileIo.SettingsExist())
            {
                Util.FileIo.LoadSettings(out WaveMaker.Settings);

                if (WaveMaker.Settings == null)
                {
                    Util.Logging.LogMsg("Internal Error", "Unable to load settings file");
                    throw new UnableToLoadException("Unable to load settings file");
                }
            }
            else
            {
                Util.FileIo.CreateSettings(out WaveMaker.Settings);

                if (WaveMaker.Settings == null)
                {
                    Util.Logging.LogMsg("Internal Error", "Unable to create settings file");
                    throw new UnableToLoadException("Unable to create settings file");
                }
            }


        }

        private static void LoadKeybinds()
        {
            if (Util.FileIo.KeybindsExist())
            {
                Util.FileIo.LoadKeybinds(out WaveMaker.Keybinds);

                if (WaveMaker.Keybinds == null)
                {
                    Util.Logging.LogMsg("Internal Error", "Unable to load keybinds file");
                    throw new UnableToLoadException("Unable to load keybinds file");
                }
            }
            else
            {
                Util.FileIo.CreateKeybinds(out WaveMaker.Keybinds);

                if (WaveMaker.Keybinds == null)
                {
                    Util.Logging.LogMsg("Internal Error", "Unable to create keybinds file");
                    throw new UnableToLoadException("Unable to create keybinds file");
                }
            }
        }

        private static void LoadDownloads()
        {
            Util.FileDownloader.DownloadAll(out WaveMaker.Prem, out WaveMaker.Ban, out WaveMaker.Beta);
            Util.FileDownloader.DownloadInfo(out WaveMaker.Controller);
                   
        }

    }
}
