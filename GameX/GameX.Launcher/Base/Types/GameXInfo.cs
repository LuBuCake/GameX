namespace GameX.Launcher.Base.Types
{
    public class GameXInfo
    {
        public string GameXName { get; set; }
        public string GameXLogo { get; set; }
        public string GameXFile { get; set; }

        public override string ToString()
        {
            return GameXName;
        }
    }
}