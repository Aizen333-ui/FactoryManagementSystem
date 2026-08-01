using FactoryManagementCore;
using Microsoft.Data.SqlClient;

namespace FactoryManagementAdminTool
{
    public partial class DatabaseBackup : UserControl
    {
        public DatabaseBackup()
        {
            InitializeComponent();

            // Set default backup location
            txtPath.Text =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments),
                    "FactoryDB_Backup.bak");
        }

        // Opens file dialog to select backup destination
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using SaveFileDialog dlg = new SaveFileDialog();

            // Allow SQL backup file selection
            dlg.Filter =
                "SQL Backup (*.bak)|*.bak|All files (*.*)|*.*";

            dlg.FileName =
                "FactoryDB_Backup.bak";

            dlg.Title =
                "Choose backup file location";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text =
                    dlg.FileName;
            }
        }

        // Creates SQL Server database backup
        private void btnBackup_Click(object sender, EventArgs e)
        {
            string path =
                txtPath.Text.Trim();

            // Validate backup location
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show(
                    "Choose a backup file path.");

                return;
            }

            try
            {
                // Create destination folder if it does not exist
                string? folder =
                    Path.GetDirectoryName(path);

                if (!string.IsNullOrEmpty(folder) &&
                    !Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // SQL Server backup command
                // SQL Server service account must have access to this location
                string query =
                @"
                    BACKUP DATABASE [FactoryDB]
                    TO DISK = @path
                    WITH FORMAT,
                         INIT,
                         NAME = N'FactoryDB-Full Backup',
                         SKIP,
                         NOREWIND,
                         NOUNLOAD,
                         STATS = 10";

                // Execute backup query
                DBHelper.ExecuteNonQuery(
                    query,
                    new[]
                    {
                        new SqlParameter("@path", path)
                    });

                // Display success message
                lblResult.ForeColor =
                    Color.FromArgb(
                        22,
                        163,
                        74);

                lblResult.Text =
                    "Backup completed successfully:\n" +
                    path;

                // Save backup action in audit log
                Logger.AddLog(
                    Session.CurrentUser,
                    "BACKUP",
                    "Database Backup",
                    $"Database backup created successfully at '{path}'",
                    "Success"
                );

                MessageBox.Show(
                    "Database backup completed.\n\n" +
                    path,
                    "Backup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Display backup failure message
                lblResult.ForeColor =
                    Color.FromArgb(
                        220,
                        38,
                        38);

                lblResult.Text =
                    "Backup failed: " +
                    ex.Message;

                // Save failed backup attempt in audit log
                Logger.AddLog(
                    Session.CurrentUser,
                    "BACKUP",
                    "Database Backup",
                    $"Database backup failed: {ex.Message}",
                    "Failed"
                );

                MessageBox.Show(
                    "Backup failed.\n\n" +
                    "SQL Server must have write permission to the path.\n" +
                    "For local SQL Express, prefer a folder SQL Server can access " +
                    "(for example under C:\\Backup).\n\n" +
                    ex.Message,

                    "Backup",

                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Returns user to admin dashboard
        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard =
                (AdminDashboard)this.FindForm();

            // Reset selected sidebar item
            dashboard.ResetSidebarSelection();

            // Restore dashboard header
            dashboard.SetHeaderTitle(
                "Admin Dashboard");

            // Open default dashboard page
            dashboard.LoadPage(
                new AdminDash());
        }
    }
}