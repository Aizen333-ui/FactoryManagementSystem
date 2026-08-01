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
            // Load default dashboard page when application starts
            LoadPage(new AdminDash());
        }

        // Loads a UserControl page inside the dashboard content area
        public void LoadPage(UserControl page)
        {
            // If no card container exists, load directly into main panel
            if (card == null)
            {
                panelMain.Controls.Clear();
                page.Dock = DockStyle.Fill;
                panelMain.Controls.Add(page);
                page.BringToFront();
                return;
            }

            // Load page inside dashboard card container
            card.Controls.Clear();
            page.Dock = DockStyle.Fill;
            page.Margin = new Padding(0);

            // Add inner spacing around page content
            page.Padding = new Padding(24);
            card.Controls.Add(page);
            page.BringToFront();
        }

        // Updates dashboard header title
        public void SetHeaderTitle(string title)
        {
            lblTitle.Text = title;
        }

        // Opens user management page
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

        // Opens admin account management page
        private void btnAdminAccounts_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnAdminAccounts);
            lblTitle.Text = "Admin Accounts";
            LoadPage(new AdminAccounts());
        }

        // Opens system settings page
        private void btnSystemSettings_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnSystemSettings);
            lblTitle.Text = "System Settings";
            LoadPage(new SystemSettings());
        }

        // Opens reports page
        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnReports);
            lblTitle.Text = "Reports";
            LoadPage(new AdminReports());
        }

        // Opens database backup page
        private void btnBackup_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnBackup);
            lblTitle.Text = "Database Backup";
            LoadPage(new DatabaseBackup());
        }

        // Clears currently selected sidebar button
        public void ResetSidebarSelection()
        {
            if (activeButton != null)
            {
                activeButton.BackColor = Color.Transparent;
                activeButton.ForeColor = Color.White;
                activeButton = null;
            }
        }

        // Handles admin logout process
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Ask for confirmation before closing session
            if (MessageBox.Show(
                    "Do you want to logout?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // Record logout activity in audit logs
            Logger.AddLog(
                Session.CurrentUser,
                "LOGOUT",
                "Security",
                "Admin logged out of the system",
                "Success"
            );

            // Close dashboard and return to login screen
            this.Close();
        }
    }
}