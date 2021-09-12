using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GameX.Launcher
{
    public static class Program
    {
        public static string RuntimeDll { get; set; }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            App GameXSelector = new App();

            if (GameXSelector.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(RuntimeDll))
            {
                if (!File.Exists($"{Directory.GetCurrentDirectory()}/{RuntimeDll}"))
                {
                    MessageBox.Show("The specified file wasn't found, please do not modify the contents of this application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Assembly assembly = Assembly.LoadFile($"{Directory.GetCurrentDirectory()}/{RuntimeDll}");
                Type type = assembly.GetType("GameX.App");
                DevExpress.XtraEditors.XtraForm GUI = (DevExpress.XtraEditors.XtraForm) Activator.CreateInstance(type);
                Application.Run(GUI);
            }
            else
            {
                Application.Exit();
            }
        }
    }
}