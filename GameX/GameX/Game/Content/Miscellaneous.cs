using GameX.Base.Types;

namespace GameX.Game.Content
{
    public class Miscellaneous
    {
        public static ListItem[] Handness()
        {
            return new ListItem[]
            {
                new ListItem("Default"),
                new ListItem("R-Handed", 0),
                new ListItem("L-Handed", 1)
            };
        }

        public static ListItem[] WeaponMode()
        {
            return new ListItem[]
            {
                new ListItem("Default"),
                new ListItem("Male", 0),
                new ListItem("Female", 1)
            };
        }
    }
}
