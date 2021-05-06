using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GameX.Base.Helpers;
using GameX.Base.Types;
using GameX.Game.Content;
using GameX.Game.Helpers;
using GameX.Game.Modules;

namespace GameX.Base.Modules
{
    public static class Terminal
    {
        private static App Main { get; set; }
        private static string[] ConsoleTextInterfaces { get; set; }
        public static int ActiveInterface { get; private set; }

        public static void StartModule(App GameXRef)
        {
            Main = GameXRef;
            ConsoleTextInterfaces = new string[3];
            ActiveInterface = 0;
            WriteLine("[Console] Module started successfully.");
        }

        public static void Interface_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            ListItem Interface = CBE.SelectedItem as ListItem;

            ActiveInterface = Interface.Value;
            Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
            ScrollToEnd();
        }

        public static void ClearConsole_Click(object sender, EventArgs e)
        {
            ConsoleTextInterfaces[ActiveInterface] = "";
            Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
        }

        public static void ScrollToEnd()
        {
            Main.ConsoleOutputMemoEdit.SelectionStart = Main.ConsoleOutputMemoEdit.Text.Length;
            Main.ConsoleOutputMemoEdit.MaskBox?.MaskBoxScrollToCaret();
        }

        private static void ShowCommands()
        {
            string[] Commands =
            {
                Environment.NewLine + "Commands must be written without spaces and " + "arguments must be separated from the command and others arguments with :: (double colons).",

                Environment.NewLine + "Example: SetHealth::P1::1000",
                "SetHealth is the command, P1 the first argument and 1000 the second argument.",

                Environment.NewLine + "App commands:",
                "WriteJson::Chars - Writes the default json for each of the base characters, to load them restart the App.",
                "WriteJson::Items - Writes the default json for each of the base items, to load them restart the App.",
                "Help - Shows all available commands.",
                "FPS - Shows the current FPS.",
                "FrameTime - Shows the last frametime.",
                "CurTime - Shows the elapsed time in seconds since the program opened",
                "Exit - Closes the App.",

                Environment.NewLine + "Network commands:",
                "GetIP - Returns all available IPs to use in a connection.",

                Environment.NewLine + "Server commands:",
                "ServerClients - Lists all connected clients.",
                "ServerStats - Shows the server's statistics.",

                Environment.NewLine + "Client commands:",
                "ClientStats - Shows the client's statistics.",

                Environment.NewLine + "Biohazard commands:",
                "GetHealth::P1/P2/P3/P4 - Gets the current health for the respective player.",
                "SetHealth::P1/P2/P3/P4::Ammount - Sets the health for the respective player, this needs to be set between 0 and 1000."
            };

            foreach (string Command in Commands)
                WriteLine(Command);
        }

        private static bool ProcessDevCommand(string[] Command)
        {
            switch (Command[0])
            {
                case "writeappinfo":
                    GameXInfo appinfo = new GameXInfo()
                    {
                        GameXName = "Resident Evil 5",
                        GameXFile = "addons/GameX.Biohazard.5/GameX.Biohazard.5.dll",
                        GameXLogo = new[] { "addons/GameX.Biohazard.5/image/application/logo_a.eia", "addons/GameX.Biohazard.5/image/application/logo_b.eia" },
                        GameXLogoColors = new[] { Color.DarkOrange, Color.White },
                        Platform = "x86"
                    };
                    Serializer.WriteDataFile(@"addons/GameX.Biohazard.5/appinfo.json", Serializer.SerializeGameXInfo(appinfo));
                    break;
                case "encodeimage":
                {
                    string CurrentDirectory = $"{Directory.GetCurrentDirectory()}/";
                    DirectoryInfo DirInfor = new DirectoryInfo(CurrentDirectory);
                    FileInfo[] ImageFiles = DirInfor.GetFiles("*.png");

                    if (ImageFiles.Length <= 0)
                    {
                        WriteLine("[App] No files found with the specified extension.");
                        return true;
                    }

                    foreach (FileInfo ImageFile in ImageFiles)
                    {
                        Encoder.EncodeFile(CurrentDirectory + ImageFile.Name, ".png");
                    }

                    return true;
                }
                case "decodeimage":
                {
                    string CurrentDirectory = $"{Directory.GetCurrentDirectory()}/";
                    DirectoryInfo DirInfor = new DirectoryInfo(CurrentDirectory);
                    FileInfo[] ImageFiles = DirInfor.GetFiles("*.eia");

                    if (ImageFiles.Length <= 0)
                    {
                        WriteLine("[App] No files found with the specified extension.");
                        return true;
                    }

                    foreach (FileInfo ImageFile in ImageFiles)
                    {
                        Encoder.DecodeFile(CurrentDirectory + ImageFile.Name, ".eia");
                    }

                    return true;
                }
            }

            return false;
        }

