using System.Drawing;
using System.Drawing.Drawing2D;

namespace FactoryManagementAdminTool
{
    partial class ResetPassword
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label lblUsername;
        private Label lblNewPassword;
        private Label lblConfirmPassword;

        private TextBox txtUsername;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;

        private Button btnSave;
        private Button btnCancel;

        private void RoundButton(Button btn)
        {
            GraphicsPath path = new GraphicsPath();

            int radius = 18;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius,
                        btn.Height - radius,
                        radius,
                        radius,
                        0,
                        90);

            path.AddArc(0,
                        btn.Height - radius,
                        radius,
                        radius,
                        90,
                        90);

            path.CloseAllFigures();

            btn.Region = new Region(path);
        }


        private Panel CreateRoundedBox(Control control)
        {
            int verticalPadding = 10;
            int height = 50;
            if (control is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 12F);
                verticalPadding = (height - tb.PreferredHeight) / 2;
                if (verticalPadding < 0) verticalPadding = 0;
            }

            Panel panel = new Panel();

            panel.Width = 440;
            panel.Height = height;
            panel.BackColor = Color.White;
            panel.Padding = new Padding(10, verticalPadding, 10, verticalPadding);

            control.Dock = DockStyle.Fill;
            panel.Controls.Add(control);


            panel.Paint += (s, e) =>
            {
                using GraphicsPath path = new GraphicsPath();

                int radius = 12;

                Rectangle rect = new Rectangle(
                    1,
                    1,
                    panel.Width - 2,
                    panel.Height - 2
                );


                path.AddArc(rect.X,
                            rect.Y,
                            radius,
                            radius,
                            180,
                            90);

                path.AddArc(rect.Right - radius,
                            rect.Y,
                            radius,
                            radius,
                            270,
                            90);

                path.AddArc(rect.Right - radius,
                            rect.Bottom - radius,
                            radius,
                            radius,
                            0,
                            90);

                path.AddArc(rect.X,
                            rect.Bottom - radius,
                            radius,
                            radius,
                            90,
                            90);

                path.CloseAllFigures();


                e.Graphics.SmoothingMode =
                    SmoothingMode.AntiAlias;


                using Pen pen =
                    new Pen(Color.LightGray, 1.5f);

                e.Graphics.DrawPath(pen, path);
            };


            return panel;
        }



        private void InitializeComponent()
        {
            SuspendLayout();


            this.Text = "Reset Password";
            this.Size = new Size(580, 600);
            this.StartPosition =
                FormStartPosition.CenterParent;

            this.BackColor = Color.White;

            this.FormBorderStyle =
                FormBorderStyle.FixedDialog;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Load += (s, e) => {
                lblTitle.Left = (this.ClientSize.Width - lblTitle.Width) / 2;
            };


            lblTitle = new Label();

            lblTitle.Text = "Reset Password";
            lblTitle.Font =
                new Font("Segoe UI", 20, FontStyle.Bold);

            lblTitle.AutoSize = true;
            lblTitle.Location =
                new Point(180, 35);



            lblUsername = new Label();
            lblUsername.Text = "Username";
            lblUsername.Font = new Font("Segoe UI", 13);
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(70, 110);

            txtUsername = new TextBox();
            txtUsername.ReadOnly = true;

            Panel userBox = CreateRoundedBox(txtUsername);
            userBox.Location = new Point(70, 148);



            lblNewPassword = new Label();
            lblNewPassword.Text = "New Password";
            lblNewPassword.Font = new Font("Segoe UI", 13);
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(70, 228);

            txtNewPassword = new TextBox();
            txtNewPassword.PasswordChar = '*';

            Panel passBox = CreateRoundedBox(txtNewPassword);
            passBox.Location = new Point(70, 266);



            lblConfirmPassword = new Label();
            lblConfirmPassword.Text = "Confirm Password";
            lblConfirmPassword.Font = new Font("Segoe UI", 13);
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(70, 346);

            txtConfirmPassword = new TextBox();
            txtConfirmPassword.PasswordChar = '*';

            Panel confirmBox = CreateRoundedBox(txtConfirmPassword);
            confirmBox.Location = new Point(70, 384);




            btnSave = new Button();

            btnSave.Text = "Save";

            btnSave.Size = new Size(170, 50);
            btnSave.Location = new Point(70, 464);
            btnSave.BackColor = Color.FromArgb(94, 60, 255);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnSave.FlatAppearance.BorderSize = 0;
            RoundButton(btnSave);

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new Size(170, 50);
            btnCancel.Location = new Point(290, 464);
            btnCancel.BackColor = Color.Gray;
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnCancel.FlatAppearance.BorderSize = 0;
            RoundButton(btnCancel);


            // ---- Events ----
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;


            // ---- Add Controls ----
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