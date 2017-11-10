using System;
using SDG.Unturned;
using UnityEngine;
using System.Reflection;
// ReSharper disable PossibleNullReferenceException

namespace TsunamiHack.Tsunami.Manager
{
    public static class Hook
    {
        public static void Cast()
        {
            try
            {
                var hook = new GameObject();
                var instance = hook.AddComponent<Quake>();
                UnityEngine.Object.DontDestroyOnLoad(instance);
                Rebrand();
                
            }
            catch (Exception e)
            {
                Util.Logging.Exception(e);
            }
        }

        public static void Rebrand()
        {
            try
            {
                typeof(Provider).GetField("APP_NAME", BindingFlags.Public | BindingFlags.Static).SetValue(null, "TSUNAMI HACK BY TIDAL");
                typeof(Provider).GetField("APP_AUTHOR", BindingFlags.Public | BindingFlags.Static).SetValue(null, "Tidal");

//                typeof(Sleek).GetField("_backgroundColor", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(typeof(MenuTitleUI).GetField("authorLabel", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null), Color.cyan);


//                FieldInfo field =  typeof(MenuTitleUI).GetField("titleLabel", BindingFlags.NonPublic | BindingFlags.Static);
//                var titlelabel = new SleekLabel();
//                var title = typeof(MenuTitleUI).GetField("titleLabel", BindingFlags.NonPublic | BindingFlags.Static).GetValue(titlelabel);
//                var fieldinfo = typeof(MenuTitleUI).GetField("titleLabel", BindingFlags.NonPublic | BindingFlags.Static);
//                 
//                titlelabel.foregroundColor = Color.cyan;
//                titlelabel.backgroundColor = Color.cyan;
//                
//                fieldinfo.SetValue(fieldinfo.GetValue(new SleekLabel()), titlelabel );

            }
            catch (Exception e)
            {
                Util.Logging.Exception(e);
            }
            
        }
        
    }
}
