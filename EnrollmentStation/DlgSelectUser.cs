using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Forms;
using EnrollmentStation.Code;

namespace EnrollmentStation
{
    public partial class DlgSelectUser : Form
    {
        public DlgSelectUser()
        {
            InitializeComponent();
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


        private void DlgSelectUser_Load(object sender, EventArgs e)
        {
            AcceptButton = btnOk;

            listBox1.Items.Add("Please wait...");

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
            worker.DoWork += (o, args) =>
            {
                Domain d = Domain.GetCurrentDomain();

                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, d.Name))
                {
                    UserPrincipal p = new UserPrincipal(context);
                    p.Enabled = true;
                    p.Name = "*";

                    using (PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {
                        searcher.QueryFilter = p;
                        PrincipalSearchResult<Principal> results = searcher.FindAll();

                        List<UserContainer> containers = new List<UserContainer>();

                        foreach (Principal result in results)
                        {
                            DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

                            if (de == null)
                                continue;

                            UserContainer container = new UserContainer();

                            container.Name = GetValue(de.Properties["displayName"]);
                            container.Username = GetValue(de.Properties["samAccountName"]);
                            container.DirectoryEntry = de;

                            containers.Add(container);
                        }

                        worker.ReportProgress(100, containers);
                    }
                }
            };

            worker.ProgressChanged += Worker_ProgressChanged;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<UserContainer> containers = (List<UserContainer>)e.UserState;

            listBox1.Items.Clear();

            listBox1.BeginUpdate();
            foreach (UserContainer userContainer in containers)
            {
                listBox1.Items.Add(userContainer);
            }
            listBox1.EndUpdate();
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
                if (!string.IsNullOrWhiteSpace(Name))
                    return Name + " (" + Username + ")";

                return Username;
            }
        }
    }
}