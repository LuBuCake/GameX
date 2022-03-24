using GameX.Base.Modules;
using System;

namespace GameX.Game.Modules
{
    public class Biohazard
    {
        public static bool ModuleStarted { get; set; }
        public static Player[] Players { get; set; }

        public static void StartModule()
        {
            Players = new[]
            {
                new Player(0),
                new Player(1),
                new Player(2),
                new Player(3)
            };

            ModuleStarted = true;
            Terminal.WriteLine("[Biohazard] Module started successfully.");
        }

        public static void FinishModule()
        {
            Players = null;
            ModuleStarted = false;
            Terminal.WriteLine("[Biohazard] Module finished successfully.");
        }

        public static void EnableColorFilter(bool Enable)
        {
            Memory.WriteBytes(Enable ? new byte[] { 0xE9, 0x51, 0x04, 0x00, 0x00, 0x90 } : new byte[] { 0x0F, 0x84, 0x50, 0x04, 0x00, 0x00 }, "re5dx9.exe", 0x3C7113);
            Memory.WriteBytes(Enable ? new byte[] { 0xE9, 0x88, 0x66, 0x00, 0x00, 0x90 } : new byte[] { 0x0F, 0x87, 0x87, 0x66, 0x00, 0x00 }, "re5dx9.exe", 0x945D8);
        }

        public static void NoFileChecking(bool Enable)
        {
            Memory.WriteBytes(!Enable ? new byte[] {0xC3, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90} : new byte[] {83, 0x3D, 0x0C, 0xAD, 0x23, 0x01, 0x00}, "re5dx9.exe", 0xE340);
        }

        public static void OnlineCharSwapFixes(bool Enable)
        {
            Memory.WriteBytes(Enable ? new byte[] {0xEB} : new byte[] {0x75}, "re5dx9.exe", 0x223E17);
            Memory.WriteBytes(Enable ? new byte[] {0xEB} : new byte[] {0x75}, "re5dx9.exe", 0x2240EF);
            Memory.WriteBytes(Enable ? new byte[] {0x90, 0x90, 0x90} : new byte[] {0x89, 0x57, 0x0C}, "re5dx9.exe", 0x32FB3B);
            Memory.WriteBytes(Enable ? new byte[] {0x00} : new byte[] {0x14}, "re5dx9.exe", 0x89186C);
        }

        public static void SetStoryModeCharacter(int Index, int Character, int Costume)
        {
            if (Index > 1)
                return;

            Memory.Write(Character, "re5dx9.exe", 0xDA383C, 0x71398 + (0x50 * Index));
            Memory.Write(Costume, "re5dx9.exe", 0xDA383C, 0x7139C + (0x50 * Index));
        }

        public static void WeskerNoSunglassDrop(bool Enable)
        {
            Memory.WriteBytes(Enable ? new byte[] { 0xEB } : new byte[] { 0x75 }, "re5dx9.exe", 0x77B1C1);
        }

        public static void WeskerNoDashCost(bool Enable)
        {
            Memory.Write(Enable ? 0 : 100, "re5dx9.exe", 0xDA3814, 0x80);
        }

        public static int GetActiveGameMode()
        {
            return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x954, 0x2088);
        }

