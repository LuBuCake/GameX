using System.Collections.Generic;
using System.IO;
using GameX.Enum;
using GameX.Database.Content;
using GameX.Database.Type;
using GameX.Database.ViewBag;

namespace GameX.Database
{
    public class DB
    {
        public List<Character> Characters { get; set; }
        public List<Map> Maps { get; set; }
        public List<Item> AllItems { get; set; }
        public List<Speech> AllSpeech { get; set; }
        public List<Item> ComboBoxItems { get; set; }
        public List<Move> MovementMoves { get; set; }
        public List<Move> DamageMoves { get; set; }
        public List<Move> ActionMoves { get; set; }
        public List<Move> DashMoves { get; set; }
        public List<Simple> Palettes { get; set; }
        public List<Simple> Rates { get; set; }
        public List<Simple> MeleeKillSeconds { get; set; }
        public List<Simple> ComboTimerSeconds { get; set; }
        public List<Simple> ComboBonusTimerSeconds { get; set; }
        public List<Simple> Handness { get; set; }
        public List<Simple> WeaponMode { get; set; }
        public List<Simple> WeaponPlacement { get; set; }
        public List<Hotkey> Hotkeys { get; set; }
        public List<LoadoutViewBag> Loadouts { get; set; }
    }

    public static class DBContext
    {
        private static DB Database { get; set; }

        private static void BuildDatabase()
        {
            Database = new DB();

            bool WriteChars = false;
            bool WriteItems = false;
            bool WriteSpeech = false;

            string prefabDir = Directory.GetCurrentDirectory() + "/addons/GameX.Biohazard.5/prefabs/";
            string charDir = prefabDir + "character/";
            string itemDir = prefabDir + "item/";
            string speechDir = prefabDir + "speech/";

            if (!Directory.Exists(prefabDir))
                Directory.CreateDirectory(prefabDir);

            if (!Directory.Exists(charDir))
            {
                Directory.CreateDirectory(charDir);
                WriteChars = true;
            }

            if (!Directory.Exists(itemDir))
            {
                Directory.CreateDirectory(itemDir);
                WriteItems = true;
            }

            if (!Directory.Exists(speechDir))
            {
                Directory.CreateDirectory(speechDir);
                WriteSpeech = true;
            }

            Database.Characters = CharacterContent.GetCollection(WriteChars);
            Database.Maps = MapContent.GetCollection();
            Database.AllItems = ItemContent.GetCollection(WriteItems);
            Database.AllSpeech = SpeechContent.GetCollection(WriteSpeech);

            List<Item> ComboBoxItems = new List<Item>(Database.AllItems);
            ComboBoxItems.RemoveAll(x => x.Name == "Hand To Hand");

            Database.ComboBoxItems = ComboBoxItems;
            Database.MovementMoves = MoveContent.GetCollection(MoveTypeEnum.Movement);
            Database.DamageMoves = MoveContent.GetCollection(MoveTypeEnum.Damage);
            Database.ActionMoves = MoveContent.GetCollection(MoveTypeEnum.Action);
            Database.DashMoves = MoveContent.GetCollection(MoveTypeEnum.Dash);
            Database.Palettes = PaletteContent.GetCollection("The Bezier");
            Database.Rates = RateContent.GetCollection();
            Database.MeleeKillSeconds = MeleeKillSecondsContent.GetCollection();
            Database.ComboTimerSeconds = ComboTimerSecondsContent.GetCollection();
            Database.ComboBonusTimerSeconds = ComboBonusTimerSecondsContent.GetCollection();
            Database.Handness = HandnessContent.GetCollection();
            Database.WeaponMode = WeaponModeContent.GetCollection();
            Database.WeaponPlacement = WeaponPlacementContent.GetCollection();
            Database.Hotkeys = HotkeyContent.GetCollection();
            Database.Loadouts = LoadoutViewBagContent.GetCollection();
        }

        public static void UpdateLoadouts()
        {
            Database.Loadouts = LoadoutViewBagContent.GetCollection();
        }

        public static DB GetDatabase()
        {
            if (Database == null)
                BuildDatabase();

            return Database;
        }
    }
}
