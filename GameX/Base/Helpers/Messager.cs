using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GameX.Base.Helpers
{
    public class Messager
    {
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

        public bool IsApplicationIdle()
        {
            NativeMessage result;
            return PeekMessage(out result, IntPtr.Zero, 0, 0, 0) == 0;
        }
    }
}
