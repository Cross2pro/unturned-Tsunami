﻿using System;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types
{
    
    internal class Popup
    {


        public Rect PopupRect;
        public int Id;
        public string PopupTitle;
        public string PopupMessage;
        public bool CloseAble;

        public bool PopupOpened;
        public bool InUse;

        public bool IsMoving;
        public Vector2 StartingPosition;
        public Vector2 EndingPosition;
        public float MovingSpeed;
        public float MovingDuration;

        public bool LimiterEnabled;
        public float Duration;
        public DateTime StartTime;
        public TimeSpan Difference;
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


        [Obsolete]
        public void EnableMoving(Vector2 starting, Vector2 ending, float speed = 1f)
        {
            if (PopupRect == null)
                throw new TypeNotInitalizedException("Initalize Popup before making dynamic");

            StartingPosition = starting;
            EndingPosition = ending;
            MovingSpeed = speed;
        }

        public void EnableMoving(Vector2 start, Vector2 end, int duration)
        {
            if(PopupRect == null)
                throw new TypeNotInitalizedException("Init popup before making dynamic");
            
            if (start == end) return;
            
            IsMoving = true;
            StartingPosition = start;
            EndingPosition = end;
            MovingDuration = duration;

            var dist = Vector2.Distance(start, end);
            MovingSpeed = dist / duration;
            MovingSpeed = MovingSpeed > 50 ? 50 : MovingSpeed;
        }

        public void EnableLimited(float duration)
        {
            LimiterEnabled = true;
            Duration = duration;
            StartTime = DateTime.Now;
            EndTime = StartTime.AddSeconds(duration);
            Difference = EndTime - StartTime; 
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
        public Popup(Rect windowRect, string title, string message, int id, bool isMoving = false)
        {
            PopupOpened = false;
            PopupRect = windowRect;
            PopupTitle = title;
            PopupMessage = message;
            Id = id;
            IsMoving = isMoving;
        }
    }
}