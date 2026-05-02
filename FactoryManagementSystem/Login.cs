using System;
using System.Windows.Forms;

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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // EMPTY CHECK
            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ===================== OWNER LOGIN =====================
            if (username == "owner" && password == "owner123")
            {
                MessageBox.Show("Owner logged in!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Make sure the form name exactly matches your class:
                // public partial class OwnerDashBoard : Form
                OwnerDashBoard owner = new OwnerDashBoard();

                this.Hide();
                owner.FormClosed += (s, args) => this.Show();
                owner.Show();
                return;
            }

            // ===================== FACTORY 1 LOGIN =====================
            if (username == "factory" && password == "fpass")
            {
                MessageBox.Show("Factory Manager logged in!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

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
