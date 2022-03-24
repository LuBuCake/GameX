﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GameX.Base.Helpers;
using GameX.Base.Types;

namespace GameX.Base.Modules
{
    public static class Memory
    {
        #region Imports

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr pHandle);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr pHandle, int lpBaseAddress, int dwSize, int flNewProtect, out int lpflOldProtect);

        [DllImport("kernel32.dll")]
        public static extern int VirtualAllocEx(IntPtr pHandle, int lpBaseAddress, int dwSize, int flAllocType, int flProtect);

        [DllImport("kernel32.dll")]
        public static extern int VirtualFreeEx(IntPtr pHandle, int lpBaseAddress, int dwSize, int dwFreeType);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr pHandle, int lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr pHandle, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32")]
        public static extern bool IsWow64Process(IntPtr hProcess, out bool lpSystemInfo);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region Props

        public static bool ModuleStarted { get; set; }
        public static bool DebugMode { get; private set; }
        private static Process _Process { get; set; }
        private static IntPtr _Handle { get; set; }

        #endregion

        #region Module

        public static void StartModule(Process Target, MEMORY_ACCESS AccessLevel = MEMORY_ACCESS.PROCESS_ALL_ACCESS)
        {
            _Process = Target;
            _Handle = OpenProcess((int)AccessLevel, false, _Process.Id);
            EnterDebugMode();
            SetForegroundWindow(_Process.MainWindowHandle);
            ModuleStarted = true;
            Terminal.WriteLine("[Memory] Module started successfully.");
        }

        public static void FinishModule()
        {
            ExitDebugMode();
            CloseHandle(_Handle);
            _Process?.Dispose();
            _Process = null;
            _Handle = IntPtr.Zero;
            SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
            ModuleStarted = false;
            Terminal.WriteLine("[Memory] Module finished successfully.");
        }

        #endregion

        #region Read / Write

        public static byte[] ReadRawAddress(int Address, int Size = 4)
        {
            byte[] buffer = new byte[Size];
            int bytesread = 0;

            ReadProcessMemory(_Handle, Address, buffer, buffer.Length, ref bytesread);

            return buffer;
        }

        public static bool WriteRawAddress(int Address, byte[] Value)
        {
            int byteswritten = 0;
            bool write = WriteProcessMemory(_Handle, Address, Value, Value.Length, ref byteswritten);

            return write && byteswritten > 0;
        }

        public static int ReadPointer(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int PointerResult = BaseAddress;

            if (ModuleName != "")
            {
                IntPtr ModuleBaseAddress = Processes.GetBaseAddressFromModule(_Process, ModuleName);

                if (ModuleBaseAddress != IntPtr.Zero)
                    PointerResult += ModuleBaseAddress.ToInt32();
            }

            foreach (int Offset in Offsets)
            {
                byte[] buffer = ReadRawAddress(PointerResult);
                PointerResult = BitConverter.ToInt32(buffer, 0);
                PointerResult += Offset;
            }

            return PointerResult;
        }

        public static T Read<T>(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            return MarshalType<T>.ByteArrayToObject(ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), MarshalType<T>.Size));
        }

        public static byte[] ReadBytes(int NumOfBytes, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            return ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), NumOfBytes);
        }

        public static void Write<T>(T Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, MarshalType<T>.ObjectToByteArray(Value));
        }

        public static void WriteBytes(byte[] Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, Value);
        }

        #endregion

        #region MEMORY MANAGMENT (REQUIRES DEBUGMODE)

        private static Dictionary<string, Detour> Detours { get; set; }

        public static void EnterDebugMode()
        {
            try
            {
                Process.EnterDebugMode();
            }
            catch (Win32Exception)
            {
                Terminal.WriteLine("[Memory] Failed entering debug mode.");
                DebugMode = false;
                return;
            }

            DebugMode = true;

            if (Detours == null)
                Detours = new Dictionary<string, Detour>();

            Terminal.WriteLine("[Memory] Entered debug mode.");
        }

        public static void ExitDebugMode()
        {
            if (!DebugMode)
                return;

            RemoveDetours();
            Process.LeaveDebugMode();
            DebugMode = false;

            Terminal.WriteLine("[Memory] Exited debug mode.");
        }

        public static int ChangeProtection(int lpBaseAddress, int dwSize, int flNewProtect)
        {
            bool Changed = VirtualProtectEx(_Handle, lpBaseAddress, dwSize, flNewProtect, out int lpflOldProtect);

            if (Changed)
                Terminal.WriteLine($"[Memory] Protection successfully changed at {lpBaseAddress:X}.");
            else
                Terminal.WriteLine($"[Memory] Protection change failed at {lpBaseAddress:X}.");

            return lpflOldProtect;
        }

        public static void RemoveDetours()
        {
            if (Detours == null)
                return;

            if (!_Process.HasExited)
            {
                List<string> DetourNames = new List<string>();

                foreach (KeyValuePair<string, Detour> detour in Detours)
                {
                    DetourNames.Add(detour.Key);
                }

                foreach (string detour in DetourNames)
                {
                    RemoveDetour(detour);
                }
            }

            Detours.Clear();
            Detours = null;
        }

        public static byte[] DetourJump(int JumpAddress, int LandAddress, int JumpInstructionLength, int JumpSize = 5, bool AddressOnly = false)
        {
            byte[] JumpInstruction = new byte[JumpInstructionLength];
            JumpInstruction[0] = 0xE9;
            byte[] Address = BitConverter.GetBytes(LandAddress - JumpAddress - JumpSize);
            Address.CopyTo(JumpInstruction, 1);

            if (AddressOnly)
                return Address;

            if (JumpInstructionLength <= 5)
                return JumpInstruction;

            for (int i = 5; i < JumpInstructionLength; i++)
                JumpInstruction[i] = 0x90;

            return JumpInstruction;
        }

        public static Detour GetDetour(string DetourName)
        {
            if (!DetourActive(DetourName))
                return null;

            Detours.TryGetValue(DetourName, out Detour Detour);
            return Detour;
        }

        public static bool DetourActive(string DetourName)
        {
            return Detours != null && Detours.TryGetValue(DetourName, out _);
        }

        public static Detour CreateDetour(string DetourName, byte[] DetourContent, int CallAddress, byte[] CallInstruction, bool JumpBack = false, int JumpBackAddress = 0)
        {
            if (!DebugMode)
                return null;

            if (DetourActive(DetourName))
                return GetDetour(DetourName);

            Terminal.WriteLine($"[Memory] Patching {CallAddress:X} for {DetourName}.");

            int DetourAddress = VirtualAllocEx(_Handle, 0, DetourContent.Length, (int) MEMORY_INFORMATION.MEM_COMMIT | (int) MEMORY_INFORMATION.MEM_RESERVE, (int) MEMORY_PROTECTION.PAGE_EXECUTE_READ);

            if (DetourAddress == 0)
            {
                Terminal.WriteLine($"[Memory] WARNING: Memory allocation failed for {DetourName}, skipping.");
                return null;
            }

            Terminal.WriteLine($"[Memory] {DetourName} applyed! Memory allocated at {DetourAddress:X}.");

            WriteRawAddress(CallAddress, DetourJump(CallAddress, DetourAddress, CallInstruction.Length));
            WriteRawAddress(DetourAddress, DetourContent);

            if (JumpBack && JumpBackAddress != 0)
                WriteRawAddress(DetourAddress + DetourContent.Length, DetourJump(DetourAddress + DetourContent.Length, JumpBackAddress, 5));

            Detour Detour = new Detour(DetourName, DetourAddress, CallAddress, CallInstruction, DetourContent, JumpBack, JumpBackAddress);
            Detours.Add(DetourName, Detour);

            return Detour;
        }

        public static bool RemoveDetour(string DetourName)
        {
            if (!DebugMode)
                return false;

            if (!DetourActive(DetourName))
                return false;

            Detour Detour = GetDetour(DetourName);
            WriteRawAddress(Detour.CallAddress(), Detour.CallInstruction());
            VirtualFreeEx(_Handle, Detour.Address(), 0, (int) MEMORY_INFORMATION.MEM_RELEASE);
            Detours.Remove(DetourName);
            Terminal.WriteLine($"[Memory] {DetourName} removed sucessfully.");
            return true;
        }

        #endregion
    }
}