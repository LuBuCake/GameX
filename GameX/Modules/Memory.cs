using GameX.Helpers;
using GameX.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameX.Modules
{
    public class Memory
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr pHandle);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtectEx(IntPtr pHandle, int lpBaseAddress, int dwSize, int flNewProtect, out int lpflOldProtect);

        [DllImport("kernel32.dll")]
        private static extern int VirtualAllocEx(IntPtr pHandle, int lpBaseAddress, int dwSize, int flAllocType, int flProtect);

        [DllImport("kernel32.dll")]
        private static extern int VirtualFreeEx(IntPtr pHandle, int lpBaseAddress, int dwSize, int dwFreeType);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr pHandle, int lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr pHandle, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32")]
        public static extern bool IsWow64Process(IntPtr hProcess, out bool lpSystemInfo);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public enum MEMORY_ACCESS
        {
            [Description("Grants all other accesses.")]
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

        public enum MEMORY_PROTECTION
        {
            [Description("Enables execute access to the committed region of pages.")]
            PAGE_EXECUTE = 0x10,

            [Description("Enables execute or read-only access to the committed region of pages.")]
            PAGE_EXECUTE_READ = 0x20,

            [Description("Enables execute, read-only, or read/write access to the committed region of pages.")]
            PAGE_EXECUTE_READWRITE = 0x40,

            [Description("Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object.")]
            PAGE_EXECUTE_WRITECOPY = 0x80,

            [Description("Disables all access to the committed region of pages.")]
            PAGE_NOACCESS = 0x01,

            [Description("Enables read-only access to the committed region of pages.")]
            PAGE_READONLY = 0x02,

            [Description("Enables read-only or read/write access to the committed region of pages.")]
            PAGE_READWRITE = 0x04,

            [Description("Enables read-only or copy-on-write access to a mapped view of a file mapping object.")]
            PAGE_WRITECOPY = 0x08,

            [Description("Sets all locations in the pages as invalid targets for CFG.")]
            PAGE_TARGETS_INVALID = 0x40000000,

            [Description("Pages in the region will not have their CFG information updated while the protection changes for VirtualProtect.")]
            PAGE_TARGETS_NO_UPDATE = 0x40000000
        }

        public enum MEMORY_INFORMATION
        {
            [Description("Detourates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages.")]
            MEM_COMMIT = 0x00001000,

            [Description("Indicates free pages not accessible to the calling process and available to be Detourated.")]
            MEM_FREE = 0x10000,

            [Description("Reserves a range of the process's virtual address space without Detourating any actual physical storage in memory or in the paging file on disk.")]
            MEM_RESERVE = 0x00002000,

            [Description("Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest.")]
            MEM_RESET = 0x00080000,

            [Description("MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier.")]
            MEM_RESET_UNDO = 0x1000000,

            [Description("Decommits the specified region of committed pages. After the operation, the pages are in the reserved state.")]
            MEM_DECOMMIT = 0x00004000,

            [Description("Releases the specified region of pages, or placeholder (for a placeholder, the address space is released and available for other Detours). After this operation, the pages are in the free state.")]
            MEM_RELEASE = 0x00008000
        }

        public Process pProcess { get; set; }
        public IntPtr pHandle { get; set; }
        public IntPtr pBase { get; set; }
        public bool DebugMode { get; set; }

        public Memory(Process ProcessObject, MEMORY_ACCESS AccessLevel = MEMORY_ACCESS.PROCESS_ALL_ACCESS)
        {
            pProcess = ProcessObject;
            pHandle = OpenProcess((int)AccessLevel, false, pProcess.Id);
            pBase = Processes.GetBaseAddressFromModule(pProcess, pProcess.MainModule?.ModuleName);

            EnterDebugMode();
        }

        public void Dispose()
        {
            ExitDebugMode();
            CloseHandle(pHandle);
            pProcess.Dispose();
            pProcess = null;
            pHandle = IntPtr.Zero;
            pBase = IntPtr.Zero;
        }

        /*READ WRITE*/

        public byte[] ReadRawAddress(int Address, int Size = 4)
        {
            byte[] buffer = new byte[Size];
            int bytesread = 0;

            ReadProcessMemory(pHandle, Address, buffer, buffer.Length, ref bytesread);

            return buffer;
        }

        public bool WriteRawAddress(int Address, byte[] Value)
        {
            int byteswritten = 0;
            bool write = WriteProcessMemory(pHandle, Address, Value, Value.Length, ref byteswritten);

            return write && byteswritten > 0;
        }

        public int ReadPointer(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int PointerResult = BaseAddress;

            if (ModuleName != "")
            {
                IntPtr ModuleBaseAddress = Processes.GetBaseAddressFromModule(pProcess, ModuleName);

                if (ModuleBaseAddress != IntPtr.Zero)
                    PointerResult += ModuleBaseAddress.ToInt32();
            }

            foreach (int Offset in Offsets)
            {
                var buffer = ReadRawAddress(PointerResult);
                PointerResult = BitConverter.ToInt32(buffer, 0);
                PointerResult += Offset;
            }

            return PointerResult;
        }

        public byte[] ReadBytes(int NumOfBytes, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            return ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), NumOfBytes);
        }

        public short ReadInt16(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), 2);
            return BitConverter.ToInt16(result, 0);
        }

        public int ReadInt32(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets));
            return BitConverter.ToInt32(result, 0);
        }

        public uint ReadUInt32(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets));
            return BitConverter.ToUInt32(result, 0);
        }

        public float ReadFloat(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets));
            return BitConverter.ToSingle(result, 0);
        }

        public void WriteBytes(byte[] Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, Value);
        }

        public void WriteInt16(int Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes((short)Value));
        }

        public void WriteInt32(int Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes(Value));
        }

        public void WriteUint32(uint Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes(Value));
        }

        public void WriteFloat(float Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes(Value));
        }

        /*MEMORY INJECTIONS (REQUIRES DEBUGMODE BECAUSE ITS CODE INJECTION DUH)*/

        public Dictionary<string, Detour> Detours;

        public void EnterDebugMode()
        {
            try
            {
                Process.EnterDebugMode();
            }
            catch (Win32Exception)
            {
                Terminal.WriteLine("Failed entering debug mode.");
                Terminal.WriteLine("WARNING: Code injection is only allowed in Admin Mode, expect limitations in User Mode.");
                DebugMode = false;
                return;
            }

            DebugMode = true;

            if (Detours == null)
                Detours = new Dictionary<string, Detour>();

            Terminal.WriteLine("Entered debug mode.");
        }

        public void ExitDebugMode()
        {
            if (!DebugMode)
                return;

            RemoveDetours();
            Process.LeaveDebugMode();
            DebugMode = false;

            Terminal.WriteLine("Exited debug mode.");
        }

        public int ChangeProtection(int lpBaseAddress, int dwSize, int flNewProtect)
        {
            bool Changed = VirtualProtectEx(pHandle, lpBaseAddress, dwSize, flNewProtect, out int lpflOldProtect);

            if (Changed)
                Terminal.WriteLine($"Protection changed at 0x{lpBaseAddress.ToString("X")} successfully.");
            else
                Terminal.WriteLine($"Protection change failed at 0x{lpBaseAddress.ToString("X")}.");

            return lpflOldProtect;
        }

        public void RemoveDetours()
        {
            if (Detours == null)
                return;

            if (!pProcess.HasExited)
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

        public Detour GetDetour(string DetourName)
        {
            if (!DetourActive(DetourName))
                return null;

            Detours.TryGetValue(DetourName, out Detour Detour);
            return Detour;
        }

        public bool DetourActive(string DetourName)
        {
            if (Detours == null || !Detours.TryGetValue(DetourName, out Detour Detour))
                return false;

            return true;
        }

        public Detour CreateDetour(string DetourName, byte[] DetourContent, int CallAddress, byte[] CallInstruction, bool JumpBack = false, int JumpBackAddress = 0)
        {
            if (!DebugMode)
                return null;

            if (DetourActive(DetourName))
                return GetDetour(DetourName);

            Terminal.WriteLine($"Hooking 0x{CallAddress.ToString("X")} - for {DetourName}");

            int DetourAddress = VirtualAllocEx(pHandle, 0, DetourContent.Length, (int)MEMORY_INFORMATION.MEM_COMMIT | (int)MEMORY_INFORMATION.MEM_RESERVE, (int)MEMORY_PROTECTION.PAGE_EXECUTE_READ);

            if (DetourAddress == 0)
            {
                Terminal.WriteLine($"WARNING: Memory allocation failed for {DetourName}, skipping.");
                return null;
            }

            Terminal.WriteLine($"{DetourName} hooked! Memory allocated at 0x{DetourAddress.ToString("X")}");

            WriteRawAddress(CallAddress, DetourJump(CallAddress, DetourAddress, CallInstruction.Length));
            WriteRawAddress(DetourAddress, DetourContent);

            if (JumpBack && JumpBackAddress != 0)
                WriteRawAddress(DetourAddress + DetourContent.Length, DetourJump(DetourAddress + DetourContent.Length, JumpBackAddress, 5));

            Detour Detour = new Detour(DetourName, DetourAddress, CallAddress, CallInstruction, DetourContent, JumpBack, JumpBackAddress);
            Detours.Add(DetourName, Detour);

            return Detour;
        }

        public bool RemoveDetour(string DetourName)
        {
            if (!DebugMode)
                return false;

            if (!DetourActive(DetourName))
                return false;

            Detour Detour = GetDetour(DetourName);
            WriteRawAddress(Detour.CallAddress(), Detour.CallInstruction());
            VirtualFreeEx(pHandle, Detour.Address(), 0, (int)MEMORY_INFORMATION.MEM_RELEASE);
            Detours.Remove(DetourName);
            return true;
        }
    }
}
