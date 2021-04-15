namespace GameX.Game.Base
{
    public class Master
    {
        private App Main { get; set; }

        public Player[] Players;

        public Master(App main)
        {
            Main = main;
            Players = new Player[]
            {
                new Player(Main, 0),
                new Player(Main, 1),
                new Player(Main, 2),
                new Player(Main, 3)
            };
        }

        public void NoFileChecking(bool Enable)
        {
            Main.Kernel.WriteBytes(!Enable ? new byte[] { 0xC3, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 } : new byte[] { 83, 0x3D, 0x0C, 0xAD, 0x23, 0x01, 0x00 }, "re5dx9.exe", 0xE340);
        }

        public void OnlineCharSwapFixes(bool Enable)
        {
            Main.Kernel.WriteBytes(Enable ? new byte[] { 0xEB } : new byte[] { 0x75 }, "re5dx9.exe", 0x223E17);
            Main.Kernel.WriteBytes(Enable ? new byte[] { 0xEB } : new byte[] { 0x75 }, "re5dx9.exe", 0x2240EF);
            Main.Kernel.WriteBytes(Enable ? new byte[] { 0x90, 0x90, 0x90 } : new byte[] { 0x89, 0x57, 0x0C }, "re5dx9.exe", 0x32FB3B);
            Main.Kernel.WriteBytes(Enable ? new byte[] { 0x00 } : new byte[] { 0x14 }, "re5dx9.exe", 0x89186C);
        }

        public void SetStoryModeCharacter(int Index, int Character, int Costume)
        {
            if (Index > 1)
                return;

            Main.Kernel.WriteInt32(Character, "re5dx9.exe", 0xDA383C, 0x71398 + (0x50 * Index));
            Main.Kernel.WriteInt32(Costume, "re5dx9.exe", 0xDA383C, 0x7139C + (0x50 * Index));
        }

        public string GetActiveGameMode()
        {
            int gamemode = Main.Kernel.ReadInt32("re5dx9.exe", 0x00DA383C, 0x954, 0x2088);

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

        public int LocalPlayer()
        {
            return Main.Kernel.ReadInt32("re5dx9.exe", 0x00DA383C, 0x954, 0x24B0);
        }

        public string LocalPlayerNick()
        {
            byte[] bytes = Main.Kernel.ReadBytes(10, "re5dx9.exe", 0xDA383C, 0x86200);
            char[] chars = System.Text.Encoding.UTF8.GetString(bytes).ToCharArray();

            return new string(chars);
        }

        public int ActivePlayers()
        {
            return Main.Kernel.ReadInt32("re5dx9.exe", 0x00DA383C, 0x34);
        }

        public bool InGame()
        {
            return Main.Kernel.ReadInt32("re5dx9.exe", 0x00E39D44, 0x220) == 1;
        }
    }
}
