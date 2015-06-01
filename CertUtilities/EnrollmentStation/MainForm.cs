using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class MainForm : Form
    {
        private YubikeyNeoManager _neoManager;
        private DataStore _dataStore;
        private Settings _settings;

        private const string FileStore = "store.xml";
        private const string FileSettings = "settings.xml";

        public MainForm()
        {
            InitializeComponent();
        }

        private void hSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgHsmSettings dialog = new DlgHsmSettings();

            dialog.ShowDialog();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _dataStore = DataStore.Load(FileStore);
            _settings = Settings.Load(FileSettings);

            _neoManager = new YubikeyNeoManager();

            RefreshYubikeyInfo();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _neoManager.Dispose();
        }

        private void cmdYubikeyRefresh_Click(object sender, EventArgs e)
        {
            // TODO: Do on event, instead of button
            RefreshYubikeyInfo();
        }

        private void RefreshYubikeyInfo()
        {
            lblYubikeyStatus.Text = "Updating ...";

            cmdEnableCcid.Enabled = false;
            cmdExportCertificate.Enabled = false;
            cmdViewCertificate.Enabled = false;

            cmdYubikeyEnroll.Enabled = false;
            cmdYubikeyTerminate.Enabled = false;
            cmdYubikeyReset.Enabled = false;

            lblYubikeySerial.Text = string.Empty;
            lblYubikeyMode.Text = string.Empty;
            lblYubikeyCertificateSubject.Text = string.Empty;
            lblYubikeyCertificateIssuer.Text = string.Empty;
            lblYubikeyEnrollState.Text = "<unknown>";
            lblYubikeyPivVersion.Text = string.Empty;

            changeResetPINToolStripMenuItem.Enabled = false;
            importUnknownSmartcardToolStripMenuItem.Enabled = false;

            try
            {
                bool devicePresent = _neoManager.RefreshDevice();

                if (devicePresent)
                {
                    cmdYubikeyReset.Enabled = true;

                    YubicoNeoMode mode = _neoManager.GetMode();
                    int serialNumber = _neoManager.GetSerialNumber();

                    lblYubikeySerial.Text = serialNumber.ToString();
                    lblYubikeyMode.Text = mode.ToString();

                    cmdEnableCcid.Enabled = !YubikeyNeoManager.ModeHasCcid(mode);

                    using (YubikeyPivTool pivTool = YubikeyPivTool.StartPiv())
                    {
                        lblYubikeyPivVersion.Text = pivTool.GetVersion();

                        X509Certificate2 cert = pivTool.GetCertificate9a();

                        if (cert != null)
                        {
                            lblYubikeyCertificateSubject.Text = cert.Subject;
                            lblYubikeyCertificateIssuer.Text = cert.Issuer;

                            cmdExportCertificate.Enabled = true;
                            cmdViewCertificate.Enabled = true;
                        }
                        else
                        {
                            lblYubikeyCertificateSubject.Text = "Not set";
                            lblYubikeyCertificateIssuer.Text = string.Empty;

                            cmdYubikeyEnroll.Enabled = true;
                        }
                    }

                    List<EnrolledYubikey> enrolleds = _dataStore.Search(serialNumber).ToList();

                    if (enrolleds.Any())
                    {
                        lblYubikeyEnrollState.Text = enrolleds.Count + " previous";

                        cmdYubikeyTerminate.Enabled = true;
                        changeResetPINToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        lblYubikeyEnrollState.Text = "No";
                        importUnknownSmartcardToolStripMenuItem.Enabled = true;
                    }
                }
                else
                {
                    lblYubikeySerial.Text = "No device";
                    lblYubikeyMode.Text = string.Empty;

                    lblYubikeyPivVersion.Text = string.Empty;

                    lblYubikeyCertificateSubject.Text = string.Empty;
                    lblYubikeyCertificateIssuer.Text = string.Empty;

                    lblYubikeyEnrollState.Text = string.Empty;
                }

                lblYubikeyStatus.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblYubikeyStatus.Text = ex.Message;
            }
        }

        private void cmdEnableCcid_Click(object sender, EventArgs e)
        {
            lblYubikeyStatus.Text = "Enabling ...";

            DialogResult confirmResult = MessageBox.Show("Enable CCID?", "Enable", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (confirmResult != DialogResult.Yes)
            {
                RefreshYubikeyInfo();
                return;
            }

            try
            {
                YubicoNeoMode currentMode = _neoManager.GetMode();
                YubicoNeoMode newMode = currentMode;
                switch (currentMode)
                {
                    case YubicoNeoMode.OtpOnly:
                        newMode = YubicoNeoMode.OtpCcid;
                        break;
                    case YubicoNeoMode.U2fOnly:
                        newMode = YubicoNeoMode.U2fCcid;
                        break;
                    case YubicoNeoMode.OtpU2f:
                        newMode = YubicoNeoMode.OtpU2fCcid;
                        break;
                    case YubicoNeoMode.OtpOnly_WithEject:
                        newMode = YubicoNeoMode.OtpCcid_WithEject;
                        break;
                    case YubicoNeoMode.U2fOnly_WithEject:
                        newMode = YubicoNeoMode.U2fCcid_WithEject;
                        break;
                    case YubicoNeoMode.OtpU2f_WithEject:
                        newMode = YubicoNeoMode.OtpU2fCcid_WithEject;
                        break;
                    case YubicoNeoMode.CcidOnly:
                    case YubicoNeoMode.OtpCcid:
                    case YubicoNeoMode.U2fCcid:
                    case YubicoNeoMode.OtpU2fCcid:
                    case YubicoNeoMode.CcidOnly_WithEject:
                    case YubicoNeoMode.OtpCcid_WithEject:
                    case YubicoNeoMode.U2fCcid_WithEject:
                    case YubicoNeoMode.OtpU2fCcid_WithEject:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (newMode != currentMode)
                    _neoManager.SetMode(newMode);

                RefreshYubikeyInfo();
            }
            catch (Exception ex)
            {
                lblYubikeyStatus.Text = ex.Message;
            }
        }

        private void cmdExportCertificate_Click(object sender, EventArgs e)
        {
            lblYubikeyStatus.Text = "Saving ..";

            try
            {
                bool devicePresent = _neoManager.RefreshDevice();

                if (!devicePresent)
                {
                    lblYubikeyStatus.Text = "No device";
                    return;
                }

                string id = _neoManager.GetSerialNumber().ToString();
                X509Certificate2 cert;
                using (YubikeyPivTool pivTool = YubikeyPivTool.StartPiv())
                    cert = pivTool.GetCertificate9a();

                if (cert == null)
                {
                    lblYubikeyStatus.Text = "No certificate";
                    return;
                }

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = id + ".crt";

                DialogResult dlgResult = dlg.ShowDialog();

                if (dlgResult != DialogResult.OK)
                {
                    lblYubikeyStatus.Text = string.Empty;
                    return;
                }

                using (Stream fs = dlg.OpenFile())
                {
                    byte[] data = cert.GetRawCertData();
                    fs.Write(data, 0, data.Length);
                }

                lblYubikeyStatus.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblYubikeyStatus.Text = ex.Message;
            }
        }

        private void cmdYubikeyViewCertificate_Click(object sender, EventArgs e)
        {
            lblYubikeyStatus.Text = "Viewing ..";

            try
            {
                bool devicePresent = _neoManager.RefreshDevice();

                if (!devicePresent)
                {
                    lblYubikeyStatus.Text = "No device";
                    return;
                }

                X509Certificate2 cert;
                using (YubikeyPivTool pivTool = YubikeyPivTool.StartPiv())
                    cert = pivTool.GetCertificate9a();

                if (cert == null)
                {
                    lblYubikeyStatus.Text = "No certificate";
                    return;
                }

                X509Certificate2UI.DisplayCertificate(cert);
            }
            catch (Exception ex)
            {
                lblYubikeyStatus.Text = ex.Message;
            }
        }

        private void cmdYubikeyTerminate_Click(object sender, EventArgs e)
        {
            X509Certificate2 currentCert;
            int serial;
            using (YubikeyPivTool piv = new YubikeyPivTool())
                currentCert = piv.GetCertificate9a();

            serial = _neoManager.GetSerialNumber();

            EnrolledYubikey currentEnrolled = _dataStore.Search(serial).SingleOrDefault(s => s.Certificate != null && s.Certificate.Serial == currentCert.SerialNumber);

            if (currentEnrolled == null)
            {
                MessageBox.Show("Unable to find the current Smartcard in the store. It cannot be revoked.", "Error finding Smartcard details", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                return;
            }

            DialogResult dlgResult = MessageBox.Show("This will terminate the Yubikey, wiping the PIN, PUK, Management Key and Certificates. " +
                                                     "This will also revoke the certificiate. Proceeed?" + Environment.NewLine + Environment.NewLine +
                                                     "Will revoke: " + currentCert.Subject + Environment.NewLine +
                                                     "By: " + currentCert.Issuer, "Terminate (WILL revoke)",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

            if (dlgResult != DialogResult.Yes)
                return;

            // Multiple certs
            IEnumerable<EnrolledYubikey> toRevoke = new[] { currentEnrolled };
            List<EnrolledYubikey> previous = _dataStore.Search(serial).ToList();

            {
                int otherCertsPreviouslyEnrolledCount = previous.Count(x => x.Certificate.Serial != currentCert.SerialNumber);
                if (otherCertsPreviouslyEnrolledCount > 0)
                {
                    dlgResult = MessageBox.Show("There has previously been enrolled " + otherCertsPreviouslyEnrolledCount + " certificates for this " +
                                                "device, which have not since been revoked. Revoke these also?", "Revoke excess certificates",
                                                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                    bool revokeAll = dlgResult == DialogResult.Yes;
                    if (revokeAll)
                        toRevoke = toRevoke.Concat(previous);

                    if (dlgResult == DialogResult.Cancel)
                        return;
                }
            }

            // Begin
            bool couldRevokeCurrentCert = false;
            foreach (EnrolledYubikey yubikey in toRevoke)
            {
                try
                {
                    CertificateUtilities.RevokeCertificate(yubikey.CA, yubikey.Certificate.Serial);

                    // Revoked
                    _dataStore.Remove(yubikey);

                    if (yubikey == currentEnrolled)
                        couldRevokeCurrentCert = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Unable to revoke certificate " + yubikey.Certificate.Serial + " of " + yubikey.CA +
                        " enrolled on " + yubikey.EnrolledAt + ". There was an error." + Environment.NewLine +
                        Environment.NewLine + ex.Message, "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (couldRevokeCurrentCert)
            {
                // Wipe the Yubikey
                using (YubikeyPivTool piv = new YubikeyPivTool())
                {
                    piv.BlockPin();
                    piv.BlockPuk();

                    bool reset = piv.ResetDevice();
                    if (!reset)
                    {
                        MessageBox.Show("Unable to reset the yubikey. Try resetting it manually.",
                            "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                // Inform the user, do not wipe
                MessageBox.Show("Unable to revoke the certificate currently on the Yubikey. The yubikey will not be wiped.",
                    "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            _dataStore.Save(FileStore);
            RefreshYubikeyInfo();
        }

        private void revokeLostSmartcardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgRevokeCertificate revokeCertificate = new DlgRevokeCertificate(_dataStore);

            revokeCertificate.ShowDialog();

            if (revokeCertificate.RevokedAny)
            {
                _dataStore.Save(FileStore);
            }
        }

        private void cmdYubikeyEnroll_Click(object sender, EventArgs e)
        {
            DlgEnroll enrollDialog = new DlgEnroll(_settings, _dataStore);

            enrollDialog.ShowDialog();

            _dataStore.Save(FileStore);

            RefreshYubikeyInfo();
        }

        private void cmdYubikeyReset_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show("This will reset the Yubikey, wiping the PIN, PUK, Management Key and Certificates. This will NOT revoke any certifcates. Proceeed?", "Reset (No revoking)", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2);

            if (dlgResult != DialogResult.Yes)
                return;

            lblYubikeyStatus.Text = "Resetting ..";

            try
            {
                bool devicePresent = _neoManager.RefreshDevice();

                if (!devicePresent)
                {
                    lblYubikeyStatus.Text = "No device";
                    return;
                }

                using (YubikeyPivTool pivTool = YubikeyPivTool.StartPiv())
                {
                    // Attempt an invalid PIN X times
                    lblYubikeyStatus.Text = "Resetting PIN ..";

                    pivTool.BlockPin();

                    // Attempt an invalid PUK X times
                    lblYubikeyStatus.Text = "Resetting PUK ..";

                    pivTool.BlockPuk();

                    lblYubikeyStatus.Text = "Resetting device ..";

                    bool resetDevice = pivTool.ResetDevice();

                    if (resetDevice)
                    {
                        lblYubikeyStatus.Text = "Reset sucessfully..";

                        RefreshYubikeyInfo();
                    }
                    else
                    {
                        lblYubikeyStatus.Text = "Something went wrong..";
                    }
                }
            }
            catch (Exception ex)
            {
                lblYubikeyStatus.Text = ex.Message;
            }
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgSettings settingsDialog = new DlgSettings(_settings);

            DialogResult result = settingsDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Save
                _settings.Save(FileSettings);
            }
            else
            {
                // Reload previous
                _settings = Settings.Load(FileSettings);
            }
        }

        private void changeResetPINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgChangePin changePin = new DlgChangePin(_dataStore);

            changePin.ShowDialog();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void importUnknownSmartcardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgImport frm = new DlgImport(_dataStore, _settings);

            DialogResult result = frm.ShowDialog();

            if (result == DialogResult.OK)
            {
                _dataStore.Save(FileStore);
            }

            RefreshYubikeyInfo();
        }
    }
}
