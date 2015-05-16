using System;
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
            _settings.EnrollmentAgentCertificate = "07C3F21783B06583974C14A2AE89C1EABED4954E";
            UpdateView();
        }

        private void txtCaTemplate_TextChanged(object sender, EventArgs e)
        {
            _settings.EnrollmentCaTemplate = txtCaTemplate.Text;
        }
    }
}
