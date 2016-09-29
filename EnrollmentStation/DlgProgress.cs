using System;
using System.ComponentModel;
using System.Windows.Forms;
using EnrollmentStation.Code;
using EnrollmentStation.Code.Utilities;

namespace EnrollmentStation
{
    public partial class DlgProgress : Form
    {
        private BackgroundWorker _worker;

        public DlgProgress(string title)
        {
            InitializeComponent();

            _worker = new BackgroundWorker();
            _worker.RunWorkerCompleted += WorkerOnRunWorkerCompleted;
            _worker.ProgressChanged += WorkerOnProgressChanged;
            _worker.WorkerReportsProgress = true;

            Text = title;
        }

        private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            string status = progressChangedEventArgs.UserState as string;
            this.InvokeIfNeeded(() =>
            {
                prgProgress.Maximum = 100;
                prgProgress.Value = progressChangedEventArgs.ProgressPercentage;

                if (status != null)
                    lblStatus.Text = status;
            });
        }

        private void WorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            this.InvokeIfNeeded(Close);
        }

        public Action<BackgroundWorker> WorkerAction
        {
            set { _worker.DoWork += (sender, args) => value(_worker); }
        }

        private void DlgProgress_Load(object sender, EventArgs e)
        {
            _worker.RunWorkerAsync();
        }
    }
}
