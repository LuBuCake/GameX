using System;

namespace GameX.Game.Base
{
    public class Player
    {
        private App Main { get; set; }
        private int _INDEX { get; set; }

        public Player(App main, int Index)
        {
            Main = main;
            _INDEX = Index;
        }

        public Tuple<int, int> GetCharacter()
        {
            int Character = Main.Kernel.ReadInt32("re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * _INDEX));
            int Costume = Main.Kernel.ReadInt32("re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * _INDEX));

            return new Tuple<int, int>(Character, Costume);
        }

        public void SetCharacter(int Character, int Costume)
        {
            Main.Kernel.WriteInt32(Character, "re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * _INDEX));
            Main.Kernel.WriteInt32(Costume, "re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * _INDEX));
        }

        public short GetHealth()
        {
            return Main.Kernel.ReadInt16("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1364);
        }

        public void SetHealth(short Value)
        {
            Main.Kernel.WriteInt16(Value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1364);
        }

        public short GetMaxHealth()
        {
            return Main.Kernel.ReadInt16("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1366);
        }

        public void SetMaxHealth(short Value)
        {
            Main.Kernel.WriteInt16(Value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1366);
        }

        public bool IsAI()
        {
            return Main.Kernel.ReadInt32("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2DA8) != 0;
        }
    }
}
