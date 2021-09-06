using GameX.Base.Types;

namespace GameX.Base.Content
{
    public class Interfaces
    {
        public static ListItem[] Available()
        {
            ListItem Console = new ListItem("Console", 0);

            return new ListItem[]
            {
                Console
            };
        }
    }
}