using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor
{
    internal class MouseHook : Hook
    {

        public event EventHandler<MouseEventExtArgs> MouseDown;
        public event EventHandler<MouseEventExtArgs> MouseUp;
        public event EventHandler<MouseEventExtArgs> MouseClick;
        public event EventHandler<MouseEventExtArgs> MouseWheel;
        public event EventHandler<MouseEventExtArgs> MouseMove;

        private int mOldX;
        private int mOldY;

        public MouseHook() : base(HookType.WH_MOUSE_LL, true)
        {
            base.Callback += mouseHook_Callback;
        }

        public void TryUnhook()
        {
            if (MouseClick == null &&
                MouseDown == null &&
                MouseMove == null &&
                MouseUp == null &&
                MouseWheel == null)
            {
                base.Unhook();
            }
        }

        private void mouseHook_Callback(int nCode, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Marshall the data from callback.
            MouseLLHookStruct mouseHookStruct =
                (MouseLLHookStruct) Marshal.PtrToStructure(lParam, typeof (MouseLLHookStruct));

            //detect button clicked
            MouseButtons button = MouseButtons.None;
            short mouseDelta = 0;
            bool mouseDown = false;
            bool mouseUp = false;

            switch ((int) wParam)
            {
                case WM_LBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.Left;
                    break;
                case WM_LBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.Left;
                    break;
                case WM_RBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.Right;
                    break;
                case WM_RBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.Right;
                    break;
                case WM_MBUTTONDOWN:
                    mouseDown = true;
                    button = MouseButtons.Middle;
                    break;
                case WM_MBUTTONUP:
                    mouseUp = true;
                    button = MouseButtons.Middle;
                    break;
                case WM_MOUSEWHEEL:
                    //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
                    //One wheel click is defined as WHEEL_DELTA, which is 120. 
                    //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                    mouseDelta = (short) ((mouseHookStruct.MouseData >> 16) & 0xffff);
                    break;
                case WM_XBUTTONDOWN:
                    mouseDown = true;
                    switch (mouseHookStruct.MouseData >> 16)
                    {
                        case XBUTTON1:
                            button = MouseButtons.XButton1;
                            break;
                        case XBUTTON2:
                            button = MouseButtons.XButton2;
                            break;
                    }
                    break;
                case WM_XBUTTONUP:
                    mouseUp = true;
                    switch (mouseHookStruct.MouseData >> 16)
                    {
                        case XBUTTON1:
                            button = MouseButtons.XButton1;
                            break;
                        case XBUTTON2:
                            button = MouseButtons.XButton2;
                            break;
                    }
                    break;
            }

            //generate event 
            MouseEventExtArgs e = new MouseEventExtArgs(
                button,
                mouseUp || mouseDown ? 1 : 0,
                mouseHookStruct.Point.X,
                mouseHookStruct.Point.Y,
                mouseDelta);

            //Mouse down
            if (MouseDown != null && mouseDown)
            {
                MouseDown.Invoke(null, e);
            }

            //If someone listens to click and a click is heppened
            if (MouseClick != null && mouseDown)
            {
                MouseClick.Invoke(null, e);
            }

            //Mouse up
            if (MouseUp != null && mouseUp)
            {
                MouseUp.Invoke(null, e);
            }

            //Wheel was moved
            if (MouseWheel != null && mouseDelta != 0)
            {
                MouseWheel.Invoke(null, e);
            }

            handled = handled || e.Handled;

            //If someone listens to move and there was a change in coordinates raise move event
            if ((MouseMove != null) && (mOldX != mouseHookStruct.Point.X || mOldY != mouseHookStruct.Point.Y))
            {
                mOldX = mouseHookStruct.Point.X;
                mOldY = mouseHookStruct.Point.Y;

                MouseMove.Invoke(null, e);
                handled = handled || (e.Handled && (int) wParam == WM_MOUSEMOVE);
            }
        }

        #region PInvoke Declarations

        private const int
            WM_MOUSEMOVE = 0x200,
            WM_MOUSEWHEEL = 0x020A,
            WM_LBUTTONDOWN = 0x201,
            WM_RBUTTONDOWN = 0x204,
            WM_MBUTTONDOWN = 0x207,
            WM_XBUTTONDOWN = 0x20B,
            WM_LBUTTONUP = 0x202,
            WM_RBUTTONUP = 0x205,
            WM_MBUTTONUP = 0x208,
            WM_XBUTTONUP = 0x20C,
            WM_LBUTTONDBLCLK = 0x203,
            WM_RBUTTONDBLCLK = 0x206,
            WM_MBUTTONDBLCLK = 0x209,
            WM_XBUTTONDBLCLK = 0x20D;

        private const int
            XBUTTON1 = 0x0001,
            XBUTTON2 = 0x0002;

        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            /// <summary>
            /// Specifies the X-coordinate of the point. 
            /// </summary>
            public int X;

            /// <summary>
            /// Specifies the Y-coordinate of the point. 
            /// </summary>
            public int Y;
        }

        /// <summary>
        /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MouseLLHookStruct
        {
            /// <summary>
            /// Specifies a Point structure that contains the X- and Y-coordinates of the cursor, in screen coordinates. 
            /// </summary>
            public Point Point;

            /// <summary>
            /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. 
            /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward, 
            /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user. 
            /// One wheel click is defined as WHEEL_DELTA, which is 120. 
            ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
            /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
            /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, MouseData is not used. 
            ///XBUTTON1
            ///The first X button was pressed or released.
            ///XBUTTON2
            ///The second X button was pressed or released.
            /// </summary>
            public int MouseData;

            /// <summary>
            /// Specifies the event-injected flag. An application can use the following value to test the mouse Flags. Value Purpose 
            ///LLMHF_INJECTED Test the event-injected flag.  
            ///0
            ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
            ///1-15
            ///Reserved.
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
