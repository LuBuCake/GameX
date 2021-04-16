using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameX.Types;

namespace GameX.Content
{
    public class FPSModes
    {
        public static FPSMode[] Modes()
        {
            FPSMode Half = new FPSMode("30 FPS", 30);
            FPSMode Full = new FPSMode("60 FPS", 60);
            FPSMode Double = new FPSMode("120 FPS", 120);

            return new FPSMode[]
            {
                Half, Full, Double
            };
        }
    }
}
