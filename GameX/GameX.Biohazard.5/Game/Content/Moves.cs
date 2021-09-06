using System.Collections.Generic;
using GameX.Game.Types;
using GameX.Game.Helpers;

namespace GameX.Game.Content
{
    public class Moves
    {
        public static List<Move> GetDefaultMelees(MoveType Types)
        {
            #region Objects

            // Movement

            Move MoveFront = new Move
            {
                Name = "Move Front",
                Value = 2,
                Type = MoveType.Movement
            };

            Move MoveLeft = new Move
            {
                Name = "Move Left",
                Value = 3,
                Type = MoveType.Movement
            };

            Move MoveRight = new Move
            {
                Name = "Move Right",
                Value = 4,
                Type = MoveType.Movement
            };

            Move MoveBack = new Move
            {
                Name = "Move Back",
                Value = 6,
                Type = MoveType.Movement
            };

            Move QuickTurn = new Move
            {
                Name = "Quick Turn",
                Value = 9,
                Type = MoveType.Movement
            };

            Move DashStart = new Move
            {
                Name = "Dash (Start)",
                Value = 71,
                Type = MoveType.Movement
            };

            Move Dashing = new Move
            {
                Name = "Dashing",
                Value = 72,
                Type = MoveType.Movement
            };

            Move DashStop = new Move
            {
                Name = "Dash (Stop)",
                Value = 74,
                Type = MoveType.Movement
            };

            Move DashRight = new Move
            {
                Name = "Dash (Turn Right)",
                Value = 75,
                Type = MoveType.Movement
            };

            Move DashLeft = new Move
            {
                Name = "Dash (Turn Left)",
                Value = 76,
                Type = MoveType.Movement
            };

            Move DashBack = new Move
            {
                Name = "Dash (Turn Back)",
                Value = 77,
                Type = MoveType.Movement
            };

            // Damage

            Move ArmFront = new Move
            {
                Name = "Arm Front",
                Value = 48,
                Type = MoveType.Damage
            };

            Move LegFront = new Move
            {
                Name = "Leg Front",
                Value = 49,
                Type = MoveType.Damage
            };

            Move HeadFlash = new Move
            {
                Name = "Head / Flash",
                Value = 50,
                Type = MoveType.Damage
            };

            Move ArmBack = new Move
            {
                Name = "Arm Back",
                Value = 51,
                Type = MoveType.Damage
            };

            Move FinisherFront = new Move
            {
                Name = "Finisher Front",
                Value = 53,
                Type = MoveType.Damage
            };

            Move FinisherBack = new Move
            {
                Name = "Finisher Back",
                Value = 52,
                Type = MoveType.Damage
            };

            Move Help = new Move
            {
                Name = "Help",
                Value = 55,
                Type = MoveType.Damage
            };

            Move Ground = new Move
            {
                Name = "Ground",
                Value = 56,
                Type = MoveType.Damage
            };

            Move LegBack = new Move
            {
                Name = "Leg Back",
                Value = 57,
                Type = MoveType.Damage
            };

            Move ReunionLegFront = new Move
            {
                Name = "Reunion Leg Front",
                Value = 172,
                Type = MoveType.Damage
            };

            Move ReunionHeadFlash = new Move
            {
                Name = "Reunion Head / Flash",
                Value = 173,
                Type = MoveType.Damage
            };

            Move KnifeHelpSide = new Move
            {
                Name = "Knife Help (Side)",
                Value = 58,
                Type = MoveType.Damage
            };

            Move KnifeHelpUp = new Move
            {
                Name = "Knife Help (Up)",
                Value = 62,
                Type = MoveType.Damage
            };

            Move DashKnee = new Move
            {
                Name = "Dash Knee",
                Value = 73,
                Type = MoveType.Damage
            };

            // Action

            Move Duck = new Move
            {
                Name = "Duck",
                Value = 78,
                Type = MoveType.Action
            };

            Move Reload = new Move
            {
                Name = "Reload",
                Value = 31,
                Type = MoveType.Action
            };

            Move RollFront = new Move
            {
                Name = "Roll Front",
                Value = 81,
                Type = MoveType.Action
            };

            Move RollBack = new Move
            {
                Name = "Roll Back",
                Value = 82,
                Type = MoveType.Action
            };

            Move RollLeft = new Move
            {
                Name = "Roll Left",
                Value = 83,
                Type = MoveType.Action
            };

            Move RollRight = new Move
            {
                Name = "Roll Right",
                Value = 84,
                Type = MoveType.Action
            };

            Move Knife = new Move
            {
                Name = "Knife",
                Value = 34,
                Type = MoveType.Action
            };

            Move Taunt = new Move
            {
                Name = "Taunt",
                Value = 47,
                Type = MoveType.Action
            };

            Move Partner = new Move
            {
                Name = "Call Partner",
                Value = 129,
                Type = MoveType.Action
            };

            #endregion

            switch (Types)
            {
                case MoveType.Movement:
                    return new List<Move>()
                    {
                        MoveFront,
                        MoveBack,
                        MoveRight,
                        MoveLeft,
                        QuickTurn,
                        DashStart,
                        DashStop,
                        Dashing,
                        DashRight,
                        DashLeft,
                        DashBack
                    };
                case MoveType.Damage:
                    return new List<Move>()
                    {
                        ReunionHeadFlash,
                        ReunionLegFront,
                        FinisherFront,
                        FinisherBack,
                        HeadFlash,
                        ArmFront,
                        ArmBack,
                        LegFront,
                        LegBack,
                        Ground,
                        Help,
                        KnifeHelpUp,
                        KnifeHelpSide,
                        DashKnee
                    };
                case MoveType.Action:
                    return new List<Move>()
                    {
                        RollFront,
                        RollBack,
                        RollRight,
                        RollLeft,
                        Duck,
                        Reload,
                        Taunt,
                        Knife,
                        Partner
                    };
                default:
                    return new List<Move>();
            }
        }
    }
}
