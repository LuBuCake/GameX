using GameX.Types;

namespace GameX.Content
{
    public class Indexes
    {
        public static ListItem[] Available()
        {
            ListItem P1 = new ListItem("Player 1", 0);
            ListItem P2 = new ListItem("Player 2", 1);
            ListItem P3 = new ListItem("Player 3", 2);
            ListItem P4 = new ListItem("Player 4", 3);

            return new ListItem[]
            {
                P1, P2, P3, P4
            };
        }
    }
}
