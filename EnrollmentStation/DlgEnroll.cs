using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using EnrollmentStation.Code;
using Tulpep.ActiveDirectoryObjectPicker;

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
        private readonly Timer _hsmUpdateTimer = new Timer();

        public DlgEnroll(Settings settings, DataStore dataStore)
        {
            _settings = settings;
            _dataStore = dataStore;

            _enrollWorker = new BackgroundWorker();
            _enrollWorker.DoWork += EnrollWorkerOnDoWork;
            _enrollWorker.ProgressChanged += EnrollWorkerOnProgressChanged;
            _enrollWorker.RunWorkerCompleted += EnrollWorkerOnRunWorkerCompleted;
            _enrollWorker.WorkerReportsProgress = true;

            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DlgEnroll_Load(object sender, EventArgs e)
        {
            AcceptButton = cmdEnroll;
            
            // Start worker that checks for inserted yubikeys
            YubikeyDetector.Instance.StateChanged += YubikeyStateChange;
            YubikeyDetector.Instance.Start();

            _devicePresent = YubikeyDetector.Instance.CurrentState;

            // Call once for initial setup
            YubikeyStateChange();

            _hsmUpdateTimer.Interval = 1000;
            _hsmUpdateTimer.Tick += HsmUpdateTimerOnTick;
            _hsmUpdateTimer.Start();

            RefreshHSM();

            try
            {
                Domain domain = Domain.GetComputerDomain();

                if (!string.IsNullOrWhiteSpace(domain.Name))
                    llBrowseUser.Visible = true;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                llBrowseUser.Visible = false;
            }

            // Prepare algorithms
            foreach (YubikeyAlgorithm item in YubikeyPolicyUtility.GetYubicoAlgorithms())
            {
                drpAlgorithm.Items.Add(item);

                if (item.Value == _settings.DefaultAlgorithm)
                    drpAlgorithm.SelectedItem = item;
            }
        }

        private void HsmUpdateTimerOnTick(object sender, EventArgs eventArgs)
        {
            RefreshHSM();
        }

        private void DlgEnroll_FormClosing(object sender, FormClosingEventArgs e)
        {
            YubikeyDetector.Instance.StateChanged -= YubikeyStateChange;
        }

        private void YubikeyStateChange()
        {
            using (YubikeyDetector.Instance.GetExclusiveLock())
            {
                _devicePresent = YubikeyNeoManager.Instance.RefreshDevice();
                _hasBeenEnrolled = _dataStore.Search(YubikeyNeoManager.Instance.GetSerialNumber()).Any();
            }

            RefreshView();
        }

        private void RefreshView()
        {
            RefreshInsertedKeyInfo();
            RefreshEligibleForEnroll();
        }

        private void RefreshHSM()
        {
            _hsmPresent = HsmRng.IsHsmPresent();
            specialYubiHsm.InvokeIfNeeded(() => specialYubiHsm.Text = _hsmPresent ? "Yes" : "No");
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
                int deviceId = YubikeyNeoManager.Instance.GetSerialNumber();
                string neoFirmware = YubikeyNeoManager.Instance.GetVersion().ToString();
                Version pivFirmware;

                using (YubikeyPivTool piv = new YubikeyPivTool())
                    pivFirmware = piv.GetVersion();

                _enrollWorker.ReportProgress(1);

                // 2 - Generate PUK, prep PIN
                byte[] randomKey = Utilities.GenerateRandomKey();
                string puk = Utilities.MapBytesToString(randomKey.Take(8).ToArray());

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
                    YubikeyAlgorithm algorithm = (YubikeyAlgorithm)drpAlgorithm.SelectedItem;

                    bool keyGenerated = pivTool.GenerateKey9a(algorithm.Value, out publicKey);

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
                string csrError;
                bool madeCsr = MakeCsr(Utilities.ExportPublicKeyToPEMFormat(publicKey), pin, out csrError, out csr);

                if (!madeCsr)
                {
                    doWorkEventArgs.Cancel = true;
                    _enrollWorkerMessage = "Unable to generate a CSR" + Environment.NewLine + csrError;
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

                    YubicoPivReturnCode imported = pivTool.SetCertificate9a(cert);

                    if (imported != YubicoPivReturnCode.YKPIV_OK)
                    {
                        doWorkEventArgs.Cancel = true;
                        _enrollWorkerMessage = $"Unable to import a certificate, return code {imported}";
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
                newEnrollment.YubikeyVersions.PivApplet = pivFirmware.ToString();

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
                YubicoNeoMode currentMode = YubikeyNeoManager.Instance.GetMode();

                if (currentMode.HasCcid)
                    lblInsertedMode.ForeColor = Color.Black;
                else
                    lblInsertedMode.ForeColor = Color.Red;

                lblInsertedSerial.Text = YubikeyNeoManager.Instance.GetSerialNumber().ToString();
                lblInsertedMode.Text = currentMode.ToString();

                lblInsertedFirmware.Text = YubikeyNeoManager.Instance.GetVersion().ToString();
            }
        }

        private void RefreshEligibleForEnroll()
        {
            bool eligible = true;

            if (!_devicePresent)
                eligible = false;

            if (!YubikeyPolicyUtility.IsValidPin(txtPin.Text))
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

        private bool MakeCsr(string pubKeyAsPem, string pin, out string error, out string csr)
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
                start.CreateNoWindow = true;
                start.UseShellExecute = false;
                start.RedirectStandardError = true;
                start.RedirectStandardOutput = true;

                Process proc = Process.Start(start);
                proc.WaitForExit();

                string stdErr = proc.StandardError.ReadToEnd();
                string stdOut = proc.StandardOutput.ReadToEnd();

                if (File.Exists(tmpCsr))
                    csr = File.ReadAllText(tmpCsr);

                error = "Output: " + stdOut + Environment.NewLine + "Error: " + stdErr;

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
            DirectoryObjectPickerDialog picker = new DirectoryObjectPickerDialog()
            {
                AllowedObjectTypes = ObjectTypes.Users,
                DefaultObjectTypes = ObjectTypes.Users,
                AllowedLocations = Locations.All,
                DefaultLocations = Locations.JoinedDomain,
                MultiSelect = false,
                ShowAdvancedView = true,
                AttributesToFetch = { "samAccountName" }
            };

            if (picker.ShowDialog() == DialogResult.OK)
            {
                DirectoryObject sel = picker.SelectedObjects.FirstOrDefault();
                string userName = sel?.FetchedAttributes.FirstOrDefault() as string;

                if (sel != null)
                {
                    txtUser.Text = userName;
                }
            }
        }

        private void textBoxes_TextChanged(object sender, EventArgs e)
        {
            RefreshEligibleForEnroll();
        }

        private void txtPin_Validating(object sender, CancelEventArgs e)
        {
            if (!YubikeyPolicyUtility.IsValidPin(txtPin.Text))
            {
                txtPin.BackColor = Color.LightCoral;
                e.Cancel = true;
            }
            else
            {
                txtPin.BackColor = Color.White;
                e.Cancel = false;
            }
        }

        private void txtPinAgain_Validating(object sender, CancelEventArgs e)
        {
            if (txtPin.Text != txtPinAgain.Text)
            {
                txtPinAgain.BackColor = Color.LightCoral;
                e.Cancel = true;
            }
            else
            {
                txtPinAgain.BackColor = Color.White;
                e.Cancel = false;
            }
        }
    }
}