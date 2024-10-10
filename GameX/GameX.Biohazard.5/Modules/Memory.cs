using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GameX.Helpers;
using GameX.Enum;
using GameX.Modules.Sub;
using System.Text;
using System.IO;
using System.Threading;

namespace GameX.Modules
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
        public static extern bool ReadProcessMemory(IntPtr pHandle, int lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr pHandle, int lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern int CreateRemoteThread(IntPtr hProcess, int lpThreadAttributes, uint dwStackSize, int lpStartAddress, int lpParameter, uint dwCreationFlags, out int lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region Props

        public static bool ModuleStarted { get; set; }
        public static bool DebugMode { get; private set; }
        public static bool QOLDllInjected { get; private set; }
        public static bool InternalInjected { get; private set; }
        public static Process _Process { get; set; }
        public static IntPtr _Handle { get; set; }

        #endregion

        #region Module

        public static void StartModule(Process Target, MEMORY_ACCESS AccessLevel = MEMORY_ACCESS.PROCESS_ALL_ACCESS)
        {
            _Process = Target;
            _Handle = OpenProcess((int)AccessLevel, false, _Process.Id);
            EnterDebugMode();
            SetForegroundWindow(_Process.MainWindowHandle);
            InjectInternalLibrary();
            QOLDllInjected = ProcessHelper.ProcessHasModule(_Process, "QOL.dll");
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
            InternalInjected = false;
            QOLDllInjected = false;
            ModuleStarted = false;
            Terminal.WriteLine("[Memory] Module finished successfully.");
        }

        public static void InjectInternalLibrary()
        {
            if (ProcessHelper.ProcessHasModule(_Process, "GameX.Biohazard.5.Internal.dll"))
            {
                Terminal.WriteLine("[Memory] Internal library already injected.");
                InternalInjected = true;
            }
            else
                InternalInjected = InjectDLL($"{Directory.GetCurrentDirectory()}\\addons\\GameX.Biohazard.5\\GameX.Biohazard.5.Internal.dll");

            if (InternalInjected)
            {
                Thread.Sleep(1000);
                _Process.Refresh();
#if DEBUG
                bool InternalModuleFound = ProcessHelper.ProcessHasModule(_Process, "GameX.Biohazard.5.Internal.dll");
                Terminal.WriteLine($"[Memory][DEBUG] InternalModuleFound: {InternalModuleFound}");
#endif
            }
        }

        #endregion

        #region Read / Write

        public static byte[] ReadRawAddress(int Address, int Size = 4)
        {
            byte[] buffer = new byte[Size];
            ReadProcessMemory(_Handle, Address, buffer, buffer.Length, out _);
            return buffer;
        }

        public static bool WriteRawAddress(int Address, byte[] Value)
        {
            bool write = WriteProcessMemory(_Handle, Address, Value, Value.Length, out int byteswritten);
            return write && byteswritten > 0;
        }

        public static int ReadPointer(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int PointerResult = BaseAddress;

            if (!string.IsNullOrEmpty(ModuleName))
            {
                IntPtr ModuleBaseAddress = ProcessHelper.GetBaseAddressFromModule(_Process, ModuleName);

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

        public static bool InjectDLL(string FilePath)
        {
            int allocatedMemory = VirtualAllocEx(_Handle, 0, FilePath.Length, (int)MEMORY_INFORMATION.MEM_COMMIT, (int)MEMORY_PROTECTION.PAGE_READWRITE);
            if (allocatedMemory == 0)
            {
                Terminal.WriteLine("[Memory] WARNING: Memory allocation failed for Dll injection, aborting.");
                return false;
            }

            byte[] dllBytes = Encoding.ASCII.GetBytes(FilePath);
            WriteProcessMemory(_Handle, allocatedMemory, dllBytes, dllBytes.Length, out _);
            int lpThreadId = CreateRemoteThread(_Handle, 0, 0, GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA"), allocatedMemory, 0, out _);

            if (lpThreadId == 0)
            {
                Terminal.WriteLine("[Memory] WARNING: Remote thread creation failed for Dll injection, aborting.");
                return false;
            }

            VirtualFreeEx(_Handle, allocatedMemory, dllBytes.Length, (int)MEMORY_INFORMATION.MEM_RELEASE);

            Terminal.WriteLine($"[Memory] Dll \"{FilePath}\" injected successfully.");
            return true;
        }

        public static bool EjectDll(string DllName)
        {
            IntPtr ModuleAddress = ProcessHelper.GetBaseAddressFromModule(_Process, DllName);
            int freeLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "FreeLibrary");

            int hThread = CreateRemoteThread(_Handle, 0, 0, freeLibraryAddr, (int)ModuleAddress, 0, out _);
            if (hThread == 0)
            {
                Terminal.WriteLine("[Memory] WARNING: Error calling remote thread for Dll ejection, aborting.");
                return false;
            }

            return true;
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

            int DetourAddress = VirtualAllocEx(_Handle, 0, DetourContent.Length, (int)MEMORY_INFORMATION.MEM_COMMIT | (int)MEMORY_INFORMATION.MEM_RESERVE, (int)MEMORY_PROTECTION.PAGE_EXECUTE_READ);

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
            VirtualFreeEx(_Handle, Detour.Address(), 0, (int)MEMORY_INFORMATION.MEM_RELEASE);
            Detours.Remove(DetourName);
            Terminal.WriteLine($"[Memory] {DetourName} removed sucessfully.");
            return true;
        }

        #endregion
    }
}