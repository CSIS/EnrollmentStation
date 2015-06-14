using System;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgPleaseInsertYubikey : Form
    {
        private readonly EnrolledYubikey _key;

        private YubikeyNeoManager _neo;

        private bool _hadDevice;
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

        public new DialogResult ShowDialog()
        {
            if (_hasBeenFound)
            {
                DialogResult = DialogResult.OK;
                return DialogResult;
            }

            return base.ShowDialog();
        }

        private void DlgPleaseInsertYubikey_Load(object sender, EventArgs e)
        {
            YubikeyDetector.Instance.StateChanged += YubikeyStateChanged;
            YubikeyDetector.Instance.Start();

            UpdateView();
        }

        private void DlgPleaseInsertYubikey_FormClosing(object sender, FormClosingEventArgs e)
        {
            YubikeyDetector.Instance.StateChanged -= YubikeyStateChanged;
        }

        private void YubikeyStateChanged()
        {
            CheckForYubikey();

            this.InvokeIfNeeded(() =>
            {
                UpdateView();

                if (_hasBeenFound)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            });
        }

        private void CheckForYubikey()
        {
            _hadDevice = _neo.RefreshDevice();
            if (!_hadDevice)
                return;

            _hasBeenFound = _neo.GetSerialNumber() == _key.DeviceSerial;
        }

        private void UpdateView()
        {
            if (!_hadDevice)
            {
                lblStatus.Text = "No device .. Please insert a device.";
                return;
            }

            if (!_hasBeenFound)
            {
                lblStatus.Text = "Incorrect device inserted ...";
                return;
            }

            lblStatus.Text = string.Empty;
        }
    }
}
