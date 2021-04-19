using DevExpress.XtraEditors;
using GameX.Game.Content;
using GameX.Game.Modules;
using GameX.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GameX.Base.Types;

namespace GameX.Base.Modules
{
    public class Terminal
    {
        private static App Main { get; set; }
        private static string[] ConsoleTextInterfaces { get; set; }
        public static int ActiveInterface { get; private set; }

        public enum ConsoleInterface
        {
            Console = 0,
            Server = 1,
            Client = 2
        }

        public static void StartModule(App GameXRef)
        {
            Main = GameXRef;
            ConsoleTextInterfaces = new string[3];
            ActiveInterface = 0;
        }

        public static void Interface_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            ListItem Interface = CBE.SelectedItem as ListItem;

            ActiveInterface = Interface.Value;
            Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
        }

        public static void ClearConsole_Click(object sender, EventArgs e)
        {
            ConsoleTextInterfaces[ActiveInterface] = "";
            Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
        }

        private static void ShowCommands()
        {
            string[] Commands =
            {
                Environment.NewLine + "[Console] Commands must be written as follows, no spaces except for arguments that need spacing like messages., "
                + "and for arguments you must separe the command and each argument with '::'.",
                "Example: SetHealth::P1::750 = SetHealth is the command, P1 the first argument and finally 750, the third argument.",

                Environment.NewLine + "App commands:",
                "WriteJson::Chars/Items - Writes the default json for each of the collection's objects.",
                "Reinject - Clears both the app and game edits and performs a reinject in the game's process.",
                "Help - Shows all available commands.",
                "FPS - Shows the current FPS.",
                "FrameTime - Shows the last frametime.",
                "CurTime - Shows the elapsed time in seconds since the program opened",
                "Exit - Closes the App.",

                Environment.NewLine + "Network commands:",
                "GetPublicIPv4 - Returns your public IPv4.",
                "GetPrivateIPv4 - Returns your private IPv4.",

                Environment.NewLine + "Server commands:",
                "ServerClients - Lists all connected clients.",
                "ServerStats - Shows the server's statistics.",

                Environment.NewLine + "Client commands:",
                "ClientStats - Shows the client's statistics.",

                Environment.NewLine + "Game commands:",
                "GetHealth::P1/P2/P3/P4 - Gets the current health for the respective player.",
                "SetHealth::P1/P2/P3/P4::Ammount - Sets the health for the respective player, this needs to be set between 0 and 1000."
            };

            foreach (string Command in Commands)
                WriteLine(Command);
        }

        private static bool ProcessGameCommand(string[] Command)
        {
            if (Command[0] == "gethealth" && Command.Length == 2)
            {
                if (!Main.Initialized || !Biohazard.ModuleStarted)
                {
                    WriteLine("[Console] The game is not running.");
                    return true;
                }

                if (int.TryParse(Command[1][1].ToString(), out int Player))
                {
                    if (!(Player >= 1 && Player <= 4))
                    {
                        WriteLine("[Console] Please specify a player index between 1 and 4");
                        return true;
                    }

                    if (!Biohazard.Players[Player - 1].IsActive())
                    {
                        WriteLine("[Console] The selected player is not present.");
                        return true;
                    }

                    WriteLine($"{Biohazard.Players[Player - 1].GetHealth()}");
                    return true;
                }

                return false;
            }
            else if (Command[0] == "sethealth" && Command.Length == 3)
            {
                if (!Main.Initialized || !Biohazard.ModuleStarted)
                {
                    WriteLine("[Console] The game is not running.");
                    return true;
                }

                if (int.TryParse(Command[1][1].ToString(), out int Player))
                {
                    if (int.TryParse(Command[2], out int HP))
                    {
                        if (!(Player >= 1 && Player <= 4))
                        {
                            WriteLine("[Console] Please specify a player index between 1 and 4");
                            return true;
                        }

                        if (Biohazard.GetActiveGameMode() == "Versus")
                        {
                            WriteLine("[Console] Versus mode detected, operation ignored.");
                            return true;
                        }

                        if (!Biohazard.Players[Player - 1].IsActive())
                        {
                            WriteLine("[Console] The selected player is not present.");
                            return true;
                        }

                        short TargetHP = (short)Utility.Clamp(HP, 0, Biohazard.Players[Player - 1].GetMaxHealth());
                        Biohazard.Players[Player - 1].SetHealth(TargetHP);
                        WriteLine($"Player {Player} health set to {TargetHP}.");

                        return true;
                    }
                }

                return false;
            }

            return false;
        }

        private static bool ProcessNetworkCommand(string[] Command)
        {
            if (Command[0] == "getpublicipv4")
            {
                if (Network.HasConnection)
                    WriteLine($"[Console] Your public IPv4 is: {Network.PublicIPv4}");
                else
                    WriteLine("[Console] The Network module is not running, enable it before trying its commands.");

                return true;
            }
            else if (Command[0] == "getprivateipv4")
            {
                if (Network.HasConnection)
                    WriteLine($"[Console] Your private IPv4 is: {Network.PrivateIPv4}");
                else
                    WriteLine("[Console] The Network module is not running, enable it before trying its commands.");

                return true;
            }

            return false;
        }

