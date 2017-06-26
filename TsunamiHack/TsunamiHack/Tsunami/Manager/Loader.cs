using TsunamiHack.Tsunami.Types;

namespace TsunamiHack.Tsunami.Manager
{
    class Loader
    {

        public static void LoadAll()
        {
            try
            {
                LoadConfig();
                LoadKeybinds();
                LoadDownloads();
            }
            catch (UnableToLoadException e)
            {

            }
        }

        public static void LoadConfig()
        {
            
        }

        public static void LoadKeybinds()
        {
            if (Util.FileIO.KeybindsExist())
            {
                if (Util.FileIO.LoadKeybinds(out WaveMaker.Keybinds))
                {
                    Util.Logging.LogMsg("Internal Error", "Unable to load keybinds file");
                    throw new UnableToLoadException("Unable to load keybinds file");
                }
            }
            else
            {
                if (!Util.FileIO.CreateKeybinds(out WaveMaker.Keybinds) || WaveMaker.Keybinds == null)
                {
                    Util.Logging.LogMsg("Internal Error", "Unable to create keybinds file");
                    throw new UnableToLoadException("Unable to create keybinds file");
                }
            }
        }

        public static void LoadDownloads()
        {
            //TODO: add method calls for downloading lists
        }

    }
}
