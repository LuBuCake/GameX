namespace GameX.Game.Types
{
    public class Item
    {
        public int Value { get; set; }
        public int Group { get; set; }
        public int GroupIndex { get; set; }
        public string Name { get; set; }
        public string Portrait { get; set; }
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