        private static bool ProcessServerCommand(string[] Command)
        {
            if (Command[0] == "serverclients")
            {
                if (Network.Server == null)
                {
                    WriteLine("[Console] There is no server running, start one before trying its commands.");
                    return true;
                }

                List<string> clients = Network.Server.ListClients().ToList();

                if (clients != null && clients.Count > 0)
                {
                    WriteLine("[Console] Connected clients:");

                    foreach (string cli in clients)
                    {
                        WriteLine(cli);
                    }
                }
                else
                {
                    WriteLine("[Console] No clients connected.");
                }

                return true;
            }
            else if (Command[0] == "serverstats")
            {
                if (Network.Server == null)
                {
                    WriteLine("[Console] There is no server running, start one before trying its commands.");
                    return true;
                }

                WriteLine(Network.Server.Statistics.ToString());
                return true;
            }

            return false;
        }

        private static bool ProcessClientCommand(string[] Command)
        {
            if (Command[0] == "clientstats")
            {
                if (Network.Server == null)
                {
                    WriteLine("[Console] You are not connected to any server, connect to one first.");
                    return true;
                }

                WriteLine(Network.BuddyServer.Connector.Statistics.ToString());
                return true;
            }

            return false;
        }

        private static void ProcessCommand(string RawCommand)
        {
            WriteLine(RawCommand);

            string[] Command;

            if (RawCommand.Contains("::"))
            {
                string[] TempSplit = RawCommand.Split(new string[] { "::" }, StringSplitOptions.None);
                Command = new string[TempSplit.Length];

                for (int i = 0; i < TempSplit.Length; i++)
                {
                    Command[i] = TempSplit[i];
                }

                Command[0] = Utility.RemoveWhiteSpace(Command[0].ToLower());
            }
            else
            {
                Command = new string[1];
                Command[0] = Utility.RemoveWhiteSpace(RawCommand.ToLower());
            }

            if (Command[0] == "writejson" && Utility.RemoveWhiteSpace(Command[1].ToLower()) == "chars")
                Characters.WriteDefaultChars();
            else if (Command[0] == "writejson" && Utility.RemoveWhiteSpace(Command[1].ToLower()) == "items")
                WriteLine("Not implemented yet.");
            else if (Command[0] == "reinject")
                Main.Target_Exited(null, null);
            else if (Command[0] == "help")
                ShowCommands();
            else if (Command[0] == "fps")
                WriteLine(Main.FramesPerSecond.ToString().Substring(0, 5));
            else if (Command[0] == "frametime")
                WriteLine(Main.FrameTime.ToString().Substring(0, 5));
            else if (Command[0] == "curtime")
                WriteLine(((int)Main.CurTime).ToString());
            else if (Command[0] == "exit")
                Application.Exit();
            else if (!ProcessGameCommand(Command) && !ProcessNetworkCommand(Command) && !ProcessServerCommand(Command) && !ProcessClientCommand(Command))
                WriteLine("[Console] Unknown or incorrect use of command. Type Help to see all available commands and their syntax.");
        }

        public static void ValidateInput(object sender, EventArgs e)
        {
            TextEdit TE = sender as TextEdit;

            if (TE.Text != "")
            {
                if (ActiveInterface == (int)ConsoleInterface.Console)
                    ProcessCommand(TE.Text);
                else
                {
                    if (ActiveInterface == (int)ConsoleInterface.Server)
                        Network.Server_BroadcastMessage("[CHAT]" + $"{Main.PlayerNameTextEdit.Text}: " + TE.Text, "", true);
                    else
                    {
                        Network.Client_SendMessage("[CHAT]" + $"{Main.PlayerNameTextEdit.Text}: " + TE.Text, true);
                    }
                }
            }

            TE.Text = "";
        }

        public static void WriteMessage(string Message, int Interface)
        {
            Main.ConsoleInputTextEdit.Text = "";

            string Current = ConsoleTextInterfaces[Interface];

            if (string.IsNullOrWhiteSpace(Current))
                Current = Message;
            else
                Current += Environment.NewLine + Message;

            ConsoleTextInterfaces[Interface] = Current;

            if (Interface == ActiveInterface)
            {
                Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
                Main.ConsoleOutputMemoEdit.SelectionStart = Main.ConsoleOutputMemoEdit.Text.Length;
                Main.ConsoleOutputMemoEdit.MaskBox.MaskBoxScrollToCaret();
            }
        }

        public static void WriteLine(string Output)
        {
            Main.ConsoleInputTextEdit.Text = "";

            string Current = ConsoleTextInterfaces[(int)ConsoleInterface.Console];

            if (string.IsNullOrWhiteSpace(Current))
                Current = Output;
            else
                Current += Environment.NewLine + Output;

            ConsoleTextInterfaces[(int)ConsoleInterface.Console] = Current;
            Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
            Main.ConsoleOutputMemoEdit.SelectionStart = Main.ConsoleOutputMemoEdit.Text.Length;
            Main.ConsoleOutputMemoEdit.MaskBox.MaskBoxScrollToCaret();
        }
    }
}
