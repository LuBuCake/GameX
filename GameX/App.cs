using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using GameX.Content;
using GameX.Game.Content;
using GameX.Game.Modules;
using GameX.Game.Types;
using GameX.Helpers;
using GameX.Modules;
using GameX.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GameX
{
    public partial class App : XtraForm
    {
        /*App Init*/

        private Messager Peaker { get; set; }
        private Stopwatch FrameElapser { get; set; }
        private Stopwatch CurTimeElapser { get; set; }
        public double FrameTime { get; private set; }
        public double CurTime { get; private set; }
        public double UpdateMode { get; set; }
        public double FramesPerSecond { get; private set; }

        public App()
        {
            InitializeComponent();
            Keyboard.CreateHook(GameX_Keyboard);

            Peaker = new Messager();
            FrameElapser = new Stopwatch();
            CurTimeElapser = new Stopwatch();

            Application.Idle += Application_Idle;
            Application.ApplicationExit += Application_ApplicationExit;

            Terminal.LoadApp(this, ConsoleOutputMemoEdit, ConsoleInputTextEdit);
            ConsoleInputTextEdit.Validating += Terminal.ValidateInput;
        }

        private void App_Load(object sender, EventArgs e)
        {
            LoadControls();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            while (Peaker.IsApplicationIdle())
            {
                Think();
            }
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (TargetProcess != null)
            {
                if (Initialized)
                    GameX_End();

                TargetProcess.Exited += null;
                TargetProcess.EnableRaisingEvents = false;
            }

            Kernel?.Dispose();
            TargetProcess?.Dispose();
            Keyboard.RemoveHook();
            Application.Idle += null;
        }

        /*App Handlers*/

        private Process TargetProcess { get; set; }
        public Memory Kernel { get; private set; }
        public bool Verified { get; private set; }
        public bool Initialized { get; private set; }

        private string GameX_Target = "re5dx9";
        private string GameX_TargetVersion = "1.0.0.129";
        private string[] GameX_TargetModules = { "steam_api.dll", "maluc.dll" };

        private bool ValidateTarget(Process process)
        {
            try
            {
                if (GameX_TargetVersion != "" && (process.MainModule == null || !process.MainModule.FileVersionInfo.ToString().Contains(GameX_TargetVersion)))
                    return false;

                if (GameX_TargetModules.Length > 0)
                {
                    if (GameX_TargetModules.Any(Module => !Processes.ProcessHasModule(process, Module)))
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

        private bool HandleProcess()
        {
            if (TargetProcess == null)
            {
                TargetProcess = Processes.GetProcessByName(GameX_Target);
                Verified = false;
                Initialized = false;
                Text = "GameX - Resident Evil 5 / Biohazard 5 - Waiting game";

                if (TargetProcess != null)
                {
                    Terminal.WriteLine("Game found, validating.");
                    Text = "GameX - Resident Evil 5 / Biohazard 5 - Validanting";
                }
            }
            else
            {
                if (Verified || !TargetProcess.WaitForInputIdle())
                    return Verified;

                if (ValidateTarget(TargetProcess))
                {
                    Terminal.WriteLine("Game validated.");

                    TargetProcess.EnableRaisingEvents = true;
                    TargetProcess.Exited += Process_Exited;
                    Kernel = new Memory(TargetProcess);
                    CheckDebugModeControls(Kernel.DebugMode);
                    GameX_Start();
                    Verified = true;
                    Initialized = true;
                    Text = "GameX - Resident Evil 5 / Biohazard 5 - " + (Kernel.DebugMode ? "Running in Admin Mode" : "Running in User Mode");
                }
                else
                {
                    Terminal.WriteLine("Failed validating, unsupported version.");
                    Terminal.WriteLine("Follow the guide on https://steamcommunity.com/sharedfiles/filedetails/?id=864823595 to learn how to download and install the latest patch available.");

                    TargetProcess.EnableRaisingEvents = true;
                    TargetProcess.Exited += Process_Exited;
                    Verified = true;
                    Initialized = false;
                    Text = "GameX - Resident Evil 5 / Biohazard 5 - Unsupported Version";
                }
            }

            return Verified;
        }

        public void Process_Exited(object sender, EventArgs e)
        {
            Kernel?.Dispose();
            Kernel = null;
            TargetProcess?.Dispose();
            TargetProcess = null;
            Verified = false;
            Initialized = false;

            Terminal.WriteLine("Runtime cleared successfully.");
            Terminal.WriteLine("Waiting game.");
        }

        private void Think()
        {
            if (!CurTimeElapser.IsRunning)
                CurTimeElapser.Start();

            if (!FrameElapser.IsRunning)
                FrameElapser.Start();

            TimeSpan Elapsed = FrameElapser.Elapsed;

            if (Elapsed.TotalSeconds >= UpdateMode)
            {
                CurTime = CurTimeElapser.Elapsed.TotalSeconds;

                FrameElapser.Stop();
                FrameElapser.Reset();

                FramesPerSecond = 1.0 / Elapsed.TotalSeconds;
                FrameTime = 1.0 / FramesPerSecond;

                if (HandleProcess() && Initialized)
                    GameX_Update();
            }
        }

        /*Loading/Saving & Event Handling*/

        private void LoadControls()
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

            #endregion

            for (int Index = 0; Index < CharacterCombos.Length; Index++)
            {
                List<Character> AvailableChars = Characters.GetChars();
                CharacterCombos[Index].Properties.Items.Clear();

                foreach (Character Char in AvailableChars)
                {
                    Char.Index = Index;
                    CharacterCombos[Index].Properties.Items.Add(Char);
                }

                CharCosFreezes[Index].CheckedChanged += CharCosFreeze_CheckedChanged;
                InfiniteHP[Index].CheckedChanged += OnOff_CheckedChanged;
                Untargetable[Index].CheckedChanged += OnOff_CheckedChanged;

                CharacterCombos[Index].SelectedIndexChanged += CharComboBox_IndexChanged;
                CostumeCombos[Index].SelectedIndexChanged += CosComboBox_IndexChanged;

                CharacterCombos[Index].SelectedIndex = 0;
            }

            UpdateModeComboBoxEdit.Properties.Items.AddRange(Rates.Available());
            UpdateModeComboBoxEdit.SelectedIndexChanged += UpdateMode_IndexChanged;
            UpdateModeComboBoxEdit.SelectedIndex = 1;

            SkinComboBoxEdit.Properties.Items.AddRange(Skins.AllSkins());
            SkinComboBoxEdit.SelectedIndexChanged += Skin_IndexChanged;
            SkinComboBoxEdit.SelectedIndex = 0;

            PaletteComboBoxEdit.SelectedIndexChanged += Palette_IndexChanged;

            SaveSettingsButton.Click += Configuration_Save;
            LoadSettingsButton.Click += Configuration_Load;

            Terminal.WriteLine("App initialized.");
            Configuration_Load(null, null);
            Terminal.WriteLine("Waiting game.");
        }

        private void Configuration_Save(object sender, EventArgs e)
        {
            Properties.Settings.Default.FPSMode = UpdateModeComboBoxEdit.SelectedIndex;
            Properties.Settings.Default.NickName = NickNameTextEdit.Text;
            Properties.Settings.Default.Skin = UserLookAndFeel.Default.ActiveSkinName;
            Properties.Settings.Default.Pallete = UserLookAndFeel.Default.ActiveSvgPaletteName;
            Properties.Settings.Default.Save();

            Terminal.WriteLine("Settings saved.");
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            UpdateModeComboBoxEdit.SelectedIndex = Properties.Settings.Default.FPSMode;
            NickNameTextEdit.Text = Properties.Settings.Default.NickName;

            foreach (ListItem Skin in SkinComboBoxEdit.Properties.Items)
            {
                if (Skin.Text == Properties.Settings.Default.Skin)
                    SkinComboBoxEdit.SelectedItem = Skin;
            }

            if (PaletteComboBoxEdit.Enabled)
            {
                foreach (ListItem Pallete in PaletteComboBoxEdit.Properties.Items)
                {
                    if (Pallete.Text == Properties.Settings.Default.Pallete)
                        PaletteComboBoxEdit.SelectedItem = Pallete;
                }
            }

            UserLookAndFeel.Default.SetSkinStyle(Properties.Settings.Default.Skin, Properties.Settings.Default.Pallete);

            Terminal.WriteLine("Settings Loaded.");
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
                CB.Enabled = DebugMode;
            }
        }

        private void UpdateMode_IndexChanged(object sender, EventArgs e)
        {
            UpdateMode = 1.0 / (UpdateModeComboBoxEdit.SelectedItem as ListItem).Value;
        }

        private void Skin_IndexChanged(object sender, EventArgs e)
        {
            UserLookAndFeel.Default.SetSkinStyle(SkinComboBoxEdit.Text);

            PaletteComboBoxEdit.Properties.Items.Clear();

            string Skin = SkinComboBoxEdit.Text;

            if (Skin == "The Bezier" || Skin == "Basic" || Skin == "Office 2019 Colorful" || Skin == "Office 2019 Black" || Skin == "Office 2019 White")
            {
                ListItem[] AllPallets = Skins.AllPaletts(SkinComboBoxEdit.Text);

                PaletteComboBoxEdit.Enabled = true;
                PaletteComboBoxEdit.Properties.Items.AddRange(AllPallets);
                PaletteComboBoxEdit.SelectedIndex = 0;
            }
            else
            {
                PaletteComboBoxEdit.Text = "";
                PaletteComboBoxEdit.Enabled = false;
            }
        }

        private void Palette_IndexChanged(object sender, EventArgs e)
        {
            if (PaletteComboBoxEdit.Text == "")
                return;

            UserLookAndFeel.Default.SetSkinStyle(SkinComboBoxEdit.Text, PaletteComboBoxEdit.Text);
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

            CostumeCombos[CBEChar.Index].Properties.Items.Clear();

            foreach (Costume Cos in CBEChar.Costumes)
            {
                Cos.Index = CBEChar.Index;
                CostumeCombos[CBEChar.Index].Properties.Items.Add(Cos);
            }

            CostumeCombos[CBEChar.Index].SelectedIndex = 0;
        }

        private void CosComboBox_IndexChanged(object sender, EventArgs e)
        {
            PictureEdit[] CharPicBoxes =
            {
                P1CharPictureBox,
                P2CharPictureBox,
                P3CharPictureBox,
                P4CharPictureBox
            };

            ComboBoxEdit[] CharacterCombos =
            {
                P1CharComboBox,
                P2CharComboBox,
                P3CharComboBox,
                P4CharComboBox
            };

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            Costume CBECos = CBE.SelectedItem as Costume;

            Image Portrait = Characters.GetCharacterPortrait(CharacterCombos[CBECos.Index].Text + " " + CBE.Text);

            if (Portrait == null)
                return;

            CharPicBoxes[CBECos.Index].Image = Portrait;

            if (Initialized)
            {
                Character_ApplyCharacters(CBECos.Index);
                Character_DetourValueUpdate();
            }
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
                int Player = int.Parse(CB.Name[1].ToString());
                Game.Players[Player].SetUntargetable(false);
            }
        }

        /*GameX Calls*/

        public Master Game { get; private set; }

        private void GameX_Start()
        {
            Game = new Master(this);

            try
            {
                Character_Detour();
                RickFixes_Detour();

                Game.NoFileChecking(true);
                Game.OnlineCharSwapFixes(true);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void GameX_End()
        {
            try
            {
                Game.NoFileChecking(false);
                Game.OnlineCharSwapFixes(false);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void GameX_Update()
        {
            try
            {
                CharacterPanel_Update();
            }
            catch (Exception)
            {
                return;
            }
        }

        private static void GameX_Keyboard(int input)
        {

        }

        /*Mods*/

        #region Character

        private void Character_Detour()
        {
            if (!Kernel.DetourActive("Character_Global"))
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

                Detour Character_Global = Kernel.CreateDetour("Character_Global", DetourClean, 0x00C91A88, CallInstruction, true, 0x00C91A8E);

                if (Character_Global == null)
                    return;
            }

            if (!Kernel.DetourActive("Character_StoryChar"))
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

                Detour Character_StoryChar = Kernel.CreateDetour("Character_StoryChar", DetourClean, 0x00C9200D, CallInstruction, true, 0x00C92012);

                if (Character_StoryChar == null)
                    return;
            }

            if (!Kernel.DetourActive("Character_StoryCos"))
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

                Detour Character_StoryCos = Kernel.CreateDetour("Character_StoryCos", DetourClean, 0x00C9201D, CallInstruction, true, 0x00C92027);

                if (Character_StoryCos == null)
                    return;
            }

            if (!Kernel.DetourActive("Character_StorySave"))
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

                Detour Character_StorySave = Kernel.CreateDetour("Character_StorySave", DetourClean, 0x00E6E0BE, CallInstruction, true, 0x00E6E0C4);

                if (Character_StorySave == null)
                    return;
            }

            Character_DetourValueUpdate();
        }

        private void Character_DetourUpdate()
        {
            if (!Kernel.DetourActive("Character_Global") || !Kernel.DetourActive("Character_StoryChar") || !Kernel.DetourActive("Character_StoryCos") || !Kernel.DetourActive("Character_StorySave"))
                return;

            Detour DetourBase = Kernel.GetDetour("Character_Global");
            int DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x2D });
            Kernel.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x30 });
            Kernel.WriteRawAddress(DetourBase_Address + 30, !P3FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x33 });
            Kernel.WriteRawAddress(DetourBase_Address + 42, !P4FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x36 });

            DetourBase = Kernel.GetDetour("Character_StoryChar");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x15 });
            Kernel.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x13 });

            DetourBase = Kernel.GetDetour("Character_StoryCos");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x15 });
            Kernel.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x13 });

            DetourBase = Kernel.GetDetour("Character_StorySave");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 20, P1FreezeCharCosButton.Checked ? new byte[] { 0x01 } : new byte[] { 0x00 });
            Kernel.WriteRawAddress(DetourBase_Address + 49, P2FreezeCharCosButton.Checked ? new byte[] { 0x01 } : new byte[] { 0x00 });
        }

        private void Character_DetourValueUpdate()
        {
            if (!Kernel.DetourActive("Character_Global") || !Kernel.DetourActive("Character_StoryChar") || !Kernel.DetourActive("Character_StoryCos") || !Kernel.DetourActive("Character_StorySave"))
                return;

            int CHAR1A = Kernel.ReadPointer("re5dx9.exe", 0xDA383C, 0x6FE00);
            int CHAR2A = Kernel.ReadPointer("re5dx9.exe", 0xDA383C, 0x6FE50);
            int CHAR3A = Kernel.ReadPointer("re5dx9.exe", 0xDA383C, 0x6FEA0);
            int CHAR4A = Kernel.ReadPointer("re5dx9.exe", 0xDA383C, 0x6FEF0);

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

            Detour DetourBase = Kernel.GetDetour("Character_Global");
            int DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Kernel.WriteRawAddress(DetourBase_Address + 14, Char2A);
            Kernel.WriteRawAddress(DetourBase_Address + 26, Char3A);
            Kernel.WriteRawAddress(DetourBase_Address + 38, Char4A);

            Kernel.WriteRawAddress(DetourBase_Address + 54, Character1);
            Kernel.WriteRawAddress(DetourBase_Address + 59, Costume1);
            Kernel.WriteRawAddress(DetourBase_Address + 69, Character2);
            Kernel.WriteRawAddress(DetourBase_Address + 74, Costume2);
            Kernel.WriteRawAddress(DetourBase_Address + 84, Character3);
            Kernel.WriteRawAddress(DetourBase_Address + 89, Costume3);
            Kernel.WriteRawAddress(DetourBase_Address + 99, Character4);
            Kernel.WriteRawAddress(DetourBase_Address + 104, Costume4);

            DetourBase = Kernel.GetDetour("Character_StoryChar");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Kernel.WriteRawAddress(DetourBase_Address + 14, Char2A);

            Kernel.WriteRawAddress(DetourBase_Address + 30, Character1);
            Kernel.WriteRawAddress(DetourBase_Address + 40, Character2);

            DetourBase = Kernel.GetDetour("Character_StoryCos");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Kernel.WriteRawAddress(DetourBase_Address + 14, Char2A);

            Kernel.WriteRawAddress(DetourBase_Address + 30, Costume1);
            Kernel.WriteRawAddress(DetourBase_Address + 40, Costume2);

            DetourBase = Kernel.GetDetour("Character_StorySave");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 2, Char1A);

            Kernel.WriteRawAddress(DetourBase_Address + 30, Character1);
            Kernel.WriteRawAddress(DetourBase_Address + 37, Costume1);
            Kernel.WriteRawAddress(DetourBase_Address + 59, Character2);
            Kernel.WriteRawAddress(DetourBase_Address + 66, Costume2);
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

            Game.Players[Index].SetCharacter((CharacterCombos[Index].SelectedItem as Character).Value, (CostumeCombos[Index].SelectedItem as Costume).Value);
        }

        #endregion

        #region Rick Fixes

        private void RickFixes_Detour()
        {
            if (!Kernel.DetourActive("RickFixes_Movement_1"))
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

                Detour RickFixes_Movement_1 = Kernel.CreateDetour("RickFixes_Movement_1", Function_A, 0x0079F66D, Function_A_Original, true, 0x0079F678);

                if (RickFixes_Movement_1 != null)
                {
                    byte[] jne = new byte[6];
                    jne[0] = 0x0F;
                    jne[1] = 0x85;
                    Memory.DetourJump(RickFixes_Movement_1.Address() + 0x0B, 0x0079F67A, 6, 6, true).CopyTo(jne, 2);
                    Kernel.WriteRawAddress(RickFixes_Movement_1.Address() + 0x0B, jne);
                }
            }

            if (!Kernel.DetourActive("RickFixes_Movement_2"))
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

                Detour RickFixes_Movement_2 = Kernel.CreateDetour("RickFixes_Movement_2", Function_A, 0x0079F6C1, Function_A_Original, true, 0x0079F6C8);

                if (RickFixes_Movement_2 != null)
                {
                    byte[] jne = new byte[6];
                    jne[0] = 0x0F;
                    jne[1] = 0x85;
                    Memory.DetourJump(RickFixes_Movement_2.Address() + 0x08, 0x0079F6CA, 6, 6, true).CopyTo(jne, 2);
                    Kernel.WriteRawAddress(RickFixes_Movement_2.Address() + 0x08, jne);
                }
            }

            if (!Kernel.DetourActive("RickFixes_Movement_3"))
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

                Detour RickFixes_Movement_3 = Kernel.CreateDetour("RickFixes_Movement_3", Function_A, 0x0079F698, Function_A_Original, true, 0x0079F6A0);

                if (RickFixes_Movement_3 != null)
                {
                    byte[] jne = new byte[6];
                    jne[0] = 0x0F;
                    jne[1] = 0x85;
                    Memory.DetourJump(RickFixes_Movement_3.Address() + 0x0B, 0x0079F6A2, 6, 6, true).CopyTo(jne, 2);
                    Kernel.WriteRawAddress(RickFixes_Movement_3.Address() + 0x0B, jne);
                }
            }

            if (!Kernel.DetourActive("RickFixes_Movement_4"))
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

                Detour RickFixes_Movement_4 = Kernel.CreateDetour("RickFixes_Movement_4", Function_A, 0x0079F6E9, Function_A_Original, true, 0x0079F6F1);

                if (RickFixes_Movement_4 != null)
                {
                    byte[] jne = new byte[6];
                    jne[0] = 0x0F;
                    jne[1] = 0x85;
                    Memory.DetourJump(RickFixes_Movement_4.Address() + 0x08, 0x0079F6F3, 6, 6, true).CopyTo(jne, 2);
                    Kernel.WriteRawAddress(RickFixes_Movement_4.Address() + 0x08, jne);
                }
            }
        }

        #endregion

        /*General Read-Write*/

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

            #endregion

            for (int i = 0; i < 4; i++)
            {
                // Characters & Costumes //
                if (!CheckButtons[i].Checked && !CharacterCombos[i].IsPopupOpen && !CostumeCombos[i].IsPopupOpen)
                {
                    Tuple<int, int> CharCos = Game.Players[i].GetCharacter();

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

                bool PlayerPresent = Game.Players[i].IsActive();

                // Health Bar //
                HealthBars[i].Properties.Maximum = PlayerPresent ? Game.Players[i].GetMaxHealth() : 1;
                HealthBars[i].EditValue = PlayerPresent ? Game.Players[i].GetHealth() : 0;

                // Player Name //
                PlayerGroupBoxes[i].Text = $"Player {i + 1} - " + (Game.InGame() ? ((i == Game.LocalPlayer()) ? Game.LocalPlayerNick() : (PlayerPresent ? (Game.Players[i].IsAI() ? "CPU AI" : "Connected") : "Disconnected")) : "Disconnected");

                if (Game.GetActiveGameMode() == "Versus")
                    continue;

                // Infinite HP //
                if (InfiniteHP[i].Checked && PlayerPresent)
                    Game.Players[i].SetHealth(Game.Players[i].GetMaxHealth());

                // Untergetable //
                if (Untargetable[i].Checked && PlayerPresent)
                    Game.Players[i].SetUntargetable(true);
            }
        }

        #endregion
    }
}
