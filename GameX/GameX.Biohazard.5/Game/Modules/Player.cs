using System;
using GameX.Base.Modules;

namespace GameX.Game.Modules
{
    public class Player
    {
        private int _INDEX { get; set; }

        public Player(int Index)
        {
            _INDEX = Index;
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
                Memory.WriteBytes(Enable ? new byte[] {0x01} : new byte[] {0x00}, "re5dx9.exe", 0x00DA383C,
                    0x24 + (0x04 * _INDEX), 0x21C5 + (+0x48 * slot));
            }
        }

        public void SetInfiniteAmmo(bool Enable)
        {
            for (int slot = 0; slot < 9; slot++)
            {
                Memory.WriteBytes(Enable ? new byte[] {0x80} : new byte[] {0x00}, "re5dx9.exe", 0x00DA383C,
                    0x24 + (0x04 * _INDEX), 0x21C7 + (+0x48 * slot));
            }
        }
    }
}