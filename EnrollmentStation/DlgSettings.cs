using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CERTCLIENTLib;
using EnrollmentStation.Code;
using EnrollmentStation.Code.DataObjects;
using EnrollmentStation.Code.Utilities;
using YubicoLib.YubikeyPiv;

namespace EnrollmentStation
{
    public partial class DlgSettings : Form
    {
        private const int CC_UIPICKCONFIG = 0x1;
        private static readonly Regex _managementKeyRegex = new Regex("^[a-fA-F0-9]{48}$", RegexOptions.Compiled);
        private string _selectedCertificateThumb;

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
            // Prepare algorithms
            foreach (YubikeyAlgorithm item in YubikeyPolicyUtility.GetYubicoAlgorithms())
            {
                drpAlgorithm.Items.Add(item);

                if (item.Value == YubikeyPivNative.YKPIV_ALGO_RSA2048)
                    drpAlgorithm.SelectedItem = item;
            }

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

        private void DisplaySelectedCertificate(WindowsCertificate cert)
        {
            if (cert == null)
                return;

            string nameInfo = cert.Certificate.GetNameInfo(X509NameType.SimpleName, false);

            if (WindowsCertStoreUtilities.IsAgentCertificate(cert.Certificate))
                txtAgentCert.Text = $"{nameInfo} (Valid Enrollment Agent), store: {cert.StoreLocation}";
            else
                txtAgentCert.Text = $"{nameInfo} (NOT a valid Enrollment Agent), store: {cert.StoreLocation}";
        }

        private void UpdateView()
        {
            if (_settings.EnrollmentManagementKey != null && _settings.EnrollmentManagementKey.Length > 0)
                txtManagementKey.Text = BitConverter.ToString(_settings.EnrollmentManagementKey).Replace("-", "");

            if (!string.IsNullOrWhiteSpace(_settings.CSREndpoint))
                txtCSREndpoint.Text = _settings.CSREndpoint;

            if (!string.IsNullOrWhiteSpace(_settings.EnrollmentCaTemplate))
                txtCaTemplate.Text = _settings.EnrollmentCaTemplate;

            if (!string.IsNullOrWhiteSpace(_settings.EnrollmentAgentCertificate))
            {
                _selectedCertificateThumb = _settings.EnrollmentAgentCertificate;
                WindowsCertificate cert = WindowsCertStoreUtilities.FindCertificate(_settings.EnrollmentAgentCertificate);

                if (cert != null)
                {
                   DisplaySelectedCertificate(cert);
                }
                else
                    MessageBox.Show(this,
                        "The certificate with the thumbprint " + _selectedCertificateThumb +
                        " was not found in your certificate store.", "Certificate not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }

            if (_settings.DefaultAlgorithm > 0)
            {
                YubikeyAlgorithm algo = YubikeyPolicyUtility.GetYubicoAlgorithms().FirstOrDefault(s => s.Value == _settings.DefaultAlgorithm);

                if (algo != null)
                    drpAlgorithm.SelectedItem = algo;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            _settings.CSREndpoint = txtCSREndpoint.Text;
            _settings.EnrollmentAgentCertificate = _selectedCertificateThumb;
            _settings.EnrollmentManagementKey = Utilities.StringToByteArray(txtManagementKey.Text);
            _settings.EnrollmentCaTemplate = txtCaTemplate.Text;
            _settings.DefaultAlgorithm = ((YubikeyAlgorithm)drpAlgorithm.SelectedItem).Value;

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
            X509Certificate2[] eligible = WindowsCertStoreUtilities.GetAgentCertificates().Select(s => s.Certificate).ToArray();
            X509Certificate2Collection selected = X509Certificate2UI.SelectFromCollection(new X509Certificate2Collection(eligible), "Chose a certificate", "Pick an enrollment agent certificate to use.", X509SelectionFlag.SingleSelection);

            if (selected.Count > 0)
            {
                _selectedCertificateThumb = selected[0].Thumbprint;

                WindowsCertificate cert = WindowsCertStoreUtilities.FindCertificate(_selectedCertificateThumb);
                DisplaySelectedCertificate(cert);
            }
        }

        private void llGenerateMgtKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtManagementKey.Text = BitConverter.ToString(Utilities.GenerateRandomKey()).Replace("-", string.Empty).Substring(0, 48);
            txtManagementKey.BackColor = Color.White;
        }

        private void txtManagementKey_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_managementKeyRegex.IsMatch(txtManagementKey.Text))
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