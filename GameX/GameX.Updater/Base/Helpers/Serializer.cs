using System.IO;
using GameX.Updater.Base.Types;
using Newtonsoft.Json;

namespace GameX.Updater.Base.Helpers
{
    public class Serializer
    {
        public static string SerializeAppVersion(AppVersion Data)
        {
            return JsonConvert.SerializeObject(Data, Formatting.Indented);
        }

        public static AppVersion DeserializeAppVersion(string Data)
        {
            return JsonConvert.DeserializeObject<AppVersion>(Data);
        }

        public static void WriteDataFile(string Path, string Data)
        {
            File.WriteAllText(Path, Data);
        }

        public static string ReadDataFile(string Path)
        {
            return File.ReadAllText(Path);
        }
    }
}
