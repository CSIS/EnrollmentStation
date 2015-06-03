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

                DlgPleaseInsertYubikey yubiWaiter = new DlgPleaseInsertYubikey(_yubikey);
                DialogResult result = yubiWaiter.ShowDialog();

                if (result != DialogResult.OK)
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
                X509Certificate2 currentCert = new X509Certificate2(_yubikey.Certificate.RawCertificate);

                DialogResult dlgResult = MessageBox.Show("This will terminate the Yubikey, wiping the PIN, PUK, Management Key and Certificates. " +
                                                         "This will also revoke the certificiate. Proceeed?" + Environment.NewLine + Environment.NewLine +
                                                         "Will revoke: " + currentCert.Subject + Environment.NewLine +
                                                         "By: " + currentCert.Issuer, "Terminate (WILL revoke)",
                                                         MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (dlgResult != DialogResult.Yes)
                    return;

                DlgPleaseInsertYubikey yubiWaiter = new DlgPleaseInsertYubikey(_yubikey);
                DialogResult result = yubiWaiter.ShowDialog();

                if (result != DialogResult.OK)
                    return;

                // Begin
                try
                {
                    CertificateUtilities.RevokeCertificate(_yubikey.CA, _yubikey.Certificate.Serial);

                    // Revoked
                    _dataStore.Remove(_yubikey);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Unable to revoke certificate " + _yubikey.Certificate.Serial + " of " + _yubikey.CA +
                        " enrolled on " + _yubikey.EnrolledAt + ". There was an error." + Environment.NewLine +
                        Environment.NewLine + ex.Message, "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

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

                _dataStore.Save(MainForm.FileStore);
            }
        }
    }
}