using System.Reflection;
using GameX.Base.Types;

namespace GameX.Base.Modules
{
    public static class Updater
    {
        public static AppVersion GetAppVersion()
        {
            Assembly CurApp = Assembly.GetExecutingAssembly();
            AssemblyName CurName = new AssemblyName(CurApp.FullName);

            return new AppVersion()
            {
                Current = CurName.Version,
                VersionCheckRoute = "",
                VersionFileRoute = ""
            };
        }

        public static void CheckForUpdates(AppVersion Object)
        {

        }
    }
}
