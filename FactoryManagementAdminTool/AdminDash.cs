using System.Data;
using FactoryManagementCore;

namespace FactoryManagementAdminTool
{
    public partial class AdminDash : UserControl
    {
        public AdminDash()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                // Total Users

                DataTable total = DBHelper.ExecuteDataTable(
                    "SELECT COUNT(*) AS Total FROM Users", null);

                lblTotalUsers.Text = total.Rows[0]["Total"].ToString();

                // Active Users

                DataTable active = DBHelper.ExecuteDataTable(
                    "SELECT COUNT(*) AS Total FROM Users WHERE IsActive = 1", null);

                lblActiveUsers.Text = active.Rows[0]["Total"].ToString();

                // Disabled Users

                DataTable disabled = DBHelper.ExecuteDataTable(
                    "SELECT COUNT(*) AS Total FROM Users WHERE IsActive = 0", null);

                lblDisabledUsers.Text = disabled.Rows[0]["Total"].ToString();

                // System Admins
                DataTable admins = DBHelper.ExecuteDataTable(
                        "SELECT COUNT(*) AS Total FROM SystemAdmins", null);

                lblSystemAdmin.Text = admins.Rows[0]["Total"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Dashboard loading error: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

    }
}