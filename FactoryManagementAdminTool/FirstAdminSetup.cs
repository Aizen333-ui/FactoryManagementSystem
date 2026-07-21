using FactoryManagementCore;
using Microsoft.Data.SqlClient;
namespace FactoryManagementAdminTool
{
    public partial class FirstAdminSetup : Form
    {
        public FirstAdminSetup()
        {
            InitializeComponent();
            RoundPanel(panelSetup);

            panelSetup.Resize += (s, e) => RoundPanel(panelSetup);
        }
        private void btnCreateAdmin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }


            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }


            string query = @"
                INSERT INTO SystemAdmins
                (Username, PasswordHash)
                VALUES
                (@username, @password)";


            SqlParameter[] parameters =
            {
                new SqlParameter("@username", txtUsername.Text),
                new SqlParameter("@password", txtPassword.Text)
            };


            int result = DBHelper.ExecuteNonQuery(query, parameters);


            if (result > 0)
            {
                Logger.AddLog(
                    txtUsername.Text.Trim(),
                    "CREATE",
                    "First Admin Setup",
                    $"Created first administrator account '{txtUsername.Text.Trim()}'",
                    "Success"
                );
                MessageBox.Show("Administrator created successfully");

                this.Hide();

                AdminLogin login = new AdminLogin();
                login.Show();
            }
        }
    }
}


         

