using FactoryManagementCore;
using Microsoft.Data.SqlClient;

namespace FactoryManagementAdminTool
{
    public partial class ResetPassword : Form
    {
        private int userId;


        public ResetPassword(int id, string username)
        {
            InitializeComponent();

            userId = id;

            txtUsername.Text = username;
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Enter a new password.");
                return;
            }


            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }


            try
            {
                string query = @"
                    UPDATE Users
                    SET PasswordHash = @password
                    WHERE UserID = @id";


                SqlParameter[] parameters =
                {
                    new SqlParameter("@password", txtNewPassword.Text),
                    new SqlParameter("@id", userId)
                };


                int result =
                    FactoryManagementCore.DBHelper.ExecuteNonQuery(
                        query,
                        parameters
                    );


                if (result > 0)
                {
                    MessageBox.Show(
                        "Password reset successfully."
                    );

                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "User not found."
                    );
                }

            }
            catch (Exception ex)
            {
                Logger.AddLog(
                    Session.CurrentUser,
                    "RESET PASSWORD",
                    "Manage Users",
                    $"Failed to reset password: {ex.Message}",
                    "Failed"
                );
                MessageBox.Show(
                    "Error resetting password: " + ex.Message
                );
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}