        public static int LocalPlayer()
        {
            return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x954, 0x24B0);
        }

        public static string LocalPlayerNick()
        {
            byte[] bytes = Memory.ReadBytes(10, "re5dx9.exe", 0xDA383C, 0x86200);
            char[] chars = System.Text.Encoding.UTF8.GetString(bytes).ToCharArray();

            return new string(chars);
        }

        public static int ActivePlayers()
        {
            return Memory.Read<int>("re5dx9.exe", 0x00DA383C, 0x34);
        }

        public static bool InGame()
        {
            return Memory.Read<int>("re5dx9.exe", 0x00E39D44, 0x220) == 1;
        }

        public static void SetMelee(string Name, byte Value)
        {
            int Address;

            switch(Name)
            {
                case "Reunion Head / Flash":
                    Address = 0x00B5D583;
                    break;
                case "Reunion Leg Front":
                    Address = 0x00B5D463;
                    break;
                case "Head / Flash":
                    Address = 0x00B5D4F3;
                    break;
                case "Arm Back":
                    Address = 0x00B5D613;
                    break;
                case "Arm Front":
                    Address = 0x00B5D343;
                    break;
                case "Leg Back":
                    Address = 0x00B5D733;
                    break;
                case "Leg Front":
                    Address = 0x00B5D3D3;
                    break;
                case "Finisher Back":
                    Address = 0x00B6C2BF;
                    break;
                case "Finisher Front":
                    Address = 0x00B6C37F;
                    break;
                case "Taunt":
                    Address = 0x00B5D2C7;
                    break;
                case "Knife":
                    Address = 0x00B77C6D;
                    break;
                case "Help":
                    Address = 0x00B5D6A3;
                    break;
                case "Quick Turn":
                    Address = 0x00B5D072;
                    break;
                case "Partner Command":
                    Address = 0x00B5F1FD;
                    break;
                case "Move Left":
                    Address = 0x00B5CD97;
                    break;
                case "Move Right":
                    Address = 0x00B5CE17;
                    break;
                case "Move Back":
                    Address = 0x00B5CEFD;
                    break;
                case "Reload":
                    Address = 0x00B6BCB2;
                    break;
                default:
                    Address = 0;
                    break;
            }

            if (Address > 0)
            {
                Memory.WriteBytes(new[] { Value }, "", Address);
            }
        }

        public static void SetWeaponPlacement(int Mode)
        {
            switch (Mode)
            {
                case 0:
                    Memory.WriteBytes(new[] { (byte)0x75 }, "re5dx9.exe", 0x84D0AC);
                    Memory.WriteBytes(new[] { (byte)0x01 }, "re5dx9.exe", 0x84D0D9);
                    Memory.WriteBytes(new[] { (byte)0x75 }, "re5dx9.exe", 0x84D164);
                    Memory.WriteBytes(new[] { (byte)0x02 }, "re5dx9.exe", 0x84D196);
                    break;
                case 1:
                    Memory.WriteBytes(new[] { (byte)0xEB }, "re5dx9.exe", 0x84D0AC);
                    Memory.WriteBytes(new[] { (byte)0x03 }, "re5dx9.exe", 0x84D0D9);
                    Memory.WriteBytes(new[] { (byte)0x75 }, "re5dx9.exe", 0x84D164);
                    Memory.WriteBytes(new[] { (byte)0x02 }, "re5dx9.exe", 0x84D196);
                    break;
                case 2:
                    Memory.WriteBytes(new[] { (byte)0x75 }, "re5dx9.exe", 0x84D0AC);
                    Memory.WriteBytes(new[] { (byte)0x01 }, "re5dx9.exe", 0x84D0D9);
                    Memory.WriteBytes(new[] { (byte)0xEB }, "re5dx9.exe", 0x84D164);
                    Memory.WriteBytes(new[] { (byte)0x02 }, "re5dx9.exe", 0x84D196);
                    break;
            }
        }

        public static void DisableMeleeCamera(bool Enable)
        {
            Memory.WriteBytes(new[] { Enable ? (byte)0xEB : (byte)0x76 }, "re5dx9.exe", 0x446331);
        }

        public static void EnableReunionSpecialMoves(bool Enable)
        {
            uint bytes_a = 0x7AA23840 - 5 - 0x007EA500;
            uint bytes_b = 0x7AA237F0 - 5 - 0x007E9D58;

            byte[] jmp_a = new byte[5];
            byte[] jmp_b = new byte[6];

            jmp_a[0] = 0xE9;
            jmp_b[0] = 0xE9;
            jmp_b[5] = 0x90;

            byte[] original_a = { 0xA1, 0x6C, 0x98, 0x23, 0x01 };
            byte[] original_b = { 0x83, 0x7A, 0x58, 0x05, 0x74, 0x14 };

            BitConverter.GetBytes(bytes_a).CopyTo(jmp_a, 1);
            BitConverter.GetBytes(bytes_b).CopyTo(jmp_b, 1);

            Memory.WriteBytes(Enable ? jmp_a : original_a, "", 0x007EA500);
            Memory.WriteBytes(Enable ? jmp_b : original_b, "", 0x007E9D58);

            Memory.WriteBytes(Enable ? new[] { (byte)0xEB } : new[] { (byte)0x73 }, "", 0x7AA23879);
            Memory.WriteBytes(Enable ? new[] { (byte)0xEB } : new[] { (byte)0x73 }, "", 0x7AA23829);
        }
    }
}