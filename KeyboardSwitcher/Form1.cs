using System;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
    public partial class Form1 : Form
    {
        WorkerLayer worker = new WorkerLayer();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            worker.EnableGlobalHotKey();
        }

       
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            worker.DisableGlobalHotKey();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void findWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new ExceptionForm();
            form.Show();
        }
    }
}
