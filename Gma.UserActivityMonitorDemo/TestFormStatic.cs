using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

namespace Gma.UserActivityMonitorDemo
{



    public partial class TestFormStatic : Form
    {
        public TestFormStatic()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {


        }

        //##################################################################

        #region Check boxes to set or remove particular event handlers.

        private void checkBoxOnMouseMove_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnMouseMove.Checked)
            {
                HookManager.MouseMove += HookManager_MouseMove;
            }
            else
            {
                HookManager.MouseMove -= HookManager_MouseMove;
            }
        }

        private void checkBoxOnMouseClick_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnMouseClick.Checked)
            {
                HookManager.MouseClick += HookManager_MouseClick;
            }
            else
            {
                HookManager.MouseClick -= HookManager_MouseClick;
            }
        }

        private void checkBoxOnMouseUp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnMouseUp.Checked)
            {
                HookManager.MouseUp += HookManager_MouseUp;
            }
            else
            {
                HookManager.MouseUp -= HookManager_MouseUp;
            }
        }

        private void checkBoxOnMouseDown_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnMouseDown.Checked)
            {
                HookManager.MouseDown += HookManager_MouseDown;
            }
            else
            {
                HookManager.MouseDown -= HookManager_MouseDown;
            }
        }

        private void checkBoxMouseDoubleClick_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMouseDoubleClick.Checked)
            {
                HookManager.MouseDoubleClick += HookManager_MouseDoubleClick;
            }
            else
            {
                HookManager.MouseDoubleClick -= HookManager_MouseDoubleClick;
            }
        }

        private void checkBoxMouseWheel_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMouseWheel.Checked)
            {
                HookManager.MouseWheel += HookManager_MouseWheel;
            }
            else
            {
                HookManager.MouseWheel -= HookManager_MouseWheel;
            }
        }

        private void checkBoxKeyDown_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKeyDown.Checked)
            {
                HookManager.KeyDown += HookManager_KeyDown;
            }
            else
            {
                HookManager.KeyDown -= HookManager_KeyDown;
            }
        }


        private void checkBoxKeyUp_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKeyUp.Checked)
            {
                HookManager.KeyUp += HookManager_KeyUp;
            }
            else
            {
                HookManager.KeyUp -= HookManager_KeyUp;
            }
        }

        private void checkBoxKeyPress_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxKeyPress.Checked)
            {
                HookManager.KeyPress += HookManager_KeyPress;
            }
            else
            {
                HookManager.KeyPress -= HookManager_KeyPress;
            }
        }

        #endregion

        //##################################################################

        #region Event handlers of particular events. They will be activated when an appropriate checkbox is checked.

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            textBoxLog.ScrollToCaret();
        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            textBoxLog.AppendText(string.Format("KeyUp - {0}\n", e.KeyCode));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBoxLog.AppendText(string.Format("KeyPress - {0}\n", e.KeyChar));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
            labelMousePosition.Text = string.Format("x={0:0000}; y={1:0000}", e.X, e.Y);
        }

        private void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxLog.AppendText(string.Format("MouseClick - {0}\n", e.Button));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxLog.AppendText(string.Format("MouseUp - {0}\n", e.Button));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBoxLog.AppendText(string.Format("MouseDoubleClick - {0}\n", e.Button));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
            labelWheel.Text = string.Format("Wheel={0:000}", e.Delta);
        }

        #endregion



        private void checkBoxOnSetFocus_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnSetFocus.Checked)
            {

                HookManager.SetFocus += HookManager_SetFocus;
            }
            else
            {

                HookManager.SetFocus -= HookManager_SetFocus;
            }

        }

        void HookManager_SetFocus(IntPtr hWnd, IntPtr hOldWnd, ref bool handled)
        {
            textBoxLog.AppendText(string.Format("SetFocus - 0x{0:X8}\n",(int)hWnd));
            textBoxLog.ScrollToCaret();
        }



        private void checkBoxOnTaskList_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnTaskList.Checked)
            {
                HookManager.TaskList += HookManager_TaskList;
            }
            else
            {
                HookManager.TaskList -= HookManager_TaskList;
            }
        }

        void HookManager_TaskList(int lParam, ref bool handled)
        {
            textBoxLog.AppendText(string.Format("TaskList - {0}\n", lParam));
            textBoxLog.ScrollToCaret();
        }

        private void checkBoxOnActivate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnActivate.Checked)
            {
                HookManager.Activate += new ActivateEventHandler(HookManager_Activate);
            }
            else
            {
                HookManager.Activate -= new ActivateEventHandler(HookManager_Activate);
            }
        }

        void HookManager_Activate(IntPtr hWnd, bool isMouse, ref bool handled)
        {
            textBoxLog.AppendText(string.Format("Activate - 0x{0:X8} {1}\n", (int)hWnd,isMouse?"Мышью":""));
            textBoxLog.ScrollToCaret();
        }

        private void checkBoxOnClipboardChange_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOnClipboardChange.Checked)
            {
                HookManager.ClipboardChanged += new EventHandler(HookManager_ClipboardChanged);
            }
            else
            {
                HookManager.ClipboardChanged -= new EventHandler(HookManager_ClipboardChanged);
            }
        }

        void HookManager_ClipboardChanged(object sender, EventArgs e)
        {
            textBoxLog.AppendText(string.Format("Clipboard - Changed\n"));
            textBoxLog.ScrollToCaret();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxLog.Clear();
        }




    }
}