        private static bool ProcessGameCommand(string[] Command)
        {
            if (Command[0] != "gethealth" || Command.Length != 2)
            {
                if (Command[0] != "sethealth" || Command.Length != 3)
                    return false;

                if (!Main.Initialized || !Biohazard.ModuleStarted)
                {
                    WriteLine("[App] The game is not running.");
                    return true;
                }

                if (!int.TryParse(Command[1][1].ToString(), out int Player))
                    return false;

                if (!int.TryParse(Command[2], out int HP))
                    return false;

                if (!(Player >= 1 && Player <= 4))
                {
                    WriteLine("[Console] Please specify a player index between 1 and 4");
                    return true;
                }

                if (Biohazard.GetActiveGameMode() == (int)GameEnums.GameMode.Versus)
                {
                    WriteLine("[Biohazard] Versus mode detected, operation ignored.");
                    return true;
                }

                if (!Biohazard.Players[Player - 1].IsActive())
                {
                    WriteLine("[Biohazard] The selected player is not present.");
                    return true;
                }

                short TargetHP = (short) Utility.Clamp(HP, 0, Biohazard.Players[Player - 1].GetMaxHealth());
                Biohazard.Players[Player - 1].SetHealth(TargetHP);
                WriteLine($"Player {Player} health set to {TargetHP}.");

                return true;
            }
            else
            {
                if (!Main.Initialized || !Biohazard.ModuleStarted)
                {
                    WriteLine("[App] The game is not running.");
                    return true;
                }

                if (!int.TryParse(Command[1][1].ToString(), out int Player))
                    return false;

                if (!(Player >= 1 && Player <= 4))
                {
                    WriteLine("[Console] Please specify a player index between 1 and 4");
                    return true;
                }

                if (!Biohazard.Players[Player - 1].IsActive())
                {
                    WriteLine("[Biohazard] The selected player is not present.");
                    return true;
                }

                WriteLine($"The player {Player} has {Biohazard.Players[Player - 1].GetHealth()} health points.");
                return true;
            }
        }

        private static bool ProcessNetworkCommand(string[] Command)
        {
            switch (Command[0])
            {
                case "getip":
                {
                    if (!Network.HasConnection)
                    {
                        WriteLine("[App] The Network module is not running, enable it before trying its commands.");
                        return true;
                    }

                    string Output = Network.PrivateIPv4.Aggregate("[Network] Your available IPs for connection are: ", (current, IP) => current + $" ({IP})");

                    WriteLine(Output);

                    return true;
                }
                default:
                    return false;
            }
        }

        private static bool ProcessServerCommand(string[] Command)
        {
            switch (Command[0])
            {
                case "serverclients":
                {
                    if (Network._Server == null)
                    {
                        WriteLine("[App] There is no server running, start one before trying its commands.");
                        return true;
                    }

                    List<string> clients = Network._Server.ListClients().ToList();

                    if (clients.Count > 0)
                    {
                        WriteLine("[Network] Connected clients:");

                        foreach (string cli in clients)
                        {
                            WriteLine(cli);
                        }
                    }
                    else
                    {
                        WriteLine("[Network] No clients connected.");
                    }

                    return true;
                }
                case "serverstats":
                {
                    if (Network._Server == null)
                    {
                        WriteLine("[App] There is no server running, start one before trying its commands.");
                        return true;
                    }

                    WriteLine(Network._Server.Statistics.ToString());
                    return true;
                }
                default:
                    return false;
            }
        }

        private static bool ProcessClientCommand(string[] Command)
        {
            if (Command[0] != "clientstats")
                return false;

            if (Network._Server == null)
            {
                WriteLine("[Console] You are not connected to any server, connect to one first.");
                return true;
            }

            WriteLine(Network._Client.Statistics.ToString());
            return true;
        }

