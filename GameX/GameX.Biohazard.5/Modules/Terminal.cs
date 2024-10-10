using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using GameX.Helpers;
using GameX.Enum;
using DevExpress.XtraEditors;

namespace GameX.Modules
{
    public static class Terminal
    {
        private static App GUI { get; set; }
        private static List<string> InputList { get; set; }
        private static string[] InputText { get; set; }

        public static void Setup(App Instance)
        {
            GUI = Instance;
            InputList = new List<string>();
            InputText = new string[0];
        }

        public static void ClearConsole_Click(object sender, EventArgs e)
        {
            InputList.Clear();
            InputText = InputList.ToArray();
            GUI.ConsoleOutputMemoEdit.Lines = InputText;
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

                Environment.NewLine + "App commands:",
                "Help - Shows all available commands.",
                "Exit - Closes the App.",
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
                case "exit":
                    Application.Exit();
                    break;
                default:
                {
                    if (!ProcessDevCommand(Command))
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
            string Input = $"[{DateTime.Now:HH:mm:ss}][App][{Ex.GetType().Name}] {new StackTrace(Ex).GetFrame(0).GetMethod().Name}: {Ex.Message}";
            UpdateTextAndProcessEvents(Input);
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

            Input = $"[{DateTime.Now:HH:mm:ss}]{Input}";
            UpdateTextAndProcessEvents(Input);
        }

        private static void UpdateTextAndProcessEvents(string Input)
        {
            if (InputList.Count >= 100)
                InputList.RemoveAt(0);

            InputList.Add(Input);
            InputText = InputList.ToArray();

            if (GUI.ConsoleOutputMemoEdit.InvokeRequired)
{
                GUI.ConsoleOutputMemoEdit.Invoke((MethodInvoker)delegate
                {
                    GUI.ConsoleOutputMemoEdit.Lines = InputText;
                    ScrollToEnd();
                });

                GUI.MasterTabControl.Invoke((MethodInvoker)delegate
                {
                    if (GUI.MasterTabControl.SelectedTabPage != GUI.MasterTabControl.TabPages.Where(x => x.Name == "TabPageConsole").FirstOrDefault())
                        GUI.TabPageConsoleButton.ImageOptions.Image = Properties.Resources.consoleunread;
                });

                return;
            }

            GUI.ConsoleOutputMemoEdit.Lines = InputText;
            ScrollToEnd();

            if (GUI.MasterTabControl.SelectedTabPage != GUI.MasterTabControl.TabPages.Where(x => x.Name == "TabPageConsole").FirstOrDefault())
                GUI.TabPageConsoleButton.ImageOptions.Image = Properties.Resources.consoleunread;
        }
    }
}