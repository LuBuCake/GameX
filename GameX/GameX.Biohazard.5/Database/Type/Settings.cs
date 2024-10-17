using System.Collections.Generic;

namespace GameX.Database.Type
{
    public class Settings
    {
        public int UpdateRate { get; set; }
        public string SkinName { get; set; }
        public bool DisableMeleeCamera { get; set; }
        public bool ReunionSpecialMoves { get; set; }
        public int WeaponPlacement { get; set; }
        public bool WeskerNoSunglassDrop { get; set; }
        public bool WeskerNoDashHPCost { get; set; }
        public bool WeskerInfiniteDash { get; set; }
        public bool WeskerNoWeaponOnChest { get; set; }
        public bool ControllerAim { get; set; }
        public bool FilterRemover { get; set; }
        public bool StunRodMeleeKill { get; set; }
        public bool NoHandTremors { get; set; }
        public bool ResetScore { get; set; }
        public bool MaxComboTimer { get; set; }
        public bool MaxComboBonusTimer { get; set; }
        public bool NoTimerDecrease { get; set; }
        public int MeleeKillSeconds { get; set; }
        public int ComboTimerDuration { get; set; }
        public int ComboBonusTimerDuration { get; set; }
        public List<int> VocalizerHotkeys { get; set; }
        public List<List<List<int>>> VocalizerSpeechGroups { get; set; }
    }
}