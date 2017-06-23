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
                LoadKeybinds();
            }   
        }

        public static void LoadDownloads()
        {
            
        }

    }
}
