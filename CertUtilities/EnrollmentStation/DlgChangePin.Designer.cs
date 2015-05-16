namespace EnrollmentStation
{
    partial class DlgChangePin
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.lblResetPossible = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.cmdClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPinOld = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPinNewAgain = new System.Windows.Forms.TextBox();
            this.txtPinNew = new System.Windows.Forms.TextBox();
            this.cmdReset = new System.Windows.Forms.Button();
            this.cmdChange = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPinTriesLeft = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPinTriesLeft);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblInstructions);
            this.groupBox1.Controls.Add(this.lblResetPossible);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblSerialNumber);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 140);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Yubikey";
            // 
            // lblInstructions
            // 
            this.lblInstructions.Location = new System.Drawing.Point(6, 103);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(328, 34);
            this.lblInstructions.TabIndex = 5;
            this.lblInstructions.Text = "lblInstructions";
            // 
            // lblResetPossible
            // 
            this.lblResetPossible.AutoSize = true;
            this.lblResetPossible.Location = new System.Drawing.Point(113, 40);
            this.lblResetPossible.Name = "lblResetPossible";
            this.lblResetPossible.Size = new System.Drawing.Size(84, 13);
            this.lblResetPossible.TabIndex = 4;
            this.lblResetPossible.Text = "lblResetPossible";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Reset?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial number";
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.AutoSize = true;
            this.lblSerialNumber.Location = new System.Drawing.Point(113, 16);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(80, 13);
            this.lblSerialNumber.TabIndex = 1;
            this.lblSerialNumber.Text = "lblSerialNumber";
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(277, 242);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 1;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtPinOld);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtPinNewAgain);
            this.groupBox2.Controls.Add(this.txtPinNew);
            this.groupBox2.Location = new System.Drawing.Point(12, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(340, 78);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PINs";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Old PIN";
            // 
            // txtPinOld
            // 
            this.txtPinOld.Location = new System.Drawing.Point(116, 19);
            this.txtPinOld.MaxLength = 8;
            this.txtPinOld.Name = "txtPinOld";
            this.txtPinOld.PasswordChar = '*';
            this.txtPinOld.Size = new System.Drawing.Size(100, 20);
            this.txtPinOld.TabIndex = 5;
            this.txtPinOld.TextChanged += new System.EventHandler(this.textField_Changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "New PIN";
            // 
            // txtPinNewAgain
            // 
            this.txtPinNewAgain.Location = new System.Drawing.Point(222, 46);
            this.txtPinNewAgain.MaxLength = 8;
            this.txtPinNewAgain.Name = "txtPinNewAgain";
            this.txtPinNewAgain.PasswordChar = '*';
            this.txtPinNewAgain.Size = new System.Drawing.Size(100, 20);
            this.txtPinNewAgain.TabIndex = 1;
            this.txtPinNewAgain.TextChanged += new System.EventHandler(this.textField_Changed);
            // 
            // txtPinNew
            // 
            this.txtPinNew.Location = new System.Drawing.Point(116, 46);
            this.txtPinNew.MaxLength = 8;
            this.txtPinNew.Name = "txtPinNew";
            this.txtPinNew.PasswordChar = '*';
            this.txtPinNew.Size = new System.Drawing.Size(100, 20);
            this.txtPinNew.TabIndex = 0;
            this.txtPinNew.TextChanged += new System.EventHandler(this.textField_Changed);
            // 
            // cmdReset
            // 
            this.cmdReset.Location = new System.Drawing.Point(196, 242);
            this.cmdReset.Name = "cmdReset";
            this.cmdReset.Size = new System.Drawing.Size(75, 23);
            this.cmdReset.TabIndex = 3;
            this.cmdReset.Text = "Reset";
            this.cmdReset.UseVisualStyleBackColor = true;
            this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
            // 
            // cmdChange
            // 
            this.cmdChange.Location = new System.Drawing.Point(115, 242);
            this.cmdChange.Name = "cmdChange";
            this.cmdChange.Size = new System.Drawing.Size(75, 23);
            this.cmdChange.TabIndex = 4;
            this.cmdChange.Text = "Change";
            this.cmdChange.UseVisualStyleBackColor = true;
            this.cmdChange.Click += new System.EventHandler(this.cmdChange_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "PIN tries left";
            // 
            // lblPinTriesLeft
            // 
            this.lblPinTriesLeft.AutoSize = true;
            this.lblPinTriesLeft.Location = new System.Drawing.Point(113, 64);
            this.lblPinTriesLeft.Name = "lblPinTriesLeft";
            this.lblPinTriesLeft.Size = new System.Drawing.Size(73, 13);
            this.lblPinTriesLeft.TabIndex = 7;
            this.lblPinTriesLeft.Text = "lblPinTriesLeft";
            // 
            // DlgChangePin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 272);
            this.Controls.Add(this.cmdChange);
            this.Controls.Add(this.cmdReset);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgChangePin";
            this.Text = "DlgChangePin";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblResetPossible;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPinNewAgain;
        private System.Windows.Forms.TextBox txtPinNew;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPinOld;
        private System.Windows.Forms.Button cmdReset;
        private System.Windows.Forms.Button cmdChange;
        private System.Windows.Forms.Label lblPinTriesLeft;
        private System.Windows.Forms.Label label5;
    }
}