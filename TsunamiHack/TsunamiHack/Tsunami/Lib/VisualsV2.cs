using UnityEngine;
using System.Collections;
using TsunamiHack.Tsunami.Menu;
using Object = UnityEngine.Object;

namespace TsunamiHack.Tsunami.Lib
{
    internal class VisualsV2 : MonoBehaviour
    {
        public static Visuals Menu;  
        
        public void Start(Visuals parent)
        {
            Menu = parent;
        }

        public void OnUpdate()
        {
            if (Menu.EnableEsp)
            {

            }
            else
            {
                StartCoroutine("ScrubEsp");
            }
        }
       

    }
}