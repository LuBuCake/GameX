using GameX.Game.Types;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameX.Game.Content
{
    public class Characters
    {
        public static List<Character> GetChars()
        {
            Character Chris = new Character
            {
                Name = "Chris",
                Value = 0,
                Costumes = new List<Costume>() {
                new Costume { Name = "BSAA", Value = 0 },
                new Costume { Name = "Melee Vest", Value = 1 },
                new Costume { Name = "Bulletproof Vest", Value = 2 },
                new Costume { Name = "Full Armor", Value = 3 },
                new Costume { Name = "Plain", Value = 4 },
                new Costume { Name = "Safari", Value = 5 },
                new Costume { Name = "STARS", Value = 6 },
                new Costume { Name = "Warrior", Value = 7 },
                new Costume { Name = "Lost in Nightmares", Value = 8 },
                new Costume { Name = "Heavy Metal", Value = 9 }
            }
            };

            Character Sheva = new Character
            {
                Name = "Sheva",
                Value = 1,
                Costumes = new List<Costume>() {
                new Costume { Name = "BSAA", Value = 0 },
                new Costume { Name = "Melee Vest", Value = 1 },
                new Costume { Name = "Bulletproof Vest", Value = 2 },
                new Costume { Name = "Full Armor", Value = 3 },
                new Costume { Name = "Plain", Value = 4 },
                new Costume { Name = "Amazon", Value = 5 },
                new Costume { Name = "Clubbin", Value = 6 },
                new Costume { Name = "Business", Value = 7 },
                new Costume { Name = "Fairy Tail", Value = 8 }
            }
            };

            Character Jill = new Character
            {
                Name = "Jill",
                Value = 2,
                Costumes = new List<Costume>() {
                new Costume { Name = "BSAA", Value = 0 },
                new Costume { Name = "Battlesuit", Value = 1 },
                new Costume { Name = "Desesperate Escape", Value = 2 },
                new Costume { Name = "Lost in Nightmares", Value = 3 }
            }
            };

            Character Wesker = new Character
            {
                Name = "Wesker",
                Value = 3,
                Costumes = new List<Costume>() {
                new Costume { Name = "Midnight", Value = 0 },
                new Costume { Name = "STARS", Value = 1 }
            }
            };

            Character Josh = new Character
            {
                Name = "Josh",
                Value = 4,
                Costumes = new List<Costume>() {
                new Costume { Name = "Story Mode", Value = 0 },
                new Costume { Name = "Reunion BSAA", Value = 1 }
            }
            };

            Character Excella = new Character
            {
                Name = "Excella",
                Value = 5,
                Costumes = new List<Costume>() {
                new Costume { Name = "Tricell", Value = 0 }
            }
            };

            Character Barry = new Character
            {
                Name = "Barry",
                Value = 6,
                Costumes = new List<Costume>() {
                new Costume { Name = "STARS", Value = 0 }
            }
            };

            Character Rebecca = new Character
            {
                Name = "Rebecca",
                Value = 7,
                Costumes = new List<Costume>() {
                new Costume { Name = "STARS", Value = 0 }
            }
            };

            Character Irving = new Character
            {
                Name = "Irving",
                Value = 134,
                Costumes = new List<Costume>() {
                new Costume { Name = "Merchant of Death", Value = 0 }
            }
            };

            return new List<Character>()
            {
                Chris,
                Sheva,
                Jill,
                Wesker,
                Josh,
                Excella,
                Barry,
                Rebecca,
                Irving
            };
        }

        public static Image GetCharacterPortrait(string CharCos)
        {
            try
            {
                Dictionary<string, string> ImageFile = new Dictionary<string, string>();
                ImageFile.Add("Chris BSAA", @"GameX/Resources/Images/Characters/c0.png");
                ImageFile.Add("Chris Melee Vest", @"GameX/Resources/Images/Characters/c0.png");
                ImageFile.Add("Chris Bulletproof Vest", @"GameX/Resources/Images/Characters/c0.png");
                ImageFile.Add("Chris Full Armor", @"GameX/Resources/Images/Characters/c0.png");
                ImageFile.Add("Chris Plain", @"GameX/Resources/Images/Characters/c0.png");
                ImageFile.Add("Chris Safari", @"GameX/Resources/Images/Characters/c5.png");
                ImageFile.Add("Chris STARS", @"GameX/Resources/Images/Characters/c6.png");
                ImageFile.Add("Chris Warrior", @"GameX/Resources/Images/Characters/c7.png");
                ImageFile.Add("Chris Lost in Nightmares", @"GameX/Resources/Images/Characters/c0.png");
                ImageFile.Add("Chris Heavy Metal", @"GameX/Resources/Images/Characters/c9.png");
                ImageFile.Add("Sheva BSAA", @"GameX/Resources/Images/Characters/s0.png");
                ImageFile.Add("Sheva Melee Vest", @"GameX/Resources/Images/Characters/s0.png");
                ImageFile.Add("Sheva Bulletproof Vest", @"GameX/Resources/Images/Characters/s0.png");
                ImageFile.Add("Sheva Full Armor", @"GameX/Resources/Images/Characters/s0.png");
                ImageFile.Add("Sheva Plain", @"GameX/Resources/Images/Characters/s0.png");
                ImageFile.Add("Sheva Amazon", @"GameX/Resources/Images/Characters/s5.png");
                ImageFile.Add("Sheva Clubbin", @"GameX/Resources/Images/Characters/s6.png");
                ImageFile.Add("Sheva Business", @"GameX/Resources/Images/Characters/s7.png");
                ImageFile.Add("Sheva Fairy Tail", @"GameX/Resources/Images/Characters/s8.png");
                ImageFile.Add("Jill BSAA", @"GameX/Resources/Images/Characters/j0.png");
                ImageFile.Add("Jill Battlesuit", @"GameX/Resources/Images/Characters/j1.png");
                ImageFile.Add("Jill Desesperate Escape", @"GameX/Resources/Images/Characters/j1.png");
                ImageFile.Add("Jill Lost in Nightmares", @"GameX/Resources/Images/Characters/j0.png");
                ImageFile.Add("Wesker Midnight", @"GameX/Resources/Images/Characters/w0.png");
                ImageFile.Add("Wesker STARS", @"GameX/Resources/Images/Characters/w1.png");
                ImageFile.Add("Josh Story Mode", @"GameX/Resources/Images/Characters/js0.png");
                ImageFile.Add("Josh Reunion BSAA", @"GameX/Resources/Images/Characters/js0.png");
                ImageFile.Add("Excella Tricell", @"GameX/Resources/Images/Characters/e0.png");
                ImageFile.Add("Barry STARS", @"GameX/Resources/Images/Characters/b0.png");
                ImageFile.Add("Rebecca STARS", @"GameX/Resources/Images/Characters/r0.png");
                ImageFile.Add("Irving Merchant of Death", @"GameX/Resources/Images/Characters/i0.png");

                bool Exists = ImageFile.TryGetValue(CharCos, out string Output);

                if (!Exists)
                    return null;

                Image Result = Image.FromFile(Output);

                return Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
