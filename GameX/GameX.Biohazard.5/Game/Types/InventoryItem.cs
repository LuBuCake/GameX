namespace GameX.Game.Types
{
    public class InventoryItem
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Firepower { get; set; }
        public int ReloadSpeed { get; set; }
        public int Capacity { get; set; }
        public int Critical { get; set; }
        public int Piercing { get; set; }
        public int Range { get; set; }
        public int Scope { get; set; }
        public bool InfiniteAmmo { get; set; }
        public bool RapidFire { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}