using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

namespace KeyboardSwitcher
{

    public class WindowSelectorEventArgs
    {
        public string ProgramName { get; set; }
        public string ClassName { get; set; }
        public string WindowCaption { get; set; }
        public int HWindow { get; set; }
        public int HParent { get; set; }

        public int HCaret { get; set; }
        public int HFocus { get; set; }
        public int HActive { get; set; }
        public int HCapture { get; set; }
        public int HGetForegroundWindow { get; set; }
        public int HGetFocus { get; set; }
        public int HGetActiveWindow { get; set; }
    }

    [Flags]
    public enum TextReplaceMethod
    {
        Off = 0,
        Default = 1,
        TextBox = 8,
        RichTextBox = 16,
        Scintilla = 32,
        OtherTextControl = 64 | UseClipboard,
        Devenv = 128 | UseClipboard | UseKeyboard,
        Unknown = 256 | UseClipboard | UseKeyboard,
        UseClipboard = 0x1000,
        UseKeyboard = 0x2000
    }


    public enum Language : short
    {
        Off = 0,
        Default = 1,
        en_US = 0x0409, //en-US
        ru_RU = 0x0419 //ru-RU
    }


    public static partial class Misc
    {

        private static IDictionary<string, object> GetClipboardData() //Копирование буфера обмена 
        {
            var dict = new Dictionary<string, object>();
            var dataObject = Clipboard.GetDataObject();
            if (dataObject != null)
                foreach (var format in dataObject.GetFormats())
                {
                    dict.Add(format, dataObject.GetData(format));
                }
            return dict;
        }

        private static void SetClipboardData(IDictionary<string, object> dict, bool isCopy = true) //Восстановление буфера обмена 
        {
            DataObject dataObject = new DataObject();
            foreach (var kvp in dict)
            {
                dataObject.SetData(kvp.Key, kvp.Value);
            }
            Clipboard.SetDataObject(dataObject, isCopy);
        }

        public static IntPtr GetFocusedHandle(uint threadId, WindowSelectorEventArgs e = null) //Получение хендла активного контрола 
        {
            //@kurumpa http://stackoverflow.com/a/28409126
            IntPtr hWnd = IntPtr.Zero;
            IntPtr focusedHandle = IntPtr.Zero;
            var info = new WinApi.GUITHREADINFO();
            info.cbSize = Marshal.SizeOf(info);
            var success = WinApi.GetGUIThreadInfo(threadId, ref info);

            // target = hwndCaret || hwndFocus || (AttachThreadInput + GetFocus) || hwndActive
            var currentThreadId = WinApi.GetCurrentThreadId();
            if (currentThreadId != threadId) WinApi.AttachThreadInput(threadId, currentThreadId, true);
            focusedHandle = WinApi.GetFocus();
            if (e != null) e.HGetActiveWindow = (int)WinApi.GetActiveWindow();
            if (currentThreadId != threadId) WinApi.AttachThreadInput(threadId, currentThreadId, false);
            if (e != null)
            {
                e.HGetFocus = (int)focusedHandle;
                if (success)
                {
                    e.HCaret = (int)info.hwndCaret;
                    e.HFocus = (int)info.hwndFocus;
                    e.HActive = (int)info.hwndActive;
                    e.HCapture = (int)info.hwndCapture;
                }
            }
            if (success)
            {
                if (info.hwndCaret != IntPtr.Zero)
                {
                    hWnd = info.hwndCaret;
                }
                else if (info.hwndFocus != IntPtr.Zero)
                {
                    hWnd = info.hwndFocus;
                }
                else if (focusedHandle != IntPtr.Zero)
                {
                    hWnd = focusedHandle;
                }
                else if (info.hwndActive != IntPtr.Zero)
                {
                    hWnd = info.hwndActive;
                }
            }
            else
            {
                hWnd = focusedHandle;
            }
            return hWnd;
        }

        public static void MouseWheelToCursorPos(MouseEventExtArgs e, int mkKeyState = 0)
        {
            var hWnd = WinApi.WindowFromPoint(e.Location);
            var wParam = (IntPtr)((e.Delta << 16) | mkKeyState); 
            var lParam = (IntPtr)((e.X & 0xFFFF) | (e.Y << 16));
            WinApi.PostMessage(hWnd, WinApi.WM_MOUSEWHEEL, wParam, lParam);
        }
    }
}
