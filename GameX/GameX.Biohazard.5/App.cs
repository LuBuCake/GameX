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
using GameX.Game.Modules;
using GameX.Game.Types;

namespace GameX
{
    public partial class App : XtraForm
    {
        /* App Init */

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

        private bool CharChangeReceived { get; set; }

        #endregion

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

            Target_Process?.Dispose();
            Target_Process = null;
            Memory.FinishModule();
            Network.FinishModule();
            Keyboard.RemoveHook();
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

            TimeSpan Elapsed = FrameElapser.Elapsed;

            if (Elapsed.TotalSeconds < UpdateMode)
                return;

            CurTime = CurTimeElapser.Elapsed.TotalSeconds;

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

        /* Target Processing */

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
                Target_Process = Processes.GetProcessByName(Target);
                Verified = false;
                Initialized = false;
                Text = "GameX - Resident Evil 5 / Biohazard 5 - Waiting game";

                if (Target_Process == null)
                    return Verified;

                Terminal.WriteLine("[App] Game found, validating.");
                Text = "GameX - Resident Evil 5 / Biohazard 5 - Validanting";

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
                Text = "GameX - Resident Evil 5 / Biohazard 5 - " +
                       (Memory.DebugMode ? "Running in Admin Mode" : "Running in User Mode");

                return Verified;
            }

            Terminal.WriteLine("[App] Failed validating, unsupported version.");
            Terminal.WriteLine(
                "[App] Follow the guide on https://steamcommunity.com/sharedfiles/filedetails/?id=864823595 to learn how to download and install the latest patch available.");

            Target_Process.EnableRaisingEvents = true;
            Target_Process.Exited += Target_Exited;
            Verified = true;
            Initialized = false;
            Text = "GameX - Resident Evil 5 / Biohazard 5 - Unsupported Version";

