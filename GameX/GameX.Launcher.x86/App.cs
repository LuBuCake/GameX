using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GameX.Launcher.Base.Content;
using GameX.Launcher.Base.Helpers;
using GameX.Launcher.Base.Types;

namespace GameX.Launcher
{
    public partial class App : XtraForm
    {
        // App Init //

        private GameXInfo[] Games { get; set; }
        private GameXInfo Downloading { get; set; }

        public App()
        {
            InitializeComponent();
        }

        private async void App_Load(object sender, EventArgs e)
        {
            bool UpdaterMustUpdate = await CheckForLauncherUpdate();

            if (UpdaterMustUpdate)
                return;

            SetupControls();
        }

        private void SetupControls()
        {
            Games = GameXInfos.Available();

            string AppDirectory = Directory.GetCurrentDirectory();
            string AddonsDirectory = AppDirectory + "/addons/";

            if (!Directory.Exists(AddonsDirectory))
                Directory.CreateDirectory(AddonsDirectory);

            foreach (GameXInfo Game in Games)
            {
                string AddonDir = AddonsDirectory + Game.GameXFile.Replace(".dll", "") + "/";

                if (!Directory.Exists(AddonDir) || !File.Exists(AddonDir + Game.GameXFile))
                {
                    Game.Downloaded = false;
                    continue;
                }

                Game.Downloaded = true;
            }

            GameXComboEdit.Properties.Items.AddRange(Games);
            GameXComboEdit.SelectedIndexChanged += GameX_IndexChanged;
            GameXComboEdit.SelectedIndex = 0;

            GameXButton.Click += GameX_Click;
        }

        // Event Handlers //

        private async void GameX_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            GameXInfo Info = CBE.SelectedItem as GameXInfo;

            try
            {
                string AppDirectory = Directory.GetCurrentDirectory();
                string AddonsDirectory = AppDirectory + "/addons/";
                string AddonDir = AddonsDirectory + Info.GameXFile.Replace(".dll", "") + "/";

                if (!Info.Downloaded)
                {
                    GameXPictureEdit.Image = null;
                    GameXButton.Text = "Download";
                    GameXButton.Enabled = true;
                    return;
                }

                if (Info.Downloaded && !Info.Updated)
                {
                    Info.Current = AssemblyName.GetAssemblyName(AddonDir + Info.GameXFile).Version;

                    GameXButton.Text = "Checking";
                    GameXButton.Enabled = false;
                    bool UpdateAvailable = await CheckForAddonUpdate(Info);
                    GameXButton.Text = UpdateAvailable ? "Update" : "Launch";
                    GameXButton.Enabled = true;
                }
                else
                {
                    GameXButton.Text = "Launch";
                    GameXButton.Enabled = true;
                }

                if (Info.Logo == null)
                {
                    string ImagesDir = "addons/" + Info.GameXFile.Replace(".dll", "") + "/images/application/";

                    Image LogoA = Image.FromFile(ImagesDir + Info.GameXLogo[0]);
                    Image LogoB = Image.FromFile(ImagesDir + Info.GameXLogo[1]);

                    if (LogoA == null || LogoB == null)
                        return;

                    LogoA = await Task.Run(() => LogoA.ColorReplace(Info.GameXLogoColors[0], true));
                    LogoB = await Task.Run(() => LogoB.ColorReplace(Info.GameXLogoColors[1], true));

                    Info.Logo = await Task.Run(() => Utility.MergeImage(LogoA, LogoB));
                }

                GameXPictureEdit.Image = Info.Logo;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void GameX_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;
            GameXInfo Game = GameXComboEdit.SelectedItem as GameXInfo;

            string AppDirectory = Directory.GetCurrentDirectory();

            switch (SB.Text)
            {
                case "Download":
                case "Update":
                    bool HasConnection = await Task.Run(() => Utility.TestConnection("8.8.8.8"));

                    if (!HasConnection)
                    {
                        return;
                    }

                    string UpdaterDirectory = AppDirectory + "/updater/";

                    if (!Directory.Exists(UpdaterDirectory))
                        Directory.CreateDirectory(UpdaterDirectory);

                    SB.Enabled = false;
                    SB.Text = "Starting";

                    WebClient Downloader = new WebClient();
                    Downloader.DownloadProgressChanged += ReportAddonDownloadProgress;
                    Downloader.DownloadFileCompleted += AddonDownloadFinished;
                    Downloader.DownloadFileAsync(new Uri(Game.RepositoryRoute + "latest.zip"), UpdaterDirectory + "latest.zip");

                    Downloading = Game;
                    break;
                case "Launch":
                    Program.RuntimeDll = "addons/" + Game.GameXFile.Replace(".dll", "") + "/" + Game.GameXFile;
                    DialogResult = DialogResult.OK;
                    break;
            }
        }

