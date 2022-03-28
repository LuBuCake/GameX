using GameX.Enum;

namespace GameX.Database.Type
{
    public class Move
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public MoveTypeEnum Type { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
