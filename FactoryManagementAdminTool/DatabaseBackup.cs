using FactoryManagementCore;
using Microsoft.Data.SqlClient;

namespace FactoryManagementAdminTool
{
    public partial class DatabaseBackup : UserControl
    {
        public DatabaseBackup()
        {
            InitializeComponent();
            txtPath.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "FactoryDB_Backup.bak");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "SQL Backup (*.bak)|*.bak|All files (*.*)|*.*";
            dlg.FileName = "FactoryDB_Backup.bak";
            dlg.Title = "Choose backup file location";

            if (dlg.ShowDialog() == DialogResult.OK)
                txtPath.Text = dlg.FileName;
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text.Trim();

            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Choose a backup file path.");
                return;
            }

            try
            {
                string? folder = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                // SQL Server must be able to write to this path (local machine / shared disk)
                string query = @"
                    BACKUP DATABASE [FactoryDB]
                    TO DISK = @path
                    WITH FORMAT, INIT,
                         NAME = N'FactoryDB-Full Backup',
                         SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                DBHelper.ExecuteNonQuery(
                    query,
                    new[] { new SqlParameter("@path", path) });

                lblResult.ForeColor = Color.FromArgb(22, 163, 74);
                lblResult.Text = "Backup completed successfully:\n" + path;
                Logger.AddLog(
                    Session.CurrentUser,
                    "BACKUP",
                    "Database Backup",
                    $"Database backup created successfully at '{path}'",
                    "Success"
                );
                MessageBox.Show(
                    "Database backup completed.\n\n" + path,
                    "Backup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblResult.ForeColor = Color.FromArgb(220, 38, 38);
                lblResult.Text = "Backup failed: " + ex.Message;
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard = (AdminDashboard)this.FindForm();
            dashboard.ResetSidebarSelection();
            dashboard.SetHeaderTitle("Admin Dashboard");
            dashboard.LoadPage(new AdminDash());
        }
    }
}
