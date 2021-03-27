using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameX.Helpers;

namespace GameX.Modules
{
    public class KernelAccess
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr pHandle, int lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr pHandle, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public Process pProcess;

        public IntPtr pHandle;

        public IntPtr pBase;

        public KernelAccess(Process ProcessObject)
        {
            pProcess = ProcessObject;
            pHandle = OpenProcess(0x1F0FFF, false, pProcess.Id);
            pBase = KernelHelper.GetBaseAddressFromModule(pProcess, pProcess.MainModule.ModuleName);

            try
            {
                Process.EnterDebugMode();
            }
            catch(Win32Exception)
            {
                MessageBox.Show("You must run this program with raised privileges in order to use DebugMode!");
            }
        }

        public void WriteRawAddress(int Address, byte[] bufffer)
        {
            int byteswritten = 0;
            WriteProcessMemory(pHandle, Address, bufffer, bufffer.Length, ref byteswritten);
        }

        public byte[] ReadRawAddress(int Address, int Size = 4)
        {
            byte[] buffer = new byte[Size];
            int bytesread = 0;

            ReadProcessMemory(pHandle, Address, buffer, buffer.Length, ref bytesread);

            return buffer;
        }

        public int ReadPointer(string ModuleName, int BaseAddress, params int[] Offsets)
        {
            byte[] buffer;
            int PointerResult = BaseAddress;

            if (ModuleName != "")
            {
                IntPtr ModuleBaseAddress = KernelHelper.GetBaseAddressFromModule(pProcess, ModuleName);

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
    }
}
