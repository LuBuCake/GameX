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
using GameX.Launcher.Helpers;
using GameX.Launcher.Database;
using GameX.Launcher.Database.Type;

namespace GameX.Launcher
{
    public partial class App : XtraForm
    {
        private Addon Downloading { get; set; }
        private bool DOWNLOAD_UPDATER_IF_NOT_PRESENT = true;

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
            DB db = DBContext.GetDatabase();

            string AppDirectory = Directory.GetCurrentDirectory();
            string AddonsDirectory = AppDirectory + "/addons/";

            if (!Directory.Exists(AddonsDirectory))
                Directory.CreateDirectory(AddonsDirectory);

            foreach (Addon Game in db.Addons)
            {
                string AddonDir = AddonsDirectory + Game.File.Replace(".dll", "") + "/";

                if (!Directory.Exists(AddonDir) || !File.Exists(AddonDir + Game.File))
                {
                    Game.Downloaded = false;
                    continue;
                }

                Game.Downloaded = true;
            }

            GameXComboEdit.Properties.Items.AddRange(db.Addons);
            GameXComboEdit.SelectedIndexChanged += GameX_IndexChanged;
            GameXComboEdit.SelectedIndex = 0;

            GameXButton.Click += GameX_Click;
        }

        // Event Handlers //

        private async void GameX_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            Addon Info = CBE.SelectedItem as Addon;

            try
            {
                string AppDirectory = Directory.GetCurrentDirectory();
                string AddonsDirectory = AppDirectory + "/addons/";
                string AddonDir = AddonsDirectory + Info.File.Replace(".dll", "") + "/";

                if (!Info.Downloaded)
                {
                    GameXPictureEdit.Image = null;
                    GameXButton.Text = "Download";
                    GameXButton.Enabled = true;
                    return;
                }

                if (Info.Downloaded && !Info.Updated)
                {
                    Info.Current = AssemblyName.GetAssemblyName(AddonDir + Info.File).Version;

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
                    string ImagesDir = "addons/" + Info.File.Replace(".dll", "") + "/images/application/";

                    Image LogoA = Image.FromFile(ImagesDir + Info.Images[0]);
                    Image LogoB = Image.FromFile(ImagesDir + Info.Images[1]);

                    if (LogoA == null || LogoB == null)
                        return;

                    LogoA = await Task.Run(() => LogoA.ColorReplace(Info.ImageColors[0], true));
                    LogoB = await Task.Run(() => LogoB.ColorReplace(Info.ImageColors[1], true));

                    Info.Logo = await Task.Run(() => ImageHelper.MergeImage(LogoA, LogoB));
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
            Addon Game = GameXComboEdit.SelectedItem as Addon;

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
                    Program.RuntimeDll = "addons/" + Game.File.Replace(".dll", "") + "/" + Game.File;
                    DialogResult = DialogResult.OK;
                    break;
            }
        }

        private async Task<bool> CheckForAddonUpdate(Addon Game)
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

                        Serializer.WriteDataFile(UpdaterDirectory + "updateapp.json", Serializer.Serialize(_version));
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

                    int Latest = int.Parse(LatestVerion.Replace(".", ""));
                    int Current = 0;

                    string FilePath = Directory.GetCurrentDirectory() + "/updater.exe";
                    string FilePathConfig = Directory.GetCurrentDirectory() + "/updater.exe.config";

                    if (!File.Exists(FilePath) || !File.Exists(FilePathConfig))
                    {
                        if (!DOWNLOAD_UPDATER_IF_NOT_PRESENT)
                            return false;
                    }
                    else
                    {
                        Assembly CurApp = Assembly.Load(File.ReadAllBytes(FilePath));
                        AssemblyName CurName = new AssemblyName(CurApp.FullName);
                        Current = int.Parse(CurName.Version.ToString().Replace(".", ""));                   
                    }

                    if (Current >= Latest)
                        return false;

                    if (File.Exists(FilePath))
                        File.Delete(FilePath);

                    if (File.Exists(FilePathConfig))
                        File.Delete(FilePathConfig);

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