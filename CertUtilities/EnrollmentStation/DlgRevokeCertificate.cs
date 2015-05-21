using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgRevokeCertificate : Form
    {
        private readonly DataStore _dataStore;
        private EnrolledYubikey _currentItem;

        public bool RevokedAny { get; private set; }

        public DlgRevokeCertificate(DataStore dataStore)
        {
            InitializeComponent();

            _dataStore = dataStore;

            Display(null);
            UpdateView();
        }

        private void UpdateView()
        {
            IEnumerable<EnrolledYubikey> items = _dataStore.Search();

            // Apply filters
            if (txtFreetext.Text.Length > 0)
            {
                items =
                    items.Where(s =>
                            (s.Certificate != null &&
                                ((s.Certificate.Serial != null && s.Certificate.Serial.Contains(txtFreetext.Text)) ||
                                (s.Certificate.Thumbprint != null && s.Certificate.Thumbprint.Contains(txtFreetext.Text)) ||
                                (s.Certificate.Issuer != null && s.Certificate.Issuer.Contains(txtFreetext.Text)) ||
                                (s.Certificate.Subject != null && s.Certificate.Subject.Contains(txtFreetext.Text)))
                                ) ||
                            s.DeviceSerial.ToString().Contains(txtFreetext.Text) ||
                            s.CA.Contains(txtFreetext.Text) ||
                            s.ManagementKey.Contains(txtFreetext.Text) ||
                            s.PukKey.Contains(txtFreetext.Text) ||
                            s.Username.Contains(txtFreetext.Text));
            }

            // Update views
            lstItems.Items.Clear();

            foreach (EnrolledYubikey item in items)
            {
                ListViewItem lsItem = new ListViewItem();

                lsItem.Tag = item;

                // Fields
                // Serial
                lsItem.Text = item.DeviceSerial.ToString();

                // User
                lsItem.SubItems.Add(item.Username);

                // Enrolled
                lsItem.SubItems.Add(item.EnrolledAt.ToLocalTime().ToString());

                // Certificate Serial
                lsItem.SubItems.Add(item.Certificate != null ? item.Certificate.Serial : "<unknown>");

                lstItems.Items.Add(lsItem);
            }
        }

        private void Display(EnrolledYubikey item)
        {
            cmdRevoke.Enabled = false;

            lblDisplaySerial.Text = "";
            lblDisplayUser.Text = "";
            lblDisplayEnrolled.Text = "";

            lblDisplayCertSerial.Text = "";
            lblDisplayCertThumbprint.Text = "";
            lblDisplayCA.Text = "";

            lblDisplayVersionNeo.Text = "";
            lblDisplayVersionPiv.Text = "";

            _currentItem = item;

            if (item == null)
                return;

            cmdRevoke.Enabled = _currentItem.Certificate != null;

            lblDisplaySerial.Text = item.DeviceSerial.ToString();
            lblDisplayUser.Text = item.Username;
            lblDisplayEnrolled.Text = item.EnrolledAt.ToLocalTime().ToString();

            if (item.Certificate != null)
            {
                lblDisplayCertSerial.Text = item.Certificate.Serial;
                lblDisplayCertThumbprint.Text = item.Certificate.Thumbprint;
            }

            lblDisplayCA.Text = item.CA;

            lblDisplayVersionNeo.Text = item.YubikeyVersions.NeoFirmware;
            lblDisplayVersionPiv.Text = item.YubikeyVersions.PivApplet;
        }

        private void txtFreetext_TextChanged(object sender, System.EventArgs e)
        {
            UpdateView();
        }

        private void lstItems_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lstItems.SelectedItems.Count < 1)
            {
                Display(null);
                return;
            }

            EnrolledYubikey item = lstItems.SelectedItems[0].Tag as EnrolledYubikey;

            Display(item);
        }

        private void cmdRevoke_Click(object sender, System.EventArgs e)
        {
            if (_currentItem == null)
                // Somebody be hacking
                return;

            if (_currentItem.Certificate == null)
                return;

            DialogResult dlgResult = MessageBox.Show("Revoke the certificate enrolled at " + _currentItem.EnrolledAt.ToLocalTime() + " for " + _currentItem.Username + ". This action will revoke " +
                            "the certificate, but will NOT wipe the yubikey." + Environment.NewLine + Environment.NewLine +
                            "Certificate: " + _currentItem.Certificate.Serial + Environment.NewLine +
                            "Subject: " + _currentItem.Certificate.Subject + Environment.NewLine +
                            "Issuer: " + _currentItem.Certificate.Issuer + Environment.NewLine +
                            "Valid: " + _currentItem.Certificate.StartDate + " to " + _currentItem.Certificate.ExpireDate,
                            "Revoke certificate?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (dlgResult != DialogResult.Yes)
                return;

            try
            {
                CertificateUtilities.RevokeCertificate(_currentItem.CA, _currentItem.Certificate.Serial);

                RevokedAny = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("We were unable to revoke the certificate. Details: " + ex.Message, "An error occurred.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            // Remove the item from the datastore
            _dataStore.Remove(_currentItem);

            // Update the view
            UpdateView();
            Display(null);
        }
    }
}
