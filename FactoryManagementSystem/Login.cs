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

            // ===================== VALIDATION CHECK =====================
            // Ensure both fields are filled before processing login
            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===================== OWNER LOGIN =====================
            if (username == "owner" && password == "owner123")
            {
                


                OwnerDashBoard owner = new OwnerDashBoard();

                this.Hide();
                owner.FormClosed += (s, args) => this.Show();
                owner.Show();
                return;
            }

            // ===================== FACTORY LOGIN =====================
            if (username == "factory" && password == "fpass")
            {

                FactoryDashBoard factory1 = new FactoryDashBoard();

                this.Hide();
                factory1.FormClosed += (s, args) => this.Show();
                factory1.Show();
                return;
            }



            // ===================== INVALID LOGIN =====================
            MessageBox.Show("Invalid username or password!", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
