using FactoryManagementCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryManagementAdminTool
{
    public partial class SystemSettings : UserControl
    {
        public SystemSettings()
        {
            InitializeComponent();
            LoadSettings();
            LoadAuditLogs();
   
        }

        private void LoadSettings()
        {
            txtConnection.Text = DBHelper.ConnectionString;

            try
            {
                object users = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Users", null);
                object admins = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM SystemAdmins", null);
                lblStats.Text = $"Users: {users}   |   Admins: {admins}";
                lblStatus.Text = "Database connection: OK";
                lblStatus.ForeColor = Color.FromArgb(22, 163, 74);
            }
            catch (Exception ex)
            {
                lblStats.Text = "Unable to load stats.";
                lblStatus.Text = "Database connection: FAILED — " + ex.Message;
                lblStatus.ForeColor = Color.FromArgb(220, 38, 38);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DBHelper.ConnectionString))
                {
                    con.Open();
                }

                MessageBox.Show(
                    "Connection successful.",
                    "System Settings",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadSettings();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Connection failed:\n" + ex.Message,
                    "System Settings",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
               
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSettings();
            LoadAuditLogs();
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard = (AdminDashboard)this.FindForm();
            dashboard.ResetSidebarSelection();
            dashboard.SetHeaderTitle("Admin Dashboard");
            dashboard.LoadPage(new AdminDash());
            
        }

        private void btnReloadLogs_Click(object sender, EventArgs e)
        {
            LoadAuditLogs();
            
        }
        private void btnExportLogs_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAuditLogs.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No logs available to export.",
                        "Export Logs",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }


                SaveFileDialog save = new SaveFileDialog();

                save.Filter = "Text Files (*.txt)|*.txt";
                save.FileName = "FactoryManagement_Logs_" +
                                DateTime.Now.ToString("yyyyMMdd_HHmmss")
                                + ".txt";


                if (save.ShowDialog() != DialogResult.OK)
                    return;



                using (StreamWriter writer = new StreamWriter(save.FileName))
                {

                    writer.WriteLine(
                        "====================================");

                    writer.WriteLine(
                        " Factory Management System Logs");

                    writer.WriteLine(
                        " Generated: " + DateTime.Now);

                    writer.WriteLine(
                        "====================================");


                    writer.WriteLine();



                    foreach (DataGridViewRow row in dgvAuditLogs.Rows)
                    {

                        if (row.IsNewRow)
                            continue;


                        string logLine =
                            "Date: " + row.Cells["LogDate"].Value +
                            " | User: " + row.Cells["Username"].Value +
                            " | Action: " + row.Cells["Action"].Value +
                            " | Status: " + row.Cells["Status"].Value;


                        writer.WriteLine(logLine);

                        writer.WriteLine(
                            "------------------------------------");

                    }

                }


                Logger.AddLog(
                    Session.CurrentUser,
                    "Export Logs",
                    "System Settings",
                    "System logs exported successfully",
                    "Success"
                );


                MessageBox.Show(
                    "Logs exported successfully.",
                    "Export Logs",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {

                Logger.AddLog(
                    Session.CurrentUser,
                    "Export Logs",
                    "System Settings",
                    ex.Message,
                    "Failed"
                );


                MessageBox.Show(
                    "Error exporting logs:\n" + ex.Message,
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            }
        }
        private void LoadAuditLogs()
        {
            try
            {
                string query = @"
                            SELECT 
                                LogID,
                                Username,
                                Action,
                                Module,
                                Description,
                                Status,
                                LogDate
                            FROM AuditLogs
                            ORDER BY LogID DESC";

                DataTable dt = DBHelper.ExecuteDataTable(query, null);

                dgvAuditLogs.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading audit logs: " + ex.Message);
            }
        }

        
    }
}
