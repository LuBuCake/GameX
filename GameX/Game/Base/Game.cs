using GameX.Modules;

namespace GameX.Game.Base
{
    public class Game
    {
        private Memory Kernel { get; set; }

        public Player[] PLAYER;

        public Game(Memory kernel)
        {
            Kernel = kernel;
            PLAYER = new Player[]
            {
                new Player(kernel, 0),
                new Player(kernel, 1),
                new Player(kernel, 2),
                new Player(kernel, 3)
            };
        }

        public void SetStoryModeCharacter(int Index, int Character, int Costume)
        {
            if (Index > 1)
                return;

            Kernel.WriteInt32(Character, "re5dx9.exe", 0xDA383C, 0x71398 + (0x50 * Index));
            Kernel.WriteInt32(Costume, "re5dx9.exe", 0xDA383C, 0x7139C + (0x50 * Index));
        }

        public string GetActiveGameMode()
        {
            int gamemode = Kernel.ReadInt32("re5dx9.exe", 0x00DA383C, 0x954, 0x2088);

            switch (gamemode)
            {
                case 0:
                    return "Campaign";
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

        public int LocalPlayer()
        {
            return Kernel.ReadInt32("re5dx9.exe", 0x00DA383C, 0x954, 0x24B0);
        }

        public string LocalPlayerNick()
        {
            byte[] bytes = Kernel.ReadBytes(10, "re5dx9.exe", 0xDA383C, 0x86200);
            char[] chars = System.Text.Encoding.UTF8.GetString(bytes).ToCharArray();

            return new string(chars);
        }

        public int ActivePlayers()
        {
            return Kernel.ReadInt32("re5dx9.exe", 0x00DA383C, 0x34);
        }

        public bool InGame()
        {
            return Kernel.ReadInt32("re5dx9.exe", 0x00E39D44, 0x220) == 1;
        }
    }
}
