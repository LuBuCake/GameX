using System;

namespace GameX.Updater.Base.Types
{
    public class AppVersion
    {
        public Version Current { get; set; }
        public Version Latest { get; set; }
        public string VersionCheckRoute { get; set; }
        public string VersionFileRoute { get; set; }
    }
}
