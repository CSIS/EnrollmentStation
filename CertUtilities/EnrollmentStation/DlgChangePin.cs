using System;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgChangePin : Form
    {
        private EnrolledYubikey _yubikey;

        public DlgChangePin(EnrolledYubikey key)
        {
            if (_yubikey == null)
                throw new ArgumentNullException("key");

            _yubikey = key;

            InitializeComponent();
        }

        private void DlgChangePin_Load(object sender, EventArgs e)
        {
            using (YubikeyNeoManager neo = new YubikeyNeoManager())
            {
                bool haveDevice = neo.RefreshDevice();

                if (!haveDevice)
                {
                    DlgPleaseInsertYubikey dialog = new DlgPleaseInsertYubikey(_yubikey);
                    dialog.ShowDialog();
                }

                int yubiSerial = neo.GetSerialNumber();
                lblSerialNumber.Text = yubiSerial.ToString();
            }

            int remainingTries;

            using (YubikeyPivTool piv = new YubikeyPivTool())
                remainingTries = piv.GetPinTriesLeft();

            lblPinTriesLeft.Text = remainingTries.ToString();

            if (!string.IsNullOrEmpty(txtPinNew.Text) && txtPinNew.Text == txtPinNewAgain.Text)
            {
                if (!string.IsNullOrEmpty(txtPinOld.Text) && remainingTries > 0)
                    cmdChange.Enabled = true;
            }
        }

        private void cmdChange_Click(object sender, EventArgs e)
        {
            using (YubikeyPivTool piv = new YubikeyPivTool())
            {
                bool authed = piv.Authenticate(_yubikey.ManagementKeyBytes);

                if (!authed)
                {
                    MessageBox.Show("Unable to authenticate.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Change PIN
                int remainingTries;
                bool changed = piv.ChangePin(txtPinOld.Text, txtPinNew.Text, out remainingTries);

                if (changed)
                    MessageBox.Show("The PIN code has been changed.", "Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("An error occured while changing the PIN code. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void textField_Changed(object sender, EventArgs e)
        {
            //TODO: Check pin
        }
    }
}