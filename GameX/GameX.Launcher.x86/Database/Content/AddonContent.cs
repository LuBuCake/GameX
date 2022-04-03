using System.Collections.Generic;
using System.Drawing;
using GameX.Launcher.Database.Type;

namespace GameX.Launcher.Database.Content
{
    public class AddonContent
    {
        public static List<Addon> GetCollection()
        {
            Addon Biohazard_5 = new Addon()
            {
                Name = "Resident Evil 5", 
                File = "GameX.Biohazard.5.dll",
                Images = new[] { "logo_a.png", "logo_b.png" },
                ImageColors = new [] {Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Biohazard.5/"
            };

            Addon Biohazard_6 = new Addon()
            {
                Name = "Resident Evil 6",
                File = "GameX.Biohazard.6.dll",
                Images = new[] { "logo_a.png", "logo_b.png" },
                ImageColors = new[] { Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Biohazard.6/"
            };

            Addon Biohazard_Evelations_1 = new Addon()
            {
                Name = "Resident Evil Revelations",
                File = "GameX.Biohazard.Rev.1.dll",
                Images = new[] { "logo_a.png", "logo_b.png" },
                ImageColors = new[] { Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Biohazard.Revelations.1/"
            };

            Addon Biohazard_Evelations_2 = new Addon()
            {
                Name = "Resident Evil Revelations 2",
                File = "GameX.Biohazard.Rev.2.dll",
                Images = new[] { "logo_a.png", "logo_b.png" },
                ImageColors = new[] { Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Biohazard.Revelations.2/"
            };

            return new List<Addon>()
            {
                Biohazard_5
            };
        }
    }
}
