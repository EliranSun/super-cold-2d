namespace action_triggers.scripts
{
    public class PlayerInfo
    {
        public string Gender;
        public string Name;

        public PlayerInfo(string name, string gender = null)
        {
            Name = name;
            Gender = gender;
        }
    }
}