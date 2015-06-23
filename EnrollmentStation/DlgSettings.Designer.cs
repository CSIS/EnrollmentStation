using System.ComponentModel;
using System.Windows.Forms;

namespace EnrollmentStation
{
    partial class DlgSettings
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
            this.txtAgentCert = new System.Windows.Forms.TextBox();
            this.llBrowseAgentCert = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCSREndpoint = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCaTemplate = new System.Windows.Forms.TextBox();
            this.llBrowseCA = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblHSMAvaliable = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.llGenerateMgtKey = new System.Windows.Forms.LinkLabel();
            this.txtManagementKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAgentCert);
            this.groupBox1.Controls.Add(this.llBrowseAgentCert);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCSREndpoint);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCaTemplate);
            this.groupBox1.Controls.Add(this.llBrowseCA);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CA Settings";
            // 
            // txtAgentCert
            // 
            this.txtAgentCert.BackColor = System.Drawing.SystemColors.Control;
            this.txtAgentCert.Location = new System.Drawing.Point(119, 39);
            this.txtAgentCert.Name = "txtAgentCert";
            this.txtAgentCert.ReadOnly = true;
            this.txtAgentCert.Size = new System.Drawing.Size(317, 20);
            this.txtAgentCert.TabIndex = 4;
            this.txtAgentCert.Validating += new System.ComponentModel.CancelEventHandler(this.txtAgentCert_Validating);
            // 
            // llBrowseAgentCert
            // 
            this.llBrowseAgentCert.AutoSize = true;
            this.llBrowseAgentCert.Location = new System.Drawing.Point(442, 43);
            this.llBrowseAgentCert.Name = "llBrowseAgentCert";
            this.llBrowseAgentCert.Size = new System.Drawing.Size(42, 13);
            this.llBrowseAgentCert.TabIndex = 5;
            this.llBrowseAgentCert.TabStop = true;
            this.llBrowseAgentCert.Text = "Browse";
            this.llBrowseAgentCert.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llBrowseAgentCert_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Agent certificate";
            // 
            // txtCSREndpoint
            // 
            this.txtCSREndpoint.Location = new System.Drawing.Point(119, 14);
            this.txtCSREndpoint.Name = "txtCSREndpoint";
            this.txtCSREndpoint.Size = new System.Drawing.Size(317, 20);
            this.txtCSREndpoint.TabIndex = 1;
            this.txtCSREndpoint.Validating += new System.ComponentModel.CancelEventHandler(this.txtCSREndpoint_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Cert template";
            // 
            // txtCaTemplate
            // 
            this.txtCaTemplate.Location = new System.Drawing.Point(119, 64);
            this.txtCaTemplate.Name = "txtCaTemplate";
            this.txtCaTemplate.Size = new System.Drawing.Size(317, 20);
            this.txtCaTemplate.TabIndex = 7;
            this.txtCaTemplate.Text = "SmartcardLogon";
            this.txtCaTemplate.Validating += new System.ComponentModel.CancelEventHandler(this.txtCaTemplate_Validating);
            // 
            // llBrowseCA
            // 
            this.llBrowseCA.AutoSize = true;
            this.llBrowseCA.Location = new System.Drawing.Point(442, 18);
            this.llBrowseCA.Name = "llBrowseCA";
            this.llBrowseCA.Size = new System.Drawing.Size(42, 13);
            this.llBrowseCA.TabIndex = 2;
            this.llBrowseCA.TabStop = true;
            this.llBrowseCA.Text = "Browse";
            this.llBrowseCA.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llBrowseCA_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "CSR endpoint";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(340, 177);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(421, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblHSMAvaliable);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.llGenerateMgtKey);
            this.groupBox3.Controls.Add(this.txtManagementKey);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(504, 69);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "YubiKey";
            // 
            // lblHSMAvaliable
            // 
            this.lblHSMAvaliable.AutoSize = true;
            this.lblHSMAvaliable.Location = new System.Drawing.Point(120, 19);
            this.lblHSMAvaliable.Name = "lblHSMAvaliable";
            this.lblHSMAvaliable.Size = new System.Drawing.Size(84, 13);
            this.lblHSMAvaliable.TabIndex = 1;
            this.lblHSMAvaliable.Text = "lblHSMAvaliable";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "YubiHSM avaliable";
            // 
            // llGenerateMgtKey
            // 
            this.llGenerateMgtKey.AutoSize = true;
            this.llGenerateMgtKey.Location = new System.Drawing.Point(443, 43);
            this.llGenerateMgtKey.Name = "llGenerateMgtKey";
            this.llGenerateMgtKey.Size = new System.Drawing.Size(51, 13);
            this.llGenerateMgtKey.TabIndex = 4;
            this.llGenerateMgtKey.TabStop = true;
            this.llGenerateMgtKey.Text = "Generate";
            this.llGenerateMgtKey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llGenerateMgtKey_LinkClicked);
            // 
            // txtManagementKey
            // 
            this.txtManagementKey.Location = new System.Drawing.Point(120, 39);
            this.txtManagementKey.MaxLength = 48;
            this.txtManagementKey.Name = "txtManagementKey";
            this.txtManagementKey.Size = new System.Drawing.Size(317, 20);
            this.txtManagementKey.TabIndex = 3;
            this.txtManagementKey.Validating += new System.ComponentModel.CancelEventHandler(this.txtManagementKey_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Management key";
            // 
            // DlgSettings
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(508, 208);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.DlgSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private LinkLabel llBrowseCA;
        private Label label1;
        private Button btnSave;
        private Button btnCancel;
        private TextBox txtCSREndpoint;
        private GroupBox groupBox3;
        private TextBox txtManagementKey;
        private Label label4;
        private TextBox txtAgentCert;
        private LinkLabel llBrowseAgentCert;
        private Label label3;
        private LinkLabel llGenerateMgtKey;
        private Label lblHSMAvaliable;
        private Label label2;
        private Label label5;
        private TextBox txtCaTemplate;
    }
}