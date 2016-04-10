using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

namespace KeyboardSwitcher
{
    public static class WindowSelector
    {
        public delegate void WindowSelectorEventHandler(object o, WindowSelectorEventArgs e);
        public static event WindowSelectorEventHandler UiUpdate;
        private static IntPtr hWndFoundWindow = IntPtr.Zero;
        private static bool isHookEnable = false;

        public static void MouseActivate(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && UiUpdate != null && !isHookEnable)
            {
                hWndFoundWindow = IntPtr.Zero;
                HookManager.MouseMove += HookManager_MouseMove;
                HookManager.MouseUp += HookManager_MouseUp;
                Cursor.Current = Cursors.Cross;
                isHookEnable = true;
            }
        }

        public static void KeyboardActivate(bool isEnable = false)
        {
            if (isEnable && UiUpdate != null && !isHookEnable)
            {
                hWndFoundWindow = IntPtr.Zero;
                HookManager.KeyPress += HookManager_KeyPress;
                isHookEnable = true;
            }
            else if (isHookEnable)
            {
                HookManager.KeyPress -= HookManager_KeyPress;
                RefreshWindow(hWndFoundWindow);
                isHookEnable = false;
            }
        }


        private static void HookManager_MouseUp(object sender, MouseEventExtArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                HookManager.MouseMove -= HookManager_MouseMove;
                HookManager.MouseUp -= HookManager_MouseUp;
                RefreshWindow(hWndFoundWindow);
                Cursor.Current = Cursors.Default;
                isHookEnable = false;
            }
        }

        private static void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            var hWnd = WinApi.GetForegroundWindow();
            GetWindowInfo(hWnd, true);
        }

        private static void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            var hWnd = WinApi.WindowFromPoint(e.Location);
            GetWindowInfo(hWnd);
        }

        private static void GetWindowInfo(IntPtr hWnd, bool isInFocus = false)
        {
            // http://www.codeproject.com/Articles/1698/MS-Spy-style-Window-Finder
            if (hWnd != IntPtr.Zero && WinApi.IsWindow(hWnd) && hWnd != hWndFoundWindow)
            {
                WindowSelectorEventArgs e = new WindowSelectorEventArgs();

                //Программа
                int pId;
                var threadId = WinApi.GetWindowThreadProcessId(hWnd, out pId);
                using (var p = Process.GetProcessById(pId))
                    e.ProgramName = p.MainModule.ModuleName;

                if (isInFocus)
                {
                    e.HGetForegroundWindow = (int)hWnd;
                    var h = Misc.GetFocusedHandle(threadId, e);
                    if (h != IntPtr.Zero) hWnd = h;
                    if(hWnd == hWndFoundWindow) return;
                }

                //Класс
                var temp = new StringBuilder(256);
                WinApi.GetClassName(hWnd, temp, temp.Capacity);
                e.ClassName = temp.ToString();
                temp.Clear();

                //Заголовок
                temp.Capacity = 256;
                WinApi.GetWindowText(hWnd, temp, temp.Capacity);
                e.WindowCaption = temp.ToString();
                temp.Clear();

                //Дескриптор
                e.HWindow = (int)hWnd;
                e.HParent = (int)WinApi.GetParent(hWnd);

                //Очищаем
                RefreshWindow(hWndFoundWindow);

                //Рисуем прямоугольник
                HighlightFoundWindow(hWnd);

                hWndFoundWindow = hWnd;
                
                if (UiUpdate != null)
                    UiUpdate.Invoke(null, e);
            }
        }

        private static void HighlightFoundWindow(IntPtr hWnd)
        {
            var r = new WinApi.RECT();
            WinApi.GetClientRect(hWnd, out r);
            using (var g = Graphics.FromHwnd(hWnd))
                g.DrawRectangle(new Pen(Color.ForestGreen, 3),
                    r.Left, r.Top, r.Right - 1, r.Bottom - 1);
        }

        private static void RefreshWindow(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero && WinApi.IsWindow(hWnd))
            {
                WinApi.InvalidateRect(hWnd, IntPtr.Zero, true);
                WinApi.UpdateWindow(hWnd);
                WinApi.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero,
                    WinApi.RedrawWindowFlags.Frame |
                    WinApi.RedrawWindowFlags.Invalidate |
                    WinApi.RedrawWindowFlags.UpdateNow |
                    WinApi.RedrawWindowFlags.AllChildren);
            }
        }

    }
}
