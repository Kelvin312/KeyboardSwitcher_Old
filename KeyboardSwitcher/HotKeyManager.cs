using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

namespace KeyboardSwitcher
{
    public static class HotKeyManager
    {
        #region fu

        public static void EnableGlobalHotKey()
        {
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyUp += HookManager_KeyUp;
            
            HookManager.MouseDown += HookManager_MouseDown;
            HookManager.MouseUp += HookManager_MouseUp;
            HookManager.MouseWheel += HookManager_MouseWheel;
        }

        public static void DisableGlobalHotKey()
        {
            HookManager.KeyDown -= HookManager_KeyDown;
            HookManager.KeyUp -= HookManager_KeyUp;

            HookManager.MouseDown -= HookManager_MouseDown;
            HookManager.MouseUp -= HookManager_MouseUp;
            HookManager.MouseWheel -= HookManager_MouseWheel;
        }

        private static Dictionary<Keys, bool> keyStateMap = new Dictionary<Keys, bool>();
        private static Dictionary<MouseButtons, bool> mouseStateMap = new Dictionary<MouseButtons, bool>();
        private static int countKeyPressed = 0;
        private static int countMousePressed = 0;
        private static bool error = false;


        private static void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            isSwitcher = false;
            if (!(keyStateMap.ContainsKey(e.KeyCode) && keyStateMap[e.KeyCode]))
            {
                KeyboardChange(e, true);
                if (++countKeyPressed > 10) error = true;
            }
            keyStateMap[e.KeyCode] = true;
        }

        private static void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyStateMap.ContainsKey(e.KeyCode) && keyStateMap[e.KeyCode])
            {
                KeyboardChange(e, false);
                if (--countKeyPressed < 0) error = true;
            }
            keyStateMap[e.KeyCode] = false;
        }

        private static void HookManager_MouseDown(object sender, MouseEventExtArgs e)
        {
            isSwitcher = false;
            if (!(mouseStateMap.ContainsKey(e.Button) && mouseStateMap[e.Button]))
            {
                MouseChange(e, true);
                if (++countMousePressed > 6) error = true;
            }
            mouseStateMap[e.Button] = true;
        }

        private static void HookManager_MouseUp(object sender, MouseEventExtArgs e)
        {
            if (mouseStateMap.ContainsKey(e.Button) && mouseStateMap[e.Button])
            {
                MouseChange(e, false);
                if (--countMousePressed < 0) error = true;
            }
            mouseStateMap[e.Button] = false;
        }

        private static void HookManager_MouseWheel(object sender, MouseEventExtArgs e)
        {
            isSwitcher = false;
            MouseChange(e, true);
        }

        #endregion

        public static event KeyEventHandler RunSwitcher;
        public static event EventHandler<MouseEventExtArgs> RunXMove;
        public static event EventHandler StopXMove;
        public static event EventHandler<MouseEventExtArgs> RunXScroll;

        private static bool isSwitcher = false;
        private static int xMove = 0;
        private static Keys switcherKey = Keys.LShiftKey;
        private static Keys xMoveKey = Keys.LWin;

        private static void KeyboardChange(KeyEventArgs e, bool isDown)
        {
            if (xMove == 2)
            {
                StopXMove(null, null);
            }
            if (e.KeyCode == xMoveKey && isDown && countKeyPressed == 0 && countMousePressed == 0)
            {
                xMove = 1;
            }
            else
            {
                xMove = 0;
            }

            //Смена раскладки
            if (e.KeyCode == switcherKey)
            {
                if (isDown && countKeyPressed == 0 && countMousePressed == 0)
                {
                    isSwitcher = true;
                }
                else if(isSwitcher)
                {
                    RunSwitcher.Invoke(null, null);
                }
            }
        }

        private static void MouseChange(MouseEventExtArgs e, bool isDown)
        {
            if (xMove == 2)
            {
                StopXMove(null, null);
            }
            if (xMove == 1 && e.Button == MouseButtons.Left && isDown)
            {
                xMove = 2;
                RunXMove(null, e);
                e.Handled = true;
            }
            else
            {
                xMove = 0;
            }

            if (e.Delta != 0 && !e.Handled && countKeyPressed == 0 && countMousePressed == 0)
            {
                RunXScroll.Invoke(null,e);
                e.Handled = true;
            }
        }



        private static void RegistrationHotKey(int key, int id)
        {
            
        }

        private static void AddTreeList()
        {
            
        }

        private static void GenerateEvent()
        {
            /* Нажатие (удержание)
             * Отпускания всей комбинации
             * Изменение комбинации
             */
        }
    }
}
