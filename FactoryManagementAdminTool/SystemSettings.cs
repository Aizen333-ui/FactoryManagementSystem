using FactoryManagementCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryManagementAdminTool
{
    public partial class SystemSettings : UserControl
    {

        // ============================================================
        // Constructor
        //
        // Initializes the UI and loads:
        // - Database settings
        // - System statistics
        // - Audit logs
        // ============================================================

        public SystemSettings()
        {
            InitializeComponent();

            dgvAuditLogs.ReadOnly = true;
            dgvAuditLogs.MultiSelect = false;
            dgvAuditLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadSettings();
            LoadAuditLogs();

            this.HandleCreated += SystemSettings_HandleCreated;

            dgvAuditLogs.DataBindingComplete += DgvAuditLogs_DataBindingComplete;
        }
        // ============================================================
        // Event handler for the HandleCreated event.
        // ============================================================
        private void SystemSettings_HandleCreated(object sender, EventArgs e)
        {
            dgvAuditLogs.ClearSelection();
            dgvAuditLogs.CurrentCell = null;
        }
        // ============================================================
        // Event handler for the DataBindingComplete event of the DataGridView.
        private void DgvAuditLogs_DataBindingComplete(
            object sender,
            DataGridViewBindingCompleteEventArgs e)
        {
            dgvAuditLogs.ClearSelection();
            dgvAuditLogs.CurrentCell = null;
        }

        // ============================================================
        // Loads system configuration information.
        //
        // Displays:
        // - Current database connection string
        // - Total users count
        // - Total administrators count
        // - Database connection status
        // ============================================================

        private void LoadSettings()
        {
            // Display current connection string
            txtConnection.Text =
                DBHelper.ConnectionString;


            try
            {
                // Retrieve total registered users
                object users =
                    DBHelper.ExecuteScalar(
                        "SELECT COUNT(*) FROM Users",
                        null);


                // Retrieve total system administrators
                object admins =
                    DBHelper.ExecuteScalar(
                        "SELECT COUNT(*) FROM SystemAdmins",
                        null);



                // Update statistics display
                lblStats.Text =
                    $"Users: {users}   |   Admins: {admins}";


                // Update connection status
                lblStatus.Text =
                    "Database connection: OK";


                lblStatus.ForeColor =
                    Color.FromArgb(
                        22,
                        163,
                        74);
            }
            catch (Exception ex)
            {
                // Display error state if database
                // information cannot be loaded.
                lblStats.Text =
                    "Unable to load stats.";


                lblStatus.Text =
                    "Database connection: FAILED — "
                    + ex.Message;


                lblStatus.ForeColor =
                    Color.FromArgb(
                        220,
                        38,
                        38);
            }
        }



        // ============================================================
        // Tests database connectivity manually.
        //
        // Opens a temporary SQL connection and confirms whether
        // the database server is reachable.
        // ============================================================

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(
                        DBHelper.ConnectionString))
                {
                    con.Open();
                }



                MessageBox.Show(
                    "Connection successful.",
                    "System Settings",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);



                // Refresh status after successful connection
                LoadSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Connection failed:\n"
                    + ex.Message,
                    "System Settings",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }



        // ============================================================
        // Refreshes all System Settings information.
        //
        // Reloads:
        // - Database status
        // - User/admin statistics
        // - Audit logs
        // ============================================================

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSettings();

            LoadAuditLogs();
        }



        // ============================================================
        // Returns user back to Admin Dashboard.
        //
        // Resets sidebar selection and dashboard header.
        // ============================================================

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard =
                (AdminDashboard)this.FindForm();


            dashboard.ResetSidebarSelection();

            dashboard.SetHeaderTitle(
                "Admin Dashboard");


            dashboard.LoadPage(
                new AdminDash());
        }



        // ============================================================
        // Reloads audit logs manually.
        // ============================================================

        private void btnReloadLogs_Click(object sender, EventArgs e)
        {
            LoadAuditLogs();
        }



        // ============================================================
        // Exports audit logs to a text file.
        //
        // Process:
        // - Validates available logs
        // - Opens save dialog
        // - Writes log records into text file
        // - Creates audit entry
        // ============================================================

        private void btnExportLogs_Click(object sender, EventArgs e)
        {
            try
            {
                // Prevent exporting empty data
                if (dgvAuditLogs.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No logs available to export.",
                        "Export Logs",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }



                SaveFileDialog save =
                    new SaveFileDialog();


                save.Filter =
                    "Text Files (*.txt)|*.txt";


                save.FileName =
                    "FactoryManagement_Logs_" +
                    DateTime.Now.ToString(
                        "yyyyMMdd_HHmmss")
                    + ".txt";



                // Stop if user cancels save operation
                if (save.ShowDialog()
                    != DialogResult.OK)
                {
                    return;
                }



                using (StreamWriter writer =
                    new StreamWriter(
                        save.FileName))
                {

                    // File header
                    writer.WriteLine(
                        "====================================");


                    writer.WriteLine(
                        " Factory Management System Logs");


                    writer.WriteLine(
                        " Generated: "
                        + DateTime.Now);


                    writer.WriteLine(
                        "====================================");


                    writer.WriteLine();



                    // Write every audit record
                    foreach (DataGridViewRow row
                        in dgvAuditLogs.Rows)
                    {
                        if (row.IsNewRow)
                            continue;



                        string logLine =
                            "Date: "
                            + row.Cells["LogDate"].Value

                            + " | User: "
                            + row.Cells["Username"].Value

                            + " | Action: "
                            + row.Cells["Action"].Value

                            + " | Status: "
                            + row.Cells["Status"].Value;



                        writer.WriteLine(logLine);


                        writer.WriteLine(
                            "------------------------------------");
                    }
                }



                // Record successful export activity
                Logger.AddLog(
                    Session.CurrentUser,
                    "Export Logs",
                    "System Settings",
                    "System logs exported successfully",
                    "Success");



                MessageBox.Show(
                    "Logs exported successfully.",
                    "Export Logs",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Record export failure
                Logger.AddLog(
                    Session.CurrentUser,
                    "Export Logs",
                    "System Settings",
                    ex.Message,
                    "Failed");



                MessageBox.Show(
                    "Error exporting logs:\n"
                    + ex.Message,
                    "Export Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }



        // ============================================================
        // Loads audit history from database.
        //
        // Retrieves:
        // - Username
        // - Action performed
        // - Module name
        // - Description
        // - Status
        // - Date/time
        //
        // Results are displayed in Audit Logs DataGridView.
        // ============================================================

        private void LoadAuditLogs()
        {
            try
            {
                string query = @"
                    SELECT 
                        Username,
                        Action,
                        Module,
                        Description,
                        Status,
                        LogDate
                    FROM AuditLogs
                    ORDER BY LogID DESC";


                DataTable dt =
                    DBHelper.ExecuteDataTable(
                        query,
                        null);


                dgvAuditLogs.DataSource = dt;

                dgvAuditLogs.ClearSelection();
                dgvAuditLogs.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading audit logs: "
                    + ex.Message);
            }
        }

    }
}