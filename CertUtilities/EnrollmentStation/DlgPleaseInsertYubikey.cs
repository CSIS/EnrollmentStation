using System;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgPleaseInsertYubikey : Form
    {
        private readonly EnrolledYubikey _key;

        private YubikeyNeoManager _neo;

        private bool _hasBeenFound;

        public DlgPleaseInsertYubikey(EnrolledYubikey key)
        {
            _key = key;
            _neo = new YubikeyNeoManager();

            CheckForYubikey();

            InitializeComponent();

            lblSerial.Text = _key.DeviceSerial.ToString();
            lblUsername.Text = _key.Username;
        }

        private void DlgPleaseInsertYubikey_Load(object sender, EventArgs e)
        {
            YubikeyDetector.Instance.StateChanged += YubikeyStateChanged;
            YubikeyDetector.Instance.Start();
        }

        private void DlgPleaseInsertYubikey_FormClosing(object sender, FormClosingEventArgs e)
        {
            YubikeyDetector.Instance.StateChanged -= YubikeyStateChanged;
        }

        private void YubikeyStateChanged()
        {
            CheckForYubikey();

            if (InvokeRequired)
                Invoke((Action)(() =>
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }));
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public new DialogResult ShowDialog()
        {
            if (_hasBeenFound)
            {
                DialogResult = DialogResult.OK;
                return DialogResult;
            }

            return base.ShowDialog();
        }

        private void CheckForYubikey()
        {
            bool hadDevice = _neo.RefreshDevice();
            if (!hadDevice)
                return;

            int serial = _neo.GetSerialNumber();
            if (serial != _key.DeviceSerial) 
                return;

            // Success
            _hasBeenFound = true;
        }
    }
}
