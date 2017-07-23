namespace TsunamiHack.Tsunami.Types
{
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
