using SDG.Unturned;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types
{
    internal class GunAsset
    {
        public System.Guid guid;
        public float recoilminx;
        public float recoilminy;
        public float recoilmaxx;
        public float recoilmaxy;

        public float spreadaim;
        public float spreadhip;

        public GunAsset(ItemGunAsset input)
        {
            recoilmaxx = input.recoilMax_x;
            recoilmaxy = input.recoilMax_y;
            recoilminx = input.recoilMin_x;
            recoilminy = input.recoilMin_y;

            spreadaim = input.spreadAim;
            spreadhip = input.spreadHip;
            
            guid = input.GUID;
        }

    }
}