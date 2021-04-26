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

        public static string GetActiveGameMode()
        {
            int gamemode = Memory.ReadInt32("re5dx9.exe", 0x00DA383C, 0x954, 0x2088);

            switch (gamemode)
            {
                case 0:
                    return "Story";
                case 1:
                    return "Versus";
                case 2:
                    return "The Mercenaries";
                case 3:
                    return "Lost in Nightmares";
                case 4:
                    return "Desesperate Scape";
                case 5:
                    return "The Mercenaries Reunion";
                default:
                    return "Unknown";
            }
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