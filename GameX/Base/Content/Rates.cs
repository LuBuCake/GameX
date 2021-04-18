using GameX.Base.Types;

namespace GameX.Base.Content
{
    public class Rates
    {
        public static ListItem[] Available()
        {
            ListItem Half = new ListItem("30 FPS", 30);
            ListItem Full = new ListItem("60 FPS", 60);
            ListItem Double = new ListItem("120 FPS", 120);

            return new ListItem[]
            {
                Half, Full, Double
            };
        }
    }
}
