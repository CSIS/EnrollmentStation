using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgEnroll : Form
    {
        private YubikeyNeoManager _neoManager;
        private readonly Settings _settings;
        private readonly DataStore _dataStore;
        private bool _devicePresent;
        private bool _hsmPresent;

        public DlgEnroll(Settings settings, DataStore dataStore)
        {
            _settings = settings;
            _dataStore = dataStore;
            _neoManager = new YubikeyNeoManager();

            InitializeComponent();
        }

        private void InsertedYubikeyWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (true)
            {
                _devicePresent = _neoManager.RefreshDevice();

                RefreshInsertedKeyInfo();

                _hsmPresent = HsmRng.IsHsmPresent();
                lblYubiHsm.Text = _hsmPresent ? "Yes" : "No";

                Thread.Sleep(1000);
            }
        }

        private void RefreshInsertedKeyInfo()
        {
            foreach (Control control in gbInsertedYubikey.Controls)
            {
                control.Visible = _devicePresent;
            }

            if (!_devicePresent)
                return;

            YubicoNeoMode currentMode = _neoManager.GetMode();

            bool ccidEnabled;

            switch (currentMode)
            {
                case YubicoNeoMode.OtpOnly:
                case YubicoNeoMode.U2fOnly:
                case YubicoNeoMode.OtpU2f:
                case YubicoNeoMode.OtpOnly_WithEject:
                case YubicoNeoMode.U2fOnly_WithEject:
                case YubicoNeoMode.OtpU2f_WithEject:
                    ccidEnabled = false;
                    break;
                case YubicoNeoMode.CcidOnly:
                case YubicoNeoMode.OtpCcid:
                case YubicoNeoMode.U2fCcid:
                case YubicoNeoMode.OtpU2fCcid:
                case YubicoNeoMode.CcidOnly_WithEject:
                case YubicoNeoMode.OtpCcid_WithEject:
                case YubicoNeoMode.U2fCcid_WithEject:
                case YubicoNeoMode.OtpU2fCcid_WithEject:
                    ccidEnabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ccidEnabled)
                lblInsertedMode.ForeColor = Color.Black;
            else
                lblInsertedMode.ForeColor = Color.Red;

            lblInsertedSerial.Text = _neoManager.GetSerialNumber().ToString();
            lblInsertedMode.Text = currentMode.ToString(); //TODO: Get text in the format OTP+CCID
            lblInsertedFirmware.Text = _neoManager.GetVersion().ToString();
        }

        private void RefreshEligibleForEnroll()
        {
            bool eligible = !string.IsNullOrEmpty(_settings.CSREndpoint);

            if (string.IsNullOrEmpty(_settings.EnrollmentAgentCertificate))
                eligible = false;

            if (string.IsNullOrEmpty(_settings.EnrollmentManagementKey))
                eligible = false;

            if (string.IsNullOrEmpty(_settings.EnrollmentCaTemplate))
                eligible = false;

            if (txtPin.Text.Length <= 0 || txtPin.Text.Length > 8)
                eligible = false;

            if (txtPin.Text != txtPinAgain.Text)
                eligible = false;

            if (string.IsNullOrEmpty(txtUser.Text))
                eligible = false;

            cmdEnroll.Enabled = eligible;
        }

        private void cmdEnroll_Click(object sender, EventArgs e)
        {
            if (!_devicePresent)
                return;

            prgEnroll.Maximum = 14;
            prgEnroll.Value = 0;

            // 1. Prep device info
            int deviceId = _neoManager.GetSerialNumber();
            string neoFirmware = _neoManager.GetVersion().ToString();
            string pivFirmware;

            using (YubikeyPivTool piv = new YubikeyPivTool())
                pivFirmware = piv.GetVersion();

            prgEnroll.Value = 1;

            // 2 - Generate PUK, prep PIN
            string puk;

            if (_hsmPresent)
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
            string ca = _settings.CSREndpoint;
            string caTemplate = _settings.EnrollmentCaTemplate;
            string user = txtUser.Text;

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
                    MessageBox.Show("Unable to reset the YubiKey", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                prgEnroll.Value = 5;

                // 6 - Yubico: Management Key
                bool authenticated = pivTool.Authenticate(YubikeyPivTool.DefaultManagementKey);

                if (!authenticated)
                {
                    MessageBox.Show("Unable to authenticate with the YubiKey", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                bool setMgmKey = pivTool.SetManagementKey(mgmKey);

                if (!setMgmKey)
                {
                    MessageBox.Show("Unable to set the management key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                int tmp;
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

            newEnrollment.Certificate.Serial = cert.SerialNumber;
            newEnrollment.Certificate.Thumbprint = cert.Thumbprint;
            newEnrollment.Certificate.Subject = cert.Subject;
            newEnrollment.Certificate.Issuer = cert.Issuer;
            newEnrollment.Certificate.StartDate = cert.NotBefore;
            newEnrollment.Certificate.ExpireDate = cert.NotAfter;
            newEnrollment.Certificate.RawCertificate = cert.RawData;

            newEnrollment.CA = ca;
            newEnrollment.Username = user;
            newEnrollment.ManagementKey = mgmKeyString;
            newEnrollment.PukKey = puk;
            newEnrollment.Chuid = BitConverter.ToString(chuid).Replace("-", "");

            newEnrollment.EnrolledAt = DateTime.UtcNow;

            newEnrollment.YubikeyVersions.NeoFirmware = neoFirmware;
            newEnrollment.YubikeyVersions.PivApplet = pivFirmware;

            _dataStore.Add(newEnrollment);

            prgEnroll.Value = 14;

            DialogResult = DialogResult.OK;
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
                    csr = File.ReadAllText(tmpCsr);

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

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void DlgEnroll_Load(object sender, EventArgs e)
        {
            BackgroundWorker insertedYubikeyWorker = new BackgroundWorker();
            insertedYubikeyWorker.DoWork += InsertedYubikeyWorkerOnDoWork;
            insertedYubikeyWorker.RunWorkerAsync();
        }

        private void llBrowseUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DlgSelectUser dialog = new DlgSelectUser();
            dialog.ShowDialog();

            txtUser.Text = dialog.SelectedUser;
        }
    }
}