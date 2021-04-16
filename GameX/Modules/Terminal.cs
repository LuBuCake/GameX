using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace GameX.Modules
{
    public class Terminal
    {
        private static App Main { get; set; }
        private static MemoEdit ConsoleOutput { get; set; }
        private static TextEdit ConsoleInput { get; set; }

        public static void LoadApp(App GameXInstance, MemoEdit ConsoleOut, TextEdit ConsoleIn)
        {
            Main = GameXInstance;
            ConsoleOutput = ConsoleOut;
            ConsoleInput = ConsoleIn;
        }

        private static void ShowCommands()
        {
            string[] Commands =
            {
                "OBS: Commands must be written without any spaces.",
                "Engine Commands: ",
                "clear - Clears the console output.",
                "help - Shows all available commands.",
                "fps - Shows the current FPS.",
                "frametime - Shows the last frametime.",
                "curtime - Shows the elapsed time in seconds since the program opened",
                "exit - Closes the App.",
                "Game Commands: ",
                "gethealth p1/p2/p3/p4 - Gets the current health for the respective player.",
                "sethealth p1/p2/p3/p4 value - Sets the health for the respective player, this needs to be set between 0 and 1000."
            };

            foreach(string Command in Commands)
                WriteLine(Command);
        }

        private static bool ProcessGameCommand(string Command)
        {
            if (Command.Contains("gethealth") && Command.Length == 11)
            {
                if (!Main.Initialized || Main.Game == null)
                {
                    WriteLine("The game is not running.");
                    return true;
                }

                if (Command[9] == 'p' && int.TryParse(Command[10].ToString(), out int Player))
                    WriteLine($"{Main.Game.Players[Player-1].GetHealth()}");
                else
                    return false;

                return true;
            }
            else if (Command.Contains("sethealth") && Command.Length >= 12 && Command.Length <= 15)
            {
                if (!Main.Initialized || Main.Game == null)
                {
                    WriteLine("The game is not running.");
                    return true;
                }

                if (Command[9] == 'p' && int.TryParse(Command[10].ToString(), out int Player))
                {
                    if (int.TryParse(Command.Substring(11, Command.Length - 11), out int HP))
                    {
                        Main.Game.Players[Player - 1].SetHealth((short)HP);
                        WriteLine($"Player {Player} health set to {HP}.");
                    }
                    else
                        return false;
                }
                else
                    return false;

                return true;
            }

            return false;
        }

        private static void ProcessCommand(string Command)
        {
            WriteLine(Command);

            Command = Command.ToLower();
            Command = Command.Trim();

            if (Command == "clear")
                Clear();
            else if (Command == "help")
                ShowCommands();
            else if (Command == "fps")
                WriteLine(Main.FramesPerSecond.ToString());
            else if (Command == "frametime")
                WriteLine(Main.FrameTime.ToString());
            else if (Command == "curtime")
                WriteLine(Main.CurTime.ToString());
            else if (Command == "exit")
                Application.Exit();
            else if (!ProcessGameCommand(Command))
                WriteLine($"Unknown or incorrect use of command. Type help to see all available commands and their syntax.");
        }

        public static void ValidateInput(object sender, EventArgs e)
        {
            TextEdit TE = sender as TextEdit;

            if (TE.Text != "")
                ProcessCommand(TE.Text);

            TE.Text = "";
        }

        public static void WriteLine(string Output)
        {
            ConsoleInput.Text = "";

            if (string.IsNullOrWhiteSpace(Output))
                return;

            ConsoleOutput.Text += (ConsoleOutput.Text != "" ? Environment.NewLine : "") + Output;
            ConsoleOutput.SelectionStart = ConsoleOutput.Text.Length;
            ConsoleOutput.MaskBox.MaskBoxScrollToCaret();
        }

        public static void Clear()
        {
            ConsoleOutput.Text = "";
        }
    }
}
