﻿using System;

namespace TsunamiHack.Tsunami.Types
{
    [Serializable]
    public class Setting
    {
        public string Name;
        public object Value;

        public Setting(string name, object value)
        {
            Name = name;
            Value = value;
        }

    }
}
