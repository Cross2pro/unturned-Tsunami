using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsunamiHack.Tsunami.Types;

namespace TsunamiHack.Tsunami.Manager
{
    class Loader
    {

        static void LoadAdd()
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

        static void LoadConfig()
        {
            
        }

        static void LoadKeybinds()
        {
            
        }

        static void LoadDownloads()
        {
            
        }

    }
}
