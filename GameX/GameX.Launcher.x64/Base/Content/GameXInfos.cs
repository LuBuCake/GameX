using System.Drawing;
using GameX.Launcher.Base.Types;

namespace GameX.Launcher.Base.Content
{
    public class GameXInfos
    {
        public static GameXInfo[] Available()
        {
            GameXInfo Biohazard_2 = new GameXInfo()
            {
                GameXName = "Resident Evil 2 Remake",
                GameXFile = "GameX.Biohazard.2.dll",
                GameXLogo = new[] { "logo_a", "logo_b" },
                GameXLogoColors = new[] { Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Biohazard.2/"
            };

            GameXInfo Biohazard_3 = new GameXInfo()
            {
                GameXName = "Resident Evil 3 Remake",
                GameXFile = "GameX.Biohazard.3.dll",
                GameXLogo = new[] { "logo_a", "logo_b" },
                GameXLogoColors = new[] { Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Biohazard.3/"
            };

            GameXInfo Biohazard_7 = new GameXInfo()
            {
                GameXName = "Resident Evil 7",
                GameXFile = "GameX.Biohazard.7.dll",
                GameXLogo = new[] { "logo_a", "logo_b" },
                GameXLogoColors = new[] { Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Biohazard.7/"
            };

            GameXInfo Biohazard_8 = new GameXInfo()
            {
                GameXName = "Resident Evil Village",
                GameXFile = "GameX.Biohazard.Village.dll",
                GameXLogo = new[] { "logo_a", "logo_b" },
                GameXLogoColors = new[] { Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Biohazard.Village/"
            };

            return null;
        }
    }
}
