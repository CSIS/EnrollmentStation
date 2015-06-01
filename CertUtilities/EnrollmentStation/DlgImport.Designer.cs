namespace EnrollmentStation
{
    partial class DlgImport
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
            this.cmdImport = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPuk = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtManagement = new System.Windows.Forms.TextBox();
            this.cmdClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCertificateSubject = new System.Windows.Forms.Label();
            this.lblCertificateSerial = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCa = new System.Windows.Forms.LinkLabel();
            this.cmdViewCertificate = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdImport
            // 
            this.cmdImport.Location = new System.Drawing.Point(508, 242);
            this.cmdImport.Name = "cmdImport";
            this.cmdImport.Size = new System.Drawing.Size(75, 23);
            this.cmdImport.TabIndex = 9;
            this.cmdImport.Text = "Import";
            this.cmdImport.UseVisualStyleBackColor = true;
            this.cmdImport.Click += new System.EventHandler(this.cmdImport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCa);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtUser);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtPuk);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtManagement);
            this.groupBox2.Location = new System.Drawing.Point(12, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(652, 78);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Keys";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(421, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Username";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(485, 19);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(161, 20);
            this.txtUser.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "PUK";
            // 
            // txtPuk
            // 
            this.txtPuk.Location = new System.Drawing.Point(116, 19);
            this.txtPuk.MaxLength = 8;
            this.txtPuk.Name = "txtPuk";
            this.txtPuk.Size = new System.Drawing.Size(112, 20);
            this.txtPuk.TabIndex = 5;
            this.txtPuk.TextChanged += new System.EventHandler(this.txtPuk_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Management Key";
            // 
            // txtManagement
            // 
            this.txtManagement.Location = new System.Drawing.Point(116, 46);
            this.txtManagement.MaxLength = 48;
            this.txtManagement.Name = "txtManagement";
            this.txtManagement.Size = new System.Drawing.Size(299, 20);
            this.txtManagement.TabIndex = 0;
            this.txtManagement.TextChanged += new System.EventHandler(this.txtManagement_TextChanged);
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(589, 242);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 6;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCertificateSubject);
            this.groupBox1.Controls.Add(this.lblCertificateSerial);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblInstructions);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblSerialNumber);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(652, 140);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Yubikey";
            // 
            // lblCertificateSubject
            // 
            this.lblCertificateSubject.AutoSize = true;
            this.lblCertificateSubject.Location = new System.Drawing.Point(113, 64);
            this.lblCertificateSubject.Name = "lblCertificateSubject";
            this.lblCertificateSubject.Size = new System.Drawing.Size(100, 13);
            this.lblCertificateSubject.TabIndex = 10;
            this.lblCertificateSubject.Text = "lblCertificateSubject";
            // 
            // lblCertificateSerial
            // 
            this.lblCertificateSerial.AutoSize = true;
            this.lblCertificateSerial.Location = new System.Drawing.Point(113, 40);
            this.lblCertificateSerial.Name = "lblCertificateSerial";
            this.lblCertificateSerial.Size = new System.Drawing.Size(90, 13);
            this.lblCertificateSerial.TabIndex = 9;
            this.lblCertificateSerial.Text = "lblCertificateSerial";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Certificate";
            // 
            // lblInstructions
            // 
            this.lblInstructions.Location = new System.Drawing.Point(6, 103);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(328, 34);
            this.lblInstructions.TabIndex = 5;
            this.lblInstructions.Text = "lblInstructions";
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(421, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "CA";
            // 
            // lblCa
            // 
            this.lblCa.AutoSize = true;
            this.lblCa.Location = new System.Drawing.Point(482, 49);
            this.lblCa.Name = "lblCa";
            this.lblCa.Size = new System.Drawing.Size(30, 13);
            this.lblCa.TabIndex = 10;
            this.lblCa.TabStop = true;
            this.lblCa.Text = "lblCa";
            this.lblCa.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblCa_LinkClicked);
            // 
            // cmdViewCertificate
            // 
            this.cmdViewCertificate.Location = new System.Drawing.Point(12, 242);
            this.cmdViewCertificate.Name = "cmdViewCertificate";
            this.cmdViewCertificate.Size = new System.Drawing.Size(96, 23);
            this.cmdViewCertificate.TabIndex = 11;
            this.cmdViewCertificate.Text = "View Certificate";
            this.cmdViewCertificate.UseVisualStyleBackColor = true;
            this.cmdViewCertificate.Click += new System.EventHandler(this.cmdViewCertificate_Click);
            // 
            // DlgImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 273);
            this.Controls.Add(this.cmdViewCertificate);
            this.Controls.Add(this.cmdImport);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import smartcard";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdImport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPuk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtManagement;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.Label lblCertificateSubject;
        private System.Windows.Forms.Label lblCertificateSerial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.LinkLabel lblCa;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cmdViewCertificate;
    }
}