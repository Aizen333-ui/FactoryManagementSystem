using FactoryManagementCore;
using Microsoft.Data.SqlClient;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace FactoryManagementAdminTool
{
    public partial class AdminAccounts : UserControl
    {
        public AdminAccounts()
        {
            InitializeComponent();
            LoadAdmins();
        }

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Enter username and password.");
                return;
            }

            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                object exists = DBHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM SystemAdmins WHERE Username=@u",
                    new[] { new SqlParameter("@u", txtUsername.Text.Trim()) });

                if (Convert.ToInt32(exists) > 0)
                {
                    MessageBox.Show("Username already exists.");
                    return;
                }

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

                    Logger.AddLog(
                        Session.CurrentUser,
                        "CREATE",
                        "Admin Accounts",
                        $"Created admin account '{txtUsername.Text.Trim()}'",
                        "Success"
                        );
                    MessageBox.Show("Admin account created.");
                    ClearFields();
                    LoadAdmins();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding admin: " + ex.Message);
            }
        }

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

            int id = Convert.ToInt32(dgvAdmins.CurrentRow.Cells["AdminID"].Value);

            try
            {
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    if (txtPassword.Text != txtConfirm.Text)
                    {
                        MessageBox.Show("Passwords do not match.");
                        return;
                    }

                    DBHelper.ExecuteNonQuery(
                        @"UPDATE SystemAdmins
                          SET Username=@username, PasswordHash=@password
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
                    DBHelper.ExecuteNonQuery(
                        @"UPDATE SystemAdmins SET Username=@username WHERE AdminID=@id",
                        new[]
                        {
                            new SqlParameter("@username", txtUsername.Text.Trim()),
                            new SqlParameter("@id", id)
                        });
                    Logger.AddLog(
                        Session.CurrentUser,
                        "UPDATE",
                        "Admin Accounts",
                        $"Updated admin account '{txtUsername.Text.Trim()}'",
                        "Success"
                    );
                }

                MessageBox.Show("Admin updated successfully.");
                ClearFields();
                LoadAdmins();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating admin: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAdmins.CurrentRow == null)
            {
                MessageBox.Show("Select an admin first.");
                return;
            }

            object countObj = DBHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM SystemAdmins", null);
            if (Convert.ToInt32(countObj) <= 1)
            {
                MessageBox.Show("Cannot delete the last admin account.");
                return;
            }

            if (MessageBox.Show(
                    "Delete this admin account?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            int id = Convert.ToInt32(dgvAdmins.CurrentRow.Cells["AdminID"].Value);

            string deletedUsername =
                dgvAdmins.CurrentRow.Cells["Username"].Value.ToString();
            try
            {
                DBHelper.ExecuteNonQuery(
                    "DELETE FROM SystemAdmins WHERE AdminID=@id",
                    new[] { new SqlParameter("@id", id) });
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

        private void dgvAdmins_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            txtUsername.Text = dgvAdmins.Rows[e.RowIndex].Cells["Username"].Value?.ToString() ?? "";
            txtPassword.Clear();
            txtConfirm.Clear();
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirm.Clear();
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
