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
    public partial class App : DevExpress.XtraEditors.XtraForm
    {
        /*App Properties*/

        private Messager Peaker { get; }
        private Memory Kernel { get; set; }
        private Process pProcess { get; set; }
        private bool Initialized { get; set; }

        private string TargetProcess = "re5dx9";
        private string TargetVersion = "1.0.0.129";
        private readonly string[] TargetModulesCheck = { "steam_api.dll", "maluc.dll" };

        /*App Init*/

        public App()
        {
            InitializeComponent();
            Keyboard.CreateHook(KeyReader);

            Peaker = new Messager();

            Application.Idle += Application_Idle;
            Application.ApplicationExit += Application_ApplicationExit;
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
            Keyboard.RemoveHook();
            Application.Idle += null;
            Kernel?.Dispose();
        }

        private void App_Load(object sender, EventArgs e)
        {
            LoadControls();
        }

        private static void KeyReader(int input)
        {

        }

        /*App Methods*/

        private bool HandleProcess()
        {
            if (pProcess == null)
            {
                pProcess = Processes.GetProcessByName(TargetProcess);
                Initialized = false;
                Text = "GameX - Waiting";
            }
            else
            {
                if (Initialized)
                    return Initialized;

                if (!pProcess.Responding)
                {
                    Text = "GameX - Checking";
                    return Initialized;
                }

                if (ValidateTarget(pProcess))
                {
                    pProcess.EnableRaisingEvents = true;
                    pProcess.Exited += ClearRuntime;
                    Kernel = new Memory(pProcess);
                    Initialized = true;
                    Text = "GameX - Running";

                    GameX_Inject();
                }
                else
                {
                    pProcess.Dispose();
                    pProcess = null;
                    Text = "GameX - Incompatible Version";
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
            GameX_Deinject();

            Kernel.Dispose();
            Kernel = null;
            pProcess.Dispose();
            pProcess = null;
        }

        private void Think()
        {
            if (HandleProcess())
            {
                GameX_Update();
            }
        }

        /*Event Handlers*/

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

            for (int CBEIndex = 0; CBEIndex < CharacterCombos.Length; CBEIndex++)
            {
                List<Character> AvailableChars = Characters.GetChars();
                CharacterCombos[CBEIndex].Properties.Items.Clear();

                foreach (Character Char in AvailableChars)
                {
                    Char.Index = CBEIndex;
                    CharacterCombos[CBEIndex].Properties.Items.Add(Char);
                }

                CharCosFreezes[CBEIndex].CheckedChanged += CharCosFreeze_CheckedChanged;

                CharacterCombos[CBEIndex].SelectedIndexChanged += CharComboBox_IndexChanged;
                CostumeCombos[CBEIndex].SelectedIndexChanged += CosComboBox_IndexChanged;

                CharacterCombos[CBEIndex].SelectedIndex = 0;
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
                Character_InstructionValueUpdate();
            }
        }

        private void CharCosFreeze_CheckedChanged(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            CheckButton CKBTN = sender as CheckButton;

            if (!CKBTN.Checked)
                return;

            Character_ApplyCharacters(int.Parse(CKBTN.Name[1].ToString()) - 1);
        }

        /*User Field*/

        private RESIDENTEVIL5 RE5 { get; set; }

        private void GameX_Inject()
        {
            RE5 = new RESIDENTEVIL5(Kernel);

            Character_Inject();
        }

        private void GameX_Deinject()
        {
            RE5 = null;
        }

        private void GameX_Update()
        {
            Character_Update();
            Character_InstructionUpdate();
        }

        #region Character

        private void Character_Inject()
        {
            if (!Kernel.DetourActive("MOD_Char"))
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

                Detour Try = Kernel.CreateDetour("MOD_Char", DetourClean, 0x00C91A88, CallInstruction, true, 0x00C91A8E);

                if (Try == null)
                    return;
            }

            Character_InstructionValueUpdate();
        }

        private void Character_Update()
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

            CheckButton[] CheckButtons =
            {
                P1FreezeCharCosButton,
                P2FreezeCharCosButton,
                P3FreezeCharCosButton,
                P4FreezeCharCosButton
            };

            for (int i = 0; i < CheckButtons.Length; i++)
            {
                if (CheckButtons[i].Checked)
                    continue;

                Tuple<int, int> CharCos = RE5.GetCharacter(i);

                if (!CharacterCombos[i].IsPopupOpen)
                {
                    foreach (object Char in CharacterCombos[i].Properties.Items)
                    {
                        if ((Char as Character).Value == CharCos.Item1)
                            CharacterCombos[i].SelectedItem = Char;
                    }
                }

                if (!CostumeCombos[i].IsPopupOpen)
                {
                    foreach (object Cos in CostumeCombos[i].Properties.Items)
                    {
                        if ((Cos as Costume).Value == CharCos.Item2)
                            CostumeCombos[i].SelectedItem = Cos;
                    }
                }
            }
        }

        private void Character_InstructionUpdate()
        {
            if (!Kernel.DetourActive("MOD_Char"))
                return;

            Detour MOD_Char_Detour = Kernel.GetDetour("MOD_Char");
            int Base = MOD_Char_Detour.Address();

            Kernel.WriteRawAddress(Base + 6, !P1FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x28 });
            Kernel.WriteRawAddress(Base + 18, !P2FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x2B });
            Kernel.WriteRawAddress(Base + 30, !P3FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x3E });
            Kernel.WriteRawAddress(Base + 42, !P4FreezeCharCosButton.Checked ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x31 });
        }

        private void Character_InstructionValueUpdate()
        {
            if (!Kernel.DetourActive("MOD_Char"))
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

            Detour MOD_Char_Detour = Kernel.GetDetour("MOD_Char");
            int Base = MOD_Char_Detour.Address();

            Kernel.WriteRawAddress(Base + 2, Char1A);
            Kernel.WriteRawAddress(Base + 14, Char2A);
            Kernel.WriteRawAddress(Base + 26, Char3A);
            Kernel.WriteRawAddress(Base + 38, Char4A);

            Kernel.WriteRawAddress(Base + 54, Character1);
            Kernel.WriteRawAddress(Base + 59, Costume1);
            Kernel.WriteRawAddress(Base + 69, Character2);
            Kernel.WriteRawAddress(Base + 74, Costume2);
            Kernel.WriteRawAddress(Base + 84, Character3);
            Kernel.WriteRawAddress(Base + 89, Costume3);
            Kernel.WriteRawAddress(Base + 99, Character4);
            Kernel.WriteRawAddress(Base + 104, Costume4);
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

            RE5.SetCharacter(Index, (CharacterCombos[Index].SelectedItem as Character).Value, (CostumeCombos[Index].SelectedItem as Costume).Value);
        }

        #endregion
    }
}
