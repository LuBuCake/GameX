using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Net.NetworkInformation;
using DevExpress.XtraEditors;
using GameX.Updater.Base.Helpers;
using GameX.Updater.Base.Modules;
using GameX.Updater.Base.Types;

namespace GameX.Updater
{
    public partial class App : XtraForm
    {
        private AppVersion _Version { get; set; }

        public App()
        {
            InitializeComponent();
        }

        private void App_Load(object sender, EventArgs e)
        {
            Main();
        }

        private async Task Main()
        {
            bool HasConnection = await Task.Run(() => TestConnection("8.8.8.8"));

            if (!HasConnection)
            {
                Terminal.WriteLine("[App] Failed to check for updates, the connection was either null or too slow.");
                SaveLog();
            }

            Terminal.StartModule(this);
            Terminal.WriteLine("[App] App initialized.");

            string AppDirectory = Directory.GetCurrentDirectory();
            string UpdaterDirectory = AppDirectory + "/updater/";

            if (!Directory.Exists(UpdaterDirectory) || !File.Exists(UpdaterDirectory + "updateapp.json"))
            {
                Terminal.WriteLine("[App] No update schedule found, exiting.");
                SaveLog();
            }

            _Version = Serializer.DeserializeAppVersion(Serializer.ReadDataFile(UpdaterDirectory + "updateapp.json"));

            WebClient Downloader = new WebClient();
            Downloader.DownloadProgressChanged += ReportDownloadProgress;
            Downloader.DownloadFileCompleted += DownloadFinished;
            Downloader.DownloadFileAsync(new Uri(_Version.FileRoute), UpdaterDirectory + "latest.zip");

            Terminal.WriteLine("[App] Download started.");
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

        private void SaveLog()
        {
            string log = ConsoleOutputMemoEdit.Text;
            Serializer.WriteDataFile(Directory.GetCurrentDirectory() + "/Updater.exe.txt", log);
            Application.Exit();
        }

        private void ReportDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Terminal.WriteLine($"[App] Downloading: {e.ProgressPercentage}%", true);
        }

        private async void DownloadFinished(object sender, AsyncCompletedEventArgs e)
        {
            string AppDirectory = Directory.GetCurrentDirectory();
            string UpdaterDirectory = AppDirectory + "/updater/";

            if (!File.Exists(UpdaterDirectory + "latest.zip"))
            {
                Terminal.WriteLine("[App] Download finished but the file has gone missing, exiting.");
                SaveLog();
            }

            Terminal.WriteLine("[App] Download finished, unpacking.");
            await ExtractLatestPackage();
            Terminal.WriteLine("[App] Finished unpacking, all files have been updated.");

            if (!string.IsNullOrWhiteSpace(_Version.StartLauncher))
            {
                switch (_Version.StartLauncher)
                {
                    case "x86":
                        Process.Start(AppDirectory + "/GameX-x86.exe");
                        break;
                    case "x64":
                        Process.Start(AppDirectory + "/GameX-x64.exe");
                        break;
                }
            }

            SaveLog();
        }

        private async Task ExtractLatestPackage()
        {
            string AppDirectory = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
            string ZipPath = AppDirectory + "/updater/latest.zip";

            using (ZipArchive archive = ZipFile.OpenRead(ZipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.GetFullPath(Path.Combine(AppDirectory, entry.FullName));

                    if (destinationPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    {
                        if (!Directory.Exists(destinationPath))
                            Directory.CreateDirectory(destinationPath);

                        continue;
                    }

                    try
                    {
                        await Task.Run(() => entry.ExtractToFile(destinationPath, true));
                        Terminal.WriteLine($"[App] {entry.FullName} extracted.");
                    }
                    catch (Exception)
                    {
                        Terminal.WriteLine($"[App] {entry.FullName} skipped.");
                    }
                }
            }

            Directory.Delete(AppDirectory + "/updater/", true);
        }
    }
}
