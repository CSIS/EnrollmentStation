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
            this.lstItems = new System.Windows.Forms.ListView();
            this.clmSerial = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFreetext = new System.Windows.Forms.TextBox();
            this.clmUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmEnrolledAt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmCertificateSerial = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdRevoke = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDisplaySerial = new System.Windows.Forms.Label();
            this.lblDisplayEnrolled = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDisplayUser = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblDisplayCertSerial = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDisplayCertThumbprint = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblDisplayCA = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblDisplayVersionPiv = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblDisplayVersionNeo = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstItems
            // 
            this.lstItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmSerial,
            this.clmUser,
            this.clmEnrolledAt,
            this.clmCertificateSerial});
            this.lstItems.Location = new System.Drawing.Point(287, 12);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(485, 322);
            this.lstItems.TabIndex = 0;
            this.lstItems.UseCompatibleStateImageBehavior = false;
            this.lstItems.View = System.Windows.Forms.View.Details;
            this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
            // 
            // clmSerial
            // 
            this.clmSerial.Text = "Serial";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFreetext);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 322);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filters";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Textsearch";
            // 
            // txtFreetext
            // 
            this.txtFreetext.Location = new System.Drawing.Point(93, 19);
            this.txtFreetext.Name = "txtFreetext";
            this.txtFreetext.Size = new System.Drawing.Size(170, 20);
            this.txtFreetext.TabIndex = 1;
            this.txtFreetext.TextChanged += new System.EventHandler(this.txtFreetext_TextChanged);
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblDisplayVersionPiv);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.lblDisplayVersionNeo);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.lblDisplayCertSerial);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lblDisplayCertThumbprint);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.lblDisplayCA);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.lblDisplayUser);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblDisplayEnrolled);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblDisplaySerial);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmdRevoke);
            this.groupBox2.Location = new System.Drawing.Point(12, 340);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(760, 140);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Yubikey";
            // 
            // cmdRevoke
            // 
            this.cmdRevoke.Location = new System.Drawing.Point(679, 111);
            this.cmdRevoke.Name = "cmdRevoke";
            this.cmdRevoke.Size = new System.Drawing.Size(75, 23);
            this.cmdRevoke.TabIndex = 0;
            this.cmdRevoke.Text = "Revoke";
            this.cmdRevoke.UseVisualStyleBackColor = true;
            this.cmdRevoke.Click += new System.EventHandler(this.cmdRevoke_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Serial";
            // 
            // lblDisplaySerial
            // 
            this.lblDisplaySerial.AutoSize = true;
            this.lblDisplaySerial.Location = new System.Drawing.Point(90, 26);
            this.lblDisplaySerial.Name = "lblDisplaySerial";
            this.lblDisplaySerial.Size = new System.Drawing.Size(77, 13);
            this.lblDisplaySerial.TabIndex = 2;
            this.lblDisplaySerial.Text = "lblDisplaySerial";
            // 
            // lblDisplayEnrolled
            // 
            this.lblDisplayEnrolled.AutoSize = true;
            this.lblDisplayEnrolled.Location = new System.Drawing.Point(90, 51);
            this.lblDisplayEnrolled.Name = "lblDisplayEnrolled";
            this.lblDisplayEnrolled.Size = new System.Drawing.Size(89, 13);
            this.lblDisplayEnrolled.TabIndex = 4;
            this.lblDisplayEnrolled.Text = "lblDisplayEnrolled";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Enrolled";
            // 
            // lblDisplayUser
            // 
            this.lblDisplayUser.AutoSize = true;
            this.lblDisplayUser.Location = new System.Drawing.Point(90, 75);
            this.lblDisplayUser.Name = "lblDisplayUser";
            this.lblDisplayUser.Size = new System.Drawing.Size(73, 13);
            this.lblDisplayUser.TabIndex = 6;
            this.lblDisplayUser.Text = "lblDisplayUser";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "User";
            // 
            // lblDisplayCertSerial
            // 
            this.lblDisplayCertSerial.AutoSize = true;
            this.lblDisplayCertSerial.Location = new System.Drawing.Point(356, 51);
            this.lblDisplayCertSerial.Name = "lblDisplayCertSerial";
            this.lblDisplayCertSerial.Size = new System.Drawing.Size(96, 13);
            this.lblDisplayCertSerial.TabIndex = 12;
            this.lblDisplayCertSerial.Text = "lblDisplayCertSerial";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(272, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Cert Serial";
            // 
            // lblDisplayCertThumbprint
            // 
            this.lblDisplayCertThumbprint.AutoSize = true;
            this.lblDisplayCertThumbprint.Location = new System.Drawing.Point(356, 75);
            this.lblDisplayCertThumbprint.Name = "lblDisplayCertThumbprint";
            this.lblDisplayCertThumbprint.Size = new System.Drawing.Size(123, 13);
            this.lblDisplayCertThumbprint.TabIndex = 10;
            this.lblDisplayCertThumbprint.Text = "lblDisplayCertThumbprint";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(272, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Cert Thumbprint";
            // 
            // lblDisplayCA
            // 
            this.lblDisplayCA.AutoSize = true;
            this.lblDisplayCA.Location = new System.Drawing.Point(356, 26);
            this.lblDisplayCA.Name = "lblDisplayCA";
            this.lblDisplayCA.Size = new System.Drawing.Size(65, 13);
            this.lblDisplayCA.TabIndex = 8;
            this.lblDisplayCA.Text = "lblDisplayCA";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(272, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "CA";
            // 
            // lblDisplayVersionPiv
            // 
            this.lblDisplayVersionPiv.AutoSize = true;
            this.lblDisplayVersionPiv.Location = new System.Drawing.Point(676, 50);
            this.lblDisplayVersionPiv.Name = "lblDisplayVersionPiv";
            this.lblDisplayVersionPiv.Size = new System.Drawing.Size(101, 13);
            this.lblDisplayVersionPiv.TabIndex = 16;
            this.lblDisplayVersionPiv.Text = "lblDisplayVersionPiv";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(592, 50);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "PIV Version";
            // 
            // lblDisplayVersionNeo
            // 
            this.lblDisplayVersionNeo.AutoSize = true;
            this.lblDisplayVersionNeo.Location = new System.Drawing.Point(676, 26);
            this.lblDisplayVersionNeo.Name = "lblDisplayVersionNeo";
            this.lblDisplayVersionNeo.Size = new System.Drawing.Size(106, 13);
            this.lblDisplayVersionNeo.TabIndex = 14;
            this.lblDisplayVersionNeo.Text = "lblDisplayVersionNeo";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(592, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(49, 13);
            this.label17.TabIndex = 13;
            this.label17.Text = "Firmware";
            // 
            // DlgRevokeCertificate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 492);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lstItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgRevokeCertificate";
            this.Text = "Revoke Lost Smartcards";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstItems;
        private System.Windows.Forms.ColumnHeader clmSerial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFreetext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader clmUser;
        private System.Windows.Forms.ColumnHeader clmEnrolledAt;
        private System.Windows.Forms.ColumnHeader clmCertificateSerial;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdRevoke;
        private System.Windows.Forms.Label lblDisplayVersionPiv;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblDisplayVersionNeo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblDisplayCertSerial;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDisplayCertThumbprint;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblDisplayCA;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblDisplayUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblDisplayEnrolled;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDisplaySerial;
        private System.Windows.Forms.Label label2;
    }
}