        private async Task<bool> CheckForAddonUpdate(GameXInfo Game)
        {
            bool HasConnection = await Task.Run(() => Utility.TestConnection("8.8.8.8"));

            if (!HasConnection)
            {
                return false;
            }

            using (WebClient GitHubChecker = new WebClient())
            {
                try
                {
                    string LatestVerion = await Task.Run(() => GitHubChecker.DownloadString(Game.RepositoryRoute + "latest.txt"));

                    int Current = int.Parse(Game.Current.ToString().Replace(".", ""));
                    int Latest = int.Parse(LatestVerion.Replace(".", ""));

                    return Current < Latest;
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

        private async Task<bool> CheckForLauncherUpdate(bool IgnoreUpdater = false)
        {
            if (!IgnoreUpdater)
            {
                bool UpdaterMustUpdate = await CheckForUpdaterUpdate();

                if (UpdaterMustUpdate)
                    return true;
            }

            bool HasConnection = await Task.Run(() => Utility.TestConnection("8.8.8.8"));

            if (!HasConnection)
            {
                return false;
            }

            using (WebClient GitHubChecker = new WebClient())
            {
                try
                {
                    string LatestVerion = await Task.Run(() => GitHubChecker.DownloadString("https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Launcher.x86/latest.txt"));

                    Assembly CurApp = Assembly.GetExecutingAssembly();
                    AssemblyName CurName = new AssemblyName(CurApp.FullName);

                    int Current = int.Parse(CurName.Version.ToString().Replace(".", ""));
                    int Latest = int.Parse(LatestVerion.Replace(".", ""));

                    if (Current >= Latest)
                    {
                        return false;
                    }

                    AppVersion _version = new AppVersion()
                    {
                        FileRoute = "https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Launcher.x86/latest.zip",
                        StartLauncher = "x86"
                    };

                    if (Utility.MessageBox_Information($"A new launcher version is available. Click OK to update it.") == DialogResult.OK)
                    {
                        string AppDirectory = Directory.GetCurrentDirectory();
                        string UpdaterDirectory = AppDirectory + "/updater/";

                        if (!Directory.Exists(UpdaterDirectory))
                            Directory.CreateDirectory(UpdaterDirectory);

                        Serializer.WriteDataFile(UpdaterDirectory + "updateapp.json", Serializer.SerializeAppVersion(_version));
                        Process.Start(AppDirectory + "/Updater.exe");
                        Application.Exit();
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private async Task<bool> CheckForUpdaterUpdate()
        {
            bool HasConnection = await Task.Run(() => Utility.TestConnection("8.8.8.8"));

            if (!HasConnection)
            {
                return false;
            }

            using (WebClient GitHubChecker = new WebClient())
            {
                try
                {
                    string LatestVerion = await Task.Run(() => GitHubChecker.DownloadString("https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Updater/latest.txt"));
                    string FilePath = Directory.GetCurrentDirectory() + "/updater.exe";

                    AssemblyName CurName = new AssemblyName();

                    bool FileExists = File.Exists(FilePath);

                    if (FileExists)
                    {
                        Assembly CurApp = Assembly.LoadFile(FilePath);
                        CurName = new AssemblyName(CurApp.FullName);
                    }

                    int Current = int.Parse(FileExists ? CurName.Version.ToString().Replace(".", "") : "0");
                    int Latest = int.Parse(LatestVerion.Replace(".", ""));

                    if (Current >= Latest)
                    {
                        return false;
                    }

                    string AppDirectory = Directory.GetCurrentDirectory();
                    string UpdaterDirectory = AppDirectory + "/updater/";

                    if (!Directory.Exists(UpdaterDirectory))
                        Directory.CreateDirectory(UpdaterDirectory);

                    GameXButton.Enabled = false;
                    GameXComboEdit.Enabled = false;

                    GitHubChecker.DownloadProgressChanged += ReportUpdaterDownloadProgress;
                    GitHubChecker.DownloadFileCompleted += UpdaterDownloadFinished;
                    GitHubChecker.DownloadFileAsync(new Uri("https://raw.githubusercontent.com/LuBuCake/GameX/main/GameX/GameX.Versioning/GameX.Updater/latest.zip"), UpdaterDirectory + "latest.zip");

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private void ReportUpdaterDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Text = $"GameX - Downloading Updater {e.ProgressPercentage}%";
        }

        private async void UpdaterDownloadFinished(object sender, AsyncCompletedEventArgs e)
        {
            Text = $"GameX - Extracting Updater";
            await ExtractLatestPackage();
            GameXButton.Enabled = true;
            GameXComboEdit.Enabled = true;
            Text = $"GameX - MT Framework";
            CheckForLauncherUpdate(true);
            SetupControls();
        }

        private void ReportAddonDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            GameXButton.Text = $"{e.ProgressPercentage}%";
        }

        private async void AddonDownloadFinished(object sender, AsyncCompletedEventArgs e)
        {
            GameXButton.Text = "Extracting";
            await ExtractLatestPackage();
            GameXButton.Enabled = true;
            GameXButton.Text = "Launch";

            GameX_IndexChanged(GameXComboEdit, null);
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
                    }
                    catch (Exception)
                    {
                        // ignore
                    }
                }
            }

            if (Downloading != null)
            {
                Downloading.Downloaded = true;
                Downloading = null;
            }

            Directory.Delete(AppDirectory + "/updater/", true);
        }
    }
}