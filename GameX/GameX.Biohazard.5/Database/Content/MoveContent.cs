using System.Collections.Generic;
using GameX.Database.Type;
using GameX.Enum;

namespace GameX.Database.Content
{
    public static class MoveContent
    {
        public static List<Move> GetCollection(MoveTypeEnum Type)
        {
            #region Objects

            // Movement

            Move MoveFront = new Move
            {
                Name = "Move Front",
                Value = 2,
                Type = MoveTypeEnum.Movement
            };

            Move MoveLeft = new Move
            {
                Name = "Move Left",
                Value = 3,
                Type = MoveTypeEnum.Movement
            };

            Move MoveRight = new Move
            {
                Name = "Move Right",
                Value = 4,
                Type = MoveTypeEnum.Movement
            };

            Move MoveBack = new Move
            {
                Name = "Move Back",
                Value = 6,
                Type = MoveTypeEnum.Movement
            };

            Move QuickTurn = new Move
            {
                Name = "Quick Turn",
                Value = 9,
                Type = MoveTypeEnum.Movement
            };

            Move DashStart = new Move
            {
                Name = "Dash (Start)",
                Value = 71,
                Type = MoveTypeEnum.Movement
            };

            Move Dashing = new Move
            {
                Name = "Dashing",
                Value = 72,
                Type = MoveTypeEnum.Movement
            };

            Move DashStop = new Move
            {
                Name = "Dash (Stop)",
                Value = 74,
                Type = MoveTypeEnum.Movement
            };

            Move DashRight = new Move
            {
                Name = "Dash (Turn Right)",
                Value = 75,
                Type = MoveTypeEnum.Movement
            };

            Move DashLeft = new Move
            {
                Name = "Dash (Turn Left)",
                Value = 76,
                Type = MoveTypeEnum.Movement
            };

            Move DashBack = new Move
            {
                Name = "Dash (Turn Back)",
                Value = 77,
                Type = MoveTypeEnum.Movement
            };

            // Damage

            Move ArmFront = new Move
            {
                Name = "Arm Front",
                Value = 48,
                Type = MoveTypeEnum.Damage
            };

            Move LegFront = new Move
            {
                Name = "Leg Front",
                Value = 49,
                Type = MoveTypeEnum.Damage
            };

            Move HeadFlash = new Move
            {
                Name = "Head / Flash",
                Value = 50,
                Type = MoveTypeEnum.Damage
            };

            Move ArmBack = new Move
            {
                Name = "Arm Back",
                Value = 51,
                Type = MoveTypeEnum.Damage
            };

            Move FinisherFront = new Move
            {
                Name = "Finisher Front",
                Value = 53,
                Type = MoveTypeEnum.Damage
            };

            Move FinisherBack = new Move
            {
                Name = "Finisher Back",
                Value = 52,
                Type = MoveTypeEnum.Damage
            };

            Move Help = new Move
            {
                Name = "Help",
                Value = 55,
                Type = MoveTypeEnum.Damage
            };

            Move Ground = new Move
            {
                Name = "Ground",
                Value = 56,
                Type = MoveTypeEnum.Damage
            };

            Move LegBack = new Move
            {
                Name = "Leg Back",
                Value = 57,
                Type = MoveTypeEnum.Damage
            };

            Move ReunionLegFront = new Move
            {
                Name = "Reunion Leg Front",
                Value = 172,
                Type = MoveTypeEnum.Damage
            };

            Move ReunionHeadFlash = new Move
            {
                Name = "Reunion Head / Flash",
                Value = 173,
                Type = MoveTypeEnum.Damage
            };

            Move KnifeHelpSide = new Move
            {
                Name = "Knife Help (Side)",
                Value = 58,
                Type = MoveTypeEnum.Damage
            };

            Move KnifeHelpUp = new Move
            {
                Name = "Knife Help (Up)",
                Value = 62,
                Type = MoveTypeEnum.Damage
            };

            Move DashKnee = new Move
            {
                Name = "Dash Knee",
                Value = 73,
                Type = MoveTypeEnum.Damage
            };

            // Action

            Move Duck = new Move
            {
                Name = "Duck",
                Value = 78,
                Type = MoveTypeEnum.Action
            };

            Move Reload = new Move
            {
                Name = "Reload",
                Value = 31,
                Type = MoveTypeEnum.Action
            };

            Move RollFront = new Move
            {
                Name = "Roll Front",
                Value = 81,
                Type = MoveTypeEnum.Action
            };

            Move RollBack = new Move
            {
                Name = "Roll Back",
                Value = 82,
                Type = MoveTypeEnum.Action
            };

            Move RollLeft = new Move
            {
                Name = "Roll Left",
                Value = 83,
                Type = MoveTypeEnum.Action
            };

            Move RollRight = new Move
            {
                Name = "Roll Right",
                Value = 84,
                Type = MoveTypeEnum.Action
            };

            Move Knife = new Move
            {
                Name = "Knife",
                Value = 34,
                Type = MoveTypeEnum.Action
            };

            Move Taunt = new Move
            {
                Name = "Taunt",
                Value = 47,
                Type = MoveTypeEnum.Action
            };

            Move Partner = new Move
            {
                Name = "Call Partner",
                Value = 129,
                Type = MoveTypeEnum.Action
            };

            #endregion

            switch (Type)
            {
                case MoveTypeEnum.Movement:
                    return new List<Move>()
                    {
                        MoveFront,
                        MoveBack,
                        MoveRight,
                        MoveLeft,
                        QuickTurn
                    };
                case MoveTypeEnum.Damage:
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
                case MoveTypeEnum.Action:
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
                case MoveTypeEnum.Dash:
                    return new List<Move>()
                    {
                        DashStart,
                        DashStop,
                        Dashing,
                        DashRight,
                        DashLeft,
                        DashBack
                    };
                default:
                    return new List<Move>();
            }
        }
    }
}
