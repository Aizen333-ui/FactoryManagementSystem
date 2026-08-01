using FactoryManagementCore;
using Microsoft.Data.SqlClient;

namespace FactoryManagementAdminTool
{
    public partial class ResetPassword : Form
    {
        // Stores the UserID of the account whose password
        // is being changed.
        private int userId;

        // ============================================================
        // Constructor
        //
        // Initializes the form and receives the selected user's
        // information from ManageUsers.
        //
        // id       : Selected user's database ID
        // username : Selected user's username displayed in textbox
        // ============================================================

        public ResetPassword(int id, string username)
        {
            InitializeComponent();

            userId = id;

            // Username is displayed as read-only
            // to show which account is being updated.
            txtUsername.Text = username;
        }

        // ============================================================
        // Saves the new password.
        //
        // Validation:
        // - Password cannot be empty
        // - Password confirmation must match
        //
        // After successful update:
        // - Shows confirmation message
        // - Closes the reset window
        // ============================================================

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if password field is empty
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show(
                    "Enter a new password.");

                return;
            }

            // Verify both password fields match
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(
                    "Passwords do not match.");

                return;
            }

            try
            {
                // Update password for selected user
                string query = @"
                    UPDATE Users
                    SET PasswordHash = @password
                    WHERE UserID = @id";

                // Parameterized query prevents SQL injection
                SqlParameter[] parameters =
                {
                    new SqlParameter(
                        "@password",
                        txtNewPassword.Text),

                    new SqlParameter(
                        "@id",
                        userId)
                };

                int result =
                    DBHelper.ExecuteNonQuery(
                        query,
                        parameters);

                // Check whether database update succeeded
                if (result > 0)
                {
                    MessageBox.Show(
                        "Password reset successfully.");

                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "User not found.");
                }
            }
            catch (Exception ex)
            {
                // Store failed password reset attempt
                // in audit logs for tracking.
                Logger.AddLog(
                    Session.CurrentUser,
                    "RESET PASSWORD",
                    "Manage Users",
                    $"Failed to reset password: {ex.Message}",
                    "Failed"
                );

                MessageBox.Show(
                    "Error resetting password: " + ex.Message);
            }
        }

        // ============================================================
        // Cancels password reset operation.
        // Closes the form without saving changes.
        // ============================================================

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}