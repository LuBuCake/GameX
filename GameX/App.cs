using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;
using GameX.Modules;
using GameX.Helpers;
using System.Diagnostics;

namespace GameX
{
    public partial class App : DevExpress.XtraEditors.XtraForm
    {
        /*App Properties*/

        private bool Initialized { get; set; }
        private KernelAccess Kernel { get; set; }
        private Process pProcess { get; set; }
        private MessagePeaker Peaker { get; set; }

        private string TargetProcess = "";
        private string TargetVersion = "";
        private string[] TargetModules = { };

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
        }

        /*App Methods*/

        private bool HandleProcess()
        {
            if (pProcess == null)
            {
                pProcess = KernelHelper.GetProcessByName(TargetProcess);
                Initialized = false;
            }
            else
            {
                if (!Initialized)
                {
                    if (ValidateTarget(pProcess))
                    {
                        pProcess.EnableRaisingEvents = true;
                        pProcess.Exited += ClearAndRestore;
                        Kernel = new KernelAccess(pProcess);
                        Initialized = true;
                    }
                }
            }

            return Initialized;
        }

        private bool ValidateTarget(Process pProcess)
        {
            if (TargetVersion != "" && !pProcess.MainModule.FileVersionInfo.ToString().Contains(TargetVersion))
                return false;

            if (TargetModules.Length > 0)
            {
                foreach (string Module in TargetModules)
                {
                    if (!KernelHelper.ProcessHasModule(pProcess, Module))
                        return false;
                }
            }

            return true;
        }

        private void ClearAndRestore(object sender, EventArgs e)
        {
            pProcess.Dispose();
            pProcess = null;
            Kernel.Dispose();
            Kernel = null;
        }

        private void Think()
        {
            if (HandleProcess())
            {

            }
        }

    }
}
