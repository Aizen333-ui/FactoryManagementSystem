using FactoryManagementCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
 
            InitializeComponent();
            LoadPage(new AdminDash());
        }

        public void LoadPage(UserControl page)
        {
            if (card == null)
            {
                panelMain.Controls.Clear();
                page.Dock = DockStyle.Fill;
                panelMain.Controls.Add(page);
                page.BringToFront();
                return;
            }

            card.Controls.Clear();
            page.Dock = DockStyle.Fill;
            page.Margin = new Padding(0);
            page.Padding = new Padding(24);
            card.Controls.Add(page);
            page.BringToFront();
        }

        public void SetHeaderTitle(string title)
        {
            lblTitle.Text = title;
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            OpenManageUsers();
        }

        public void OpenManageUsers()
        {
            SetActiveButton(btnManageUsers);
            lblTitle.Text = "Manage Users";
            LoadPage(new ManageUsers());
        }

        private void btnAdminAccounts_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAdminAccounts);
            lblTitle.Text = "Admin Accounts";
            LoadPage(new AdminAccounts());
        }

        private void btnSystemSettings_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSystemSettings);
            lblTitle.Text = "System Settings";
            LoadPage(new SystemSettings());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnReports);
            lblTitle.Text = "Reports";
            LoadPage(new AdminReports());
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnBackup);
            lblTitle.Text = "Database Backup";
            LoadPage(new DatabaseBackup());
        }

        public void ResetSidebarSelection()
        {
            if (activeButton != null)
            {
                activeButton.BackColor = Color.Transparent;
                activeButton.ForeColor = Color.White;
                activeButton = null;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Do you want to logout?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            Logger.AddLog(
                Session.CurrentUser,
                "LOGOUT",
                "Security",
                "Admin logged out of the system",
                "Success"
            );

            this.Close();
        }
    }
}
