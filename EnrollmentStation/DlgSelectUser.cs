using System;
using System.ComponentModel;
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
            AcceptButton = btnOk;

            listBox1.Items.Add("Please wait...");

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerAsync();

            worker.DoWork += (o, args) =>
            {
                Domain d = Domain.GetCurrentDomain();

                using (var context = new PrincipalContext(ContextType.Domain, d.Name))
                {
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {
                        PrincipalSearchResult<Principal> results = searcher.FindAll();
                        listBox1.Items.Clear();
                        listBox1.BeginUpdate();

                        foreach (var result in results)
                        {
                            DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

                            UserContainer container = new UserContainer();
                            container.Name = GetValue(de.Properties["givenName"]) + " " + GetValue(de.Properties["sn"]);
                            container.Username = GetValue(de.Properties["samAccountName"]);
                            container.DirectoryEntry = de;

                            listBox1.Items.Add(container);
                        }

                        listBox1.EndUpdate();
                    }
                }
            };
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count <= 0)
                return;

            UserContainer user = (UserContainer)listBox1.SelectedItems[0];
            SelectedUser = user.Username;
        }

        private class UserContainer
        {
            public DirectoryEntry DirectoryEntry { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name + " (" + Username + ")";
            }
        }
    }
}
