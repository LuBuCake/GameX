using System.IO;
using Newtonsoft.Json;

namespace GameX.Helpers
{
    public static class Serializer
    {
        #region Serializer

        public static string Serialize<T>(T Data)
        {
            return JsonConvert.SerializeObject(Data, Formatting.Indented);
        }

        public static T Deserialize<T>(string Data)
        {
            return JsonConvert.DeserializeObject<T>(Data);
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