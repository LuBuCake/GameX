using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using GameX.Base.Content;
using GameX.Base.Helpers;
using GameX.Base.Modules;
using GameX.Base.Types;
using GameX.Game.Content;
using GameX.Game.Helpers;
using GameX.Game.Modules;
using GameX.Game.Types;

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

        #region NET Props

        private bool CharCosSelectionChangeReceived { get; set; }
        private bool CharCosFreezeChangeReceived { get; set; }

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
            Network.FinishModule();
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
            CreatePrefabs(Enums.PrefabType.All);
            SetupControls();

            Updater.CheckForUpdates(false);
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
        private Process Target_Process { get; set; }
        private List<string> Target_Modules { get; set; }

        private void Target_Setup()
        {
            Target = "re5dx9.exe";
            Target_Version = "1.0.0.129";
            Target_Modules = new List<string>()
            {
                "steam_api.dll", "maluc.dll"
            };
        }

        private bool Target_Handle()
        {
            if (Target_Process == null)
            {
                ResetHealthBars();

                Target_Process = Processes.GetProcessByName(Target);
                Verified = false;
                Initialized = false;
                Text = "GameX - Resident Evil 5 - Waiting game";

                if (Target_Process == null)
                    return Verified;

                Terminal.WriteLine("[App] Game found, validating.");
                Text = "GameX - Resident Evil 5 - Validanting";

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
                Text = "GameX - Resident Evil 5 - " + (Memory.DebugMode ? "Running in Admin Mode" : "Running in User Mode");

                return Verified;
            }

            Terminal.WriteLine("[App] Failed validating, unsupported version.");
            Terminal.WriteLine("[App] Follow the guide on https://steamcommunity.com/sharedfiles/filedetails/?id=864823595 to learn how to download and install the latest patch available.");

            Target_Process.EnableRaisingEvents = true;
            Target_Process.Exited += Target_Exited;
            Verified = true;
            Initialized = false;
            Text = "GameX - Resident Evil 5 - Unsupported Version";

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

            ComboBoxEdit[] CharacterCombos =
            {
                P1CharComboBox,
                P2CharComboBox,
                P3CharComboBox,
                P4CharComboBox
            };

            ComboBoxEdit[] CostumeCombos =
            {
                P1CosComboBox,
                P2CosComboBox,
                P3CosComboBox,
                P4CosComboBox
            };

            ComboBoxEdit[] Handness =
            {
                P1HandnessComboBox,
                P2HandnessComboBox,
                P3HandnessComboBox,
                P4HandnessComboBox
            };

            ComboBoxEdit[] WeaponMode =
            {
                P1WeaponModeComboBox,
                P2WeaponModeComboBox,
                P3WeaponModeComboBox,
                P4WeaponModeComboBox
            };

            CheckButton[] CharCosFreezes =
            {
                P1FreezeCharCosButton,
                P2FreezeCharCosButton,
                P3FreezeCharCosButton,
                P4FreezeCharCosButton
            };

            CheckButton[] InfiniteHP =
            {
                P1InfiniteHPButton,
                P2InfiniteHPButton,
                P3InfiniteHPButton,
                P4InfiniteHPButton
            };

            CheckButton[] Untargetable =
            {
                P1UntargetableButton,
                P2UntargetableButton,
                P3UntargetableButton,
                P4UntargetableButton
            };

            CheckButton[] InfiniteAmmo =
            {
                P1InfiniteAmmoButton,
                P2InfiniteAmmoButton,
                P3InfiniteAmmoButton,
                P4InfiniteAmmoButton
            };

            CheckButton[] Rapidfire =
            {
                P1RapidfireButton,
                P2RapidfireButton,
                P3RapidfireButton,
                P4RapidfireButton
            };

            SimpleButton[] DropButtons =
            {
                P1DropSimpleButton,
                P2DropSimpleButton,
                P3DropSimpleButton
            };

            SimpleButton[] EnableDisable =
            {
                ControllerAimButton,
                ColorFilterButton
            };

            SimpleButton[] OnOff =
            {
                WeskerSunglassesButton,
                WeskerInfiniteDashButton,
                WeskerNoDashCostButton
            };

            #endregion

            for (int Index = 0; Index < CharacterCombos.Length; Index++)
            {
                CharacterCombos[Index].Properties.Items.AddRange(Characters.GetCharactersFromFolder());
                WeaponMode[Index].Properties.Items.AddRange(Miscellaneous.WeaponMode());
                Handness[Index].Properties.Items.AddRange(Miscellaneous.Handness());

                CharCosFreezes[Index].CheckedChanged += CharCosFreeze_CheckedChanged;
                InfiniteHP[Index].CheckedChanged += OnOff_CheckedChanged;
                Untargetable[Index].CheckedChanged += OnOff_CheckedChanged;
                InfiniteAmmo[Index].CheckedChanged += OnOff_CheckedChanged;
                Rapidfire[Index].CheckedChanged += OnOff_CheckedChanged;

                CharacterCombos[Index].SelectedIndexChanged += CharComboBox_IndexChanged;
                CostumeCombos[Index].SelectedIndexChanged += CosComboBox_IndexChanged;
                WeaponMode[Index].SelectedIndexChanged += WeaponMode_IndexChanged;
                Handness[Index].SelectedIndexChanged += Handness_IndexChanged;

                CharacterCombos[Index].SelectedIndex = 0;
                WeaponMode[Index].SelectedIndex = 0;
                Handness[Index].SelectedIndex = 0;

                if (Index < EnableDisable.Length)
                {
                    EnableDisable[Index].Click += EnableDisable_Click;
                }

                if (Index == 3)
                    continue;

                DropButtons[Index].Click += Network.Server_DropClient;
                OnOff[Index].Click += OnOff_Click;
            }

            UpdateModeComboBoxEdit.Properties.Items.AddRange(Rates.Available());
            UpdateModeComboBoxEdit.SelectedIndexChanged += UpdateMode_IndexChanged;
            UpdateModeComboBoxEdit.SelectedIndex = 1;

            PaletteComboBoxEdit.Properties.Items.AddRange(Design.AllPaletts("The Bezier"));
            PaletteComboBoxEdit.SelectedIndexChanged += Palette_IndexChanged;
            PaletteComboBoxEdit.SelectedIndex = 0;

            SaveSettingsButton.Click += Configuration_Save;
            LoadSettingsButton.Click += Configuration_Load;

            NetworkManagerButton.Click += StartNetwork_Click;
            StartServerButton.Click += Network.StartServer_Click;

            BuddyServerConnectionButton.Click += Network.StartClient_Click;

            MasterTabControl.SelectedPageChanged += MasterTabPage_PageChanged;

            ConsoleModeComboBoxEdit.Properties.Items.AddRange(Interfaces.Available());
            ConsoleModeComboBoxEdit.SelectedIndexChanged += Terminal.Interface_IndexChanged;
            ConsoleModeComboBoxEdit.SelectedIndex = 0;

            ClearConsoleSimpleButton.Click += Terminal.ClearConsole_Click;
            UpdateButton.Click += Update_Click;

            ResetHealthBars();
            SetLogo();

            Terminal.WriteLine("[App] App initialized.");
            Configuration_Load(null, null);
        }

        private void SetLogo()
        {
            Color Window = CommonSkins.GetSkin(UserLookAndFeel.Default).TranslateColor(SystemColors.Window);
            int total = Window.R + Window.G + Window.B;

            GameXInfo appinfo = Serializer.DeserializeGameXInfo(Serializer.ReadDataFile(@"addons/GameX.Biohazard.5/appinfo.json"));

            Image LogoA = Utility.GetImageFromStream(appinfo.GameXLogo[0]);
            Image LogoB = Utility.GetImageFromStream(appinfo.GameXLogo[1]);

            if (LogoA == null || LogoB == null)
                return;

            LogoA = LogoA.ColorReplace(appinfo.GameXLogoColors[0], true);
            LogoB = LogoB.ColorReplace(total > 380 ? Color.Black : Color.White, true);

            Image Logo = Utility.MergeImage(LogoA, LogoB);
            AboutPictureEdit.Image = Logo;
        }

        private void ResetHealthBars()
        {
            ProgressBarControl[] HealthBars =
            {
                P1HealthBar,
                P2HealthBar,
                P3HealthBar,
                P4HealthBar
            };

            foreach (ProgressBarControl Bar in HealthBars)
            {
                Bar.Properties.Maximum = 1;
                Bar.Properties.Minimum = 0;
                Bar.EditValue = 1;
                Bar.BackColor = CommonSkins.GetSkin(UserLookAndFeel.Default).TranslateColor(SystemColors.Window);
                Bar.Properties.StartColor = Color.FromArgb(0, 0, 0, 0);
                Bar.Properties.EndColor = Color.FromArgb(0, 0, 0, 0);
            }
        }

        private void CheckDebugModeControls(bool DebugMode)
        {
            if (DebugMode)
                return;

            CheckButton[] CheckButtons =
            {
                P1FreezeCharCosButton,
                P2FreezeCharCosButton,
                P3FreezeCharCosButton,
                P4FreezeCharCosButton
            };

            foreach (CheckButton CB in CheckButtons)
            {
                CB.Enabled = false;
                CB.Checked = false;
            }
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
                PlayerName = PlayerNameTextEdit.Text,
                SkinName = UserLookAndFeel.Default.ActiveSvgPaletteName
            };

            try
            {
                Serializer.WriteDataFile(@"addons/GameX.Biohazard.5/appsettings.json", Serializer.SerializeSettings(Setts));
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
                PlayerName = "Player",
                SkinName = "VS Dark"
            };

            try
            {
                if (File.Exists(@"addons/GameX.Biohazard.5/appsettings.json"))
                    Setts = Serializer.DeserializeSettings(File.ReadAllText(@"addons/GameX.Biohazard.5/appsettings.json"));
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                throw;
            }

            UpdateModeComboBoxEdit.SelectedIndex = Utility.Clamp(Setts.UpdateRate, 0, 2);
            PlayerNameTextEdit.Text = Setts.PlayerName;

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

            ResetHealthBars();
            SetLogo();
        }

        private async void Update_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;

            SB.Text = "Checking";
            SB.Enabled = false;

            try
            {
                await Updater.CheckForUpdates(true);
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
            }

            SB.Text = "Update";
            SB.Enabled = true;
        }

        // MODS //

        private void CharComboBox_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit[] CostumeCombos =
            {
                P1CosComboBox,
                P2CosComboBox,
                P3CosComboBox,
                P4CosComboBox
            };

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            Character CBEChar = CBE.SelectedItem as Character;

            int Index = int.Parse(CBE.Name[1].ToString()) - 1;

            CostumeCombos[Index].Properties.Items.Clear();

            foreach (Costume Cos in CBEChar.Costumes)
            {
                CostumeCombos[Index].Properties.Items.Add(Cos);
            }

            CostumeCombos[Index].SelectedIndex = 0;
        }

        private void CosComboBox_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit[] CharacterCombos =
            {
                P1CharComboBox,
                P2CharComboBox,
                P3CharComboBox,
                P4CharComboBox
            };

            PictureEdit[] CharPicBoxes =
            {
                P1CharPictureBox,
                P2CharPictureBox,
                P3CharPictureBox,
                P4CharPictureBox
            };

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            Costume CBECos = CBE.SelectedItem as Costume;

            int Index = int.Parse(CBE.Name[1].ToString()) - 1;

            Image Portrait = Utility.GetImageFromFile(@"addons/GameX.Biohazard.5/images/character/" + CBECos.Portrait);

            if (Portrait != null)
                CharPicBoxes[Index].Image = Portrait;

            if (!CharCosSelectionChangeReceived)
                Character_SendSelectionChange(Index, CharacterCombos[Index].SelectedIndex, CBE.SelectedIndex);

            if (!Initialized)
                return;

            Character_ApplyCharacters(Index);
            Character_DetourValueUpdate();
        }

        private void WeaponMode_IndexChanged(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            int Index = int.Parse(CBE.Name[1].ToString()) - 1;

            Biohazard.Players[Index].SetWeaponMode(CBE.SelectedIndex != 0 ? new[] {(byte) (CBE.SelectedItem as ListItem).Value} : new[] {(byte) Biohazard.Players[Index].GetDefaultWeaponMode()});
        }

        private void Handness_IndexChanged(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            int Index = int.Parse(CBE.Name[1].ToString()) - 1;

            Biohazard.Players[Index].SetHandness(CBE.SelectedIndex != 0 ? new[] {(byte) (CBE.SelectedItem as ListItem).Value} : new[] {(byte) Biohazard.Players[Index].GetDefaultHandness()});
        }

        private void CharCosFreeze_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton CKBTN = sender as CheckButton;
            CKBTN.Text = CKBTN.Checked ? "Frozen" : "Freeze";

            int Index = int.Parse(CKBTN.Name[1].ToString()) - 1;

            if (!CharCosFreezeChangeReceived && CKBTN.Enabled)
                Character_SendFreezeChange(Index, CKBTN.Checked);

            if (!Initialized)
                return;

            Character_DetourUpdate();
            Character_ApplyCharacters(Index);
        }

        private void OnOff_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton CB = sender as CheckButton;

            if (CB.Checked && CB.Text.Contains("OFF"))
                CB.Text = CB.Text.Replace("OFF", "ON");

            if (!CB.Checked && CB.Text.Contains("ON"))
                CB.Text = CB.Text.Replace("ON", "OFF");

            if (!Initialized)
                return;

            if (CB.Name.Contains("Untargetable") && !CB.Checked)
            {
                int Player = int.Parse(CB.Name[1].ToString()) - 1;
                Biohazard.Players[Player].SetUntargetable(false);
            }
            else if (CB.Name.Contains("InfiniteAmmo") && !CB.Checked)
            {
                int Player = int.Parse(CB.Name[1].ToString()) - 1;
                Biohazard.Players[Player].SetInfiniteAmmo(false);
            }
            else if (CB.Name.Contains("Rapidfire") && !CB.Checked)
            {
                int Player = int.Parse(CB.Name[1].ToString()) - 1;
                Biohazard.Players[Player].SetRapidFire(false);
            }
        }

        private void OnOff_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;
            SB.Text = SB.Text == "ON" ? "OFF" : "ON";

            if (SB.Name.Equals("WeskerSunglassesButton"))
            {
                Biohazard.WeskerNoSunglassDrop(SB.Text.Equals("ON"));
            }
            else if (SB.Name.Equals("WeskerNoDashCostButton"))
            {
                Biohazard.WeskerNoDashCost(SB.Text.Equals("ON"));
            }
        }

        private void EnableDisable_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;
            SB.Text = SB.Text == "Enable" ? "Disable" : "Enable";

            switch (SB.Name)
            {
                case "ControllerAimButton":
                {
                    if (Initialized)
                        Biohazard.EnableControllerAim(SB.Text.Equals("Disable"));

                    break;
                }
                case "ColorFilterButton":
                {
                    if (Initialized)
                        Biohazard.EnableColorFilter(SB.Text.Equals("Disable"));

                    break;
                }
            }
        }

        private async void StartNetwork_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;

            object[] ToDisable =
            {
                BuddyServerConnectionButton,
                StartServerButton
            };

            SimpleButton[] DropButtons =
            {
                P1DropSimpleButton,
                P2DropSimpleButton,
                P3DropSimpleButton
            };

            LabelControl[] ClientNames =
            {
                P1ClientLabelControl,
                P2ClientLabelControl,
                P3ClientLabelControl
            };

            if (!Network.ModuleStarted)
            {
                SB.Enabled = false;
                SB.Text = "Enabling";

                await Task.Run(() => Network.StartModule(this));

                SB.Enabled = true;

                if (Network.ModuleStarted)
                {
                    foreach (dynamic Control in ToDisable)
                        Control.Enabled = true;

                    SB.Text = "Disable";
                    return;
                }

                Terminal.WriteLine("[App] The connection response was either null or too slow, please check your internet and try again.", Enums.MessageBoxType.Error);
                return;
            }

            Network.FinishModule();

            foreach (dynamic Control in ToDisable)
            {
                Control.Enabled = false;

                if (Control.Text == "Disconnect")
                    Control.Text = "Connect";
            }

            foreach (SimpleButton SDB in DropButtons)
                SDB.Enabled = false;

            foreach (LabelControl LC in ClientNames)
                LC.Text = "No client connected";

            SB.Text = "Enable";
        }

        #endregion

        #region GameX Calls

        private void GameX_CheckControls()
        {
            SimpleButton[] EnableDisable =
            {
                ControllerAimButton,
                ColorFilterButton,
                WeskerSunglassesButton,
                WeskerNoDashCostButton
            };

            foreach (SimpleButton SB in EnableDisable)
            {
                switch (SB.Name)
                {
                    case "ControllerAimButton":
                    {
                        if (SB.Text == "Disable")
                            Biohazard.EnableControllerAim(true);

                        break;
                    }
                    case "ColorFilterButton":
                    {
                        if (SB.Text == "Disable")
                            Biohazard.EnableColorFilter(true);

                        break;
                    }
                    case "WeskerSunglassesButton":
                    {
                        if (SB.Text == "ON")
                            Biohazard.WeskerNoSunglassDrop(true);

                        break;
                    }
                    case "WeskerNoDashCostButton":
                    {
                        if (SB.Text == "ON")
                            Biohazard.WeskerNoDashCost(true);

                        break;
                    }
                }
            }
        }

        private void GameX_Start()
        {
            try
            {
                Biohazard.StartModule();

                Character_Detour();
                RickFixes_Detour();

                GameX_CheckControls();

                Biohazard.NoFileChecking(true);
                Biohazard.OnlineCharSwapFixes(true);
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

                Biohazard.NoFileChecking(false);
                Biohazard.OnlineCharSwapFixes(false);
                Biohazard.EnableControllerAim(false);
                Biohazard.EnableColorFilter(false);
                Biohazard.WeskerNoSunglassDrop(false);
                Biohazard.WeskerNoDashCost(false);

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

                Character_Update();
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private static void GameX_Keyboard(int input)
        {
        }

        #endregion

        #region External

        public void CreatePrefabs(Enums.PrefabType Prefab, bool Override = false)
        {
            string prefabDir = Directory.GetCurrentDirectory() + "/addons/GameX.Biohazard.5/prefabs/";

            if (!Directory.Exists(prefabDir))
                Directory.CreateDirectory(prefabDir);

            string charDir = prefabDir + "character/";

            if (!Directory.Exists(charDir))
                Directory.CreateDirectory(charDir);

            string itemDir = prefabDir + "item/";

            if (!Directory.Exists(itemDir))
                Directory.CreateDirectory(itemDir);

            DirectoryInfo CharactersFolder = new DirectoryInfo(charDir);
            DirectoryInfo ItemsFolder = new DirectoryInfo(itemDir);

            FileInfo[] CharacterFiles = CharactersFolder.GetFiles("*.json");
            FileInfo[] ItemFiles = ItemsFolder.GetFiles("*.json");

            switch (Prefab)
            {
                case Enums.PrefabType.Character:
                    {
                        if (CharacterFiles.Length < 9 || Override)
                        {
                            Characters.WriteDefaultChars();
                        }

                        break;
                    }
                case Enums.PrefabType.Item:
                    {
                        if (ItemFiles.Length < 67 || Override)
                        {
                            Items.WriteDefaultItems();
                        }

                        break;
                    }
                case Enums.PrefabType.All:
                    {
                        if (CharacterFiles.Length < 9 || Override)
                        {
                            Characters.WriteDefaultChars();
                        }

                        if (ItemFiles.Length < 67 || Override)
                        {
                            Items.WriteDefaultItems();
                        }

                        break;
                    }
            }
        }

        #endregion

        #region Mods

        #region Character

        private void Character_Detour()
        {
            if (!Memory.DetourActive("Character_Global"))
            {
                byte[] DetourClean =
                {
                    0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x2D,
                    0x0F, 0x1F, 0x40, 0x00,
                    0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x30,
                    0x0F, 0x1F, 0x40, 0x00,
                    0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x33,
                    0x0F, 0x1F, 0x40, 0x00,
                    0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x36,
                    0x0F, 0x1F, 0x40, 0x00,
                    0xEB, 0x3F,
                    0x0F, 0x1F, 0x00,
                    0xBB, 0x00, 0x00, 0x00, 0x00,
                    0xBF, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x30,
                    0x0F, 0x1F, 0x00,
                    0xBB, 0x00, 0x00, 0x00, 0x00,
                    0xBF, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x21,
                    0x0F, 0x1F, 0x00,
                    0xBB, 0x00, 0x00, 0x00, 0x00,
                    0xBF, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x12,
                    0x0F, 0x1F, 0x00,
                    0xBB, 0x00, 0x00, 0x00, 0x00,
                    0xBF, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x03,
                    0x0F, 0x1F, 0x00,
                    0x89, 0x5E, 0x08,
                    0x89, 0x7E, 0x0C
                };

                byte[] CallInstruction =
                {
                    0x89, 0x5E, 0x08,
                    0x89, 0x7E, 0x0C
                };

                Detour Character_Global = Memory.CreateDetour("Character_Global", DetourClean, 0x00C91A88, CallInstruction, true, 0x00C91A8E);

                if (Character_Global == null)
                    return;
            }

            if (!Memory.DetourActive("Character_StoryChar"))
            {
                byte[] DetourClean =
                {
                    0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x15,
                    0x0F, 0x1F, 0x40, 0x00,
                    0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x13,
                    0x0F, 0x1F, 0x40, 0x00,
                    0xEB, 0x17,
                    0x0F, 0x1F, 0x00,
                    0xB9, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x0D,
                    0x0F, 0x1F, 0x00,
                    0xB9, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x03,
                    0x0F, 0x1F, 0x00,
                    0x89, 0x4E, 0x08,
                    0x8B, 0x17
                };

                byte[] CallInstruction =
                {
                    0x89, 0x4E, 0x08,
                    0x8B, 0x17
                };

                Detour Character_StoryChar = Memory.CreateDetour("Character_StoryChar", DetourClean, 0x00C9200D, CallInstruction, true, 0x00C92012);

                if (Character_StoryChar == null)
                    return;
            }

            if (!Memory.DetourActive("Character_StoryCos"))
            {
                byte[] DetourClean =
                {
                    0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x15,
                    0x0F, 0x1F, 0x40, 0x00,
                    0x81, 0xFE, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x13,
                    0x0F, 0x1F, 0x40, 0x00,
                    0xEB, 0x17,
                    0x0F, 0x1F, 0x00,
                    0xB9, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x0D,
                    0x0F, 0x1F, 0x00,
                    0xB9, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x03,
                    0x0F, 0x1F, 0x00,
                    0x89, 0x4E, 0x0C,
                    0x0F, 0xBF, 0x97, 0x64, 0x13, 0x00, 0x00
                };

                byte[] CallInstruction =
                {
                    0x89, 0x4E, 0x0C,
                    0x0F, 0xBF, 0X97, 0x64, 0x13, 0x00, 0x00
                };

                Detour Character_StoryCos = Memory.CreateDetour("Character_StoryCos", DetourClean, 0x00C9201D, CallInstruction, true, 0x00C92027);

                if (Character_StoryCos == null)
                    return;
            }

            if (!Memory.DetourActive("Character_StorySave"))
            {
                byte[] DetourClean =
                {
                    0x81, 0xFF, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x09,
                    0x0F, 0x1F, 0x40, 0x00,
                    0xEB, 0x3D,
                    0x0F, 0x1F, 0x00,
                    0xB0, 0x01,
                    0x3C, 0x00,
                    0x75, 0x17,
                    0x0F, 0x1F, 0x40, 0x00,
                    0xC7, 0x47, 0x08, 0x00, 0x00, 0x00, 0x00,
                    0xC7, 0x47, 0x0C, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x03,
                    0x0F, 0x1F, 0x00,
                    0xB0, 0x01,
                    0x3C, 0x00,
                    0x75, 0x17,
                    0x0F, 0x1F, 0x40, 0x00,
                    0xC7, 0x47, 0x58, 0x00, 0x00, 0x00, 0x00,
                    0xC7, 0x47, 0x5C, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x03,
                    0x0F, 0x1F, 0x00,
                    0x8D, 0xB6, 0x80, 0x00, 0x00, 0x00
                };

                byte[] CallInstruction =
                {
                    0x8D, 0xB6, 0x80, 0X00, 0X00, 0X00
                };

                Detour Character_StorySave = Memory.CreateDetour("Character_StorySave", DetourClean, 0x00E6E0BE, CallInstruction, true, 0x00E6E0C4);

                if (Character_StorySave == null)
                    return;
            }

            if (!Memory.DetourActive("Character_StoryCosPersistent"))
            {
                byte[] DetourClean =
                {
                    0x81, 0xFF, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x1D,
                    0x0F, 0x1F, 0x40, 0x00,
                    0x81, 0xFF, 0x00, 0x00, 0x00, 0x00,
                    0x74, 0x1B,
                    0x0F, 0x1F, 0x40, 0x00,
                    0x0F, 0xBE, 0x84, 0x2A, 0x70, 0xC9, 0x0C, 0x01,
                    0xEB, 0x17,
                    0x0F, 0x1F, 0x00,
                    0xB8, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x0D,
                    0x0F, 0x1F, 0x00,
                    0xB8, 0x00, 0x00, 0x00, 0x00,
                    0xEB, 0x03,
                    0x0F, 0x1F, 0x00
                };

                byte[] CallInstruction =
                {
                    0x0F, 0xBE, 0x84, 0x2A, 0x70, 0xC9, 0x0C, 0x01
                };

                Detour Character_StoryCosPersistent = Memory.CreateDetour("Character_StoryCosPersistent", DetourClean, 0x0072FAB5, CallInstruction, true, 0x0072FABD);

                if (Character_StoryCosPersistent == null)
                    return;
            }

            Character_DetourValueUpdate();
            Character_DetourUpdate();
        }

        private void Character_DetourUpdate()
        {
            if (!Memory.DetourActive("Character_Global") || !Memory.DetourActive("Character_StoryChar") || !Memory.DetourActive("Character_StoryCos") || !Memory.DetourActive("Character_StorySave") || !Memory.DetourActive("Character_StoryCosPersistent"))
                return;

            Detour DetourBase = Memory.GetDetour("Character_Global");
            int DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x2D});
            Memory.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x30});
            Memory.WriteRawAddress(DetourBase_Address + 30, !P3FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x33});
            Memory.WriteRawAddress(DetourBase_Address + 42, !P4FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x36});

            DetourBase = Memory.GetDetour("Character_StoryChar");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x15});
            Memory.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x13});

            DetourBase = Memory.GetDetour("Character_StoryCos");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x15});
            Memory.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x13});

            DetourBase = Memory.GetDetour("Character_StorySave");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 20, P1FreezeCharCosButton.Checked ? new byte[] {0x01} : new byte[] {0x00});
            Memory.WriteRawAddress(DetourBase_Address + 49, P2FreezeCharCosButton.Checked ? new byte[] {0x01} : new byte[] {0x00});

            DetourBase = Memory.GetDetour("Character_StoryCosPersistent");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 6, P1FreezeCharCosButton.Checked ? new byte[] {0x74, 0x1D} : new byte[] {0x90, 0x90});
            Memory.WriteRawAddress(DetourBase_Address + 18, P2FreezeCharCosButton.Checked ? new byte[] {0x74, 0x1B} : new byte[] {0x90, 0x90});
        }

        private void Character_DetourValueUpdate()
        {
            if (!Memory.DetourActive("Character_Global") || !Memory.DetourActive("Character_StoryChar") || !Memory.DetourActive("Character_StoryCos") || !Memory.DetourActive("Character_StorySave") || !Memory.DetourActive("Character_StoryCosPersistent"))
                return;

            int CHAR1A = Memory.ReadPointer("re5dx9.exe", 0xDA383C, 0x6FE00);
            int CHAR2A = Memory.ReadPointer("re5dx9.exe", 0xDA383C, 0x6FE50);
            int CHAR3A = Memory.ReadPointer("re5dx9.exe", 0xDA383C, 0x6FEA0);
            int CHAR4A = Memory.ReadPointer("re5dx9.exe", 0xDA383C, 0x6FEF0);

            byte[] Char1A = BitConverter.GetBytes(CHAR1A);
            byte[] Char2A = BitConverter.GetBytes(CHAR2A);
            byte[] Char3A = BitConverter.GetBytes(CHAR3A);
            byte[] Char4A = BitConverter.GetBytes(CHAR4A);

            int intCharacter1 = (P1CharComboBox.SelectedItem as Character).Value;
            int intCostume1 = (P1CosComboBox.SelectedItem as Costume).Value;
            int intCharacter2 = (P2CharComboBox.SelectedItem as Character).Value;
            int intCostume2 = (P2CosComboBox.SelectedItem as Costume).Value;
            int intCharacter3 = (P3CharComboBox.SelectedItem as Character).Value;
            int intCostume3 = (P3CosComboBox.SelectedItem as Costume).Value;
            int intCharacter4 = (P4CharComboBox.SelectedItem as Character).Value;
            int intCostume4 = (P4CosComboBox.SelectedItem as Costume).Value;

            byte[] Character1 = BitConverter.GetBytes(intCharacter1);
            byte[] Costume1 = BitConverter.GetBytes(intCostume1);
            byte[] Character2 = BitConverter.GetBytes(intCharacter2);
            byte[] Costume2 = BitConverter.GetBytes(intCostume2);
            byte[] Character3 = BitConverter.GetBytes(intCharacter3);
            byte[] Costume3 = BitConverter.GetBytes(intCostume3);
            byte[] Character4 = BitConverter.GetBytes(intCharacter4);
            byte[] Costume4 = BitConverter.GetBytes(intCostume4);

            Detour DetourBase = Memory.GetDetour("Character_Global");
            int DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Memory.WriteRawAddress(DetourBase_Address + 14, Char2A);
            Memory.WriteRawAddress(DetourBase_Address + 26, Char3A);
            Memory.WriteRawAddress(DetourBase_Address + 38, Char4A);

            Memory.WriteRawAddress(DetourBase_Address + 54, Character1);
            Memory.WriteRawAddress(DetourBase_Address + 59, Costume1);
            Memory.WriteRawAddress(DetourBase_Address + 69, Character2);
            Memory.WriteRawAddress(DetourBase_Address + 74, Costume2);
            Memory.WriteRawAddress(DetourBase_Address + 84, Character3);
            Memory.WriteRawAddress(DetourBase_Address + 89, Costume3);
            Memory.WriteRawAddress(DetourBase_Address + 99, Character4);
            Memory.WriteRawAddress(DetourBase_Address + 104, Costume4);

            DetourBase = Memory.GetDetour("Character_StoryChar");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Memory.WriteRawAddress(DetourBase_Address + 14, Char2A);

            Memory.WriteRawAddress(DetourBase_Address + 30, Character1);
            Memory.WriteRawAddress(DetourBase_Address + 40, Character2);

            DetourBase = Memory.GetDetour("Character_StoryCos");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Memory.WriteRawAddress(DetourBase_Address + 14, Char2A);

            Memory.WriteRawAddress(DetourBase_Address + 30, Costume1);
            Memory.WriteRawAddress(DetourBase_Address + 40, Costume2);

            DetourBase = Memory.GetDetour("Character_StorySave");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 2, Char1A);

            Memory.WriteRawAddress(DetourBase_Address + 30, Character1);
            Memory.WriteRawAddress(DetourBase_Address + 37, Costume1);
            Memory.WriteRawAddress(DetourBase_Address + 59, Character2);
            Memory.WriteRawAddress(DetourBase_Address + 66, Costume2);

            DetourBase = Memory.GetDetour("Character_StoryCosPersistent");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Memory.WriteRawAddress(DetourBase_Address + 14, Char2A);

            Memory.WriteRawAddress(DetourBase_Address + 38, Costume1);
            Memory.WriteRawAddress(DetourBase_Address + 48, Costume2);
        }

        private void Character_ApplyCharacters(int Index)
        {
            ComboBoxEdit[] CharacterCombos =
            {
                P1CharComboBox,
                P2CharComboBox,
                P3CharComboBox,
                P4CharComboBox
            };

            ComboBoxEdit[] CostumeCombos =
            {
                P1CosComboBox,
                P2CosComboBox,
                P3CosComboBox,
                P4CosComboBox
            };

            int Character = (CharacterCombos[Index].SelectedItem as Character).Value;
            int Costume = (CostumeCombos[Index].SelectedItem as Costume).Value;

            Biohazard.SetStoryModeCharacter(Index, Character, Costume);
            Biohazard.Players[Index].SetCharacter(Character, Costume);
        }

        // NET

        public void Character_SendSelectionChange(int index, int character, int costume)
        {
            if (!Network.ModuleStarted || Network._Server == null && Network._Client == null || Network._Server != null && Network._Server.ListClients().ToList().Count < 1)
                return;

            NetCharacterSelectionChange Change = new NetCharacterSelectionChange()
            {
                Index = index, Character = character, Costume = costume
            };

            string SerializedChange = $"[CHARSELECTIONCHANGE]{Serializer.SerializeCharacterSelectionChanged(Change)}";

            if (Network._Server != null)
            {
                Network.Server_BroadcastMessage(SerializedChange, "", false, true);
                return;
            }

            Network.Client_SendMessage(SerializedChange, false, true);
        }

        public void Character_ReceiveSelectionChange(NetCharacterSelectionChange Change, Client Client = null)
        {
            if (Network._Server != null && Client != null && Network._Server.ListClients().Where(x => x != Client.IP).ToList().Count > 0)
            {
                string SerializedChange = $"[CHARSELECTIONCHANGE]{Serializer.SerializeCharacterSelectionChanged(Change)}";
                Network.Server_BroadcastMessage(SerializedChange, Client.IP, false, true);
            }

            ComboBoxEdit[] CharacterCombos =
            {
                P1CharComboBox,
                P2CharComboBox,
                P3CharComboBox,
                P4CharComboBox
            };

            ComboBoxEdit[] CostumeCombos =
            {
                P1CosComboBox,
                P2CosComboBox,
                P3CosComboBox,
                P4CosComboBox
            };

            CharCosSelectionChangeReceived = true;

            Threading.SetControlPropertyThreadSafe(CharacterCombos[Change.Index], "SelectedIndex", Change.Character);
            Threading.SetControlPropertyThreadSafe(CostumeCombos[Change.Index], "SelectedIndex", Change.Costume);

            CharCosSelectionChangeReceived = false;
        }

        public void Character_SendFreezeChange(int index, bool freeze)
        {
            if (!Network.ModuleStarted || Network._Server == null && Network._Client == null || Network._Server != null && Network._Server.ListClients().ToList().Count < 1)
                return;

            NetCharacterFreezeChange Change = new NetCharacterFreezeChange()
            {
                Index = index,
                Freeze = freeze
            };

            string SerializedChange = $"[CHARFREEZECHANGE]{Serializer.SerializeCharacterFreezeChanged(Change)}";

            if (Network._Server != null)
            {
                Network.Server_BroadcastMessage(SerializedChange, "", false, true);
                return;
            }

            Network.Client_SendMessage(SerializedChange, false, true);
        }

        public void Character_ReceiveFreezeChange(NetCharacterFreezeChange Change, Client Client = null)
        {
            if (Network._Server != null && Client != null && Network._Server.ListClients().Where(x => x != Client.IP).ToList().Count > 0)
            {
                string SerializedChange = $"[CHARFREEZECHANGE]{Serializer.SerializeCharacterFreezeChanged(Change)}";
                Network.Server_BroadcastMessage(SerializedChange, Client.IP, false, true);
            }

            CheckButton[] CharCosFreezes =
            {
                P1FreezeCharCosButton,
                P2FreezeCharCosButton,
                P3FreezeCharCosButton,
                P4FreezeCharCosButton
            };

            CharCosFreezeChangeReceived = true;

            if (CharCosFreezes[Change.Index].Enabled)
                Threading.SetControlPropertyThreadSafe(CharCosFreezes[Change.Index], "Checked", Change.Freeze);

            CharCosFreezeChangeReceived = false;
        }

        #endregion

        #region Rick Fixes

        private void RickFixes_Detour()
        {
            if (!Memory.DetourActive("RickFixes_Movement_1"))
            {
                byte[] Function_A =
                {
                    0xF7, 0x84, 0x3A, 0xC8, 0x01, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00,
                    0x0F, 0x85, 0x00, 0x00, 0x00, 0x00,
                    0xF7, 0x84, 0x3A, 0xC4, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00
                };

                byte[] Function_A_Original =
                {
                    0xF7, 0x84, 0x17, 0xC4, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00
                };

                Detour RickFixes_Movement_1 = Memory.CreateDetour("RickFixes_Movement_1", Function_A, 0x0079F66D, Function_A_Original, true, 0x0079F678);

                if (RickFixes_Movement_1 != null)
                {
                    byte[] jne = new byte[6];
                    jne[0] = 0x0F;
                    jne[1] = 0x85;
                    Memory.DetourJump(RickFixes_Movement_1.Address() + 0x0B, 0x0079F67A, 6, 6, true).CopyTo(jne, 2);
                    Memory.WriteRawAddress(RickFixes_Movement_1.Address() + 0x0B, jne);
                }
            }

            if (!Memory.DetourActive("RickFixes_Movement_2"))
            {
                byte[] Function_A =
                {
                    0xF6, 0x84, 0x3A, 0xC8, 0x01, 0x00, 0x00, 0x02,
                    0x0F, 0x85, 0x00, 0x00, 0x00, 0x00,
                    0x84, 0x9C, 0x3A, 0xC4, 0x01, 0x00, 0x00
                };

                byte[] Function_A_Original =
                {
                    0x84, 0x9C, 0x17, 0xC4, 0x01, 0x00, 0x00
                };

                Detour RickFixes_Movement_2 = Memory.CreateDetour("RickFixes_Movement_2", Function_A, 0x0079F6C1, Function_A_Original, true, 0x0079F6C8);

                if (RickFixes_Movement_2 != null)
                {
                    byte[] jne = new byte[6];
                    jne[0] = 0x0F;
                    jne[1] = 0x85;
                    Memory.DetourJump(RickFixes_Movement_2.Address() + 0x08, 0x0079F6CA, 6, 6, true).CopyTo(jne, 2);
                    Memory.WriteRawAddress(RickFixes_Movement_2.Address() + 0x08, jne);
                }
            }

            if (!Memory.DetourActive("RickFixes_Movement_3"))
            {
                byte[] Function_A =
                {
                    0xF7, 0x84, 0x3A, 0xC8, 0x01, 0x00, 0x00, 0x00, 0X00, 0X80, 0X00,
                    0x0F, 0x85, 0x00, 0x00, 0x00, 0x00,
                    0xF6, 0x84, 0x3A, 0xC4, 0x01, 0x00, 0x00, 0X40
                };

                byte[] Function_A_Original =
                {
                    0xF6, 0x84, 0x17, 0xC4, 0x01, 0x00, 0x00, 0X40
                };

                Detour RickFixes_Movement_3 = Memory.CreateDetour("RickFixes_Movement_3", Function_A, 0x0079F698, Function_A_Original, true, 0x0079F6A0);

                if (RickFixes_Movement_3 != null)
                {
                    byte[] jne = new byte[6];
                    jne[0] = 0x0F;
                    jne[1] = 0x85;
                    Memory.DetourJump(RickFixes_Movement_3.Address() + 0x0B, 0x0079F6A2, 6, 6, true).CopyTo(jne, 2);
                    Memory.WriteRawAddress(RickFixes_Movement_3.Address() + 0x0B, jne);
                }
            }

            if (!Memory.DetourActive("RickFixes_Movement_4"))
            {
                byte[] Function_A =
                {
                    0xF6, 0x84, 0x3A, 0xC8, 0x01, 0x00, 0x00, 0x10,
                    0x0F, 0x85, 0x00, 0x00, 0x00, 0x00,
                    0xF6, 0x84, 0x3A, 0xC4, 0x01, 0x00, 0x00, 0X80
                };

                byte[] Function_A_Original =
                {
                    0xF6, 0x84, 0x17, 0xC4, 0x01, 0x00, 0x00, 0X80
                };

                Detour RickFixes_Movement_4 = Memory.CreateDetour("RickFixes_Movement_4", Function_A, 0x0079F6E9, Function_A_Original, true, 0x0079F6F1);

                if (RickFixes_Movement_4 != null)
                {
                    byte[] jne = new byte[6];
                    jne[0] = 0x0F;
                    jne[1] = 0x85;
                    Memory.DetourJump(RickFixes_Movement_4.Address() + 0x08, 0x0079F6F3, 6, 6, true).CopyTo(jne, 2);
                    Memory.WriteRawAddress(RickFixes_Movement_4.Address() + 0x08, jne);
                }
            }
        }

        #endregion

        #endregion

        #region General Read/Write

        private void Character_Update()
        {
            #region Controls

            ProgressBarControl[] HealthBars =
            {
                P1HealthBar,
                P2HealthBar,
                P3HealthBar,
                P4HealthBar
            };

            GroupControl[] PlayerGroupBoxes =
            {
                TabPageCharGPPlayer1,
                TabPageCharGPPlayer2,
                TabPageCharGPPlayer3,
                TabPageCharGPPlayer4
            };

            ComboBoxEdit[] CharacterCombos =
            {
                P1CharComboBox,
                P2CharComboBox,
                P3CharComboBox,
                P4CharComboBox
            };

            ComboBoxEdit[] CostumeCombos =
            {
                P1CosComboBox,
                P2CosComboBox,
                P3CosComboBox,
                P4CosComboBox
            };

            ComboBoxEdit[] Handness =
            {
                P1HandnessComboBox,
                P2HandnessComboBox,
                P3HandnessComboBox,
                P4HandnessComboBox
            };

            ComboBoxEdit[] WeaponMode =
            {
                P1WeaponModeComboBox,
                P2WeaponModeComboBox,
                P3WeaponModeComboBox,
                P4WeaponModeComboBox
            };

            CheckButton[] CheckButtons =
            {
                P1FreezeCharCosButton,
                P2FreezeCharCosButton,
                P3FreezeCharCosButton,
                P4FreezeCharCosButton
            };

            CheckButton[] InfiniteHP =
            {
                P1InfiniteHPButton,
                P2InfiniteHPButton,
                P3InfiniteHPButton,
                P4InfiniteHPButton
            };

            CheckButton[] Untargetable =
            {
                P1UntargetableButton,
                P2UntargetableButton,
                P3UntargetableButton,
                P4UntargetableButton
            };

            CheckButton[] InfiniteAmmo =
            {
                P1InfiniteAmmoButton,
                P2InfiniteAmmoButton,
                P3InfiniteAmmoButton,
                P4InfiniteAmmoButton
            };

            CheckButton[] RapidFire =
            {
                P1RapidfireButton,
                P2RapidfireButton,
                P3RapidfireButton,
                P4RapidfireButton
            };

            #endregion

            for (int i = 0; i < 4; i++)
            {
                // Characters & Costumes //
                if (!CheckButtons[i].Checked && !CharacterCombos[i].IsPopupOpen && !CostumeCombos[i].IsPopupOpen)
                {
                    Tuple<int, int> CharCos = Biohazard.Players[i].GetCharacter();

                    foreach (object Char in CharacterCombos[i].Properties.Items)
                    {
                        if ((Char as Character).Value == CharCos.Item1)
                            CharacterCombos[i].SelectedItem = Char;
                    }

                    foreach (object Cos in CostumeCombos[i].Properties.Items)
                    {
                        if ((Cos as Costume).Value == CharCos.Item2)
                            CostumeCombos[i].SelectedItem = Cos;
                    }
                }

                bool PlayerPresent = Biohazard.Players[i].IsActive();
                double PlayerHealthPercent = PlayerPresent ? (double)Biohazard.Players[i].GetHealth() / Biohazard.Players[i].GetMaxHealth() : 1.0;

                // Health Bar //
                HealthBars[i].Properties.Maximum = PlayerPresent ? Biohazard.Players[i].GetMaxHealth() : 1;
                HealthBars[i].EditValue = PlayerPresent ? Biohazard.Players[i].GetHealth() : 1;
                HealthBars[i].Properties.StartColor = PlayerPresent ? Color.FromArgb((int)(255.0 - 155.0 * PlayerHealthPercent), (int)(0.0 + 255.0 * PlayerHealthPercent), 0) : Color.FromArgb(0, 0, 0, 0);
                HealthBars[i].Properties.EndColor = PlayerPresent ? Color.FromArgb((int)(255.0 - 155.0 * PlayerHealthPercent), (int)(0.0 + 255.0 * PlayerHealthPercent), 0) : Color.FromArgb(0, 0, 0, 0);

                // Player Name //
                PlayerGroupBoxes[i].Text = $"Player {i + 1} - " + (Biohazard.InGame() ? i == Biohazard.LocalPlayer() ? Biohazard.LocalPlayerNick() : PlayerPresent ? Biohazard.Players[i].IsAI() ? "CPU AI" : "Connected" : "Disconnected" : "Disconnected");

                // Handness //
                if (Handness[i].SelectedIndex > 0 && PlayerPresent)
                    Biohazard.Players[i].SetHandness(new[] { (byte)(Handness[i].SelectedItem as ListItem).Value });

                // Weapon Mode //
                if (WeaponMode[i].SelectedIndex > 0 && PlayerPresent)
                    Biohazard.Players[i].SetWeaponMode(new[] { (byte)(WeaponMode[i].SelectedItem as ListItem).Value });

                // If versus then end the current iteration //
                if (Biohazard.GetActiveGameMode() == (int)GameEnums.GameMode.Versus)
                    continue;

                // Infinite HP //
                if (InfiniteHP[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetHealth(Biohazard.Players[i].GetMaxHealth());

                // Untergetable //
                if (Untargetable[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetUntargetable(true);

                // Infinite Ammo //
                if (InfiniteAmmo[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetInfiniteAmmo(true);

                // Rapidfire //
                if (RapidFire[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetRapidFire(true);

                // Infinite Dash //
                if (WeskerInfiniteDashButton.Text.Equals("ON") && PlayerPresent && Biohazard.Players[i].GetCharacter().Item1.Equals(3))
                    Biohazard.Players[i].ResetDash();

            }
        }

        #endregion
    }
}