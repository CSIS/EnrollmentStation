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

                if (item.Value == YubikeyPivTool.YKPIV_ALGO_RSA2048)
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
                X509Certificate2 cert = IterateStores(cert2 => cert2.Thumbprint == _settings.EnrollmentAgentCertificate).SingleOrDefault();
                if (cert != null)
                    txtAgentCert.Text = cert.GetNameInfo(X509NameType.SimpleName, false) + " (Valid Enrollment Agent)";
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
            X509Certificate2[] eligible = IterateStores(CanBeUsed).ToArray();
            X509Certificate2Collection selected = X509Certificate2UI.SelectFromCollection(new X509Certificate2Collection(eligible), "Chose a certificate", "Pick an enrollment agent certificate to use.", X509SelectionFlag.SingleSelection);

            if (selected.Count > 0)
            {
                _selectedCertificateThumb = selected[0].Thumbprint;
                txtAgentCert.Text = selected[0].GetNameInfo(X509NameType.SimpleName, false) + " (Valid Enrollment Agent)";
                txtAgentCert.BackColor = SystemColors.Control;
            }
        }

        private IEnumerable<X509Certificate2> IterateStores(Func<X509Certificate2, bool> filter)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    if (filter(certificate))
                        yield return certificate;
                }
            }
            finally
            {
                store.Close();
            }

            store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    if (filter(certificate))
                        yield return certificate;
                }
            }
            finally
            {
                store.Close();
            }
        }

        private bool CanBeUsed(X509Certificate2 cert)
        {
            if (!cert.HasPrivateKey)
                return false;

            // Enhanced Key Usage is 2.5.29.37
            X509EnhancedKeyUsageExtension ekuExtension = null;
            foreach (X509Extension extension in cert.Extensions)
            {
                if (extension.Oid.Value == "2.5.29.37")
                    ekuExtension = (X509EnhancedKeyUsageExtension)extension;
            }

            if (ekuExtension == null)
                return false;

            // Certificate Request Agent is 1.3.6.1.4.1.311.20.2.1
            foreach (Oid oid in ekuExtension.EnhancedKeyUsages)
            {
                if (oid.Value == "1.3.6.1.4.1.311.20.2.1")
                    return true;
            }

            return false;
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