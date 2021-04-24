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

        // NET DATA //

        public static string SerializeCharacterSelectionChanged(NetCharacterSelectionChange Data)
        {
            return JsonConvert.SerializeObject(Data);
        }

        public static NetCharacterSelectionChange DeserializeCharacterSelectionChanged(string Data)
        {
            return JsonConvert.DeserializeObject<NetCharacterSelectionChange>(Data);
        }

        public static string SerializeCharacterFreezeChanged(NetCharacterFreezeChange Data)
        {
            return JsonConvert.SerializeObject(Data);
        }

        public static NetCharacterFreezeChange DeserializeCharacterFreezeChanged(string Data)
        {
            return JsonConvert.DeserializeObject<NetCharacterFreezeChange>(Data);
        }

        // CLIENT OBJECTS //

        public static string SerializeCharacter(Character Data)
        {
            return JsonConvert.SerializeObject(Data, Formatting.Indented);
        }

        public static Character DeserializeCharacter(string Data)
        {
            return JsonConvert.DeserializeObject<Character>(Data);
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