using System;
using GameX.Base.Modules;
using GameX.Game.Types;

namespace GameX.Game.Modules
{
    public class Inventory
    {
        private int _INDEX { get; }
        public InventoryItem[] Loadout { get; set; }
        public InventoryItem[] RealTime { get; set; }

        public Inventory(int Index)
        {
            _INDEX = Index;
            Loadout = new InventoryItem[10];
            RealTime = new InventoryItem[10];
        }


    }
}
