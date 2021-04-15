using DevExpress.XtraEditors;
using GameX.Game.Base;
using GameX.Game.Content;
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
        /*App Properties*/

        private Messager Peaker { get; }
        public Memory Kernel { get; private set; }
        private Process pProcess { get; set; }
        private bool Initialized { get; set; }

        private string TargetProcess = "re5dx9";
        private string TargetVersion = "1.0.0.129";
        private string[] TargetModulesCheck = { "steam_api.dll", "maluc.dll" };

        private DateTime OpenTime { get; set; }
        public double FrameTime { get; private set; }
        public double CurTime { get; private set; }

        /*App Init*/

        public App()
        {
            InitializeComponent();
            Keyboard.CreateHook(GameX_Keyboard);

            Peaker = new Messager();
            OpenTime = DateTime.Now;

            Application.Idle += Application_Idle;
            Application.ApplicationExit += Application_ApplicationExit;
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
            if (pProcess != null)
            {
                if (Initialized)
                    GameX_End();

                pProcess.Exited += null;
                pProcess.EnableRaisingEvents = false;
            }

            Kernel?.Dispose();
            Keyboard.RemoveHook();
            Application.Idle += null;
        }

        /*App Methods*/

        private void HandleTime()
        {
            TimeSpan CurTimeSpan = DateTime.Now - OpenTime;
            FrameTime = CurTimeSpan.TotalSeconds - CurTime;
            CurTime = CurTimeSpan.TotalSeconds;
        }

        private bool HandleProcess()
        {
            if (pProcess == null)
            {
                pProcess = Processes.GetProcessByName(TargetProcess);
                Initialized = false;
                Text = "GameX - Resident Evil 5 / Biohazard 5 - Waiting";
            }
            else
            {
                if (Initialized)
                    return Initialized;

                if (!pProcess.Responding)
                {
                    Text = "GameX - Resident Evil 5 / Biohazard 5 - Validating process response";
                    return Initialized;
                }

                if (ValidateTarget(pProcess))
                {
                    pProcess.EnableRaisingEvents = true;
                    pProcess.Exited += ClearRuntime;
                    Kernel = new Memory(pProcess);
                    Initialized = true;
                    Text = "GameX - Resident Evil 5 / Biohazard 5 - " + (Kernel.DebugMode ? "Running in Admin Mode" : "Running in User Mode");
                    CheckDebugModeControls(Kernel.DebugMode);
                    GameX_Start();
                }
                else
                {
                    pProcess.Dispose();
                    pProcess = null;
                    Text = "GameX - Resident Evil 5 / Biohazard 5 - Incompatible Version";
                }
            }

            return Initialized;
        }

        private bool ValidateTarget(Process process)
        {
            try
            {
                if (TargetVersion != "" && (process.MainModule == null || !process.MainModule.FileVersionInfo.ToString().Contains(TargetVersion)))
                    return false;

                if (TargetModulesCheck.Length > 0)
                {
                    if (TargetModulesCheck.Any(Module => !Processes.ProcessHasModule(process, Module)))
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

        private void ClearRuntime(object sender, EventArgs e)
        {
            Kernel.Dispose();
            Kernel = null;
            pProcess.Dispose();
            pProcess = null;

            Initialized = false;
        }

        private void Think()
        {
            HandleTime();

            if (HandleProcess())
            {
                GameX_Update();
            }
        }

        /*Load and Events Handlers*/

        private void LoadControls()
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
                InfiniteHP[Index].CheckedChanged += InfiniteHP_CheckedChanged;

                CharacterCombos[Index].SelectedIndexChanged += CharComboBox_IndexChanged;
                CostumeCombos[Index].SelectedIndexChanged += CosComboBox_IndexChanged;

                CharacterCombos[Index].SelectedIndex = 0;
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
                CB.Enabled = DebugMode;
            }
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

            Character_ApplyCharacters(int.Parse(CKBTN.Name[1].ToString()) - 1);
        }

        private void InfiniteHP_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton CKBTN = sender as CheckButton;
            CKBTN.Text = "Infinite HP: " + (CKBTN.Checked ? "On" : "Off");
        }

        /*User Field*/

        private Master Game { get; set; }

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
                Character_DetourUpdate();
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

        /*Mod Sections*/

        #region Character

        private void Character_Detour()
        {
            if (!Kernel.DetourActive("MOD_CHAR_Extra"))
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

                Detour MOD_CHAR_Extra = Kernel.CreateDetour("MOD_CHAR_Extra", DetourClean, 0x00C91A88, CallInstruction, true, 0x00C91A8E);

                if (MOD_CHAR_Extra == null)
                    return;
            }

            if (!Kernel.DetourActive("MOD_CHAR_StoryCharFix"))
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

                Detour MOD_CHAR_StoryCharFix = Kernel.CreateDetour("MOD_CHAR_StoryCharFix", DetourClean, 0x00C9200D, CallInstruction, true, 0x00C92012);

                if (MOD_CHAR_StoryCharFix == null)
                    return;
            }

            if (!Kernel.DetourActive("MOD_CHAR_StoryCosFix"))
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

                Detour MOD_CHAR_StoryCosFix = Kernel.CreateDetour("MOD_CHAR_StoryCosFix", DetourClean, 0x00C9201D, CallInstruction, true, 0x00C92027);

                if (MOD_CHAR_StoryCosFix == null)
                    return;
            }

            if (!Kernel.DetourActive("MOD_CHAR_SaveBypass"))
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

                Detour MOD_CHAR_SaveBypass = Kernel.CreateDetour("MOD_CHAR_SaveBypass", DetourClean, 0x00E6E0BE, CallInstruction, true, 0x00E6E0C4);

                if (MOD_CHAR_SaveBypass == null)
                    return;
            }

            Character_DetourValueUpdate();
        }

        private void Character_DetourUpdate()
        {
            if (!Kernel.DetourActive("MOD_CHAR_Extra") || !Kernel.DetourActive("MOD_CHAR_StoryCharFix") || !Kernel.DetourActive("MOD_CHAR_StoryCosFix") || !Kernel.DetourActive("MOD_CHAR_SaveBypass"))
                return;

            Detour DetourBase = Kernel.GetDetour("MOD_CHAR_Extra");
            int DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x2D });
            Kernel.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x30 });
            Kernel.WriteRawAddress(DetourBase_Address + 30, !P3FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x33 });
            Kernel.WriteRawAddress(DetourBase_Address + 42, !P4FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x36 });

            DetourBase = Kernel.GetDetour("MOD_CHAR_StoryCharFix");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x15 });
            Kernel.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x13 });

            DetourBase = Kernel.GetDetour("MOD_CHAR_StoryCosFix");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 6, !P1FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x15 });
            Kernel.WriteRawAddress(DetourBase_Address + 18, !P2FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x13 });

            DetourBase = Kernel.GetDetour("MOD_CHAR_SaveBypass");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 20, P1FreezeCharCosButton.Checked ? new byte[] { 0x01 } : new byte[] { 0x00 });
            Kernel.WriteRawAddress(DetourBase_Address + 49, P2FreezeCharCosButton.Checked ? new byte[] { 0x01 } : new byte[] { 0x00 });
        }

        private void Character_DetourValueUpdate()
        {
            if (!Kernel.DetourActive("MOD_CHAR_Extra") || !Kernel.DetourActive("MOD_CHAR_StoryCharFix") || !Kernel.DetourActive("MOD_CHAR_StoryCosFix") || !Kernel.DetourActive("MOD_CHAR_SaveBypass"))
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

            Detour DetourBase = Kernel.GetDetour("MOD_CHAR_Extra");
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

            DetourBase = Kernel.GetDetour("MOD_CHAR_StoryCharFix");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Kernel.WriteRawAddress(DetourBase_Address + 14, Char2A);

            Kernel.WriteRawAddress(DetourBase_Address + 30, Character1);
            Kernel.WriteRawAddress(DetourBase_Address + 40, Character2);

            DetourBase = Kernel.GetDetour("MOD_CHAR_StoryCosFix");
            DetourBase_Address = DetourBase.Address();

            Kernel.WriteRawAddress(DetourBase_Address + 2, Char1A);
            Kernel.WriteRawAddress(DetourBase_Address + 14, Char2A);

            Kernel.WriteRawAddress(DetourBase_Address + 30, Costume1);
            Kernel.WriteRawAddress(DetourBase_Address + 40, Costume2);

            DetourBase = Kernel.GetDetour("MOD_CHAR_SaveBypass");
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

            #endregion

            int ActivePlayers = Game.ActivePlayers();

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

                // Health Bar //
                HealthBars[i].Properties.Maximum = (ActivePlayers - 1) >= i ? Game.Players[i].GetMaxHealth() : 1;
                HealthBars[i].EditValue = (ActivePlayers - 1) >= i ? Game.Players[i].GetHealth() : 0;

                // Player Name //
                PlayerGroupBoxes[i].Text = $"Player {i + 1} - " + (Game.InGame() ? ((i == Game.LocalPlayer()) ? Game.LocalPlayerNick() : ((ActivePlayers - 1) >= i ? (Game.Players[i].IsAI() ? "CPU AI" : "Connected") : "Disconnected")) : "Disconnected");

                if (Game.GetActiveGameMode() == "Versus")
                    continue;

                // Infinite HP //
                if (InfiniteHP[i].Checked && (ActivePlayers - 1) >= i)
                    Game.Players[i].SetHealth(Game.Players[i].GetMaxHealth());
            }
        }

        #endregion
    }
}
