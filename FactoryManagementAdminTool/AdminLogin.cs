using FactoryManagementCore;
using Microsoft.Data.SqlClient;

namespace FactoryManagementAdminTool
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
            RoundPanel(panelLogin);

            panelLogin.Resize += (s, e) => RoundPanel(panelLogin);
        }
        // --- LOGIN BUTTON CLICK EVENT ---

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

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


            string query =
            "SELECT COUNT(*) FROM SystemAdmins WHERE Username=@username AND PasswordHash=@password";


            SqlParameter[] parameters =
            {
                new SqlParameter("@username", username),
                new SqlParameter("@password", password)
            };


            object result = DBHelper.ExecuteScalar(query, parameters);


            int count = Convert.ToInt32(result);


            if (count > 0)
            {
                Session.CurrentUser = username.Trim();

                Logger.AddLog(
                    Session.CurrentUser,
                    "LOGIN",
                    "Security",
                    "Admin logged into the system",
                    "Success"
                );

                this.Hide();
                using (AdminDashboard dashboard = new AdminDashboard())
                {
                    dashboard.ShowDialog();
                }
                txtPassword.Clear();
                this.Show();
            }
            else
            {
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
