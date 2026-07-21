namespace FactoryManagementSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            RoundPanel(panelLogin);

            panelLogin.Resize += (s, e) => RoundPanel(panelLogin);
        }
        // --- LOGIN BUTTON CLICK EVENT ---

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();


            if (username == "" || password == "")
            {
                MessageBox.Show(
                    "Please enter username and password.",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }


                    string query = @"
                        SELECT Role
                        FROM Users
                        WHERE Username = @username 
                        AND Password = @password
                    ";


            Microsoft.Data.SqlClient.SqlParameter[] parameters =
                    {
                new Microsoft.Data.SqlClient.SqlParameter("@username", username),
                new Microsoft.Data.SqlClient.SqlParameter("@password", password)
                    };


            object result = DBHelper.ExecuteScalar(query, parameters);


            if (result == null)
            {
                MessageBox.Show(
                    "Invalid username or password!",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Logger.AddLog(
                    username,
                    "LOGIN",
                    "Security",
                    "Failed login attempt",
                    "Failed"
                );
                return;
            }
            Session.CurrentUser = username;
            Logger.AddLog(
                Session.CurrentUser,
                "LOGIN",
                "Security",
                "User logged into the system",
                "Success"
            );

            string role = result.ToString();



            // ================= OWNER =================

            if (role == "Owner")
            {
                OwnerDashBoard owner = new OwnerDashBoard();

                this.Hide();

                owner.FormClosed += (s, args) =>
                {
                    this.Show();
                };

                owner.Show();
            }



            // ================= FACTORY =================

            else if (role == "Manager")
            {
                FactoryDashBoard factory = new FactoryDashBoard();

                this.Hide();

                factory.FormClosed += (s, args) =>
                {
                    this.Show();
                };

                factory.Show();
            }


        }
    }
}
