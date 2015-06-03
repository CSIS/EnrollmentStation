using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Forms;

namespace EnrollmentStation
{
    public partial class DlgSelectUser : Form
    {
        public DlgSelectUser()
        {
            InitializeComponent();
        }

        private void DlgSelectUser_Load(object sender, EventArgs e)
        {
            Domain d = Domain.GetCurrentDomain();

            using (var context = new PrincipalContext(ContextType.Domain, d.Name))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        string firstName = GetValue(de.Properties["givenName"]);
                        string lastName = GetValue(de.Properties["sn"]);
                        string samAccountName = GetValue(de.Properties["samAccountName"]);
                        //string userPrincipalName = de.Properties["userPrincipalName"].Value.ToString();

                        ListViewItem item = new ListViewItem();
                        item.Tag = de;
                        item.Text = firstName + " " + lastName + " (" + samAccountName + ")";

                        listView1.Items.Add(item);
                    }
                }
            }
        }

        private string GetValue(PropertyValueCollection item)
        {
            if (item == null || item.Value == null)
                return string.Empty;

            return item.Value.ToString();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public string SelectedUser { get; set; }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
                return;

            DirectoryEntry de = (DirectoryEntry)listView1.SelectedItems[0].Tag;
            SelectedUser = de.Properties["samAccountName"].Value.ToString();
        }
    }
}
