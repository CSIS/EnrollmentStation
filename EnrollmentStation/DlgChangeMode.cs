using System;
using System.Drawing;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgChangeMode : Form
    {
        private YubicoNeoMode _currentMode;

        private bool _stateWaitingToRemove = false;
        private bool _deferCheckboxEvents = false;

        public DlgChangeMode()
        {
            InitializeComponent();

            _currentMode = new YubicoNeoMode(YubicoNeoModeEnum.OtpOnly);
            SetStatus(Color.Yellow, "Insert a Yubikey");
        }

        private void SetStatus(string text)
        {
            SetStatus(BackColor, text);
        }

        private void SetStatus(Color color, string text)
        {
            lblStatus.Text = text;
            lblStatus.BackColor = color;
        }

        private void DlgChangeMode_Load(object sender, EventArgs e)
        {
            AcceptButton = cmdChange;

            YubikeyDetector.Instance.StateChanged += InstanceOnStateChanged;
            YubikeyDetector.Instance.Start();

            UpdateCurrentView();
        }

        private void DlgChangeMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            YubikeyDetector.Instance.StateChanged -= InstanceOnStateChanged;
        }

        private void InstanceOnStateChanged()
        {
            if (_stateWaitingToRemove && !YubikeyDetector.Instance.CurrentState)
            {
                // The current key has been removed - reset the state
                _stateWaitingToRemove = false;
            }

            this.InvokeIfNeeded(UpdateCurrentView);
        }

        private void UpdateCurrentView()
        {
            if (_stateWaitingToRemove)
                // Don't update the UI - wait for the user to remove the current Yubikey
                return;

            using (YubikeyDetector.Instance.GetExclusiveLock())
            {
                bool device = YubikeyNeoManager.Instance.RefreshDevice();

                foreach (Control control in grpChangeMode.Controls)
                    control.Enabled = device;

                if (!device)
                {
                    SetStatus(Color.Yellow, "Insert a Yubikey");
                    return;
                }

                _currentMode = YubikeyNeoManager.Instance.GetMode();

                _deferCheckboxEvents = true;
                chkOTP.Checked = _currentMode.HasOtp;
                chkCCID.Checked = _currentMode.HasCcid;
                chkU2f.Checked = _currentMode.HasU2f;
                chkEject.Checked = _currentMode.HasEjectMode;
                _deferCheckboxEvents = false;

                SetStatus(Color.GreenYellow, "Currently set to " + _currentMode);
            }
        }

        private void checkBox_Changed(object sender, EventArgs e)
        {
            if (_deferCheckboxEvents)
                return;

            _currentMode.HasOtp = chkOTP.Checked;
            _currentMode.HasCcid = chkCCID.Checked;
            _currentMode.HasU2f = chkU2f.Checked;
            _currentMode.HasEjectMode = chkEject.Checked;

            // Enable Eject mode if CCID is enabled (the view is not locked) and if CCID is checked (Eject applies only to CCID)
            chkEject.Enabled = chkCCID.Checked && chkCCID.Enabled;

            cmdChange.Enabled = _currentMode.IsValid;
        }

        private void cmdChange_Click(object sender, EventArgs e)
        {
            SetStatus(Color.Orange, "DO NOT REMOVE THE YUBIKEY");

            using (YubikeyDetector.Instance.GetExclusiveLock())
            {
                bool device = YubikeyNeoManager.Instance.RefreshDevice();
                if (!device)
                {
                    SetStatus(Color.Yellow, "Insert a Yubikey");
                    return;
                }

                try
                {
                    YubikeyNeoManager.Instance.SetMode(_currentMode.Mode);
                    SetStatus(Color.GreenYellow, "The mode was set. Please remove the Yubikey from the system.");
                }
                catch (Exception ex)
                {
                    SetStatus(Color.Red, "Was unable to set the mode, please remove the Yubikey. Details: " + ex.Message);
                }

                _stateWaitingToRemove = true;
                foreach (Control control in grpChangeMode.Controls)
                    control.Enabled = false;
            }
        }
    }
}
