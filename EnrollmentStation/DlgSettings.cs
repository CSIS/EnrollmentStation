using System;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CERTCLIENTLib;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgSettings : Form
    {
        private const int CC_UIPICKCONFIG = 0x1;
        private static Regex managementKeyRegex = new Regex("^[A-F0-9]{48}$", RegexOptions.Compiled);

        private readonly Settings _settings;

        public DlgSettings(Settings settings)
        {
            _settings = settings;

            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DlgSettings_Load(object sender, EventArgs e)
        {
            UpdateView();

            try
            {
                Domain domain = Domain.GetComputerDomain();

                if (!string.IsNullOrWhiteSpace(domain.Name))
                    llBrowseCA.Visible = true;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                llBrowseCA.Visible = false;
            }
        }

        private void UpdateView()
        {
            lblHSMAvaliable.Text = HsmRng.IsHsmPresent() ? "Yes" : "No";

            if (_settings.EnrollmentManagementKey != null && _settings.EnrollmentManagementKey.Length > 0)
                txtManagementKey.Text = BitConverter.ToString(_settings.EnrollmentManagementKey).Replace("-", "");

            if (!string.IsNullOrWhiteSpace(_settings.CSREndpoint))
                txtCSREndpoint.Text = _settings.CSREndpoint;

            if (!string.IsNullOrWhiteSpace(_settings.EnrollmentCaTemplate))
                txtCaTemplate.Text = _settings.EnrollmentCaTemplate;

            if (!string.IsNullOrWhiteSpace(_settings.EnrollmentAgentCertificate))
                txtAgentCert.Text = _settings.EnrollmentAgentCertificate;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            _settings.CSREndpoint = txtCSREndpoint.Text;
            _settings.EnrollmentAgentCertificate = txtAgentCert.Text;
            _settings.EnrollmentManagementKey = Utilities.StringToByteArray(txtManagementKey.Text);
            _settings.EnrollmentCaTemplate = txtCaTemplate.Text;

            _settings.Save(MainForm.FileSettings);

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void llBrowseCA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                CCertConfig objCertConfig = new CCertConfig();

                string config = objCertConfig.GetConfig(CC_UIPICKCONFIG);

                if (!string.IsNullOrEmpty(config))
                {
                    txtCSREndpoint.Text = config;
                    txtCSREndpoint.BackColor = Color.White;
                }
            }
            catch (Exception)
            {
            }
        }

        private void llBrowseAgentCert_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            X509Store store = new X509Store(StoreName.My);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection eligible = new X509Certificate2Collection();

                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    if (!certificate.HasPrivateKey)
                        continue;

                    // Enhanced Key Usage is 2.5.29.37
                    X509EnhancedKeyUsageExtension ekuExtension = null;
                    foreach (X509Extension extension in certificate.Extensions)
                    {
                        if (extension.Oid.Value == "2.5.29.37")
                            ekuExtension = (X509EnhancedKeyUsageExtension)extension;
                    }

                    if (ekuExtension == null)
                        continue;

                    // Certificate Request Agent is 1.3.6.1.4.1.311.20.2.1
                    bool canBeUsed = false;
                    foreach (Oid oid in ekuExtension.EnhancedKeyUsages)
                    {
                        if (oid.Value == "1.3.6.1.4.1.311.20.2.1")
                            canBeUsed = true;
                    }

                    if (canBeUsed)
                        eligible.Add(certificate);
                }

                X509Certificate2Collection selected = X509Certificate2UI.SelectFromCollection(eligible, "Chose a certificate", "Pick an enrollment agent certificate to use.", X509SelectionFlag.SingleSelection);

                foreach (X509Certificate2 certificate in selected)
                {
                    txtAgentCert.Text = certificate.Thumbprint;
                    txtAgentCert.BackColor = SystemColors.Control;
                    break;
                }
            }
            finally
            {
                store.Close();
            }
        }

        private void llGenerateMgtKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            byte[] newKey = new byte[24];

            if (HsmRng.IsHsmPresent())
                newKey = HsmRng.FetchRandom(24);
            else
            {
                using (RNGCryptoServiceProvider cryptoService = new RNGCryptoServiceProvider())
                    cryptoService.GetBytes(newKey);
            }

            txtManagementKey.Text = BitConverter.ToString(newKey).Replace("-", string.Empty);
            txtManagementKey.BackColor = Color.White;
        }

        private void txtManagementKey_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!managementKeyRegex.IsMatch(txtManagementKey.Text))
            {
                txtManagementKey.BackColor = Color.LightCoral;
                e.Cancel = true;
            }
            else
            {
                txtManagementKey.BackColor = Color.White;
                e.Cancel = false;
            }
        }

        private void txtCSREndpoint_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCSREndpoint.Text))
            {
                txtCSREndpoint.BackColor = Color.LightCoral;
                e.Cancel = true;
            }
            else
            {
                txtCSREndpoint.BackColor = Color.White;
                e.Cancel = false;
            }
        }

        private void txtAgentCert_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAgentCert.Text))
            {
                txtAgentCert.BackColor = Color.LightCoral;
                e.Cancel = true;
            }
            else
            {
                txtAgentCert.BackColor = SystemColors.Control;
                e.Cancel = false;
            }
        }

        private void txtCaTemplate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCaTemplate.Text))
            {
                txtCaTemplate.BackColor = Color.LightCoral;
                e.Cancel = true;
            }
            else
            {
                txtCaTemplate.BackColor = Color.White;
                e.Cancel = false;
            }
        }
    }
}
