using System;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types
{
    [Serializable]
    public class TsuColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public TsuColor(Color value)
        {
            r = value.r;
            g = value.g;
            b = value.b;
            a = value.a;
        }

        public TsuColor(float R, float G, float B, float A)
        {
            r = R;
            g = G;
            b = B;
            a = A;
        }
        
        public Color Convert()
        {
            return new Color(r,g,b,a);
        }
    }
}