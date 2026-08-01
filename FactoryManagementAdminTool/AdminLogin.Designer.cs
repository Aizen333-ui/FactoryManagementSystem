using System.Drawing.Drawing2D;

namespace FactoryManagementAdminTool
{
    partial class AdminLogin
    {
        // Login form controls
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelLogin;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPass;

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;

        private System.Windows.Forms.Button btnLogin;

        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblMessage;

        // Applies rounded corners to a panel
        private void RoundPanel(Panel panel)
        {
            GraphicsPath path = new GraphicsPath();

            int radius = 20;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);

            path.CloseAllFigures();

            panel.Region = new Region(path);
        }

        // Applies rounded corners to controls such as buttons
        private void RoundControl(Control ctl, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);

            path.CloseAllFigures();


            ctl.Region = new Region(path);

            // Reapply rounded shape after resizing
            ctl.Resize += (s, e) =>
            {
                GraphicsPath p = new GraphicsPath();

                p.AddArc(0, 0, radius, radius, 180, 90);
                p.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
                p.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
                p.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);

                p.CloseAllFigures();

                ctl.Region = new Region(p);
            };
        }

        // Creates rounded input box containers for text fields
        private Panel CreateRoundedBox(Control innerControl, int height = 55)
        {
            int verticalPadding = 10;

            int boxHeight = height;


            // Apply textbox styling
            if (innerControl is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;

                tb.Font = new Font("Segoe UI", 12F);

                tb.Margin = new Padding(0);


                // Center textbox text vertically
                verticalPadding =
                    (boxHeight - tb.PreferredHeight) / 2;


                if (verticalPadding < 0)
                    verticalPadding = 0;
            }



            // Outer rounded container
            Panel container = new Panel();

            container.Height = boxHeight;
            container.Width = innerControl.Width > 0 ? innerControl.Width + 24 : 800;
            container.BackColor = Color.White;
            container.Padding = new Padding(8, verticalPadding, 8, verticalPadding);
            container.Margin = new Padding(0, 0, 0, 15);
            container.Size = new Size(container.Width, container.Height + 2);
            container.AutoSize = false;
            container.Anchor = AnchorStyles.Left;

            // Fill container with textbox
            innerControl.Dock = DockStyle.Fill;
            innerControl.BackColor = Color.White;
            innerControl.Margin = new Padding(0);

            container.Controls.Add(innerControl);

            // Draw rounded border
            container.Paint += (s, e) =>
            {
                int radius = 12;

                Rectangle rect = new Rectangle(1, 1, container.Width - 2, container.Height - 2);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

                    path.CloseAllFigures();

                    e.Graphics.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    // Background fill
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillPath(brush, path);
                    }

                    // Border
                    using (Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };


            return container;
        }

        // Initializes login form UI components
        private void InitializeComponent()
        {
            this.panelLogin = new System.Windows.Forms.Panel();

            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();

            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();

            this.btnLogin = new System.Windows.Forms.Button();

            this.lblLogo = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();


            this.SuspendLayout();


            // Center login card when form size changes
            this.Resize += (s, e) =>
            {
                panelLogin.Left =
                    (this.ClientSize.Width -panelLogin.Width) / 2;
                    
                panelLogin.Top =
                    (this.ClientSize.Height -panelLogin.Height) / 2;

            };

            // ================= FORM SETTINGS =================

            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login";

            // ================= LOGIN CARD =================

            panelLogin.BackColor =
                Color.FromArgb(245, 247, 250);


            panelLogin.Size =
                new Size(600, 700);

            panelLogin.Location = new Point(
                (this.ClientSize.Width - panelLogin.Width) / 2,
                (this.ClientSize.Height - panelLogin.Height) / 2);

            // Logo
            lblLogo.Font = new Font("Segoe UI", 22F);
            lblLogo.ForeColor = Color.FromArgb(20, 20, 20);
            lblLogo.Location = new Point(260, 50);
            lblLogo.Size = new Size(250, 65);
            lblLogo.Text = "🏢";

            // Title
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(20, 20, 20);
            lblTitle.Location = new Point(90, 110);
            lblTitle.Size = new Size(450, 45);
            lblTitle.Text = "Welcome Back";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // Message text
            lblMessage.Font = new Font("Segoe UI", 11F);
            lblMessage.ForeColor = Color.Gray;
            lblMessage.Location = new Point(150, 170);
            lblMessage.Size = new Size(360, 30);
            lblMessage.Text = "Sign in to access admin dashboard";

            // Username label
            lblUser.Font = new Font("Segoe UI", 13F);
            lblUser.ForeColor = Color.Black;
            lblUser.Location = new Point(70, 240);
            lblUser.Size = new Size(200, 30);
            lblUser.Text = "Username";

            // Username textbox
            txtUsername.Font = new Font("Segoe UI", 13F);
            txtUsername.BackColor = Color.White;

            // Password label
            lblPass.Font = new Font("Segoe UI", 13F);
            lblPass.ForeColor = Color.Black;
            lblPass.Location = new Point(70, 370);
            lblPass.Size = new Size(200, 30);
            lblPass.Text = "Password";

            // Password textbox
            txtPassword.Font = new Font("Segoe UI", 13F);
            txtPassword.BackColor = Color.White;
            txtPassword.PasswordChar = '*';

            // Rounded username input box
            Panel userBox = CreateRoundedBox(txtUsername);
            userBox.Location = new Point(70, 290);
            userBox.Width = 460;

            // Rounded password input box
            Panel passBox = CreateRoundedBox(txtPassword);
            passBox.Location = new Point(70, 410);
            passBox.Width = 460;

            // Login button
            btnLogin.BackColor = Color.FromArgb(37, 99, 235);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;

            btnLogin.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(180, 530);
            btnLogin.Size = new Size(260, 80);
            btnLogin.Text = "Login";
            btnLogin.Cursor = Cursors.Hand;

            btnLogin.Click += new EventHandler(btnLogin_Click);

            // Rounded login button
            RoundControl(btnLogin, 40);

            // Add controls to login card
            panelLogin.Controls.Add(lblTitle);
            panelLogin.Controls.Add(lblUser);
            panelLogin.Controls.Add(userBox);
            panelLogin.Controls.Add(lblPass);
            panelLogin.Controls.Add(passBox);
            panelLogin.Controls.Add(btnLogin);
            panelLogin.Controls.Add(lblLogo);
            panelLogin.Controls.Add(lblMessage);


            this.Controls.Add(panelLogin);


            this.ResumeLayout(false);
        }
    }
}