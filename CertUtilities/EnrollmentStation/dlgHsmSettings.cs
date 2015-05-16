using System;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgHsmSettings : Form
    {
        public DlgHsmSettings()
        {
            InitializeComponent();

            CheckHsmPresent();
        }

        private void CheckHsmPresent()
        {
            lblYubiHsmPresent.Text = HsmRng.IsHsmPresent() ? "Yes" : "No";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckHsmPresent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
