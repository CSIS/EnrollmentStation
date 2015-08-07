using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgEnroll : Form
    {
        private readonly Settings _settings;
        private readonly DataStore _dataStore;
        private bool _devicePresent;
        private bool _hasBeenEnrolled;
        private bool _hsmPresent;

        private string _enrollWorkerMessage;
        private readonly BackgroundWorker _enrollWorker;
        private readonly YubikeyNeoManager _neoManager;

        public DlgEnroll(Settings settings, DataStore dataStore)
        {
            _settings = settings;
            _dataStore = dataStore;
            _neoManager = new YubikeyNeoManager();

            _enrollWorker = new BackgroundWorker();
            _enrollWorker.DoWork += EnrollWorkerOnDoWork;
            _enrollWorker.ProgressChanged += EnrollWorkerOnProgressChanged;
            _enrollWorker.RunWorkerCompleted += EnrollWorkerOnRunWorkerCompleted;
            _enrollWorker.WorkerReportsProgress = true;

            InitializeComponent();
        }

        private void DlgEnroll_Load(object sender, EventArgs e)
        {
            // Start worker that checks for inserted yubikeys
            YubikeyDetector.Instance.StateChanged += YubikeyStateChange;
            YubikeyDetector.Instance.Start();

            // Call once for initial setup
            YubikeyStateChange();
            RefreshEligibleForEnroll();
        }

        private void DlgEnroll_FormClosing(object sender, FormClosingEventArgs e)
        {
            YubikeyDetector.Instance.StateChanged -= YubikeyStateChange;
        }

        private void YubikeyStateChange()
        {
            using (YubikeyDetector.Instance.GetExclusiveLock())
            {
                _devicePresent = _neoManager.RefreshDevice();
                _hasBeenEnrolled = _dataStore.Search(_neoManager.GetSerialNumber()).Any();
            }

            RefreshEligibleForEnroll();
            RefreshInsertedKeyInfo();

            _hsmPresent = HsmRng.IsHsmPresent();
            lblYubiHsm.Text = _hsmPresent ? "Yes" : "No";
        }

        private void EnrollWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (runWorkerCompletedEventArgs.Error != null)
            {
                // An error happened
                MessageBox.Show("An exception has occurred: " + runWorkerCompletedEventArgs.Error.Message + Environment.NewLine + Environment.NewLine + runWorkerCompletedEventArgs.Error.StackTrace, "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                prgEnroll.Value = prgEnroll.Minimum;
            }
            else if (runWorkerCompletedEventArgs.Cancelled)
            {
                // An error happened
                MessageBox.Show(_enrollWorkerMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                prgEnroll.Value = prgEnroll.Minimum;
            }
            else
            {
                prgEnroll.Value = prgEnroll.Maximum;

                // Success
                MessageBox.Show("Successfully enrolled Yubikey for user", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
            }

            cmdEnroll.Enabled = true;

            foreach (Control control in groupBox1.Controls)
                control.Enabled = true;

            foreach (Control control in groupBox3.Controls)
                control.Enabled = true;
        }

        private void EnrollWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // 0. Get lock on yubikey
            using (YubikeyDetector.Instance.GetExclusiveLock())
            {
                // 1. Prep device info
                int deviceId = _neoManager.GetSerialNumber();
                string neoFirmware = _neoManager.GetVersion().ToString();
                string pivFirmware;

                using (YubikeyPivTool piv = new YubikeyPivTool())
                    pivFirmware = piv.GetVersion();

                _enrollWorker.ReportProgress(1);

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

                _enrollWorker.ReportProgress(2);

                // 3 - Prep CA
                string enrollmentAgent = _settings.EnrollmentAgentCertificate;
                string ca = _settings.CSREndpoint;
                string caTemplate = _settings.EnrollmentCaTemplate;
                string user = txtUser.Text;

                _enrollWorker.ReportProgress(3);

                // 4 - Prep Management Key
                // TODO: Consider a new key every time?
                byte[] mgmKey = _settings.EnrollmentManagementKey;

                _enrollWorker.ReportProgress(4);

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
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to reset the YubiKey";
                        return;
                    }

                    _enrollWorker.ReportProgress(5);

                    // 6 - Yubico: Management Key
                    bool authenticated = pivTool.Authenticate(YubikeyPivTool.DefaultManagementKey);

                    if (!authenticated)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to authenticate with the YubiKey";
                        return;
                    }

                    bool setMgmKey = pivTool.SetManagementKey(mgmKey);

                    if (!setMgmKey)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to set the management key";
                        return;
                    }

                    _enrollWorker.ReportProgress(6);

                    // 7 - Yubico: Set CHUID
                    bool setChuid = pivTool.SetCHUID(Guid.NewGuid(), out chuid);

                    if (!setChuid)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to set CHUID";
                        return;
                    }

                    _enrollWorker.ReportProgress(7);

                    // 8 - Yubico: PIN
                    int tmp;
                    bool setPin = pivTool.ChangePin(YubikeyPivTool.DefaultPin, pin, out tmp);

                    if (!setPin)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to set the PIN code";
                        return;
                    }

                    _enrollWorker.ReportProgress(8);

                    // 9 - Yubico: PUK
                    bool setPuk = pivTool.ChangePuk(YubikeyPivTool.DefaultPuk, puk, out tmp);

                    if (!setPuk)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to set the PUK code";
                        return;
                    }

                    _enrollWorker.ReportProgress(9);

                    // 10 - Yubico: Generate Key
                    bool keyGenerated = pivTool.GenerateKey9a(out publicKey);

                    if (!keyGenerated)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to generate a keypair";
                        return;
                    }

                    _enrollWorker.ReportProgress(10);
                }

                // 11 - Yubico: Make CSR
                string csr;
                bool madeCsr = MakeCsr(Utilities.ExportPublicKeyToPEMFormat(publicKey), pin, out csr);

                if (!madeCsr)
                {
                    doWorkEventArgs.Cancel = true;
                    _enrollWorkerMessage = "Unable to generate a CSR";
                    return;
                }

                _enrollWorker.ReportProgress(11);

                // 12 - Enroll
                string enrollError;
                bool enrolled = CertificateUtilities.Enroll(user, enrollmentAgent, ca, caTemplate, csr, out enrollError, out cert);

                if (!enrolled)
                {
                    doWorkEventArgs.Cancel = true;
                    _enrollWorkerMessage = "Unable to enroll a certificate." + Environment.NewLine + enrollError;
                    return;
                }

                _enrollWorker.ReportProgress(12);

                using (YubikeyPivTool pivTool = new YubikeyPivTool())
                {
                    // 13 - Yubico: Import Cert
                    bool authenticatedForCert = pivTool.Authenticate(mgmKey);

                    if (!authenticatedForCert)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to authenticate prior to importing a certificate";
                        return;
                    }

                    bool imported = pivTool.SetCertificate9a(cert);

                    if (!imported)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = "Unable to import a certificate";
                        return;
                    }

                    _enrollWorker.ReportProgress(13);
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
                newEnrollment.ManagementKey = mgmKey;
                newEnrollment.PukKey = puk;
                newEnrollment.Chuid = BitConverter.ToString(chuid).Replace("-", "");

                newEnrollment.EnrolledAt = DateTime.UtcNow;

                newEnrollment.YubikeyVersions.NeoFirmware = neoFirmware;
                newEnrollment.YubikeyVersions.PivApplet = pivFirmware;

                _dataStore.Add(newEnrollment);

                _enrollWorker.ReportProgress(14);

                // 15 - Save store
                _dataStore.Save(MainForm.FileStore);

                _enrollWorker.ReportProgress(15);

                // Report
                doWorkEventArgs.Cancel = false;
                _enrollWorkerMessage = "Success";
            }
        }

        private void EnrollWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            prgEnroll.Value = progressChangedEventArgs.ProgressPercentage;
        }

        private void RefreshInsertedKeyInfo()
        {
            foreach (Control control in gbInsertedYubikey.Controls)
            {
                if (control.Name.StartsWith("lbl"))
                    control.Visible = _devicePresent;
            }

            if (!_devicePresent)
                return;

            lblAlreadyEnrolled.Visible = _hasBeenEnrolled;

            using (YubikeyDetector.Instance.GetExclusiveLock())
            {
                YubicoNeoMode currentMode = _neoManager.GetMode();

                if (currentMode.HasCcid)
                    lblInsertedMode.ForeColor = Color.Black;
                else
                    lblInsertedMode.ForeColor = Color.Red;

                lblInsertedSerial.Text = _neoManager.GetSerialNumber().ToString();
                lblInsertedMode.Text = currentMode.ToString();
                lblInsertedFirmware.Text = _neoManager.GetVersion().ToString();
            }
        }

        private void RefreshEligibleForEnroll()
        {
            bool eligible = true;

            if (!_devicePresent)
                eligible = false;

            if (txtPin.Text.Length <= 0)
                eligible = false;

            if (txtPin.Text != txtPinAgain.Text)
                eligible = false;

            if (string.IsNullOrEmpty(txtUser.Text))
                eligible = false;

            if (_hasBeenEnrolled)
                eligible = false;

            cmdEnroll.Enabled = eligible;
        }

        private void cmdEnroll_Click(object sender, EventArgs e)
        {
            if (!_devicePresent)
                return;

            using (YubikeyDetector.Instance.GetExclusiveLock())
            using (YubikeyPivTool piv = new YubikeyPivTool())
            {
                if (piv.GetCertificate9a() != null)
                {
                    // Already enrolled
                    DialogResult resp = MessageBox.Show("The inserted Yubikey has already been enrolled. Are you sure you wish to overwrite it?", "Already enrolled", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    if (resp != DialogResult.Yes)
                        return;
                }
            }

            cmdEnroll.Enabled = false;

            foreach (Control control in groupBox1.Controls)
                control.Enabled = false;

            foreach (Control control in groupBox3.Controls)
                control.Enabled = false;

            _enrollWorker.RunWorkerAsync();
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

        private void llBrowseUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DlgSelectUser dialog = new DlgSelectUser();
            dialog.ShowDialog();

            txtUser.Text = dialog.SelectedUser;
        }

        private void textBoxes_TextChanged(object sender, EventArgs e)
        {
            RefreshEligibleForEnroll();
        }
    }
}