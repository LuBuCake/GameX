using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GameX.Base.Helpers;
using GameX.Base.Types;

namespace GameX.Base.Modules
{
    public static class Terminal
    {
        private static App Main { get; set; }

        public static void StartModule(App GameXRef)
        {
            Main = GameXRef;
            WriteLine("[Console] Module started successfully.");
        }

        public static void ClearConsole_Click(object sender, EventArgs e)
        {
            Main.ConsoleOutputMemoEdit.Text = "";
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

                Environment.NewLine + "App commands:",
                "Help - Shows all available commands.",
                "FPS - Shows the current FPS.",
                "FrameTime - Shows the last frametime.",
                "CurTime - Shows the elapsed time in seconds since the program opened",
                "Exit - Closes the App.",
            };

            foreach (string Command in Commands)
                WriteLine(Command);
        }

        private static bool ProcessDevCommand(string[] Command)
        {
            switch (Command[0])
            {
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
            return false;
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
            {
                ProcessCommand(TE.Text);
            }

            TE.Text = "";
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

            string Current = Main.ConsoleOutputMemoEdit.Text;

            if (string.IsNullOrWhiteSpace(Current))
                Current = Output;
            else
                Current += Environment.NewLine + Output;

            if (Main.ConsoleOutputMemoEdit.InvokeRequired)
            {
                Main.ConsoleOutputMemoEdit.Invoke((MethodInvoker) delegate
                {
                    Main.ConsoleOutputMemoEdit.Text = Current;
                    ScrollToEnd();
                });
                return;
            }

            Main.ConsoleOutputMemoEdit.Text = Current;
            ScrollToEnd();
        }
    }
}