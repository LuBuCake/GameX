﻿using GameX.Modules;
using System;

namespace GameX.Game.Base
{
    public class RESIDENTEVIL5
    {
        private Memory Kernel { get; set; }

        public RESIDENTEVIL5(Memory kernel)
        {
            Kernel = kernel;
        }

        #region Game

        public int LocalPlayer()
        {
            return Kernel.ReadInt32("re5dx9.exe", 0xDA383C, 0x954, 0x24B0);
        }

        #endregion

        #region Character

        public Tuple<int, int> GetCharacter(int Index)
        {
            int Character = Kernel.ReadInt32("re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * Index));
            int Costume = Kernel.ReadInt32("re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * Index));

            return new Tuple<int, int>(Character, Costume);
        }

        public void SetCharacter(int Index, int Character, int Costume)
        {
            Kernel.WriteInt32(Character, "re5dx9.exe", 0xDA383C, 0x6FE08 + (0x50 * Index));
            Kernel.WriteInt32(Costume, "re5dx9.exe", 0xDA383C, 0x6FE0C + (0x50 * Index));
        }

        #endregion
    }
}
