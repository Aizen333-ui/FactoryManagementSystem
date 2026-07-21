using FactoryManagementCore;
using System.Data;

namespace FactoryManagementAdminTool
{
    public partial class AdminReports : UserControl
    {
        public AdminReports()
        {
            InitializeComponent();
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                object totalUsers = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Users", null);
                object activeUsers = DBHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM Users WHERE IsActive = 1", null);
                object disabledUsers = DBHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM Users WHERE IsActive = 0", null);
                object totalAdmins = DBHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM SystemAdmins", null);

                lblTotalUsers.Text = totalUsers?.ToString() ?? "0";
                lblActiveUsers.Text = activeUsers?.ToString() ?? "0";
                lblDisabledUsers.Text = disabledUsers?.ToString() ?? "0";
                lblAdmins.Text = totalAdmins?.ToString() ?? "0";

                DataTable byRole = DBHelper.ExecuteDataTable(
                    @"SELECT Role, COUNT(*) AS Total,
                             SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS Active,
                             SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) AS Disabled
                      FROM Users
                      GROUP BY Role
                      ORDER BY Role",
                    null);

                dgvByRole.DataSource = byRole;

                DataTable recent = DBHelper.ExecuteDataTable(
                    @"SELECT TOP 20 UserID, FullName, Username, Role, IsActive
                      FROM Users
                      ORDER BY UserID DESC",
                    null);

                dgvRecent.DataSource = recent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard = (AdminDashboard)this.FindForm();
            dashboard.ResetSidebarSelection();
            dashboard.SetHeaderTitle("Admin Dashboard");
            dashboard.LoadPage(new AdminDash());
        }
    }
}
