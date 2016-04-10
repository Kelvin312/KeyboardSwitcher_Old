using System;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
    public partial class ExceptionForm : Form
    {
        public ExceptionForm()
        {
            InitializeComponent();
            WindowSelector.UiUpdate += WindowSelector_UiUpdate;
        }
        private void ExceptionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WindowSelector.UiUpdate -= WindowSelector_UiUpdate;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        void WindowSelector_UiUpdate(object sender, WindowSelectorEventArgs e)
        {
            cmbProgramName.Text = e.ProgramName;
            txtClassName.Text = e.ClassName;
            txtWindowCaption.Text = e.WindowCaption;

            txtHandle.Text = string.Format("0x{0:X8}", e.HWindow);
            txtParent.Text = string.Format("0x{0:X8}", e.HParent);

            txtActive.Text = string.Format("0x{0:X8}", e.HActive);
            txtCaret.Text = string.Format("0x{0:X8}", e.HCaret);
            txtFocus.Text = string.Format("0x{0:X8}", e.HFocus);
            txtGetActiveWindow.Text = string.Format("0x{0:X8}", e.HGetActiveWindow);
            txtGetFocus.Text = string.Format("0x{0:X8}", e.HGetFocus);
            txtGetForegroundWindow.Text = string.Format("0x{0:X8}", e.HGetForegroundWindow);
            //txtCapture.Text = string.Format("0x{0:X8}", e.HCapture);
        }

        private void btnSelectWindow_MouseDown(object sender, MouseEventArgs e)
        {
            WindowSelector.MouseActivate(e);
        }

        private void btnSelectInput_CheckedChanged(object sender, EventArgs e)
        {
            WindowSelector.KeyboardActivate((sender as CheckBox).Checked);
            morePanel.Enabled = (sender as CheckBox).Checked;
        }
    }
}
