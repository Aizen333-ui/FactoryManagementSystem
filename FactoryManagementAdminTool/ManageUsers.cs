using FactoryManagementCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryManagementAdminTool
{
    public partial class ManageUsers : UserControl
    {
        public ManageUsers()
        {
            InitializeComponent();
            LoadUsers();
        }

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRole.SelectedItem == null)
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Select a user first.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please fill name, username, and role.");
                return;
            }

            int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
            bool isActive = Convert.ToBoolean(dgvUsers.CurrentRow.Cells["IsActive"].Value);

            try
            {
                string query;
                SqlParameter[] parameters;

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    // Keep existing password when field left blank
                    query = @"
                    UPDATE Users
                    SET FullName=@fullname, Username=@username, Role=@role, IsActive=@isactive
                    WHERE UserID=@id";

                    parameters = new[]
                    {
                        new SqlParameter("@fullname", txtFullName.Text.Trim()),
                        new SqlParameter("@username", txtUsername.Text.Trim()),
                        new SqlParameter("@role", cmbRole.SelectedItem.ToString()),
                        new SqlParameter("@isactive", isActive),
                        new SqlParameter("@id", id)
                    };
                }
                else
                {
                    query = @"
                    UPDATE Users
                    SET FullName=@fullname, Username=@username, Password=@password,
                        Role=@role, IsActive=@isactive
                    WHERE UserID=@id";

                    parameters = new[]
                    {
                        new SqlParameter("@fullname", txtFullName.Text.Trim()),
                        new SqlParameter("@username", txtUsername.Text.Trim()),
                        new SqlParameter("@password", txtPassword.Text),
                        new SqlParameter("@role", cmbRole.SelectedItem.ToString()),
                        new SqlParameter("@isactive", isActive),
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
                       $"Updated user account '{txtUsername.Text.Trim()}'",
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
            cmbRole.SelectedIndex = -1;
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
