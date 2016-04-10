namespace KeyboardSwitcher
{
    partial class ExceptionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectProgramm = new System.Windows.Forms.Button();
            this.btnSelectWindow = new System.Windows.Forms.Button();
            this.cmbProgramName = new System.Windows.Forms.ComboBox();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtWindowCaption = new System.Windows.Forms.TextBox();
            this.txtHandle = new System.Windows.Forms.TextBox();
            this.txtParent = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGetForegroundWindow = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGetFocus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.morePanel = new System.Windows.Forms.Panel();
            this.txtCaret = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtFocus = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGetActiveWindow = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtActive = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSelectInput = new System.Windows.Forms.CheckBox();
            this.morePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Program name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Window class name:";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(231, 295);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(312, 295);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelectProgramm
            // 
            this.btnSelectProgramm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectProgramm.Location = new System.Drawing.Point(340, 179);
            this.btnSelectProgramm.Name = "btnSelectProgramm";
            this.btnSelectProgramm.Size = new System.Drawing.Size(44, 23);
            this.btnSelectProgramm.TabIndex = 4;
            this.btnSelectProgramm.Text = "...";
            this.btnSelectProgramm.UseVisualStyleBackColor = true;
            // 
            // btnSelectWindow
            // 
            this.btnSelectWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectWindow.Location = new System.Drawing.Point(340, 140);
            this.btnSelectWindow.Name = "btnSelectWindow";
            this.btnSelectWindow.Size = new System.Drawing.Size(44, 23);
            this.btnSelectWindow.TabIndex = 5;
            this.btnSelectWindow.Text = "+";
            this.btnSelectWindow.UseVisualStyleBackColor = true;
            this.btnSelectWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSelectWindow_MouseDown);
            // 
            // cmbProgramName
            // 
            this.cmbProgramName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProgramName.FormattingEnabled = true;
            this.cmbProgramName.Location = new System.Drawing.Point(12, 181);
            this.cmbProgramName.Name = "cmbProgramName";
            this.cmbProgramName.Size = new System.Drawing.Size(322, 21);
            this.cmbProgramName.TabIndex = 6;
            // 
            // txtClassName
            // 
            this.txtClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassName.Location = new System.Drawing.Point(12, 142);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(322, 20);
            this.txtClassName.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Window caption";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Handle";
            // 
            // txtWindowCaption
            // 
            this.txtWindowCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWindowCaption.Location = new System.Drawing.Point(12, 103);
            this.txtWindowCaption.Name = "txtWindowCaption";
            this.txtWindowCaption.Size = new System.Drawing.Size(322, 20);
            this.txtWindowCaption.TabIndex = 10;
            // 
            // txtHandle
            // 
            this.txtHandle.Location = new System.Drawing.Point(12, 64);
            this.txtHandle.Name = "txtHandle";
            this.txtHandle.Size = new System.Drawing.Size(113, 20);
            this.txtHandle.TabIndex = 11;
            // 
            // txtParent
            // 
            this.txtParent.Location = new System.Drawing.Point(12, 25);
            this.txtParent.Name = "txtParent";
            this.txtParent.Size = new System.Drawing.Size(113, 20);
            this.txtParent.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Parent";
            // 
            // txtGetForegroundWindow
            // 
            this.txtGetForegroundWindow.Location = new System.Drawing.Point(9, 16);
            this.txtGetForegroundWindow.Name = "txtGetForegroundWindow";
            this.txtGetForegroundWindow.Size = new System.Drawing.Size(113, 20);
            this.txtGetForegroundWindow.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "GetForegroundWindow";
            // 
            // txtGetFocus
            // 
            this.txtGetFocus.Location = new System.Drawing.Point(9, 55);
            this.txtGetFocus.Name = "txtGetFocus";
            this.txtGetFocus.Size = new System.Drawing.Size(113, 20);
            this.txtGetFocus.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "GetFocus";
            // 
            // morePanel
            // 
            this.morePanel.Controls.Add(this.txtGetActiveWindow);
            this.morePanel.Controls.Add(this.txtCaret);
            this.morePanel.Controls.Add(this.label10);
            this.morePanel.Controls.Add(this.txtActive);
            this.morePanel.Controls.Add(this.label6);
            this.morePanel.Controls.Add(this.label11);
            this.morePanel.Controls.Add(this.label8);
            this.morePanel.Controls.Add(this.txtFocus);
            this.morePanel.Controls.Add(this.txtGetForegroundWindow);
            this.morePanel.Controls.Add(this.label9);
            this.morePanel.Controls.Add(this.label7);
            this.morePanel.Controls.Add(this.txtGetFocus);
            this.morePanel.Enabled = false;
            this.morePanel.Location = new System.Drawing.Point(3, 208);
            this.morePanel.Name = "morePanel";
            this.morePanel.Size = new System.Drawing.Size(394, 81);
            this.morePanel.TabIndex = 19;
            // 
            // txtCaret
            // 
            this.txtCaret.Location = new System.Drawing.Point(271, 55);
            this.txtCaret.Name = "txtCaret";
            this.txtCaret.Size = new System.Drawing.Size(113, 20);
            this.txtCaret.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(268, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "hwndCaret";
            // 
            // txtFocus
            // 
            this.txtFocus.Location = new System.Drawing.Point(140, 55);
            this.txtFocus.Name = "txtFocus";
            this.txtFocus.Size = new System.Drawing.Size(113, 20);
            this.txtFocus.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(137, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "hwndFocus";
            // 
            // txtGetActiveWindow
            // 
            this.txtGetActiveWindow.Location = new System.Drawing.Point(271, 16);
            this.txtGetActiveWindow.Name = "txtGetActiveWindow";
            this.txtGetActiveWindow.Size = new System.Drawing.Size(113, 20);
            this.txtGetActiveWindow.TabIndex = 23;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(268, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "GetActiveWindow";
            // 
            // txtActive
            // 
            this.txtActive.Location = new System.Drawing.Point(140, 16);
            this.txtActive.Name = "txtActive";
            this.txtActive.Size = new System.Drawing.Size(113, 20);
            this.txtActive.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(137, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "hwndActive";
            // 
            // btnSelectInput
            // 
            this.btnSelectInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectInput.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelectInput.Location = new System.Drawing.Point(340, 101);
            this.btnSelectInput.Name = "btnSelectInput";
            this.btnSelectInput.Size = new System.Drawing.Size(44, 23);
            this.btnSelectInput.TabIndex = 20;
            this.btnSelectInput.Text = "Key";
            this.btnSelectInput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSelectInput.UseVisualStyleBackColor = true;
            this.btnSelectInput.CheckedChanged += new System.EventHandler(this.btnSelectInput_CheckedChanged);
            // 
            // ExceptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 329);
            this.Controls.Add(this.btnSelectInput);
            this.Controls.Add(this.morePanel);
            this.Controls.Add(this.txtParent);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtHandle);
            this.Controls.Add(this.txtWindowCaption);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.cmbProgramName);
            this.Controls.Add(this.btnSelectWindow);
            this.Controls.Add(this.btnSelectProgramm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExceptionForm";
            this.Text = "WindowInfo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExceptionForm_FormClosing);
            this.morePanel.ResumeLayout(false);
            this.morePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectProgramm;
        private System.Windows.Forms.Button btnSelectWindow;
        private System.Windows.Forms.ComboBox cmbProgramName;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWindowCaption;
        private System.Windows.Forms.TextBox txtHandle;
        private System.Windows.Forms.TextBox txtParent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGetForegroundWindow;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGetFocus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel morePanel;
        private System.Windows.Forms.TextBox txtGetActiveWindow;
        private System.Windows.Forms.TextBox txtCaret;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtActive;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtFocus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox btnSelectInput;
    }
}