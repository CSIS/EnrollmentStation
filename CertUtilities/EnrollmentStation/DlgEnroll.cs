using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgEnroll : Form
    {
        private YubikeyNeoManager _neoManager;
        private readonly Settings _settings;
        private readonly DataStore _dataStore;

        public DlgEnroll(Settings settings, DataStore dataStore)
        {
            _settings = settings;
            _dataStore = dataStore;
            _neoManager = new YubikeyNeoManager();

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            lblDomain.Text = _settings.EnrollmentDomain;
            lblCA.Text = _settings.CA;

            RefreshYubiDetails();
            RefreshEligibleForEnroll();

            lblStatus.Text = "";
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            RefreshYubiDetails();
            RefreshEligibleForEnroll();
        }

        private void RefreshEligibleForEnroll()
        {
            bool eligible = true;

            if (string.IsNullOrEmpty(_settings.CA))
                eligible = false;

            if (string.IsNullOrEmpty(_settings.EnrollmentAgentCertificate))
                eligible = false;

            if (string.IsNullOrEmpty(_settings.EnrollmentManagementKey))
                eligible = false;

            if (string.IsNullOrEmpty(_settings.EnrollmentCaTemplate))
                eligible = false;

            if (string.IsNullOrEmpty(_settings.EnrollmentDomain))
                eligible = false;

            if (txtPin.Text.Length <= 0 || txtPin.Text.Length > 8)
                eligible = false;

            if (txtPin.Text != txtPinAgain.Text)
                eligible = false;

            if (string.IsNullOrEmpty(txtUser.Text))
                eligible = false;

            // TODO: Check if user exists

            cmdEnroll.Enabled = eligible;
        }

        private void RefreshYubiDetails()
        {
            cmdEnroll.Enabled = false;

            bool device = _neoManager.RefreshDevice();

            if (device)
            {
                cmdEnroll.Enabled = true;
                lblSerial.Text = _neoManager.GetSerialNumber().ToString();
            }
            else
            {
                lblSerial.Text = "No device";
            }

            bool hasHsm = HsmRng.IsHsmPresent();

            if (hasHsm)
            {
                lblYubiHsm.Text = "Yes";
            }
            else
            {
                lblYubiHsm.Text = "No";
            }
        }

        private void txtPin_TextChanged(object sender, EventArgs e)
        {
            if (txtPin.Text != txtPinAgain.Text)
            {
                cmdEnroll.Enabled = false;
                lblStatus.Text = "PIN's must be equal";
            }
            else
            {
                lblStatus.Text = string.Empty;
                RefreshEligibleForEnroll();
            }
        }

        private void cmdEnroll_Click(object sender, EventArgs e)
        {
            int tmp;

            prgEnroll.Maximum = 14;
            prgEnroll.Value = 0;

            bool hadDevice = _neoManager.RefreshDevice();

            if (!hadDevice)
            {
                lblStatus.Text = "No device?";
                return;
            }

            // 1. Prep device info
            int deviceId = _neoManager.GetSerialNumber();

            prgEnroll.Value = 1;

            // 2 - Generate PUK, prep PIN
            string puk;

            if (HsmRng.IsHsmPresent())
            {
                byte[] random = HsmRng.FetchRandom(8);
                puk = Utilities.MapBytesToString(random);
            }
            else
            {
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] random = new byte[8];
                    rng.GetBytes(random);

                    puk = Utilities.MapBytesToString(random);
                }
            }

            string pin = txtPin.Text;

            prgEnroll.Value = 2;

            // 3 - Prep CA
            string enrollmentAgent = _settings.EnrollmentAgentCertificate;
            string ca = _settings.CA;
            string caTemplate = _settings.EnrollmentCaTemplate;
            string user = _settings.EnrollmentDomain + "\\" + txtUser.Text;

            prgEnroll.Value = 3;

            // 4 - Prep Management Key
            // TODO: Consider a new key every time?
            string mgmKeyString = _settings.EnrollmentManagementKey;
            byte[] mgmKey = Utilities.StringToByteArray(mgmKeyString);

            prgEnroll.Value = 4;

            RSAParameters publicKey;
            X509Certificate2 cert;
            byte[] chuid;
            using (YubikeyPivTool pivTool = new YubikeyPivTool())
            {
                // 5 - Yubico: Reset device
                pivTool.BlockPin();
                pivTool.BlockPuk();
                bool reset = pivTool.ResetDevice();

                if (!reset)
                {
                    MessageBox.Show("Unable to reset the Smartcard", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                prgEnroll.Value = 5;

                // 6 - Yubico: Management Key
                bool authenticated = pivTool.Authenticate(YubikeyPivTool.DefaultManagementKey);

                if (!authenticated)
                {
                    MessageBox.Show("Unable to authenticate with the Smartcard", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                bool setMgmKey = pivTool.SetManagementKey(mgmKey);

                if (!setMgmKey)
                {
                    MessageBox.Show("Unable to set the Management Key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                prgEnroll.Value = 6;

                // 7 - Yubico: Set CHUID
                bool setChuid = pivTool.SetCHUID(Guid.NewGuid(), out chuid);

                if (!setChuid)
                {
                    MessageBox.Show("Unable to set CHUID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                prgEnroll.Value = 7;

                // 8 - Yubico: PIN
                bool setPin = pivTool.ChangePin(YubikeyPivTool.DefaultPin, pin, out tmp);

                if (!setPin)
                {
                    MessageBox.Show("Unable to set the PIN code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                prgEnroll.Value = 8;

                // 9 - Yubico: PUK
                bool setPuk = pivTool.ChangePuk(YubikeyPivTool.DefaultPuk, puk, out tmp);

                if (!setPuk)
                {
                    MessageBox.Show("Unable to set the PUK code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                prgEnroll.Value = 9;

                // 10 - Yubico: Generate Key
                bool keyGenerated = pivTool.GenerateKey9a(out publicKey);

                if (!keyGenerated)
                {
                    MessageBox.Show("Unable to generate a keypair", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                prgEnroll.Value = 10;
            }

            // 11 - Yubico: Make CSR
            string csr;
            bool madeCsr = MakeCsr(Utilities.ExportPublicKeyToPEMFormat(publicKey), pin, out csr);

            if (!madeCsr)
            {
                MessageBox.Show("Unable to generate a CSR", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            prgEnroll.Value = 11;

            // 12 - Enroll
            bool enrolled = CertificateUtilities.Enroll(user, enrollmentAgent, ca, caTemplate, csr, out cert);

            if (!enrolled)
            {
                MessageBox.Show("Unable to enroll a certificate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            prgEnroll.Value = 12;

            using (YubikeyPivTool pivTool = new YubikeyPivTool())
            {
                // 13 - Yubico: Import Cert
                bool authenticatedForCert = pivTool.Authenticate(mgmKey);

                if (!authenticatedForCert)
                {
                    MessageBox.Show("Unable to authenticate prior to importing a certificate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                bool imported = pivTool.SetCertificate9a(cert);

                if (!imported)
                {
                    MessageBox.Show("Unable to import a certificate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                prgEnroll.Value = 13;
            }

            // 14 - Create enrolled item
            EnrolledYubikey newEnrollment = new EnrolledYubikey();
            newEnrollment.DeviceSerial = deviceId;

            newEnrollment.CertificateSerial = cert.SerialNumber;
            newEnrollment.CertificateThumbprint = cert.Thumbprint;

            newEnrollment.CA = ca;
            newEnrollment.Username = user;
            newEnrollment.ManagementKey = mgmKeyString;
            newEnrollment.PukKey = puk;
            newEnrollment.Chuid = BitConverter.ToString(chuid).Replace("-", "");

            newEnrollment.EnrolledAt = DateTime.UtcNow;

            _dataStore.Add(newEnrollment);

            prgEnroll.Value = 14;

            DialogResult = DialogResult.OK;
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text))
            {
                cmdEnroll.Enabled = false;
                lblStatus.Text = "Username is missing";
            }
            else
            {
                lblStatus.Text = string.Empty;
                RefreshEligibleForEnroll();
            }
        }

        private bool MakeCsr(string pubKeyAsPem, string pin, out string csr)
        {
            csr = null;

            // TODO: Fix.. use no files at all - this is an ugly hack
            // I wasn't able to create a CSR and sign it using code - so we're using Yubico's code for now
            string tmpPub = Path.GetRandomFileName();
            string tmpCsr = Path.GetRandomFileName();

            try
            {
                using (StreamWriter sw = new StreamWriter(tmpPub))
                {
                    sw.WriteLine("-----BEGIN PUBLIC KEY-----");

                    for (int i = 0; i < pubKeyAsPem.Length; i += 48)
                    {
                        string substr = pubKeyAsPem.Substring(i, Math.Min(48, pubKeyAsPem.Length - i));
                        sw.WriteLine(substr);
                    }

                    sw.WriteLine("-----END PUBLIC KEY-----");
                }

                const string binary = @"Binaries\PivTool\yubico-piv-tool.exe";

                ProcessStartInfo start = new ProcessStartInfo(binary);
                start.Arguments = "-a verify-pin -P " + pin + " -s 9a -a request-certificate -S \"/CN=example/O=test/\" -i " + tmpPub + " -o " + tmpCsr + "";
                start.WorkingDirectory = Path.GetFullPath(".");
                //start.RedirectStandardError = true;
                //start.RedirectStandardOutput = true;
                //start.UseShellExecute = false;

                Process proc = Process.Start(start);
                proc.WaitForExit();

                //string stdErr = proc.StandardError.ReadToEnd();
                //string stdOut = proc.StandardOutput.ReadToEnd();

                if (File.Exists(tmpCsr))
                {
                    csr = File.ReadAllText(tmpCsr);
                }

                return proc.ExitCode == 0;
            }
            finally
            {
                if (File.Exists(tmpPub))
                    File.Delete(tmpPub);

                if (File.Exists(tmpCsr))
                    File.Delete(tmpCsr);
            }
        }
    }
}
