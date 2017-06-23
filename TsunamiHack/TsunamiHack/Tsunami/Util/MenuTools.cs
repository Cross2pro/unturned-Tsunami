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
        public enum HorizontalLoc
        {
            Left,
            LeftMid,
            Center,
            RightMid,
            Right
        };

        public enum VerticalLoc
        {
            Top,
            TopMid,
            Center,
            BottomMid,
            Bottom
        }

        public static Rect getRectAtLoc(Vector2 menuSize, HorizontalLoc hori, VerticalLoc vert, bool usePadding, float padding = 0f)
        {
            var outputRect = new Rect();
            int width = (int) menuSize.x;
            int height = (int) menuSize.y;

            int xpos;
            int ypos;

            switch (hori)
            {
                case HorizontalLoc.Left:
                    xpos = 0;
                    if (usePadding)
                    {
                        xpos += (int) padding;
                    }
                    break;
                case HorizontalLoc.LeftMid:
                    xpos = (((Screen.width / 2) / 2) - (width / 2));
                    break;
                case HorizontalLoc.Center:
                    xpos = ((Screen.width / 2) - (width / 2));
                    break;
                case HorizontalLoc.RightMid:
                    xpos = (((Screen.width / 2) + (Screen.width / 4)) - (width / 2));
                    break;
                case HorizontalLoc.Right:
                    xpos = Screen.width - width;
                    if (usePadding)
                    {
                        xpos -= (int) padding;
                    }
                    break;
                default:
                    goto case HorizontalLoc.Center;
            }

            switch (vert)
            {
                case VerticalLoc.Top:
                    ypos = 0;
                    if (usePadding)
                    {
                        ypos += (int) padding;
                    }
                    break;
                case VerticalLoc.TopMid:
                    ypos = ((Screen.height / 2) / 2) - (height / 2);
                    break;
                case VerticalLoc.Center:
                    ypos = ((Screen.height / 2) - (height / 2));
                    break;
                case VerticalLoc.BottomMid:
                    ypos = ((Screen.height / 2) + (Screen.height / 4) - (height / 2));
                    break;
                case VerticalLoc.Bottom:
                    ypos = Screen.height - height;
                    if (usePadding)
                    {
                        ypos -= (int) padding;
                    }
                    break;
                default:
                    goto case VerticalLoc.Center;
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
