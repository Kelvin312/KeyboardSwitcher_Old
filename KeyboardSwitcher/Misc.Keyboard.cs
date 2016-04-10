using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
    public static partial class Misc
    {
        private static void PressHotKey(Keys[] keys, bool isScan = true, int sleep = 40) //Нажимает последовательность клавиш 
        {
            var inputs = new WinApi.INPUT[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                inputs[i] = MakeKeyInput(keys[i], true, isScan);
            }
            WinApi.SendInput((uint)keys.Length, inputs, Marshal.SizeOf(typeof(WinApi.INPUT)));
            Thread.Sleep(sleep); //25 ms мало
            for (int i = keys.Length - 1; i >= 0; i--)
            {
                inputs[i] = MakeKeyInput(keys[i], false, isScan);
            }
            WinApi.SendInput((uint)keys.Length, inputs, Marshal.SizeOf(typeof(WinApi.INPUT)));
        }

        private static bool IsExtendedKey(Keys vkCode)
        {
            return
                vkCode == Keys.Menu ||
                vkCode == Keys.LMenu ||
                vkCode == Keys.RMenu ||
                vkCode == Keys.ControlKey ||
                vkCode == Keys.LControlKey ||
                vkCode == Keys.RControlKey ||
                vkCode == Keys.Insert ||
                vkCode == Keys.Delete ||
                vkCode == Keys.Home ||
                vkCode == Keys.End ||
                vkCode == Keys.PageUp ||
                vkCode == Keys.PageDown ||
                vkCode == Keys.Right ||
                vkCode == Keys.Up ||
                vkCode == Keys.Left ||
                vkCode == Keys.Down ||
                vkCode == Keys.NumLock ||
                vkCode == Keys.Cancel ||
                vkCode == Keys.Snapshot ||
                vkCode == Keys.Divide;
        }

        private static WinApi.INPUT MakeKeyInput(Keys vkCode, bool isDown, bool isScan)
        {
            return new WinApi.INPUT
            {
                type = WinApi.INPUT_KEYBOARD,
                ki = new WinApi.KEYBOARD_INPUT
                {
                    wVk = isScan ? (ushort)0 : (ushort)vkCode,
                    wSc = isScan ? (ushort)WinApi.MapVirtualKey((uint)vkCode, WinApi.MAPVK_VK_TO_VSC) : (ushort)0,
                    Flags = (isScan ? WinApi.KEYEVENTF_SCANCODE :
                    (IsExtendedKey(vkCode) ? WinApi.KEYEVENTF_EXTENDEDKEY : 0))
                    | (isDown ? 0 : WinApi.KEYEVENTF_KEYUP),
                    Time = 0,
                    dwExtraInfo = 0
                }
            };
        }

        private static void SendHotKey(Keys[] keys) //Постит клавиши в процесс 
        { // http://forum.sources.ru/index.php?showtopic=184180&st=0

            //             if (h != NULL)
            //    {
            //        HWND child = ::FindWindowEx(h,NULL,"Edit", NULL);

            //        UINT lparam_Ctrl1 = ::MapVirtualKey(VK_CONTROL, 0) << 16 | 1 ;
            //        UINT lparam_A1 = ::MapVirtualKey((int)'A', 0) << 16 | 1 ;

            //        UINT lparam_A2 = 1 << 31 | 1 << 30 | ::MapVirtualKey((int)'A', 0) << 16 | 1 ;
            //        UINT lparam_Ctrl2 = 1 << 31 | 1 << 30 | ::MapVirtualKey(VK_CONTROL, 0) << 16 | 1 ;

            //        DWORD pid;
            //        DWORD tid = GetWindowThreadProcessId(child, &pid);
            //        HANDLE hProc = OpenProcess(PROCESS_QUERY_INFORMATION | SYNCHRONIZE, FALSE, pid);
            //        //Ctrl + A

            //        AttachThreadInput(GetCurrentThreadId(), tid, TRUE);

            //        LRESULT pl1_Ctrl = ::PostMessage(child, WM_KEYDOWN, VK_CONTROL, lparam_Ctrl1 );
            //        WaitForInputIdle(hProc, INFINITE);

            //        BYTE state[256];
            //        GetKeyboardState(state);
            //        state[VK_CONTROL] = 0x80;
            //        SetKeyboardState(state);

            //        LRESULT pl1_A = ::PostMessage(child, WM_KEYDOWN, (int)'A', lparam_A1 );
            //        WaitForInputIdle(hProc, INFINITE);
            ///*
            //        GetKeyboardState(state);
            //        state['A'] = 0x80;
            //        SetKeyboardState(state);
            //*/

            //        LRESULT pl2_A = ::PostMessage(child, WM_KEYUP, (int)'A', lparam_A2);
            //        WaitForInputIdle(hProc, INFINITE);
            ///*
            //        GetKeyboardState(state);
            //        state['A'] = 0x0;
            //        SetKeyboardState(state);
            //*/

            //        LRESULT pl2_Ctrl = ::PostMessage(child, WM_KEYUP, VK_CONTROL, lparam_Ctrl2);        
            //        WaitForInputIdle(hProc, INFINITE);
            ///*
            //        GetKeyboardState(state);
            //        state[VK_CONTROL] = 0x0;
            //        SetKeyboardState(state);
            //*/

            //        AttachThreadInput(GetCurrentThreadId(), tid, FALSE);

            //    }
        }
    }
}
