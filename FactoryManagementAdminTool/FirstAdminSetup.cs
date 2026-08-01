using FactoryManagementCore;
using Microsoft.Data.SqlClient;

namespace FactoryManagementAdminTool
{
    public partial class FirstAdminSetup : Form
    {
        // Initializes first admin setup form
        public FirstAdminSetup()
        {
            InitializeComponent();

            // Apply rounded shape to setup panel
            RoundPanel(panelSetup);

            // Reapply rounded shape when panel size changes
            panelSetup.Resize += (s, e) => RoundPanel(panelSetup);
        }

        // Handles creation of the first administrator account
        private void btnCreateAdmin_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            // Confirm password matches
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            // SQL query for inserting new administrator
            string query = @"
                INSERT INTO SystemAdmins
                (Username, PasswordHash)
                VALUES
                (@username, @password)";

            // Query parameters to prevent SQL injection
            SqlParameter[] parameters =
            {
                new SqlParameter("@username", txtUsername.Text),
                new SqlParameter("@password", txtPassword.Text)
            };

            // Execute insert query
            int result = DBHelper.ExecuteNonQuery(query, parameters);

            // Check if admin account was created successfully
            if (result > 0)
            {
                // Save account creation activity in audit logs
                Logger.AddLog(
                    txtUsername.Text.Trim(),
                    "CREATE",
                    "First Admin Setup",
                    $"Created first administrator account '{txtUsername.Text.Trim()}'",
                    "Success"
                );

                MessageBox.Show("Administrator created successfully");

                // Hide setup form and open login screen
                this.Hide();

                AdminLogin login = new AdminLogin();
                login.Show();
            }
        }
    }
}