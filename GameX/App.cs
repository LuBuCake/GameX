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
        /*App Init*/

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public Point Location;
        }

        [DllImport("user32.dll")]
        public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

        public App()
        {
            InitializeComponent();
            Application.Idle += Application_Idle;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            while (IsApplicationIdle())
            {
                Think();
            }
        }

        private bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, 0, 0, 0) == 0;
        }

        /*App Properties*/

        private KernelAccess Kernel { get; set; }     
        private bool Initialized { get; set; }
        private Process pProcess { get; set; }

        /*App Methods*/

        private bool HandleProcess()
        {
            if (pProcess == null)
            {
                pProcess = KernelHelper.GetProcessByName("re5dx9");
                Initialized = false;
            }
            else
            {
                if (!Initialized)
                {
                    if (KernelHelper.ProcessHasModule(pProcess, "steam_api.dll"))
                    {
                        pProcess.EnableRaisingEvents = true;
                        pProcess.Exited += Process_Exited;
                        Kernel = new KernelAccess(pProcess);
                        Initialized = true;
                    }
                }
            }

            return Initialized;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            pProcess.Dispose();
            pProcess = null;
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
