using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EnrollmentStation.Code;
using EnrollmentStation.Code.DataObjects;
using YubicoLib.YubikeyNeo;
using YubicoLib.YubikeyPiv;

namespace EnrollmentStation
{
    public partial class DlgResetPin : Form
    {
        private readonly Settings _settings;
        private EnrolledYubikey _yubikey;

        public DlgResetPin(Settings settings, EnrolledYubikey key)
        {
            _settings = settings;
            _yubikey = key;

            if (_yubikey == null)
                throw new ArgumentNullException("key");

            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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

            if (!YubikeyPolicyUtility.IsValidPin(txtPinNew.Text))
                eligible = false;

            if (txtPinNew.Text != txtPinNewAgain.Text)
                eligible = false;

            cmdChange.Enabled = eligible;
        }

        private void DlgChangePin_Load(object sender, EventArgs e)
        {
            AcceptButton = cmdChange;

            string devName = YubikeyNeoManager.Instance.ListDevices().FirstOrDefault();
            bool hadDevice = !string.IsNullOrEmpty(devName);

            if (!hadDevice)
                return;

            using (YubikeyNeoDevice dev = YubikeyNeoManager.Instance.OpenDevice(devName))
            {
                int yubiSerial = dev.GetSerialNumber();
                lblSerialNumber.Text = yubiSerial.ToString();
            }
        }

        private void cmdChange_Click(object sender, EventArgs e)
        {
            string devName = YubikeyNeoManager.Instance.ListDevices().FirstOrDefault();
            bool hadDevice = !string.IsNullOrEmpty(devName);

            if (!hadDevice)
                return;

            using (YubikeyPivDevice piv = YubikeyPivManager.Instance.OpenDevice(devName))
            {
                bool authed = piv.Authenticate(_yubikey.ManagementKey);

                if (!authed)
                {
                    MessageBox.Show("Unable to authenticate.", "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Change retries count
                bool setRetries = piv.ChangePinPukRetries(_settings.PinRetries, _settings.PukRetries);

                if (!setRetries)
                    MessageBox.Show("Unable to set PIN/PUK try counts", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Change PIN
                bool changedPin = piv.ChangePin(YubikeyPivDevice.DefaultPin, txtPinNew.Text, out _);
                bool changedPuk = piv.ChangePuk(YubikeyPivDevice.DefaultPuk, _yubikey.PukKey, out _);

                if (changedPin && changedPuk)
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

        private void txtPinNew_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!YubikeyPolicyUtility.IsValidPin(txtPinNew.Text))
            {
                txtPinNew.BackColor = Color.LightCoral;
                e.Cancel = true;
            }
            else
            {
                txtPinNew.BackColor = Color.White;
                e.Cancel = false;
            }
        }

        private void txtPinNewAgain_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtPinNew.Text != txtPinNewAgain.Text)
            {
                txtPinNewAgain.BackColor = Color.LightCoral;
                e.Cancel = true;
            }
            else
            {
                txtPinNewAgain.BackColor = Color.White;
                e.Cancel = false;
            }
        }
    }
}