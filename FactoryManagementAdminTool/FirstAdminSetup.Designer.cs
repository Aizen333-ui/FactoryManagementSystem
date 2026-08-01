using System.Drawing.Drawing2D;

namespace FactoryManagementAdminTool
{
    public partial class FirstAdminSetup : Form
    {
        private Label lblLogo;
        private Label lblTitle;
        private Label lblMessage;

        private Label lblName;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblConfirmPassword;

        private TextBox txtName;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;

        private Button btnCreateAdmin;

        private Panel panelSetup;

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
            int verticalPadding = 10;
            int boxHeight = height;

            if (innerControl is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 12F);
                tb.Margin = new Padding(0);
                verticalPadding = (boxHeight - tb.PreferredHeight) / 2;
                if (verticalPadding < 0) verticalPadding = 0;
            }

            Panel container = new Panel();
            container.Height = boxHeight;
            container.Width = (innerControl.Width > 0) ? innerControl.Width + 24 : 800;
            container.BackColor = Color.White;
            container.Padding = new Padding(8, verticalPadding, 8, verticalPadding);
            container.Margin = new Padding(0, 0, 0, 15);
            container.Size = new Size(container.Width, container.Height + 2);
            container.AutoSize = false;
            container.Anchor = AnchorStyles.Left;
            innerControl.Dock = DockStyle.Fill;
            innerControl.BackColor = Color.White;
            innerControl.Margin = new Padding(0);

            container.Controls.Add(innerControl);

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

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Fill
                    using (SolidBrush brush = new SolidBrush(Color.White))
                        e.Graphics.FillPath(brush, path);

                    using (Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                        e.Graphics.DrawPath(pen, path);
                }
            };

            return container;
        }
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.btnCreateAdmin = new System.Windows.Forms.Button();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panelSetup = new System.Windows.Forms.Panel();

            this.SuspendLayout();

            // FORM
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Setup";



            // PANEL (CARD)
            this.panelSetup.BackColor = Color.FromArgb(245, 247, 250);
            this.panelSetup.Size = new Size(600, 760);
            this.panelSetup.Location = new Point(
                (Screen.PrimaryScreen.Bounds.Width - this.panelSetup.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.panelSetup.Height) / 2
            );



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
            this.lblTitle.Text = "Create Administrator";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // MESSAGE
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblMessage.ForeColor = System.Drawing.Color.Gray;
            this.lblMessage.Location = new System.Drawing.Point(150, 170);
            this.lblMessage.Size = new System.Drawing.Size(360, 30);
            this.lblMessage.Text = "Sign up to create your admin account";

            //FULL NAME LABEL
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblName.ForeColor = System.Drawing.Color.Black;
            this.lblName.Location = new System.Drawing.Point(70, 220);
            this.lblName.Size = new System.Drawing.Size(200, 30);
            this.lblName.Text = "Full Name";


            // FULL NAME TEXTBOX
            this.txtName.Font = new Font("Segoe UI", 13F);
            this.txtName.BackColor = Color.White;
            this.txtName.BorderStyle = BorderStyle.FixedSingle;


            Panel nameBox = CreateRoundedBox(txtName);
            nameBox.Location = new Point(70, 260);
            nameBox.Width = 460;


            // USER LABEL
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblUsername.ForeColor = System.Drawing.Color.Black;
            this.lblUsername.Location = new System.Drawing.Point(70, 340);
            this.lblUsername.Size = new System.Drawing.Size(200, 30);
            this.lblUsername.Text = "Username";

            // USER TEXTBOX
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtUsername.BackColor = System.Drawing.Color.White;
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // PASS LABEL
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblPassword.ForeColor = System.Drawing.Color.Black;
            this.lblPassword.Location = new System.Drawing.Point(70, 460);
            this.lblPassword.Size = new System.Drawing.Size(200, 30);
            this.lblPassword.Text = "Password";

            // PASS TEXTBOX
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.PasswordChar = '*';

            //CONFIRM PASS LABEL
            this.lblConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.lblConfirmPassword.ForeColor = System.Drawing.Color.Black;
            this.lblConfirmPassword.Location = new System.Drawing.Point(70, 580);
            this.lblConfirmPassword.Size = new System.Drawing.Size(200, 30);
            this.lblConfirmPassword.Text = "Confirm Password";

            //CONFIRM PASS TEXTBOX
            this.txtConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.txtConfirmPassword.BackColor = System.Drawing.Color.White;
            this.txtConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfirmPassword.PasswordChar = '*';

            // ===== USERNAME ROUNDED BOX =====
            Panel userBox = CreateRoundedBox(txtUsername);
            userBox.Location = new Point(70, 380);
            userBox.Width = 460;

            // ===== PASSWORD ROUNDED BOX =====
            Panel passBox = CreateRoundedBox(txtPassword);
            passBox.Location = new Point(70, 500);
            passBox.Width = 460;

            // ===== CONFIRM PASSWORD ROUNDED BOX =====
            Panel confirmPassBox = CreateRoundedBox(txtConfirmPassword);
            confirmPassBox.Location = new Point(70, 620);
            confirmPassBox.Width = 460;

            // CREATE ADMIN BUTTON
            this.btnCreateAdmin.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnCreateAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateAdmin.FlatAppearance.BorderSize = 0;
            this.btnCreateAdmin.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnCreateAdmin.ForeColor = System.Drawing.Color.White;
            this.btnCreateAdmin.Location = new System.Drawing.Point(180, 650);
            this.btnCreateAdmin.Size = new System.Drawing.Size(260, 80);
            this.btnCreateAdmin.Text = "Create Admin";
            this.btnCreateAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateAdmin.Click += new System.EventHandler(this.btnCreateAdmin_Click);
            RoundControl(btnCreateAdmin, 40);

            // ADD CONTROLS

            this.panelSetup.Controls.Add(this.lblTitle);
            this.panelSetup.Controls.Add(this.lblName);
            this.panelSetup.Controls.Add(nameBox);

            this.panelSetup.Controls.Add(this.lblUsername);
            this.panelSetup.Controls.Add(userBox);

            this.panelSetup.Controls.Add(this.lblPassword);
            this.panelSetup.Controls.Add(passBox);

            this.panelSetup.Controls.Add(this.lblConfirmPassword);
            this.panelSetup.Controls.Add(confirmPassBox);

            this.panelSetup.Controls.Add(this.btnCreateAdmin);

            this.panelSetup.Controls.Add(this.lblLogo);
            this.panelSetup.Controls.Add(this.lblMessage);

            this.Controls.Add(this.panelSetup);

            this.ResumeLayout(false);
        }
    }
}