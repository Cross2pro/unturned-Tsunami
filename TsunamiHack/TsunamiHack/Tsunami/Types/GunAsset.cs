﻿using SDG.Unturned;
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

        public float shakemaxx;
        public float shakemaxy;
        public float shakemaxz;
        public float shakeminx;
        public float shakeminy;
        public float shakeminz;

        public GunAsset(ItemGunAsset input)
        {
            recoilmaxx = input.recoilMax_x;
            recoilmaxy = input.recoilMax_y;
            recoilminx = input.recoilMin_x;
            recoilminy = input.recoilMin_y;

            spreadaim = input.spreadAim;
            spreadhip = input.spreadHip;

            shakemaxx = input.shakeMax_x;
            shakemaxy = input.shakeMax_y;
            shakemaxz = input.shakeMax_z;
            shakeminx = input.shakeMin_x;
            shakeminy = input.shakeMin_y;
            shakeminz = input.shakeMin_z;
            
            guid = input.GUID;
        }

    }
}