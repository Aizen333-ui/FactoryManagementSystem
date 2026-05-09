using System.Drawing.Drawing2D;

namespace FactoryManagementSystem
{
    partial class Login
    {
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void RoundPanel(Panel panel)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, 20, 20, 180, 90);
            path.AddArc(panel.Width - 20, 0, 20, 20, 270, 90);
            path.AddArc(panel.Width - 20, panel.Height - 20, 20, 20, 0, 90);
            path.AddArc(0, panel.Height - 20, 20, 20, 90, 90);
            path.CloseAllFigures();
            panel.Region = new Region(path);
        }
        private void RoundControl(Control ctl, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            ctl.Region = new Region(path);

            // Reapply on resize
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
        private Panel CreateRoundedBox(Control innerControl, int height = 55)
        {
            Panel container = new Panel();
            // Prefer innerControl's explicit size when provided so visual and actual control sizes match
            container.Height = (innerControl.Height > 0) ? innerControl.Height + 20 : height;
            container.Width = (innerControl.Width > 0) ? innerControl.Width + 24 : 800;
            container.BackColor = Color.White;
            container.Padding = new Padding(12, 10, 14, 12);
            container.Margin = new Padding(0, 0, 0, 15);
            container.Size = new Size(container.Width, container.Height + 2);
            container.AutoSize = false;
            container.Anchor = AnchorStyles.Left;   // 👈 VERY IMPORTANT
            // place inner control to fill the rounded container and remove its own extra margins
            innerControl.Dock = DockStyle.Fill;
            innerControl.BackColor = Color.White;
            innerControl.Margin = new Padding(0);
            if (innerControl is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 12F);
                tb.Margin = new Padding(0);
                container.Padding = new Padding(8, 6, 8, 6);
            }

            container.Controls.Add(innerControl);

            container.Paint += (s, e) =>
            {
                int radius = 12;

                // 👇 IMPORTANT: shrink drawing area
                Rectangle rect = new Rectangle(1, 1, container.Width - 2, container.Height - 2);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                    path.CloseAllFigures();

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Fill
                    using (SolidBrush brush = new SolidBrush(Color.White))
                        e.Graphics.FillPath(brush, path);

                    // 👇 BORDER (now visible on ALL sides)
                    using (Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            return container;
        }
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
            this.Resize += (s, e) =>
            {
                panelLogin.Left = (this.ClientSize.Width - panelLogin.Width) / 2;
                panelLogin.Top = (this.ClientSize.Height - panelLogin.Height) / 2;
            };
            // FORM
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // PANEL (CARD)
            this.panelLogin.BackColor = Color.FromArgb(245, 247, 250);
            this.panelLogin.Size = new Size(600, 700);
            this.panelLogin.Location = new Point(120, 80);

            // LOGO
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.lblLogo.Location = new System.Drawing.Point(260, 50);
            this.lblLogo.Size = new System.Drawing.Size(250, 65);
            this.lblLogo.Text = "🏢";

            // TITLE
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.lblTitle.Location = new System.Drawing.Point(90, 110);
            this.lblTitle.Size = new System.Drawing.Size(450, 45);
            this.lblTitle.Text = "Welcome Back";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // MESSAGE
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblMessage.ForeColor = System.Drawing.Color.Gray;
            this.lblMessage.Location = new System.Drawing.Point(150, 170);
            this.lblMessage.Size = new System.Drawing.Size(360, 30);
            this.lblMessage.Text = "Sign in to access your dashboard";

            // USER LABEL
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblUser.ForeColor = System.Drawing.Color.Black;
            this.lblUser.Location = new System.Drawing.Point(70, 240);
            this.lblUser.Size = new System.Drawing.Size(200, 30);
            this.lblUser.Text = "Username";

            // USER TEXTBOX
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtUsername.BackColor = System.Drawing.Color.White;
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // PASS LABEL
            this.lblPass.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblPass.ForeColor = System.Drawing.Color.Black;
            this.lblPass.Location = new System.Drawing.Point(70, 370);
            this.lblPass.Size = new System.Drawing.Size(200, 30);
            this.lblPass.Text = "Password";

            // PASS TEXTBOX
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.PasswordChar = '*';
            // ===== USERNAME ROUNDED BOX =====
            Panel userBox = CreateRoundedBox(txtUsername);
            userBox.Location = new Point(70, 290);
            userBox.Width = 460;

            // ===== PASSWORD ROUNDED BOX =====
            Panel passBox = CreateRoundedBox(txtPassword);
            passBox.Location = new Point(70, 410);
            passBox.Width = 460;

            // LOGIN BUTTON
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(180, 530);
            this.btnLogin.Size = new System.Drawing.Size(260, 80);
            this.btnLogin.Text = "Login";
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            RoundControl(btnLogin, 40);

            // ADD CONTROLS
            this.panelLogin.Controls.Add(this.lblTitle);
            this.panelLogin.Controls.Add(this.lblUser);
            this.panelLogin.Controls.Add(userBox);
            this.panelLogin.Controls.Add(this.lblPass);
            this.panelLogin.Controls.Add(passBox);
            this.panelLogin.Controls.Add(this.btnLogin);
            this.panelLogin.Controls.Add(this.lblLogo);
            this.panelLogin.Controls.Add(this.lblMessage);
            this.Controls.Add(this.panelLogin);

            this.ResumeLayout(false);
        }
    }
}
