using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GameX.Helpers
{
    public static class KernelHelper
    {
        public enum MEMORY_ACCESSES
        {
            [Description("Grants all permissions.")]
            PROCESS_ALL_ACCESS = 0x1F0FFF,

            [Description("Required to create a process.")]
            PROCESS_CREATE_PROCESS = 0x0080,

            [Description("Required to create a thread.")]
            PROCESS_CREATE_THREAD = 0x0002,

            [Description("Required to suspend or resume a process.")]
            PROCESS_SUSPEND_RESUME = 0x0800,

            [Description("Required to terminate a process using TerminateProcess.")]
            PROCESS_TERMINATE = 0x0001,

            [Description("Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).")]
            PROCESS_VM_OPERATION = 0x0008,

            [Description("Required to read memory in a process using ReadProcessMemory.")]
            PROCESS_VM_READ = 0x0010,

            [Description("Required to write to memory in a process using WriteProcessMemory.")]
            PROCESS_VM_WRITE = 0x0020,

            [Description("Required to wait for the process to terminate using the wait functions. (LONG)")]
            SYNCHRONIZE = 0x00100000
        }

        public static bool IsAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static Process GetProcessByName(string ProcessName)
        {
            int PID = GetProcessIDByName(ProcessName);

            if (PID != 0)
                return Process.GetProcessById(PID);

            return null;
        }

        public static int GetProcessIDByName(string ProcessName)
        {
            Process[] Processes = Process.GetProcesses();

            if (ProcessName.ToLower().Contains(".exe"))
                ProcessName = ProcessName.Replace(".exe", "");
            if (ProcessName.ToLower().Contains(".bin"))
                ProcessName = ProcessName.Replace(".bin", "");

            foreach (Process Process in Processes)
            {
                if (Process.ProcessName.Equals(ProcessName, StringComparison.CurrentCultureIgnoreCase))
                    return Process.Id;
            }

            return 0;
        }

        public static bool ProcessHasModule(Process pProcess, string ModuleName)
        {
            foreach(ProcessModule Module in pProcess.Modules)
            {
                if (Module.ModuleName.Equals(ModuleName))
                    return true;
            }

            return false;
        }

        public static ProcessModule GetProcessModule(Process pProcess, string ModuleName)
        {
            foreach (ProcessModule Module in pProcess.Modules)
            {
                if (Module.ModuleName.Equals(ModuleName))
                    return Module;
            }

            return null;
        }

        public static IntPtr GetBaseAddressFromModule(Process pProcess, string ModuleName)
        {
            ProcessModule Module = GetProcessModule(pProcess, ModuleName);

            if (Module != null)
                return Module.BaseAddress;
            else
                return IntPtr.Zero;
        }
    }
}
