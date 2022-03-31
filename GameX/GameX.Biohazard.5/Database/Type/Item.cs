using GameX.Enum;

namespace GameX.Database.Type
{
    public class Item
    {
        public int ID { get; set; }
        public ItemGroupEnum Group { get; set; }
        public int GroupIndex { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int[] Firepower { get; set; }
        public double[] ReloadSpeed { get; set; }
        public int[] Capacity { get; set; }
        public int[] Critical { get; set; }
        public int[] Piercing { get; set; }
        public int[] Range { get; set; }
        public int[] Scope { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
