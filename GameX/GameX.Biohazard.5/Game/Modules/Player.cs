using System;
using System.Numerics;
using GameX.Base.Modules;
using GameX.Game.Helpers;

namespace GameX.Game.Modules
{
    public class Player
    {
        private int _INDEX { get; set; }
        public Inventory _Inventory { get; set; }

        public Player(int Index)
        {
            _INDEX = Index;
            _Inventory = new Inventory(Index);
        }

        public int GetBaseAddress()
        {
            return Memory.ReadInt16("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX));
        }

        public bool IsActive()
        {
            return GetBaseAddress() != 0;
        }

        public Tuple<int, int> GetCharacter()
        {
            int Character = Memory.ReadInt32("re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * _INDEX));
            int Costume = Memory.ReadInt32("re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * _INDEX));

            return new Tuple<int, int>(Character, Costume);
        }

        public void SetCharacter(int Character, int Costume)
        {
            Memory.WriteInt32(Character, "re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * _INDEX));
            Memory.WriteInt32(Costume, "re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * _INDEX));
        }

        public short GetHealth()
        {
            return Memory.ReadInt16("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1364);
        }

        public void SetHealth(short Value)
        {
            Memory.WriteInt16(Value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1364);
        }

        public short GetMaxHealth()
        {
            return Memory.ReadInt16("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1366);
        }

        public void SetMaxHealth(short Value)
        {
            Memory.WriteInt16(Value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1366);
        }

        public void SetUntargetable(bool Invulnerable)
        {
            Memory.WriteInt16(Invulnerable ? 0 : 1, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x135C);
        }

        public bool IsAI()
        {
            return Memory.ReadInt32("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2DA8) == 1;
        }

        public int GetDefaultHandness()
        {
            Tuple<int, int> Char = GetCharacter();

            switch (Char.Item1)
            {
                case 1:
                    return 1;
                default:
                    return 0;
            }
        }

        public int GetDefaultWeaponMode()
        {
            Tuple<int, int> Char = GetCharacter();

            switch (Char.Item1)
            {
                case 0:
                case 3:
                case 4:
                case 6:
                case 134:
                    return 0;
                default:
                    return 1;
            }
        }

        public void SetHandness(byte[] Mode)
        {
            // 0 = Right 1 = Left

            Memory.WriteBytes(Mode, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1AF9);
        }

        public void SetWeaponMode(byte[] Mode)
        {
            // 0 = Male 1 = Female

            Memory.WriteBytes(Mode, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1AF8);
        }

        public void SetRapidFire(bool Enable)
        {
            for (int slot = 0; slot < 9; slot++)
            {
                Memory.WriteBytes(Enable ? new byte[] {0x01} : new byte[] {0x00}, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x21C5 + (+0x48 * slot));
            }
        }

        public void SetInfiniteAmmo(bool Enable)
        {
            for (int slot = 0; slot < 9; slot++)
            {
                Memory.WriteBytes(Enable ? new byte[] {0x80} : new byte[] {0x00}, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x21C7 + (+0x48 * slot));
            }
        }

        public void ResetDash()
        {
            Memory.WriteInt32(0, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x210F);
            Memory.WriteFloat(1.3f, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x10DC, 0x120, 0x1C);
            Memory.WriteFloat(0f, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x10DC, 0x120, 0x2C);
        }

        public void SetMeleeTarget(int EntityAddress)
        {
            Memory.WriteInt32(EntityAddress, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2DA4);
        }

        public int GetMelee()
        {
            return Memory.ReadInt32("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x10E0);
        }

        public bool GetTargetedMelee()
        {
            Tuple<int, int> CharCos = GetCharacter();
            int Melee = GetMelee();

            switch (CharCos.Item1)
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

        public Vector3 GetPosition()
        {
            float X = Memory.ReadFloat("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2AD0);
            float Y = Memory.ReadFloat("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2AD4);
            float Z = Memory.ReadFloat("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2AD8);

            return new Vector3(X, Y, Z);
        }

        public void SetPosition(Vector3 Position)
        {
            Memory.WriteFloat(Position.X, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2AD0);
            Memory.WriteFloat(Position.Y, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2AD4);
            Memory.WriteFloat(Position.Z, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2AD8);
        }

        public void SetMeleePosition(Vector3 Position)
        {
            Memory.WriteFloat(Position.X, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2E10);
            Memory.WriteFloat(Position.Y, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2E14);
            Memory.WriteFloat(Position.Z, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2E18);
        }
    }
}