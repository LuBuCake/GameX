using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameX.Game.Helpers;

namespace GameX.Game.Types
{
    public class Move
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public MoveType Type { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
