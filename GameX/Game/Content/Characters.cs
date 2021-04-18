using GameX.Game.Types;
using GameX.Helpers;
using GameX.Modules;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameX.Game.Content
{
    public class Characters
    {
        public static List<Character> GetDefaultChars()
        {
            Character Chris = new Character
            {
                Name = "Chris",
                Value = 0,
                Costumes = new List<Costume>() {
                new Costume { Name = "BSAA", Portrait = @"GameX/Resources/Images/Characters/c0.png", File = "uPl00ChrisNormal", Value = 0 },
                new Costume { Name = "Melee Vest", Portrait = @"GameX/Resources/Images/Characters/c0.png", File = "uPl00ChrisArmorA", Value = 1 },
                new Costume { Name = "Bulletproof Vest", Portrait = @"GameX/Resources/Images/Characters/c0.png", File = "uPl00ChrisArmorB", Value = 2 },
                new Costume { Name = "Full Armor", Portrait = @"GameX/Resources/Images/Characters/c0.png", File = "uPl00ChrisArmorF", Value = 3 },
                new Costume { Name = "Plain", Portrait = @"GameX/Resources/Images/Characters/c0.png", File = "uPl00ChrisPlain", Value = 4 },
                new Costume { Name = "Safari", Portrait = @"GameX/Resources/Images/Characters/c5.png", File = "uPl00ChrisCos1", Value = 5 },
                new Costume { Name = "STARS", Portrait = @"GameX/Resources/Images/Characters/c6.png", File = "uPl00ChrisCos2", Value = 6 },
                new Costume { Name = "Warrior", Portrait = @"GameX/Resources/Images/Characters/c7.png", File = "uPl00ChrisCos3", Value = 7 },
                new Costume { Name = "Lost in Nightmares", Portrait = @"GameX/Resources/Images/Characters/c0.png", File = "uPl00ChrisEp1", Value = 8 },
                new Costume { Name = "Heavy Metal", Portrait = @"GameX/Resources/Images/Characters/c9.png", File = "uPl00ChrisCos4", Value = 9 }
            }
            };

            Character Sheva = new Character
            {
                Name = "Sheva",
                Value = 1,
                Costumes = new List<Costume>() {
                new Costume { Name = "BSAA", Portrait = @"GameX/Resources/Images/Characters/s0.png", File = "uPl01ShebaNormal", Value = 0 },
                new Costume { Name = "Melee Vest", Portrait = @"GameX/Resources/Images/Characters/s0.png", File = "uPl01ShebaArmorA", Value = 1 },
                new Costume { Name = "Bulletproof Vest", Portrait = @"GameX/Resources/Images/Characters/s0.png", File = "uPl01ShebaArmorB", Value = 2 },
                new Costume { Name = "Full Armor", Portrait = @"GameX/Resources/Images/Characters/s0.png", File = "uPl01ShebaArmorF", Value = 3 },
                new Costume { Name = "Plain", Portrait = @"GameX/Resources/Images/Characters/s0.png", File = "uPl01ShebaPlain", Value = 4 },
                new Costume { Name = "Amazon", Portrait = @"GameX/Resources/Images/Characters/s5.png", File = "uPl01ShebaCos1", Value = 5 },
                new Costume { Name = "Clubbin", Portrait = @"GameX/Resources/Images/Characters/s6.png", File = "uPl01ShebaCos2", Value = 6 },
                new Costume { Name = "Business", Portrait = @"GameX/Resources/Images/Characters/s7.png", File = "uPl01ShebaCos3", Value = 7 },
                new Costume { Name = "Fairy Tail", Portrait = @"GameX/Resources/Images/Characters/s8.png", File = "uPl01ShebaCos4", Value = 8 }
            }
            };

            Character Jill = new Character
            {
                Name = "Jill",
                Value = 2,
                Costumes = new List<Costume>() {
                new Costume { Name = "BSAA", Portrait = @"GameX/Resources/Images/Characters/j0.png", File = "uPl02JillCos1", Value = 0 },
                new Costume { Name = "Battlesuit", Portrait = @"GameX/Resources/Images/Characters/j1.png", File = "uPl02JillCos2", Value = 1 },
                new Costume { Name = "Desesperate Escape", Portrait = @"GameX/Resources/Images/Characters/j1.png", File = "uPl02JillCos3", Value = 2 },
                new Costume { Name = "Lost in Nightmares", Portrait = @"GameX/Resources/Images/Characters/j0.png", File = "uPl02JillCos4", Value = 3 }
            }
            };

            Character Wesker = new Character
            {
                Name = "Wesker",
                Value = 3,
                Costumes = new List<Costume>() {
                new Costume { Name = "Midnight", Portrait = @"GameX/Resources/Images/Characters/w0.png", File = "uPl03WeskerCos1", Value = 0 },
                new Costume { Name = "STARS", Portrait = @"GameX/Resources/Images/Characters/w1.png", File = "uPl03WeskerCos2", Value = 1 }
            }
            };

            Character Josh = new Character
            {
                Name = "Josh",
                Value = 4,
                Costumes = new List<Costume>() {
                new Costume { Name = "Story Mode", Portrait = @"GameX/Resources/Images/Characters/js0.png", File = "uPl04JoshCos1", Value = 0 },
                new Costume { Name = "Reunion BSAA", Portrait = @"GameX/Resources/Images/Characters/js0.png", File = "uPl04JoshCos2", Value = 1 }
            }
            };

            Character Excella = new Character
            {
                Name = "Excella",
                Value = 5,
                Costumes = new List<Costume>() {
                new Costume { Name = "Tricell", Portrait = @"GameX/Resources/Images/Characters/e0.png", File = "uPl05ExcellaNormal", Value = 0 }
            }
            };

            Character Barry = new Character
            {
                Name = "Barry",
                Value = 6,
                Costumes = new List<Costume>() {
                new Costume { Name = "STARS", Portrait = @"GameX/Resources/Images/Characters/b0.png", File = "uPl06BarryNormal", Value = 0 }
            }
            };

            Character Rebecca = new Character
            {
                Name = "Rebecca",
                Value = 7,
                Costumes = new List<Costume>() {
                new Costume { Name = "STARS", Portrait = @"GameX/Resources/Images/Characters/r0.png", File = "uPl07RebeccaNormal", Value = 0 }
            }
            };

            Character Irving = new Character
            {
                Name = "Irving",
                Value = 134,
                Costumes = new List<Costume>() {
                new Costume { Name = "Story Mode", Portrait = @"GameX/Resources/Images/Characters/i0.png", File = "uPl86Irving", Value = 0 }
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

        public static void WriteDefaultChars()
        {
            List<Character> Chars = GetDefaultChars();

            foreach (Character Char in Chars)
            {
                Serializer.WriteDataFile(@"GameX/Objects/Characters/" + $"{Char.Name}.json", Serializer.SerializeCharacter(Char));
            }

            Terminal.WriteLine("Characters jsons written sucessfully.");
        }

        public static List<Character> GetCharactersFromFolder()
        {
            DirectoryInfo CharactersFolder = new DirectoryInfo(@"GameX/Objects/Characters/");
            FileInfo[] CharacterFiles = CharactersFolder.GetFiles("*.json");
            List<Character> AvailableCharacters = new List<Character>();

            foreach (FileInfo CharFile in CharacterFiles)
            {
                AvailableCharacters.Add(Serializer.DeserializeCharacter(File.ReadAllText(@"GameX/Objects/Characters/" + CharFile.Name)));
            }

            return AvailableCharacters.OrderBy(x => x.Value).ToList();
        }
    }
}
