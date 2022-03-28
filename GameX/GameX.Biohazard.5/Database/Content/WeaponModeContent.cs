using System.Collections.Generic;
using GameX.Database.Type;

namespace GameX.Database.Content
{
    public static class WeaponModeContent
    {
        public static List<Simple> GetCollection()
        {
            return new List<Simple>()
            {
                new Simple("Default"),
                new Simple("Male", 0),
                new Simple("Female", 1)
            };
        }
    }
}
