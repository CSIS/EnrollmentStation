using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgPleaseInsertYubikey : Form
    {
        private readonly EnrolledYubikey _key;

        private BackgroundWorker _worker = new BackgroundWorker();
        private YubikeyNeoManager _neo;

        private bool _hasBeenFound = false;

        public DlgPleaseInsertYubikey(EnrolledYubikey key)
        {
            _key = key;
            _neo = new YubikeyNeoManager();

            CheckForYubikey();

            InitializeComponent();

            lblSerial.Text = _key.DeviceSerial.ToString();
            lblUsername.Text = _key.Username;

            _worker.DoWork += WorkerOnDoWork;
            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
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
            _worker.RunWorkerAsync();
        }

        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            DialogResult = DialogResult.OK;
            Close();
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

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (!_hasBeenFound)
            {
                Thread.Sleep(1000);

                CheckForYubikey();
            }
        }
    }
}
