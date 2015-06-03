using System.ComponentModel;
using System.Windows.Forms;

namespace EnrollmentStation
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbInsertedKey = new System.Windows.Forms.GroupBox();
            this.lblInsertedFirmware = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblInsertedMode = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dlblInsertedSerial = new System.Windows.Forms.Label();
            this.lblInsertedSerial = new System.Windows.Forms.Label();
            this.btnEnableCCID = new System.Windows.Forms.Button();
            this.btnViewCert = new System.Windows.Forms.Button();
            this.btnExportCert = new System.Windows.Forms.Button();
            this.gbSelectedKey = new System.Windows.Forms.GroupBox();
            this.lblYubikeyFirmware = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblYubikeyPivVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblYubikeySerial = new System.Windows.Forms.Label();
            this.gbSelectedKeyCertificate = new System.Windows.Forms.GroupBox();
            this.lblCertThumbprint = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblCertCA = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblCertUser = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCertEnrolledOn = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCertSerial = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lstItems = new System.Windows.Forms.ListView();
            this.clmSerial = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmEnrolledAt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmCertificateSerial = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.revokeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewCertificateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCertificateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetPINToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnEnrollKey = new System.Windows.Forms.ToolStripButton();
            this.tsbSettings = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblHSMPresent = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbInsertedKey.SuspendLayout();
            this.gbSelectedKey.SuspendLayout();
            this.gbSelectedKeyCertificate.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(996, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(0, 52);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstItems);
            this.splitContainer1.Size = new System.Drawing.Size(996, 679);
            this.splitContainer1.SplitterDistance = 332;
            this.splitContainer1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbInsertedKey);
            this.panel1.Controls.Add(this.btnEnableCCID);
            this.panel1.Controls.Add(this.btnViewCert);
            this.panel1.Controls.Add(this.btnExportCert);
            this.panel1.Controls.Add(this.gbSelectedKey);
            this.panel1.Controls.Add(this.gbSelectedKeyCertificate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 679);
            this.panel1.TabIndex = 0;
            // 
            // gbInsertedKey
            // 
            this.gbInsertedKey.Controls.Add(this.lblInsertedFirmware);
            this.gbInsertedKey.Controls.Add(this.label8);
            this.gbInsertedKey.Controls.Add(this.lblInsertedMode);
            this.gbInsertedKey.Controls.Add(this.label14);
            this.gbInsertedKey.Controls.Add(this.dlblInsertedSerial);
            this.gbInsertedKey.Controls.Add(this.lblInsertedSerial);
            this.gbInsertedKey.Location = new System.Drawing.Point(4, 282);
            this.gbInsertedKey.Name = "gbInsertedKey";
            this.gbInsertedKey.Size = new System.Drawing.Size(325, 355);
            this.gbInsertedKey.TabIndex = 30;
            this.gbInsertedKey.TabStop = false;
            this.gbInsertedKey.Text = "Inserted Yubikey";
            // 
            // lblInsertedFirmware
            // 
            this.lblInsertedFirmware.AutoSize = true;
            this.lblInsertedFirmware.Location = new System.Drawing.Point(113, 73);
            this.lblInsertedFirmware.Name = "lblInsertedFirmware";
            this.lblInsertedFirmware.Size = new System.Drawing.Size(97, 13);
            this.lblInsertedFirmware.TabIndex = 33;
            this.lblInsertedFirmware.Text = "lblInsertedFirmware";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Firmware";
            // 
            // lblInsertedMode
            // 
            this.lblInsertedMode.AutoSize = true;
            this.lblInsertedMode.Location = new System.Drawing.Point(113, 50);
            this.lblInsertedMode.Name = "lblInsertedMode";
            this.lblInsertedMode.Size = new System.Drawing.Size(82, 13);
            this.lblInsertedMode.TabIndex = 29;
            this.lblInsertedMode.Text = "lblInsertedMode";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(71, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "Current Mode";
            // 
            // dlblInsertedSerial
            // 
            this.dlblInsertedSerial.AutoSize = true;
            this.dlblInsertedSerial.Location = new System.Drawing.Point(7, 26);
            this.dlblInsertedSerial.Name = "dlblInsertedSerial";
            this.dlblInsertedSerial.Size = new System.Drawing.Size(73, 13);
            this.dlblInsertedSerial.TabIndex = 27;
            this.dlblInsertedSerial.Text = "Serial Number";
            // 
            // lblInsertedSerial
            // 
            this.lblInsertedSerial.AutoSize = true;
            this.lblInsertedSerial.Location = new System.Drawing.Point(113, 26);
            this.lblInsertedSerial.Name = "lblInsertedSerial";
            this.lblInsertedSerial.Size = new System.Drawing.Size(81, 13);
            this.lblInsertedSerial.TabIndex = 26;
            this.lblInsertedSerial.Text = "lblInsertedSerial";
            // 
            // btnEnableCCID
            // 
            this.btnEnableCCID.Enabled = false;
            this.btnEnableCCID.Location = new System.Drawing.Point(12, 643);
            this.btnEnableCCID.Name = "btnEnableCCID";
            this.btnEnableCCID.Size = new System.Drawing.Size(88, 23);
            this.btnEnableCCID.TabIndex = 29;
            this.btnEnableCCID.Text = "Enable CCID";
            this.btnEnableCCID.UseVisualStyleBackColor = true;
            this.btnEnableCCID.Click += new System.EventHandler(this.btnEnableCCID_Click);
            // 
            // btnViewCert
            // 
            this.btnViewCert.Enabled = false;
            this.btnViewCert.Location = new System.Drawing.Point(106, 643);
            this.btnViewCert.Name = "btnViewCert";
            this.btnViewCert.Size = new System.Drawing.Size(104, 23);
            this.btnViewCert.TabIndex = 28;
            this.btnViewCert.Text = "View Certificate";
            this.btnViewCert.UseVisualStyleBackColor = true;
            this.btnViewCert.Click += new System.EventHandler(this.btnViewCert_Click);
            // 
            // btnExportCert
            // 
            this.btnExportCert.Enabled = false;
            this.btnExportCert.Location = new System.Drawing.Point(216, 643);
            this.btnExportCert.Name = "btnExportCert";
            this.btnExportCert.Size = new System.Drawing.Size(104, 23);
            this.btnExportCert.TabIndex = 27;
            this.btnExportCert.Text = "Export Certificate";
            this.btnExportCert.UseVisualStyleBackColor = true;
            this.btnExportCert.Click += new System.EventHandler(this.btnExportCert_Click);
            // 
            // gbSelectedKey
            // 
            this.gbSelectedKey.Controls.Add(this.lblYubikeyFirmware);
            this.gbSelectedKey.Controls.Add(this.label17);
            this.gbSelectedKey.Controls.Add(this.label4);
            this.gbSelectedKey.Controls.Add(this.lblYubikeyPivVersion);
            this.gbSelectedKey.Controls.Add(this.label2);
            this.gbSelectedKey.Controls.Add(this.lblYubikeySerial);
            this.gbSelectedKey.Location = new System.Drawing.Point(3, 3);
            this.gbSelectedKey.Name = "gbSelectedKey";
            this.gbSelectedKey.Size = new System.Drawing.Size(326, 122);
            this.gbSelectedKey.TabIndex = 22;
            this.gbSelectedKey.TabStop = false;
            this.gbSelectedKey.Text = "YubiKey";
            // 
            // lblYubikeyFirmware
            // 
            this.lblYubikeyFirmware.AutoSize = true;
            this.lblYubikeyFirmware.Location = new System.Drawing.Point(113, 92);
            this.lblYubikeyFirmware.Name = "lblYubikeyFirmware";
            this.lblYubikeyFirmware.Size = new System.Drawing.Size(97, 13);
            this.lblYubikeyFirmware.TabIndex = 25;
            this.lblYubikeyFirmware.Text = "lblYubikeyFirmware";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 92);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(49, 13);
            this.label17.TabIndex = 24;
            this.label17.Text = "Firmware";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "PIV Version";
            // 
            // lblYubikeyPivVersion
            // 
            this.lblYubikeyPivVersion.AutoSize = true;
            this.lblYubikeyPivVersion.Location = new System.Drawing.Point(113, 68);
            this.lblYubikeyPivVersion.Name = "lblYubikeyPivVersion";
            this.lblYubikeyPivVersion.Size = new System.Drawing.Size(105, 13);
            this.lblYubikeyPivVersion.TabIndex = 22;
            this.lblYubikeyPivVersion.Text = "lblYubikeyPivVersion";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Serial Number";
            // 
            // lblYubikeySerial
            // 
            this.lblYubikeySerial.AutoSize = true;
            this.lblYubikeySerial.Location = new System.Drawing.Point(113, 20);
            this.lblYubikeySerial.Name = "lblYubikeySerial";
            this.lblYubikeySerial.Size = new System.Drawing.Size(81, 13);
            this.lblYubikeySerial.TabIndex = 18;
            this.lblYubikeySerial.Text = "lblYubikeySerial";
            // 
            // gbSelectedKeyCertificate
            // 
            this.gbSelectedKeyCertificate.Controls.Add(this.lblCertThumbprint);
            this.gbSelectedKeyCertificate.Controls.Add(this.label11);
            this.gbSelectedKeyCertificate.Controls.Add(this.lblCertCA);
            this.gbSelectedKeyCertificate.Controls.Add(this.label13);
            this.gbSelectedKeyCertificate.Controls.Add(this.lblCertUser);
            this.gbSelectedKeyCertificate.Controls.Add(this.label7);
            this.gbSelectedKeyCertificate.Controls.Add(this.lblCertEnrolledOn);
            this.gbSelectedKeyCertificate.Controls.Add(this.label5);
            this.gbSelectedKeyCertificate.Controls.Add(this.lblCertSerial);
            this.gbSelectedKeyCertificate.Controls.Add(this.label6);
            this.gbSelectedKeyCertificate.Location = new System.Drawing.Point(3, 131);
            this.gbSelectedKeyCertificate.Name = "gbSelectedKeyCertificate";
            this.gbSelectedKeyCertificate.Size = new System.Drawing.Size(326, 145);
            this.gbSelectedKeyCertificate.TabIndex = 21;
            this.gbSelectedKeyCertificate.TabStop = false;
            this.gbSelectedKeyCertificate.Text = "Certificate";
            // 
            // lblCertThumbprint
            // 
            this.lblCertThumbprint.AutoSize = true;
            this.lblCertThumbprint.Location = new System.Drawing.Point(113, 115);
            this.lblCertThumbprint.Name = "lblCertThumbprint";
            this.lblCertThumbprint.Size = new System.Drawing.Size(89, 13);
            this.lblCertThumbprint.TabIndex = 10;
            this.lblCertThumbprint.Text = "lblCertThumbprint";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 115);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Thumbprint";
            // 
            // lblCertCA
            // 
            this.lblCertCA.AutoSize = true;
            this.lblCertCA.Location = new System.Drawing.Point(113, 92);
            this.lblCertCA.Name = "lblCertCA";
            this.lblCertCA.Size = new System.Drawing.Size(50, 13);
            this.lblCertCA.TabIndex = 8;
            this.lblCertCA.Text = "lblCertCA";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 92);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "CA";
            // 
            // lblCertUser
            // 
            this.lblCertUser.AutoSize = true;
            this.lblCertUser.Location = new System.Drawing.Point(113, 16);
            this.lblCertUser.Name = "lblCertUser";
            this.lblCertUser.Size = new System.Drawing.Size(58, 13);
            this.lblCertUser.TabIndex = 6;
            this.lblCertUser.Text = "lblCertUser";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "User";
            // 
            // lblCertEnrolledOn
            // 
            this.lblCertEnrolledOn.AutoSize = true;
            this.lblCertEnrolledOn.Location = new System.Drawing.Point(113, 68);
            this.lblCertEnrolledOn.Name = "lblCertEnrolledOn";
            this.lblCertEnrolledOn.Size = new System.Drawing.Size(88, 13);
            this.lblCertEnrolledOn.TabIndex = 4;
            this.lblCertEnrolledOn.Text = "lblCertEnrolledOn";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Enrolled on";
            // 
            // lblCertSerial
            // 
            this.lblCertSerial.AutoSize = true;
            this.lblCertSerial.Location = new System.Drawing.Point(113, 42);
            this.lblCertSerial.Name = "lblCertSerial";
            this.lblCertSerial.Size = new System.Drawing.Size(62, 13);
            this.lblCertSerial.TabIndex = 2;
            this.lblCertSerial.Text = "lblCertSerial";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Serial";
            // 
            // lstItems
            // 
            this.lstItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmSerial,
            this.clmUser,
            this.clmEnrolledAt,
            this.clmCertificateSerial});
            this.lstItems.ContextMenuStrip = this.contextMenuStrip1;
            this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstItems.FullRowSelect = true;
            this.lstItems.Location = new System.Drawing.Point(0, 0);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(660, 679);
            this.lstItems.TabIndex = 1;
            this.lstItems.UseCompatibleStateImageBehavior = false;
            this.lstItems.View = System.Windows.Forms.View.Details;
            this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
            // 
            // clmSerial
            // 
            this.clmSerial.Text = "Serial";
            // 
            // clmUser
            // 
            this.clmUser.Text = "User";
            this.clmUser.Width = 106;
            // 
            // clmEnrolledAt
            // 
            this.clmEnrolledAt.Text = "Enrolled (local time)";
            this.clmEnrolledAt.Width = 118;
            // 
            // clmCertificateSerial
            // 
            this.clmCertificateSerial.Text = "Certificate";
            this.clmCertificateSerial.Width = 153;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.revokeToolStripMenuItem,
            this.viewCertificateToolStripMenuItem,
            this.exportCertificateToolStripMenuItem,
            this.resetPINToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 92);
            // 
            // revokeToolStripMenuItem
            // 
            this.revokeToolStripMenuItem.Name = "revokeToolStripMenuItem";
            this.revokeToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.revokeToolStripMenuItem.Text = "Revoke";
            this.revokeToolStripMenuItem.Click += new System.EventHandler(this.revokeToolStripMenuItem_Click);
            // 
            // viewCertificateToolStripMenuItem
            // 
            this.viewCertificateToolStripMenuItem.Name = "viewCertificateToolStripMenuItem";
            this.viewCertificateToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.viewCertificateToolStripMenuItem.Text = "View Certificate";
            this.viewCertificateToolStripMenuItem.Click += new System.EventHandler(this.viewCertificateToolStripMenuItem_Click);
            // 
            // exportCertificateToolStripMenuItem
            // 
            this.exportCertificateToolStripMenuItem.Name = "exportCertificateToolStripMenuItem";
            this.exportCertificateToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.exportCertificateToolStripMenuItem.Text = "Export Certificate";
            this.exportCertificateToolStripMenuItem.Click += new System.EventHandler(this.exportCertificateToolStripMenuItem_Click);
            // 
            // resetPINToolStripMenuItem
            // 
            this.resetPINToolStripMenuItem.Name = "resetPINToolStripMenuItem";
            this.resetPINToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.resetPINToolStripMenuItem.Text = "Reset PIN";
            this.resetPINToolStripMenuItem.Click += new System.EventHandler(this.resetPINToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEnrollKey,
            this.tsbSettings});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(996, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnEnrollKey
            // 
            this.btnEnrollKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEnrollKey.Image = ((System.Drawing.Image)(resources.GetObject("btnEnrollKey.Image")));
            this.btnEnrollKey.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEnrollKey.Name = "btnEnrollKey";
            this.btnEnrollKey.Size = new System.Drawing.Size(23, 22);
            this.btnEnrollKey.Text = "Enroll Yubikey";
            this.btnEnrollKey.Click += new System.EventHandler(this.btnEnrollKey_Click);
            // 
            // tsbSettings
            // 
            this.tsbSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(23, 22);
            this.tsbSettings.Text = "Show settings";
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHSMPresent});
            this.statusStrip1.Location = new System.Drawing.Point(0, 734);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(996, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblHSMPresent
            // 
            this.lblHSMPresent.Name = "lblHSMPresent";
            this.lblHSMPresent.Size = new System.Drawing.Size(85, 17);
            this.lblHSMPresent.Text = "lblHSMPresent";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 756);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbInsertedKey.ResumeLayout(false);
            this.gbInsertedKey.PerformLayout();
            this.gbSelectedKey.ResumeLayout(false);
            this.gbSelectedKey.PerformLayout();
            this.gbSelectedKeyCertificate.ResumeLayout(false);
            this.gbSelectedKeyCertificate.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private SplitContainer splitContainer1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem revokeToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton btnEnrollKey;
        private ToolStripButton tsbSettings;
        private Panel panel1;
        private ToolStripMenuItem viewCertificateToolStripMenuItem;
        private ToolStripMenuItem exportCertificateToolStripMenuItem;
        private GroupBox gbSelectedKeyCertificate;
        private Label lblCertThumbprint;
        private Label label11;
        private Label lblCertCA;
        private Label label13;
        private Label lblCertUser;
        private Label label7;
        private Label lblCertEnrolledOn;
        private Label label5;
        private Label lblCertSerial;
        private Label label6;
        private ColumnHeader clmSerial;
        private ColumnHeader clmUser;
        private ColumnHeader clmEnrolledAt;
        private ColumnHeader clmCertificateSerial;
        private GroupBox gbSelectedKey;
        private Label lblYubikeyFirmware;
        private Label label17;
        private Label label4;
        private Label lblYubikeyPivVersion;
        private Label label2;
        private Label lblYubikeySerial;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblHSMPresent;
        private Button btnViewCert;
        private Button btnExportCert;
        private Button btnEnableCCID;
        private GroupBox gbInsertedKey;
        private Label lblInsertedFirmware;
        private Label label8;
        private Label lblInsertedMode;
        private Label label14;
        private Label dlblInsertedSerial;
        private Label lblInsertedSerial;
        private ListView lstItems;
        private ToolStripMenuItem resetPINToolStripMenuItem;
    }
}