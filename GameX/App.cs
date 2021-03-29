using GameX.Helpers;
using GameX.Modules;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GameX
{
    public partial class App : DevExpress.XtraEditors.XtraForm
    {
        /*App Properties*/

        private MessagePeaker Peaker { get; set; }
        private Memory Kernel { get; set; }
        private Process pProcess { get; set; }
        private bool Initialized { get; set; }

        private string TargetProcess = "re5dx9";
        private string TargetVersion = "1.0.0.129";
        private string[] TargetModulesCheck = { "maluc.dll", "steam_api.dll" };

        /*App Init*/

        public App()
        {
            InitializeComponent();

            Peaker = new MessagePeaker();

            Application.Idle += Application_Idle;
            Application.ApplicationExit += Application_ApplicationExit;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            while (Peaker.IsApplicationIdle())
            {
                Think();
            }
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            Application.Idle += null;

            if (Kernel != null)
                Kernel.Dispose();
        }

        /*App Methods*/

        private bool HandleProcess()
        {
            if (pProcess == null)
            {
                pProcess = MemoryHelper.GetProcessByName(TargetProcess);
                Initialized = false;

                Text = "GameX - Waiting";
            }
            else
            {
                if (!Initialized)
                {
                    if (ValidateTarget(pProcess))
                    {
                        pProcess.EnableRaisingEvents = true;
                        pProcess.Exited += ClearRuntime;
                        Kernel = new Memory(pProcess);
                        Initialized = true;

                        Text = "GameX - Running";
                    }
                    else
                    {
                        pProcess.Dispose();
                        pProcess = null;

                        Text = "GameX - Incompatible Version";
                    }
                }
            }

            return Initialized;
        }

        private bool ValidateTarget(Process pProcess)
        {
            try
            {
                if (TargetVersion != "" && !pProcess.MainModule.FileVersionInfo.ToString().Contains(TargetVersion))
                    return false;

                if (TargetModulesCheck.Length > 0)
                {
                    foreach (string Module in TargetModulesCheck)
                    {
                        if (!MemoryHelper.ProcessHasModule(pProcess, Module))
                            return false;
                    }
                }
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        private void ClearRuntime(object sender, EventArgs e)
        {
            Kernel.Dispose();
            Kernel = null;
            pProcess.Dispose();
            pProcess = null;
        }

        private void Think()
        {
            if (HandleProcess())
            {
                
            }
        }

    }
}
