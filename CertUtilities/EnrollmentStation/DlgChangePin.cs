using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgChangePin : Form
    {
        private EnrolledYubikey _yubikey;
        private readonly DataStore _store;

        public DlgChangePin(DataStore store)
        {
            InitializeComponent();

            _store = store;
            using (YubikeyNeoManager neo = new YubikeyNeoManager())
            {
                bool hadDevice = neo.RefreshDevice();
                if (!hadDevice)
                {
                    DialogResult = DialogResult.Abort;
                    return;
                }

                int serial = neo.GetSerialNumber();
                List<EnrolledYubikey> keys = _store.Search(serial).ToList();

                _yubikey = keys.Count == 1 ? keys.First() : null;

                if (_yubikey == null)
                {
                    // Try matching a single EnrolledKey by its certificate
                    X509Certificate2 currentCert;
                    using (YubikeyPivTool piv = new YubikeyPivTool())
                        currentCert = piv.GetCertificate9a();

                    if (currentCert != null)
                    {
                        List<EnrolledYubikey> eligible = keys.Where(s=>s.CertificateThumbprint == currentCert.Thumbprint).ToList();

                        if (eligible.Count == 1)
                            _yubikey = eligible.First();
                    }
                }

                lblSerialNumber.Text = serial.ToString();

                if (_yubikey == null)
                {
                    lblResetPossible.Text = "Not possible";
                    lblInstructions.Text = "Enter the old PIN and the new desired PIN to change it.";
                }
                else
                {
                    lblResetPossible.Text = "Possible";
                    lblInstructions.Text = "Enter the old PIN and the new desired PIN to change it, or just the new PIN to reset it.";
                }
            }

            UpdateView();
        }

        private void UpdateView()
        {
            cmdChange.Enabled = false;
            cmdReset.Enabled = false;

            int remainingTries;
            using (YubikeyPivTool piv = new YubikeyPivTool())
                remainingTries = piv.GetPinTriesLeft();

            lblPinTriesLeft.Text = remainingTries.ToString();

            if (!string.IsNullOrEmpty(txtPinNew.Text) && txtPinNew.Text == txtPinNewAgain.Text)
            {
                cmdReset.Enabled = _yubikey != null;

                if (!string.IsNullOrEmpty(txtPinOld.Text) && remainingTries > 0)
                    cmdChange.Enabled = true;
            }
        }

        private void cmdChange_Click(object sender, EventArgs e)
        {
            using (YubikeyPivTool piv = new YubikeyPivTool())
            {
                bool authed = piv.Authenticate(Utilities.StringToByteArray(_yubikey.ManagementKey));
                if (!authed)
                {
                    MessageBox.Show("Unable to authenticate.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Change PIN
                int remainingTries;
                bool changed = piv.ChangePin(txtPinOld.Text, txtPinNew.Text, out remainingTries);

                if (changed)
                {
                    MessageBox.Show("Changed the PIN.", "Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("The new PIN was not set. There are " + remainingTries + " tries left before it is blocked.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    UpdateView();
                }
            }
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            using (YubikeyPivTool piv = new YubikeyPivTool())
            {
                bool authed = piv.Authenticate(Utilities.StringToByteArray(_yubikey.ManagementKey));
                if (!authed)
                {
                    MessageBox.Show("Unable to authenticate.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Block PIN
                piv.BlockPin();

                // Change PIN
                bool changed = piv.UnblockPin(_yubikey.PukKey, txtPinNew.Text);

                if (changed)
                {
                    MessageBox.Show("Changed the PIN.", "Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("The code was not set. Something went wrong - the key may be blocked.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    UpdateView();
                }
            }
        }

        private void textField_Changed(object sender, EventArgs e)
        {
            UpdateView();
        }
    }
}
