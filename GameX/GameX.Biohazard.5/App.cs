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
using GameX.Helpers;
using GameX.Modules;
using GameX.Database;
using GameX.Enum;
using GameX.Database.Type;
using GameX.Database.ViewBag;
using DevExpress.XtraEditors.Controls;

namespace GameX
{
    public partial class App : XtraForm
    {
        #region Base Props

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
            #region Controls

            SimpleButton[] MasterTabButtons =
            {
                TabPageCharButton,
                TabPageMeleeButton,
                TabPageInventoryButton,
                TabPageSettingsButton,
                TabPageConsoleButton
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

            CheckButton[] InfiniteResource =
            {
                P1InfiniteResourceButton,
                P2InfiniteResourceButton,
                P3InfiniteResourceButton,
                P4InfiniteResourceButton
            };

            CheckButton[] InfiniteThrowable =
            {
                P1InfiniteThrowableButton,
                P2InfiniteThrowableButton,
                P3InfiniteThrowableButton,
                P4InfiniteThrowableButton
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
                P1SlotKnifeItemCB,
                P2Slot1ItemCB,
                P2Slot2ItemCB,
                P2Slot3ItemCB,
                P2Slot4ItemCB,
                P2Slot5ItemCB,
                P2Slot6ItemCB,
                P2Slot7ItemCB,
                P2Slot8ItemCB,
                P2Slot9ItemCB,
                P2SlotKnifeItemCB,
                P3Slot1ItemCB,
                P3Slot2ItemCB,
                P3Slot3ItemCB,
                P3Slot4ItemCB,
                P3Slot5ItemCB,
                P3Slot6ItemCB,
                P3Slot7ItemCB,
                P3Slot8ItemCB,
                P3Slot9ItemCB,
                P3SlotKnifeItemCB,
                P4Slot1ItemCB,
                P4Slot2ItemCB,
                P4Slot3ItemCB,
                P4Slot4ItemCB,
                P4Slot5ItemCB,
                P4Slot6ItemCB,
                P4Slot7ItemCB,
                P4Slot8ItemCB,
                P4Slot9ItemCB,
                P4SlotKnifeItemCB
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
                P1SlotKnifeQuantityTE,
                P2Slot1QuantityTE,
                P2Slot2QuantityTE,
                P2Slot3QuantityTE,
                P2Slot4QuantityTE,
                P2Slot5QuantityTE,
                P2Slot6QuantityTE,
                P2Slot7QuantityTE,
                P2Slot8QuantityTE,
                P2Slot9QuantityTE,
                P2SlotKnifeQuantityTE,
                P3Slot1QuantityTE,
                P3Slot2QuantityTE,
                P3Slot3QuantityTE,
                P3Slot4QuantityTE,
                P3Slot5QuantityTE,
                P3Slot6QuantityTE,
                P3Slot7QuantityTE,
                P3Slot8QuantityTE,
                P3Slot9QuantityTE,
                P3SlotKnifeQuantityTE,
                P4Slot1QuantityTE,
                P4Slot2QuantityTE,
                P4Slot3QuantityTE,
                P4Slot4QuantityTE,
                P4Slot5QuantityTE,
                P4Slot6QuantityTE,
                P4Slot7QuantityTE,
                P4Slot8QuantityTE,
                P4Slot9QuantityTE,
                P4SlotKnifeQuantityTE
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
                P1SlotKnifeMaxQuantityTE,
                P2Slot1MaxQuantityTE,
                P2Slot2MaxQuantityTE,
                P2Slot3MaxQuantityTE,
                P2Slot4MaxQuantityTE,
                P2Slot5MaxQuantityTE,
                P2Slot6MaxQuantityTE,
                P2Slot7MaxQuantityTE,
                P2Slot8MaxQuantityTE,
                P2Slot9MaxQuantityTE,
                P2SlotKnifeMaxQuantityTE,
                P3Slot1MaxQuantityTE,
                P3Slot2MaxQuantityTE,
                P3Slot3MaxQuantityTE,
                P3Slot4MaxQuantityTE,
                P3Slot5MaxQuantityTE,
                P3Slot6MaxQuantityTE,
                P3Slot7MaxQuantityTE,
                P3Slot8MaxQuantityTE,
                P3Slot9MaxQuantityTE,
                P3SlotKnifeMaxQuantityTE,
                P4Slot1MaxQuantityTE,
                P4Slot2MaxQuantityTE,
                P4Slot3MaxQuantityTE,
                P4Slot4MaxQuantityTE,
                P4Slot5MaxQuantityTE,
                P4Slot6MaxQuantityTE,
                P4Slot7MaxQuantityTE,
                P4Slot8MaxQuantityTE,
                P4Slot9MaxQuantityTE,
                P4SlotKnifeMaxQuantityTE
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
                P1SlotKnifeFirepowerSE,
                P2Slot1FirepowerSE,
                P2Slot2FirepowerSE,
                P2Slot3FirepowerSE,
                P2Slot4FirepowerSE,
                P2Slot5FirepowerSE,
                P2Slot6FirepowerSE,
                P2Slot7FirepowerSE,
                P2Slot8FirepowerSE,
                P2Slot9FirepowerSE,
                P2SlotKnifeFirepowerSE,
                P3Slot1FirepowerSE,
                P3Slot2FirepowerSE,
                P3Slot3FirepowerSE,
                P3Slot4FirepowerSE,
                P3Slot5FirepowerSE,
                P3Slot6FirepowerSE,
                P3Slot7FirepowerSE,
                P3Slot8FirepowerSE,
                P3Slot9FirepowerSE,
                P3SlotKnifeFirepowerSE,
                P4Slot1FirepowerSE,
                P4Slot2FirepowerSE,
                P4Slot3FirepowerSE,
                P4Slot4FirepowerSE,
                P4Slot5FirepowerSE,
                P4Slot6FirepowerSE,
                P4Slot7FirepowerSE,
                P4Slot8FirepowerSE,
                P4Slot9FirepowerSE,
                P4SlotKnifeFirepowerSE
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
                P1SlotKnifeReloadSpeedSE,
                P2Slot1ReloadSpeedSE,
                P2Slot2ReloadSpeedSE,
                P2Slot3ReloadSpeedSE,
                P2Slot4ReloadSpeedSE,
                P2Slot5ReloadSpeedSE,
                P2Slot6ReloadSpeedSE,
                P2Slot7ReloadSpeedSE,
                P2Slot8ReloadSpeedSE,
                P2Slot9ReloadSpeedSE,
                P2SlotKnifeReloadSpeedSE,
                P3Slot1ReloadSpeedSE,
                P3Slot2ReloadSpeedSE,
                P3Slot3ReloadSpeedSE,
                P3Slot4ReloadSpeedSE,
                P3Slot5ReloadSpeedSE,
                P3Slot6ReloadSpeedSE,
                P3Slot7ReloadSpeedSE,
                P3Slot8ReloadSpeedSE,
                P3Slot9ReloadSpeedSE,
                P3SlotKnifeReloadSpeedSE,
                P4Slot1ReloadSpeedSE,
                P4Slot2ReloadSpeedSE,
                P4Slot3ReloadSpeedSE,
                P4Slot4ReloadSpeedSE,
                P4Slot5ReloadSpeedSE,
                P4Slot6ReloadSpeedSE,
                P4Slot7ReloadSpeedSE,
                P4Slot8ReloadSpeedSE,
                P4Slot9ReloadSpeedSE,
                P4SlotKnifeReloadSpeedSE
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
                P1SlotKnifeCapacitySE,
                P2Slot1CapacitySE,
                P2Slot2CapacitySE,
                P2Slot3CapacitySE,
                P2Slot4CapacitySE,
                P2Slot5CapacitySE,
                P2Slot6CapacitySE,
                P2Slot7CapacitySE,
                P2Slot8CapacitySE,
                P2Slot9CapacitySE,
                P2SlotKnifeCapacitySE,
                P3Slot1CapacitySE,
                P3Slot2CapacitySE,
                P3Slot3CapacitySE,
                P3Slot4CapacitySE,
                P3Slot5CapacitySE,
                P3Slot6CapacitySE,
                P3Slot7CapacitySE,
                P3Slot8CapacitySE,
                P3Slot9CapacitySE,
                P3SlotKnifeCapacitySE,
                P4Slot1CapacitySE,
                P4Slot2CapacitySE,
                P4Slot3CapacitySE,
                P4Slot4CapacitySE,
                P4Slot5CapacitySE,
                P4Slot6CapacitySE,
                P4Slot7CapacitySE,
                P4Slot8CapacitySE,
                P4Slot9CapacitySE,
                P4SlotKnifeCapacitySE
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
                P1SlotKnifeCriticalSE,
                P2Slot1CriticalSE,
                P2Slot2CriticalSE,
                P2Slot3CriticalSE,
                P2Slot4CriticalSE,
                P2Slot5CriticalSE,
                P2Slot6CriticalSE,
                P2Slot7CriticalSE,
                P2Slot8CriticalSE,
                P2Slot9CriticalSE,
                P2SlotKnifeCriticalSE,
                P3Slot1CriticalSE,
                P3Slot2CriticalSE,
                P3Slot3CriticalSE,
                P3Slot4CriticalSE,
                P3Slot5CriticalSE,
                P3Slot6CriticalSE,
                P3Slot7CriticalSE,
                P3Slot8CriticalSE,
                P3Slot9CriticalSE,
                P3SlotKnifeCriticalSE,
                P4Slot1CriticalSE,
                P4Slot2CriticalSE,
                P4Slot3CriticalSE,
                P4Slot4CriticalSE,
                P4Slot5CriticalSE,
                P4Slot6CriticalSE,
                P4Slot7CriticalSE,
                P4Slot8CriticalSE,
                P4Slot9CriticalSE,
                P4SlotKnifeCriticalSE
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
                P1SlotKnifePiercingSE,
                P2Slot1PiercingSE,
                P2Slot2PiercingSE,
                P2Slot3PiercingSE,
                P2Slot4PiercingSE,
                P2Slot5PiercingSE,
                P2Slot6PiercingSE,
                P2Slot7PiercingSE,
                P2Slot8PiercingSE,
                P2Slot9PiercingSE,
                P2SlotKnifePiercingSE,
                P3Slot1PiercingSE,
                P3Slot2PiercingSE,
                P3Slot3PiercingSE,
                P3Slot4PiercingSE,
                P3Slot5PiercingSE,
                P3Slot6PiercingSE,
                P3Slot7PiercingSE,
                P3Slot8PiercingSE,
                P3Slot9PiercingSE,
                P3SlotKnifePiercingSE,
                P4Slot1PiercingSE,
                P4Slot2PiercingSE,
                P4Slot3PiercingSE,
                P4Slot4PiercingSE,
                P4Slot5PiercingSE,
                P4Slot6PiercingSE,
                P4Slot7PiercingSE,
                P4Slot8PiercingSE,
                P4Slot9PiercingSE,
                P4SlotKnifePiercingSE
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
                P1SlotKnifeRangeSE,
                P2Slot1RangeSE,
                P2Slot2RangeSE,
                P2Slot3RangeSE,
                P2Slot4RangeSE,
                P2Slot5RangeSE,
                P2Slot6RangeSE,
                P2Slot7RangeSE,
                P2Slot8RangeSE,
                P2Slot9RangeSE,
                P2SlotKnifeRangeSE,
                P3Slot1RangeSE,
                P3Slot2RangeSE,
                P3Slot3RangeSE,
                P3Slot4RangeSE,
                P3Slot5RangeSE,
                P3Slot6RangeSE,
                P3Slot7RangeSE,
                P3Slot8RangeSE,
                P3Slot9RangeSE,
                P3SlotKnifeRangeSE,
                P4Slot1RangeSE,
                P4Slot2RangeSE,
                P4Slot3RangeSE,
                P4Slot4RangeSE,
                P4Slot5RangeSE,
                P4Slot6RangeSE,
                P4Slot7RangeSE,
                P4Slot8RangeSE,
                P4Slot9RangeSE,
                P4SlotKnifeRangeSE
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
                P1SlotKnifeScopeSE,
                P2Slot1ScopeSE,
                P2Slot2ScopeSE,
                P2Slot3ScopeSE,
                P2Slot4ScopeSE,
                P2Slot5ScopeSE,
                P2Slot6ScopeSE,
                P2Slot7ScopeSE,
                P2Slot8ScopeSE,
                P2Slot9ScopeSE,
                P2SlotKnifeScopeSE,
                P3Slot1ScopeSE,
                P3Slot2ScopeSE,
                P3Slot3ScopeSE,
                P3Slot4ScopeSE,
                P3Slot5ScopeSE,
                P3Slot6ScopeSE,
                P3Slot7ScopeSE,
                P3Slot8ScopeSE,
                P3Slot9ScopeSE,
                P3SlotKnifeScopeSE,
                P4Slot1ScopeSE,
                P4Slot2ScopeSE,
                P4Slot3ScopeSE,
                P4Slot4ScopeSE,
                P4Slot5ScopeSE,
                P4Slot6ScopeSE,
                P4Slot7ScopeSE,
                P4Slot8ScopeSE,
                P4Slot9ScopeSE,
                P4SlotKnifeScopeSE
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
                P1SlotKnifeInfiniteAmmoCheckEdit,
                P2Slot1InfiniteAmmoCheckEdit,
                P2Slot2InfiniteAmmoCheckEdit,
                P2Slot3InfiniteAmmoCheckEdit,
                P2Slot4InfiniteAmmoCheckEdit,
                P2Slot5InfiniteAmmoCheckEdit,
                P2Slot6InfiniteAmmoCheckEdit,
                P2Slot7InfiniteAmmoCheckEdit,
                P2Slot8InfiniteAmmoCheckEdit,
                P2Slot9InfiniteAmmoCheckEdit,
                P2SlotKnifeInfiniteAmmoCheckEdit,
                P3Slot1InfiniteAmmoCheckEdit,
                P3Slot2InfiniteAmmoCheckEdit,
                P3Slot3InfiniteAmmoCheckEdit,
                P3Slot4InfiniteAmmoCheckEdit,
                P3Slot5InfiniteAmmoCheckEdit,
                P3Slot6InfiniteAmmoCheckEdit,
                P3Slot7InfiniteAmmoCheckEdit,
                P3Slot8InfiniteAmmoCheckEdit,
                P3Slot9InfiniteAmmoCheckEdit,
                P3SlotKnifeInfiniteAmmoCheckEdit,
                P4Slot1InfiniteAmmoCheckEdit,
                P4Slot2InfiniteAmmoCheckEdit,
                P4Slot3InfiniteAmmoCheckEdit,
                P4Slot4InfiniteAmmoCheckEdit,
                P4Slot5InfiniteAmmoCheckEdit,
                P4Slot6InfiniteAmmoCheckEdit,
                P4Slot7InfiniteAmmoCheckEdit,
                P4Slot8InfiniteAmmoCheckEdit,
                P4Slot9InfiniteAmmoCheckEdit,
                P4SlotKnifeInfiniteAmmoCheckEdit
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
                P1SlotKnifeRapidFireCheckEdit,
                P2Slot1RapidFireCheckEdit,
                P2Slot2RapidFireCheckEdit,
                P2Slot3RapidFireCheckEdit,
                P2Slot4RapidFireCheckEdit,
                P2Slot5RapidFireCheckEdit,
                P2Slot6RapidFireCheckEdit,
                P2Slot7RapidFireCheckEdit,
                P2Slot8RapidFireCheckEdit,
                P2Slot9RapidFireCheckEdit,
                P2SlotKnifeRapidFireCheckEdit,
                P3Slot1RapidFireCheckEdit,
                P3Slot2RapidFireCheckEdit,
                P3Slot3RapidFireCheckEdit,
                P3Slot4RapidFireCheckEdit,
                P3Slot5RapidFireCheckEdit,
                P3Slot6RapidFireCheckEdit,
                P3Slot7RapidFireCheckEdit,
                P3Slot8RapidFireCheckEdit,
                P3Slot9RapidFireCheckEdit,
                P3SlotKnifeRapidFireCheckEdit,
                P4Slot1RapidFireCheckEdit,
                P4Slot2RapidFireCheckEdit,
                P4Slot3RapidFireCheckEdit,
                P4Slot4RapidFireCheckEdit,
                P4Slot5RapidFireCheckEdit,
                P4Slot6RapidFireCheckEdit,
                P4Slot7RapidFireCheckEdit,
                P4Slot8RapidFireCheckEdit,
                P4Slot9RapidFireCheckEdit,
                P4SlotKnifeRapidFireCheckEdit
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
                P1SlotKnifeFrozenCheckEdit,
                P2Slot1FrozenCheckEdit,
                P2Slot2FrozenCheckEdit,
                P2Slot3FrozenCheckEdit,
                P2Slot4FrozenCheckEdit,
                P2Slot5FrozenCheckEdit,
                P2Slot6FrozenCheckEdit,
                P2Slot7FrozenCheckEdit,
                P2Slot8FrozenCheckEdit,
                P2Slot9FrozenCheckEdit,
                P2SlotKnifeFrozenCheckEdit,
                P3Slot1FrozenCheckEdit,
                P3Slot2FrozenCheckEdit,
                P3Slot3FrozenCheckEdit,
                P3Slot4FrozenCheckEdit,
                P3Slot5FrozenCheckEdit,
                P3Slot6FrozenCheckEdit,
                P3Slot7FrozenCheckEdit,
                P3Slot8FrozenCheckEdit,
                P3Slot9FrozenCheckEdit,
                P3SlotKnifeFrozenCheckEdit,
                P4Slot1FrozenCheckEdit,
                P4Slot2FrozenCheckEdit,
                P4Slot3FrozenCheckEdit,
                P4Slot4FrozenCheckEdit,
                P4Slot5FrozenCheckEdit,
                P4Slot6FrozenCheckEdit,
                P4Slot7FrozenCheckEdit,
                P4Slot8FrozenCheckEdit,
                P4Slot9FrozenCheckEdit,
                P4SlotKnifeFrozenCheckEdit
            };

            #endregion

            DB db = DBContext.GetDatabase();

            for (int i = 0; i < MasterTabButtons.Length; i++)
            {
                MasterTabButtons[i].Click += MasterTabPageButton_Click;
            }

            for (int i = 0; i < CharacterCombos.Length; i++)
            {
                CharacterCombos[i].Properties.Items.AddRange(db.Characters);
                WeaponMode[i].Properties.Items.AddRange(db.WeaponMode);
                Handness[i].Properties.Items.AddRange(db.Handness);

                CharCosFreezes[i].CheckedChanged += CharCosFreeze_CheckedChanged;
                InfiniteHP[i].CheckedChanged += EnableDisable_StateChanged;
                Untargetable[i].CheckedChanged += EnableDisable_StateChanged;
                InfiniteAmmo[i].CheckedChanged += EnableDisable_StateChanged;
                InfiniteResource[i].CheckedChanged += EnableDisable_StateChanged;
                InfiniteThrowable[i].CheckedChanged += EnableDisable_StateChanged;
                Rapidfire[i].CheckedChanged += EnableDisable_StateChanged;

                CharacterCombos[i].SelectedIndexChanged += CharComboBox_IndexChanged;
                CostumeCombos[i].SelectedIndexChanged += CosComboBox_IndexChanged;
                WeaponMode[i].SelectedIndexChanged += WeaponMode_IndexChanged;
                Handness[i].SelectedIndexChanged += Handness_IndexChanged;

                CharacterCombos[i].SelectedIndex = 0;
                WeaponMode[i].SelectedIndex = 0;
                Handness[i].SelectedIndex = 0;

                if (i < EnableDisable.Length)
                {
                    EnableDisable[i].Click += EnableDisable_StateChanged;
                }

                if (i == 3)
                    continue;

                CheckUncheck[i].CheckedChanged += EnableDisable_StateChanged;
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

                foreach (object item in MeleeCombosA[i].Properties.Items)
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

            for (int i = 0; i < 40; i++)
            {
                ItemCombos[i].Properties.Items.AddRange(db.ComboBoxItems);
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

                InfiniteAmmoChecks[i].CheckedChanged += EnableDisable_StateChanged;
                RapidfireChecks[i].CheckedChanged += EnableDisable_StateChanged;
                FrozenChecks[i].CheckedChanged += EnableDisable_StateChanged;
            }

            P1FreezeAllCheckEdit.CheckedChanged += EnableDisable_StateChanged;
            P2FreezeAllCheckEdit.CheckedChanged += EnableDisable_StateChanged;

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
            HandTremorCheckEdit.CheckedChanged += EnableDisable_StateChanged;
            NoTimerDecreaseCE.CheckedChanged += EnableDisable_StateChanged;

            ConsoleInputTextEdit.Validating += Terminal.ValidateInput;
            ClearConsoleSimpleButton.Click += Terminal.ClearConsole_Click;

            InventoryPlayerSelectionRG.EditValueChanged += RadioGroup_EditValueChanged;

            ControllerAimButton.Click += EnableDisable_StateChanged;

            LoadoutSaveButton.Click += SimpleButton_Click;
            LoadoutApplyButton.Click += SimpleButton_Click;
            LoadoutComboBox.Properties.Items.AddRange(db.Loadouts);

            AddMinuteButton.Click += SimpleButton_Click;
            RemoveMinuteButton.Click += SimpleButton_Click;
            ZeroTimeButton.Click += SimpleButton_Click;

            if (db.Loadouts.Count > 0)
                LoadoutComboBox.SelectedIndex = 0;

            MasterTabControl.SelectedTabPageIndex = 0;

            ResetHealthBars();
            SetupImages();

            Terminal.WriteLine("[App] App initialized.");
            Configuration_Load(null, null);
        }

        private void Application_Init()
        {
            Target_Setup();

            Keyboard.CreateHook(GameX_Keyboard);
            Application.ApplicationExit += Application_ApplicationExit;

            Terminal.StartModule(this);
            Biohazard.Setup(this);

            MainLoop.Tick += Application_Update;
            MainLoop.Enabled = true;
            MainLoop.Start();
        }

        private void Application_Update(object sender, EventArgs e)
        {
            if (Target_Handle() && Initialized)
                GameX_Update();
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            MainLoop.Enabled = false;
            MainLoop.Stop();

            if (Target_Process != null)
            {
                if (Initialized)
                    GameX_End();

                Target_Process.Exited += null;
                Target_Process.EnableRaisingEvents = false;
            }

            Memory.FinishModule();
            Keyboard.RemoveHook();

            Target_Process?.Dispose();
            Target_Process = null;
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

                Target_Process = ProcessHelper.GetProcessByName(Target);
                Verified = false;
                Initialized = false;
                Text = "GameX - Resident Evil 5 - Waiting game";

                if (Target_Process == null)
                    return Verified;

                Terminal.WriteLine("[App] Game found, validating.");
                Text = "GameX - Resident Evil 5 - Validanting";

                return Verified;
            }

            if (Verified || Target_Process.MainWindowHandle == IntPtr.Zero || !Target_Process.WaitForInputIdle())
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
                    if (Target_Modules.Any(Module => !ProcessHelper.ProcessHasModule(Target_Process, Module)))
                    {
                        return false;
                    }
                }
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex);
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

