using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using DevExpress.XtraEditors;
using GameX.Updater.Base.Helpers;
using GameX.Updater.Base.Modules;
using GameX.Updater.Base.Types;

namespace GameX.Updater
{
    public partial class App : XtraForm
    {
        public App()
        {
            InitializeComponent();
        }

        private void App_Load(object sender, EventArgs e)
        {
            Main();
        }

        private void Main()
        {
            Terminal.StartModule(this);
            Terminal.WriteLine("[App] App initialized.");

            string AppDirectory = Directory.GetCurrentDirectory();
            string UpdaterDirectory = AppDirectory + "/updater/";

            if (!Directory.Exists(UpdaterDirectory) || !File.Exists(UpdaterDirectory + "updateapp.json"))
            {
                Terminal.WriteLine("[App] No update schedule found, exiting.");
                SaveLog();
            }

            AppVersion Object = Serializer.DeserializeAppVersion(Serializer.ReadDataFile(UpdaterDirectory + "updateapp.json"));

            using (WebClient Downloader = new WebClient())
            {
                Downloader.DownloadProgressChanged += ReportDownloadProgress;
                Downloader.DownloadFileCompleted += DownloadFinished;
                Downloader.DownloadFileAsync(new Uri(Object.VersionFileRoute), UpdaterDirectory + "latest.zip");
            }

            Terminal.WriteLine("[App] Download started.");
        }

        private void ReportDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Terminal.WriteLine($"[App] Downloading ({e.ProgressPercentage}%): {e.BytesReceived}/{e.TotalBytesToReceive}");
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
            await Task.Run(() => ZipFile.ExtractToDirectory(UpdaterDirectory + "latest.zip", AppDirectory));
            Terminal.WriteLine("[App] Finished unpacking, all files have been updated.");
            SaveLog();
        }

        private void SaveLog()
        {
            string log = ConsoleOutputMemoEdit.Text;
            Serializer.WriteDataFile(Directory.GetCurrentDirectory() + "/updater.txt", log);
            Application.Exit();
        }
    }
}
