using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Office.Drawing;

namespace GameX.Game.Types
{
    public class Item
    {
        // Base
        public int Value { get; set; }
        public int Group { get; set; }
        public int GroupIndex { get; set; }
        public string Name { get; set; }
        public string Portrait { get; set; }

        // Variables
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }

        // Upgrade Levels
        public int[] Firepower { get; set; }
        public double[] ReloadSpeed { get; set; }
        public int[] Capacity { get; set; }
        public int[] Critical { get; set; }
        public int[] Piercing { get; set; }
        public int[] Range { get; set; }
        public int[] Scope { get; set; }

        // Flags
        public bool InfiniteAmmo { get; set; }
        public bool RapidFire { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