        #region Helpers

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

            Image Logo = ImageHelper.MergeImage(LogoA, LogoB);
            AboutPictureEdit.Image = Logo;

            TabPageConsoleButton.ImageOptions.Image = Properties.Resources.consoleunread;
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
                    return ((GroupControl)P2InventoryTab.Controls[Slot != 9 ? $"P2Slot{Slot + 1}EditGP" : "P2SlotKnifeEditGP"]).Controls[ControlName];
                case 2:
                    return ((GroupControl)P3InventoryTab.Controls[Slot != 9 ? $"P3Slot{Slot + 1}EditGP" : "P3SlotKnifeEditGP"]).Controls[ControlName];
                case 3:
                    return ((GroupControl)P4InventoryTab.Controls[Slot != 9 ? $"P4Slot{Slot + 1}EditGP" : "P4SlotKnifeEditGP"]).Controls[ControlName];
            }

            return null;
        }

        #endregion

        #region Event Handlers

        private void MasterTabPage_PageChanged(object sender, EventArgs e)
        {
            XtraTabControl XTC = sender as XtraTabControl;
            XtraTabPage XTP = XTC.SelectedTabPage;

            if (XTP.Name == "TabPageInventory")
            {
                Width = 1250;
                Height = 523;
                MasterTabControl.Width = 1225;
                MasterTabControl.Height = 439; //467
                TabInventoryGP.Width = 1206;
                TabInventoryGP.Height = 418;
            }
            else
            {
                Width = 664;
                Height = 523;
                MasterTabControl.Width = 640;
                MasterTabControl.Height = 439; //467
                TabInventoryGP.Width = 622;
                TabInventoryGP.Height = 418;
            }

            if (XTP.Name == "TabPageConsole")
            {
                Terminal.ScrollToEnd();
                TabPageConsoleButton.ImageOptions.Image = Properties.Resources.consoleread;
            }
        }

        private void MasterTabPageButton_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;

            XtraTabPage Page = MasterTabControl.TabPages.Where(x => x.Name == SB.Name.Replace("Button", "")).FirstOrDefault();

            if (Page == null)
                return;

            MasterTabControl.SelectedTabPage = Page;
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
                Terminal.WriteLine(Ex);
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
                Terminal.WriteLine(Ex);
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
            MainLoop.Interval = (UpdateModeComboBoxEdit.SelectedItem as Simple).Value;
        }

        private void Palette_IndexChanged(object sender, EventArgs e)
        {
            if (PaletteComboBoxEdit.Text == "")
                return;

            UserLookAndFeel.Default.SetSkinStyle(UserLookAndFeel.Default.ActiveSkinName, PaletteComboBoxEdit.Text);

            ResetHealthBars();
            SetupImages();
        }

        private void SimpleButton_Click(object sender, EventArgs e)
        {
            SimpleButton SB = sender as SimpleButton;

            if (SB.Name.Equals("LoadoutSaveButton"))
            {
                string SaveDialog = XtraInputBox.Show("Loadout Name", "Save Loadout", "Name").ToString();

                if (string.IsNullOrEmpty(SaveDialog))
                    return;

                int SelectedPlayer = (int)InventoryPlayerSelectionRG.EditValue;

                LoadoutViewBag Loadout = new LoadoutViewBag();
                Loadout.Name = SaveDialog;
                Loadout.Slots = new TemporaryItemViewBag[10];

                for (int i = 0; i < 10; i++)
                    Loadout.Slots[i] = Biohazard.Players[SelectedPlayer].Inventory.LoadoutSlots[i].ToMemory;

                Serializer.WriteDataFile(@"addons/GameX.Biohazard.5/prefabs/loadout/" + SaveDialog.Replace(" ", "").Trim() + ".json", Serializer.Serialize(Loadout));

                DBContext.UpdateLoadouts();
                DB db = DBContext.GetDatabase();

                LoadoutComboBox.Properties.Items.Clear();
                LoadoutComboBox.Properties.Items.AddRange(db.Loadouts);

                if (db.Loadouts.Count > 0)
                    LoadoutComboBox.SelectedIndex = 0;

                Terminal.WriteLine($"[App] Loadout \"{Loadout.Name}\" saved successfully!");
                Utility.MessageBox_Information($"Loadout \"{Loadout.Name}\" saved successfully!");
            }
            else if (SB.Name.Equals("LoadoutApplyButton"))
            {
                int SelectedPlayer = (int)InventoryPlayerSelectionRG.EditValue;

                LoadoutViewBag Loadout = LoadoutComboBox.SelectedItem as LoadoutViewBag;

                for (int i = 0; i < 10; i++)
                    Biohazard.Players[SelectedPlayer].Inventory.LoadoutSlots[i].SetFromLoadoutTemporaryItem(Loadout.Slots[i], Initialized);

                Terminal.WriteLine($"[App] Loadout \"{Loadout.Name}\" applied successfully on P{SelectedPlayer + 1}'s inventory.");
                //Utility.MessageBox_Information($"Loadout \"{Loadout.Name}\" applied successfully!");
            }
            else if (SB.Name.Equals("AddMinuteButton"))
            {
                if (!Initialized)
                    return;

                Biohazard.Timer += 60f;
            }
            else if (SB.Name.Equals("RemoveMinuteButton"))
            {
                if (!Initialized)
                    return;

                Biohazard.Timer -= 60f;
            }
            else if (SB.Name.Equals("ZeroTimeButton"))
            {
                if (!Initialized)
                    return;

                Biohazard.Timer = 0f;
            }
        }

        private void SpinEdit_Spin(object sender, SpinEventArgs e)
        {
            SpinEdit SE = sender as SpinEdit;
            int Index = !SE.Name.Contains("Knife") ? int.Parse(SE.Name[6].ToString()) - 1 : 9;
            int SelectedPlayer = int.Parse(SE.Name[1].ToString()) - 1;

            ComboBoxEdit ItemCombo = (ComboBoxEdit)GetInventoryControl(SelectedPlayer, Index, $"P{SelectedPlayer + 1}Slot" + (Index != 9 ? (Index + 1).ToString() : "Knife") + "ItemCB");
            Item SelectedItem = ItemCombo.SelectedItem as Item;

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

            Slot[] SlotCollection = Biohazard.Players[SelectedPlayer].Inventory.LoadoutSlots;

            if (!SE.Name.Contains("ReloadSpeed"))
            {
                SpinEdit ReloadSpeedSE = (SpinEdit)GetInventoryControl(SelectedPlayer, Index, $"P{SelectedPlayer + 1}Slot" + (Index != 9 ? (Index + 1).ToString() : "Knife") + "ReloadSpeedSE");
                ReloadSpeedSE.Text = ((decimal)SlotCollection[Index].ToMemoryItem.ReloadSpeed[SlotCollection[Index].ReloadSpeed]).ToString("F");
            }

            if ((e.IsSpinUp && PropArray.Length > NewValueIndex) || (!e.IsSpinUp && NewValueIndex >= 0))
            {
                SE.Text = SE.Name.Contains("ReloadSpeed") ? ((decimal)PropArray[NewValueIndex]).ToString("F") : PropArray[NewValueIndex].ToString();

                if (SE.Name.Contains("Capacity"))
                {
                    TextEdit QuantityTE = (TextEdit)GetInventoryControl(SelectedPlayer, Index, $"P{SelectedPlayer + 1}Slot" + (Index != 9 ? (Index + 1).ToString() : "Knife") + "QuantityTE");
                    TextEdit MaxQuantityTE = (TextEdit)GetInventoryControl(SelectedPlayer, Index, $"P{SelectedPlayer + 1}Slot" + (Index != 9 ? (Index + 1).ToString() : "Knife") + "MaxQuantityTE");

                    QuantityTE.Text = PropArray[NewValueIndex].ToString();
                    MaxQuantityTE.Text = PropArray[NewValueIndex].ToString();
                }

                SlotCollection[Index].UpdateItemToMemory(Initialized);
            }
            else
                e.Handled = true;
        }

        private void InventoryTextEditNumeric_EditValueChanging(object sender, ChangingEventArgs e)
        {
            bool IsValid = int.TryParse(e.NewValue.ToString(), out _);

            if (!IsValid)
                e.NewValue = "0";

            TextEdit TE = sender as TextEdit;
            int Index = !TE.Name.Contains("Knife") ? int.Parse(TE.Name[6].ToString()) - 1 : 9;
            int SelectedPlayer = int.Parse(TE.Name[1].ToString()) - 1;

            Slot[] SlotCollection = Biohazard.Players[SelectedPlayer].Inventory.LoadoutSlots;

            ComboBoxEdit CBE = (ComboBoxEdit)GetInventoryControl(SelectedPlayer, Index, Index != 9 ? $"P{SelectedPlayer + 1}Slot{Index + 1}ItemCB" : $"P{SelectedPlayer + 1}SlotKnifeItemCB");
            Item SelectedItem = CBE.SelectedItem as Item;

            int NewValue = int.Parse(e.NewValue.ToString());
            NewValue = Utility.Clamp(NewValue, 0, SelectedItem.Capacity[SelectedItem.Capacity.Length - 1]);

            if (TE.Name.Contains("MaxQuantity"))
                SlotCollection[Index].ToMemory.MaxQuantity = NewValue;
            else if (TE.Name.Contains("Quantity"))
                SlotCollection[Index].ToMemory.Quantity = NewValue;
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
            int SelectedPlayer = int.Parse(CBE.Name[1].ToString()) - 1;
            Slot[] SlotCollection = Biohazard.Players[SelectedPlayer].Inventory.LoadoutSlots;

            Item SelectedItem = CBE.SelectedItem as Item;

            if ((int)InventoryPlayerSelectionRG.EditValue == SelectedPlayer)
            {
                Image Final = ImageHelper.GetImageFromFile(@"addons/GameX.Biohazard.5/images/inventory/" + SelectedItem.Alias + ".png");
                SlotPicBoxes[Index].Image = Final;
            }

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

            Biohazard.Players[SelectedPlayer].Inventory.LoadoutSlots[Index].UpdateItemToMemory(Initialized);
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

            Image Portrait = ImageHelper.GetImageFromFile(@"addons/GameX.Biohazard.5/images/character/" + CBECos.Portrait);

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

                    CE.CheckedChanged -= EnableDisable_StateChanged;
                    CE.Checked = Biohazard.EnableReunionSpecialMoves(CE.Checked);
                    CE.CheckedChanged += EnableDisable_StateChanged;
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
                else if (CE.Name.Contains("Slot"))
                {
                    int Index = !CE.Name.Contains("Knife") ? int.Parse(CE.Name[6].ToString()) - 1 : 9;
                    int SelectedPlayer = int.Parse(CE.Name[1].ToString()) - 1;
                    Biohazard.Players[SelectedPlayer].Inventory.LoadoutSlots[Index].UpdateItemToMemory(Initialized);
                }
                else if (CE.Name.Equals("HandTremorCheckEdit"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.DisableHandTremor(CE.Checked);
                }
                else if (CE.Name.Equals("NoTimerDecreaseCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.NoTimerDecrease(CE.Checked);
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
                else if (CB.Name.Contains("InfiniteResource") && !CB.Checked)
                {
                    if (!Initialized)
                        return;

                    int Player = int.Parse(CB.Name[1].ToString()) - 1;
                    Biohazard.Players[Player].SetInfiniteResource(false);
                }
                else if (CB.Name.Contains("InfiniteThrowable") && !CB.Checked)
                {
                    if (!Initialized)
                        return;

                    int Player = int.Parse(CB.Name[1].ToString()) - 1;
                    Biohazard.Players[Player].SetInfiniteThrowable(false);
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
                else if (SB.Name.Equals("ControllerAimButton"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.EnableControllerAim(SB.Text.Equals("Disable"));
                }
            }
        }

        private void RadioGroup_EditValueChanged(object sender, EventArgs e)
        {
            RadioGroup RG = sender as RadioGroup;

            if (RG.Name.Equals("InventoryPlayerSelectionRG"))
            {
                int SelectedPlayer = (int)InventoryPlayerSelectionRG.EditValue;
                Slot[] SlotCollection = Biohazard.Players[SelectedPlayer].Inventory.LoadoutSlots;

                PlayerInventoryTabControl.SelectedTabPageIndex = SelectedPlayer;

                for (int i = 0; i < 10; i++)
                {
                    SlotCollection[i].BeingUpdatedOnGUI = true;

                    ComboBoxEdit CBE = (ComboBoxEdit)GetInventoryControl(SelectedPlayer, i, i != 9 ? $"P{SelectedPlayer + 1}Slot{i + 1}ItemCB" : $"P{SelectedPlayer + 1}SlotKnifeItemCB");
                    InventoryComboBox_IndexChanged(CBE, null);

                    SlotCollection[i].BeingUpdatedOnGUI = false;
                }
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

        #endregion

        #region GameX Calls

        private void GameX_CheckControls()
        {
            SimpleButton[] EnableDisable =
            {
                ColorFilterButton,
                ControllerAimButton
            };

            CheckEdit[] CheckUncheck =
            {
                WeskerGlassesCheckEdit,
                MeleeCameraCheckEdit,
                ReunionSpecialMovesCheckEdit,
                HandTremorCheckEdit,
                NoTimerDecreaseCE
            };

            foreach (CheckEdit CE in CheckUncheck)
            {
                switch (CE.Name)
                {
                    case "WeskerGlassesCheckEdit":
                        if (CE.Checked)
                            Biohazard.WeskerNoSunglassDrop(true);
                        break;
                    case "MeleeCameraCheckEdit":
                        if (CE.Checked)
                            Biohazard.DisableMeleeCamera(true);
                        break;
                    case "ReunionSpecialMovesCheckEdit":
                        if (CE.Checked)
                        {
                            CE.CheckedChanged -= EnableDisable_StateChanged;
                            CE.Checked = Biohazard.EnableReunionSpecialMoves(true);
                            CE.CheckedChanged += EnableDisable_StateChanged;
                        }
                        break;
                    case "HandTremorCheckEdit":
                        if (CE.Checked)
                            Biohazard.DisableHandTremor(true);
                        break;
                    case "NoTimerDecreaseCE":
                        if (CE.Checked)
                            Biohazard.NoTimerDecrease(true);
                        break;
                }
            }

            foreach (SimpleButton SB in EnableDisable)
            {
                switch (SB.Name)
                {
                    case "ColorFilterButton":
                        if (SB.Text == "Disable")
                            Biohazard.EnableColorFilter(true);

                        break;
                    case "ControllerAimButton":
                        if (SB.Text == "Disable")
                            Biohazard.EnableControllerAim(true);

                        break;
                }
            }

            Melee_ApplyComboBox(null, true);
            WeaponPlacement_IndexChanged(WeaponPlacementComboBox, null);
            RadioGroup_EditValueChanged(InventoryPlayerSelectionRG, null);
        }

        private void GameX_Start()
        {
            try
            {
                Biohazard.StartModule();
                Biohazard.NoFileChecking(true);
                Biohazard.OnlineCharSwapFixes(true);

                GameX_CheckControls();
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex);
            }
        }

        private void GameX_End()
        {
            try
            {
                ScoreTE.Text = "0";
                ComboTimerTE.Text = "00:00";
                ComboBonusTimerTE.Text = "00:00";
                CurrentTimerTE.Text = "00:00:00";

                if (!Biohazard.ModuleStarted)
                    return;

                if (MeleeAnytimeSwitch.IsOn)
                    MeleeAnytimeSwitch.Toggle();

                Biohazard.NoFileChecking(false);
                Biohazard.OnlineCharSwapFixes(false);
                //Biohazard.EnableColorFilter(false);
                //Biohazard.WeskerNoSunglassDrop(false);
                //Biohazard.WeskerNoDashCost(false);
                //Biohazard.SetWeaponPlacement(0);
                //Biohazard.DisableMeleeCamera(false);
                //Biohazard.EnableReunionSpecialMoves(false);
                //Biohazard.EnableControllerAim(false);

                Biohazard.FinishModule();
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex);
            }
        }

        private void GameX_Update()
        {
            try
            {
                if (!Biohazard.ModuleStarted)
                    return;

                TrainerUpdate();
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex);
            }
        }

        private static void GameX_Keyboard(int input)
        {

        }

        #endregion

        #region Methods

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

        private void TrainerUpdate()
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

            CheckButton[] InfiniteResource =
            {
                P1InfiniteResourceButton,
                P2InfiniteResourceButton,
                P3InfiniteResourceButton,
                P4InfiniteResourceButton
            };

            CheckButton[] InfiniteThrowable =
            {
                P1InfiniteThrowableButton,
                P2InfiniteThrowableButton,
                P3InfiniteThrowableButton,
                P4InfiniteThrowableButton
            };

            CheckButton[] RapidFire =
            {
                P1RapidfireButton,
                P2RapidfireButton,
                P3RapidfireButton,
                P4RapidfireButton
            };

            #endregion

            #region Game Mode Update

            ScoreTE.Text = Biohazard.Score.ToString();
            ComboTimerTE.Text = TimeSpan.FromSeconds(Biohazard.ComboTimer).ToString("mm':'ss");
            ComboBonusTimerTE.Text = TimeSpan.FromSeconds(Biohazard.ComboBonusTimer).ToString("mm':'ss");
            CurrentTimerTE.Text = TimeSpan.FromSeconds(Biohazard.Timer).ToString("hh':'mm':'ss");

            if (Biohazard.GameMode == (int)GameModeEnum.Mercenaries || Biohazard.GameMode == (int)GameModeEnum.Reunion)
            {
                if (ResetScoreCE.Checked && Biohazard.Timer == 0 && Biohazard.Score > 0)
                    Biohazard.Score = 0;

                if (ComboTimerMaxCE.Checked)
                    Biohazard.ComboTimer = Biohazard.ComboTimerDuration;

                if (ComboBonusTimerMaxCE.Checked)
                    Biohazard.ComboBonusTimer = Biohazard.ComboBonusTimerDuration;
            }

            #endregion

            #region Character Update

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
                        int CharValue = (CharacterCombos[i].SelectedItem as Character).Value;
                        int Cosvalue = (CostumeCombos[i].SelectedItem as Costume).Value;

                        if (i < 2)
                            Biohazard.SetStoryModeCharacter(i, CharValue, Cosvalue);

                        Biohazard.Players[i].Character = CharValue;
                        Biohazard.Players[i].Costume = Cosvalue;
                    }
                }

                // Inventory //
                for (int slot = 0; slot < 10; slot++)
                    Biohazard.Players[i].Inventory.LoadoutSlots[slot].Update();

                // Health Bar //
                double PlayerHealthPercent = PlayerPresent ? Utility.Clamp((double)Biohazard.Players[i].Health / Biohazard.Players[i].MaxHealth, 0.0, 1.0) : 1.0;
                PlayerHealthPercent = double.IsNaN(PlayerHealthPercent) ? 0.0 : PlayerHealthPercent;

                HealthBars[i].Properties.Maximum = PlayerPresent ? Biohazard.Players[i].MaxHealth : 1;
                HealthBars[i].EditValue = PlayerPresent ? Biohazard.Players[i].Health : 1;
                HealthBars[i].Properties.StartColor = PlayerPresent ? Color.FromArgb((int)(255.0 - (155.0 * PlayerHealthPercent)), (int)(0.0 + (255.0 * PlayerHealthPercent)), 0) : Color.FromArgb(0, 0, 0, 0);
                HealthBars[i].Properties.EndColor = PlayerPresent ? Color.FromArgb((int)(255.0 - (155.0 * PlayerHealthPercent)), (int)(0.0 + (255.0 * PlayerHealthPercent)), 0) : Color.FromArgb(0, 0, 0, 0);

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

                // Wesker Dash cost
                Biohazard.WeskerNoDashCost(WeskerNoDashCostCheckEdit.Checked); // && Biohazard.GameMode != (int)GameModeEnum.Versus

                // If versus then end the current iteration //
                // if (Biohazard.GameMode == (int)GameModeEnum.Versus)
                //continue;

                // Infinite HP //
                if (InfiniteHP[i].Checked && PlayerPresent)
                    Biohazard.Players[i].Health = Biohazard.Players[i].MaxHealth;

                // Untergetable //
                if (Untargetable[i].Checked && PlayerPresent)
                    Biohazard.Players[i].Invulnerable = true;

                // Infinite Ammo //
                if (InfiniteAmmo[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetInfiniteAmmo(true);

                // Infinite Resource //
                if (InfiniteResource[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetInfiniteResource(true);

                // Infinite Throwable //
                if (InfiniteThrowable[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetInfiniteThrowable(true);

                // Rapidfire //
                if (RapidFire[i].Checked && PlayerPresent)
                    Biohazard.Players[i].SetRapidFire(true);

                // Infinite Dash //
                if (WeskerInfiniteDashCheckEdit.Checked && PlayerPresent && Biohazard.Players[i].Character == (int)CharacterEnum.Wesker)
                    Biohazard.Players[i].ResetDash();
            }

            #endregion
        }

        #endregion
    }
}