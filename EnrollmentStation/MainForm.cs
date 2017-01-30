using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using EnrollmentStation.Code;
using EnrollmentStation.Code.DataObjects;
using EnrollmentStation.Code.Utilities;
using YubicoLib.YubikeyNeo;
using YubicoLib.YubikeyPiv;

namespace EnrollmentStation
{
    public partial class MainForm : Form
    {
        private DataStore _dataStore;
        private Settings _settings;

        public const string FileStore = "store.json";
        public const string FileSettings = "settings.json";

        private bool _hsmPresent;

        private readonly Timer _hsmUpdateTimer = new Timer();

        public MainForm()
        {
            CheckForIllegalCrossThreadCalls = false; //TODO: remove

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshUserStore();
            RefreshSettings();

            RefreshSelectedKeyInfo();

            RefreshInsertedKey();

            _hsmUpdateTimer.Interval = 1000;
            _hsmUpdateTimer.Tick += HsmUpdateTimerOnTick;
            _hsmUpdateTimer.Start();

            lblStatusStripVersion.Text = "Version: " + Assembly.GetExecutingAssembly().GetName().Version;

            // Start worker that checks for inserted yubikeys
            YubikeyDetector.Instance.StateChanged += YubikeyStateChange;
            YubikeyDetector.Instance.Start();

            // Determine if we need to get the settings set
            if (!File.Exists(FileSettings) || _settings.DefaultAlgorithm == 0)
            {
                DlgSettings dialog = new DlgSettings(_settings);
                DialogResult res = dialog.ShowDialog();

                if (res != DialogResult.OK)
                {
                    MessageBox.Show("You have to set the settings. Restart the application to set the settings.");
                    Close();
                }
            }

            RefreshHsm();
        }

        private void HsmUpdateTimerOnTick(object sender, EventArgs e)
        {
            RefreshHsm();
        }

        private void YubikeyStateChange()
        {
            string devName = YubikeyNeoManager.Instance.ListDevices().FirstOrDefault();
            bool hasDevice = !string.IsNullOrEmpty(devName);

            btnExportCert.Enabled = false;
            btnViewCert.Enabled = false;

            if (hasDevice)
            {
                using (YubikeyNeoDevice dev = YubikeyNeoManager.Instance.OpenDevice(devName))
                {
                    YubicoLib.YubikeyNeo.YubicoNeoMode currentMode = dev.GetMode();
                    bool enableCcid = !currentMode.HasCcid;

                    btnExportCert.Enabled = !enableCcid;
                    btnViewCert.Enabled = !enableCcid;
                }
            }

            RefreshInsertedKey();
        }

        private void RefreshSelectedKeyInfo()
        {
            foreach (Control control in gbSelectedKey.Controls)
            {
                if (control.Name.StartsWith("lbl"))
                    control.Visible = lstItems.SelectedItems.Count == 1;
            }

            foreach (Control control in gbSelectedKeyCertificate.Controls)
            {
                if (control.Name.StartsWith("lbl"))
                    control.Visible = lstItems.SelectedItems.Count == 1;
            }

            if (lstItems.SelectedItems.Count <= 0)
                return;

            EnrolledYubikey item = lstItems.SelectedItems[0].Tag as EnrolledYubikey;

            if (item == null)
                return;

            lblYubikeySerial.Text = item.DeviceSerial.ToString();
            lblYubikeyFirmware.Text = item.YubikeyVersions.NeoFirmware;
            lblYubikeyPivVersion.Text = item.YubikeyVersions.PivApplet;

            lblCertCA.Text = item.CA;
            lblCertEnrolledOn.Text = item.EnrolledAt.ToString();
            lblCertSerial.Text = item.Certificate.Serial;
            lblCertThumbprint.Text = item.Certificate.Thumbprint;
            lblCertUser.Text = item.Username;
        }

        private void RefreshHsm()
        {
            _hsmPresent = HsmRng.IsHsmPresent();
            btnViewCert.InvokeIfNeeded(() => lblHSMPresent.Text = "HSM present: " + (_hsmPresent ? "Yes" : "No"));
        }

        private void RefreshInsertedKey()
        {
            List<string> listDevices = YubikeyNeoManager.Instance.ListDevices().ToList();
            string devName = listDevices.FirstOrDefault();
            bool hasDevice = !string.IsNullOrEmpty(devName);

            foreach (Control control in gbInsertedKey.Controls)
            {
                if (control.Name.StartsWith("lbl"))
                    control.Visible = hasDevice;
            }

            if (hasDevice)
            {
                using (YubikeyNeoDevice dev = YubikeyNeoManager.Instance.OpenDevice(devName))
                {
                    int serialNumber = dev.GetSerialNumber();
                    lblInsertedSerial.Text = serialNumber.ToString();
                    lblInsertedFirmware.Text = dev.GetVersion().ToString();
                    lblInsertedMode.Text = dev.GetMode().ToString();

                    lblInsertedHasBeenEnrolled.Text = _dataStore.Search(serialNumber).Any().ToString();
                }
            }

            if (listDevices.Count > 1)
                lblMultipleKeys.Text = $"{listDevices.Count:N0} keys inserted";
            else
                lblMultipleKeys.Text = "";
        }

