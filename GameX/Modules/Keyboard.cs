using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameX.Modules
{
    public class Keyboard
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookCallback lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public enum VK
        {
            LBUTTON = 0x01,     // Left mouse button
            RBUTTON = 0x02,     // Right mouse button
            CANCEL = 0x03,      // Control-break processing
            MBUTTON = 0x04,     // Middle mouse button (for three-button mouse only)
            BACK = 0x08,        // BACKSPACE key
            TAB = 0x09,         // TAB key
            CLEAR = 0x0C,       // CLEAR key
            RETURN = 0x0D,      // ENTER key
            SHIFT = 0x10,       // SHIFT key
            CONTROL = 0x11,     // CTRL key
            MENU = 0x12,        // ALT key
            PAUSE = 0x13,       // PAUSE key
            CAPS = 0x14,        // CAPS LOCK key
            ESCAPE = 0x1B,      // ESC key
            SPACE = 0x20,       // SPACEBAR
            PAGEUP = 0x21,      // PAGE UP key
            PAGEDOWN = 0x22,    // PAGE DOWN key
            END = 0x23,         // END key
            HOME = 0x24,        // HOME key
            LEFT = 0x25,        // LEFT ARROW key
            UP = 0x26,          // UP ARROW key
            RIGHT = 0x27,       // RIGHT ARROW key
            DOWN = 0x28,        // DOWN ARROW key
            INSERT = 0x2D,      // INS key
            DELETE = 0x2E,      // DEL key
            KEY_0 = 0x30,       // 0 key
            KEY_1 = 0x31,       // 1 key
            KEY_2 = 0x32,       // 2 key
            KEY_3 = 0x33,       // 3 key
            KEY_4 = 0x34,       // 4 key
            KEY_5 = 0x35,       // 5 key
            KEY_6 = 0x36,       // 6 key
            KEY_7 = 0x37,       // 7 key
            KEY_8 = 0x38,       // 8 key
            KEY_9 = 0x39,       // 9 key
            KEY_A = 0x41,       // A key
            KEY_B = 0x42,       // B key
            KEY_C = 0x43,       // C key
            KEY_D = 0x44,       // D key
            KEY_E = 0x45,       // E key
            KEY_F = 0x46,       // F key
            KEY_G = 0x47,       // G key
            KEY_H = 0x48,       // H key
            KEY_I = 0x49,       // I key
            KEY_J = 0x4A,       // J key
            KEY_K = 0x4B,       // K key
            KEY_L = 0x4C,       // L key
            KEY_M = 0x4D,       // M key
            KEY_N = 0x4E,       // N key
            KEY_O = 0x4F,       // O key
            KEY_P = 0x50,       // P key
            KEY_Q = 0x51,       // Q key
            KEY_R = 0x52,       // R key
            KEY_S = 0x53,       // S key
            KEY_T = 0x54,       // T key
            KEY_U = 0x55,       // U key
            KEY_V = 0x56,       // V key
            KEY_W = 0x57,       // W key
            KEY_X = 0x58,       // X key
            KEY_Y = 0x59,       // Y key
            KEY_Z = 0x5A,       // Z key
            LWIN = 0x5B,        // Left Windows key (Microsoft Natural keyboard)
            RWIN = 0x5C,        // Right Windows key (Natural keyboard)
            NUMPAD0 = 0x60,     // Numeric keypad 0 key
            NUMPAD1 = 0x61,     // Numeric keypad 1 key
            NUMPAD2 = 0x62,     // Numeric keypad 2 key
            NUMPAD3 = 0x63,     // Numeric keypad 3 key
            NUMPAD4 = 0x64,     // Numeric keypad 4 key
            NUMPAD5 = 0x65,     // Numeric keypad 5 key
            NUMPAD6 = 0x66,     // Numeric keypad 6 key
            NUMPAD7 = 0x67,     // Numeric keypad 7 key
            NUMPAD8 = 0x68,     // Numeric keypad 8 key
            NUMPAD9 = 0x69,     // Numeric keypad 9 key
            MULTIPLY = 0x6A,    // Multiply key
            ADD = 0x6B,         // Add key
            SEPARATOR = 0x6C,   // Separator key
            SUBTRACT = 0x6D,    // Subtract key
            DECIMAL = 0x6E,     // Decimal key
            DIVIDE = 0x6F,      // Divide key
            F1 = 0x70,          // F1 key
            F2 = 0x71,          // F2 key
            F3 = 0x72,          // F3 key
            F4 = 0x73,          // F4 key
            F5 = 0x74,          // F5 key
            F6 = 0x75,          // F6 key
            F7 = 0x76,          // F7 key
            F8 = 0x77,          // F8 key
            F9 = 0x78,          // F9 key
            F10 = 0x79,         // F10 key
            F11 = 0x7A,         // F11 key
            F12 = 0x7B,         // F12 key
            NUMLOCK = 0x90,     // NUM LOCK key
            SCROLL = 0x91,      // SCROLL LOCK key
            LSHIFT = 0xA0,      // Left SHIFT key
            RSHIFT = 0xA1,      // Right SHIFT key
            LCONTROL = 0xA2,    // Left CONTROL key
            RCONTROL = 0xA3,    // Right CONTROL key
        }

        public delegate IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void KeyHandler(int input);

        private static IntPtr WindowHooked;

        private static HookCallback hookCallback;

        private static KeyHandler KeyReader;

        public static void CreateHook(KeyHandler _KeyReader)
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule mainModule = currentProcess.MainModule;
            hookCallback = HookFunc;
            KeyReader = _KeyReader;
            WindowHooked = SetWindowsHookEx(13, hookCallback, GetModuleHandle(mainModule?.ModuleName), 0u);
        }

        public static bool RemoveHook()
        {
            return UnhookWindowsHookEx(WindowHooked);
        }

        private static IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int num = wParam.ToInt32();
            if (nCode >= 0 && (num == 256 || num == 260))
            {
                KeyReader(Marshal.ReadInt32(lParam));
            }
            return CallNextHookEx(WindowHooked, nCode, wParam, lParam);
        }
    }
}
