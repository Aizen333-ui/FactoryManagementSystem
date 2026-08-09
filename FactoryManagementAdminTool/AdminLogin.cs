using FactoryManagementCore;
using Microsoft.Data.SqlClient;

namespace FactoryManagementAdminTool
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();

            // Apply rounded corners to login panel
            RoundPanel(panelLogin);

            // Reapply rounded shape when panel size changes
            panelLogin.Resize += (s, e) => RoundPanel(panelLogin);
        }

        // Handles admin login authentication
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Get user input values
            string username = txtUsername.Text;

            string password = txtPassword.Text;


            // Validate required fields
            if (username == "" || password == "")
            {
                MessageBox.Show(
                    "Please enter username and password",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // Check admin credentials from database
            string query =
                "SELECT COUNT(*) FROM SystemAdmins WHERE Username=@username AND Password=@password";


            SqlParameter[] parameters =
            {
                new SqlParameter("@username", username),
                new SqlParameter("@password", password)
            };


            object result =
                DBHelper.ExecuteScalar(query, parameters);


            int count =
                Convert.ToInt32(result);

            // Login successful
            if (count > 0)
            {
                // Store currently logged-in admin session
                Session.CurrentUser =
                    username.Trim();

                // Record successful login activity
                Logger.AddLog(
                    Session.CurrentUser,
                    "LOGIN",
                    "Security",
                    "Admin logged into the system",
                    "Success"
                );

                // Open admin dashboard
                this.Hide();

                using (AdminDashboard dashboard = new AdminDashboard())
                {
                    dashboard.ShowDialog();
                }


                // Show login screen again after dashboard closes
                txtPassword.Clear();

                this.Show();
            }
            else
            {
                // Record failed login attempt
                Logger.AddLog(
                    username.Trim(),
                    "LOGIN",
                    "Security",
                    "Invalid login attempt",
                    "Failed"
                );


                MessageBox.Show(
                    "Invalid username or password",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}