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
using GameX.Modules.Sub;
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
                TabPageVocalizerButton,
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

            CheckEdit[] CheckUncheck =
            {
                WeskerNoWeaponOnChestCE,
                WeskerGlassesCE,
                WeskerInfiniteDashCE,
                WeskerNoDashCostCE
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

            ComboBoxEdit[] HotkeyCombos =
            {
                VocalizerHotkeyG1CB,
                VocalizerHotkeyG2CB,
                VocalizerHotkeyG3CB,
                VocalizerHotkeyG4CB,
                VocalizerHotkeyG5CB,
                VocalizerHotkeyG6CB,
                VocalizerHotkeyG7CB,
                VocalizerHotkeyG8CB,
                VocalizerHotkeyG9CB,
            };

            ComboBoxEdit[] VocalizerSpeechChrisCombos =
            {
                VocalizerChrisG1S1CB,
                VocalizerChrisG1S2CB,
                VocalizerChrisG1S3CB,
                VocalizerChrisG1S4CB,
                VocalizerChrisG1S5CB,

                VocalizerChrisG2S1CB,
                VocalizerChrisG2S2CB,
                VocalizerChrisG2S3CB,
                VocalizerChrisG2S4CB,
                VocalizerChrisG2S5CB,

                VocalizerChrisG3S1CB,
                VocalizerChrisG3S2CB,
                VocalizerChrisG3S3CB,
                VocalizerChrisG3S4CB,
                VocalizerChrisG3S5CB,

                VocalizerChrisG4S1CB,
                VocalizerChrisG4S2CB,
                VocalizerChrisG4S3CB,
                VocalizerChrisG4S4CB,
                VocalizerChrisG4S5CB,

                VocalizerChrisG5S1CB,
                VocalizerChrisG5S2CB,
                VocalizerChrisG5S3CB,
                VocalizerChrisG5S4CB,
                VocalizerChrisG5S5CB,

                VocalizerChrisG6S1CB,
                VocalizerChrisG6S2CB,
                VocalizerChrisG6S3CB,
                VocalizerChrisG6S4CB,
                VocalizerChrisG6S5CB,

                VocalizerChrisG7S1CB,
                VocalizerChrisG7S2CB,
                VocalizerChrisG7S3CB,
                VocalizerChrisG7S4CB,
                VocalizerChrisG7S5CB,

                VocalizerChrisG8S1CB,
                VocalizerChrisG8S2CB,
                VocalizerChrisG8S3CB,
                VocalizerChrisG8S4CB,
                VocalizerChrisG8S5CB,

                VocalizerChrisG9S1CB,
                VocalizerChrisG9S2CB,
                VocalizerChrisG9S3CB,
                VocalizerChrisG9S4CB,
                VocalizerChrisG9S5CB,
            };

            ComboBoxEdit[] VocalizerSpeechShevaCombos =
            {
                VocalizerShevaG1S1CB,
                VocalizerShevaG1S2CB,
                VocalizerShevaG1S3CB,
                VocalizerShevaG1S4CB,
                VocalizerShevaG1S5CB,

                VocalizerShevaG2S1CB,
                VocalizerShevaG2S2CB,
                VocalizerShevaG2S3CB,
                VocalizerShevaG2S4CB,
                VocalizerShevaG2S5CB,

                VocalizerShevaG3S1CB,
                VocalizerShevaG3S2CB,
                VocalizerShevaG3S3CB,
                VocalizerShevaG3S4CB,
                VocalizerShevaG3S5CB,

                VocalizerShevaG4S1CB,
                VocalizerShevaG4S2CB,
                VocalizerShevaG4S3CB,
                VocalizerShevaG4S4CB,
                VocalizerShevaG4S5CB,

                VocalizerShevaG5S1CB,
                VocalizerShevaG5S2CB,
                VocalizerShevaG5S3CB,
                VocalizerShevaG5S4CB,
                VocalizerShevaG5S5CB,

                VocalizerShevaG6S1CB,
                VocalizerShevaG6S2CB,
                VocalizerShevaG6S3CB,
                VocalizerShevaG6S4CB,
                VocalizerShevaG6S5CB,

                VocalizerShevaG7S1CB,
                VocalizerShevaG7S2CB,
                VocalizerShevaG7S3CB,
                VocalizerShevaG7S4CB,
                VocalizerShevaG7S5CB,

                VocalizerShevaG8S1CB,
                VocalizerShevaG8S2CB,
                VocalizerShevaG8S3CB,
                VocalizerShevaG8S4CB,
                VocalizerShevaG8S5CB,

                VocalizerShevaG9S1CB,
                VocalizerShevaG9S2CB,
                VocalizerShevaG9S3CB,
                VocalizerShevaG9S4CB,
                VocalizerShevaG9S5CB,
            };

            ComboBoxEdit[] VocalizerSpeechJillCombos =
            {
                VocalizerJillG1S1CB,
                VocalizerJillG1S2CB,
                VocalizerJillG1S3CB,
                VocalizerJillG1S4CB,
                VocalizerJillG1S5CB,

                VocalizerJillG2S1CB,
                VocalizerJillG2S2CB,
                VocalizerJillG2S3CB,
                VocalizerJillG2S4CB,
                VocalizerJillG2S5CB,

                VocalizerJillG3S1CB,
                VocalizerJillG3S2CB,
                VocalizerJillG3S3CB,
                VocalizerJillG3S4CB,
                VocalizerJillG3S5CB,

                VocalizerJillG4S1CB,
                VocalizerJillG4S2CB,
                VocalizerJillG4S3CB,
                VocalizerJillG4S4CB,
                VocalizerJillG4S5CB,

                VocalizerJillG5S1CB,
                VocalizerJillG5S2CB,
                VocalizerJillG5S3CB,
                VocalizerJillG5S4CB,
                VocalizerJillG5S5CB,

                VocalizerJillG6S1CB,
                VocalizerJillG6S2CB,
                VocalizerJillG6S3CB,
                VocalizerJillG6S4CB,
                VocalizerJillG6S5CB,

                VocalizerJillG7S1CB,
                VocalizerJillG7S2CB,
                VocalizerJillG7S3CB,
                VocalizerJillG7S4CB,
                VocalizerJillG7S5CB,

                VocalizerJillG8S1CB,
                VocalizerJillG8S2CB,
                VocalizerJillG8S3CB,
                VocalizerJillG8S4CB,
                VocalizerJillG8S5CB,

                VocalizerJillG9S1CB,
                VocalizerJillG9S2CB,
                VocalizerJillG9S3CB,
                VocalizerJillG9S4CB,
                VocalizerJillG9S5CB,
            };

            ComboBoxEdit[] VocalizerSpeechWeskerCombos =
            {
                VocalizerWeskerG1S1CB,
                VocalizerWeskerG1S2CB,
                VocalizerWeskerG1S3CB,
                VocalizerWeskerG1S4CB,
                VocalizerWeskerG1S5CB,

                VocalizerWeskerG2S1CB,
                VocalizerWeskerG2S2CB,
                VocalizerWeskerG2S3CB,
                VocalizerWeskerG2S4CB,
                VocalizerWeskerG2S5CB,

                VocalizerWeskerG3S1CB,
                VocalizerWeskerG3S2CB,
                VocalizerWeskerG3S3CB,
                VocalizerWeskerG3S4CB,
                VocalizerWeskerG3S5CB,

                VocalizerWeskerG4S1CB,
                VocalizerWeskerG4S2CB,
                VocalizerWeskerG4S3CB,
                VocalizerWeskerG4S4CB,
                VocalizerWeskerG4S5CB,

                VocalizerWeskerG5S1CB,
                VocalizerWeskerG5S2CB,
                VocalizerWeskerG5S3CB,
                VocalizerWeskerG5S4CB,
                VocalizerWeskerG5S5CB,

                VocalizerWeskerG6S1CB,
                VocalizerWeskerG6S2CB,
                VocalizerWeskerG6S3CB,
                VocalizerWeskerG6S4CB,
                VocalizerWeskerG6S5CB,

                VocalizerWeskerG7S1CB,
                VocalizerWeskerG7S2CB,
                VocalizerWeskerG7S3CB,
                VocalizerWeskerG7S4CB,
                VocalizerWeskerG7S5CB,

                VocalizerWeskerG8S1CB,
                VocalizerWeskerG8S2CB,
                VocalizerWeskerG8S3CB,
                VocalizerWeskerG8S4CB,
                VocalizerWeskerG8S5CB,

                VocalizerWeskerG9S1CB,
                VocalizerWeskerG9S2CB,
                VocalizerWeskerG9S3CB,
                VocalizerWeskerG9S4CB,
                VocalizerWeskerG9S5CB,
            };

            ComboBoxEdit[] VocalizerSpeechJoshCombos =
            {
                VocalizerJoshG1S1CB,
                VocalizerJoshG1S2CB,
                VocalizerJoshG1S3CB,
                VocalizerJoshG1S4CB,
                VocalizerJoshG1S5CB,

                VocalizerJoshG2S1CB,
                VocalizerJoshG2S2CB,
                VocalizerJoshG2S3CB,
                VocalizerJoshG2S4CB,
                VocalizerJoshG2S5CB,

                VocalizerJoshG3S1CB,
                VocalizerJoshG3S2CB,
                VocalizerJoshG3S3CB,
                VocalizerJoshG3S4CB,
                VocalizerJoshG3S5CB,

                VocalizerJoshG4S1CB,
                VocalizerJoshG4S2CB,
                VocalizerJoshG4S3CB,
                VocalizerJoshG4S4CB,
                VocalizerJoshG4S5CB,

                VocalizerJoshG5S1CB,
                VocalizerJoshG5S2CB,
                VocalizerJoshG5S3CB,
                VocalizerJoshG5S4CB,
                VocalizerJoshG5S5CB,

                VocalizerJoshG6S1CB,
                VocalizerJoshG6S2CB,
                VocalizerJoshG6S3CB,
                VocalizerJoshG6S4CB,
                VocalizerJoshG6S5CB,

                VocalizerJoshG7S1CB,
                VocalizerJoshG7S2CB,
                VocalizerJoshG7S3CB,
                VocalizerJoshG7S4CB,
                VocalizerJoshG7S5CB,

                VocalizerJoshG8S1CB,
                VocalizerJoshG8S2CB,
                VocalizerJoshG8S3CB,
                VocalizerJoshG8S4CB,
                VocalizerJoshG8S5CB,

                VocalizerJoshG9S1CB,
                VocalizerJoshG9S2CB,
                VocalizerJoshG9S3CB,
                VocalizerJoshG9S4CB,
                VocalizerJoshG9S5CB,
            };

            ComboBoxEdit[] VocalizerSpeechExcellaCombos =
            {
                VocalizerExcellaG1S1CB,
                VocalizerExcellaG1S2CB,
                VocalizerExcellaG1S3CB,
                VocalizerExcellaG1S4CB,
                VocalizerExcellaG1S5CB,

                VocalizerExcellaG2S1CB,
                VocalizerExcellaG2S2CB,
                VocalizerExcellaG2S3CB,
                VocalizerExcellaG2S4CB,
                VocalizerExcellaG2S5CB,

                VocalizerExcellaG3S1CB,
                VocalizerExcellaG3S2CB,
                VocalizerExcellaG3S3CB,
                VocalizerExcellaG3S4CB,
                VocalizerExcellaG3S5CB,

                VocalizerExcellaG4S1CB,
                VocalizerExcellaG4S2CB,
                VocalizerExcellaG4S3CB,
                VocalizerExcellaG4S4CB,
                VocalizerExcellaG4S5CB,

                VocalizerExcellaG5S1CB,
                VocalizerExcellaG5S2CB,
                VocalizerExcellaG5S3CB,
                VocalizerExcellaG5S4CB,
                VocalizerExcellaG5S5CB,

                VocalizerExcellaG6S1CB,
                VocalizerExcellaG6S2CB,
                VocalizerExcellaG6S3CB,
                VocalizerExcellaG6S4CB,
                VocalizerExcellaG6S5CB,

                VocalizerExcellaG7S1CB,
                VocalizerExcellaG7S2CB,
                VocalizerExcellaG7S3CB,
                VocalizerExcellaG7S4CB,
                VocalizerExcellaG7S5CB,

                VocalizerExcellaG8S1CB,
                VocalizerExcellaG8S2CB,
                VocalizerExcellaG8S3CB,
                VocalizerExcellaG8S4CB,
                VocalizerExcellaG8S5CB,

                VocalizerExcellaG9S1CB,
                VocalizerExcellaG9S2CB,
                VocalizerExcellaG9S3CB,
                VocalizerExcellaG9S4CB,
                VocalizerExcellaG9S5CB,
            };

            ComboBoxEdit[] VocalizerSpeechBarryCombos =
            {
                VocalizerBarryG1S1CB,
                VocalizerBarryG1S2CB,
                VocalizerBarryG1S3CB,
                VocalizerBarryG1S4CB,
                VocalizerBarryG1S5CB,

                VocalizerBarryG2S1CB,
                VocalizerBarryG2S2CB,
                VocalizerBarryG2S3CB,
                VocalizerBarryG2S4CB,
                VocalizerBarryG2S5CB,

                VocalizerBarryG3S1CB,
                VocalizerBarryG3S2CB,
                VocalizerBarryG3S3CB,
                VocalizerBarryG3S4CB,
                VocalizerBarryG3S5CB,

                VocalizerBarryG4S1CB,
                VocalizerBarryG4S2CB,
                VocalizerBarryG4S3CB,
                VocalizerBarryG4S4CB,
                VocalizerBarryG4S5CB,

                VocalizerBarryG5S1CB,
                VocalizerBarryG5S2CB,
                VocalizerBarryG5S3CB,
                VocalizerBarryG5S4CB,
                VocalizerBarryG5S5CB,

                VocalizerBarryG6S1CB,
                VocalizerBarryG6S2CB,
                VocalizerBarryG6S3CB,
                VocalizerBarryG6S4CB,
                VocalizerBarryG6S5CB,

                VocalizerBarryG7S1CB,
                VocalizerBarryG7S2CB,
                VocalizerBarryG7S3CB,
                VocalizerBarryG7S4CB,
                VocalizerBarryG7S5CB,

                VocalizerBarryG8S1CB,
                VocalizerBarryG8S2CB,
                VocalizerBarryG8S3CB,
                VocalizerBarryG8S4CB,
                VocalizerBarryG8S5CB,

                VocalizerBarryG9S1CB,
                VocalizerBarryG9S2CB,
                VocalizerBarryG9S3CB,
                VocalizerBarryG9S4CB,
                VocalizerBarryG9S5CB,
            };

            ComboBoxEdit[] VocalizerSpeechRebeccaCombos =
            {
                VocalizerRebeccaG1S1CB,
                VocalizerRebeccaG1S2CB,
                VocalizerRebeccaG1S3CB,
                VocalizerRebeccaG1S4CB,
                VocalizerRebeccaG1S5CB,

                VocalizerRebeccaG2S1CB,
                VocalizerRebeccaG2S2CB,
                VocalizerRebeccaG2S3CB,
                VocalizerRebeccaG2S4CB,
                VocalizerRebeccaG2S5CB,

                VocalizerRebeccaG3S1CB,
                VocalizerRebeccaG3S2CB,
                VocalizerRebeccaG3S3CB,
                VocalizerRebeccaG3S4CB,
                VocalizerRebeccaG3S5CB,

                VocalizerRebeccaG4S1CB,
                VocalizerRebeccaG4S2CB,
                VocalizerRebeccaG4S3CB,
                VocalizerRebeccaG4S4CB,
                VocalizerRebeccaG4S5CB,

                VocalizerRebeccaG5S1CB,
                VocalizerRebeccaG5S2CB,
                VocalizerRebeccaG5S3CB,
                VocalizerRebeccaG5S4CB,
                VocalizerRebeccaG5S5CB,

                VocalizerRebeccaG6S1CB,
                VocalizerRebeccaG6S2CB,
                VocalizerRebeccaG6S3CB,
                VocalizerRebeccaG6S4CB,
                VocalizerRebeccaG6S5CB,

                VocalizerRebeccaG7S1CB,
                VocalizerRebeccaG7S2CB,
                VocalizerRebeccaG7S3CB,
                VocalizerRebeccaG7S4CB,
                VocalizerRebeccaG7S5CB,

                VocalizerRebeccaG8S1CB,
                VocalizerRebeccaG8S2CB,
                VocalizerRebeccaG8S3CB,
                VocalizerRebeccaG8S4CB,
                VocalizerRebeccaG8S5CB,

                VocalizerRebeccaG9S1CB,
                VocalizerRebeccaG9S2CB,
                VocalizerRebeccaG9S3CB,
                VocalizerRebeccaG9S4CB,
                VocalizerRebeccaG9S5CB,
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

            string[] DefaultHotkeys =
            {
                "F1",
                "F2",
                "F3",
                "F4",
                "F5",
                "F6",
                "F7",
                "F8",
                "F9"
            };

            for (int i = 0; i < HotkeyCombos.Length; i++)
            {
                HotkeyCombos[i].Properties.Items.AddRange(db.Hotkeys);
                HotkeyCombos[i].SelectedIndexChanged += HotkeysCombo_SelectedIndexChanged;
                HotkeyCombos[i].SelectedIndex = db.Hotkeys.IndexOf(db.Hotkeys.First(x => x.Name == DefaultHotkeys[i]));
            }

            for (int i = 0; i < 45; i++)
            {
                VocalizerSpeechChrisCombos[i].Properties.Items.AddRange(db.AllSpeech.Where(x => x.Character == CharacterEnum.Chris).First().Lines);
                VocalizerSpeechChrisCombos[i].SelectedIndex = 0;

                VocalizerSpeechShevaCombos[i].Properties.Items.AddRange(db.AllSpeech.Where(x => x.Character == CharacterEnum.Sheva).First().Lines);
                VocalizerSpeechShevaCombos[i].SelectedIndex = 0;

                VocalizerSpeechJillCombos[i].Properties.Items.AddRange(db.AllSpeech.Where(x => x.Character == CharacterEnum.Jill).First().Lines);
                VocalizerSpeechJillCombos[i].SelectedIndex = 0;

                VocalizerSpeechWeskerCombos[i].Properties.Items.AddRange(db.AllSpeech.Where(x => x.Character == CharacterEnum.Wesker).First().Lines);
                VocalizerSpeechWeskerCombos[i].SelectedIndex = 0;

                VocalizerSpeechJoshCombos[i].Properties.Items.AddRange(db.AllSpeech.Where(x => x.Character == CharacterEnum.Josh).First().Lines);
                VocalizerSpeechJoshCombos[i].SelectedIndex = 0;

                VocalizerSpeechExcellaCombos[i].Properties.Items.AddRange(db.AllSpeech.Where(x => x.Character == CharacterEnum.Excella).First().Lines);
                VocalizerSpeechExcellaCombos[i].SelectedIndex = 0;

                VocalizerSpeechBarryCombos[i].Properties.Items.AddRange(db.AllSpeech.Where(x => x.Character == CharacterEnum.Barry).First().Lines);
                VocalizerSpeechBarryCombos[i].SelectedIndex = 0;

                VocalizerSpeechRebeccaCombos[i].Properties.Items.AddRange(db.AllSpeech.Where(x => x.Character == CharacterEnum.Rebecca).First().Lines);
                VocalizerSpeechRebeccaCombos[i].SelectedIndex = 0;
            }

            P1FreezeAllCheckEdit.CheckedChanged += EnableDisable_StateChanged;
            P2FreezeAllCheckEdit.CheckedChanged += EnableDisable_StateChanged;
            P3FreezeAllCheckEdit.CheckedChanged += EnableDisable_StateChanged;
            P4FreezeAllCheckEdit.CheckedChanged += EnableDisable_StateChanged;

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

            MeleeCameraCE.CheckedChanged += EnableDisable_StateChanged;
            ReunionSpecialMovesCE.CheckedChanged += EnableDisable_StateChanged;
            StunRodMeleeKillCE.CheckedChanged += EnableDisable_StateChanged;
            HandTremorCE.CheckedChanged += EnableDisable_StateChanged;
            NoTimerDecreaseCE.CheckedChanged += EnableDisable_StateChanged;
            NoFPSCapCE.CheckedChanged += EnableDisable_StateChanged;
            ResetScoreCE.CheckedChanged += EnableDisable_StateChanged;

            ConsoleInputTextEdit.Validating += Terminal.ValidateInput;
            ClearConsoleSimpleButton.Click += Terminal.ClearConsole_Click;

            InventoryPlayerSelectionRG.EditValueChanged += RadioGroup_EditValueChanged;

            ControllerAimButton.Click += EnableDisable_StateChanged;
            ColorFilterButton.Click += EnableDisable_StateChanged;

            LoadoutSaveButton.Click += SimpleButton_Click;
            LoadoutApplyButton.Click += SimpleButton_Click;
            LoadoutComboBox.Properties.Items.AddRange(db.Loadouts);

            AddMinuteButton.Click += SimpleButton_Click;
            RemoveMinuteButton.Click += SimpleButton_Click;
            ZeroTimeButton.Click += SimpleButton_Click;

            MeleeKillCB.Properties.Items.AddRange(db.MeleeKillSeconds);
            MeleeKillCB.SelectedIndexChanged += TimerCombos_SelectedIndexChanged;
            MeleeKillCB.SelectedIndex = 0;

            ComboTimerCB.Properties.Items.AddRange(db.ComboTimerSeconds);
            ComboTimerCB.SelectedIndexChanged += TimerCombos_SelectedIndexChanged;
            ComboTimerCB.SelectedIndex = 0;

            ComboBonusTimerCB.Properties.Items.AddRange(db.ComboBonusTimerSeconds);
            ComboBonusTimerCB.SelectedIndexChanged += TimerCombos_SelectedIndexChanged;
            ComboBonusTimerCB.SelectedIndex = 0;

            MapsCB.Properties.Items.AddRange(db.Maps);
            MapsCB.SelectedIndexChanged += MapsCombo_SelectedIndexChanged;
            MapsCB.SelectedIndex = 0;

            if (db.Loadouts.Count > 0)
                LoadoutComboBox.SelectedIndex = 0;

            MasterTabControl.SelectedTabPageIndex = 0;

            VocalizerCharSelectCB.Properties.Items.AddRange(db.Characters.Where(x => x.Name != "Irving").ToList());
            VocalizerCharSelectCB.SelectedIndexChanged += VocalizerCharCombo_IndexChanged;
            VocalizerCharSelectCB.SelectedIndex = 0;

            VocalizerTabControl.SelectedTabPageIndex = 0;
            VocalizerEnableCE.Enabled = false;

            ResetHealthBars();
            SetupImages();

            RadioGroup_EditValueChanged(InventoryPlayerSelectionRG, null);

            Configuration_Load(null, null);

            Terminal.WriteLine("[App] App initialized.");
        }

        private void Application_Init()
        {
            Target_Setup();

            Keyboard.CreateHook(GameX_Keyboard);
            Application.ApplicationExit += Application_ApplicationExit;

            Terminal.Setup(this);
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
                GameX_Start();

                Target_Process.EnableRaisingEvents = true;
                Target_Process.Exited += Target_Exited;
                Verified = true;
                Initialized = true;
                Text = "GameX - Resident Evil 5 - " + (Memory.DebugMode ? "Running as administrator" : "Running as normal user");

                return Verified;
            }

            Terminal.WriteLine("[App] Failed validating, unsupported version.");
            Terminal.WriteLine("[App] Read the instructions at https://github.com/LuBuCake/GameX/blob/main/README.md to learn how to download and install the latest patch available.");

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

            Target_Process?.Dispose();
            Target_Process = null;
            Verified = false;
            Initialized = false;

            ScoreTE.Text = "0";
            ComboTimerTE.Text = "00:00";
            ComboBonusTimerTE.Text = "00:00";
            CurrentTimerTE.Text = "00:00:00";

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

        private void Melee_ApplyComboBox(ComboBoxEdit CE, bool ApplyAll = false)
        {
            #region Controls

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

            #endregion

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

        private int ChooseVocalizerHotkey(int Group, bool SetHotkey = false, int Key = 0)
        {
            DB db = DBContext.GetDatabase();

            #region Controls

            ComboBoxEdit[] HotkeyCombos =
            {
                VocalizerHotkeyG1CB,
                VocalizerHotkeyG2CB,
                VocalizerHotkeyG3CB,
                VocalizerHotkeyG4CB,
                VocalizerHotkeyG5CB,
                VocalizerHotkeyG6CB,
                VocalizerHotkeyG7CB,
                VocalizerHotkeyG8CB,
                VocalizerHotkeyG9CB,
            };

            #endregion

            if (SetHotkey)
                HotkeyCombos[Group].SelectedIndex = db.Hotkeys.IndexOf(db.Hotkeys.First(x => x.Key == Key));

            return (HotkeyCombos[Group].SelectedItem as Hotkey).Key;
        }

        private int ChooseVocalizerLine(int Character, int Group, int Slot, bool SetLine = false, int Line = 0)
        {
            DB db = DBContext.GetDatabase();

            #region Controls

            #region Chris

            ComboBoxEdit[] ChrisGroup1Combos =
            {
                VocalizerChrisG1S1CB,
                VocalizerChrisG1S2CB,
                VocalizerChrisG1S3CB,
                VocalizerChrisG1S4CB,
                VocalizerChrisG1S5CB
            };

            ComboBoxEdit[] ChrisGroup2Combos =
            {
                VocalizerChrisG2S1CB,
                VocalizerChrisG2S2CB,
                VocalizerChrisG2S3CB,
                VocalizerChrisG2S4CB,
                VocalizerChrisG2S5CB
            };

            ComboBoxEdit[] ChrisGroup3Combos =
            {
                VocalizerChrisG3S1CB,
                VocalizerChrisG3S2CB,
                VocalizerChrisG3S3CB,
                VocalizerChrisG3S4CB,
                VocalizerChrisG3S5CB
            };

            ComboBoxEdit[] ChrisGroup4Combos =
            {
                VocalizerChrisG4S1CB,
                VocalizerChrisG4S2CB,
                VocalizerChrisG4S3CB,
                VocalizerChrisG4S4CB,
                VocalizerChrisG4S5CB
            };

            ComboBoxEdit[] ChrisGroup5Combos =
            {
                VocalizerChrisG5S1CB,
                VocalizerChrisG5S2CB,
                VocalizerChrisG5S3CB,
                VocalizerChrisG5S4CB,
                VocalizerChrisG5S5CB
            };

            ComboBoxEdit[] ChrisGroup6Combos =
            {
                VocalizerChrisG6S1CB,
                VocalizerChrisG6S2CB,
                VocalizerChrisG6S3CB,
                VocalizerChrisG6S4CB,
                VocalizerChrisG6S5CB
            };

            ComboBoxEdit[] ChrisGroup7Combos =
            {
                VocalizerChrisG7S1CB,
                VocalizerChrisG7S2CB,
                VocalizerChrisG7S3CB,
                VocalizerChrisG7S4CB,
                VocalizerChrisG7S5CB
            };

            ComboBoxEdit[] ChrisGroup8Combos =
            {
                VocalizerChrisG8S1CB,
                VocalizerChrisG8S2CB,
                VocalizerChrisG8S3CB,
                VocalizerChrisG8S4CB,
                VocalizerChrisG8S5CB
            };

            ComboBoxEdit[] ChrisGroup9Combos =
            {
                VocalizerChrisG9S1CB,
                VocalizerChrisG9S2CB,
                VocalizerChrisG9S3CB,
                VocalizerChrisG9S4CB,
                VocalizerChrisG9S5CB
            };

            List<ComboBoxEdit[]> ChrisGroups = new List<ComboBoxEdit[]>
            {
                ChrisGroup1Combos, ChrisGroup2Combos, ChrisGroup3Combos, ChrisGroup4Combos, ChrisGroup5Combos, ChrisGroup6Combos, ChrisGroup7Combos, ChrisGroup8Combos, ChrisGroup9Combos,
            };

            #endregion

            #region Sheva

            ComboBoxEdit[] ShevaGroup1Combos =
            {
                VocalizerShevaG1S1CB,
                VocalizerShevaG1S2CB,
                VocalizerShevaG1S3CB,
                VocalizerShevaG1S4CB,
                VocalizerShevaG1S5CB
            };

            ComboBoxEdit[] ShevaGroup2Combos =
            {
                VocalizerShevaG2S1CB,
                VocalizerShevaG2S2CB,
                VocalizerShevaG2S3CB,
                VocalizerShevaG2S4CB,
                VocalizerShevaG2S5CB
            };

            ComboBoxEdit[] ShevaGroup3Combos =
            {
                VocalizerShevaG3S1CB,
                VocalizerShevaG3S2CB,
                VocalizerShevaG3S3CB,
                VocalizerShevaG3S4CB,
                VocalizerShevaG3S5CB
            };

            ComboBoxEdit[] ShevaGroup4Combos =
            {
                VocalizerShevaG4S1CB,
                VocalizerShevaG4S2CB,
                VocalizerShevaG4S3CB,
                VocalizerShevaG4S4CB,
                VocalizerShevaG4S5CB
            };

            ComboBoxEdit[] ShevaGroup5Combos =
            {
                VocalizerShevaG5S1CB,
                VocalizerShevaG5S2CB,
                VocalizerShevaG5S3CB,
                VocalizerShevaG5S4CB,
                VocalizerShevaG5S5CB
            };

            ComboBoxEdit[] ShevaGroup6Combos =
            {
                VocalizerShevaG6S1CB,
                VocalizerShevaG6S2CB,
                VocalizerShevaG6S3CB,
                VocalizerShevaG6S4CB,
                VocalizerShevaG6S5CB
            };

            ComboBoxEdit[] ShevaGroup7Combos =
            {
                VocalizerShevaG7S1CB,
                VocalizerShevaG7S2CB,
                VocalizerShevaG7S3CB,
                VocalizerShevaG7S4CB,
                VocalizerShevaG7S5CB
            };

            ComboBoxEdit[] ShevaGroup8Combos =
            {
                VocalizerShevaG8S1CB,
                VocalizerShevaG8S2CB,
                VocalizerShevaG8S3CB,
                VocalizerShevaG8S4CB,
                VocalizerShevaG8S5CB
            };

            ComboBoxEdit[] ShevaGroup9Combos =
            {
                VocalizerShevaG9S1CB,
                VocalizerShevaG9S2CB,
                VocalizerShevaG9S3CB,
                VocalizerShevaG9S4CB,
                VocalizerShevaG9S5CB
            };

            List<ComboBoxEdit[]> ShevaGroups = new List<ComboBoxEdit[]>
            {
                ShevaGroup1Combos, ShevaGroup2Combos, ShevaGroup3Combos, ShevaGroup4Combos, ShevaGroup5Combos, ShevaGroup6Combos, ShevaGroup7Combos, ShevaGroup8Combos, ShevaGroup9Combos,
            };

            #endregion

            #region Jill

            ComboBoxEdit[] JillGroup1Combos =
            {
                VocalizerJillG1S1CB,
                VocalizerJillG1S2CB,
                VocalizerJillG1S3CB,
                VocalizerJillG1S4CB,
                VocalizerJillG1S5CB
            };

            ComboBoxEdit[] JillGroup2Combos =
            {
                VocalizerJillG2S1CB,
                VocalizerJillG2S2CB,
                VocalizerJillG2S3CB,
                VocalizerJillG2S4CB,
                VocalizerJillG2S5CB
            };

            ComboBoxEdit[] JillGroup3Combos =
            {
                VocalizerJillG3S1CB,
                VocalizerJillG3S2CB,
                VocalizerJillG3S3CB,
                VocalizerJillG3S4CB,
                VocalizerJillG3S5CB
            };

            ComboBoxEdit[] JillGroup4Combos =
            {
                VocalizerJillG4S1CB,
                VocalizerJillG4S2CB,
                VocalizerJillG4S3CB,
                VocalizerJillG4S4CB,
                VocalizerJillG4S5CB
            };

            ComboBoxEdit[] JillGroup5Combos =
            {
                VocalizerJillG5S1CB,
                VocalizerJillG5S2CB,
                VocalizerJillG5S3CB,
                VocalizerJillG5S4CB,
                VocalizerJillG5S5CB
            };

            ComboBoxEdit[] JillGroup6Combos =
            {
                VocalizerJillG6S1CB,
                VocalizerJillG6S2CB,
                VocalizerJillG6S3CB,
                VocalizerJillG6S4CB,
                VocalizerJillG6S5CB
            };

            ComboBoxEdit[] JillGroup7Combos =
            {
                VocalizerJillG7S1CB,
                VocalizerJillG7S2CB,
                VocalizerJillG7S3CB,
                VocalizerJillG7S4CB,
                VocalizerJillG7S5CB
            };

            ComboBoxEdit[] JillGroup8Combos =
            {
                VocalizerJillG8S1CB,
                VocalizerJillG8S2CB,
                VocalizerJillG8S3CB,
                VocalizerJillG8S4CB,
                VocalizerJillG8S5CB
            };

            ComboBoxEdit[] JillGroup9Combos =
            {
                VocalizerJillG9S1CB,
                VocalizerJillG9S2CB,
                VocalizerJillG9S3CB,
                VocalizerJillG9S4CB,
                VocalizerJillG9S5CB
            };

            List<ComboBoxEdit[]> JillGroups = new List<ComboBoxEdit[]>
            {
                JillGroup1Combos, JillGroup2Combos, JillGroup3Combos, JillGroup4Combos, JillGroup5Combos, JillGroup6Combos, JillGroup7Combos, JillGroup8Combos, JillGroup9Combos,
            };

            #endregion

            #region Wesker

            ComboBoxEdit[] WeskerGroup1Combos =
            {
                VocalizerWeskerG1S1CB,
                VocalizerWeskerG1S2CB,
                VocalizerWeskerG1S3CB,
                VocalizerWeskerG1S4CB,
                VocalizerWeskerG1S5CB
            };

            ComboBoxEdit[] WeskerGroup2Combos =
            {
                VocalizerWeskerG2S1CB,
                VocalizerWeskerG2S2CB,
                VocalizerWeskerG2S3CB,
                VocalizerWeskerG2S4CB,
                VocalizerWeskerG2S5CB
            };

            ComboBoxEdit[] WeskerGroup3Combos =
            {
                VocalizerWeskerG3S1CB,
                VocalizerWeskerG3S2CB,
                VocalizerWeskerG3S3CB,
                VocalizerWeskerG3S4CB,
                VocalizerWeskerG3S5CB
            };

            ComboBoxEdit[] WeskerGroup4Combos =
            {
                VocalizerWeskerG4S1CB,
                VocalizerWeskerG4S2CB,
                VocalizerWeskerG4S3CB,
                VocalizerWeskerG4S4CB,
                VocalizerWeskerG4S5CB
            };

            ComboBoxEdit[] WeskerGroup5Combos =
            {
                VocalizerWeskerG5S1CB,
                VocalizerWeskerG5S2CB,
                VocalizerWeskerG5S3CB,
                VocalizerWeskerG5S4CB,
                VocalizerWeskerG5S5CB
            };

            ComboBoxEdit[] WeskerGroup6Combos =
            {
                VocalizerWeskerG6S1CB,
                VocalizerWeskerG6S2CB,
                VocalizerWeskerG6S3CB,
                VocalizerWeskerG6S4CB,
                VocalizerWeskerG6S5CB
            };

            ComboBoxEdit[] WeskerGroup7Combos =
            {
                VocalizerWeskerG7S1CB,
                VocalizerWeskerG7S2CB,
                VocalizerWeskerG7S3CB,
                VocalizerWeskerG7S4CB,
                VocalizerWeskerG7S5CB
            };

            ComboBoxEdit[] WeskerGroup8Combos =
            {
                VocalizerWeskerG8S1CB,
                VocalizerWeskerG8S2CB,
                VocalizerWeskerG8S3CB,
                VocalizerWeskerG8S4CB,
                VocalizerWeskerG8S5CB
            };

            ComboBoxEdit[] WeskerGroup9Combos =
            {
                VocalizerWeskerG9S1CB,
                VocalizerWeskerG9S2CB,
                VocalizerWeskerG9S3CB,
                VocalizerWeskerG9S4CB,
                VocalizerWeskerG9S5CB
            };

            List<ComboBoxEdit[]> WeskerGroups = new List<ComboBoxEdit[]>
            {
                WeskerGroup1Combos, WeskerGroup2Combos, WeskerGroup3Combos, WeskerGroup4Combos, WeskerGroup5Combos, WeskerGroup6Combos, WeskerGroup7Combos, WeskerGroup8Combos, WeskerGroup9Combos,
            };

            #endregion

            #region Josh

            ComboBoxEdit[] JoshGroup1Combos =
            {
                VocalizerJoshG1S1CB,
                VocalizerJoshG1S2CB,
                VocalizerJoshG1S3CB,
                VocalizerJoshG1S4CB,
                VocalizerJoshG1S5CB
            };

            ComboBoxEdit[] JoshGroup2Combos =
            {
                VocalizerJoshG2S1CB,
                VocalizerJoshG2S2CB,
                VocalizerJoshG2S3CB,
                VocalizerJoshG2S4CB,
                VocalizerJoshG2S5CB
            };

            ComboBoxEdit[] JoshGroup3Combos =
            {
                VocalizerJoshG3S1CB,
                VocalizerJoshG3S2CB,
                VocalizerJoshG3S3CB,
                VocalizerJoshG3S4CB,
                VocalizerJoshG3S5CB
            };

            ComboBoxEdit[] JoshGroup4Combos =
            {
                VocalizerJoshG4S1CB,
                VocalizerJoshG4S2CB,
                VocalizerJoshG4S3CB,
                VocalizerJoshG4S4CB,
                VocalizerJoshG4S5CB
            };

            ComboBoxEdit[] JoshGroup5Combos =
            {
                VocalizerJoshG5S1CB,
                VocalizerJoshG5S2CB,
                VocalizerJoshG5S3CB,
                VocalizerJoshG5S4CB,
                VocalizerJoshG5S5CB
            };

            ComboBoxEdit[] JoshGroup6Combos =
            {
                VocalizerJoshG6S1CB,
                VocalizerJoshG6S2CB,
                VocalizerJoshG6S3CB,
                VocalizerJoshG6S4CB,
                VocalizerJoshG6S5CB
            };

            ComboBoxEdit[] JoshGroup7Combos =
            {
                VocalizerJoshG7S1CB,
                VocalizerJoshG7S2CB,
                VocalizerJoshG7S3CB,
                VocalizerJoshG7S4CB,
                VocalizerJoshG7S5CB
            };

            ComboBoxEdit[] JoshGroup8Combos =
            {
                VocalizerJoshG8S1CB,
                VocalizerJoshG8S2CB,
                VocalizerJoshG8S3CB,
                VocalizerJoshG8S4CB,
                VocalizerJoshG8S5CB
            };

            ComboBoxEdit[] JoshGroup9Combos =
            {
                VocalizerJoshG9S1CB,
                VocalizerJoshG9S2CB,
                VocalizerJoshG9S3CB,
                VocalizerJoshG9S4CB,
                VocalizerJoshG9S5CB
            };

            List<ComboBoxEdit[]> JoshGroups = new List<ComboBoxEdit[]>
            {
                JoshGroup1Combos, JoshGroup2Combos, JoshGroup3Combos, JoshGroup4Combos, JoshGroup5Combos, JoshGroup6Combos, JoshGroup7Combos, JoshGroup8Combos, JoshGroup9Combos,
            };

            #endregion

            #region Excella

            ComboBoxEdit[] ExcellaGroup1Combos =
            {
                VocalizerExcellaG1S1CB,
                VocalizerExcellaG1S2CB,
                VocalizerExcellaG1S3CB,
                VocalizerExcellaG1S4CB,
                VocalizerExcellaG1S5CB
            };

            ComboBoxEdit[] ExcellaGroup2Combos =
            {
                VocalizerExcellaG2S1CB,
                VocalizerExcellaG2S2CB,
                VocalizerExcellaG2S3CB,
                VocalizerExcellaG2S4CB,
                VocalizerExcellaG2S5CB
            };

            ComboBoxEdit[] ExcellaGroup3Combos =
            {
                VocalizerExcellaG3S1CB,
                VocalizerExcellaG3S2CB,
                VocalizerExcellaG3S3CB,
                VocalizerExcellaG3S4CB,
                VocalizerExcellaG3S5CB
            };

            ComboBoxEdit[] ExcellaGroup4Combos =
            {
                VocalizerExcellaG4S1CB,
                VocalizerExcellaG4S2CB,
                VocalizerExcellaG4S3CB,
                VocalizerExcellaG4S4CB,
                VocalizerExcellaG4S5CB
            };

            ComboBoxEdit[] ExcellaGroup5Combos =
            {
                VocalizerExcellaG5S1CB,
                VocalizerExcellaG5S2CB,
                VocalizerExcellaG5S3CB,
                VocalizerExcellaG5S4CB,
                VocalizerExcellaG5S5CB
            };

            ComboBoxEdit[] ExcellaGroup6Combos =
            {
                VocalizerExcellaG6S1CB,
                VocalizerExcellaG6S2CB,
                VocalizerExcellaG6S3CB,
                VocalizerExcellaG6S4CB,
                VocalizerExcellaG6S5CB
            };

            ComboBoxEdit[] ExcellaGroup7Combos =
            {
                VocalizerExcellaG7S1CB,
                VocalizerExcellaG7S2CB,
                VocalizerExcellaG7S3CB,
                VocalizerExcellaG7S4CB,
                VocalizerExcellaG7S5CB
            };

            ComboBoxEdit[] ExcellaGroup8Combos =
            {
                VocalizerExcellaG8S1CB,
                VocalizerExcellaG8S2CB,
                VocalizerExcellaG8S3CB,
                VocalizerExcellaG8S4CB,
                VocalizerExcellaG8S5CB
            };

            ComboBoxEdit[] ExcellaGroup9Combos =
            {
                VocalizerExcellaG9S1CB,
                VocalizerExcellaG9S2CB,
                VocalizerExcellaG9S3CB,
                VocalizerExcellaG9S4CB,
                VocalizerExcellaG9S5CB
            };

            List<ComboBoxEdit[]> ExcellaGroups = new List<ComboBoxEdit[]>
            {
                ExcellaGroup1Combos, ExcellaGroup2Combos, ExcellaGroup3Combos, ExcellaGroup4Combos, ExcellaGroup5Combos, ExcellaGroup6Combos, ExcellaGroup7Combos, ExcellaGroup8Combos, ExcellaGroup9Combos,
            };

            #endregion

            #region Barry

            ComboBoxEdit[] BarryGroup1Combos =
            {
                 VocalizerBarryG1S1CB,
                 VocalizerBarryG1S2CB,
                 VocalizerBarryG1S3CB,
                 VocalizerBarryG1S4CB,
                 VocalizerBarryG1S5CB
             };

            ComboBoxEdit[] BarryGroup2Combos =
            {
                 VocalizerBarryG2S1CB,
                 VocalizerBarryG2S2CB,
                 VocalizerBarryG2S3CB,
                 VocalizerBarryG2S4CB,
                 VocalizerBarryG2S5CB
             };

            ComboBoxEdit[] BarryGroup3Combos =
            {
                 VocalizerBarryG3S1CB,
                 VocalizerBarryG3S2CB,
                 VocalizerBarryG3S3CB,
                 VocalizerBarryG3S4CB,
                 VocalizerBarryG3S5CB
             };

            ComboBoxEdit[] BarryGroup4Combos =
            {
                 VocalizerBarryG4S1CB,
                 VocalizerBarryG4S2CB,
                 VocalizerBarryG4S3CB,
                 VocalizerBarryG4S4CB,
                 VocalizerBarryG4S5CB
             };

            ComboBoxEdit[] BarryGroup5Combos =
            {
                 VocalizerBarryG5S1CB,
                 VocalizerBarryG5S2CB,
                 VocalizerBarryG5S3CB,
                 VocalizerBarryG5S4CB,
                 VocalizerBarryG5S5CB
             };

            ComboBoxEdit[] BarryGroup6Combos =
            {
                 VocalizerBarryG6S1CB,
                 VocalizerBarryG6S2CB,
                 VocalizerBarryG6S3CB,
                 VocalizerBarryG6S4CB,
                 VocalizerBarryG6S5CB
             };

            ComboBoxEdit[] BarryGroup7Combos =
            {
                 VocalizerBarryG7S1CB,
                 VocalizerBarryG7S2CB,
                 VocalizerBarryG7S3CB,
                 VocalizerBarryG7S4CB,
                 VocalizerBarryG7S5CB
             };

            ComboBoxEdit[] BarryGroup8Combos =
            {
                 VocalizerBarryG8S1CB,
                 VocalizerBarryG8S2CB,
                 VocalizerBarryG8S3CB,
                 VocalizerBarryG8S4CB,
                 VocalizerBarryG8S5CB
             };

            ComboBoxEdit[] BarryGroup9Combos =
            {
                 VocalizerBarryG9S1CB,
                 VocalizerBarryG9S2CB,
                 VocalizerBarryG9S3CB,
                 VocalizerBarryG9S4CB,
                 VocalizerBarryG9S5CB
             };

            List<ComboBoxEdit[]> BarryGroups = new List<ComboBoxEdit[]>
             {
                 BarryGroup1Combos, BarryGroup2Combos, BarryGroup3Combos, BarryGroup4Combos, BarryGroup5Combos, BarryGroup6Combos, BarryGroup7Combos, BarryGroup8Combos, BarryGroup9Combos,
             };

            #endregion

            #region Rebecca

            ComboBoxEdit[] RebeccaGroup1Combos =
            {
                 VocalizerRebeccaG1S1CB,
                 VocalizerRebeccaG1S2CB,
                 VocalizerRebeccaG1S3CB,
                 VocalizerRebeccaG1S4CB,
                 VocalizerRebeccaG1S5CB
             };

            ComboBoxEdit[] RebeccaGroup2Combos =
            {
                 VocalizerRebeccaG2S1CB,
                 VocalizerRebeccaG2S2CB,
                 VocalizerRebeccaG2S3CB,
                 VocalizerRebeccaG2S4CB,
                 VocalizerRebeccaG2S5CB
             };

            ComboBoxEdit[] RebeccaGroup3Combos =
            {
                 VocalizerRebeccaG3S1CB,
                 VocalizerRebeccaG3S2CB,
                 VocalizerRebeccaG3S3CB,
                 VocalizerRebeccaG3S4CB,
                 VocalizerRebeccaG3S5CB
             };

            ComboBoxEdit[] RebeccaGroup4Combos =
            {
                 VocalizerRebeccaG4S1CB,
                 VocalizerRebeccaG4S2CB,
                 VocalizerRebeccaG4S3CB,
                 VocalizerRebeccaG4S4CB,
                 VocalizerRebeccaG4S5CB
             };

            ComboBoxEdit[] RebeccaGroup5Combos =
            {
                 VocalizerRebeccaG5S1CB,
                 VocalizerRebeccaG5S2CB,
                 VocalizerRebeccaG5S3CB,
                 VocalizerRebeccaG5S4CB,
                 VocalizerRebeccaG5S5CB
             };

            ComboBoxEdit[] RebeccaGroup6Combos =
            {
                 VocalizerRebeccaG6S1CB,
                 VocalizerRebeccaG6S2CB,
                 VocalizerRebeccaG6S3CB,
                 VocalizerRebeccaG6S4CB,
                 VocalizerRebeccaG6S5CB
             };

            ComboBoxEdit[] RebeccaGroup7Combos =
            {
                 VocalizerRebeccaG7S1CB,
                 VocalizerRebeccaG7S2CB,
                 VocalizerRebeccaG7S3CB,
                 VocalizerRebeccaG7S4CB,
                 VocalizerRebeccaG7S5CB
             };

            ComboBoxEdit[] RebeccaGroup8Combos =
            {
                 VocalizerRebeccaG8S1CB,
                 VocalizerRebeccaG8S2CB,
                 VocalizerRebeccaG8S3CB,
                 VocalizerRebeccaG8S4CB,
                 VocalizerRebeccaG8S5CB
             };

            ComboBoxEdit[] RebeccaGroup9Combos =
            {
                 VocalizerRebeccaG9S1CB,
                 VocalizerRebeccaG9S2CB,
                 VocalizerRebeccaG9S3CB,
                 VocalizerRebeccaG9S4CB,
                 VocalizerRebeccaG9S5CB
             };

            List<ComboBoxEdit[]> RebeccaGroups = new List<ComboBoxEdit[]>
             {
                 RebeccaGroup1Combos, RebeccaGroup2Combos, RebeccaGroup3Combos, RebeccaGroup4Combos, RebeccaGroup5Combos, RebeccaGroup6Combos, RebeccaGroup7Combos, RebeccaGroup8Combos, RebeccaGroup9Combos,
             };

            #endregion

            #endregion

            List<List<ComboBoxEdit[]>> Characters = new List<List<ComboBoxEdit[]>>
            {
                ChrisGroups, ShevaGroups, JillGroups, WeskerGroups, JoshGroups, ExcellaGroups, BarryGroups, RebeccaGroups
            };

            if (SetLine)
            {
                var Lines = db.AllSpeech.First(x => (int)x.Character == Character).Lines;
                Characters[Character][Group][Slot].SelectedIndex = Lines.IndexOf(Lines.First(x => x.Value == Line));
            }

            return (Characters[Character][Group][Slot].SelectedItem as Simple).Value;
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
                MasterTabControl.Height = 439;
                TabInventoryGP.Width = 1206;
                TabInventoryGP.Height = 418;
            }
            else if (XTP.Name == "TabPageVocalizer")
            {
                Width = 768;
                Height = 641;
                MasterTabControl.Width = 744;
                MasterTabControl.Height = 557;
            }
            else
            {
                Width = 664;
                Height = 523;
                MasterTabControl.Width = 640;
                MasterTabControl.Height = 439;
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
            List<int> VocalizerHotkeys = new List<int>();
            List<List<List<int>>> VocalizerLines = new List<List<List<int>>>();

            for (int Group = 0; Group < 9; Group++)
            {
                VocalizerHotkeys.Add(ChooseVocalizerHotkey(Group));
            }

            for (int Char = 0; Char < 8; Char++)
            {
                if (VocalizerLines[Char] == null)
                    VocalizerLines[Char] = new List<List<int>>();

                for (int Group = 0; Group < 9; Group++)
                {
                    if (VocalizerLines[Char][Group] == null)
                        VocalizerLines[Char][Group] = new List<int>();

                    for (int Slot = 0; Slot < 5; Slot++)
                        VocalizerLines[Char][Group][Slot] = ChooseVocalizerLine(Char, Group, Slot);
                }
            }

            Settings Setts = new Settings()
            {
                UpdateRate = UpdateModeComboBoxEdit.SelectedIndex,
                SkinName = UserLookAndFeel.Default.ActiveSvgPaletteName,
                DisableMeleeCamera = MeleeCameraCE.Checked,
                ReunionSpecialMoves = ReunionSpecialMovesCE.Checked,
                WeaponPlacement = WeaponPlacementComboBox.SelectedIndex,
                WeskerNoSunglassDrop = WeskerGlassesCE.Checked,
                WeskerNoDashHPCost = WeskerNoDashCostCE.Checked,
                WeskerInfiniteDash = WeskerInfiniteDashCE.Checked,
                WeskerNoWeaponOnChest = WeskerNoWeaponOnChestCE.Checked,
                ControllerAim = ControllerAimButton.Text == "Disable",
                FilterRemover = ColorFilterButton.Text == "Disable",
                StunRodMeleeKill = StunRodMeleeKillCE.Checked,
                NoHandTremors = HandTremorCE.Checked,
                ResetScore = ResetScoreCE.Checked,
                MaxComboTimer = ComboTimerMaxCE.Checked,
                MaxComboBonusTimer = ComboBonusTimerMaxCE.Checked,
                NoTimerDecrease = NoTimerDecreaseCE.Checked,
                MeleeKillSeconds = MeleeKillCB.SelectedIndex,
                ComboTimerDuration = ComboTimerCB.SelectedIndex,
                ComboBonusTimerDuration = ComboBonusTimerCB.SelectedIndex,
                VocalizerHotkeys = VocalizerHotkeys,
                VocalizerLines = VocalizerLines
            };

            try
            {
                Serializer.WriteDataFile(@"addons/GameX.Biohazard.5/appsettings.json", Serializer.Serialize(Setts));
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex);
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
            }

            UpdateModeComboBoxEdit.SelectedIndex = Utility.Clamp(Setts.UpdateRate, 0, 2);

            foreach (Simple Pallete in PaletteComboBoxEdit.Properties.Items)
            {
                if (Pallete.Text == Setts.SkinName)
                    PaletteComboBoxEdit.SelectedItem = Pallete;
            }

            MeleeCameraCE.Checked = Setts.DisableMeleeCamera;
            ReunionSpecialMovesCE.Checked = Setts.ReunionSpecialMoves;
            WeskerGlassesCE.Checked = Setts.WeskerNoSunglassDrop;
            WeskerNoDashCostCE.Checked = Setts.WeskerNoDashHPCost;
            WeskerInfiniteDashCE.Checked = Setts.WeskerInfiniteDash;
            WeskerNoWeaponOnChestCE.Checked = Setts.WeskerNoWeaponOnChest;
            StunRodMeleeKillCE.Checked = Setts.StunRodMeleeKill;
            HandTremorCE.Checked = Setts.NoHandTremors;
            ResetScoreCE.Checked = Setts.ResetScore;
            ComboTimerMaxCE.Checked = Setts.MaxComboTimer;
            ComboBonusTimerMaxCE.Checked = Setts.MaxComboBonusTimer;
            NoTimerDecreaseCE.Checked = Setts.NoTimerDecrease;

            WeaponPlacementComboBox.SelectedIndex = Setts.WeaponPlacement;
            WeaponPlacement_IndexChanged(WeaponPlacementComboBox, null);

            MeleeKillCB.SelectedIndex = Setts.MeleeKillSeconds;
            TimerCombos_SelectedIndexChanged(MeleeKillCB, null);
            ComboTimerCB.SelectedIndex = Setts.ComboTimerDuration;
            TimerCombos_SelectedIndexChanged(ComboTimerCB, null);
            ComboBonusTimerCB.SelectedIndex = Setts.ComboBonusTimerDuration;
            TimerCombos_SelectedIndexChanged(ComboBonusTimerCB, null);

            if (Setts.VocalizerHotkeys != null)
                for (int Group = 0; Group < 9; Group++)
                    ChooseVocalizerHotkey(Group, true, Setts.VocalizerHotkeys[Group]);

            if (Setts.VocalizerLines != null)
            {
                for (int Char = 0; Char < 8; Char++)
                {
                    for (int Group = 0; Group < 9; Group++)
                    {
                        for (int Slot = 0; Slot < 5; Slot++)
                            ChooseVocalizerLine(Char, Group, Slot, true, Setts.VocalizerLines[Char][Group][Slot]);
                    }
                }
            }

            if ((Setts.ControllerAim && ControllerAimButton.Text == "Enable") || (!Setts.ControllerAim && ControllerAimButton.Text == "Disable"))
                EnableDisable_StateChanged(ControllerAimButton, null);

            if ((Setts.FilterRemover && ColorFilterButton.Text == "Enable") || (!Setts.FilterRemover && ColorFilterButton.Text == "Disable"))
                EnableDisable_StateChanged(ColorFilterButton, null);

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

                if (CE.Name.Equals("WeskerNoWeaponOnChestCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.WeskerNoWeaponOnChest(CE.Checked);
                }
                else if (CE.Name.Equals("WeskerGlassesCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.WeskerNoSunglassDrop(CE.Checked);
                }
                else if (CE.Name.Equals("WeskerNoDashCostCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.WeskerNoDashCost(CE.Checked);
                }
                else if (CE.Name.Equals("MeleeCameraCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.DisableMeleeCamera(CE.Checked);
                }
                else if (CE.Name.Equals("ReunionSpecialMovesCE"))
                {
                    if (!Initialized)
                        return;

                    bool Enabled = Biohazard.EnableReunionSpecialMoves(CE.Checked);

                    CE.CheckedChanged -= EnableDisable_StateChanged;

                    if (Enabled)
                        CE.Checked = CE.Checked;
                    else
                        CE.Checked = false;

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
                else if (CE.Name.Equals("StunRodMeleeKillCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.EnableStunRodMeleeKill(CE.Checked);
                }
                else if (CE.Name.Equals("HandTremorCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.DisableHandTremor(CE.Checked);
                }
                else if (CE.Name.Equals("NoFPSCapCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.FPSCap = CE.Checked ? 360.0f : 120.0f;
                }
                else if (CE.Name.Equals("NoTimerDecreaseCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.NoTimerDecrease(CE.Checked);
                }
                else if (CE.Name.Equals("ResetScoreCE"))
                {
                    if (!Initialized)
                        return;

                    Biohazard.ZeroScoreCalculation(CE.Checked);
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

        private void TimerCombos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            Simple CBEItem = CBE.SelectedItem as Simple;

            switch (CBE.Name)
            {
                case "MeleeKillCB":
                    Biohazard.MeleeKillSeconds = (byte)CBEItem.Value;
                    break;
                case "ComboTimerCB":
                    Biohazard.ComboTimerDuration = (byte)CBEItem.Value;
                    break;
                case "ComboBonusTimerCB":
                    Biohazard.ComboBonusTimerDuration = (byte)CBEItem.Value;
                    break;
            }
        }

        private void MapsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            Map SelectedMap = CBE.SelectedItem as Map;

            Image Final = ImageHelper.GetImageFromFile(@"addons/GameX.Biohazard.5/images/map/" + SelectedMap.Alias + ".png");
            MapsPE.Image = Final;

            if (!Initialized)
                return;

            Biohazard.Stage = SelectedMap.Stage;
            Biohazard.Chapter = SelectedMap.Chapter;
        }

        private void HotkeysCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PictureEdit[] HotkeyPicBoxes =
            {
                VocalizerHotkeyG1PE,
                VocalizerHotkeyG2PE,
                VocalizerHotkeyG3PE,
                VocalizerHotkeyG4PE,
                VocalizerHotkeyG5PE,
                VocalizerHotkeyG6PE,
                VocalizerHotkeyG7PE,
                VocalizerHotkeyG8PE,
                VocalizerHotkeyG9PE
            };

            ComboBoxEdit CBE = sender as ComboBoxEdit;
            Hotkey SelectedHotkey = CBE.SelectedItem as Hotkey;
            int Index = int.Parse(CBE.Name[16].ToString()) - 1;

            string HotkeyImageDir = Directory.GetCurrentDirectory() + "/addons/GameX.Biohazard.5/images/hotkeys";
            HotkeyPicBoxes[Index].Image = Image.FromFile($"{HotkeyImageDir}/{SelectedHotkey.Image}.png");
        }

        private void VocalizerCharCombo_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            VocalizerTabControl.SelectedTabPageIndex = CBE.SelectedIndex;
        }

        #endregion

        #region GameX Calls

        private void GameX_StartControls()
        {
            ControllerAimButton.Enabled = !Memory.QOLDllInjected;
            ControllerAimButton.Text = ControllerAimButton.Enabled ? ControllerAimButton.Text : "Enable";

            VocalizerEnableCE.Enabled = Memory.InternalInjected;
            VocalizerEnableCE.Checked = Memory.InternalInjected && VocalizerEnableCE.Checked;

            #region Controls

            SimpleButton[] EnableDisable =
            {
                ColorFilterButton,
                ControllerAimButton
            };

            CheckEdit[] CheckUncheck =
            {
                WeskerNoWeaponOnChestCE,
                WeskerGlassesCE,
                MeleeCameraCE,
                ReunionSpecialMovesCE,
                StunRodMeleeKillCE,
                HandTremorCE,
                NoTimerDecreaseCE,
                ResetScoreCE
            };

            ComboBoxEdit[] Combos =
            {
                MeleeKillCB,
                ComboTimerCB,
                ComboBonusTimerCB
            };

            #endregion

            foreach (CheckEdit CE in CheckUncheck)
            {
                switch (CE.Name)
                {
                    case "WeskerNoWeaponOnCE":
                        if (CE.Checked)
                            Biohazard.WeskerNoWeaponOnChest(true);
                        break;
                    case "WeskerGlassesCE":
                        if (CE.Checked)
                            Biohazard.WeskerNoSunglassDrop(true);
                        break;
                    case "MeleeCameraCE":
                        if (CE.Checked)
                            Biohazard.DisableMeleeCamera(true);
                        break;
                    case "ReunionSpecialMovesCE":
                        if (CE.Checked)
                        {
                            CE.CheckedChanged -= EnableDisable_StateChanged;
                            CE.Checked = Biohazard.EnableReunionSpecialMoves(true);
                            CE.CheckedChanged += EnableDisable_StateChanged;
                        }
                        break;
                    case "StunRodMeleeKillCE":
                        if (CE.Checked)
                            Biohazard.EnableStunRodMeleeKill(true);
                        break;
                    case "HandTremorCE":
                        if (CE.Checked)
                            Biohazard.DisableHandTremor(true);
                        break;
                    case "NoFPSCapCE":
                        if (CE.Checked)
                            Biohazard.FPSCap = 360.0f;
                        break;
                    case "NoTimerDecreaseCE":
                        if (CE.Checked)
                            Biohazard.NoTimerDecrease(true);
                        break;
                    case "ResetScoreCE":
                        if (CE.Checked)
                            Biohazard.ZeroScoreCalculation(true);
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

            foreach (ComboBoxEdit CBE in Combos)
            {
                TimerCombos_SelectedIndexChanged(CBE, null);
            }

            Melee_ApplyComboBox(null, true);
            WeaponPlacement_IndexChanged(WeaponPlacementComboBox, null);
        }

        private void GameX_EndControls()
        {

        }

        private void GameX_Start()
        {
            try
            {
                Biohazard.NoFileChecking(true);
                Biohazard.OnlineCharSwapFixes(true);

                GameX_StartControls();
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
                if (!Initialized)
                    return;

                if (MeleeAnytimeSwitch.IsOn)
                    MeleeAnytimeSwitch.Toggle();

                Biohazard.VocalizerEnabledFlag = false;
                Biohazard.ZeroScoreCalculation(false);
                Biohazard.EnableColorFilter(false);
                Biohazard.NoFileChecking(false);
                Biohazard.OnlineCharSwapFixes(false);           
                Biohazard.WeskerNoWeaponOnChest(false);
                Biohazard.WeskerNoSunglassDrop(false);
                Biohazard.WeskerNoDashCost(false);
                Biohazard.NoTimerDecrease(false);

                if (ControllerAimButton.Enabled)
                    Biohazard.EnableControllerAim(false);

                Biohazard.SetWeaponPlacement(0);
                Biohazard.DisableMeleeCamera(false);
                Biohazard.DisableHandTremor(false);
                Biohazard.EnableStunRodMeleeKill(false);
                Biohazard.EnableReunionSpecialMoves(false);

                Biohazard.FPSCap = 120.0f;

                GameX_EndControls();
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
                if (!Initialized)
                    return;

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

                ComboBoxEdit[] VocalizerHotkeyCombos =
                {
                    VocalizerHotkeyG1CB,
                    VocalizerHotkeyG2CB,
                    VocalizerHotkeyG3CB,
                    VocalizerHotkeyG4CB,
                    VocalizerHotkeyG5CB,
                    VocalizerHotkeyG6CB,
                    VocalizerHotkeyG7CB,
                    VocalizerHotkeyG8CB,
                    VocalizerHotkeyG9CB,
                };

                #endregion

                #region Internal Update

                if (Memory.InternalInjected)
                {
                    Biohazard.VocalizerEnabledFlag = VocalizerEnableCE.Checked;

                    for (int i = 0; i < VocalizerHotkeyCombos.Length; i++)
                        Biohazard.SetVocalizerHotkey(i, (byte)(VocalizerHotkeyCombos[i].SelectedItem as Hotkey).Key);

                    Player LocalPlayer = Biohazard.Players[Biohazard.LocalPlayer];

                    if (LocalPlayer.IsActive())
                    {
                        int CurrentChar = LocalPlayer.Character;
                        if (CurrentChar > -1 && CurrentChar < 8)
                        {
                            for (int group = 0; group < 9; group++)
                            {
                                for (int slot = 0; slot < 5; slot++)
                                {
                                    int SelectedLine = ChooseVocalizerLine(CurrentChar, group, slot);
                                    Biohazard.SetVocalizerSpeechGroup(group, slot, (ushort)SelectedLine);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Game Mode Update

                double combotimer = Utility.Clamp(Biohazard.ComboTimer, 0, TimeSpan.MaxValue.TotalSeconds - 1d);
                double combobonustimer = Utility.Clamp(Biohazard.ComboBonusTimer, 0, TimeSpan.MaxValue.TotalSeconds - 1d);
                double timer = Utility.Clamp(Biohazard.Timer, 0, TimeSpan.MaxValue.TotalSeconds - 1d);

                ScoreTE.Text = Biohazard.Score.ToString();
                ComboTimerTE.Text = TimeSpan.FromSeconds(double.IsNaN(combotimer) ? 0f : combotimer).ToString("mm':'ss");
                ComboBonusTimerTE.Text = TimeSpan.FromSeconds(double.IsNaN(combobonustimer) ? 0f : combobonustimer).ToString("mm':'ss");
                CurrentTimerTE.Text = TimeSpan.FromSeconds(double.IsNaN(timer) ? 0f : timer).ToString("hh':'mm':'ss");

                if (Biohazard.GameMode == (int)GameModeEnum.Mercenaries || Biohazard.GameMode == (int)GameModeEnum.Reunion)
                {
                    if (ComboTimerMaxCE.Checked)
                        Biohazard.ComboTimer = Biohazard.ComboTimerDuration;

                    if (ComboBonusTimerMaxCE.Checked)
                        Biohazard.ComboBonusTimer = Biohazard.ComboBonusTimerDuration;
                }

                if (!MapsCB.IsPopupOpen)
                {
                    Map SelectedMap = MapsCB.SelectedItem as Map;

                    int Chapter = Biohazard.Chapter;
                    short Stage = Biohazard.Stage;

                    if (!FreezeMapCE.Checked)
                    {
                        if ((SelectedMap.Stage != -1 && SelectedMap.Stage != Stage) || SelectedMap.Chapter != Chapter)
                        {
                            foreach (object ComboObj in MapsCB.Properties.Items)
                            {
                                Map map = ComboObj as Map;

                                if ((map.Stage == -1 || map.Stage == Biohazard.Stage) && map.Chapter == Biohazard.Chapter)
                                    MapsCB.SelectedItem = ComboObj;
                            }
                        }
                    }
                    else
                    {
                        Biohazard.Stage = SelectedMap.Stage;
                        Biohazard.Chapter = SelectedMap.Chapter;
                    }
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
                    Biohazard.WeskerNoDashCost(WeskerNoDashCostCE.Checked); // && Biohazard.GameMode != (int)GameModeEnum.Versus

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
                    if (WeskerInfiniteDashCE.Checked && PlayerPresent && Biohazard.Players[i].Character == (int)CharacterEnum.Wesker)
                        Biohazard.Players[i].ResetDash();
                }

                #endregion
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex);
            }
        }

        private void GameX_Keyboard(int input)
        {
#if DEBUG
            if (ActiveForm != null)
                Terminal.WriteLine($"User pressed input: {input:X}");
#endif

            if (!Initialized)
                return;

        }

        #endregion
    }
}