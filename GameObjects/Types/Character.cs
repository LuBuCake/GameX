using System.Collections.Generic;

namespace GameObjects.Types
{
    public class Costume
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Character
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }

        public List<Costume> Costumes { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
