using System;
using UnityEngine;

namespace TsunamiHack.Tsunami.Types
{
    
    public class Popup
    {
        //TODO: Add moving rects
        //TODO: Add limited duration popups
        
        public Rect PopupRect;
        public bool PopupOpened;
        public bool IsMoving;
        public bool Initalized;

        public Vector2 StartingPosition;
        public Vector2 EndingPosition;
        public float MovingSpeed;
        public float Duration;

        public string PopupMessage;
        public string PopupTitle;
        public int Id;

        public bool CloseEnabled;

        public Popup()
        {
            PopupOpened = false;
            CloseEnabled = true;
        }

        public Popup(Rect windowRect, string title, string message, int id, bool IsMoving = false)
        {
            PopupOpened = false;
            PopupRect = windowRect;
            PopupTitle = title;
            PopupMessage = message;
            Id = id;
            this.IsMoving = IsMoving;
        }
        
        public void EnableMoving(Vector2 starting, Vector2 ending, float speed = 1f)
        {
            StartingPosition = starting;
            EndingPosition = ending;
            MovingSpeed = speed;
        }
        
        public void PopupFunct(int id)
        {           
            GUILayout.Label(PopupMessage);
            
            if (CloseEnabled)
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


    }
}