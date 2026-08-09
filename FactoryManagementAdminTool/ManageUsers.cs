using FactoryManagementCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryManagementAdminTool
{
    public partial class ManageUsers : UserControl
    {
        // ============================================================
        // Constructor
        // Initializes UI components and loads existing users.
        // ============================================================
        public ManageUsers()
        {
            InitializeComponent();
            dgvUsers.ReadOnly = true;
            dgvUsers.MultiSelect = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LoadUsers();
            this.HandleCreated += ManageUsers_HandleCreated;
            // Ensure no row is selected after binding completes
            dgvUsers.DataBindingComplete += DgvUsers_DataBindingComplete;

        }
        // ============================================================
        // Ensures no row is selected when the control is first created.
        // ============================================================
        private void ManageUsers_HandleCreated(object sender, EventArgs e)
        {
            dgvUsers.ClearSelection();
            dgvUsers.CurrentCell = null;
        }
        // ============================================================
        // Ensures no row is selected after the DataGridView finishes binding.
        // ============================================================
        private void DgvUsers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Clear selection after the grid finishes binding so no row is pre-selected
            dgvUsers.ClearSelection();
            dgvUsers.CurrentCell = null;
        }

        // ============================================================
        // Loads all users from database and displays them in DataGridView.
        // ============================================================
        private void LoadUsers()
        {
            try
            {
                string query = @"
                    SELECT 
                        UserID,
                        FullName,
                        Username,
                        Role,
                        IsActive
                    FROM Users
                    ORDER BY UserID DESC";

                DataTable dt = DBHelper.ExecuteDataTable(query, null);
                dgvUsers.DataSource = dt;
                dgvUsers.ClearSelection();
                dgvUsers.CurrentCell = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }
        // ============================================================
        // Adds a new user account.
        // Performs validation, inserts user data, and creates audit log.
        // ============================================================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRole.SelectedIndex <= 0)
            {
                MessageBox.Show("Please fill all fields including role.");
                return;
            }

            try
            {
                string query = @"
                INSERT INTO Users
                (FullName, Username, Password, Role, IsActive)
                VALUES
                (@fullname, @username, @password, @role, @isactive)";

                SqlParameter[] parameters =
                {
                    new SqlParameter("@fullname", txtFullName.Text.Trim()),
                    new SqlParameter("@username", txtUsername.Text.Trim()),
                    new SqlParameter("@password", txtPassword.Text),
                    new SqlParameter("@role", cmbRole.SelectedItem.ToString()),
                    new SqlParameter("@isactive", true)
                };

                int result = DBHelper.ExecuteNonQuery(query, parameters);

                if (result > 0)
                {
                    Logger.AddLog(
                        Session.CurrentUser,
                        "CREATE",
                        "Manage Users",
                        $"Created user account '{txtUsername.Text.Trim()}'",
                        "Success"
                    );
                    MessageBox.Show("User added successfully.");
                    LoadUsers();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message);
            }
        }
        // ============================================================
        // Updates selected user's information.
        // Password is updated only when a new password is entered.
        // Otherwise existing password remains unchanged.
        // ============================================================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Select a user first.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                cmbRole.SelectedIndex <= 0)
            {
                MessageBox.Show("Please fill name, username, and role.");
                return;
            }

            DataGridViewRow row = dgvUsers.CurrentRow;

            int id = Convert.ToInt32(row.Cells["UserID"].Value);

            string currentFullName =
                row.Cells["FullName"].Value?.ToString() ?? "";

            string currentUsername =
                row.Cells["Username"].Value?.ToString() ?? "";

            string currentRole =
                row.Cells["Role"].Value?.ToString() ?? "";

            bool currentIsActive =
                Convert.ToBoolean(row.Cells["IsActive"].Value);

            string newFullName = txtFullName.Text.Trim();
            string newUsername = txtUsername.Text.Trim();
            string newRole = cmbRole.SelectedItem.ToString();

            bool passwordChanged =
                !string.IsNullOrWhiteSpace(txtPassword.Text);

            // ============================================================
            // Check whether anything was actually changed
            // ============================================================

            bool basicDetailsChanged =
                !string.Equals(currentFullName, newFullName, StringComparison.Ordinal) ||
                !string.Equals(currentUsername, newUsername, StringComparison.Ordinal) ||
                !string.Equals(currentRole, newRole, StringComparison.Ordinal);

            if (!basicDetailsChanged && !passwordChanged)
            {
                MessageBox.Show("No changes were made.");
                return;
            }

            try
            {
                string query;
                SqlParameter[] parameters;

                if (!passwordChanged)
                {
                    query = @"
                UPDATE Users
                SET FullName = @fullname,
                    Username = @username,
                    Role = @role
                WHERE UserID = @id";

                    parameters = new[]
                    {
                new SqlParameter("@fullname", newFullName),
                new SqlParameter("@username", newUsername),
                new SqlParameter("@role", newRole),
                new SqlParameter("@id", id)
            };
                }
                else
                {
                    query = @"
                UPDATE Users
                SET FullName = @fullname,
                    Username = @username,
                    Password = @password,
                    Role = @role
                WHERE UserID = @id";

                    parameters = new[]
                    {
                new SqlParameter("@fullname", newFullName),
                new SqlParameter("@username", newUsername),
                new SqlParameter("@password", txtPassword.Text),
                new SqlParameter("@role", newRole),
                new SqlParameter("@id", id)
            };
                }

                int result = DBHelper.ExecuteNonQuery(query, parameters);

                if (result > 0)
                {
                    Logger.AddLog(
                        Session.CurrentUser,
                        "UPDATE",
                        "Manage Users",
                        $"Updated user account '{newUsername}'",
                        "Success"
                    );

                    MessageBox.Show("User updated successfully.");

                    LoadUsers();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message);
            }
        }
        // ============================================================
        // Deletes selected user permanently from Users table.
        // Requires confirmation before deletion.
        // ============================================================

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Select a user first.");
                return;
            }

            if (MessageBox.Show(
                    "Delete selected user?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);

            try
            {
                DBHelper.ExecuteNonQuery(
                    "DELETE FROM Users WHERE UserID=@id",
                    new[] { new SqlParameter("@id", id) });
                Logger.AddLog(
                    Session.CurrentUser,
                    "DELETE",
                    "Manage Users",
                    $"Deleted user account with ID '{id}'",
                    "Success"
                );
                MessageBox.Show("User deleted successfully.");
                LoadUsers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message);
            }
        }
        // ============================================================
        // Opens password reset window for selected user.
        // ============================================================

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
            string username = dgvUsers.CurrentRow.Cells["Username"].Value.ToString();

            using (ResetPassword frm = new ResetPassword(userId, username))
            {
                frm.ShowDialog(this);
            }
            Logger.AddLog(
                Session.CurrentUser,
                "RESET PASSWORD",
                "Manage Users",
                $"Reset password for user '{username}'",
                "Success"
            );
            LoadUsers();
        }
        // ============================================================
        // Enables or disables selected user account.
        // Updates IsActive status in database.
        // ============================================================
        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Please select a user.");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
            bool currentStatus = Convert.ToBoolean(dgvUsers.CurrentRow.Cells["IsActive"].Value);
            bool newStatus = !currentStatus;

            DBHelper.ExecuteNonQuery(
                "UPDATE Users SET IsActive = @status WHERE UserID = @id",
                new[]
                {
                    new SqlParameter("@status", newStatus),
                    new SqlParameter("@id", userId)
                });
            Logger.AddLog(
                Session.CurrentUser,
                newStatus ? "ENABLE USER" : "DISABLE USER",
                "Manage Users",
                $"{(newStatus ? "Enabled" : "Disabled")} user account with ID '{userId}'",
                "Success"
            );
            MessageBox.Show(newStatus
                ? "User enabled successfully."
                : "User disabled successfully.");

            LoadUsers();
            UpdateToggleButtonText(newStatus);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            txtFullName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtSearch.Clear();

            // Clear ComboBox selection
            cmbRole.SelectedIndex = 0;
            cmbRole.Text = "";

            // Clear DataGridView selection
            dgvUsers.ClearSelection();
            dgvUsers.CurrentCell = null;

            // Reset toggle button
            btnToggleStatus.Text = "Disable User";

            // Reload users
            LoadUsers();

            // Make absolutely sure nothing is selected after reload
            dgvUsers.ClearSelection();
            dgvUsers.CurrentCell = null;
        }

        // ============================================================
        // Searches users by username, full name, or role.
        // Displays filtered results in DataGridView.
        // ============================================================
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string term = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(term))
            {
                LoadUsers();
                return;
            }

            try
            {
                string query = @"
                    SELECT UserID, FullName, Username, Role, IsActive
                    FROM Users
                    WHERE Username LIKE @search OR FullName LIKE @search OR Role LIKE @search
                    ORDER BY UserID DESC";

                DataTable dt = DBHelper.ExecuteDataTable(
                    query,
                    new[] { new SqlParameter("@search", "%" + term + "%") });

                dgvUsers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }
        // ============================================================
        // Loads selected user information into input fields
        // when a DataGridView row is clicked.
        // ============================================================
        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

            txtFullName.Text = row.Cells["FullName"].Value?.ToString() ?? "";
            txtUsername.Text = row.Cells["Username"].Value?.ToString() ?? "";
            txtPassword.Clear();
            cmbRole.Text = row.Cells["Role"].Value?.ToString() ?? "";

            bool active = Convert.ToBoolean(row.Cells["IsActive"].Value);
            UpdateToggleButtonText(active);
        }
        // ============================================================
        // Changes Toggle button text based on account status.
        // ============================================================

        private void UpdateToggleButtonText(bool active)
        {
            btnToggleStatus.Text = active ? "Disable User" : "Enable User";
        }

        private void ClearFields()
        {
            txtFullName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtSearch.Clear();
            cmbRole.SelectedIndex = 0;
        }
        // ============================================================
        // Returns user to Admin Dashboard.
        // Resets sidebar and dashboard header.
        // ============================================================
        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard = (AdminDashboard)this.FindForm();
            dashboard.ResetSidebarSelection();
            dashboard.SetHeaderTitle("Admin Dashboard");
            dashboard.LoadPage(new AdminDash());
        }
    }
}