using FactoryManagementCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FactoryManagementAdminTool
{
    public partial class AdminAccounts : UserControl
    {
        public AdminAccounts()
        {
            InitializeComponent();

            // Load all existing administrator accounts when the control opens
            LoadAdmins();
        }

        /// Loads all system administrator accounts from the database
        /// and displays them in the DataGridView.
        private void LoadAdmins()
        {
            try
            {
                DataTable dt = DBHelper.ExecuteDataTable(
                    @"SELECT AdminID, Username
                      FROM SystemAdmins
                      ORDER BY AdminID DESC",
                    null);

                dgvAdmins.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading admins: " + ex.Message);
            }
        }


        /// Creates a new system administrator account after validation.
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Enter username and password.");
                return;
            }

            // Ensure password confirmation matches
            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                // Check if username already exists
                object exists = DBHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM SystemAdmins WHERE Username=@u",
                    new[]
                    {
                        new SqlParameter("@u", txtUsername.Text.Trim())
                    });

                if (Convert.ToInt32(exists) > 0)
                {
                    MessageBox.Show("Username already exists.");
                    return;
                }

                // Insert new admin account into database
                int result = DBHelper.ExecuteNonQuery(
                    @"INSERT INTO SystemAdmins (Username, PasswordHash)
                      VALUES (@username, @password)",
                    new[]
                    {
                        new SqlParameter("@username", txtUsername.Text.Trim()),
                        new SqlParameter("@password", txtPassword.Text)
                    });


                if (result > 0)
                {
                    // Record admin creation activity in audit logs
                    Logger.AddLog(
                        Session.CurrentUser,
                        "CREATE",
                        "Admin Accounts",
                        $"Created admin account '{txtUsername.Text.Trim()}'",
                        "Success"
                    );

                    MessageBox.Show("Admin account created.");

                    // Reset fields and refresh admin list
                    ClearFields();
                    LoadAdmins();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding admin: " + ex.Message);
            }
        }


        /// Updates the selected administrator account details.
        /// Password update is optional.
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvAdmins.CurrentRow == null)
            {
                MessageBox.Show("Select an admin first.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.");
                return;
            }


            // Get selected admin ID from DataGridView
            int id = Convert.ToInt32(
                dgvAdmins.CurrentRow.Cells["AdminID"].Value);

            try
            {
                // Update username and password if a new password is provided
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    if (txtPassword.Text != txtConfirm.Text)
                    {
                        MessageBox.Show("Passwords do not match.");
                        return;
                    }

                    DBHelper.ExecuteNonQuery(
                        @"UPDATE SystemAdmins
                          SET Username=@username,
                              PasswordHash=@password
                          WHERE AdminID=@id",
                        new[]
                        {
                            new SqlParameter("@username", txtUsername.Text.Trim()),
                            new SqlParameter("@password", txtPassword.Text),
                            new SqlParameter("@id", id)
                        });
                }
                else
                {
                    // Update only username when password field is empty
                    DBHelper.ExecuteNonQuery(
                        @"UPDATE SystemAdmins
                          SET Username=@username
                          WHERE AdminID=@id",
                        new[]
                        {
                            new SqlParameter("@username", txtUsername.Text.Trim()),
                            new SqlParameter("@id", id)
                        });
                }

                // Save update activity
                Logger.AddLog(
                    Session.CurrentUser,
                    "UPDATE",
                    "Admin Accounts",
                    $"Updated admin account '{txtUsername.Text.Trim()}'",
                    "Success"
                );


                MessageBox.Show("Admin updated successfully.");

                ClearFields();
                LoadAdmins();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating admin: " + ex.Message);
            }
        }


        /// Deletes the selected administrator account.
        /// Prevents deletion of the final remaining admin.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAdmins.CurrentRow == null)
            {
                MessageBox.Show("Select an admin first.");
                return;
            }


            // Prevent system lockout by keeping at least one admin account
            object countObj = DBHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM SystemAdmins", null);

            if (Convert.ToInt32(countObj) <= 1)
            {
                MessageBox.Show("Cannot delete the last admin account.");
                return;
            }


            // Ask for confirmation before deletion
            if (MessageBox.Show(
                    "Delete this admin account?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }


            int id = Convert.ToInt32(
                dgvAdmins.CurrentRow.Cells["AdminID"].Value);


            string deletedUsername =
                dgvAdmins.CurrentRow.Cells["Username"].Value.ToString();


            try
            {
                // Remove admin account from database
                DBHelper.ExecuteNonQuery(
                    "DELETE FROM SystemAdmins WHERE AdminID=@id",
                    new[]
                    {
                        new SqlParameter("@id", id)
                    });


                // Record deletion activity
                Logger.AddLog(
                    Session.CurrentUser,
                    "DELETE",
                    "Admin Accounts",
                    $"Deleted admin account '{deletedUsername}'",
                    "Success"
                );


                MessageBox.Show("Admin deleted.");

                ClearFields();
                LoadAdmins();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting admin: " + ex.Message);
            }
        }


        /// Loads selected admin details into input fields for editing.
        private void dgvAdmins_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            txtUsername.Text =
                dgvAdmins.Rows[e.RowIndex]
                .Cells["Username"]
                .Value?.ToString() ?? "";


            // Password fields are cleared for security
            txtPassword.Clear();
            txtConfirm.Clear();
        }


        /// Clears all input fields.
        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirm.Clear();
        }


        /// Returns user to the Admin Dashboard page.
        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard = (AdminDashboard)this.FindForm();

            // Reset dashboard navigation state
            dashboard.ResetSidebarSelection();
            dashboard.SetHeaderTitle("Admin Dashboard");

            // Load default dashboard page
            dashboard.LoadPage(new AdminDash());
        }
    }
}