using System.ComponentModel;

namespace GameX.Game.Helpers
{
    public enum ItemGroup
    {
        Default,
        Handgun,
        Shotgun,
        MachineGun,
        Rifle,
        Magnum,
        Launcher,
        Melee,
        Explosive,
        Ammunition,
        Heal,
        Utility,
        Special
    }

    public enum GameMode
    {
        Campaign,
        Versus,
        Mercenaries,
        LIN,
        DE,
        Reunion,
    }

    public enum MoveType
    {
        Movement,
        Damage,
        Action,
        Dash
    }

    public enum Handness
    {
        Right,
        Left 
    }

    public enum WeaponMode
    {
        Male,
        Female
    }

    public enum Characters
    {
        Chris,
        Sheva,
        Jill,
        Wesker,
        Josh,
        Excella,
        Barry,
        Rebecca,
        Irving = 134
    }

    public enum GrabMoves
    {
        FinisherFront = 53,
        LegBack = 57,
        ReunionHeadFlash = 173,
        ReunionLegFront = 172
    }

    public enum IdleMoves
    {
        StandStill,
        Idle,
        MoveFront,
        MoveLeft,
        MoveRight,
        Running,
        MoveBack,
        QuickTurn = 9
    }
}