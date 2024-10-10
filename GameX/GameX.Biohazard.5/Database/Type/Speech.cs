using GameX.Enum;
using System.Collections.Generic;

namespace GameX.Database.Type
{
    public class Speech
    {
        public CharacterEnum Character { get; set; }
        public List<Simple> Lines { get; set; }
    }
}