        private static void ProcessCommand(string RawCommand)
        {
            WriteLine($"[Input] {RawCommand}");

            string[] Command;

            if (RawCommand.Contains("::"))
            {
                string[] TempSplit = RawCommand.Split(new[] {"::"}, StringSplitOptions.None);
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

            switch (Command[0])
            {
                case "writejson" when Utility.RemoveWhiteSpace(Command[1].ToLower()) == "chars":
                    Main.CreatePrefabs(Enums.PrefabType.Character, true);
                    break;
                case "writejson" when Utility.RemoveWhiteSpace(Command[1].ToLower()) == "items":
                    Main.CreatePrefabs(Enums.PrefabType.Item, true);
                    break;
                case "help":
                    ShowCommands();
                    break;
                case "fps":
                    WriteLine($"[App] {Main.FramesPerSecond.ToString().Substring(0, 5)}");
                    break;
                case "frametime":
                    WriteLine($"[App] {Main.FrameTime.ToString().Substring(0, 5)}");
                    break;
                case "curtime":
                    WriteLine($"[App] {(int) Main.CurTime}");
                    break;
                case "exit":
                    Application.Exit();
                    break;
                default:
                {
                    if (!ProcessDevCommand(Command) && !ProcessGameCommand(Command) && !ProcessNetworkCommand(Command) && !ProcessServerCommand(Command) && !ProcessClientCommand(Command))
                        WriteLine("[Console] Unknown or incorrect use of command. Type Help to see all available commands and their syntax.");

                    break;
                }
            }
        }

        public static void ValidateInput(object sender, EventArgs e)
        {
            TextEdit TE = sender as TextEdit;

            if (TE.Text != "")
            {
                switch (ActiveInterface)
                {
                    case (int) Enums.ConsoleInterface.Console:
                        ProcessCommand(TE.Text);
                        break;
                    case (int) Enums.ConsoleInterface.Server:
                        Network.Server_BroadcastMessage("[CHAT]" + $"{Main.PlayerNameTextEdit.Text}: " + TE.Text, "", true, true);
                        break;
                    default:
                        Network.Client_SendMessage("[CHAT]" + $"{Main.PlayerNameTextEdit.Text}: " + TE.Text, true, true);
                        break;
                }
            }

            TE.Text = "";
        }

        public static void WriteMessage(string Message, int Interface)
        {
            string Current = ConsoleTextInterfaces[Interface];

            if (string.IsNullOrWhiteSpace(Current))
                Current = Message;
            else
                Current += Environment.NewLine + Message;

            ConsoleTextInterfaces[Interface] = Current;

            if (Interface != ActiveInterface)
                return;

            if (Main.ConsoleOutputMemoEdit.InvokeRequired)
            {
                Main.ConsoleOutputMemoEdit.Invoke((MethodInvoker) delegate
                {
                    Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
                    ScrollToEnd();
                });
                return;
            }

            Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
            ScrollToEnd();
        }

        public static void WriteLine(string Output, Enums.MessageBoxType MessageBox = Enums.MessageBoxType.None)
        {
            if (MessageBox != Enums.MessageBoxType.None)
            {
                string Message = Output;

                if (Message.Contains("[App] "))
                    Message = Message.Replace("[App] ", "");
                else if (Message.Contains("[Memory] "))
                    Message = Message.Replace("[Memory] ", "");
                else if (Message.Contains("[Biohazard] "))
                    Message = Message.Replace("[Biohazard] ", "");
                else if (Message.Contains("[Network] "))
                    Message = Message.Replace("[Network] ", "");
                else if (Message.Contains("[Server] "))
                    Message = Message.Replace("[Server] ", "");
                else if (Message.Contains("[Client] "))
                    Message = Message.Replace("[Client] ", "");
                else if (Message.Contains("[Console] "))
                    Message = Message.Replace("[Console] ", "");

                switch (MessageBox)
                {
                    case Enums.MessageBoxType.Error:
                        Utility.MessageBox_Error(Message);
                        break;
                    case Enums.MessageBoxType.Information:
                        Utility.MessageBox_Information(Message);
                        break;
                    case Enums.MessageBoxType.Warning:
                        Utility.MessageBox_Warning(Message);
                        break;
                }
            }

            string Current = ConsoleTextInterfaces[(int) Enums.ConsoleInterface.Console];

            if (string.IsNullOrWhiteSpace(Current))
                Current = Output;
            else
                Current += Environment.NewLine + Output;

            ConsoleTextInterfaces[(int) Enums.ConsoleInterface.Console] = Current;

            if (Main.ConsoleOutputMemoEdit.InvokeRequired)
            {
                Main.ConsoleOutputMemoEdit.Invoke((MethodInvoker) delegate
                {
                    Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
                    ScrollToEnd();
                });
                return;
            }

            Main.ConsoleOutputMemoEdit.Text = ConsoleTextInterfaces[ActiveInterface];
            ScrollToEnd();
        }
    }
}