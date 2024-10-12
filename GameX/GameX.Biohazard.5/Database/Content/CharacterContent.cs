using GameX.Database.Type;
using System.Collections.Generic;

namespace GameX.Database.Content
{
    public static class CharacterContent
    {
        public static List<Character> GetCollection()
        {
            Character Chris = new Character
            {
                Name = "Chris",
                Value = 0,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "BSAA", Portrait = @"c0.png",
                        File = "uPl00ChrisNormal", Value = 0
                    },
                    new Costume
                    {
                        Name = "Melee Vest", Portrait = @"c1.png",
                        File = "uPl00ChrisArmorA", Value = 1
                    },
                    new Costume
                    {
                        Name = "Bulletproof Vest", Portrait = @"c2.png",
                        File = "uPl00ChrisArmorB", Value = 2
                    },
                    new Costume
                    {
                        Name = "Full Armor", Portrait = @"c3.png",
                        File = "uPl00ChrisArmorF", Value = 3
                    },
                    new Costume
                    {
                        Name = "Plain", Portrait = @"c4.png",
                        File = "uPl00ChrisPlain", Value = 4
                    },
                    new Costume
                    {
                        Name = "Safari", Portrait = @"c5.png",
                        File = "uPl00ChrisCos1", Value = 5
                    },
                    new Costume
                    {
                        Name = "STARS", Portrait = @"c6.png", File = "uPl00ChrisCos2",
                        Value = 6
                    },
                    new Costume
                    {
                        Name = "Warrior", Portrait = @"c7.png",
                        File = "uPl00ChrisCos3", Value = 7
                    },
                    new Costume
                    {
                        Name = "Lost in Nightmares", Portrait = @"c8.png",
                        File = "uPl00ChrisEp1", Value = 8
                    },
                    new Costume
                    {
                        Name = "Heavy Metal", Portrait = @"c9.png",
                        File = "uPl00ChrisCos4", Value = 9
                    }
                }
            };

            Character Sheva = new Character
            {
                Name = "Sheva",
                Value = 1,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "BSAA", Portrait = @"s0.png",
                        File = "uPl01ShebaNormal", Value = 0
                    },
                    new Costume
                    {
                        Name = "Melee Vest", Portrait = @"s1.png",
                        File = "uPl01ShebaArmorA", Value = 1
                    },
                    new Costume
                    {
                        Name = "Bulletproof Vest", Portrait = @"s2.png",
                        File = "uPl01ShebaArmorB", Value = 2
                    },
                    new Costume
                    {
                        Name = "Full Armor", Portrait = @"s3.png",
                        File = "uPl01ShebaArmorF", Value = 3
                    },
                    new Costume
                    {
                        Name = "Plain", Portrait = @"s4.png",
                        File = "uPl01ShebaPlain", Value = 4
                    },
                    new Costume
                    {
                        Name = "Amazon", Portrait = @"s5.png",
                        File = "uPl01ShebaCos1", Value = 5
                    },
                    new Costume
                    {
                        Name = "Clubbin", Portrait = @"s6.png",
                        File = "uPl01ShebaCos2", Value = 6
                    },
                    new Costume
                    {
                        Name = "Business", Portrait = @"s7.png",
                        File = "uPl01ShebaCos3", Value = 7
                    },
                    new Costume
                    {
                        Name = "Fairy Tail", Portrait = @"s8.png",
                        File = "uPl01ShebaCos4", Value = 8
                    }
                }
            };

            Character Jill = new Character
            {
                Name = "Jill",
                Value = 2,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "BSAA", Portrait = @"j0.png", File = "uPl02JillCos1",
                        Value = 0
                    },
                    new Costume
                    {
                        Name = "Battlesuit", Portrait = @"j1.png",
                        File = "uPl02JillCos2", Value = 1
                    },
                    new Costume
                    {
                        Name = "Desesperate Escape", Portrait = @"j2.png",
                        File = "uPl02JillCos3", Value = 2
                    },
                    new Costume
                    {
                        Name = "Lost in Nightmares", Portrait = @"j3.png",
                        File = "uPl02JillCos4", Value = 3
                    }
                }
            };

            Character Wesker = new Character
            {
                Name = "Wesker",
                Value = 3,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "Midnight", Portrait = @"w0.png",
                        File = "uPl03WeskerCos1", Value = 0
                    },
                    new Costume
                    {
                        Name = "STARS", Portrait = @"w1.png",
                        File = "uPl03WeskerCos2", Value = 1
                    }
                }
            };

            Character Josh = new Character
            {
                Name = "Josh",
                Value = 4,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "Story Mode", Portrait = @"js0.png",
                        File = "uPl04JoshCos1", Value = 0
                    },
                    new Costume
                    {
                        Name = "Reunion BSAA", Portrait = @"js1.png",
                        File = "uPl04JoshCos2", Value = 1
                    }
                }
            };

            Character Excella = new Character
            {
                Name = "Excella",
                Value = 5,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "Tricell", Portrait = @"e0.png",
                        File = "uPl05ExcellaNormal", Value = 0
                    }
                }
            };

            Character Barry = new Character
            {
                Name = "Barry",
                Value = 6,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "STARS", Portrait = @"b0.png",
                        File = "uPl06BarryNormal", Value = 0
                    }
                }
            };

            Character Rebecca = new Character
            {
                Name = "Rebecca",
                Value = 7,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "STARS", Portrait = @"r0.png",
                        File = "uPl07RebeccaNormal", Value = 0
                    }
                }
            };

            Character Irving = new Character
            {
                Name = "Irving",
                Value = 134,
                Costumes = new List<Costume>()
                {
                    new Costume
                    {
                        Name = "Story Mode", Portrait = @"i0.png",
                        File = "uPl86Irving", Value = 0
                    }
                }
            };

            var obj = new List<Character>
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

            return obj;
        }
    }
}
