﻿using System.Drawing;
using GameX.Launcher.Base.Types;

namespace GameX.Launcher.Base.Content
{
    public class GameXInfos
    {
        public static GameXInfo[] Available()
        {
            GameXInfo Biohazard_5 = new GameXInfo()
            {
                GameXName = "Resident Evil 5", 
                GameXFile = "GameX.Biohazard.5.dll",
                GameXLogo = new[] { "logo_a", "logo_b" },
                GameXLogoColors = new [] {Color.DarkOrange, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX.Versioning/main/GameX.Biohazard.5/"
            };

            GameXInfo Biohazard_6 = new GameXInfo()
            {
                GameXName = "Resident Evil 6",
                GameXFile = "GameX.Biohazard.6.dll",
                GameXLogo = new[] { "logo_a", "logo_b" },
                GameXLogoColors = new[] { Color.FromArgb(0,120,255), Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX.Versioning/main/GameX.Biohazard.6/"
            };

            GameXInfo Biohazard_Evelations_1 = new GameXInfo()
            {
                GameXName = "Resident Evil Revelations",
                GameXFile = "GameX.Biohazard.Rev.1.dll",
                GameXLogo = new[] { "logo_a", "logo_b" },
                GameXLogoColors = new[] { Color.RoyalBlue, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX.Versioning/main/GameX.Biohazard.Revelations.1/"
            };

            GameXInfo Biohazard_Evelations_2 = new GameXInfo()
            {
                GameXName = "Resident Evil Revelations 2",
                GameXFile = "GameX.Biohazard.Rev.2.dll",
                GameXLogo = new[] { "logo_a", "logo_b" },
                GameXLogoColors = new[] { Color.Red, Color.White },
                RepositoryRoute = "https://raw.githubusercontent.com/LuBuCake/GameX.Versioning/main/GameX.Biohazard.Revelations.2/"
            };

            return new[]
            {
                Biohazard_5
            };
        }
    }
}