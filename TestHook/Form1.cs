using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TestHook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool res;
            if (checkBox1.Checked)
            {
                res = DisableWinKey();
            }
            else
            {
                res = EnableWinKey();
            }
            if (!res) throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("WinKeyKiller.dll")]
        private static extern bool EnableWinKey();

        [DllImport("WinKeyKiller.dll")]
        private static extern bool DisableWinKey();

        private void button1_Click(object sender, EventArgs e)
        {
           var res = EnableWinKey();
           if (!res) throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }
}
