using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

namespace KeyboardSwitcher
{
    public partial class WorkerLayer
    {
        public void EnableGlobalHotKey()
        {
            HookManager.MouseMove += HookManager_MouseMove;
           // HookManager.TaskList += new TaskListEventHandler(HookManager_TaskList);
            HotKeyManager.EnableGlobalHotKey();
            HotKeyManager.RunSwitcher += new KeyEventHandler(HotKeyManager_RunSwitcher);
            HotKeyManager.RunXScroll += new EventHandler<MouseEventExtArgs>(HotKeyManager_RunXScroll);
            HotKeyManager.RunXMove += new EventHandler<MouseEventExtArgs>(HotKeyManager_RunXMove);
            HotKeyManager.StopXMove += new EventHandler(HotKeyManager_StopXMove);
        }

       
        public void DisableGlobalHotKey()
        {
            HookManager.MouseMove -= HookManager_MouseMove;
           // HookManager.TaskList -= new TaskListEventHandler(HookManager_TaskList);
            HotKeyManager.DisableGlobalHotKey();
            HotKeyManager.RunSwitcher -= new KeyEventHandler(HotKeyManager_RunSwitcher);
            HotKeyManager.RunXScroll -= new EventHandler<MouseEventExtArgs>(HotKeyManager_RunXScroll);
            HotKeyManager.RunXMove -= new EventHandler<MouseEventExtArgs>(HotKeyManager_RunXMove);
            HotKeyManager.StopXMove -= new EventHandler(HotKeyManager_StopXMove);
        }

        private IntPtr hWndMoveWindow = IntPtr.Zero;
        private Point offsetMoveWindow = new Point();
        private bool isXMove = false;

        void HookManager_TaskList(int lParam, ref bool handled)
        {
            handled = true;
        }


        void HotKeyManager_RunXMove(object sender, MouseEventExtArgs e)
        {
            var hWnd = WinApi.WindowFromPoint(e.Location);
            var hParent = IntPtr.Zero;

            if (hWnd == IntPtr.Zero) return;
            while (true)
            {
                hParent = WinApi.GetParent(hWnd);
                if (hParent == IntPtr.Zero) break;
                hWnd = hParent;
            }
            if (!WinApi.IsWindow(hWnd)) return;

            WinApi.RECT r = new WinApi.RECT();
            WinApi.GetWindowRect(hWnd, out r);
            offsetMoveWindow = new Point(e.X - r.Left, e.Y - r.Top);
            hWndMoveWindow = hWnd;
            isXMove = true;
        }

        void HotKeyManager_StopXMove(object sender, EventArgs e)
        {
            isXMove = false;
        }

        void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            if (isXMove)
            {
                WinApi.SetWindowPosFlags swp = WinApi.SetWindowPosFlags.AsynchronousWindowPosition
                                               | WinApi.SetWindowPosFlags.IgnoreResize
                                               | WinApi.SetWindowPosFlags.IgnoreZOrder
                                               | WinApi.SetWindowPosFlags.DoNotActivate;
                WinApi.SetWindowPos(hWndMoveWindow, IntPtr.Zero,
                    e.X - offsetMoveWindow.X, e.Y - offsetMoveWindow.Y, 0, 0, swp);
            }
        }


        void HotKeyManager_RunXScroll(object sender, MouseEventExtArgs e)
        {
            Misc.MouseWheelToCursorPos(e);
        }

         private Thread threadSwitcher = null;
       // private Task t = Task.Factory.StartNew(Switcher.RunSwitcher);

        void HotKeyManager_RunSwitcher(object sender, KeyEventArgs e)
        {
            if (threadSwitcher == null || !threadSwitcher.IsAlive)
            {
                threadSwitcher = new Thread(Switcher.RunSwitcher);
                threadSwitcher.IsBackground = true;
                threadSwitcher.SetApartmentState(ApartmentState.STA);
                threadSwitcher.Start(Switcher.SwitcherOptions.SwitchText | Switcher.SwitcherOptions.SwitchLauot);
            }
        }

       

        





        public delegate void AppendTextEventHandler(string text);

        public event AppendTextEventHandler AppendTextEvent;

        public void AddLog(string text)
        {
            if (AppendTextEvent != null)
                AppendTextEvent.Invoke(text + "\r\n");
        }
    }
}