        private void RefreshUserStore()
        {
            _dataStore = DataStore.Load(FileStore);

            lstItems.Items.Clear();

            foreach (EnrolledYubikey yubikey in _dataStore.Yubikeys)
            {
                ListViewItem lsItem = new ListViewItem();

                lsItem.Tag = yubikey;

                // Fields
                // Serial
                lsItem.Text = yubikey.DeviceSerial.ToString();

                // User
                lsItem.SubItems.Add(yubikey.Username);

                // Enrolled
                lsItem.SubItems.Add(yubikey.EnrolledAt.ToLocalTime().ToString());

                // Certificate Serial
                lsItem.SubItems.Add(yubikey.Certificate != null ? yubikey.Certificate.Serial : "<unknown>");

                lstItems.Items.Add(lsItem);
            }

            RefreshSelectedKeyInfo();
        }

        private void RefreshSettings()
        {
            _settings = Settings.Load(FileSettings);
        }

        private void btnViewCert_Click(object sender, EventArgs e)
        {
            X509Certificate2 cert = null;

            string devName = YubikeyPivManager.Instance.ListDevices().FirstOrDefault();

            if (!string.IsNullOrEmpty(devName))
            {
                using (YubikeyPivDevice dev = YubikeyPivManager.Instance.OpenDevice(devName))
                {
                    cert = dev.GetCertificate9a();
                }
            }

            if (cert == null)
                MessageBox.Show("No certificate on device.", "No Certificate", MessageBoxButtons.OK);
            else
                X509Certificate2UI.DisplayCertificate(cert);
        }

        private void btnExportCert_Click(object sender, EventArgs e)
        {
            X509Certificate2 cert = null;
            int deviceSerial = 0;

            string devName = YubikeyPivManager.Instance.ListDevices().FirstOrDefault();

            if (!string.IsNullOrEmpty(devName))
            {
                using (YubikeyNeoDevice dev = YubikeyNeoManager.Instance.OpenDevice(devName))
                {
                    deviceSerial = dev.GetSerialNumber();
                }

                using (YubikeyPivDevice dev = YubikeyPivManager.Instance.OpenDevice(devName))
                {
                    cert = dev.GetCertificate9a();
                }
            }

            if (cert == null)
            {
                MessageBox.Show("No certificate on device.", "No Certificate", MessageBoxButtons.OK);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = deviceSerial + "-" + cert.SerialNumber + ".crt"; //TODO: GetSerialNumber() can possibly fail

            DialogResult dlgResult = saveFileDialog.ShowDialog();

            if (dlgResult != DialogResult.OK)
                return;

            using (Stream fs = saveFileDialog.OpenFile())
            {
                byte[] data = cert.GetRawCertData();
                fs.Write(data, 0, data.Length);
            }
        }

        private void tsbEnrollKey_Click(object sender, EventArgs e)
        {
            DlgEnroll enroll = new DlgEnroll(_settings, _dataStore);
            enroll.ShowDialog();

            RefreshUserStore();

            RefreshInsertedKey();
        }

        private void exportCertificateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedItems.Count <= 0)
                return;

            EnrolledYubikey item = lstItems.SelectedItems[0].Tag as EnrolledYubikey;
            if (item == null)
                return;

            X509Certificate2 cert = new X509Certificate2(item.Certificate.RawCertificate);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = item.DeviceSerial + "-" + cert.SerialNumber + ".crt";

            DialogResult dlgResult = saveFileDialog.ShowDialog();

            if (dlgResult != DialogResult.OK)
                return;

            using (Stream fs = saveFileDialog.OpenFile())
            {
                byte[] data = cert.GetRawCertData();
                fs.Write(data, 0, data.Length);
            }
        }

