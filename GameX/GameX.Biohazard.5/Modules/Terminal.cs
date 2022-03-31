using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GameX.Helpers;
using GameX.Modules;
using GameX.Enum;
using System.Diagnostics;
using System.Linq;

namespace GameX.Modules
{
    public static class Terminal
    {
        private static App GUI { get; set; }
        private static string DisplayedText { get; set; }

        public static void StartModule(App Instance)
        {
            GUI = Instance;
            DisplayedText = "";
            WriteLine("[Console] Module started successfully.");
        }

        public static void ClearConsole_Click(object sender, EventArgs e)
        {
            DisplayedText = "";
            GUI.ConsoleOutputMemoEdit.Text = DisplayedText;
        }

        public static void ScrollToEnd()
        {
            GUI.ConsoleOutputMemoEdit.SelectionStart = GUI.ConsoleOutputMemoEdit.Text.Length;
            GUI.ConsoleOutputMemoEdit.MaskBox?.MaskBoxScrollToCaret();
        }

        private static void ShowCommands()
        {
            string[] Commands =
            {
                Environment.NewLine + "Commands must be written without spaces and " + "arguments must be separated from the command and others arguments with :: (double colons).",

                Environment.NewLine + "Example: SetHealth::P1::1000",
                "SetHealth is the command, P1 the first argument and 1000 the second argument.",

                Environment.NewLine + "App commands:",
                "Help - Shows all available commands.",
                "FPS - Shows the current FPS.",
                "FrameTime - Shows the last frametime.",
                "CurTime - Shows the elapsed time in seconds since the program opened",
                "Exit - Closes the App.",

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
                default:
                    break;
            }

            return false;
        }

        private static bool ProcessGameCommand(string[] Command)
        {
            if (Command[0] != "gethealth" || Command.Length != 2)
            {
                if (Command[0] != "sethealth" || Command.Length != 3)
                    return false;

                if (!GUI.Initialized || !Biohazard.ModuleStarted)
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

                if (Biohazard.GameMode == (int)GameModeEnum.Versus)
                {
                    WriteLine("[Biohazard] Versus mode detected, operation ignored.");
                    return true;
                }

                if (!Biohazard.Players[Player - 1].IsActive())
                {
                    WriteLine("[Biohazard] The selected player is not present.");
                    return true;
                }

                short TargetHP = (short) Utility.Clamp(HP, 0, Biohazard.Players[Player - 1].MaxHealth);
                Biohazard.Players[Player - 1].Health = TargetHP;
                WriteLine($"Player {Player} health set to {TargetHP}.");

                return true;
            }
            else
            {
                if (!GUI.Initialized || !Biohazard.ModuleStarted)
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

                WriteLine($"The player {Player} has {Biohazard.Players[Player - 1].Health} health points.");
                return true;
            }
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
                case "help":
                    ShowCommands();
                    break;
                case "fps":
                    WriteLine($"[App] {GUI.FramesPerSecond.ToString().Substring(0, 5)}");
                    break;
                case "frametime":
                    WriteLine($"[App] {GUI.FrameTime.ToString().Substring(0, 5)}");
                    break;
                case "curtime":
                    WriteLine($"[App] {(int) GUI.CurTime}");
                    break;
                case "exit":
                    Application.Exit();
                    break;
                default:
                {
                    if (!ProcessDevCommand(Command) && !ProcessGameCommand(Command))
                        WriteLine("[Console] Unknown or incorrect use of command. Type Help to see all available commands and their syntax.");

                    break;
                }
            }
        }

        public static void ValidateInput(object sender, EventArgs e)
        {
            TextEdit TE = sender as TextEdit;

            if (TE.Text != "")
                ProcessCommand(TE.Text);

            TE.Text = "";
        }

        public static void WriteLine(Exception Ex)
        {
            string Input = $"[Stack][{Ex.GetType().Name}] {new StackTrace(Ex).GetFrame(0).GetMethod().Name}: {Ex.Message}";

            string Current = DisplayedText;

            if (string.IsNullOrWhiteSpace(Current))
                Current = Input;
            else
                Current += Environment.NewLine + Input;

            DisplayedText = Current;

            if (GUI.ConsoleOutputMemoEdit.InvokeRequired)
            {
                GUI.ConsoleOutputMemoEdit.Invoke((MethodInvoker)delegate
                {
                    GUI.ConsoleOutputMemoEdit.Text = DisplayedText;
                    ScrollToEnd();
                });

                GUI.MasterTabControl.Invoke((MethodInvoker)delegate
                {
                    if (GUI.MasterTabControl.SelectedTabPage != GUI.MasterTabControl.TabPages.Where(x => x.Name == "TabPageConsole").FirstOrDefault())
                        GUI.TabPageConsoleButton.ImageOptions.Image = Properties.Resources.consoleunread;
                });

                return;
            }

            GUI.ConsoleOutputMemoEdit.Text = DisplayedText;
            ScrollToEnd();

            if (GUI.MasterTabControl.SelectedTabPage != GUI.MasterTabControl.TabPages.Where(x => x.Name == "TabPageConsole").FirstOrDefault())
                GUI.TabPageConsoleButton.ImageOptions.Image = Properties.Resources.consoleunread;
        }

        public static void WriteLine(string Input, MessageBoxTypeEnum MessageBox = MessageBoxTypeEnum.None)
        {
            if (MessageBox != MessageBoxTypeEnum.None)
            {
                string Message = Input;

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
                    case MessageBoxTypeEnum.Error:
                        Utility.MessageBox_Error(Message);
                        break;
                    case MessageBoxTypeEnum.Information:
                        Utility.MessageBox_Information(Message);
                        break;
                    case MessageBoxTypeEnum.Warning:
                        Utility.MessageBox_Warning(Message);
                        break;
                }
            }

            string Current = DisplayedText;

            if (string.IsNullOrWhiteSpace(Current))
                Current = Input;
            else
                Current += Environment.NewLine + Input;

            DisplayedText = Current;

            if (GUI.ConsoleOutputMemoEdit.InvokeRequired)
            {
                GUI.ConsoleOutputMemoEdit.Invoke((MethodInvoker)delegate
                {
                    GUI.ConsoleOutputMemoEdit.Text = DisplayedText;
                    ScrollToEnd();
                });

                GUI.MasterTabControl.Invoke((MethodInvoker)delegate
                {
                    if (GUI.MasterTabControl.SelectedTabPage != GUI.MasterTabControl.TabPages.Where(x => x.Name == "TabPageConsole").FirstOrDefault())
                        GUI.TabPageConsoleButton.ImageOptions.Image = Properties.Resources.consoleunread;
                });

                return;
            }

            GUI.ConsoleOutputMemoEdit.Text = DisplayedText;
            ScrollToEnd();

            if (GUI.MasterTabControl.SelectedTabPage != GUI.MasterTabControl.TabPages.Where(x => x.Name == "TabPageConsole").FirstOrDefault())
                GUI.TabPageConsoleButton.ImageOptions.Image = Properties.Resources.consoleunread;
        }
    }
}