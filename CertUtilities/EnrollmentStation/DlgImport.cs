using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CERTCLIENTLib;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgImport : Form
    {
        private const int CC_UIPICKCONFIG = 0x1;

        private DataStore _store;
        X509Certificate2 _currentCert;

        public DlgImport(DataStore store, Settings settings)
        {
            InitializeComponent();

            lblCa.Text = settings.CA;

            _store = store;
            using (YubikeyNeoManager neo = new YubikeyNeoManager())
            {
                bool hadDevice = neo.RefreshDevice();
                if (!hadDevice)
                {
                    DialogResult = DialogResult.Abort;
                    return;
                }

                lblSerialNumber.Text = neo.GetSerialNumber().ToString();

                using (YubikeyPivTool piv = new YubikeyPivTool())
                    _currentCert = piv.GetCertificate9a();

                if (_currentCert != null)
                {
                    lblCertificateSerial.Text = _currentCert.SerialNumber;
                    lblCertificateSubject.Text = _currentCert.Subject;
                    cmdViewCertificate.Enabled = true;

                    txtUser.Text = GetUsernameEstimate(_currentCert);
                }
                else
                {
                    lblCertificateSerial.Text = "<unknown>";
                    lblCertificateSubject.Text = "<unknown>";
                    cmdViewCertificate.Enabled = false;
                }
            }

            UpdateView();
        }

        private static string GetUsernameEstimate(X509Certificate2 cert)
        {
            X509Extension altExt = null;
            foreach (X509Extension extension in cert.Extensions)
            {
                if (extension.Oid.Value != "2.5.29.17")
                    continue;

                altExt = extension;
                break;
            }

            if (altExt == null)
                return string.Empty;

            string txt = altExt.Format(true);

            Match mtch = Regex.Match(txt, @"Principal Name=(.*?)@([^\r\n]*)");

            if (!mtch.Success)
                return string.Empty;

            return mtch.Groups[2].Value + "\\" + mtch.Groups[1].Value;
        }

        private void UpdateView()
        {
            cmdImport.Enabled = false;

            if (_currentCert == null)
            {
                lblInstructions.Text = "This smartcard has no certificate. Importing is not possible";
                return;
            }

            if (string.IsNullOrEmpty(txtPuk.Text))
            {
                lblInstructions.Text = "Enter the PUK code that was used for this card";
                return;
            }

            if (string.IsNullOrEmpty(txtManagement.Text))
            {
                lblInstructions.Text = "Enter the Management Key that was used for this card";
                return;
            }

            cmdImport.Enabled = true;
            lblInstructions.Text = "Click import to import the smart card";
        }

        private void cmdImport_Click(object sender, EventArgs e)
        {
            EnrolledYubikey yubikey = new EnrolledYubikey();

            yubikey.YubikeyVersions = new YubikeyVersions();

            using (YubikeyNeoManager neo = new YubikeyNeoManager())
            {
                bool hadDevice = neo.RefreshDevice();
                if (!hadDevice)
                {
                    MessageBox.Show("Unable to find the smartcard!", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                yubikey.DeviceSerial = neo.GetSerialNumber();

                yubikey.YubikeyVersions.NeoFirmware = neo.GetVersion().ToString();
            }

            using (YubikeyPivTool piv = new YubikeyPivTool())
            {
                byte[] tmpChuid;
                piv.GetCHUID(out tmpChuid);
                yubikey.Chuid = BitConverter.ToString(tmpChuid).Replace("-", "");

                yubikey.YubikeyVersions.PivApplet = piv.GetVersion();
            }

            yubikey.Username = txtUser.Text;
            yubikey.EnrolledAt = DateTime.UtcNow;
            yubikey.ManagementKey = txtManagement.Text;
            yubikey.PukKey = txtPuk.Text;
            yubikey.CA = lblCa.Text;

            yubikey.Certificate = new CertificateDetails();
            yubikey.Certificate.StartDate = _currentCert.NotBefore;
            yubikey.Certificate.ExpireDate = _currentCert.NotAfter;
            yubikey.Certificate.Serial = _currentCert.SerialNumber;
            yubikey.Certificate.Issuer = _currentCert.Issuer;
            yubikey.Certificate.Subject = _currentCert.Subject;
            yubikey.Certificate.Thumbprint = _currentCert.Thumbprint;

            _store.Add(yubikey);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtPuk_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void txtManagement_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void lblCa_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CCertConfig objCertConfig = new CCertConfig();

            try
            {
                string config = objCertConfig.GetConfig(CC_UIPICKCONFIG);

                if (!string.IsNullOrEmpty(config))
                {
                    lblCa.Text = config;
                    UpdateView();
                }
            }
            catch (Exception)
            {

            }
        }

        private void cmdViewCertificate_Click(object sender, EventArgs e)
        {
            X509Certificate2UI.DisplayCertificate(_currentCert);
        }
    }
}
