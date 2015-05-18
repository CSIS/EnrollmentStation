namespace EnrollmentStation
{
    partial class MainForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdYubikeyReset = new System.Windows.Forms.Button();
            this.cmdYubikeyTerminate = new System.Windows.Forms.Button();
            this.cmdYubikeyEnroll = new System.Windows.Forms.Button();
            this.cmdViewCertificate = new System.Windows.Forms.Button();
            this.lblYubikeyEnrollState = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdExportCertificate = new System.Windows.Forms.Button();
            this.lblYubikeyCertificateIssuer = new System.Windows.Forms.Label();
            this.lblYubikeyCertificateSubject = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblYubikeyPivVersion = new System.Windows.Forms.Label();
            this.cmdEnableCcid = new System.Windows.Forms.Button();
            this.lblYubikeyStatus = new System.Windows.Forms.Label();
            this.lblYubikeyMode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdYubikeyRefresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblYubikeySerial = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revokeLostSmartcardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeResetPINToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdExit = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdExit);
            this.groupBox2.Controls.Add(this.cmdYubikeyReset);
            this.groupBox2.Controls.Add(this.cmdYubikeyTerminate);
            this.groupBox2.Controls.Add(this.cmdYubikeyEnroll);
            this.groupBox2.Controls.Add(this.cmdViewCertificate);
            this.groupBox2.Controls.Add(this.lblYubikeyEnrollState);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmdExportCertificate);
            this.groupBox2.Controls.Add(this.lblYubikeyCertificateIssuer);
            this.groupBox2.Controls.Add(this.lblYubikeyCertificateSubject);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lblYubikeyPivVersion);
            this.groupBox2.Controls.Add(this.cmdEnableCcid);
            this.groupBox2.Controls.Add(this.lblYubikeyStatus);
            this.groupBox2.Controls.Add(this.lblYubikeyMode);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmdYubikeyRefresh);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lblYubikeySerial);
            this.groupBox2.Location = new System.Drawing.Point(12, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(704, 218);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Yubikey Details";
            // 
            // cmdYubikeyReset
            // 
            this.cmdYubikeyReset.Location = new System.Drawing.Point(623, 103);
            this.cmdYubikeyReset.Name = "cmdYubikeyReset";
            this.cmdYubikeyReset.Size = new System.Drawing.Size(75, 23);
            this.cmdYubikeyReset.TabIndex = 18;
            this.cmdYubikeyReset.Text = "Reset";
            this.cmdYubikeyReset.UseVisualStyleBackColor = true;
            this.cmdYubikeyReset.Click += new System.EventHandler(this.cmdYubikeyReset_Click);
            // 
            // cmdYubikeyTerminate
            // 
            this.cmdYubikeyTerminate.Location = new System.Drawing.Point(623, 74);
            this.cmdYubikeyTerminate.Name = "cmdYubikeyTerminate";
            this.cmdYubikeyTerminate.Size = new System.Drawing.Size(75, 23);
            this.cmdYubikeyTerminate.TabIndex = 17;
            this.cmdYubikeyTerminate.Text = "Terminate";
            this.cmdYubikeyTerminate.UseVisualStyleBackColor = true;
            this.cmdYubikeyTerminate.Click += new System.EventHandler(this.cmdYubikeyTerminate_Click);
            // 
            // cmdYubikeyEnroll
            // 
            this.cmdYubikeyEnroll.Location = new System.Drawing.Point(623, 46);
            this.cmdYubikeyEnroll.Name = "cmdYubikeyEnroll";
            this.cmdYubikeyEnroll.Size = new System.Drawing.Size(75, 23);
            this.cmdYubikeyEnroll.TabIndex = 16;
            this.cmdYubikeyEnroll.Text = "Enroll";
            this.cmdYubikeyEnroll.UseVisualStyleBackColor = true;
            this.cmdYubikeyEnroll.Click += new System.EventHandler(this.cmdYubikeyEnroll_Click);
            // 
            // cmdViewCertificate
            // 
            this.cmdViewCertificate.Location = new System.Drawing.Point(209, 158);
            this.cmdViewCertificate.Name = "cmdViewCertificate";
            this.cmdViewCertificate.Size = new System.Drawing.Size(101, 23);
            this.cmdViewCertificate.TabIndex = 15;
            this.cmdViewCertificate.Text = "View Certificate";
            this.cmdViewCertificate.UseVisualStyleBackColor = true;
            this.cmdViewCertificate.Click += new System.EventHandler(this.cmdYubikeyViewCertificate_Click);
            // 
            // lblYubikeyEnrollState
            // 
            this.lblYubikeyEnrollState.AutoSize = true;
            this.lblYubikeyEnrollState.Location = new System.Drawing.Point(516, 51);
            this.lblYubikeyEnrollState.Name = "lblYubikeyEnrollState";
            this.lblYubikeyEnrollState.Size = new System.Drawing.Size(106, 13);
            this.lblYubikeyEnrollState.TabIndex = 14;
            this.lblYubikeyEnrollState.Text = "lblYubikeyEnrollState";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(414, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Previously Enrolled";
            // 
            // cmdExportCertificate
            // 
            this.cmdExportCertificate.Location = new System.Drawing.Point(102, 158);
            this.cmdExportCertificate.Name = "cmdExportCertificate";
            this.cmdExportCertificate.Size = new System.Drawing.Size(101, 23);
            this.cmdExportCertificate.TabIndex = 12;
            this.cmdExportCertificate.Text = "Export Certificate";
            this.cmdExportCertificate.UseVisualStyleBackColor = true;
            this.cmdExportCertificate.Click += new System.EventHandler(this.cmdExportCertificate_Click);
            // 
            // lblYubikeyCertificateIssuer
            // 
            this.lblYubikeyCertificateIssuer.AutoSize = true;
            this.lblYubikeyCertificateIssuer.Location = new System.Drawing.Point(88, 126);
            this.lblYubikeyCertificateIssuer.Name = "lblYubikeyCertificateIssuer";
            this.lblYubikeyCertificateIssuer.Size = new System.Drawing.Size(130, 13);
            this.lblYubikeyCertificateIssuer.TabIndex = 11;
            this.lblYubikeyCertificateIssuer.Text = "lblYubikeyCertificateIssuer";
            // 
            // lblYubikeyCertificateSubject
            // 
            this.lblYubikeyCertificateSubject.AutoSize = true;
            this.lblYubikeyCertificateSubject.Location = new System.Drawing.Point(88, 103);
            this.lblYubikeyCertificateSubject.Name = "lblYubikeyCertificateSubject";
            this.lblYubikeyCertificateSubject.Size = new System.Drawing.Size(138, 13);
            this.lblYubikeyCertificateSubject.TabIndex = 10;
            this.lblYubikeyCertificateSubject.Text = "lblYubikeyCertificateSubject";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Certificate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "PIV Version";
            // 
            // lblYubikeyPivVersion
            // 
            this.lblYubikeyPivVersion.AutoSize = true;
            this.lblYubikeyPivVersion.Location = new System.Drawing.Point(88, 79);
            this.lblYubikeyPivVersion.Name = "lblYubikeyPivVersion";
            this.lblYubikeyPivVersion.Size = new System.Drawing.Size(105, 13);
            this.lblYubikeyPivVersion.TabIndex = 7;
            this.lblYubikeyPivVersion.Text = "lblYubikeyPivVersion";
            // 
            // cmdEnableCcid
            // 
            this.cmdEnableCcid.Location = new System.Drawing.Point(9, 158);
            this.cmdEnableCcid.Name = "cmdEnableCcid";
            this.cmdEnableCcid.Size = new System.Drawing.Size(85, 23);
            this.cmdEnableCcid.TabIndex = 6;
            this.cmdEnableCcid.Text = "Enable CCID";
            this.cmdEnableCcid.UseVisualStyleBackColor = true;
            this.cmdEnableCcid.Click += new System.EventHandler(this.cmdEnableCcid_Click);
            // 
            // lblYubikeyStatus
            // 
            this.lblYubikeyStatus.AutoSize = true;
            this.lblYubikeyStatus.Location = new System.Drawing.Point(6, 194);
            this.lblYubikeyStatus.Name = "lblYubikeyStatus";
            this.lblYubikeyStatus.Size = new System.Drawing.Size(85, 13);
            this.lblYubikeyStatus.TabIndex = 5;
            this.lblYubikeyStatus.Text = "lblYubikeyStatus";
            // 
            // lblYubikeyMode
            // 
            this.lblYubikeyMode.AutoSize = true;
            this.lblYubikeyMode.Location = new System.Drawing.Point(88, 51);
            this.lblYubikeyMode.Name = "lblYubikeyMode";
            this.lblYubikeyMode.Size = new System.Drawing.Size(82, 13);
            this.lblYubikeyMode.TabIndex = 4;
            this.lblYubikeyMode.Text = "lblYubikeyMode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current Mode";
            // 
            // cmdYubikeyRefresh
            // 
            this.cmdYubikeyRefresh.Location = new System.Drawing.Point(623, 19);
            this.cmdYubikeyRefresh.Name = "cmdYubikeyRefresh";
            this.cmdYubikeyRefresh.Size = new System.Drawing.Size(75, 23);
            this.cmdYubikeyRefresh.TabIndex = 2;
            this.cmdYubikeyRefresh.Text = "Refresh";
            this.cmdYubikeyRefresh.UseVisualStyleBackColor = true;
            this.cmdYubikeyRefresh.Click += new System.EventHandler(this.cmdYubikeyRefresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Serial Number";
            // 
            // lblYubikeySerial
            // 
            this.lblYubikeySerial.AutoSize = true;
            this.lblYubikeySerial.Location = new System.Drawing.Point(88, 24);
            this.lblYubikeySerial.Name = "lblYubikeySerial";
            this.lblYubikeySerial.Size = new System.Drawing.Size(81, 13);
            this.lblYubikeySerial.TabIndex = 0;
            this.lblYubikeySerial.Text = "lblYubikeySerial";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(727, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.revokeLostSmartcardToolStripMenuItem,
            this.changeResetPINToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // revokeLostSmartcardToolStripMenuItem
            // 
            this.revokeLostSmartcardToolStripMenuItem.Name = "revokeLostSmartcardToolStripMenuItem";
            this.revokeLostSmartcardToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.revokeLostSmartcardToolStripMenuItem.Text = "Revoke lost smartcard";
            this.revokeLostSmartcardToolStripMenuItem.Click += new System.EventHandler(this.revokeLostSmartcardToolStripMenuItem_Click);
            // 
            // changeResetPINToolStripMenuItem
            // 
            this.changeResetPINToolStripMenuItem.Name = "changeResetPINToolStripMenuItem";
            this.changeResetPINToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.changeResetPINToolStripMenuItem.Text = "Change/Reset PIN";
            this.changeResetPINToolStripMenuItem.Click += new System.EventHandler(this.changeResetPINToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hSMToolStripMenuItem,
            this.configureToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // hSMToolStripMenuItem
            // 
            this.hSMToolStripMenuItem.Name = "hSMToolStripMenuItem";
            this.hSMToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.hSMToolStripMenuItem.Text = "HSM";
            this.hSMToolStripMenuItem.Click += new System.EventHandler(this.hSMToolStripMenuItem_Click);
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.configureToolStripMenuItem.Text = "Configure";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(623, 189);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(75, 23);
            this.cmdExit.TabIndex = 19;
            this.cmdExit.TabStop = false;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 257);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "CSIS Enrollment Station";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hSMToolStripMenuItem;
        private System.Windows.Forms.Label lblYubikeySerial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdYubikeyRefresh;
        private System.Windows.Forms.Label lblYubikeyMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblYubikeyStatus;
        private System.Windows.Forms.Button cmdEnableCcid;
        private System.Windows.Forms.Label lblYubikeyPivVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblYubikeyCertificateSubject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblYubikeyCertificateIssuer;
        private System.Windows.Forms.Button cmdExportCertificate;
        private System.Windows.Forms.Label lblYubikeyEnrollState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdViewCertificate;
        private System.Windows.Forms.Button cmdYubikeyEnroll;
        private System.Windows.Forms.Button cmdYubikeyTerminate;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revokeLostSmartcardToolStripMenuItem;
        private System.Windows.Forms.Button cmdYubikeyReset;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeResetPINToolStripMenuItem;
        private System.Windows.Forms.Button cmdExit;
    }
}

