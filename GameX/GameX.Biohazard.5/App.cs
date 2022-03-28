﻿using System;
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
using GameX.Helpers;
using GameX.Modules;
using GameX.Database;
using GameX.Enum;
using GameX.Database.Type;
using System.Reflection;
using DevExpress.Data.Helpers;
using GameX.Database.ViewBag;

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
        public bool ExceptionStop { get; private set; }

        #endregion

        #region Indexer

        public object this[string PropName]
        {
            get
            {
                Type myType = typeof(App);
                PropertyInfo myPropInfo = myType.GetProperty(PropName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(App);
                PropertyInfo myPropInfo = myType.GetProperty(PropName);
                myPropInfo.SetValue(this, value, null);
            }
        }

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

            Biohazard.Setup(this);
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
            if (Target_Handle() && Initialized && !ExceptionStop)
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
                Text = "GameX - Resident Evil 5 - " + (Memory.DebugMode ? "Running as administrator" : "Running as normal user");

                return Verified;
            }

            Terminal.WriteLine("[App] Failed validating, unsupported version.");
            Terminal.WriteLine("[App] Follow the guide at https://steamcommunity.com/sharedfiles/filedetails/?id=864823595 to learn how to download and install the latest patch available.");

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
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
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

            SimpleButton[] EnableDisable =
            {
                ColorFilterButton
            };

            CheckEdit[] CheckUncheck =
            {
                WeskerGlassesCheckEdit,
                WeskerInfiniteDashCheckEdit,
                WeskerNoDashCostCheckEdit
            };

            ComboBoxEdit[] MeleeCombosA =
            {
                ReunionHeadFlashComboBox,
                ReunionLegFrontComboBox,
                FinisherFrontComboBox,
                FinisherBackComboBox,
                HeadFlashComboBox,
                ArmFrontComboBox,
                ArmBackComboBox,
                LegFrontComboBox,
                LegBackComboBox,
                HelpComboBox
            };

            LabelControl[] MeleeLabelsA =
            {
                ReunionHeadFlashLabelControl,
                ReunionLegFrontLabelControl,
                FinisherFrontLabelControl,
                FinisherBackLabelControl,
                HeadFlashLabelControl,
                ArmFrontLabelControl,
                ArmBackLabelControl,
                LegFrontLabelControl,
                LegBackLabelControl,
                HelpLabelControl
            };

            ComboBoxEdit[] MeleeCombosB =
            {
                TauntComboBox,
                KnifeComboBox,
                QuickTurnComboBox,
                MoveLeftComboBox,
                MoveRightComboBox,
                MoveBackComboBox,
                ReloadComboBox
            };

            LabelControl[] MeleeLabelsB =
            {
                TauntLabelControl,
                KnifeLabelControl,
                QuickTurnLabelControl,
                MoveLeftLabelControl,
                MoveRightLabelControl,
                MoveBackLabelControl,
                ReloadLabelControl
            };

            ComboBoxEdit[] ItemCombos =
            {
                P1Slot1ItemCB,
                P1Slot2ItemCB,
                P1Slot3ItemCB,
                P1Slot4ItemCB,
                P1Slot5ItemCB,
                P1Slot6ItemCB,
                P1Slot7ItemCB,
                P1Slot8ItemCB,
                P1Slot9ItemCB,
                P1SlotKnifeItemCB
            };

            TextEdit[] QuantityTextEdits =
            {
                P1Slot1QuantityTE,
                P1Slot2QuantityTE,
                P1Slot3QuantityTE,
                P1Slot4QuantityTE,
                P1Slot5QuantityTE,
                P1Slot6QuantityTE,
                P1Slot7QuantityTE,
                P1Slot8QuantityTE,
                P1Slot9QuantityTE,
                P1SlotKnifeQuantityTE
            };

            TextEdit[] MaxQuantityTextEdits =
            {
                P1Slot1MaxQuantityTE,
                P1Slot2MaxQuantityTE,
                P1Slot3MaxQuantityTE,
                P1Slot4MaxQuantityTE,
                P1Slot5MaxQuantityTE,
                P1Slot6MaxQuantityTE,
                P1Slot7MaxQuantityTE,
                P1Slot8MaxQuantityTE,
                P1Slot9MaxQuantityTE,
                P1SlotKnifeMaxQuantityTE
            };

            SpinEdit[] FirepowerSpins =
            {
                P1Slot1FirepowerSE,
                P1Slot2FirepowerSE,
                P1Slot3FirepowerSE,
                P1Slot4FirepowerSE,
                P1Slot5FirepowerSE,
                P1Slot6FirepowerSE,
                P1Slot7FirepowerSE,
                P1Slot8FirepowerSE,
                P1Slot9FirepowerSE,
                P1SlotKnifeFirepowerSE
            };

            SpinEdit[] ReloadSpeedSpins =
            {
                P1Slot1ReloadSpeedSE,
                P1Slot2ReloadSpeedSE,
                P1Slot3ReloadSpeedSE,
                P1Slot4ReloadSpeedSE,
                P1Slot5ReloadSpeedSE,
                P1Slot6ReloadSpeedSE,
                P1Slot7ReloadSpeedSE,
                P1Slot8ReloadSpeedSE,
                P1Slot9ReloadSpeedSE,
                P1SlotKnifeReloadSpeedSE
            };

            SpinEdit[] CapacitySpins =
            {
                P1Slot1CapacitySE,
                P1Slot2CapacitySE,
                P1Slot3CapacitySE,
                P1Slot4CapacitySE,
                P1Slot5CapacitySE,
                P1Slot6CapacitySE,
                P1Slot7CapacitySE,
                P1Slot8CapacitySE,
                P1Slot9CapacitySE,
                P1SlotKnifeCapacitySE
            };

            SpinEdit[] CriticalSpins =
            {
                P1Slot1CriticalSE,
                P1Slot2CriticalSE,
                P1Slot3CriticalSE,
                P1Slot4CriticalSE,
                P1Slot5CriticalSE,
                P1Slot6CriticalSE,
                P1Slot7CriticalSE,
                P1Slot8CriticalSE,
                P1Slot9CriticalSE,
                P1SlotKnifeCriticalSE
            };

            SpinEdit[] PiercingSpins =
            {
                P1Slot1PiercingSE,
                P1Slot2PiercingSE,
                P1Slot3PiercingSE,
                P1Slot4PiercingSE,
                P1Slot5PiercingSE,
                P1Slot6PiercingSE,
                P1Slot7PiercingSE,
                P1Slot8PiercingSE,
                P1Slot9PiercingSE,
                P1SlotKnifePiercingSE
            };

            SpinEdit[] RangeSpins =
            {
                P1Slot1RangeSE,
                P1Slot2RangeSE,
                P1Slot3RangeSE,
                P1Slot4RangeSE,
                P1Slot5RangeSE,
                P1Slot6RangeSE,
                P1Slot7RangeSE,
                P1Slot8RangeSE,
                P1Slot9RangeSE,
                P1SlotKnifeRangeSE
            };

            SpinEdit[] ScopeSpins =
            {
                P1Slot1ScopeSE,
                P1Slot2ScopeSE,
                P1Slot3ScopeSE,
                P1Slot4ScopeSE,
                P1Slot5ScopeSE,
                P1Slot6ScopeSE,
                P1Slot7ScopeSE,
                P1Slot8ScopeSE,
                P1Slot9ScopeSE,
                P1SlotKnifeScopeSE
            };

            CheckEdit[] InfiniteAmmoChecks =
            {
                P1Slot1InfiniteAmmoCheckEdit,
                P1Slot2InfiniteAmmoCheckEdit,
                P1Slot3InfiniteAmmoCheckEdit,
                P1Slot4InfiniteAmmoCheckEdit,
                P1Slot5InfiniteAmmoCheckEdit,
                P1Slot6InfiniteAmmoCheckEdit,
                P1Slot7InfiniteAmmoCheckEdit,
                P1Slot8InfiniteAmmoCheckEdit,
                P1Slot9InfiniteAmmoCheckEdit,
                P1SlotKnifeInfiniteAmmoCheckEdit
            };

            CheckEdit[] RapidfireChecks =
            {
                P1Slot1RapidFireCheckEdit,
                P1Slot2RapidFireCheckEdit,
                P1Slot3RapidFireCheckEdit,
                P1Slot4RapidFireCheckEdit,
                P1Slot5RapidFireCheckEdit,
                P1Slot6RapidFireCheckEdit,
                P1Slot7RapidFireCheckEdit,
                P1Slot8RapidFireCheckEdit,
                P1Slot9RapidFireCheckEdit,
                P1SlotKnifeRapidFireCheckEdit
            };

            CheckEdit[] FrozenChecks =
            {
                P1Slot1FrozenCheckEdit,
                P1Slot2FrozenCheckEdit,
                P1Slot3FrozenCheckEdit,
                P1Slot4FrozenCheckEdit,
                P1Slot5FrozenCheckEdit,
                P1Slot6FrozenCheckEdit,
                P1Slot7FrozenCheckEdit,
                P1Slot8FrozenCheckEdit,
                P1Slot9FrozenCheckEdit,
                P1SlotKnifeFrozenCheckEdit
            };

            #endregion

            DB db = DBContext.GetDatabase();

            for (int Index = 0; Index < CharacterCombos.Length; Index++)
            {
                CharacterCombos[Index].Properties.Items.AddRange(db.Characters);
                WeaponMode[Index].Properties.Items.AddRange(db.WeaponMode);
                Handness[Index].Properties.Items.AddRange(db.Handness);

                CharCosFreezes[Index].CheckedChanged += CharCosFreeze_CheckedChanged;
                InfiniteHP[Index].CheckedChanged += EnableDisable_StateChanged;
                Untargetable[Index].CheckedChanged += EnableDisable_StateChanged;
                InfiniteAmmo[Index].CheckedChanged += EnableDisable_StateChanged;
                Rapidfire[Index].CheckedChanged += EnableDisable_StateChanged;

                CharacterCombos[Index].SelectedIndexChanged += CharComboBox_IndexChanged;
                CostumeCombos[Index].SelectedIndexChanged += CosComboBox_IndexChanged;
                WeaponMode[Index].SelectedIndexChanged += WeaponMode_IndexChanged;
                Handness[Index].SelectedIndexChanged += Handness_IndexChanged;

                CharacterCombos[Index].SelectedIndex = 0;
                WeaponMode[Index].SelectedIndex = 0;
                Handness[Index].SelectedIndex = 0;

                if (Index < EnableDisable.Length)
                {
                    EnableDisable[Index].Click += EnableDisable_StateChanged;
                }

                if (Index == 3)
                    continue;

                CheckUncheck[Index].CheckedChanged += EnableDisable_StateChanged;
            }

            for (int i = 0; i < MeleeCombosA.Length; i++)
            {
                List<Move> Damage = db.DamageMoves;

                if (i > 1)
                {
                    Damage.RemoveAll(item => item.Value == 172);
                    Damage.RemoveAll(item => item.Value == 173);
                }

                MeleeCombosA[i].Properties.Items.AddRange(Damage);
                MeleeCombosA[i].SelectedIndexChanged += Melee_IndexChanged;

                foreach(object item in MeleeCombosA[i].Properties.Items)
                {
                    if ((item as Move).Name == MeleeLabelsA[i].Text.Replace(":", ""))
                        MeleeCombosA[i].SelectedItem = item;
                }
            }

            for (int i = 0; i < MeleeCombosB.Length; i++)
            {
                List<Move> Damage = db.DamageMoves;
                List<Move> Movement = db.MovementMoves;
                List<Move> Action = db.ActionMoves;
                List<Move> Dash = db.DashMoves;

                Damage.RemoveAll(item => item.Value == 172);
                Damage.RemoveAll(item => item.Value == 173);

                foreach (Move move in Movement)
                    if (move.Name == MeleeLabelsB[i].Text.Replace(":", ""))
                        MeleeCombosB[i].Properties.Items.Add(move);

                foreach (Move move in Action)
                    if (move.Name == MeleeLabelsB[i].Text.Replace(":", ""))
                        MeleeCombosB[i].Properties.Items.Add(move);

                MeleeCombosB[i].Properties.Items.AddRange(Damage);
                MeleeCombosB[i].Properties.Items.AddRange(Dash);

                MeleeCombosB[i].SelectedIndexChanged += Melee_IndexChanged;

                foreach (object item in MeleeCombosB[i].Properties.Items)
                {
                    if ((item as Move).Name == MeleeLabelsB[i].Text.Replace(":", ""))
                        MeleeCombosB[i].SelectedItem = item;
                }

                MeleeCombosB[i].Enabled = false;
            }

            for (int i = 0; i < 10; i++)
            {
                ItemCombos[i].Properties.Items.AddRange(db.Items);
                ItemCombos[i].SelectedIndexChanged += InventoryComboBox_IndexChanged;
                ItemCombos[i].SelectedIndex = 0;

                QuantityTextEdits[i].EditValueChanging += InventoryTextEditNumeric_EditValueChanging;
                QuantityTextEdits[i].Text = "0";
                MaxQuantityTextEdits[i].EditValueChanging += InventoryTextEditNumeric_EditValueChanging;
                MaxQuantityTextEdits[i].Text = "0";

                FirepowerSpins[i].Spin += SpinEdit_Spin;
                ReloadSpeedSpins[i].Spin += SpinEdit_Spin;
                CapacitySpins[i].Spin += SpinEdit_Spin;
                CriticalSpins[i].Spin += SpinEdit_Spin;
                PiercingSpins[i].Spin += SpinEdit_Spin;
                RangeSpins[i].Spin += SpinEdit_Spin;
                ScopeSpins[i].Spin += SpinEdit_Spin;
            }

            P1FreezeAllCheckEdit.CheckedChanged += EnableDisable_StateChanged;

            MeleeAnytimeSwitch.Toggled += ToggleSwitch_Toggled;

            if (MeleeAnytimeSwitch.IsOn)
                MeleeAnytimeSwitch.Toggle();

            UpdateModeComboBoxEdit.Properties.Items.AddRange(db.Rates);
            UpdateModeComboBoxEdit.SelectedIndexChanged += UpdateMode_IndexChanged;
            UpdateModeComboBoxEdit.SelectedIndex = 1;

            PaletteComboBoxEdit.Properties.Items.AddRange(db.Palettes);
            PaletteComboBoxEdit.SelectedIndexChanged += Palette_IndexChanged;
            PaletteComboBoxEdit.SelectedIndex = 0;

            SaveSettingsButton.Click += Configuration_Save;
            LoadSettingsButton.Click += Configuration_Load;

            MasterTabControl.SelectedPageChanged += MasterTabPage_PageChanged;
            MasterTabPage_PageChanged(MasterTabControl, null);

            WeaponPlacementComboBox.Properties.Items.AddRange(db.WeaponPlacement);
            WeaponPlacementComboBox.SelectedIndexChanged += WeaponPlacement_IndexChanged;
            WeaponPlacementComboBox.SelectedIndex = 0;

            MeleeCameraCheckEdit.CheckedChanged += EnableDisable_StateChanged;
            ReunionSpecialMovesCheckEdit.CheckedChanged += EnableDisable_StateChanged;

            ConsoleInputTextEdit.Validating += Terminal.ValidateInput;
            ClearConsoleSimpleButton.Click += Terminal.ClearConsole_Click;

            InventoryPlayerSelectionRG.EditValueChanged += RadioGroup_EditValueChanged;

            LoadoutExportButton.Click += SimpleButton_Click;
            LoadoutImportButton.Click += SimpleButton_Click;

            ResetHealthBars();
            SetupImages();

            Terminal.WriteLine("[App] App initialized.");
            Configuration_Load(null, null);
        }

        private void SetupImages()
        {
            Color Window = CommonSkins.GetSkin(UserLookAndFeel.Default).TranslateColor(SystemColors.Window);
            int total = Window.R + Window.G + Window.B;

            Addon Game = new Addon()
            {
                Images = new[] { "addons/GameX.Biohazard.5/images/application/logo_a.png", "addons/GameX.Biohazard.5/images/application/logo_b.png" },
                ImageColors = new[] { Color.Red, Color.White },
            };

            Image LogoA = Image.FromFile(Game.Images[0]);
            Image LogoB = Image.FromFile(Game.Images[1]);

            if (LogoA == null || LogoB == null)
                return;

            LogoA = LogoA.ColorReplace(Game.ImageColors[0], true);
            LogoB = LogoB.ColorReplace(total > 380 ? Color.Black : Color.White, true);

            Image Logo = Utility.MergeImage(LogoA, LogoB);
            AboutPictureEdit.Image = Logo;

            Image InventoryBG = Image.FromFile("addons/GameX.Biohazard.5/images/inventory/background.png");
            InventoryBGPictureBox.Image = InventoryBG;
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
        }

        public object GetInventoryControl(int Player, int Slot, string ControlName)
        {
            switch (Player)
            {
                case 0:
                    return ((GroupControl)P1InventoryTab.Controls[Slot != 9 ? $"P1Slot{Slot + 1}EditGP" : "P1SlotKnifeEditGP"]).Controls[ControlName];
                case 1:
                    return ((GroupControl)P1InventoryTab.Controls[Slot != 9 ? $"P1Slot{Slot + 1}EditGP" : "P1SlotKnifeEditGP"]).Controls[ControlName];
                case 2:
                    return ((GroupControl)P1InventoryTab.Controls[Slot != 9 ? $"P1Slot{Slot + 1}EditGP" : "P1SlotKnifeEditGP"]).Controls[ControlName];
                case 3:
                    return ((GroupControl)P1InventoryTab.Controls[Slot != 9 ? $"P1Slot{Slot + 1}EditGP" : "P1SlotKnifeEditGP"]).Controls[ControlName];
            }

            return null;
        }

        #endregion

        #region Event Handlers

        // Form //

        private void MasterTabPage_PageChanged(object sender, EventArgs e)
        {
            XtraTabControl XTC = sender as XtraTabControl;
            XtraTabPage XTP = XTC.SelectedTabPage;

            if (XTP.Name == "TabPageInventory")
            {
                Width = 1250;
                Height = 523;
                MasterTabControl.Width = 1225;
                MasterTabControl.Height = 467;
                TabInventoryGP.Width = 1206;
                TabInventoryGP.Height = 418;
            }
            else
            {
                Width = 664;
                Height = 523;
                MasterTabControl.Width = 640;
                MasterTabControl.Height = 467;
                TabInventoryGP.Width = 622;
                TabInventoryGP.Height = 418;
            }

            if (XTP.Name == "TabPageConsole")
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
                Serializer.WriteDataFile(@"addons/GameX.Biohazard.5/appsettings.json", Serializer.Serialize(Setts));
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
                if (File.Exists(@"addons/GameX.Biohazard.5/appsettings.json"))
                    Setts = Serializer.Deserialize<Settings>(File.ReadAllText(@"addons/GameX.Biohazard.5/appsettings.json"));
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                throw;
            }

            UpdateModeComboBoxEdit.SelectedIndex = Utility.Clamp(Setts.UpdateRate, 0, 2);

            foreach (Simple Pallete in PaletteComboBoxEdit.Properties.Items)
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
            UpdateMode = 1.0 / (UpdateModeComboBoxEdit.SelectedItem as Simple).Value;
        }

        private void Palette_IndexChanged(object sender, EventArgs e)
        {
            if (PaletteComboBoxEdit.Text == "")
                return;

            UserLookAndFeel.Default.SetSkinStyle(UserLookAndFeel.Default.ActiveSkinName, PaletteComboBoxEdit.Text);

            ResetHealthBars();
            SetupImages();
        }

        // Functionalities //

        private void SimpleButton_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;

            if (SB.Name.Equals("LoadoutExportButton"))
            {
                using (SaveFileDialog SFD = new SaveFileDialog())
                {
                    SFD.Filter = "Loadout Json (*.json)|*.json";
                    SFD.Title = "Export Loadout";
                    SFD.RestoreDirectory = true;

                    if (SFD.ShowDialog() == DialogResult.OK)
                    {
                        int SelectedPlayer = (int)InventoryPlayerSelectionRG.EditValue;

                        LoadoutViewBag Loadout = new LoadoutViewBag();
                        Loadout.Name = Path.GetFileNameWithoutExtension(SFD.FileName);
                        Loadout.Slots = new TemporaryItemViewBag[10];

                        for (int i = 0; i < 10; i++)
                            Loadout.Slots[i] = Biohazard.Players[SelectedPlayer].Inventory.Slots[i].ToMemory;

                        Serializer.WriteDataFile(SFD.FileName, Serializer.Serialize(Loadout));

                        Terminal.WriteLine($"[App] Loadout \"{Loadout.Name}\" saved successfully at {SFD.FileName}.");
                        Utility.MessageBox_Information($"Loadout \"{Loadout.Name}\" saved successfully!");
                    }
                }
            }
            else if (SB.Name.Equals("LoadoutImportButton"))
            {
                using (OpenFileDialog OFD = new OpenFileDialog())
                {
                    OFD.Filter = "Loadout Json (*.json)|*.json";
                    OFD.Title = "Import Loadout";
                    OFD.RestoreDirectory = true;

                    if (OFD.ShowDialog() == DialogResult.OK)
                    {
                        int SelectedPlayer = (int)InventoryPlayerSelectionRG.EditValue;

                        LoadoutViewBag Loadout = Serializer.Deserialize<LoadoutViewBag>(Serializer.ReadDataFile(OFD.FileName));

                        for (int i = 0; i < 10; i++)
                            Biohazard.Players[SelectedPlayer].Inventory.Slots[i].SetFromLoadoutTemporaryItem(Loadout.Slots[i], Initialized);

                        Terminal.WriteLine($"[App] Loadout \"{Loadout.Name}\" applied successfully on P{SelectedPlayer + 1}'s inventory.");
                        Utility.MessageBox_Information($"Loadout \"{Loadout.Name}\" applied successfully!");
                    }
                }
            }
        }

        private void SpinEdit_Spin(object sender, DevExpress.XtraEditors.Controls.SpinEventArgs e)
        {
            ComboBoxEdit[] ItemCombos =
            {
                P1Slot1ItemCB,
                P1Slot2ItemCB,
                P1Slot3ItemCB,
                P1Slot4ItemCB,
                P1Slot5ItemCB,
                P1Slot6ItemCB,
                P1Slot7ItemCB,
                P1Slot8ItemCB,
                P1Slot9ItemCB,
                P1SlotKnifeItemCB
            };

            SpinEdit SE = sender as SpinEdit;
            int Index = !SE.Name.Contains("Knife") ? int.Parse(SE.Name[6].ToString()) - 1 : 9;
            int SelectedPlayer = (int)InventoryPlayerSelectionRG.EditValue;

            Item SelectedItem = ItemCombos[Index].SelectedItem as Item;

            dynamic PropArray = null;

            if (SE.Name.Contains("Firepower"))
                PropArray = SelectedItem.Firepower;
            else if (SE.Name.Contains("ReloadSpeed"))
                PropArray = SelectedItem.ReloadSpeed;
            else if (SE.Name.Contains("Capacity"))
                PropArray = SelectedItem.Capacity;
            else if (SE.Name.Contains("Critical"))
                PropArray = SelectedItem.Critical;
            else if (SE.Name.Contains("Piercing"))
                PropArray = SelectedItem.Piercing;
            else if (SE.Name.Contains("Range"))
                PropArray = SelectedItem.Range;
            else if (SE.Name.Contains("Scope"))
                PropArray = SelectedItem.Scope;

            int OldValueIndex = SE.Name.Contains("ReloadSpeed") ? Array.IndexOf(SelectedItem.ReloadSpeed, double.Parse(SE.Text)) : Array.IndexOf(PropArray, int.Parse(SE.Text));
            int NewValueIndex = e.IsSpinUp ? OldValueIndex + 1 : OldValueIndex - 1;

            Slot[] SlotCollection = Biohazard.Players[SelectedPlayer].Inventory.Slots;

            if (!SE.Name.Contains("ReloadSpeed"))
            {
                SpinEdit ReloadSpeedSE = (SpinEdit)GetInventoryControl(SelectedPlayer, Index, $"P{SelectedPlayer + 1}Slot" + (Index != 9 ? (Index + 1).ToString() : "Knife") + "ReloadSpeedSE");
                ReloadSpeedSE.Text = ((decimal)SlotCollection[Index].ToMemoryItem.ReloadSpeed[SlotCollection[Index].ReloadSpeed]).ToString("F");
            }

            if ((e.IsSpinUp && PropArray.Length > NewValueIndex) || (!e.IsSpinUp && NewValueIndex >= 0))
            {
                SE.Text = SE.Name.Contains("ReloadSpeed") ? ((decimal)PropArray[NewValueIndex]).ToString("F") : PropArray[NewValueIndex].ToString();
                SlotCollection[Index].UpdateItemToMemory(Initialized);
            }
            else
                e.Handled = true;
        }

        private void InventoryTextEditNumeric_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            bool IsValid = int.TryParse(e.NewValue.ToString(), out _);

            if (!IsValid)
                e.NewValue = "0";

            TextEdit TE = sender as TextEdit;
            int Index = !TE.Name.Contains("Knife") ? int.Parse(TE.Name[6].ToString()) - 1 : 9;

            int SelectedPlayer = (int)InventoryPlayerSelectionRG.EditValue;
            Slot[] SlotCollection = Biohazard.Players[SelectedPlayer].Inventory.Slots;

            if (TE.Name.Contains("MaxQuantity"))
                SlotCollection[Index].ToMemory.MaxQuantity = short.Parse(e.NewValue.ToString());
            else if (TE.Name.Contains("Quantity"))
                SlotCollection[Index].ToMemory.Quantity = short.Parse(e.NewValue.ToString());
            else
                return;

            if (!Initialized)
                return;

            SlotCollection[Index].SetItem(SlotCollection[Index].ToMemory);
        }

        private void InventoryComboBox_IndexChanged(object sender, EventArgs e)
        {
            PictureEdit[] SlotPicBoxes =
            {
                Slot1PictureBox,
                Slot2PictureBox,
                Slot3PictureBox,
                Slot4PictureBox,
                Slot5PictureBox,
                Slot6PictureBox,
                Slot7PictureBox,
                Slot8PictureBox,
                Slot9PictureBox,
                SlotKnifePictureBox,
            };

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            int Index = !CBE.Name.Contains("Knife") ? int.Parse(CBE.Name[6].ToString()) - 1 : 9;

            Item SelectedItem = CBE.SelectedItem as Item;

            Image Final = Image.FromFile(@"addons/GameX.Biohazard.5/images/inventory/slot.png");
            SlotPicBoxes[Index].Image = Final;

            int SelectedPlayer = (int)InventoryPlayerSelectionRG.EditValue;
            Slot[] SlotCollection = Biohazard.Players[SelectedPlayer].Inventory.Slots;
            
            if (SlotCollection[Index].BeingUpdatedOnGUI)
                return;

            ((TextEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}QuantityTE" : $"P{SelectedPlayer + 1}SlotKnifeQuantityTE")).Text = SelectedItem.Capacity[0].ToString();
            ((TextEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}MaxQuantityTE" : $"P{SelectedPlayer + 1}SlotKnifeMaxQuantityTE")).Text = SelectedItem.Capacity[0].ToString();
            ((SpinEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}FirepowerSE" : $"P{SelectedPlayer + 1}SlotKnifeFirepowerSE")).Text = SelectedItem.Firepower[0].ToString();
            ((SpinEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}ReloadSpeedSE" : $"P{SelectedPlayer + 1}SlotKnifeReloadSpeedSE")).Text = ((decimal)SelectedItem.ReloadSpeed[0]).ToString("F");
            ((SpinEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}CapacitySE" : $"P{SelectedPlayer + 1}SlotKnifeCapacitySE")).Text = SelectedItem.Capacity[0].ToString();
            ((SpinEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}CriticalSE" : $"P{SelectedPlayer + 1}SlotKnifeCriticalSE")).Text = SelectedItem.Critical[0].ToString();
            ((SpinEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}PiercingSE" : $"P{SelectedPlayer + 1}SlotKnifePiercingSE")).Text = SelectedItem.Piercing[0].ToString();
            ((SpinEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}RangeSE" : $"P{SelectedPlayer + 1}SlotKnifeRangeSE")).Text = SelectedItem.Range[0].ToString();
            ((SpinEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}ScopeSE" : $"P{SelectedPlayer + 1}SlotKnifeScopeSE")).Text = SelectedItem.Scope[0].ToString();

            Biohazard.Players[SelectedPlayer].Inventory.Slots[Index].UpdateItemToMemory(Initialized);
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
                CostumeCombos[Index].Properties.Items.Add(Cos);

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
            int Index = int.Parse(CBE.Name[1].ToString()) - 1;

            Character CBEChar = CharacterCombos[Index].SelectedItem as Character;
            Costume CBECos = CBE.SelectedItem as Costume;

            Image Portrait = Utility.GetImageFromFile(@"addons/GameX.Biohazard.5/images/character/" + CBECos.Portrait);

            if (Portrait != null)
                CharPicBoxes[Index].Image = Portrait;

            if (!Initialized)
                return;

            Biohazard.Players[Index].Character = CBEChar.Value;
            Biohazard.Players[Index].Costume = CBECos.Value;
        }

        private void WeaponMode_IndexChanged(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            int Index = int.Parse(CBE.Name[1].ToString()) - 1;

            Biohazard.Players[Index].WeaponMode = CBE.SelectedIndex != 0 ? (byte)(CBE.SelectedItem as Simple).Value : (byte)Biohazard.Players[Index].GetDefaultWeaponMode();
        }

        private void Handness_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            int Index = int.Parse(CBE.Name[1].ToString()) - 1;

            if (!Initialized)
                return;

            Biohazard.Players[Index].Handness = CBE.SelectedIndex != 0 ? (byte)(CBE.SelectedItem as Simple).Value : (byte)Biohazard.Players[Index].GetDefaultHandness();
        }

        private void WeaponPlacement_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;

            if (!Initialized)
                return;

            Biohazard.SetWeaponPlacement((CBE.SelectedItem as Simple).Value);
        }

        private void Melee_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            Melee_ApplyComboBox(CBE);
        }

        private void CharCosFreeze_CheckedChanged(object sender, EventArgs e)
        {
            CheckButton CKBTN = sender as CheckButton;
            CKBTN.Text = CKBTN.Checked ? "Frozen" : "Freeze";
        }

        public void EnableDisable_StateChanged(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(CheckEdit))
            {
                CheckEdit CE = sender as CheckEdit;

                if (CE.Name.Equals("WeskerGlassesCheckEdit"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.WeskerNoSunglassDrop(CE.Checked);
                }
                else if (CE.Name.Equals("WeskerNoDashCostCheckEdit"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.WeskerNoDashCost(CE.Checked);
                }
                else if (CE.Name.Equals("MeleeCameraCheckEdit"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.DisableMeleeCamera(CE.Checked);
                }
                else if (CE.Name.Equals("ReunionSpecialMovesCheckEdit"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.EnableReunionSpecialMoves(CE.Checked);
                }
                else if (CE.Name.Contains("FreezeAllCheckEdit"))
                {
                    int Index = int.Parse(CE.Name[1].ToString()) - 1;

                    for (int i = 0; i < 10; i++)
                    {
                        CheckEdit CheckAllCE = (CheckEdit)GetInventoryControl(Index, i, i != 9 ? $"P{Index + 1}Slot{i + 1}FrozenCheckEdit" : $"P{Index + 1}SlotKnifeFrozenCheckEdit");
                        CheckAllCE.Checked = CE.Checked;
                    }
                }
            }
            else if (sender.GetType() == typeof(CheckButton))
            {
                CheckButton CB = sender as CheckButton;

                if (CB.Checked && CB.Text.Contains("OFF"))
                    CB.Text = CB.Text.Replace("OFF", "ON");

                if (!CB.Checked && CB.Text.Contains("ON"))
                    CB.Text = CB.Text.Replace("ON", "OFF");

                if (CB.Name.Contains("Untargetable") && !CB.Checked)
                {
                    if (!Initialized)
                        return;

                    int Player = int.Parse(CB.Name[1].ToString()) - 1;
                    Biohazard.Players[Player].Invulnerable = false;
                }
                else if (CB.Name.Contains("InfiniteAmmo") && !CB.Checked)
                {
                    if (!Initialized)
                        return;

                    int Player = int.Parse(CB.Name[1].ToString()) - 1;
                    Biohazard.Players[Player].SetInfiniteAmmo(false);
                }
                else if (CB.Name.Contains("Rapidfire") && !CB.Checked)
                {
                    if (!Initialized)
                        return;

                    int Player = int.Parse(CB.Name[1].ToString()) - 1;
                    Biohazard.Players[Player].SetRapidFire(false);
                }
            }
            else if (sender.GetType() == typeof(SimpleButton))
            {
                SimpleButton SB = sender as SimpleButton;
                SB.Text = SB.Text == "Enable" ? "Disable" : "Enable";

                if (SB.Name.Equals("ColorFilterButton"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.EnableColorFilter(SB.Text.Equals("Disable"));
                }
            }
        }

        private void RadioGroup_EditValueChanged(object sender, EventArgs e)
        {
            RadioGroup RG = sender as RadioGroup;

            if (RG.Name.Equals("InventoryPlayerSelectionRG"))
            {
                PlayerInventoryTabControl.SelectedTabPageIndex = (int)InventoryPlayerSelectionRG.EditValue;
            }
        }

        private void ToggleSwitch_Toggled(object sender, EventArgs e)
        {
            ToggleSwitch TS = sender as ToggleSwitch;

            ComboBoxEdit[] MeleeCombos =
            {
                TauntComboBox,
                KnifeComboBox,
                QuickTurnComboBox,
                MoveLeftComboBox,
                MoveRightComboBox,
                MoveBackComboBox,
                ReloadComboBox
            };

            LabelControl[] MeleeLabels =
            {
                TauntLabelControl,
                KnifeLabelControl,
                QuickTurnLabelControl,
                MoveLeftLabelControl,
                MoveRightLabelControl,
                MoveBackLabelControl,
                ReloadLabelControl
            };

            switch (TS.Name)
            {
                case "MeleeAnytimeSwitch":
                    foreach(ComboBoxEdit CBE in MeleeCombos)
                    {
                        CBE.Enabled = TS.IsOn;

                        if (!TS.IsOn)
                        {
                            int index = Array.IndexOf(MeleeCombos, CBE);

                            foreach (object item in MeleeCombos[index].Properties.Items)
                            {
                                if ((item as Move).Name == MeleeLabels[index].Text.Replace(":", ""))
                                    MeleeCombos[index].SelectedItem = item;
                            }
                        }
                    }                      

                    break;
            }
        }

        private void Melee_ApplyComboBox(ComboBoxEdit CE, bool ApplyAll = false)
        {
            ComboBoxEdit[] MeleeCombos =
            {
                ReunionHeadFlashComboBox,
                ReunionLegFrontComboBox,
                FinisherFrontComboBox,
                FinisherBackComboBox,
                HeadFlashComboBox,
                ArmFrontComboBox,
                ArmBackComboBox,
                LegFrontComboBox,
                LegBackComboBox,
                HelpComboBox,
                TauntComboBox,
                KnifeComboBox,
                QuickTurnComboBox,
                MoveLeftComboBox,
                MoveRightComboBox,
                MoveBackComboBox,
                ReloadComboBox
            };

            LabelControl[] MeleeLabels =
            {
                ReunionHeadFlashLabelControl,
                ReunionLegFrontLabelControl,
                FinisherFrontLabelControl,
                FinisherBackLabelControl,
                HeadFlashLabelControl,
                ArmFrontLabelControl,
                ArmBackLabelControl,
                LegFrontLabelControl,
                LegBackLabelControl,
                HelpLabelControl,
                TauntLabelControl,
                KnifeLabelControl,
                QuickTurnLabelControl,
                MoveLeftLabelControl,
                MoveRightLabelControl,
                MoveBackLabelControl,
                ReloadLabelControl
            };

            if (!Initialized)
                return;

            if (!ApplyAll)
            {
                int index = Array.IndexOf(MeleeCombos, CE);
                Biohazard.SetMelee(MeleeLabels[index].Text.Replace(":", ""), (byte)(CE.SelectedItem as Move).Value);
            }
            else
            {
                for (int i = 0; i < MeleeCombos.Length; i++)
                {
                    Biohazard.SetMelee(MeleeLabels[i].Text.Replace(":", ""), (byte)(MeleeCombos[i].SelectedItem as Move).Value);
                }
            }
        }

        #endregion

        #region GameX Calls

        private void GameX_CheckControls()
        {
            SimpleButton[] EnableDisable =
            {
                ColorFilterButton,
            };

            CheckEdit[] CheckUncheck =
            {
                WeskerGlassesCheckEdit,
                WeskerNoDashCostCheckEdit,
                MeleeCameraCheckEdit,
                ReunionSpecialMovesCheckEdit
            };

            foreach (CheckEdit CE in CheckUncheck)
            {
                switch (CE.Name)
                {
                    case "WeskerGlassesCheckEdit":
                        if (CE.Checked)
                            Biohazard.WeskerNoSunglassDrop(true);

                        break;
                    case "WeskerNoDashCostCheckEdit":
                        if (CE.Checked)
                            Biohazard.WeskerNoDashCost(true);

                        break;
                    case "MeleeCameraCheckEdit":
                        if (CE.Checked)
                            Biohazard.DisableMeleeCamera(true);

                        break;
                    case "ReunionSpecialMovesCheckEdit":
                        if (CE.Checked)
                            Biohazard.EnableReunionSpecialMoves(true);

                        break;
                }
            }

            foreach (SimpleButton SB in EnableDisable)
            {
                switch (SB.Name)
                {
                    case "ColorFilterButton":
                    {
                        if (SB.Text == "Disable")
                            Biohazard.EnableColorFilter(true);

                        break;
                    }
                }
            }

            Melee_ApplyComboBox(null, true);
            WeaponPlacement_IndexChanged(WeaponPlacementComboBox, null);
        }

        private void GameX_Start()
        {
            try
            {
                Biohazard.StartModule();

                GameX_CheckControls();

                //Biohazard.NoFileChecking(true);
                //Biohazard.OnlineCharSwapFixes(true);
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

                if (MeleeAnytimeSwitch.IsOn)
                    MeleeAnytimeSwitch.Toggle();

                //Biohazard.NoFileChecking(false);
                //Biohazard.OnlineCharSwapFixes(false);
                Biohazard.EnableColorFilter(false);
                Biohazard.WeskerNoSunglassDrop(false);
                Biohazard.WeskerNoDashCost(false);
                Biohazard.SetWeaponPlacement(0);
                Biohazard.DisableMeleeCamera(false);
                Biohazard.EnableReunionSpecialMoves(false);

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
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.ToString());
            }
        }

        private static void GameX_Keyboard(int input)
        {
        }

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
                bool PlayerPresent = Biohazard.Players[i].IsActive();

                // Characters & Costumes //
                if (!CharacterCombos[i].IsPopupOpen && !CostumeCombos[i].IsPopupOpen)
                {
                    if (!CheckButtons[i].Checked)
                    {
                        int CurrentChar = Biohazard.Players[i].Character;
                        int CurrentCostume = Biohazard.Players[i].Costume;

                        foreach (object Char in CharacterCombos[i].Properties.Items)
                        {
                            if ((Char as Character).Value == CurrentChar)
                                CharacterCombos[i].SelectedItem = Char;
                        }

                        foreach (object Cos in CostumeCombos[i].Properties.Items)
                        {
                            if ((Cos as Costume).Value == CurrentCostume)
                                CostumeCombos[i].SelectedItem = Cos;
                        }
                    }
                    else
                    {
                        Biohazard.Players[i].Character = (CharacterCombos[i].SelectedItem as Character).Value;
                        Biohazard.Players[i].Costume = (CostumeCombos[i].SelectedItem as Costume).Value;
                    }
                }

                // Inventory //
                for (int slot = 0; slot < 10; slot++)
                {
                    if (i != 0)
                        continue;

                    Biohazard.Players[i].Inventory.Slots[slot].Update();
                }

                // Health Bar //
                double PlayerHealthPercent = PlayerPresent ? (double)Biohazard.Players[i].Health / Biohazard.Players[i].MaxHealth : 1.0;

                HealthBars[i].Properties.Maximum = PlayerPresent ? Biohazard.Players[i].MaxHealth : 1;
                HealthBars[i].EditValue = PlayerPresent ? Biohazard.Players[i].Health : 1;
                HealthBars[i].Properties.StartColor = PlayerPresent ? Color.FromArgb((int)(255.0 - 155.0 * PlayerHealthPercent), (int)(0.0 + 255.0 * PlayerHealthPercent), 0) : Color.FromArgb(0, 0, 0, 0);
                HealthBars[i].Properties.EndColor = PlayerPresent ? Color.FromArgb((int)(255.0 - 155.0 * PlayerHealthPercent), (int)(0.0 + 255.0 * PlayerHealthPercent), 0) : Color.FromArgb(0, 0, 0, 0);

                // Player Name //
                PlayerGroupBoxes[i].Text = $"Player {i + 1} - " + (Biohazard.InGame ? i == Biohazard.LocalPlayer ? Biohazard.LocalPlayerName : PlayerPresent ? Biohazard.Players[i].AI ? "CPU AI" : "Connected" : "Disconnected" : "Disconnected");

                // Handness //
                if (Handness[i].SelectedIndex > 0 && PlayerPresent)
                    Biohazard.Players[i].Handness = (byte)(Handness[i].SelectedItem as Simple).Value;

                // Weapon Mode //
                if (WeaponMode[i].SelectedIndex > 0 && PlayerPresent)
                    Biohazard.Players[i].WeaponMode = (byte)(WeaponMode[i].SelectedItem as Simple).Value;

                // Melee Anytime Cords //
                if (MeleeAnytimeSwitch.IsOn && PlayerPresent)
                {
                    Biohazard.Players[i].MeleePosition = Biohazard.Players[i].Position;

                    if (Biohazard.Players[i].MeleeTarget != 0 && Biohazard.Players[i].DoingIdleMove())
                        Biohazard.Players[i].MeleeTarget = 0;
                }

                // If versus then end the current iteration //
                if (Biohazard.GameMode == (int)GameModeEnum.Versus)
                    continue;

                // Infinite HP //
                if (InfiniteHP[i].Checked && PlayerPresent)
                    Biohazard.Players[i].Health = Biohazard.Players[i].MaxHealth;

                // Untergetable //
                if (Untargetable[i].Checked && PlayerPresent)
                    Biohazard.Players[i].Invulnerable = true;

                // Infinite Ammo //
                if (InfiniteAmmo[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetInfiniteAmmo(true);

                // Rapidfire //
                if (RapidFire[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetRapidFire(true);

                // Infinite Dash //
                if (WeskerInfiniteDashCheckEdit.Checked && PlayerPresent && Biohazard.Players[i].Character == (int)CharacterEnum.Wesker)
                    Biohazard.Players[i].ResetDash();
            }
        }

        #endregion
    }
}