﻿using System;
using System.Drawing;

namespace GameX.Launcher.Database.Type
{
    public class Addon
    {
        public string Name { get; set; }
        public string[] Images { get; set; }
        public Color[] ImageColors { get; set; }
        public string File { get; set; }
        public string RepositoryRoute { get; set; }
        public bool Downloaded { get; set; }
        public bool Updated { get; set; }
        public Image Logo { get; set; }
        public Version Current { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}