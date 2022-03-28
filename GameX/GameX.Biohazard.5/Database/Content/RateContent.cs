using System.Collections.Generic;
using GameX.Database.Type;

namespace GameX.Database.Content
{
    public static class RateContent
    {
        public static List<Simple> GetCollection()
        {
            return new List<Simple>()
            {
                new Simple("30 FPS", 30),
                new Simple("60 FPS", 60),
                new Simple("120 FPS", 120)
            };
        }
    }
}
