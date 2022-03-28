using System.Collections.Generic;
using GameX.Database.Type;

namespace GameX.Database.Content
{
    public static class WeaponPlacementContent
    {
        public static List<Simple> GetCollection()
        {
            return new List<Simple>()
            {
                new Simple("Default", 0),
                new Simple("Removed", 1),
                new Simple("Diagonal", 2)
            };
        }
    }
}
