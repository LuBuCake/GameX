using System.Collections.Generic;
using GameX.Database.Type;

namespace GameX.Database.Content
{
    public static class MeleeKillSecondsContent
    {
        public static List<Simple> GetCollection()
        {
            return new List<Simple>()
            {
                new Simple("M. Kill +5s", 5),
                new Simple("M. Kill +7s", 7),
                new Simple("M. Kill +10s", 10),
                new Simple("M. Kill +13s", 13),
                new Simple("M. Kill +15s", 15),
                new Simple("M. Kill +17s", 17),
                new Simple("M. Kill +20s", 20),
            };
        }
    }

    public static class ComboTimerSecondsContent
    {
        public static List<Simple> GetCollection()
        {
            return new List<Simple>()
            {
                new Simple("C. Timer 10s", 10),
                new Simple("C. Timer 15s", 15),
                new Simple("C. Timer 20s", 20),
                new Simple("C. Timer 25s", 25),
                new Simple("C. Timer 30s", 30),
                new Simple("C. Timer 35s", 35),
                new Simple("C. Timer 40s", 40),
                new Simple("C. Timer 45s", 45),
                new Simple("C. Timer 50s", 50),
                new Simple("C. Timer 55s", 55),
                new Simple("C. Timer 60s", 60)
            };
        }
    }

    public static class ComboBonusTimerSecondsContent
    {
        public static List<Simple> GetCollection()
        {
            return new List<Simple>()
            {
                new Simple("B. Timer 30s", 30),
                new Simple("B. Timer 40s", 40),
                new Simple("B. Timer 50s", 50),
                new Simple("B. Timer 60s", 60),
                new Simple("B. Timer 70s", 70),
                new Simple("B. Timer 80s", 80),
                new Simple("B. Timer 90s", 90),
            };
        }
    }
}
