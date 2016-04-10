using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
    internal class KeyboardHook : Hook
    {

        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        public event KeyPressEventHandler KeyPress;

        public KeyboardHook() : base(HookType.WH_KEYBOARD_LL, true)
        {
            base.Callback += keyboardHook_Callback;
        }

        public void TryUnhook()
        {
            if (KeyDown == null &&
                KeyUp == null &&
                KeyPress == null)
            {
                base.Unhook();
            }
        }

        private void keyboardHook_Callback(int nCode, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            KeyboardHookStruct keyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
            //raise KeyDown
            if (KeyDown != null && ((int)wParam == WM_KEYDOWN || (int)wParam == WM_SYSKEYDOWN))
            {
                Keys keyData = (Keys)keyboardHookStruct.VirtualKeyCode;
                KeyEventArgs e = new KeyEventArgs(keyData);
                KeyDown.Invoke(null, e);
                handled = e.Handled;
            }

            // raise KeyPress
            if (KeyPress != null && (int)wParam == WM_KEYDOWN)
            {
                bool isDownShift = (GetKeyState(VK_SHIFT) & 0x80) != 0;
                bool isDownCapslock = GetKeyState(VK_CAPITAL) != 0;

                byte[] keyState = new byte[256];
                GetKeyboardState(keyState);
                byte[] inBuffer = new byte[2];
                if (ToAscii(keyboardHookStruct.VirtualKeyCode,
                          keyboardHookStruct.ScanCode,
                          keyState,
                          inBuffer,
                          keyboardHookStruct.Flags) == 1)
                {
                    char key = (char)inBuffer[0];
                    if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                    KeyPressEventArgs e = new KeyPressEventArgs(key);
                    KeyPress.Invoke(null, e);
                    handled = handled || e.Handled;
                }
            }

            // raise KeyUp
            if (KeyUp != null && ((int)wParam == WM_KEYUP || (int)wParam == WM_SYSKEYUP))
            {
                Keys keyData = (Keys)keyboardHookStruct.VirtualKeyCode;
                KeyEventArgs e = new KeyEventArgs(keyData);
                KeyUp.Invoke(null, e);
                handled = handled || e.Handled;
            }
        }


        #region PInvoke Declarations

        [DllImport("user32")]
        private static extern int ToAscii(
            int uVirtKey,
            int uScanCode,
            byte[] lpbKeyState,
            byte[] lpwTransKey,
            int fuState);

        [DllImport("user32")]
        private static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        private const int
         WM_KEYDOWN = 0x100,
         WM_KEYUP = 0x101,
         WM_SYSKEYDOWN = 0x104,
         WM_SYSKEYUP = 0x105;


        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
            /// </summary>
            public int VirtualKeyCode;
            /// <summary>
            /// Specifies a hardware scan code for the key. 
            /// </summary>
            public int ScanCode;
            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public int Flags;
            /// <summary>
            /// Specifies the Time stamp for this message.
            /// </summary>
            public int Time;
            /// <summary>
            /// Specifies extra information associated with the message. 
            /// </summary>
            public int ExtraInfo;
        }

#endregion
    }
}
