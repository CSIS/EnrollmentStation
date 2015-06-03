using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgPleaseInsertYubikey : Form
    {
        private readonly EnrolledYubikey _key;

        public DlgPleaseInsertYubikey(EnrolledYubikey key)
        {
            _key = key;
            InitializeComponent();

            lblSerial.Text = _key.DeviceSerial.ToString();
            lblUsername.Text = _key.Username;
        }
    }
}
