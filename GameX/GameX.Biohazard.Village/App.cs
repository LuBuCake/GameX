using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using GameX.Base.Content;
using GameX.Base.Helpers;
using GameX.Base.Modules;
using GameX.Base.Types;
using GameX.Game.Modules;

namespace GameX
{
    public partial class App : XtraForm
    {
        #region Base Props

        private Stopwatch FrameElapser { get; set; }
        private Stopwatch CurTimeElapser { get; set; }
        public double FrameTime { get; private set; }
        public double CurTime { get; private set; }
        public double UpdateMode { get; set; }
        public double FramesPerSecond { get; private set; }
        public bool Verified { get; private set; }
        public bool Initialized { get; private set; }

        #endregion

        #region Application Methods

        public App()
        {
            InitializeComponent();
            Application_Init();
        }

        private void App_Load(object sender, EventArgs e)
        {
            Application_Load();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            while (Peaker.IsApplicationIdle())
            {
                Application_Update();
            }
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (Target_Process != null)
            {
                if (Initialized)
                    GameX_End();

                Target_Process.Exited += null;
                Target_Process.EnableRaisingEvents = false;
            }

            Application.Idle += null;

            Memory.FinishModule();
            Keyboard.RemoveHook();

            Target_Process?.Dispose();
            Target_Process = null;
        }

        private void Application_Init()
        {
            Target_Setup();

            FrameElapser = new Stopwatch();
            CurTimeElapser = new Stopwatch();

            FrameElapser.Start();
            CurTimeElapser.Start();

            Keyboard.CreateHook(GameX_Keyboard);

            Application.Idle += Application_Idle;
            Application.ApplicationExit += Application_ApplicationExit;

            Terminal.StartModule(this);
            ConsoleInputTextEdit.Validating += Terminal.ValidateInput;
        }

        private void Application_Load()
        {
            SetupControls();
        }

        private void Application_Update()
        {
            if (!FrameElapser.IsRunning)
                FrameElapser.Start();

            CurTime = CurTimeElapser.Elapsed.TotalSeconds;

            TimeSpan Elapsed = FrameElapser.Elapsed;

            if (Elapsed.TotalSeconds < UpdateMode)
                return;

            FrameElapser.Stop();
            FrameElapser.Reset();

            FramesPerSecond = 1.0 / Elapsed.TotalSeconds;
            FrameTime = 1.0 / FramesPerSecond;

            Application_DoWork();
        }

        private void Application_DoWork()
        {
            if (Target_Handle() && Initialized)
                GameX_Update();
        }

        #endregion

        #region Target Processing

        private string Target { get; set; }
        private string Target_Version { get; set; }
        public Process Target_Process { get; set; }
        private List<string> Target_Modules { get; set; }

        private void Target_Setup()
        {
            Target = "re8.exe";
            Target_Version = "0,0,0,0";
            Target_Modules = new List<string>()
            {
                "steam_api64.dll", "steamclient64.dll"
            };
        }

        private bool Target_Handle()
        {
            if (Target_Process == null)
            {
                Target_Process = Processes.GetProcessByName(Target);
                Verified = false;
                Initialized = false;
                Text = "GameX - Resident Evil Village - Waiting game";

                if (Target_Process == null)
                    return Verified;

                Terminal.WriteLine("[App] Game found, validating.");
                Text = "GameX - Resident Evil Village - Validanting";

                return Verified;
            }

            if (Verified || !Target_Process.WaitForInputIdle())
                return Verified;

            if (Target_Validate())
            {
                Terminal.WriteLine("[App] Game validated.");

                Memory.StartModule(Target_Process);
                CheckDebugModeControls(Memory.DebugMode);
                GameX_Start();

                Target_Process.EnableRaisingEvents = true;
                Target_Process.Exited += Target_Exited;
                Verified = true;
                Initialized = true;
                Text = "GameX - Resident Evil Village - " + (Memory.DebugMode ? "Running in Admin Mode" : "Running in User Mode");

                return Verified;
            }

            Terminal.WriteLine("[App] Failed validating, unsupported version.");

            Target_Process.EnableRaisingEvents = true;
            Target_Process.Exited += Target_Exited;
            Verified = true;
            Initialized = false;
            Text = "GameX - Resident Evil Village - Unsupported Version";

            return Verified;
        }

