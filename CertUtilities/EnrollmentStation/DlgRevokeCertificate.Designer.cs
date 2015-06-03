namespace EnrollmentStation
{
    partial class DlgRevokeCertificate
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
            this.performanceCounter1 = new System.Diagnostics.PerformanceCounter();
            this.btnExecute = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbTerminate = new System.Windows.Forms.RadioButton();
            this.rbRevoke = new System.Windows.Forms.RadioButton();
            this.rbReset = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(268, 63);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 3;
            this.btnExecute.Text = "OK";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbTerminate);
            this.groupBox1.Controls.Add(this.rbRevoke);
            this.groupBox1.Controls.Add(this.rbReset);
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 46);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Revokation type";
            // 
            // rbTerminate
            // 
            this.rbTerminate.AutoSize = true;
            this.rbTerminate.Location = new System.Drawing.Point(239, 19);
            this.rbTerminate.Name = "rbTerminate";
            this.rbTerminate.Size = new System.Drawing.Size(72, 17);
            this.rbTerminate.TabIndex = 8;
            this.rbTerminate.TabStop = true;
            this.rbTerminate.Text = "Terminate";
            this.rbTerminate.UseVisualStyleBackColor = true;
            // 
            // rbRevoke
            // 
            this.rbRevoke.AutoSize = true;
            this.rbRevoke.Location = new System.Drawing.Point(120, 19);
            this.rbRevoke.Name = "rbRevoke";
            this.rbRevoke.Size = new System.Drawing.Size(63, 17);
            this.rbRevoke.TabIndex = 7;
            this.rbRevoke.TabStop = true;
            this.rbRevoke.Text = "Revoke";
            this.rbRevoke.UseVisualStyleBackColor = true;
            // 
            // rbReset
            // 
            this.rbReset.AutoSize = true;
            this.rbReset.Location = new System.Drawing.Point(19, 19);
            this.rbReset.Name = "rbReset";
            this.rbReset.Size = new System.Drawing.Size(53, 17);
            this.rbReset.TabIndex = 6;
            this.rbReset.TabStop = true;
            this.rbReset.Text = "Reset";
            this.rbReset.UseVisualStyleBackColor = true;
            // 
            // DlgRevokeCertificate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 96);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExecute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgRevokeCertificate";
            this.Text = "Revoke Lost Smartcards";
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Diagnostics.PerformanceCounter performanceCounter1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbTerminate;
        private System.Windows.Forms.RadioButton rbRevoke;
        private System.Windows.Forms.RadioButton rbReset;
    }
}