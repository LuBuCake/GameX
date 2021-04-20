using GameX.Game.Types;
using GameX.Base.Types;
using Newtonsoft.Json;
using System.IO;

namespace GameX.Base.Helpers
{
    public class Serializer
    {
        #region Serializer

        // NET DATA //

        public static string SerializeNetData(NetItem Data)
        {
            return JsonConvert.SerializeObject(Data);
        }

        public static NetItem DeserializeNetData(string Data)
        {
            return JsonConvert.DeserializeObject<NetItem>(Data);
        }

        public static string SerializeNetCharacterData(NetCharacter Data)
        {
            return JsonConvert.SerializeObject(Data);
        }

        public static NetCharacter DeserializeNetCharacterData(string Data)
        {
            return JsonConvert.DeserializeObject<NetCharacter>(Data);
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
