using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Types
{
    public class FPSMode
    {
        public string Name { get; set; }
        public int FPS { get; set; }

        public FPSMode(string name, int fps)
        {
            Name = name;
            FPS = fps;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
