using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgRevokeCertificate : Form
    {
        private readonly DataStore _dataStore;
        private readonly EnrolledYubikey _yubikey;
        private YubikeyNeoManager _neoManager;

        public DlgRevokeCertificate(DataStore dataStore, EnrolledYubikey yubikey)
        {
            InitializeComponent();

            _dataStore = dataStore;
            _yubikey = yubikey;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _neoManager = new YubikeyNeoManager();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _neoManager.Dispose();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (rbReset.Checked)
            {
                DialogResult dlgResult = MessageBox.Show("This will reset the Yubikey, wiping the PIN, PUK, management key and certificates. This will NOT revoke the certifcates. Proceeed?", "Reset YubiKey", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2);

                if (dlgResult != DialogResult.Yes)
                    return;

                bool devicePresent = _neoManager.RefreshDevice();

                if (!devicePresent)
                    return;

                using (YubikeyPivTool pivTool = YubikeyPivTool.StartPiv())
                {
                    // Attempt an invalid PIN X times
                    pivTool.BlockPin();

                    // Attempt an invalid PUK X times
                    pivTool.BlockPuk();

                    bool resetDevice = pivTool.ResetDevice();
                }
            }

            if (rbRevoke.Checked)
            {
                DialogResult dlgResult = MessageBox.Show("Revoke the certificate enrolled at " + _yubikey.EnrolledAt.ToLocalTime() + " for " + _yubikey.Username + ". This action will revoke " +
                    "the certificate, but will NOT wipe the yubikey." + Environment.NewLine + Environment.NewLine +
                    "Certificate: " + _yubikey.Certificate.Serial + Environment.NewLine +
                    "Subject: " + _yubikey.Certificate.Subject + Environment.NewLine +
                    "Issuer: " + _yubikey.Certificate.Issuer + Environment.NewLine +
                    "Valid: " + _yubikey.Certificate.StartDate + " to " + _yubikey.Certificate.ExpireDate,
                    "Revoke certificate?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (dlgResult != DialogResult.Yes)
                    return;

                try
                {
                    CertificateUtilities.RevokeCertificate(_yubikey.CA, _yubikey.Certificate.Serial);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("We were unable to revoke the certificate. Details: " + ex.Message, "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Remove the item from the datastore
                _dataStore.Remove(_yubikey);
            }

            if (rbTerminate.Checked)
            {
                X509Certificate2 currentCert;

                using (YubikeyPivTool piv = new YubikeyPivTool())
                    currentCert = piv.GetCertificate9a();

                var serial = _neoManager.GetSerialNumber();

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
                    MessageBox.Show("Unable to revoke the certificate currently on the Yubikey. The yubikey will not be wiped.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                _dataStore.Save(MainForm.FileStore);
            }
        }
    }
}