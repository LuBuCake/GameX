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
        private string TargetVersion = "";
        private string[] TargetModulesCheck = { };

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
                        if (Module != "" && !MemoryHelper.ProcessHasModule(pProcess, Module))
                            return false;
                    }
                }
            }
            catch (Exception)
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

        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {
            if (Initialized)
            {
                /*
                byte[] AllocContent = { 0x56, 0x8B, 0xF1, 0x80, 0xBE, 0x74, 0x26, 0x00, 0x00, 0x01, 0x75, 0x21, 0x8B, 0x86, 0xA8, 0x2E, 0x00, 0x00, 0x85, 0xC0, 0x74, 0x17, 0x8B, 0x8E, 0x84, 0x26, 0x00, 0x00, 0x6A, 0x02, 0x51, 0x8B, 0x0D, 0x6C, 0x2D, 0x1A, 0x01, 0x6A, 0x00, 0x50, 0xE8, 0x03, 0x9A, 0xC3, 0xFF, 0xC6, 0x86, 0x36, 0x1E, 0x00, 0x00, 0x00, 0x66, 0x0F, 0xEF, 0xC0, 0x66, 0x0F, 0xD6, 0x86, 0x74, 0x26, 0x00, 0x00, 0x66, 0x0F, 0xD6, 0x86, 0x7C, 0x26, 0x00, 0x00, 0x66, 0x0F, 0xD6, 0x86, 0x84, 0x26, 0x00, 0x00, 0xC7, 0x86, 0x8C, 0x26, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5E, 0xC3 };
                byte[] CallInstruction = { 0x56, 0x8B, 0xF1, 0x80, 0xBE, 0x74, 0x26, 0x00, 0x00, 0x01 };

                if (toggleSwitch1.IsOn)
                {
                    Kernel.AllocMemory("Alloc Test", AllocContent, 0x00B69470, CallInstruction);
                }
                else
                {
                    Kernel.DeallocMemory("Alloc Test");
                }
                */

                byte[] AllocContent = { 0x56, 0x8B, 0xF1, 0x80, 0xBE, 0x74, 0x26, 0x00, 0x00, 0x01 };
                byte[] CallInstruction = { 0x56, 0x8B, 0xF1, 0x80, 0xBE, 0x74, 0x26, 0x00, 0x00, 0x01 };

                if (toggleSwitch1.IsOn)
                {
                    Kernel.AllocMemory("Alloc Test", AllocContent, 0x00B69470, CallInstruction, true, 0x00B6947A);
                }
                else
                {
                    Kernel.DeallocMemory("Alloc Test");
                }
            }
        }
    }
}
