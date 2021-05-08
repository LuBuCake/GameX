using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameX.Base.Helpers;
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
                VersionCheckRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Biohazard.Village/Version/latest.txt",
                VersionFileRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Biohazard.Village/Version/latest.zip"
            };
        }

        public static bool TestConnection(string HostNameOrAddress, int Timeout = 1000)
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(HostNameOrAddress, Timeout, new byte[32]);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task CheckForUpdates(bool ShowAlert)
        {
            AppVersion Object = GetAppVersion();

            bool HasConnection = await Task.Run(() => TestConnection("8.8.8.8"));

            if (!HasConnection)
            {
                Terminal.WriteLine("[App] Failed to check for updates, the connection was either null or too slow.", ShowAlert ? Enums.MessageBoxType.Error : Enums.MessageBoxType.None);
                return;
            }

            using (WebClient GitHubChecker = new WebClient())
            {
                string LatestVerion = await Task.Run(() => GitHubChecker.DownloadString(Object.VersionCheckRoute));

                int Current = int.Parse(Object.Current.ToString().Replace(".", ""));
                int Latest = int.Parse(LatestVerion.Replace(".", ""));

                if (Current >= Latest)
                {
                    if (ShowAlert)
                        Utility.MessageBox_Information("Your app's version is up-to-date.");

                    return;
                }

                Object.Latest = new Version(LatestVerion);

                if (Utility.MessageBox_YesNo($"A new version is available, would you like to update it now? Your version: {Object.Current} / Latest: {Object.Latest}", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string AppDirectory = Directory.GetCurrentDirectory();
                    string UpdaterDirectory = AppDirectory + "/updater/";

                    if (!Directory.Exists(UpdaterDirectory))
                        Directory.CreateDirectory(UpdaterDirectory);

                    Serializer.WriteDataFile(UpdaterDirectory + "updateapp.json", Serializer.SerializeAppVersion(Object));
                    //Process.Start(@"Updater.exe");
                    Application.Exit();
                }
            }
        }
    }
}
