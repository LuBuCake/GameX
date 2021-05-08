using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameX.Updater.Base.Modules
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

        public static void WriteLine(string Output)
        {

            string Current = Main.ConsoleOutputMemoEdit.Text;

            if (string.IsNullOrWhiteSpace(Current))
                Current = Output;
            else
                Current += Environment.NewLine + Output;

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
