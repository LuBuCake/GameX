using GameX.Base.Types;

namespace GameX.Base.Content
{
    public class Interfaces
    {
        public static ListItem[] Available()
        {
            ListItem Console = new ListItem("Console", 0);
            ListItem Server = new ListItem("Server", 1);
            ListItem Client = new ListItem("Client", 2);

            return new ListItem[]
            {
                Console, Server, Client
            };
        }
    }
}