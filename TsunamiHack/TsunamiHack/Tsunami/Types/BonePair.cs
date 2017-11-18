using UnityEngine;

namespace TsunamiHack.Tsunami.Types
{
    internal class BonePair
    {
        public Transform Item1;
        public Transform Item2;

        public BonePair(Transform one, Transform two)
        {
            Item1 = one;
            Item2 = two;
        }

        public static BonePair CreatePair(Transform one, Transform two)
        {
            return  new BonePair(one, two);
        }
    }
}