        private bool Target_Validate()
        {
            try
            {
                if (Target_Version != "" && (Target_Process.MainModule == null || !Target_Process.MainModule.FileVersionInfo.ToString().Contains(Target_Version)))
                    return false;

                if (Target_Modules.Count > 0)
                {
                    if (Target_Modules.Any(Module => !Processes.ProcessHasModule(Target_Process, Module)))
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void Target_Exited(object sender, EventArgs e)
        {
            Memory.FinishModule();
            Biohazard.FinishModule();

            Target_Process?.Dispose();
            Target_Process = null;
            Verified = false;
            Initialized = false;

            Terminal.WriteLine("[App] Runtime cleared successfully.");
        }

        #endregion

        #region Controllers

        private void SetupControls()
        {
            #region Controls

            #endregion

            UpdateModeComboBoxEdit.Properties.Items.AddRange(Rates.Available());
            UpdateModeComboBoxEdit.SelectedIndexChanged += UpdateMode_IndexChanged;
            UpdateModeComboBoxEdit.SelectedIndex = 1;

            PaletteComboBoxEdit.Properties.Items.AddRange(Design.AllPaletts("The Bezier"));
            PaletteComboBoxEdit.SelectedIndexChanged += Palette_IndexChanged;
            PaletteComboBoxEdit.SelectedIndex = 0;

            SaveSettingsButton.Click += Configuration_Save;
            LoadSettingsButton.Click += Configuration_Load;

            MasterTabControl.SelectedPageChanged += MasterTabPage_PageChanged;

            ConsoleModeComboBoxEdit.Properties.Items.AddRange(Interfaces.Available());
            ConsoleModeComboBoxEdit.SelectedIndexChanged += Terminal.Interface_IndexChanged;
            ConsoleModeComboBoxEdit.SelectedIndex = 0;

            ClearConsoleSimpleButton.Click += Terminal.ClearConsole_Click;

            SetLogo();

            Terminal.WriteLine("[App] App initialized.");
            Configuration_Load(null, null);
        }

        private void SetLogo()
        {
            Color Window = CommonSkins.GetSkin(UserLookAndFeel.Default).TranslateColor(SystemColors.Window);
            int total = Window.R + Window.G + Window.B;

            GameXInfo appinfo = Serializer.DeserializeGameXInfo(Serializer.ReadDataFile(@"addons/GameX.Biohazard.Village/appinfo.json"));

            Image LogoA = Utility.GetImageFromStream(appinfo.GameXLogo[0]);
            Image LogoB = Utility.GetImageFromStream(appinfo.GameXLogo[1]);

            if (LogoA == null || LogoB == null)
                return;

            LogoA = LogoA.ColorReplace(appinfo.GameXLogoColors[0], true);
            LogoB = LogoB.ColorReplace(total > 380 ? Color.Black : Color.White, true);

            Image Logo = Utility.MergeImage(LogoA, LogoB);
            AboutPictureEdit.Image = Logo;
        }

        private void CheckDebugModeControls(bool DebugMode)
        {
            if (DebugMode)
                return;
        }

        #endregion

        #region Event Handlers

        // CONTROLLERS //

        private void MasterTabPage_PageChanged(object sender, EventArgs e)
        {
            XtraTabControl XTC = sender as XtraTabControl;
            XtraTabPage XTP = XTC.SelectedTabPage;

            if (XTP.Name != "TabPageConsole")
                return;

            Terminal.ScrollToEnd();
        }

        private void Configuration_Save(object sender, EventArgs e)
        {
            Settings Setts = new Settings()
            {
                UpdateRate = UpdateModeComboBoxEdit.SelectedIndex,
                SkinName = UserLookAndFeel.Default.ActiveSvgPaletteName
            };

            try
            {
                Serializer.WriteDataFile(@"addons/GameX.Biohazard.Village/appsettings.json", Serializer.SerializeSettings(Setts));
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                throw;
            }

            Utility.MessageBox_Information("Settings saved.");

            Terminal.WriteLine("[App] Settings saved.");
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            Settings Setts = new Settings()
            {
                UpdateRate = 1,
                SkinName = "VS Dark"
            };

            try
            {
                if (File.Exists(@"addons/GameX.Biohazard.Village/appsettings.json"))
                    Setts = Serializer.DeserializeSettings(File.ReadAllText(@"addons/GameX.Biohazard.Village/appsettings.json"));
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                throw;
            }

            UpdateModeComboBoxEdit.SelectedIndex = Utility.Clamp(Setts.UpdateRate, 0, 2);

            foreach (ListItem Pallete in PaletteComboBoxEdit.Properties.Items)
            {
                if (Pallete.Text == Setts.SkinName)
                    PaletteComboBoxEdit.SelectedItem = Pallete;
            }

            if (sender != null)
                Utility.MessageBox_Information("Settings loaded.");

            Terminal.WriteLine("[App] Settings loaded.");
        }

        private void UpdateMode_IndexChanged(object sender, EventArgs e)
        {
            UpdateMode = 1.0 / (UpdateModeComboBoxEdit.SelectedItem as ListItem).Value;
        }

        private void Palette_IndexChanged(object sender, EventArgs e)
        {
            if (PaletteComboBoxEdit.Text == "")
                return;

            UserLookAndFeel.Default.SetSkinStyle(UserLookAndFeel.Default.ActiveSkinName, PaletteComboBoxEdit.Text);

            SetLogo();
        }

        // MODS //

        private void EnableDisable_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;

            SB.Text = SB.Text == "Enable" ? "Disable" : "Enable";

            switch (SB.Name)
            {
                case "InfiniteHealthButton":
                {
                    if (Initialized)
                        Biohazard.InfiniteHealth(SB.Text == "Disable");

                    break;
                }
            }
        }

        #endregion

        #region GameX Calls

        private void GameX_Start()
        {
            try
            {
                Biohazard.StartModule();
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
            }
        }

        private void GameX_End()
        {
            try
            {
                if (!Biohazard.ModuleStarted)
                    return;

                Biohazard.FinishModule();
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
            }
        }

        private void GameX_Update()
        {
            try
            {
                if (!Biohazard.ModuleStarted)
                    return;

            }
            catch (Exception)
            {
                // ignore
            }
        }

        private void GameX_Keyboard(int input)
        {
            if (!Initialized)
                return;

        }

        #endregion
    }
}