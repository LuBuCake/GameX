using GameX.Base.Modules;

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

            Memory.WriteInt32(Character, "re5dx9.exe", 0xDA383C, 0x71398 + (0x50 * Index));
            Memory.WriteInt32(Costume, "re5dx9.exe", 0xDA383C, 0x7139C + (0x50 * Index));
        }

        public static void EnableControllerAim(bool Enable)
        {
            Memory.WriteRawAddress(0x00B66ACD, Enable ? new byte[] { 0xEB } : new byte[] { 0x75 });
            Memory.WriteRawAddress(0x00B67FD0, Enable ? new byte[] { 0x90, 0X90 } : new byte[] { 0x74, 0x18 });
            Memory.WriteRawAddress(0x00B84D79, Enable ? new byte[] { 0x90, 0X90 } : new byte[] { 0x74, 0X3A });
            Memory.WriteRawAddress(0x00B74940, Enable ? new byte[] { 0xE9, 0xC5, 0x01, 0x00, 0x00, 0x90 } : new byte[] { 0x0F, 0x85, 0xC4, 0x01, 0x00, 0x00 });
            Memory.WriteRawAddress(0x00B85A80, Enable ? new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 } : new byte[] { 0x0F, 0x84, 0xC4, 0x06, 0x00, 0x00 });
            Memory.WriteRawAddress(0x00B85AEA, Enable ? new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 } : new byte[] { 0x0F, 0x84, 0x44, 0x03, 0x00, 0x00 });
            Memory.WriteRawAddress(0x00B877DB, Enable ? new byte[] { 0x90, 0x90 } : new byte[] { 0x75, 0x16 });
            Memory.WriteRawAddress(0x00B87869, Enable ? new byte[] { 0x90, 0x90 } : new byte[] { 0x74, 0x24 });
            Memory.WriteRawAddress(0x00B87640, Enable ? new byte[] { 0xEB } : new byte[] { 0x74 });
            Memory.WriteRawAddress(0x00B87710, Enable ? new byte[] { 0x90, 0x90 } : new byte[] { 0x75, 0x16 });
            Memory.WriteRawAddress(0x00B877AC, Enable ? new byte[] { 0xEB } : new byte[] { 0x74 });
            Memory.WriteRawAddress(0x00B8721A, Enable ? new byte[] { 0xEB } : new byte[] { 0x74 });
            Memory.WriteRawAddress(0x00B692C1, Enable ? new byte[] { 0x80, 0xC2, 0x01 } : new byte[] { 0x80, 0xC2, 0x02 });
        }

        public static void WeskerNoSunglassDrop(bool Enable)
        {
            Memory.WriteBytes(Enable ? new byte[] { 0xEB } : new byte[] { 0x75 }, "re5dx9.exe", 0x77B1C1);
        }

        public static void WeskerNoDashCost(bool Enable)
        {
            Memory.WriteInt32(Enable ? 0 : 100, "re5dx9.exe", 0xDA3814, 0x80);
        }

        public static int GetActiveGameMode()
        {
            return Memory.ReadInt32("re5dx9.exe", 0x00DA383C, 0x954, 0x2088);
        }

        public static int LocalPlayer()
        {
            return Memory.ReadInt32("re5dx9.exe", 0x00DA383C, 0x954, 0x24B0);
        }

        public static string LocalPlayerNick()
        {
            byte[] bytes = Memory.ReadBytes(10, "re5dx9.exe", 0xDA383C, 0x86200);
            char[] chars = System.Text.Encoding.UTF8.GetString(bytes).ToCharArray();

            return new string(chars);
        }

        public static int ActivePlayers()
        {
            return Memory.ReadInt32("re5dx9.exe", 0x00DA383C, 0x34);
        }

        public static bool InGame()
        {
            return Memory.ReadInt32("re5dx9.exe", 0x00E39D44, 0x220) == 1;
        }
    }
}