﻿using System;
using System.ComponentModel;
using System.Diagnostics;

namespace GameX.Helpers
{
    public static class Processes
    {
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
            foreach (ProcessModule Module in pProcess.Modules)
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