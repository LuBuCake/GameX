using System.Collections.Generic;
using GameX.Database.Type;

namespace GameX.Database.Content
{
    public static class HandnessContent
    {
        public static List<Simple> GetCollection()
        {
            return new List<Simple>()
            {
                new Simple("Default"),
                new Simple("R-Handed", 0),
                new Simple("L-Handed", 1)
            };
        }
    }
}
