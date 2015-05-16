using System;
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

        private readonly Settings _settings;

        public DlgSettings(Settings settings)
        {
            InitializeComponent();

            _settings = settings;
            UpdateView();
        }

        private void UpdateView()
        {
            if (string.IsNullOrEmpty(_settings.CA))
                lblCA.Text = "Not chosen";
            else
                lblCA.Text = _settings.CA;

            if (string.IsNullOrEmpty(_settings.EnrollmentAgentCertificate))
                lblAgentCertificate.Text = "Not chosen";
            else
                lblAgentCertificate.Text = _settings.EnrollmentAgentCertificate;

            txtDomain.Text = _settings.EnrollmentDomain;
            txtManagementKey.Text = _settings.EnrollmentManagementKey;
            txtCaTemplate.Text = _settings.EnrollmentCaTemplate;
        }

        private void lblCa_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CCertConfig objCertConfig = new CCertConfig();

            try
            {
                string config = objCertConfig.GetConfig(CC_UIPICKCONFIG);

                if (!string.IsNullOrEmpty(config))
                {
                    _settings.CA = config;
                    UpdateView();
                }
            }
            catch (Exception)
            {

            }
        }

        private void txtDomain_TextChanged(object sender, System.EventArgs e)
        {
            _settings.EnrollmentDomain = txtDomain.Text;
        }

        private void cmdSave_Click(object sender, System.EventArgs e)
        {
            Regex rgx = new Regex("^[A-F0-9]{48}$");

            txtManagementKey.Text = txtManagementKey.Text.ToUpper();
            if (!rgx.IsMatch(txtManagementKey.Text))
            {
                MessageBox.Show("Management key must be 48 hex-characters long", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void txtManagementKey_TextChanged(object sender, System.EventArgs e)
        {
            _settings.EnrollmentManagementKey = txtManagementKey.Text;
        }

        private void lblAgentCertificate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            X509Store store = new X509Store(StoreName.My);

            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection eligible = new X509Certificate2Collection();
            try
            {
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
            }
            finally 
            {
                store.Close();
            }

            X509Certificate2Collection selected = X509Certificate2UI.SelectFromCollection(eligible, "Chose a certificate", "Pick an enrollment agent certificate to use.", X509SelectionFlag.SingleSelection);

            foreach (X509Certificate2 certificate in selected)
                _settings.EnrollmentAgentCertificate = certificate.Thumbprint;

            UpdateView();
        }

        private void txtCaTemplate_TextChanged(object sender, EventArgs e)
        {
            _settings.EnrollmentCaTemplate = txtCaTemplate.Text;
        }
    }
}
