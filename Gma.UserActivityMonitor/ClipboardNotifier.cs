using System;
using System.Runtime.InteropServices;

namespace Gma.UserActivityMonitor
{
    /// <summary>
    /// Specifies a component that monitors the system clipboard for changes.
    /// </summary>

    internal class ClipboardNotifier
    {
        /// <summary>
        /// Occurs when the clipboard contents have changed.
        /// </summary>
        public event EventHandler ClipboardChanged;

        private IntPtr hWnd;
        private IntPtr nextHWnd;
        private EventDispatchingNativeWindow ednw;
        protected bool hooked = false;


        public void StartHook()
        {
            if (hooked) return;

            ednw = EventDispatchingNativeWindow.Instance;
            ednw.EventHandler += clipboardEventHandler;
            hWnd = ednw.Handle;
            nextHWnd = SetClipboardViewer(hWnd);
            hooked = true;
        }


        public void Unhook()
        {
            if (!hooked) return;
            ChangeClipboardChain(hWnd, nextHWnd);
            ednw.EventHandler -= clipboardEventHandler;
            hooked = false;
        }

        public void TryUnhook()
        {
            if (ClipboardChanged == null) Unhook();
        }


        /// <summary>
        /// Frees resources.
        /// </summary>
        protected void Dispose()
        {
            Unhook();
        }

        void clipboardEventHandler(ref System.Windows.Forms.Message m, ref bool handled)
        {
            if (handled) return;
            if (m.Msg == WM_DRAWCLIPBOARD)
            {
                // notify me
                if (ClipboardChanged != null)
                    ClipboardChanged(this, EventArgs.Empty);
                // pass on message
                SendMessage(nextHWnd, m.Msg, m.WParam, m.LParam);
                handled = true;
            }
            else if (m.Msg == WM_CHANGECBCHAIN)
            {
                if (m.WParam == nextHWnd)
                {
                    nextHWnd = m.LParam;
                }
                else
                {
                    SendMessage(nextHWnd, m.Msg, m.WParam, m.LParam);
                }
            }
        }

        #region PInvoke Declarations

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll")]
        private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private static readonly int
            WM_DRAWCLIPBOARD = 0x0308,
            WM_CHANGECBCHAIN = 0x030D;

        #endregion
    }
}
