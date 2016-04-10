using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
    public static partial class Switcher
    {
       
        [Flags]
        public enum SwitcherOptions
        {
            SwitchText = 1,
            SwitchLauotIsText = 2,
            SwitchLauot = 4,
            SetLauot = 8,
        }

        public static void RunSwitcher(object o)
        {
            var swo = (SwitcherOptions) o;

            if((swo & SwitcherOptions.SetLauot) != 0) Thread.Sleep(50);

            int pId;
            string pName;
            var hMainWnd = WinApi.GetForegroundWindow();
            
            var threadId = WinApi.GetWindowThreadProcessId(hMainWnd, out pId);
            using (var p = Process.GetProcessById(pId))
                pName = p.MainModule.ModuleName;

            Language layout = (Language)(short)WinApi.GetKeyboardLayout(threadId); //LOWORD

            var hWnd = Misc.GetFocusedHandle(threadId);
            if (hWnd == IntPtr.Zero) hWnd = hMainWnd;

            var cNameSb = new StringBuilder(32);
            WinApi.GetClassName(hWnd, cNameSb, cNameSb.Capacity);
            var classNames = cNameSb.ToString().Split(new char[] { ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);

            if ((swo & SwitcherOptions.SwitchText) != 0 && (layout == Language.en_US || layout == Language.ru_RU))
            {
                #region Filter

                var ma = TextReplaceMethod.OtherTextControl;
                switch (pName)
                {
                    case "devenv.exe":
                        ma = TextReplaceMethod.Devenv;
                        break;
                }
                foreach (var className in classNames)
                {
                    switch (className)
                    {
                        case "Static":
                        case "STATIC":
                        case "Button":
                        case "BUTTON":
                        case "SysListView32":
                        case "SysTreeView32":
                        case "ListBox":
                        case "ScrollBar":
                        case "ComboBox":
                        case "COMBOBOX":
                        case "msctls_hotkey32":
                        case "ConsoleWindowClass":
                            ma = TextReplaceMethod.Off;
                            break;
                        case "Edit":
                        case "EDIT":
                            ma = TextReplaceMethod.TextBox;
                            break;
                        case "RichEdit20W":
                            ma = TextReplaceMethod.RichTextBox;
                            break;
                        case "Scintilla":
                            //ma = TextReplaceMethod.Scintilla; //хз как починить
                            break;
                    }
                }
                #endregion

                if (ma != TextReplaceMethod.Off)
                {
                    var success = Misc.SwitchText(hWnd, layout, ma);
                    if (success && (swo & SwitcherOptions.SwitchLauotIsText) != 0) swo |= SwitcherOptions.SwitchText;
                }
            }

            if ((swo & SwitcherOptions.SwitchLauot) != 0)
            {
                #region Filter

                #endregion

                Misc.SwitchLayout(hWnd, layout);
            }
            else if ((swo & SwitcherOptions.SetLauot) != 0)
            {
                #region Filter

#endregion

                Misc.SetLayout(hWnd, layout);
            }

        }
    } 
}