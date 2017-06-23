using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    class MenuTools
    {
        public enum Horizontal
        {
            Left,
            LeftMid,
            Center,
            RightMid,
            Right
        };

        public enum Vertical
        {
            Top,
            TopMid,
            Center,
            BottomMid,
            Bottom
        }

        public static Rect getRectAtLoc(Vector2 menuSize, Horizontal hori, Vertical vert, bool usePadding, float padding = 0f)
        {
            var outputRect = new Rect();
            int width = (int) menuSize.x;
            int height = (int) menuSize.y;

            int xpos;
            int ypos;

            switch (hori)
            {
                case Horizontal.Left:
                    xpos = 0;
                    if (usePadding)
                    {
                        xpos += (int) padding;
                    }
                    break;
                case Horizontal.LeftMid:
                    xpos = (((Screen.width / 2) / 2) - (width / 2));
                    break;
                case Horizontal.Center:
                    xpos = ((Screen.width / 2) - (width / 2));
                    break;
                case Horizontal.RightMid:
                    xpos = (((Screen.width / 2) + (Screen.width / 4)) - (width / 2));
                    break;
                case Horizontal.Right:
                    xpos = Screen.width - width;
                    if (usePadding)
                    {
                        xpos -= (int) padding;
                    }
                    break;
                default:
                    goto case Horizontal.Center;
            }

            switch (vert)
            {
                case Vertical.Top:
                    ypos = 0;
                    if (usePadding)
                    {
                        ypos += (int) padding;
                    }
                    break;
                case Vertical.TopMid:
                    ypos = ((Screen.height / 2) / 2) - (height / 2);
                    break;
                case Vertical.Center:
                    ypos = ((Screen.height / 2) - (height / 2));
                    break;
                case Vertical.BottomMid:
                    ypos = ((Screen.height / 2) + (Screen.height / 4) - (height / 2));
                    break;
                case Vertical.Bottom:
                    ypos = Screen.height - height;
                    if (usePadding)
                    {
                        ypos -= (int) padding;
                    }
                    break;
                default:
                    goto case Vertical.Center;
            }


            if (ypos + height > Screen.height)
            {
                int difference = Screen.height - (ypos + height);
                ypos -= difference;

                if (usePadding)
                {
                    ypos -= (int) padding;
                }
            }
            else if (ypos < 0)
            {
                ypos = 0;
                if (usePadding)
                {
                    ypos += (int) padding;
                }
            }
            else if (xpos + width > Screen.width)
            {
                int difference = Screen.width - (xpos + width);
                ypos -= difference;

                if (usePadding)
                {
                    xpos -= (int) padding;
                }
            }
            else if (xpos < 0)
            {
                xpos = 0;

                if (usePadding)
                {
                    xpos += (int) padding;
                }
            }

            outputRect = new Rect(xpos, ypos, width, height);

            return outputRect;
        }
    }
}
