﻿using GameX.Modules;
using System;

namespace GameX.Game.Base
{
    public class Player
    {
        private Memory Kernel { get; set; }
        private int _INDEX { get; set; }

        public Player(Memory kernel, int Index)
        {
            Kernel = kernel;
            _INDEX = Index;
        }

        public Tuple<int, int> GetCharacter()
        {
            int Character = Kernel.ReadInt32("re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * _INDEX));
            int Costume = Kernel.ReadInt32("re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * _INDEX));

            return new Tuple<int, int>(Character, Costume);
        }

        public void SetCharacter(int Character, int Costume)
        {
            Kernel.WriteInt32(Character, "re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * _INDEX));
            Kernel.WriteInt32(Costume, "re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * _INDEX));
        }

        public short GetHealth()
        {
            return Kernel.ReadInt16("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1364);
        }

        public void SetHealth(short Value)
        {
            Kernel.WriteInt16(Value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1364);
        }

        public short GetMaxHealth()
        {
            return Kernel.ReadInt16("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1366);
        }

        public void SetMaxHealth(short Value)
        {
            Kernel.WriteInt16(Value, "re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x1366);
        }

        public bool IsAI()
        {
            return Kernel.ReadInt32("re5dx9.exe", 0x00DA383C, 0x24 + (0x04 * _INDEX), 0x2DA8) != 0;
        }
    }
}
