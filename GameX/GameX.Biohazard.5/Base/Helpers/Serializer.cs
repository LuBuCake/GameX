using System.IO;
using GameX.Base.Types;
using GameX.Game.Types;
using Newtonsoft.Json;

namespace GameX.Base.Helpers
{
    public static class Serializer
    {
        #region Serializer

        // APP SETTINGS //

        public static string SerializeSettings(Settings Data)
        {
            return JsonConvert.SerializeObject(Data, Formatting.Indented);
        }

        public static Settings DeserializeSettings(string Data)
        {
            return JsonConvert.DeserializeObject<Settings>(Data);
        }

        public static string SerializeGameXInfo(GameXInfo Data)
        {
            return JsonConvert.SerializeObject(Data, Formatting.Indented);
        }

        public static GameXInfo DeserializeGameXInfo(string Data)
        {
            return JsonConvert.DeserializeObject<GameXInfo>(Data);
        }

        public static string SerializeCharacter(Character Data)
        {
            return JsonConvert.SerializeObject(Data, Formatting.Indented);
        }

        public static Character DeserializeCharacter(string Data)
        {
            return JsonConvert.DeserializeObject<Character>(Data);
        }

        public static string SerializeItem(Item Data)
        {
            return JsonConvert.SerializeObject(Data, Formatting.Indented);
        }

        public static Item DeserializeItem(string Data)
        {
            return JsonConvert.DeserializeObject<Item>(Data);
        }

        #endregion

        #region File Writer

        public static void WriteDataFile(string Path, string Data)
        {
            File.WriteAllText(Path, Data);
        }

        public static string ReadDataFile(string Path)
        {
            return File.ReadAllText(Path);
        }

        #endregion
    }
}