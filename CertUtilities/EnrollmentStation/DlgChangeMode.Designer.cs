namespace EnrollmentStation
{
    partial class DlgChangeMode
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpChangeMode = new System.Windows.Forms.GroupBox();
            this.chkEject = new System.Windows.Forms.CheckBox();
            this.cmdChange = new System.Windows.Forms.Button();
            this.chkU2f = new System.Windows.Forms.CheckBox();
            this.chkCCID = new System.Windows.Forms.CheckBox();
            this.chkOTP = new System.Windows.Forms.CheckBox();
            this.grpChangeMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(551, 63);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "lblStatus";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpChangeMode
            // 
            this.grpChangeMode.Controls.Add(this.chkEject);
            this.grpChangeMode.Controls.Add(this.cmdChange);
            this.grpChangeMode.Controls.Add(this.chkU2f);
            this.grpChangeMode.Controls.Add(this.chkCCID);
            this.grpChangeMode.Controls.Add(this.chkOTP);
            this.grpChangeMode.Location = new System.Drawing.Point(16, 75);
            this.grpChangeMode.Name = "grpChangeMode";
            this.grpChangeMode.Size = new System.Drawing.Size(547, 100);
            this.grpChangeMode.TabIndex = 1;
            this.grpChangeMode.TabStop = false;
            this.grpChangeMode.Text = "Change Mode";
            // 
            // chkEject
            // 
            this.chkEject.AutoSize = true;
            this.chkEject.Location = new System.Drawing.Point(6, 55);
            this.chkEject.Name = "chkEject";
            this.chkEject.Size = new System.Drawing.Size(80, 17);
            this.chkEject.TabIndex = 4;
            this.chkEject.Text = "Eject Mode";
            this.chkEject.UseVisualStyleBackColor = true;
            this.chkEject.CheckedChanged += new System.EventHandler(this.checkBox_Changed);
            // 
            // cmdChange
            // 
            this.cmdChange.Location = new System.Drawing.Point(466, 71);
            this.cmdChange.Name = "cmdChange";
            this.cmdChange.Size = new System.Drawing.Size(75, 23);
            this.cmdChange.TabIndex = 3;
            this.cmdChange.Text = "Change";
            this.cmdChange.UseVisualStyleBackColor = true;
            this.cmdChange.Click += new System.EventHandler(this.cmdChange_Click);
            // 
            // chkU2f
            // 
            this.chkU2f.AutoSize = true;
            this.chkU2f.Location = new System.Drawing.Point(461, 19);
            this.chkU2f.Name = "chkU2f";
            this.chkU2f.Size = new System.Drawing.Size(46, 17);
            this.chkU2f.TabIndex = 2;
            this.chkU2f.Text = "U2F";
            this.chkU2f.UseVisualStyleBackColor = true;
            this.chkU2f.CheckedChanged += new System.EventHandler(this.checkBox_Changed);
            // 
            // chkCCID
            // 
            this.chkCCID.AutoSize = true;
            this.chkCCID.Location = new System.Drawing.Point(228, 19);
            this.chkCCID.Name = "chkCCID";
            this.chkCCID.Size = new System.Drawing.Size(51, 17);
            this.chkCCID.TabIndex = 1;
            this.chkCCID.Text = "CCID";
            this.chkCCID.UseVisualStyleBackColor = true;
            this.chkCCID.CheckedChanged += new System.EventHandler(this.checkBox_Changed);
            // 
            // chkOTP
            // 
            this.chkOTP.AutoSize = true;
            this.chkOTP.Location = new System.Drawing.Point(6, 19);
            this.chkOTP.Name = "chkOTP";
            this.chkOTP.Size = new System.Drawing.Size(48, 17);
            this.chkOTP.TabIndex = 0;
            this.chkOTP.Text = "OTP";
            this.chkOTP.UseVisualStyleBackColor = true;
            this.chkOTP.CheckedChanged += new System.EventHandler(this.checkBox_Changed);
            // 
            // DlgChangeMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 189);
            this.Controls.Add(this.grpChangeMode);
            this.Controls.Add(this.lblStatus);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgChangeMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Mode";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgChangeMode_FormClosing);
            this.Load += new System.EventHandler(this.DlgChangeMode_Load);
            this.grpChangeMode.ResumeLayout(false);
            this.grpChangeMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpChangeMode;
        private System.Windows.Forms.Button cmdChange;
        private System.Windows.Forms.CheckBox chkU2f;
        private System.Windows.Forms.CheckBox chkCCID;
        private System.Windows.Forms.CheckBox chkOTP;
        private System.Windows.Forms.CheckBox chkEject;
    }
}