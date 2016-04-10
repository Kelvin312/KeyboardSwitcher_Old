using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CBTActivityMonitor
{
    public class CBTHook
    {
        private Hook hook = new Hook(HookType.WH_CBT, false, true);

        public void Start(IntPtr mWnd)
        {
            hook.Callback += new Hook.HookCallback(hook_Callback);
            hook.StartHook();
        }

        public void Stop()
        {
            hook.Unhook();
            hook.Callback -= new Hook.HookCallback(hook_Callback);
        }

        int hook_Callback(int code, IntPtr wParam, IntPtr lParam, ref bool callNext)
        {
            switch (code)
            {
                case  HCBT_SETFOCUS:
                    var msg = RegisterWindowMessage("WM_HCBT_SETFOCUS");
                    var hWnd = FindWindow(null, "Test for the class HookManager");
                    SendMessage(hWnd, msg, wParam, lParam);
                    break;
            }
            return 0;
        }

        private const int HCBT_SETFOCUS = 9;
        private const int HCBT_SYSCOMMAND = 8;

        private const int SC_TASKLIST = 0xF130;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }


    /// <summary>
    /// A hook is a point in the system message-handling mechanism where an application
    /// can install a subroutine to monitor the message traffic in the system and process 
    /// certain types of messages before they reach the target window procedure.
    /// </summary>
    public class Hook : Component
    {
        private HookType type;
        internal bool hooked = false;
        private IntPtr hHook;
        private bool wrapCallback, global;
        private IntPtr wrappedDelegate;
        private IntPtr hWrapperInstance;
        private readonly HookProc managedDelegate;

        /// <summary>
        /// Occurs when the hook's callback is called.
        /// </summary>
        public event HookCallback Callback;

        /// <summary>
        /// Represents a method that handles a callback from a hook.
        /// </summary>
        public delegate int HookCallback(int code, IntPtr wParam, IntPtr lParam, ref bool callNext);

        /// <summary>
        /// Creates a new hook and hooks it.
        /// </summary>
        public Hook(HookType type, HookCallback callback, bool wrapCallback, bool global)
            : this(type, wrapCallback, global)
        {
            this.Callback += callback;
            StartHook();
        }

        /// <summary>
        /// Creates a new hook.
        /// </summary>
        public Hook(HookType type, bool wrapCallback, bool global)
            : this()
        {
            this.type = type;
            this.wrapCallback = wrapCallback;
            this.global = global;
        }

        /// <summary>
        /// Creates a new hook.
        /// </summary>
        public Hook(IContainer container)
            : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Creates a new hook.
        /// </summary>
        public Hook()
        {
            managedDelegate = new HookProc(InternalCallback);
        }

        /// <summary>
        /// The type of the hook.
        /// </summary>
        public HookType Type
        {
            get { return type; }
            set { type = value; }
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
            if (wrapCallback)
            {
                wrappedDelegate = AllocHookWrapper(delegt);
                hWrapperInstance = LoadLibrary("ManagedWinapiNativeHelper.dll");
                hHook = SetWindowsHookEx(type, wrappedDelegate, hWrapperInstance, 0);
            }
            else if (global)
            {
                IntPtr hModule = Marshal.GetHINSTANCE(typeof (Hook).Assembly.GetModules()[0]);
                using (Process process = Process.GetCurrentProcess())
                using (ProcessModule module = process.MainModule)
                {
                    hModule = GetModuleHandle(module.ModuleName);
                }
                hHook = SetWindowsHookEx(type, delegt, hModule, 0);
            }
            else
            {
                hHook = SetWindowsHookEx(type, delegt, IntPtr.Zero, getThreadID());
            }
            if (hHook == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
            hooked = true;
        }

        private uint getThreadID()
        {
#pragma warning disable 0618
            return (uint)AppDomain.GetCurrentThreadId();
#pragma warning restore 0618
        }

        /// <summary>
        /// Unhooks the hook.
        /// </summary>
        public virtual void Unhook()
        {
            if (!hooked) return;
            if (!UnhookWindowsHookEx(hHook)) throw new Win32Exception(Marshal.GetLastWin32Error());
            if (wrapCallback)
            {
                if (!FreeHookWrapper(wrappedDelegate)) throw new Win32Exception();
                if (!FreeLibrary(hWrapperInstance)) throw new Win32Exception();
            }
            hooked = false;
        }

        /// <summary>
        /// Unhooks the hook if necessary.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (hooked)
            {
                Unhook();
            }
            base.Dispose(disposing);
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
                bool callNext = true;
                int retval = Callback(code, wParam, lParam, ref callNext);
                if (!callNext) return retval;
            }
            return CallNextHookEx(hHook, code, wParam, lParam);
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        #region PInvoke Declarations

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(HookType hook, IntPtr callback,
           IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        internal static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam,
           IntPtr lParam);

        [DllImport("ManagedWinapiNativeHelper.dll")]
        private static extern IntPtr AllocHookWrapper(IntPtr callback);

        [DllImport("ManagedWinapiNativeHelper.dll")]
        private static extern bool FreeHookWrapper(IntPtr wrapper);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        internal static readonly int HC_ACTION = 0,
            HC_GETNEXT = 1,
            HC_SKIP = 2,
            HC_NOREMOVE = 3,
            HC_SYSMODALON = 4,
            HC_SYSMODALOFF = 5;
        #endregion
    }


    /// <summary>
    /// Hook Types. See the documentation of SetWindowsHookEx for reference.
    /// </summary>
    public enum HookType : int
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