        private void viewCertificateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedItems.Count <= 0)
                return;

            EnrolledYubikey item = lstItems.SelectedItems[0].Tag as EnrolledYubikey;
            if (item == null)
                return;

            X509Certificate2 cert = new X509Certificate2(item.Certificate.RawCertificate);
            X509Certificate2UI.DisplayCertificate(cert);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            YubikeyNeoManager.Instance.Dispose();
        }

        private void tsbSettings_Click(object sender, EventArgs e)
        {
            DlgSettings dialog = new DlgSettings(_settings);
            dialog.ShowDialog();
        }

        private void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSelectedKeyInfo();
        }

        private void resetPINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedItems.Count <= 0)
                return;

            EnrolledYubikey item = lstItems.SelectedItems[0].Tag as EnrolledYubikey;
            if (item == null)
                return;

            DlgResetPin changePin = new DlgResetPin(item);
            changePin.ShowDialog();
        }

        private void revokeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (lstItems.SelectedItems.Count <= 0)
                return;

            EnrolledYubikey item = lstItems.SelectedItems[0].Tag as EnrolledYubikey;
            if (item == null)
                return;

            DialogResult dlgResult = MessageBox.Show("Revoke the certificate enrolled at " + item.EnrolledAt.ToLocalTime() + " for " + item.Username + ". This action will revoke " +
                   "the certificate, but will NOT wipe the yubikey." + Environment.NewLine + Environment.NewLine +
                   "Certificate: " + item.Certificate.Serial + Environment.NewLine +
                   "Subject: " + item.Certificate.Subject + Environment.NewLine +
                   "Issuer: " + item.Certificate.Issuer + Environment.NewLine +
                   "Valid: " + item.Certificate.StartDate + " to " + item.Certificate.ExpireDate,
                   "Revoke certificate?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (dlgResult != DialogResult.Yes)
                return;

            DlgProgress prg = new DlgProgress("Revoking certificate");
            prg.WorkerAction = worker =>
            {
                worker.ReportProgress(20, "Revoking certificate ...");

                // Begin
                try
                {
                    CertificateUtilities.RevokeCertificate(item.CA, item.Certificate.Serial);

                    // Revoked
                    _dataStore.Remove(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Unable to revoke certificate " + item.Certificate.Serial + " of " + item.CA +
                        " enrolled on " + item.EnrolledAt + ". There was an error." + Environment.NewLine +
                        Environment.NewLine + ex.Message, "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                worker.ReportProgress(100, string.Empty);

                // Write to disk
                _dataStore.Save(FileStore);
            };

            prg.ShowDialog();

            RefreshUserStore();

            RefreshInsertedKey();
        }

        private void terminateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lstItems.SelectedItems.Count <= 0)
                return;

            EnrolledYubikey item = lstItems.SelectedItems[0].Tag as EnrolledYubikey;
            if (item == null)
                return;

            X509Certificate2 currentCert = new X509Certificate2(item.Certificate.RawCertificate);

            DialogResult dlgResult = MessageBox.Show("This will terminate the Yubikey, wiping the PIN, PUK, Management Key and Certificates. " +
                                                     "This will also revoke the certificate. Proceeed?" + Environment.NewLine + Environment.NewLine +
                                                     "Will revoke: " + currentCert.Subject + Environment.NewLine +
                                                     "By: " + currentCert.Issuer, "Terminate (WILL revoke)",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (dlgResult != DialogResult.Yes)
                return;

            DlgPleaseInsertYubikey yubiWaiter = new DlgPleaseInsertYubikey(item);
            DialogResult result = yubiWaiter.ShowDialog();

            if (result != DialogResult.OK)
                return;

            DlgProgress prg = new DlgProgress("Terminating certificate");
            prg.WorkerAction = worker =>
            {
                worker.ReportProgress(20, "Revoking certificate ...");

                // Begin
                try
                {
                    CertificateUtilities.RevokeCertificate(item.CA, item.Certificate.Serial);

                    // Revoked
                    _dataStore.Remove(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Unable to revoke certificate " + item.Certificate.Serial + " of " + item.CA +
                        " enrolled on " + item.EnrolledAt + ". There was an error." + Environment.NewLine +
                        Environment.NewLine + ex.Message, "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                // Wipe the Yubikey
                worker.ReportProgress(50, "Wiping Yubikey ...");

                string devName = YubikeyNeoManager.Instance.ListDevices().FirstOrDefault();
                bool hasDevice = !string.IsNullOrEmpty(devName);

                if (hasDevice)
                {
                    using (YubikeyNeoDevice dev = YubikeyNeoManager.Instance.OpenDevice(devName))
                    {
                        int serial = dev.GetSerialNumber();
                        if (item.DeviceSerial != serial)
                        {
                            // Something went seriously wrong - perhaps the user switched keys?
                            MessageBox.Show("Unable to reset the yubikey. The inserted key did not match the key you wanted to wipe.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    using (YubikeyPivDevice piv = YubikeyPivManager.Instance.OpenDevice(devName))
                    {
                        piv.BlockPin();
                        worker.ReportProgress(70);

                        piv.BlockPuk();
                        worker.ReportProgress(90);

                        bool reset = piv.ResetDevice();
                        if (!reset)
                        {
                            MessageBox.Show("Unable to reset the yubikey. Try resetting it manually.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        worker.ReportProgress(100);
                    }
                }

                // Write to disk
                _dataStore.Save(FileStore);
            };

            prg.ShowDialog();

            RefreshUserStore();

            RefreshInsertedKey();
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CSIS Enrollment Station" +
                            Environment.NewLine +
                            Environment.NewLine +
                            "YubiKey 4/NEO have smart card functionality and with this application, you can enroll smart cards on behalf of users " +
                            "when coupled with Windows Active Directory and Microsoft Windows Certificate Services." +
                            Environment.NewLine +
                            Environment.NewLine +
                            "https://github.com/CSIS/EnrollmentStation/" +
                            Environment.NewLine +
                            Environment.NewLine +
                            "Written by Michael Bisbjerg and Ian Qvist", "CSIS Enrollment Station"
                            , MessageBoxButtons.OK);
        }
    }
}