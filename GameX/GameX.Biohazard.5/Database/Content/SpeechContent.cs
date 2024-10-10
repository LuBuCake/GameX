using GameX.Database.Type;
using GameX.Helpers;
using GameX.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameX.Database.Content
{
    public class SpeechContent
    {
        public static List<Speech> GetCollection(bool WritePrefabs)
        {
            if (!WritePrefabs)
            {
                try
                {
                    DirectoryInfo Folder = new DirectoryInfo(@"addons/GameX.Biohazard.5/prefabs/speech/");
                    FileInfo[] Files = Folder.GetFiles("*.json");
                    List<Speech> Available = new List<Speech>();

                    foreach (FileInfo file in Files)
                    {
                        Available.Add(Serializer.Deserialize<Speech>(File.ReadAllText(@"addons/GameX.Biohazard.5/prefabs/speech/" + file.Name)));
                    }

                    return Available.OrderBy(x => x.Character).ToList();
                }
                catch (Exception Ex)
                {
                    Terminal.WriteLine(Ex);
                    return new List<Speech>();
                }
            }

            Speech Chris = new Speech
            {
                Character = Enum.CharacterEnum.Chris,
                Lines = new List<Simple>
                {
                    new Simple("Josh", 6),
                    new Simple("Barry", 7),
                    new Simple("Rebecca", 8),
                    new Simple("Excella", 9),
                    new Simple("[No File]", 10),
                    new Simple("Sheva", 11),
                    new Simple("Jill", 12),
                    new Simple("Wesker", 13),
                    new Simple("This way!", 14),
                    new Simple("Follow me!", 15),
                    new Simple("Go!", 16),
                    new Simple("Split up!", 17),
                    new Simple("Fan out!", 18),
                    new Simple("Roger.", 19),
                    new Simple("Ok.", 20),
                    new Simple("Leave it to me.", 21),
                    new Simple("I can't right now!", 22),
                    new Simple("Out of the question!", 23),
                    new Simple("I'd rather not...", 24),
                    new Simple("Scope out the area!", 25),
                    new Simple("We can cover more ground separately!", 26),
                    new Simple("Wait!", 27),
                    new Simple("Josh", 28),
                    new Simple("I need ammo!", 29),
                    new Simple("Gimme an herb!", 30),
                    new Simple("Give me a grenade!", 31),
                    new Simple("Use this!", 32),
                    new Simple("Take this!", 33),
                    new Simple("Barry!", 34),
                    new Simple("Rebecca!", 35),
                    new Simple("Excella!", 36),
                    new Simple("[No File]", 37),
                    new Simple("Sheva!", 38),
                    new Simple("Jill!", 39),
                    new Simple("Wesker!", 40),
                    new Simple("Come on!", 41),
                    new Simple("Hurry!", 42),
                    new Simple("Help!", 43),
                    new Simple("Help me out here!", 44),
                    new Simple("Help... me! (Dying)", 45),
                    new Simple("I'm not... gonna make it...! (Dying)", 46),
                    new Simple("I'm coming! (Partner Dying)", 47),
                    new Simple("Hold on! (Partner Dying)", 48),
                    new Simple("Thanks.", 49),
                    new Simple("Much Appreciated", 50),
                    new Simple("Thanks for the help", 51),
                    new Simple("Watch yourself, okay?", 52),
                    new Simple("You okay?", 53),
                    new Simple("Stay with me!", 54),
                    new Simple("Josh!!! (Partner died)", 55),
                    new Simple("Barry!!! (Partner died)", 56),
                    new Simple("Rebecca!!! (Partner died)", 57),
                    new Simple("Nice!", 58),
                    new Simple("Good job!", 59),
                    new Simple("That'll work!", 60),
                    new Simple("[No File]", 61),
                    new Simple("Great shot!", 62),
                    new Simple("Good work!", 63),
                    new Simple("Nice work!", 64),
                    new Simple("Excella!!! (Partner died)", 65),
                    new Simple("Reload!", 66),
                    new Simple("[No File]", 67),
                    new Simple("Sheva!!! (Partner died)", 68),
                    new Simple("Jill!!! (Partner died)", 69),
                    new Simple("Wesker!!! (Partner died)", 70),
                    new Simple("[No File]", 71),
                    new Simple("Shit!", 72),
                    new Simple("Tsc.", 73),
                    new Simple("[No File]", 74),
                    new Simple("[No File]", 75),
                    new Simple("[No File]", 76),
                    new Simple("[No File]", 77),
                    new Simple("[No File]", 78),
                    new Simple("Watch out!", 79),
                    new Simple("Gimme your gun!", 80),
                    new Simple("Gimme your weapon!", 81),
                    new Simple("Gimme your handgun!", 82),
                    new Simple("Gimme your shotgun!", 83),
                    new Simple("Gimme your rifle!", 84),
                    new Simple("Gimme your magnum!", 85),
                    new Simple("Gimme your machinegun!", 86),
                    new Simple("Gimme your rocket launcher!", 87),
                    new Simple("I need that!", 88),
                    new Simple("Gimme an egg!", 89),
                    new Simple("Gimme a can of first aid spray!", 90),
                    new Simple("I can't hold anymore...", 91),
                    new Simple("I've got all I can hold.", 92),
                    new Simple("You hang onto it!", 93),
                    new Simple("Take it!", 94),
                    new Simple("You grab it!", 95),
                    new Simple("Gimme a hand!", 96),
                    new Simple("A little help?", 97),
                    new Simple("You go", 98),
                    new Simple("After you", 99)
                }
            };

            var obj = new List<Speech>
            {
                Chris
            };

            try
            {
                foreach (Speech Char in obj)
                    Serializer.WriteDataFile(@"addons/GameX.Biohazard.5/prefabs/speech/" + $"{Char.Character.ToString().ToLower()}.json", Serializer.Serialize(Char));

                Terminal.WriteLine("[App] Speech prefabs written sucessfully.");
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex);
            }

            return obj;
        }
    }
}
