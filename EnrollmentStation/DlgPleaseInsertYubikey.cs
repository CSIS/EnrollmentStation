using System;
using System.Linq;
using System.Windows.Forms;
using EnrollmentStation.Code;
using EnrollmentStation.Code.DataObjects;
using EnrollmentStation.Code.Utilities;
using YubicoLib.YubikeyNeo;

namespace EnrollmentStation
{
    public partial class DlgPleaseInsertYubikey : Form
    {
        private readonly EnrolledYubikey _key;

        private bool _hadDevice;
        private bool _hasBeenFound;

        public DlgPleaseInsertYubikey(EnrolledYubikey key)
        {
            _key = key;

            CheckForYubikey();

            InitializeComponent();

            lblSerial.Text = _key.DeviceSerial.ToString();
            lblUsername.Text = _key.Username;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
            string devName = YubikeyNeoManager.Instance.ListDevices().FirstOrDefault();
            _hadDevice = !string.IsNullOrEmpty(devName);

            if (!_hadDevice)
                return;

            using (YubikeyNeoDevice dev = YubikeyNeoManager.Instance.OpenDevice(devName))
            {
                _hasBeenFound = dev.GetSerialNumber() == _key.DeviceSerial;
            }
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
