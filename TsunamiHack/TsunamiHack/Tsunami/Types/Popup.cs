using System;
using UnityEngine;
using TsunamiHack.Tsunami;

namespace TsunamiHack.Tsunami.Types
{
    
    public class Popup
    {
        //TODO: Add moving rects
        //TODO: Add limited duration popups


        [Identifier ("Generic popup vars")]
        public Rect PopupRect;
        public int Id;
        public string PopupTitle;
        public string PopupMessage;
        public bool CloseAble;

        [Identifier("Controller vars")]
        public bool PopupOpened;
        public bool InUse;

        [Identifier ("Dynamic vars")]
        public bool IsMoving;
        public Vector2 StartingPosition;
        public Vector2 EndingPosition;
        public float MovingSpeed;
        public float MovingDuration;

        [Identifier("Limited duration vars")]
        public bool LimiterEnabled;
        public float Duration;
        public DateTime StartTime;
        public DateTime Difference;
        public DateTime EndTime;

        public Popup(Rect windowRect, int id, string title, string message, bool closeable = true)
        {
            PopupOpened = false;
            InUse = false;

            PopupRect = windowRect;
            Id = id;
            PopupTitle = title;
            PopupMessage = message;
            CloseAble = closeable;
        }


        //TODO: refactor this constructor
        public void EnableMoving(Vector2 starting, Vector2 ending, float speed = 1f)
        {
            if (PopupRect == null)
                throw new TypeNotInitalizedException("Initalize Popup before making dynamic");

            StartingPosition = starting;
            EndingPosition = ending;
            MovingSpeed = speed;
        }
        
        public void PopupFunct(int id)
        {           
            GUILayout.Label(PopupMessage);
            
            if (CloseAble)
            {    
                if (GUILayout.Button("Close"))
                {
                    PopupOpened = false;
                }
            }
        }

        public Vector2 GetPopupPosition()
        {
            return new Vector2(PopupRect.x, PopupRect.y);
        }


        [Obsolete]
        public Popup(Rect windowRect, string title, string message, int id, bool IsMoving = false)
        {
            PopupOpened = false;
            PopupRect = windowRect;
            PopupTitle = title;
            PopupMessage = message;
            Id = id;
            this.IsMoving = IsMoving;
        }
    }
}