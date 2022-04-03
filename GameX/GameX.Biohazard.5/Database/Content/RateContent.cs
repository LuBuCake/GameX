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
                new Simple("5 times per sec", 200),
                new Simple("10 times per sec", 100),
                new Simple("20 times per sec", 50)
            };
        }
    }
}
