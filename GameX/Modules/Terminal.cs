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
                "Engine Commands: ",
                "clear - Clears the console output.",
                "help - Shows all available commands.",
                "fps - Shows the current FPS mode.",
                "frametime - Shows the last frametime.",
                "curtime - Shows the elapsed time in seconds since the program opened",
                "exit - Closes the App.",
                "Game Commands: "
            };

            foreach(string Command in Commands)
                WriteLine(Command);
        }

        private static void ProcessCommand(string Command)
        {
            Command = Command.ToLower();

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
            else
                WriteLine($"Unknown command {Command}. Type help to see all available commands.");
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
