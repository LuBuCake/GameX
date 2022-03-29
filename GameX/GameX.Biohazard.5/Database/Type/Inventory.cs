using GameX.Enum;

namespace GameX.Database.Type
{
    public class Inventory
    {
        private int PlayerIndex { get; set; }
        public Slot[] LoadoutSlots { get; set; }
        public Slot[] RealTimeSlots { get; set; }

        public Inventory(int PlayerIndex, App GUI)
        {
            this.PlayerIndex = PlayerIndex;
            LoadoutSlots = new Slot[10];
            RealTimeSlots = new Slot[10];

            for (int i = 0; i < 10; i++)
            {
                LoadoutSlots[i] = new Slot(SlotType.Loadout, GUI, PlayerIndex, i);
                RealTimeSlots[i] = new Slot(SlotType.RealTime, GUI, PlayerIndex, i);
            }
        }

        public override string ToString()
        {
            return $"Player {PlayerIndex + 1} - Inventory";
        }
    }
}
