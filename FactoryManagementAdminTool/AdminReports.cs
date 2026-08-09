using FactoryManagementCore;
using System.Data;

namespace FactoryManagementAdminTool
{
    public partial class AdminReports : UserControl
    {
        public AdminReports()
        {
            InitializeComponent();

            dgvByRole.ReadOnly = true;
            dgvByRole.MultiSelect = false;
            dgvByRole.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvRecent.ReadOnly = true;
            dgvRecent.MultiSelect = false;
            dgvRecent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadReport();

            this.HandleCreated += AdminReports_HandleCreated;

            dgvByRole.DataBindingComplete += DgvByRole_DataBindingComplete;
            dgvRecent.DataBindingComplete += DgvRecent_DataBindingComplete;
        }
        // Clears selection in DataGridViews when the control is created
        private void AdminReports_HandleCreated(object sender, EventArgs e)
        {
            dgvByRole.ClearSelection();
            dgvByRole.CurrentCell = null;

            dgvRecent.ClearSelection();
            dgvRecent.CurrentCell = null;
        }
        // Clears selection in DataGridViews after data binding is complete for role-based statistics
        private void DgvByRole_DataBindingComplete(
            object sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            dgvByRole.ClearSelection();
            dgvByRole.CurrentCell = null;
        }
        // Clears selection in DataGridViews after data binding is complete for recent users
        private void DgvRecent_DataBindingComplete(
            object sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            dgvRecent.ClearSelection();
            dgvRecent.CurrentCell = null;
        }

        // Loads dashboard statistics and report tables
        private void LoadReport()
        {
            try
            {
                // Get user statistics
                object totalUsers =
                    DBHelper.ExecuteScalar(
                        "SELECT COUNT(*) FROM Users",
                        null);


                object activeUsers =
                    DBHelper.ExecuteScalar(
                        "SELECT COUNT(*) FROM Users WHERE IsActive = 1",
                        null);


                object disabledUsers =
                    DBHelper.ExecuteScalar(
                        "SELECT COUNT(*) FROM Users WHERE IsActive = 0",
                        null);

                // Get total administrator accounts
                object totalAdmins =
                    DBHelper.ExecuteScalar(
                        "SELECT COUNT(*) FROM SystemAdmins",
                        null);

                // Update summary labels
                lblTotalUsers.Text =
                    totalUsers?.ToString() ?? "0";

                lblActiveUsers.Text =
                    activeUsers?.ToString() ?? "0";

                lblDisabledUsers.Text =
                    disabledUsers?.ToString() ?? "0";

                lblAdmins.Text =
                    totalAdmins?.ToString() ?? "0";

                // Load user statistics grouped by role
                DataTable byRole =
                    DBHelper.ExecuteDataTable(
                        @"SELECT Role,
                                 COUNT(*) AS Total,
                                 SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS Active,
                                 SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) AS Disabled
                          FROM Users
                          GROUP BY Role
                          ORDER BY Role",
                        null);

                dgvByRole.DataSource = byRole;
                dgvByRole.ClearSelection();
                dgvByRole.CurrentCell = null;
                // Load latest registered users
                DataTable recent =
                    DBHelper.ExecuteDataTable(
                        @"SELECT TOP 20
                                 UserID,
                                 FullName,
                                 Username,
                                 Role,
                                 IsActive
                          FROM Users
                          ORDER BY UserID DESC",
                        null);

                dgvRecent.DataSource = recent;
                dgvRecent.ClearSelection();
                dgvRecent.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading report: " + ex.Message);
            }
        }

        // Refreshes report information manually
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        // Returns user to the main admin dashboard
        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard =
                (AdminDashboard)this.FindForm();

            // Reset sidebar state
            dashboard.ResetSidebarSelection();

            // Restore dashboard title
            dashboard.SetHeaderTitle(
                "Admin Dashboard");

            // Load default dashboard page
            dashboard.LoadPage(
                new AdminDash());
        }
    }
}