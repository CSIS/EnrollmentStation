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

        public DlgPleaseInsertYubikey(EnrolledYubikey key)
        {
            _key = key;
            InitializeComponent();

            lblSerial.Text = _key.DeviceSerial.ToString();
            lblUsername.Text = _key.Username;

            _worker.DoWork += WorkerOnDoWork;
            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
            _worker.RunWorkerAsync();

            _neo = new YubikeyNeoManager();
        }

        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            while (true)
            {
                bool hadDevice = _neo.RefreshDevice();

                if (hadDevice)
                {
                    int serial = _neo.GetSerialNumber();
                    if (serial == _key.DeviceSerial)
                    {
                        return;
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
