using System;
using GameX.Base.Modules;

namespace GameX.Game.Modules
{
    public class Inventory
    {
        private int _INDEX { get; set; }
        public int Mode { get; set; }

        public Inventory(int Index)
        {
            _INDEX = Index;
        }
    }
}
