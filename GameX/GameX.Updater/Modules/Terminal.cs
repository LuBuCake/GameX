using System;
using System.Windows.Forms;
using GameX.Updater.Helpers;

namespace GameX.Updater.Modules
{
    public static class Terminal
    {
        private static App Main { get; set; }

        public static void StartModule(App GameXRef)
        {
            Main = GameXRef;
            WriteLine("[Console] Module started successfully.");
        }

        public static void ScrollToEnd()
        {
            Main.ConsoleOutputMemoEdit.SelectionStart = Main.ConsoleOutputMemoEdit.Text.Length;
            Main.ConsoleOutputMemoEdit.MaskBox?.MaskBoxScrollToCaret();
        }

        public static void WriteLine(string Output, bool DownloadReport = false)
        {
            string Current = Main.ConsoleOutputMemoEdit.Text;

            if (string.IsNullOrWhiteSpace(Current))
                Current = Output;
            else
            {
                if (DownloadReport && Current.Contains("[App] Downloading: "))
                {
                    string NewPercentage = Utility.StringBetween(Output, "[App] Downloading: ", "%");
                    string OldPercentage = Utility.StringBetween(Current, "[App] Downloading: ", "%");

                    Current = Current.Replace(OldPercentage, NewPercentage);
                }
                else
                    Current += Environment.NewLine + Output;
            }

            if (Main.ConsoleOutputMemoEdit.InvokeRequired)
            {
                Main.ConsoleOutputMemoEdit.Invoke((MethodInvoker)delegate
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
