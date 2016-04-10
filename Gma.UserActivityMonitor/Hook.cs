using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
    /// <summary>
    /// A hook is a point in the system message-handling mechanism where an application
    /// can install a subroutine to monitor the message traffic in the system and process 
    /// certain types of messages before they reach the target window procedure.
    /// </summary>
    internal class Hook
    {
        private readonly HookType type;
        private bool hooked = false;
        private IntPtr hHook;
        private readonly bool global;
        private readonly HookProc managedDelegate;
        private EventDispatchingNativeWindow ednw;

        /// <summary>
        /// Occurs when the hook's callback is called.
        /// </summary>
        public event HookCallback Callback;

        /// <summary>
        /// Represents a method that handles a callback from a hook.
        /// </summary>
        public delegate void HookCallback(int nCode, IntPtr wParam, IntPtr lParam, ref bool handled);

        /// <summary>
        /// Creates a new hook and hooks it.
        /// </summary>
        public Hook(HookType type, HookCallback callback, bool global)
            : this(type, global)
        {
            this.Callback += callback;
            StartHook();
        }

        /// <summary>
        /// Creates a new hook.
        /// </summary>
        public Hook(HookType type, bool global)
        {
            this.type = type;
            this.global = global;
            managedDelegate = InternalCallback;
        }

        /// <summary>
        /// The type of the hook.
        /// </summary>
        public HookType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Whether this hook has been started.
        /// </summary>
        public bool Hooked
        {
            get { return hooked; }
        }

        /// <summary>
        /// Hooks the hook.
        /// </summary>
        public virtual void StartHook()
        {
            if (hooked) return;
            IntPtr delegt = Marshal.GetFunctionPointerForDelegate(managedDelegate);
            if (global)
            {
                if (type == HookType.WH_KEYBOARD_LL || type == HookType.WH_MOUSE_LL)
                {
                    hHook = SetWindowsHookEx(type, delegt, getMainModuleHandle(), 0);
                }
                else
                {
                    ednw = EventDispatchingNativeWindow.Instance;
                    ednw.EventHandler += new WndProcEventHandler(hookEventHandler);
                    hHook = AllocHookWrapper(type, ednw.Handle);
                }
            }
            else
            {
                hHook = SetWindowsHookEx(type, delegt, IntPtr.Zero, getThreadID());
            }
            if (hHook == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
            hooked = true;
        }

        /// <summary>
        /// Unhooks the hook.
        /// </summary>
        public virtual void Unhook()
        {
            if (!hooked) return;
            if (global)
            {
                if (type == HookType.WH_KEYBOARD_LL || type == HookType.WH_MOUSE_LL)
                {
                    if (!UnhookWindowsHookEx(hHook)) throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                else
                {
                    if (!FreeHookWrapper(type)) throw new Win32Exception(Marshal.GetLastWin32Error());
                    ednw.EventHandler -= new WndProcEventHandler(hookEventHandler);
                }
            }
            else
            {
                if (!UnhookWindowsHookEx(hHook)) throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            hooked = false;
        }


        private void hookEventHandler(ref Message wndMsg, ref bool wndHandled)
        {
            if (wndMsg.Msg == WM_COPYDATA)
            {
                //Marshall the data from callback.
                COPYDATASTRUCT data = (COPYDATASTRUCT) Marshal.PtrToStructure(wndMsg.LParam, typeof (COPYDATASTRUCT));
                if (Callback != null && (HookType) data.dwData == type)
                {
                    MSG msg = (MSG) Marshal.PtrToStructure(data.lpData, typeof (MSG));
                    bool handled = false;
                    Callback((int) msg.message, msg.wParam, msg.lParam, ref handled);
                    if (handled)
                    {
                        wndMsg.Result = new IntPtr(1);
                        wndHandled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Override this method if you want to prevent a call
        /// to the CallNextHookEx method or if you want to return
        /// a different return value. For most hooks this is not needed.
        /// </summary>
        protected virtual int InternalCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && Callback != null)
            {
                bool handled = false;
                Callback(code, wParam, lParam, ref handled);
                if (handled) return 1;
            }
            return CallNextHookEx(hHook, code, wParam, lParam);
        }

        private uint getThreadID()
        {
#pragma warning disable 0618
            return (uint) AppDomain.GetCurrentThreadId();
#pragma warning restore 0618
        }

        private IntPtr getMainModuleHandle()
        {
            IntPtr hMod;
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
            {
                hMod = module.BaseAddress;
            }
            return hMod;
        }

        /// <summary>
        /// Unhooks the hook if necessary.
        /// </summary>
        protected void Dispose()
        {
            if (hooked)
            {
                Unhook();
            }
        }


        [DllImport("ManagedWinapiNativeHelper.dll", SetLastError = true)]
        private static extern IntPtr AllocHookWrapper(HookType type, IntPtr hWnd);

        [DllImport("ManagedWinapiNativeHelper.dll", SetLastError = true)]
        private static extern bool FreeHookWrapper(HookType type);

        #region PInvoke Declarations

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(HookType hook, IntPtr callback,
            IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        internal static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,
            IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData; // Any value the sender chooses.  Perhaps its main window handle?
            public int cbData; // The count of bytes in the message.
            public IntPtr lpData; // The address of the message.
        }

        private const int WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr hwnd;
            public UInt32 message;
            public IntPtr wParam;
            public IntPtr lParam;
            public UInt32 time;
            public int ptX;
            public int ptY;
        }

        #endregion
    }

    
    /// <summary>
    /// Hook Types. See the documentation of SetWindowsHookEx for reference.
    /// </summary>
    internal enum HookType : int
    {
        ///
        WH_JOURNALRECORD = 0,
        ///
        WH_JOURNALPLAYBACK = 1,
        ///
        WH_KEYBOARD = 2,
        ///
        WH_GETMESSAGE = 3,
        ///
        WH_CALLWNDPROC = 4,
        ///
        WH_CBT = 5,
        ///
        WH_SYSMSGFILTER = 6,
        ///
        WH_MOUSE = 7,
        ///
        WH_HARDWARE = 8,
        ///
        WH_DEBUG = 9,
        ///
        WH_SHELL = 10,
        ///
        WH_FOREGROUNDIDLE = 11,
        ///
        WH_CALLWNDPROCRET = 12,
        ///
        WH_KEYBOARD_LL = 13,
        ///
        WH_MOUSE_LL = 14
    }
}
