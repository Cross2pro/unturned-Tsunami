using UnityEngine;

namespace TsunamiHack.Tsunami.Manager
{
    internal class Quake : MonoBehaviour
    {
        private WaveMaker WM = new WaveMaker();

        public void Start()
        {
            //Add all hack components
            //Call loader

            Loader.LoadAll();
            WM.Start();
        }

        public void Update()
        {
            //Call Updates

            WM.OnUpdate();
        }
    }
}
