using System;
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
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr pHandle);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr pHandle, long lpBaseAddress, int dwSize, int flNewProtect, out int lpflOldProtect);

        [DllImport("kernel32.dll")]
        public static extern long VirtualAllocEx(IntPtr pHandle, long lpBaseAddress, int dwSize, int flAllocType, int flProtect);

        [DllImport("kernel32.dll")]
        public static extern long VirtualFreeEx(IntPtr pHandle, long lpBaseAddress, int dwSize, int dwFreeType);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr pHandle, long lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr pHandle, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32")]
        public static extern bool IsWow64Process(IntPtr hProcess, out bool lpSystemInfo);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static bool ModuleStarted { get; set; }
        public static Process Target_Process { get; set; }
        private static IntPtr Target_Handle { get; set; }
        public static bool DebugMode { get; private set; }

        public static void StartModule(Process Target, Enums.MEMORY_ACCESS AccessLevel = Enums.MEMORY_ACCESS.PROCESS_ALL_ACCESS)
        {
            Target_Process = Target;
            Target_Handle = OpenProcess((int) AccessLevel, false, Target_Process.Id);
            EnterDebugMode();
            ModuleStarted = true;
            Terminal.WriteLine("[Memory] Module started successfully.");
        }

        public static void FinishModule()
        {
            ExitDebugMode();
            CloseHandle(Target_Handle);
            Target_Process?.Dispose();
            Target_Process = null;
            Target_Handle = IntPtr.Zero;
            ModuleStarted = false;
            Terminal.WriteLine("[Memory] Module finished successfully.");
        }

        /*READ WRITE*/

        public static byte[] ReadRawAddress(long Address, int Size = 8)
        {
            byte[] buffer = new byte[Size];
            int bytesread = 0;

            ReadProcessMemory(Target_Handle, Address, buffer, buffer.Length, ref bytesread);

            return buffer;
        }

        public static bool WriteRawAddress(long Address, byte[] Value)
        {
            int byteswritten = 0;
            bool write = WriteProcessMemory(Target_Handle, Address, Value, Value.Length, ref byteswritten);

            return write && byteswritten > 0;
        }

        public static long ReadPointer(string ModuleName, long BaseAddress, params int[] Offsets)
        {
            long PointerResult = BaseAddress;

            if (ModuleName != "")
            {
                IntPtr ModuleBaseAddress = Processes.GetBaseAddressFromModule(Target_Process, ModuleName);

                if (ModuleBaseAddress != IntPtr.Zero)
                    PointerResult += Convert.ToInt64(ModuleBaseAddress.ToInt64());
            }

            foreach (int Offset in Offsets)
            {
                byte[] buffer = ReadRawAddress(PointerResult);
                PointerResult = BitConverter.ToInt64(buffer, 0);
                PointerResult += Offset;
            }

            return PointerResult;
        }

        public static byte[] ReadBytes(int NumOfBytes, string ModuleName, long BaseAddress, params int[] Offsets)
        {
            return ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), NumOfBytes);
        }

        public static short ReadInt16(string ModuleName, long BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), 2);
            return BitConverter.ToInt16(result, 0);
        }

        public static int ReadInt32(string ModuleName, long BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets));
            return BitConverter.ToInt32(result, 0);
        }

        public static uint ReadUInt32(string ModuleName, long BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets));
            return BitConverter.ToUInt32(result, 0);
        }

        public static float ReadFloat(string ModuleName, long BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets));
            return BitConverter.ToSingle(result, 0);
        }

        public static void WriteBytes(byte[] Value, string ModuleName, long BaseAddress, params int[] Offsets)
        {
            long Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, Value);
        }

        public static void WriteInt16(int Value, string ModuleName, long BaseAddress, params int[] Offsets)
        {
            long Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes((short) Value));
        }

        public static void WriteInt32(int Value, string ModuleName, long BaseAddress, params int[] Offsets)
        {
            long Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes(Value));
        }

        public static void WriteUint32(uint Value, string ModuleName, long BaseAddress, params int[] Offsets)
        {
            long Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes(Value));
        }

        public static void WriteFloat(float Value, string ModuleName, long BaseAddress, params int[] Offsets)
        {
            long Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes(Value));
        }

        /*MEMORY INJECTION (REQUIRES DEBUGMODE)*/

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
                Terminal.WriteLine("[Memory] WARNING: Code injection is only allowed in Admin Mode, expect limitations in User Mode.");
                Utility.MessageBox_Warning("Code injection is only allowed in Admin Mode, expect limitations in User Mode.");
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

        public static int ChangeProtection(long lpBaseAddress, int dwSize, int flNewProtect)
        {
            bool Changed = VirtualProtectEx(Target_Handle, lpBaseAddress, dwSize, flNewProtect, out int lpflOldProtect);

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

            if (!Target_Process.HasExited)
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

        public static byte[] DetourJump(long JumpAddress, long LandAddress, int JumpInstructionLength, int JumpSize = 5, bool AddressOnly = false)
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

        public static Detour CreateDetour(string DetourName, byte[] DetourContent, long CallAddress, byte[] CallInstruction, bool JumpBack = false, long JumpBackAddress = 0)
        {
            if (!DebugMode)
                return null;

            if (DetourActive(DetourName))
                return GetDetour(DetourName);

            Terminal.WriteLine($"[Memory] Patching {CallAddress:X} for {DetourName}.");

            long DetourAddress = VirtualAllocEx(Target_Handle, 0, DetourContent.Length, (int) Enums.MEMORY_INFORMATION.MEM_COMMIT | (int) Enums.MEMORY_INFORMATION.MEM_RESERVE, (int) Enums.MEMORY_PROTECTION.PAGE_EXECUTE_READ);

            if (DetourAddress == 0)
            {
                Terminal.WriteLine($"[Memory] WARNING: Memory allocation failed for {DetourName}, skipping.");
                return null;
            }

            Terminal.WriteLine($"[Memory] {DetourName} patched! Memory allocated at {DetourAddress:X}.");

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
            VirtualFreeEx(Target_Handle, Detour.Address(), 0, (int) Enums.MEMORY_INFORMATION.MEM_RELEASE);
            Detours.Remove(DetourName);
            return true;
        }
    }
}