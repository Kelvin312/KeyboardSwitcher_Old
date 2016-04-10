using System;
using System.Text;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
    public static partial class Misc
    {

        private static string GetText(IntPtr hWnd, TextReplaceMethod replaceMethod) //Взять текст 
        {
            string selectedText = "";
            switch (replaceMethod)
            {
                case TextReplaceMethod.TextBox:
                    {
                        int start = -1, next = -1;
                        WinApi.SendMessage(hWnd, WinApi.EM_GETSEL, out start, out next);
                        if (start != next)
                        {
                            // Возвращаемое значение длина текста в символах, не включая завершающий нулевой символ.
                            int len = (int)WinApi.SendMessage(hWnd, WinApi.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
                            StringBuilder sb = new StringBuilder(len + 1);
                            int lenRead = (int)WinApi.SendMessage(hWnd, WinApi.WM_GETTEXT, (IntPtr)sb.Capacity, sb);
                            if (lenRead > 0)
                            {
                                selectedText = sb.ToString().Substring(start, next - start);
                            }
                        }
                    }
                    break;
                case TextReplaceMethod.RichTextBox:
                    {
                        int start = -1, next = -1;
                        WinApi.SendMessage(hWnd, WinApi.EM_GETSEL, out start, out next);
                        if (start != next)
                        {
                            int len = next - start;
                            StringBuilder sb = new StringBuilder(len + 1);
                            int lenRead = (int)WinApi.SendMessage(hWnd, WinApi.EM_GETSELTEXT, IntPtr.Zero, sb);
                            if (lenRead > 0)
                            {
                                selectedText = sb.ToString();
                            }
                        }
                    }
                    break;
                case TextReplaceMethod.Scintilla:
                    {
                        int len = (int)WinApi.SendMessage(hWnd, WinApi.SCI_GETSELTEXT, IntPtr.Zero, IntPtr.Zero);
                        if (len > 1)
                        {
                            StringBuilder sb = new StringBuilder(len);
                            WinApi.SendMessage(hWnd, WinApi.SCI_GETSELTEXT, IntPtr.Zero, sb); //Не работает
                            selectedText = sb.ToString();
                        }
                    }
                    break;
                case TextReplaceMethod.OtherTextControl:
                    WinApi.SendMessage(hWnd, WinApi.WM_COPY, IntPtr.Zero, IntPtr.Zero);
                    selectedText = Clipboard.GetText();
                    break;
                case TextReplaceMethod.Devenv:
                case TextReplaceMethod.Unknown:
                    PressHotKey(new Keys[] { Keys.ControlKey, Keys.C });
                    selectedText = Clipboard.GetText();
                    break;
            }
            return selectedText;
        }

        private static void SetText(string text, IntPtr hWnd, TextReplaceMethod replaceMethod) //Вставить текст 
        {
            switch (replaceMethod)
            {
                case TextReplaceMethod.TextBox:
                case TextReplaceMethod.RichTextBox:
                    WinApi.SendMessage(hWnd, WinApi.EM_REPLACESEL, (IntPtr)1, text);
                    break;
                    case TextReplaceMethod.Scintilla:
                    WinApi.SendMessage(hWnd, WinApi.SCI_REPLACESEL, IntPtr.Zero, text); //Не работает
                    break;
                case TextReplaceMethod.OtherTextControl:
                    Clipboard.SetText(text);
                    WinApi.SendMessage(hWnd, WinApi.WM_PASTE, IntPtr.Zero, IntPtr.Zero);
                    break;
                case TextReplaceMethod.Devenv:
                case TextReplaceMethod.Unknown:
                    Clipboard.SetText(text);
                    PressHotKey(new Keys[] { Keys.ControlKey, Keys.V });
                    break;
            }
        }
    
    }
}
