using System.ComponentModel;
using System.Windows.Forms;

namespace EnrollmentStation
{
    partial class DlgEnroll
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.llBrowseUser = new System.Windows.Forms.LinkLabel();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbInsertedYubikey = new System.Windows.Forms.GroupBox();
            this.lblAlreadyEnrolled = new System.Windows.Forms.Label();
            this.specialYubiHsm = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblInsertedFirmware = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblInsertedMode = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dlblInsertedSerial = new System.Windows.Forms.Label();
            this.lblInsertedSerial = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPinAgain = new System.Windows.Forms.TextBox();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmdEnroll = new System.Windows.Forms.Button();
            this.prgEnroll = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.drpAlgorithm = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.gbInsertedYubikey.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.llBrowseUser);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 45);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Details";
            // 
            // llBrowseUser
            // 
            this.llBrowseUser.AutoSize = true;
            this.llBrowseUser.Location = new System.Drawing.Point(297, 20);
            this.llBrowseUser.Name = "llBrowseUser";
            this.llBrowseUser.Size = new System.Drawing.Size(42, 13);
            this.llBrowseUser.TabIndex = 2;
            this.llBrowseUser.TabStop = true;
            this.llBrowseUser.Text = "Browse";
            this.llBrowseUser.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llBrowseUser_LinkClicked);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(114, 16);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(177, 20);
            this.txtUser.TabIndex = 1;
            this.txtUser.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "User";
            // 
            // gbInsertedYubikey
            // 
            this.gbInsertedYubikey.Controls.Add(this.lblAlreadyEnrolled);
            this.gbInsertedYubikey.Controls.Add(this.specialYubiHsm);
            this.gbInsertedYubikey.Controls.Add(this.label5);
            this.gbInsertedYubikey.Controls.Add(this.lblInsertedFirmware);
            this.gbInsertedYubikey.Controls.Add(this.label8);
            this.gbInsertedYubikey.Controls.Add(this.lblInsertedMode);
            this.gbInsertedYubikey.Controls.Add(this.label14);
            this.gbInsertedYubikey.Controls.Add(this.dlblInsertedSerial);
            this.gbInsertedYubikey.Controls.Add(this.lblInsertedSerial);
            this.gbInsertedYubikey.Location = new System.Drawing.Point(2, 183);
            this.gbInsertedYubikey.Name = "gbInsertedYubikey";
            this.gbInsertedYubikey.Size = new System.Drawing.Size(350, 118);
            this.gbInsertedYubikey.TabIndex = 2;
            this.gbInsertedYubikey.TabStop = false;
            this.gbInsertedYubikey.Text = "Inserted Yubikey";
            // 
            // lblAlreadyEnrolled
            // 
            this.lblAlreadyEnrolled.AutoSize = true;
            this.lblAlreadyEnrolled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlreadyEnrolled.ForeColor = System.Drawing.Color.Red;
            this.lblAlreadyEnrolled.Location = new System.Drawing.Point(114, 0);
            this.lblAlreadyEnrolled.Name = "lblAlreadyEnrolled";
            this.lblAlreadyEnrolled.Size = new System.Drawing.Size(232, 13);
            this.lblAlreadyEnrolled.TabIndex = 8;
            this.lblAlreadyEnrolled.Text = "This yubikey has already been enrolled.";
            // 
            // specialYubiHsm
            // 
            this.specialYubiHsm.AutoSize = true;
            this.specialYubiHsm.Location = new System.Drawing.Point(114, 95);
            this.specialYubiHsm.Name = "specialYubiHsm";
            this.specialYubiHsm.Size = new System.Drawing.Size(82, 13);
            this.specialYubiHsm.TabIndex = 7;
            this.specialYubiHsm.Text = "specialYubiHsm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "YubiHSM avaliable";
            // 
            // lblInsertedFirmware
            // 
            this.lblInsertedFirmware.AutoSize = true;
            this.lblInsertedFirmware.Location = new System.Drawing.Point(114, 70);
            this.lblInsertedFirmware.Name = "lblInsertedFirmware";
            this.lblInsertedFirmware.Size = new System.Drawing.Size(97, 13);
            this.lblInsertedFirmware.TabIndex = 5;
            this.lblInsertedFirmware.Text = "lblInsertedFirmware";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Firmware";
            // 
            // lblInsertedMode
            // 
            this.lblInsertedMode.AutoSize = true;
            this.lblInsertedMode.Location = new System.Drawing.Point(114, 45);
            this.lblInsertedMode.Name = "lblInsertedMode";
            this.lblInsertedMode.Size = new System.Drawing.Size(82, 13);
            this.lblInsertedMode.TabIndex = 3;
            this.lblInsertedMode.Text = "lblInsertedMode";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 45);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(71, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Current Mode";
            // 
            // dlblInsertedSerial
            // 
            this.dlblInsertedSerial.AutoSize = true;
            this.dlblInsertedSerial.Location = new System.Drawing.Point(13, 20);
            this.dlblInsertedSerial.Name = "dlblInsertedSerial";
            this.dlblInsertedSerial.Size = new System.Drawing.Size(73, 13);
            this.dlblInsertedSerial.TabIndex = 0;
            this.dlblInsertedSerial.Text = "Serial Number";
            // 
            // lblInsertedSerial
            // 
            this.lblInsertedSerial.AutoSize = true;
            this.lblInsertedSerial.Location = new System.Drawing.Point(114, 20);
            this.lblInsertedSerial.Name = "lblInsertedSerial";
            this.lblInsertedSerial.Size = new System.Drawing.Size(81, 13);
            this.lblInsertedSerial.TabIndex = 1;
            this.lblInsertedSerial.Text = "lblInsertedSerial";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtPinAgain);
            this.groupBox3.Controls.Add(this.txtPin);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(2, 54);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(350, 72);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PIN code";
            // 
            // txtPinAgain
            // 
            this.txtPinAgain.Location = new System.Drawing.Point(114, 41);
            this.txtPinAgain.Name = "txtPinAgain";
            this.txtPinAgain.PasswordChar = '*';
            this.txtPinAgain.Size = new System.Drawing.Size(177, 20);
            this.txtPinAgain.TabIndex = 3;
            this.txtPinAgain.UseSystemPasswordChar = true;
            this.txtPinAgain.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
            this.txtPinAgain.Validating += new System.ComponentModel.CancelEventHandler(this.txtPinAgain_Validating);
            // 
            // txtPin
            // 
            this.txtPin.Location = new System.Drawing.Point(114, 16);
            this.txtPin.Name = "txtPin";
            this.txtPin.PasswordChar = '*';
            this.txtPin.Size = new System.Drawing.Size(177, 20);
            this.txtPin.TabIndex = 1;
            this.txtPin.UseSystemPasswordChar = true;
            this.txtPin.TextChanged += new System.EventHandler(this.textBoxes_TextChanged);
            this.txtPin.Validating += new System.ComponentModel.CancelEventHandler(this.txtPin_Validating);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "PIN (again)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "PIN";
            // 
            // cmdEnroll
            // 
            this.cmdEnroll.Location = new System.Drawing.Point(269, 307);
            this.cmdEnroll.Name = "cmdEnroll";
            this.cmdEnroll.Size = new System.Drawing.Size(75, 23);
            this.cmdEnroll.TabIndex = 4;
            this.cmdEnroll.Text = "Enroll";
            this.cmdEnroll.UseVisualStyleBackColor = true;
            this.cmdEnroll.Click += new System.EventHandler(this.cmdEnroll_Click);
            // 
            // prgEnroll
            // 
            this.prgEnroll.Location = new System.Drawing.Point(12, 307);
            this.prgEnroll.Maximum = 15;
            this.prgEnroll.Name = "prgEnroll";
            this.prgEnroll.Size = new System.Drawing.Size(251, 23);
            this.prgEnroll.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.drpAlgorithm);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(2, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 45);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Card details";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Algorithm";
            // 
            // drpAlgorithm
            // 
            this.drpAlgorithm.FormattingEnabled = true;
            this.drpAlgorithm.Location = new System.Drawing.Point(114, 17);
            this.drpAlgorithm.Name = "drpAlgorithm";
            this.drpAlgorithm.Size = new System.Drawing.Size(177, 21);
            this.drpAlgorithm.TabIndex = 1;
            // 
            // DlgEnroll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(353, 338);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.prgEnroll);
            this.Controls.Add(this.cmdEnroll);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbInsertedYubikey);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgEnroll";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enroll new SmartCard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgEnroll_FormClosing);
            this.Load += new System.EventHandler(this.DlgEnroll_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbInsertedYubikey.ResumeLayout(false);
            this.gbInsertedYubikey.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox gbInsertedYubikey;
        private TextBox txtUser;
        private Label label2;
        private GroupBox groupBox3;
        private TextBox txtPinAgain;
        private TextBox txtPin;
        private Label label7;
        private Label label6;
        private Button cmdEnroll;
        private ProgressBar prgEnroll;
        private Label lblInsertedFirmware;
        private Label label8;
        private Label lblInsertedMode;
        private Label label14;
        private Label dlblInsertedSerial;
        private Label lblInsertedSerial;
        private LinkLabel llBrowseUser;
        private Label lblAlreadyEnrolled;
        private Label specialYubiHsm;
        private Label label5;
        private GroupBox groupBox2;
        private ComboBox drpAlgorithm;
        private Label label1;
    }
}