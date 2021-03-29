using GameX.Helpers;
using GameX.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        public Process pProcess { get; set; }
        public IntPtr pHandle { get; set; }
        public IntPtr pBase { get; set; }
        public bool DebugMode { get; set; }

        public Memory(Process ProcessObject, MemoryHelper.MEMORY_ACCESS AccessLevel = MemoryHelper.MEMORY_ACCESS.PROCESS_ALL_ACCESS)
        {
            pProcess = ProcessObject;
            pHandle = OpenProcess((int)AccessLevel, false, pProcess.Id);
            pBase = MemoryHelper.GetBaseAddressFromModule(pProcess, pProcess.MainModule.ModuleName);

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
            byte[] buffer;
            int PointerResult = BaseAddress;

            if (ModuleName != "")
            {
                IntPtr ModuleBaseAddress = MemoryHelper.GetBaseAddressFromModule(pProcess, ModuleName);

                if (ModuleBaseAddress != IntPtr.Zero)
                    PointerResult += ModuleBaseAddress.ToInt32();
            }

            for (int i = 0; i < Offsets.Length; i++)
            {
                buffer = ReadRawAddress(PointerResult);
                PointerResult = BitConverter.ToInt32(buffer, 0);
                PointerResult += Offsets[i];
            }

            return PointerResult;
        }

        public byte[] ReadBytes(int NumOfBytes, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            return ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), NumOfBytes);
        }

        public byte ReadInt8(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            return ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), 1)[0];
        }
        public short ReadInt16(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), 2);
            return BitConverter.ToInt16(result, 0);
        }

        public int ReadInt32(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), 4);
            return BitConverter.ToInt32(result, 0);
        }

        public uint ReadUInt32(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), 4);
            return BitConverter.ToUInt32(result, 0);
        }

        public float ReadFloat(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] result = ReadRawAddress(ReadPointer(ModuleName, BaseAddress, Offsets), 4);
            return BitConverter.ToSingle(result, 0);
        }

        public void WriteBytes(byte[] Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, Value);
        }

        public void WriteInt8(int Value, string ModuleName, int BaseAddress, params int[] Offsets)
        {
            int Address = ReadPointer(ModuleName, BaseAddress, Offsets);
            WriteRawAddress(Address, BitConverter.GetBytes((byte)Value));
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

        /*MEMORY DETOUR (REQUIRES DEBUGMODE BECAUSE ITS CODE INJECTION)*/

        public Dictionary<string, Detour> Detours;

        public void EnterDebugMode()
        {
            try
            {
                Process.EnterDebugMode();
            }
            catch (Win32Exception)
            {
                MessageBox.Show("Remember to run this program with raised privileges if you want to use code injection!", "Whops!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DebugMode = false;
                return;
            }

            DebugMode = true;

            if (Detours == null)
                Detours = new Dictionary<string, Detour>();
        }

        public void ExitDebugMode()
        {
            if (DebugMode == true)
            {
                Process.LeaveDebugMode();
                DebugMode = false;
            }

            RemoveDetours();
        }

        public void RemoveDetours()
        {
            if (Detours == null)
                return;

            if (!pProcess.HasExited)
            {
                foreach (KeyValuePair<string, Detour> Detour in Detours)
                {
                    if (DetourActive(Detour.Key))
                        RemoveDetour(Detour.Key);
                }
            }

            Detours.Clear();
            Detours = null;
        }

        private byte[] DetourJump(int JumpAddress, int LandAddress, int JumpInstructionLength)
        {
            byte[] JumpInstruction = new byte[JumpInstructionLength];
            JumpInstruction[0] = 0xE9;
            int JumpLength = LandAddress - JumpAddress - 5;
            byte[] JumpLengthValue = BitConverter.GetBytes(JumpLength);
            JumpLengthValue.CopyTo(JumpInstruction, 1);

            if (JumpInstructionLength > 5)
                for (int i = 5; i < JumpInstructionLength; i++)
                    JumpInstruction[i] = 0x90;

            return JumpInstruction;
        }

        public Detour GetDetour(string DetourName)
        {
            if (DetourActive(DetourName))
            {
                Detours.TryGetValue(DetourName, out Detour Detour);
                return Detour;
            }

            return null;
        }

        public bool DetourActive(string DetourName)
        {
            if (Detours != null && Detours.TryGetValue(DetourName, out Detour Detour))
            {
                byte[] RegionReading = ReadRawAddress(Detour.Address(), Detour.Size());

                if (MemoryHelper.CompareByteArray(RegionReading, Detour.Content(), Detour.Size()))
                    return true;

                Detours.Remove(DetourName);
            }

            return false;
        }

        public int CreateDetour(string DetourName, byte[] DetourContent, int CallAddress, byte[] CallInstruction, bool JumpBack = false, int JumpBackAddress = 0)
        {
            if (!DebugMode)
                return 0;

            if (DetourActive(DetourName))
            {
                return GetDetour(DetourName).Address();
            }

            int DetourAddress = VirtualAllocEx(pHandle, 0, DetourContent.Length, (int)MemoryHelper.MEMORY_INFORMATION.MEM_COMMIT | (int)MemoryHelper.MEMORY_INFORMATION.MEM_RESERVE, (int)MemoryHelper.MEMORY_PROTECTION.PAGE_EXECUTE_READ);

            if (DetourAddress != 0)
            {
                WriteRawAddress(CallAddress, DetourJump(CallAddress, DetourAddress, CallInstruction.Length));
                WriteRawAddress(DetourAddress, DetourContent);

                if (JumpBack && JumpBackAddress != 0)
                {
                    WriteRawAddress(DetourAddress + DetourContent.Length, DetourJump(DetourAddress + DetourContent.Length, JumpBackAddress, 5));
                }

                Detour Detour = new Detour(DetourName, DetourAddress, CallAddress, CallInstruction, DetourContent, JumpBack);
                Detours.Add(DetourName, Detour);

                return DetourAddress;
            }

            return 0;
        }

        public bool RemoveDetour(string DetourName)
        {
            if (!DebugMode)
                return false;

            if (DetourActive(DetourName))
            {
                Detour Detour = GetDetour(DetourName);
                WriteRawAddress(Detour.CallAddress(), Detour.CallInstruction());
                VirtualFreeEx(pHandle, Detour.Address(), 0, (int)MemoryHelper.MEMORY_INFORMATION.MEM_RELEASE);
                Detours.Remove(DetourName);
                return true;
            }

            return false;
        }
    }
}
