using System;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgResetPin : Form
    {
        private EnrolledYubikey _yubikey;

        public DlgResetPin(EnrolledYubikey key)
        {
            _yubikey = key;

            if (_yubikey == null)
                throw new ArgumentNullException("key");

            InitializeComponent();
        }

        public new DialogResult ShowDialog()
        {
            DlgPleaseInsertYubikey dialog = new DlgPleaseInsertYubikey(_yubikey);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                DialogResult = DialogResult.Cancel;
                return DialogResult;
            }

            return base.ShowDialog();
        }

        private void RefreshEligibilityForReset()
        {
            bool eligible = true;

            if (txtPinNew.Text.Length <= 0)
                eligible = false;

            if (txtPinNew.Text != txtPinNewAgain.Text)
                eligible = false;

            cmdChange.Enabled = eligible;
        }

        private void DlgChangePin_Load(object sender, EventArgs e)
        {
            using (YubikeyDetector.Instance.GetExclusiveLock())
            using (YubikeyNeoManager neo = new YubikeyNeoManager())
            {
                neo.RefreshDevice();

                int yubiSerial = neo.GetSerialNumber();
                lblSerialNumber.Text = yubiSerial.ToString();
            }
        }

        private void cmdChange_Click(object sender, EventArgs e)
        {
            using (YubikeyDetector.Instance.GetExclusiveLock())
            using (YubikeyPivTool piv = new YubikeyPivTool())
            {
                bool authed = piv.Authenticate(_yubikey.ManagementKey);

                if (!authed)
                {
                    MessageBox.Show("Unable to authenticate.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Reset PIN
                piv.BlockPin();

                bool changed = piv.UnblockPin(_yubikey.PukKey, txtPinNew.Text);

                if (changed)
                {
                    MessageBox.Show("The PIN code has been reset.", "Success.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Close();
                }
                else
                    MessageBox.Show("An error occured while resetting the PIN code. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void textField_Changed(object sender, EventArgs e)
        {
            RefreshEligibilityForReset();
        }
    }
}