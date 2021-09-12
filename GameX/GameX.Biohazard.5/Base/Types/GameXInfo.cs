using System;
using System.Drawing;

namespace GameX.Base.Types
{
    public class GameXInfo
    {
        public string GameXName { get; set; }
        public string[] GameXLogo { get; set; }
        public Color[] GameXLogoColors { get; set; }
        public string GameXFile { get; set; }
        public string RepositoryRoute { get; set; }
        public bool Downloaded { get; set; }
        public bool Updated { get; set; }
        public Image Logo { get; set; }
        public Version Current { get; set; }

        public override string ToString()
        {
            return GameXName;
        }
    }
}