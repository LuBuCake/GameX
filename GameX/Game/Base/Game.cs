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
    }
}
