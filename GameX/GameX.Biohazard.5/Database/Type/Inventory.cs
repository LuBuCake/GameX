using GameX.Enum;

namespace GameX.Database.Type
{
    public class Inventory
    {
        private int PlayerIndex { get; set; }
        public Slot[] Slots { get; set; }

        public Inventory(int PlayerIndex, App GUI)
        {
            this.PlayerIndex = PlayerIndex;
            Slots = new Slot[10];

            for (int i = 0; i < 10; i++)
                Slots[i] = new Slot(GUI, PlayerIndex, i);
        }

        public override string ToString()
        {
            return $"Player {PlayerIndex + 1} - Inventory";
        }
    }
}
