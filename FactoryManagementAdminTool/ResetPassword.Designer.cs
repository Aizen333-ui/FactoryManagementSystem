using System.Drawing;
using System.Drawing.Drawing2D;

namespace FactoryManagementAdminTool
{
    partial class ResetPassword
    {
        private System.ComponentModel.IContainer components = null;


        // ============================================================
        // UI Controls
        // Contains all controls used in Reset Password form.
        // ============================================================

        private Label lblTitle;
        private Label lblUsername;
        private Label lblNewPassword;
        private Label lblConfirmPassword;

        private TextBox txtUsername;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;

        private Button btnSave;
        private Button btnCancel;



        // ============================================================
        // Creates rounded corners for buttons.
        // Provides modern UI appearance.
        // ============================================================

        private void RoundButton(Button btn)
        {
            GraphicsPath path = new GraphicsPath();

            int radius = 18;


            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);

            path.CloseAllFigures();
            btn.Region = new Region(path);
        }

        // ============================================================
        // Creates a rounded input field container.
        //
        // Used to provide:
        // - Border styling
        // - Padding
        // - Rounded appearance
        //
        // TextBox controls receive custom font and border settings.
        // ============================================================

        private Panel CreateRoundedBox(Control control)
        {
            int verticalPadding = 10;
            int height = 50;


            if (control is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 12F);

                verticalPadding = (height - tb.PreferredHeight) / 2;

                if (verticalPadding < 0)
                    verticalPadding = 0;
            }

            Panel panel = new Panel
            {
                Width = 440,
                Height = height,
                BackColor = Color.White,
                Padding = new Padding(
                    10,
                    verticalPadding,
                    10,
                    verticalPadding)
            };

            control.Dock = DockStyle.Fill;
            panel.Controls.Add(control);

            // Custom rounded border drawing
            panel.Paint += (s, e) =>
            {
                using GraphicsPath path = new GraphicsPath();

                int radius = 12;

                Rectangle rect = new Rectangle(1, 1, panel.Width - 2, panel.Height - 2);

                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

                path.CloseAllFigures();

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using Pen pen = new Pen(Color.LightGray, 1.5f);

                e.Graphics.DrawPath(pen, path);
            };

            return panel;
        }

        // ============================================================
        // Initializes Reset Password form.
        //
        // Creates:
        // - Labels
        // - Input fields
        // - Buttons
        // - Event handlers
        //
        // Also defines form appearance and layout.
        // ============================================================

        private void InitializeComponent()
        {
            SuspendLayout();

            // --------------------------------------------------------
            // Form Settings
            // --------------------------------------------------------

            this.Text = "Reset Password";
            this.Size = new Size(580, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // --------------------------------------------------------
            // Center title horizontally after form loads.
            // --------------------------------------------------------

            this.Load += (s, e) =>
            {
                lblTitle.Left =
                    (this.ClientSize.Width -
                    lblTitle.Width) / 2;
            };

            // --------------------------------------------------------
            // Form Title
            // --------------------------------------------------------

            lblTitle = new Label
            {
                Text = "Reset Password",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(180, 35)
            };

            // --------------------------------------------------------
            // Username Section
            // Username is displayed as read-only because password
            // reset is performed for the selected account.
            // --------------------------------------------------------

            lblUsername = new Label
            {
                Text = "Username",
                Font = new Font("Segoe UI", 13),
                AutoSize = true,
                Location = new Point(70, 110)
            };

            txtUsername = new TextBox
            {
                ReadOnly = true
            };

            Panel userBox =
                CreateRoundedBox(txtUsername);

            userBox.Location =
                new Point(70, 148);

            // --------------------------------------------------------
            // New Password Field
            // --------------------------------------------------------

            lblNewPassword = new Label
            {
                Text = "New Password",
                Font = new Font("Segoe UI", 13),
                AutoSize = true,
                Location = new Point(70, 228)
            };

            txtNewPassword = new TextBox
            {
                PasswordChar = '*'
            };

            Panel passBox =
                CreateRoundedBox(txtNewPassword);

            passBox.Location =
                new Point(70, 266);

            // --------------------------------------------------------
            // Confirm Password Field
            // --------------------------------------------------------

            lblConfirmPassword = new Label
            {
                Text = "Confirm Password",
                Font = new Font("Segoe UI", 13),
                AutoSize = true,
                Location = new Point(70, 346)
            };

            txtConfirmPassword = new TextBox
            {
                PasswordChar = '*'
            };

            Panel confirmBox =
                CreateRoundedBox(txtConfirmPassword);

            confirmBox.Location =
                new Point(70, 384);

            // --------------------------------------------------------
            // Save Button
            // Saves the new password.
            // --------------------------------------------------------

            btnSave = new Button
            {
                Text = "Save",
                Size = new Size(170, 50),
                Location = new Point(70, 464),
                BackColor = Color.FromArgb(94, 60, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            btnSave.FlatAppearance.BorderSize = 0;
            RoundButton(btnSave);

            // --------------------------------------------------------
            // Cancel Button
            // Closes reset password window.
            // --------------------------------------------------------

            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(170, 50),
                Location = new Point(290, 464),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };

            btnCancel.FlatAppearance.BorderSize = 0;
            RoundButton(btnCancel);

            // --------------------------------------------------------
            // Event Handlers
            // Connects buttons with backend logic.
            // --------------------------------------------------------

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            // --------------------------------------------------------
            // Add controls to form.
            // --------------------------------------------------------

            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Controls.Add(userBox);
            Controls.Add(lblNewPassword);
            Controls.Add(passBox);
            Controls.Add(lblConfirmPassword);
            Controls.Add(confirmBox);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);

            ResumeLayout(false);
        }
    }
}