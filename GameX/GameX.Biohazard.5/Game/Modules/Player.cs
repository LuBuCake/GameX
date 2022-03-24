using System;
using System.Numerics;
using GameX.Base.Modules;
using GameX.Game.Helpers;
using Newtonsoft.Json.Linq;

namespace GameX.Game.Modules
{
    public class Player
    {
        private int Index { get; set; }
        public Inventory Inventory { get; set; }

        public Player(int Index)
        {
            this.Index = Index;
            Inventory = new Inventory(Index);
        }

        #region Props

        public int Address
        {
            get { return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index)); }
        }

        public int Character
        {
            get { return Memory.Read<int>("re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * Index)); }
            set { Memory.Write(value, "re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * Index)); }
        }

        public int Costume
        {
            get { return Memory.Read<int>("re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * Index)); }
            set { Memory.Write(value, "re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * Index)); }
        }

        public bool Invulnerable
        {
            get { return Memory.Read<byte>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x135C) == 0; }
            set { Memory.Write(!value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x135C); }
        }

        public bool AI
        {
            get { return Memory.Read<byte>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x135C) == 1; }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x135C); }
        }

        public short Health
        {
            get { return Memory.Read<short>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1364); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1364); }
        }

        public short MaxHealth
        {
            get { return Memory.Read<short>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1366); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1366); }
        }

        public byte Handness
        {
            get { return Memory.Read<byte>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1AF9); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1AF9); }
        }

        public byte WeaponMode
        {
            get { return Memory.Read<byte>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1AF8); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x1AF8); }
        }

        public int Melee
        {
            get { return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x10E0); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x10E0); }
        }

        public int MeleeTarget
        {
            get { return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2DA4); }
            set { Memory.Write(value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2DA4); }
        }

        public Vector3 Position
        {
            get
            {
                float X = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD0);
                float Y = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD4);
                float Z = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD8);

                return new Vector3(X, Y, Z);
            }
            set
            {
                Memory.Write(value.X, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD0);
                Memory.Write(value.Y, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD4);
                Memory.Write(value.Z, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2AD8);
            }
        }

        public Vector3 MeleePosition
        {
            get
            {
                float X = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E10);
                float Y = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E14);
                float Z = Memory.Read<float>("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E18);

                return new Vector3(X, Y, Z);
            }
            set
            {
                Memory.Write(value.X, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E10);
                Memory.Write(value.Y, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E14);
                Memory.Write(value.Z, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x2E18);
            }
        }

        #endregion

        #region Methods

        public bool IsActive() => Address != 0;

        public int GetDefaultHandness()
        {
            switch (Character)
            {
                case (int)Characters.Sheva:
                    return (int)Helpers.Handness.Left;
                default:
                    return (int)Helpers.Handness.Right;
            }
        }

        public int GetDefaultWeaponMode()
        {
            switch (Character)
            {
                case (int)Characters.Chris:
                case (int)Characters.Wesker:
                case (int)Characters.Josh:
                case (int)Characters.Barry:
                case (int)Characters.Irving:
                    return (int)Helpers.WeaponMode.Male;
                default:
                    return (int)Helpers.WeaponMode.Female;
            }
        }

        public bool DoingGrabMelee()
        {
            switch (Character)
            {
                case (int)Characters.Chris when Melee == (int)GrabMoves.LegBack:
                case (int)Characters.Sheva when Melee == (int)GrabMoves.LegBack || Melee == (int)GrabMoves.FinisherFront:
                case (int)Characters.Jill when Melee == (int)GrabMoves.LegBack:
                case (int)Characters.Josh when Melee == (int)GrabMoves.LegBack || Melee == (int)GrabMoves.FinisherFront:
                case (int)Characters.Excella when Melee == (int)GrabMoves.LegBack || Melee == (int)GrabMoves.ReunionLegFront:
                case (int)Characters.Barry when Melee == (int)GrabMoves.ReunionHeadFlash:
                case (int)Characters.Rebecca when Melee == (int)GrabMoves.LegBack:
                case (int)Characters.Irving when Melee == (int)GrabMoves.LegBack:
                    return true;
                default:
                    return false;
            }
        }

        public bool DoingIdleMove()
        {
            switch (Melee)
            {
                case (int)IdleMoves.StandStill:
                case (int)IdleMoves.MoveFront:
                case (int)IdleMoves.MoveBack:
                case (int)IdleMoves.MoveLeft:
                case (int)IdleMoves.MoveRight:
                case (int)IdleMoves.QuickTurn:
                case (int)IdleMoves.Idle:
                case (int)IdleMoves.Running:
                    return true;
                default:
                    return false;
            }
        }

        public void SetRapidFire(bool Enable)
        {
            for (int slot = 0; slot < 9; slot++)
            {
                Memory.WriteBytes(Enable ? new byte[] { 0x01 } : new byte[] { 0x00 }, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x21C5 + (+0x48 * slot));
            }
        }

        public void SetInfiniteAmmo(bool Enable)
        {
            for (int slot = 0; slot < 9; slot++)
            {
                Memory.WriteBytes(Enable ? new byte[] { 0x80 } : new byte[] { 0x00 }, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x21C7 + (+0x48 * slot));
            }
        }

        public void ResetDash()
        {
            Memory.Write(0, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x210F);
            Memory.Write(1.3f, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x10DC, 0x120, 0x1C);
            Memory.Write(0f, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * Index), 0x10DC, 0x120, 0x2C);
        }

        #endregion
    }
}