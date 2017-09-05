using UnityEngine;

namespace TsunamiHack.Tsunami.Util
{
    internal class MenuTools
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

        /// <summary>
        /// Uses supplied values to generate a rectangle for menus
        /// </summary>
        /// <param name="menuSize"> The width and height of the rectangle</param>
        /// <param name="hori"> The horizontal placement</param>
        /// <param name="vert"> The vertical placement</param>
        /// <param name="usePadding"> Use padding or not</param>
        /// <param name="padding"> optional padding ammount</param>
        /// <returns> Returns rectangle based on input values</returns>
        
        public static Rect GetRectAtLoc(Vector2 menuSize, Horizontal hori, Vertical vert, bool usePadding, float padding = 0f)
        {
            // ReSharper disable once RedundantAssignment
            var rect = new Rect();
            var width = (int) menuSize.x;
            var height = (int) menuSize.y;

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
                var difference = Screen.height - (ypos + height);
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
                var difference = Screen.width - (xpos + width);
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

            rect = new Rect(xpos, ypos, width, height);

            return rect;
        }
    }
}
