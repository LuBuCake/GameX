using DevExpress.XtraEditors;
using GameX.Helpers;
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
                Environment.NewLine + "App Commands: ",
                "Reinject - Clears both App and Game edits and performs a reinject in the Game's process.",
                "Clear - Clears the console output.",
                "Help - Shows all available commands.",
                "FPS - Shows the current FPS.",
                "FrameTime - Shows the last frametime.",
                "CurTime - Shows the elapsed time in seconds since the program opened",
                "Exit - Closes the App." + Environment.NewLine,
                "Game Commands: ",
                "GetHealth p1/p2/p3/p4 - Gets the current health for the respective player.",
                "SetHealth p1/p2/p3/p4 Value - Sets the health for the respective player, this needs to be set between 0 and 1000."
            };

            foreach (string Command in Commands)
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
                {
                    if (!(Player >= 1 && Player <= 4))
                    {
                        WriteLine("Please specify a player index between 1 and 4");
                        return true;
                    }

                    if (!Main.Game.Players[Player - 1].IsActive())
                    {
                        WriteLine("The selected player is not present.");
                        return true;
                    }

                    WriteLine($"{Main.Game.Players[Player - 1].GetHealth()}");
                }
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
                        if (!(Player >= 1 && Player <= 4))
                        {
                            WriteLine("Please specify a player index between 1 and 4");
                            return true;
                        }

                        if (Main.Game.GetActiveGameMode() == "Versus")
                        {
                            WriteLine("Versus mode detected, operation ignored.");
                            return true;
                        }

                        if (!Main.Game.Players[Player - 1].IsActive())
                        {
                            WriteLine("The selected player is not present.");
                            return true;
                        }

                        Main.Game.Players[Player - 1].SetHealth((short)Utility.Clamp(HP, 0, 1000));
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
            Command = Command.ToLower();
            Command = Utility.RemoveWhiteSpace(Command);

            WriteLine(Command);

            if (Command == "reinject")
                Main.Process_Exited(null, null);
            else if (Command == "clear")
                Clear();
            else if (Command == "help")
                ShowCommands();
            else if (Command == "fps")
                WriteLine(Main.FramesPerSecond.ToString().Substring(0, 5));
            else if (Command == "frametime")
                WriteLine(Main.FrameTime.ToString().Substring(0, 5));
            else if (Command == "curtime")
                WriteLine(((int)Main.CurTime).ToString());
            else if (Command == "exit")
                Application.Exit();
            else if (!ProcessGameCommand(Command))
                WriteLine("Unknown or incorrect use of command. Type Help to see all available commands and their syntax.");
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
