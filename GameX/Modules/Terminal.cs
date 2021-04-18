using DevExpress.XtraEditors;
using GameX.Game.Content;
using GameX.Game.Modules;
using GameX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GameX.Modules
{
    public class Terminal
    {
        private static App Main { get; set; }
        private static MemoEdit ConsoleOutput { get; set; }
        private static TextEdit ConsoleInput { get; set; }

        public static void LoadApp(App GameXRef, MemoEdit ConsoleOut, TextEdit ConsoleIn)
        {
            Main = GameXRef;
            ConsoleOutput = ConsoleOut;
            ConsoleInput = ConsoleIn;
        }

        private static void ShowCommands()
        {
            string[] Commands =
            {
                Environment.NewLine + "App commands:",
                "WriteJson chars/items - Writes the default json for each of the collection's objects.",
                "Reinject - Clears both the app and game edits and performs a reinject in the game's process.",
                "Clear - Clears the console output.",
                "Help - Shows all available commands.",
                "FPS - Shows the current FPS.",
                "FrameTime - Shows the last frametime.",
                "CurTime - Shows the elapsed time in seconds since the program opened",
                "Exit - Closes the App.",

                Environment.NewLine + "Network commands:",
                "GetPublicIPv4 - Returns your public IPv4, you can use it to connect to other players using GameX.",
                "GetPrivateIPv4 - Returns your private IPv4, it is only used for server hosting.",

                Environment.NewLine + "Server commands:",
                "Server Clients - Lists all connected clients.",
                "Server Stats - Shows the server's statistics.",

                Environment.NewLine + "Game commands:",
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
                if (!Main.Initialized || !Biohazard.ModuleStarted)
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

                    if (!Biohazard.Players[Player - 1].IsActive())
                    {
                        WriteLine("The selected player is not present.");
                        return true;
                    }

                    WriteLine($"{Biohazard.Players[Player - 1].GetHealth()}");
                }
                else
                    return false;

                return true;
            }
            else if (Command.Contains("sethealth") && Command.Length >= 12 && Command.Length <= 15)
            {
                if (!Main.Initialized || !Biohazard.ModuleStarted)
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

                        if (Biohazard.GetActiveGameMode() == "Versus")
                        {
                            WriteLine("Versus mode detected, operation ignored.");
                            return true;
                        }

                        if (!Biohazard.Players[Player - 1].IsActive())
                        {
                            WriteLine("The selected player is not present.");
                            return true;
                        }

                       Biohazard.Players[Player - 1].SetHealth((short)Utility.Clamp(HP, 0, 1000));
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

        private static bool ProcessNetworkCommand(string Command)
        {
            if (Command == "getpublicipv4")
            {
                if (Network.HasConnection)
                    WriteLine($"Your public IPv4 is: {Network.PublicIPv4}");
                else
                    WriteLine("The Network module is not running, enable it before trying its commands.");

                return true;
            }
            else if (Command == "getprivateipv4")
            {
                if (Network.HasConnection)
                    WriteLine($"Your private IPv4 is: {Network.PrivateIPv4}");
                else
                    WriteLine("The Network module is not running, enable it before trying its commands.");

                return true;
            }

            return false;
        }

        private static bool ProcessServerCommand(string Command)
        {
            if (Command == "serverclients")
            {
                if (Network.Server == null)
                {
                    WriteLine("There is no server running, start one before trying its commands.");
                    return true;
                }

                List<string> clients = Network.Server.ListClients().ToList();

                if (clients != null && clients.Count > 0)
                {
                    WriteLine("Connected clients:");

                    foreach (string cli in clients)
                    {
                        WriteLine(cli);
                    }
                }
                else
                {
                    WriteLine("No clients connected.");
                }

                return true;
            }
            else if (Command == "serverstats")
            {
                if (Network.Server == null)
                {
                    WriteLine("There is no server running, start one before trying its commands.");
                    return true;
                }

                WriteLine(Network.Server.Statistics.ToString());
                return true;
            }

            return false;
        }

        private static void ProcessCommand(string Command)
        {
            WriteLine(Command);

            Command = Command.ToLower();
            Command = Utility.RemoveWhiteSpace(Command);
            
            if (Command == "writejsonchars")
                Characters.WriteDefaultChars();
            else if (Command == "writejsonitems")
                WriteLine("Not implemented yet.");
            else if (Command == "reinject")
                Main.Target_Exited(null, null);
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
            else if (!ProcessGameCommand(Command) && !ProcessNetworkCommand(Command) && !ProcessServerCommand(Command))
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