            return Verified;
        }

        private bool Target_Validate()
        {
            try
            {
                if (Target_Version != "" && (Target_Process.MainModule == null ||
                                             !Target_Process.MainModule.FileVersionInfo.ToString()
                                                 .Contains(Target_Version)))
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

            ResetHealthBars();

            Terminal.WriteLine("[App] Runtime cleared successfully.");
        }

        /* Control Methods */

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

                if (Index == 3)
                    continue;

                DropButtons[Index].Click += Network.Server_DropClient;
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

            ResetHealthBars();
            SetLogo();

            Terminal.WriteLine("[App] App initialized.");
            Configuration_Load(null, null);
        }

        private void SetLogo()
        {
            Color Window = CommonSkins.GetSkin(UserLookAndFeel.Default).TranslateColor(SystemColors.Window);
            int total = Window.R + Window.G + Window.B;

            Image LogoB = Utility.GetImageFromStream(@"GameX.Biohazard.5/Resources/Images/Application/logob.eia");
            Image LogoW = Utility.GetImageFromStream(@"GameX.Biohazard.5/Resources/Images/Application/logow.eia");

            if (total > 380 && LogoB != null)
            {
                AboutPictureEdit.Image = LogoB;
            }
            else if (total < 380 && LogoW != null)
            {
                AboutPictureEdit.Image = LogoW;
            }
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

        /* Event Handlers */

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
                Serializer.WriteDataFile(@"GameX.Biohazard.5/Settings.json", Serializer.SerializeSettings(Setts));
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                throw;
            }

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
                if (File.Exists(@"GameX.Biohazard.5/Settings.json"))
                    Setts = Serializer.DeserializeSettings(File.ReadAllText(@"GameX.Biohazard.5/Settings.json"));
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                throw;
            }

            UpdateModeComboBoxEdit.SelectedIndex = Setts.UpdateRate;
            PlayerNameTextEdit.Text = Setts.PlayerName;

            foreach (ListItem Pallete in PaletteComboBoxEdit.Properties.Items)
            {
                if (Pallete.Text == Setts.SkinName)
                    PaletteComboBoxEdit.SelectedItem = Pallete;
            }

            Terminal.WriteLine("[App] Settings Loaded.");
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

            Image Portrait = Utility.GetImageFromFile(CBECos.Portrait);

            if (Portrait != null)
                CharPicBoxes[Index].Image = Portrait;

            if (!CharChangeReceived)
            {
                Character_SendChange(Index, CharacterCombos[Index].SelectedIndex, CBE.SelectedIndex);
            }

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

            Biohazard.Players[Index].SetWeaponMode(CBE.SelectedIndex != 0
                ? new[] {(byte) (CBE.SelectedItem as ListItem).Value}
                : new[] {(byte) Biohazard.Players[Index].GetDefaultWeaponMode()});
        }

        private void Handness_IndexChanged(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            int Index = int.Parse(CBE.Name[1].ToString()) - 1;

            Biohazard.Players[Index].SetHandness(CBE.SelectedIndex != 0
                ? new[] {(byte) (CBE.SelectedItem as ListItem).Value}
                : new[] {(byte) Biohazard.Players[Index].GetDefaultHandness()});
        }

        private void CharCosFreeze_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton CKBTN = sender as CheckButton;
            CKBTN.Text = CKBTN.Checked ? "Frozen" : "Freeze";

            if (!Initialized)
                return;

            Character_DetourUpdate();
            Character_ApplyCharacters(int.Parse(CKBTN.Name[1].ToString()) - 1);
        }

        private void OnOff_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton CB = sender as CheckButton;

            if (CB.Checked && CB.Text.Contains("OFF"))
                CB.Text = CB.Text.Replace("OFF", "ON");

            if (!CB.Checked && CB.Text.Contains("ON"))
                CB.Text = CB.Text.Replace("ON", "OFF");

            if (Initialized && CB.Name.Contains("Untargetable") && !CB.Checked)
            {
                int Player = int.Parse(CB.Name[1].ToString()) - 1;
                Biohazard.Players[Player].SetUntargetable(false);
            }
            else if (Initialized && CB.Name.Contains("InfiniteAmmo") && !CB.Checked)
            {
                int Player = int.Parse(CB.Name[1].ToString()) - 1;
                Biohazard.Players[Player].SetInfiniteAmmo(false);
            }
            else if (Initialized && CB.Name.Contains("Rapidfire") && !CB.Checked)
            {
                int Player = int.Parse(CB.Name[1].ToString()) - 1;
                Biohazard.Players[Player].SetRapidFire(false);
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

                Terminal.WriteLine(
                    "[App] The connection response was either null or too slow, please check your internet and try again.",
                    Enums.MessageBoxType.Error);
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

        /* GameX Calls */

        private void GameX_Start()
        {
            try
            {
                Biohazard.StartModule();

                Character_Detour();
                RickFixes_Detour();

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

                CharacterPanel_Update();
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private static void GameX_Keyboard(int input)
        {
        }

        /* Mods */

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

                Detour Character_Global = Memory.CreateDetour("Character_Global", DetourClean, 0x00C91A88,
                    CallInstruction, true, 0x00C91A8E);

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

                Detour Character_StoryChar = Memory.CreateDetour("Character_StoryChar", DetourClean, 0x00C9200D,
                    CallInstruction, true, 0x00C92012);

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

                Detour Character_StoryCos = Memory.CreateDetour("Character_StoryCos", DetourClean, 0x00C9201D,
                    CallInstruction, true, 0x00C92027);

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

                Detour Character_StorySave = Memory.CreateDetour("Character_StorySave", DetourClean, 0x00E6E0BE,
                    CallInstruction, true, 0x00E6E0C4);

                if (Character_StorySave == null)
                    return;
            }

            Character_DetourValueUpdate();
        }

        private void Character_DetourUpdate()
        {
            if (!Memory.DetourActive("Character_Global") || !Memory.DetourActive("Character_StoryChar") ||
                !Memory.DetourActive("Character_StoryCos") || !Memory.DetourActive("Character_StorySave"))
                return;

            Detour DetourBase = Memory.GetDetour("Character_Global");
            int DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 6,
                !P1FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x2D});
            Memory.WriteRawAddress(DetourBase_Address + 18,
                !P2FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x30});
            Memory.WriteRawAddress(DetourBase_Address + 30,
                !P3FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x33});
            Memory.WriteRawAddress(DetourBase_Address + 42,
                !P4FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x36});

            DetourBase = Memory.GetDetour("Character_StoryChar");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 6,
                !P1FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x15});
            Memory.WriteRawAddress(DetourBase_Address + 18,
                !P2FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x13});

            DetourBase = Memory.GetDetour("Character_StoryCos");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 6,
                !P1FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x15});
            Memory.WriteRawAddress(DetourBase_Address + 18,
                !P2FreezeCharCosButton.Checked ? new byte[] {0x90, 0x90} : new byte[] {0x74, 0x13});

            DetourBase = Memory.GetDetour("Character_StorySave");
            DetourBase_Address = DetourBase.Address();

            Memory.WriteRawAddress(DetourBase_Address + 20,
                P1FreezeCharCosButton.Checked ? new byte[] {0x01} : new byte[] {0x00});
            Memory.WriteRawAddress(DetourBase_Address + 49,
                P2FreezeCharCosButton.Checked ? new byte[] {0x01} : new byte[] {0x00});
        }

        private void Character_DetourValueUpdate()
        {
            if (!Memory.DetourActive("Character_Global") || !Memory.DetourActive("Character_StoryChar") ||
                !Memory.DetourActive("Character_StoryCos") || !Memory.DetourActive("Character_StorySave"))
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

            Biohazard.Players[Index].SetCharacter((CharacterCombos[Index].SelectedItem as Character).Value,
                (CostumeCombos[Index].SelectedItem as Costume).Value);
        }

        // NET

        public void Character_SendChange(int index, int character, int costume)
        {
            if (!Network.ModuleStarted
                || Network.TCPServer == null && Network.TCPClient == null
                || Network.TCPServer != null && Network.TCPServer.ListClients().ToList().Count < 1)
                return;

            NetCharacterChange Change = new NetCharacterChange()
            {
                Index = index, Character = character, Costume = costume
            };

            string SerializedChange = $"[CHARCHANGE]{Serializer.SerializeCharacterChanged(Change)}";

            if (Network.TCPServer != null)
            {
                Network.Server_BroadcastMessage(SerializedChange, "", false, true);
                return;
            }

            Network.Client_SendMessage(SerializedChange, false, true);
        }

        public void Character_ReceiveChange(NetCharacterChange Change, Client Client = null)
        {
            if (Network.TCPServer != null && Client != null &&
                Network.TCPServer.ListClients().Where(x => x != Client.IP).ToList().Count > 0)
            {
                string SerializedChange = $"[CHARCHANGE]{Serializer.SerializeCharacterChanged(Change)}";
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

            CharChangeReceived = true;

            Threading.SetControlPropertyThreadSafe(CharacterCombos[Change.Index], "SelectedIndex", Change.Character);
            Threading.SetControlPropertyThreadSafe(CostumeCombos[Change.Index], "SelectedIndex", Change.Costume);

            CharChangeReceived = false;
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

                Detour RickFixes_Movement_1 = Memory.CreateDetour("RickFixes_Movement_1", Function_A, 0x0079F66D,
                    Function_A_Original, true, 0x0079F678);

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

                Detour RickFixes_Movement_2 = Memory.CreateDetour("RickFixes_Movement_2", Function_A, 0x0079F6C1,
                    Function_A_Original, true, 0x0079F6C8);

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

                Detour RickFixes_Movement_3 = Memory.CreateDetour("RickFixes_Movement_3", Function_A, 0x0079F698,
                    Function_A_Original, true, 0x0079F6A0);

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

                Detour RickFixes_Movement_4 = Memory.CreateDetour("RickFixes_Movement_4", Function_A, 0x0079F6E9,
                    Function_A_Original, true, 0x0079F6F1);

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

        /* General Read-Write */

        #region Character Panel

        private void CharacterPanel_Update()
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
                double PlayerHealthPercent = PlayerPresent
                    ? (double) Biohazard.Players[i].GetHealth() / Biohazard.Players[i].GetMaxHealth()
                    : 1.0;

                // Health Bar //
                HealthBars[i].Properties.Maximum = PlayerPresent ? Biohazard.Players[i].GetMaxHealth() : 1;
                HealthBars[i].EditValue = PlayerPresent ? Biohazard.Players[i].GetHealth() : 1;
                HealthBars[i].Properties.StartColor = PlayerPresent
                    ? Color.FromArgb((int) (255.0 - (155.0 * PlayerHealthPercent)),
                        (int) (0.0 + (255.0 * PlayerHealthPercent)), 0)
                    : Color.FromArgb(0, 0, 0, 0);
                HealthBars[i].Properties.EndColor = PlayerPresent
                    ? Color.FromArgb((int) (255.0 - (155.0 * PlayerHealthPercent)),
                        (int) (0.0 + (255.0 * PlayerHealthPercent)), 0)
                    : Color.FromArgb(0, 0, 0, 0);

                // Player Name //
                PlayerGroupBoxes[i].Text = $"Player {i + 1} - " + (Biohazard.InGame()
                    ? ((i == Biohazard.LocalPlayer())
                        ? Biohazard.LocalPlayerNick()
                        : (PlayerPresent ? (Biohazard.Players[i].IsAI() ? "CPU AI" : "Connected") : "Disconnected"))
                    : "Disconnected");

                // Handness //
                if (Handness[i].SelectedIndex > 0 && PlayerPresent)
                    Biohazard.Players[i].SetHandness(new[] {(byte) (Handness[i].SelectedItem as ListItem).Value});

                // Weapon Mode //
                if (WeaponMode[i].SelectedIndex > 0 && PlayerPresent)
                    Biohazard.Players[i].SetWeaponMode(new[] {(byte) (WeaponMode[i].SelectedItem as ListItem).Value});

                if (Biohazard.GetActiveGameMode() == "Versus")
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
            }
        }

        #endregion
    }
}