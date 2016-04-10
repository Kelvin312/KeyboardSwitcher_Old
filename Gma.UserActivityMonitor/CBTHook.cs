using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Gma.UserActivityMonitor
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lParam"></param>
    /// <param name="handled"></param>
    public delegate void TaskListEventHandler(int lParam, ref bool handled);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="hOldWnd"></param>
    /// <param name="handled"></param>
    public delegate void SetFocusEventHandler(IntPtr hWnd, IntPtr hOldWnd, ref bool handled);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="isMouse"></param>
    /// <param name="handled"></param>
    public delegate void ActivateEventHandler(IntPtr hWnd, bool isMouse, ref bool handled);

    internal class CBTHook : Hook
    {
        public event TaskListEventHandler TaskListCallback;
        public event SetFocusEventHandler SetFocusCallback;
        public event ActivateEventHandler ActivateCallback;


        public CBTHook() : base(HookType.WH_CBT, true)
        {
            base.Callback += cbtHook_Callback;
        }

        public void TryUnhook()
        {
            if (TaskListCallback == null &&
                SetFocusCallback == null &&
                ActivateCallback == null)
            {
                base.Unhook();
            }
        }

        private void cbtHook_Callback(int nCode, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((HCBT) nCode)
            {
                case HCBT.SetFocus:
                    // wParam
                    // Определяет дескриптор окна, получающего фокус клавиатуры.
                    // lParam
                    // Определяет дескриптор окна, теряющего фокус клавиатуры.
                    //e.HWndGaining = (IntPtr)wParam;
                    //e.HWndLosing = lParam;
                    if (SetFocusCallback != null)
                    {
                        SetFocusCallback.Invoke(wParam, lParam, ref handled);
                    }
                    break;

                case HCBT.Activate:
                    // wParam
                    // Определяет дескриптор окна, которое будет активировано
                    // lParam
                    // Specifies a long pointer to a CBTACTIVATESTRUCT structure containing the handle 
                    // to the active window and specifies whether the activation is changing because of a mouse click. 
                    if (ActivateCallback != null)
                    {
                        CBTActivateStruct cbtActivateStruct =
                            (CBTActivateStruct) Marshal.PtrToStructure(lParam, typeof (CBTActivateStruct));
                        ActivateCallback.Invoke(wParam, cbtActivateStruct.fMouse, ref handled);
                    }
                    break;
                case HCBT.SysCommand:
                    // wParam
                    // Определяет значение системной команды (SC_)
                    // lParam
                    // low-order = X
                    // high-order = Y | This parameter is –1 if the command is chosen using a system accelerator, or zero if using a mnemonic.
                    switch ((SysCommands) wParam)
                    {
                        case SysCommands.SC_TASKLIST:
                            //e.Location = new System.Drawing.Point((int)lParam & 0xFFFF, (int)lParam >> 16);
                            if (TaskListCallback != null)
                            {
                                TaskListCallback.Invoke((int) lParam, ref handled);
                            }
                            break;
                    }
                    break;
            }
        }

        #region PInvoke Declarations

        private enum HCBT : int
        {
            MoveSize = 0,
            MinMax = 1,
            QueueSync = 2,
            CreateWnd = 3,
            DestroyWnd = 4,
            Activate = 5,
            ClickSkipped = 6,
            KeySkipped = 7,
            SysCommand = 8,
            SetFocus = 9
        }

        private enum SysCommands : int
        {
            SC_SIZE = 0xF000,
            SC_MOVE = 0xF010,
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_NEXTWINDOW = 0xF040,
            SC_PREVWINDOW = 0xF050,
            SC_CLOSE = 0xF060,
            SC_VSCROLL = 0xF070,
            SC_HSCROLL = 0xF080,
            SC_MOUSEMENU = 0xF090,
            SC_KEYMENU = 0xF100,
            SC_ARRANGE = 0xF110,
            SC_RESTORE = 0xF120,
            SC_TASKLIST = 0xF130,
            SC_SCREENSAVE = 0xF140,
            SC_HOTKEY = 0xF150
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct CBTActivateStruct
        {
            public bool fMouse;
            public IntPtr hWndActive;
        }


    #endregion

    }
}
