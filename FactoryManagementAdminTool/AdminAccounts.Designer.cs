using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    partial class AdminAccounts
    {
        // UI Controls
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblConfirm;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtConfirm;

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnBack;

        private DataGridView dgvAdmins;

        /// Creates a textbox container with rounded border styling.
        /// Used to provide a modern input field appearance.
        private Panel CreateRoundedBox(Control inner, int height = 55)
        {
            int verticalPadding = 10;

            // Apply textbox-specific styling
            if (inner is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 12F);

                // Vertically center textbox text
                verticalPadding = (height - tb.PreferredHeight) / 2;

                if (verticalPadding < 0)
                    verticalPadding = 0;
            }

            // Outer container used for rounded border effect
            Panel container = new Panel();
            container.Height = height;
            container.Width = 900;
            container.BackColor = Color.White;
            container.Padding =
                new Padding(10, verticalPadding, 10, verticalPadding);


            inner.Dock = DockStyle.Fill;
            container.Controls.Add(inner);


            // Draw rounded border around textbox container
            container.Paint += (s, e) =>
            {
                Rectangle rect =
                    new Rectangle(1, 1, container.Width - 2, container.Height - 2);

                using GraphicsPath path = new GraphicsPath();

                int radius = 12;

                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

                path.CloseAllFigures();

                e.Graphics.SmoothingMode =
                    SmoothingMode.AntiAlias;

                using Pen p =
                    new Pen(Color.LightGray, 1.5f);

                e.Graphics.DrawPath(p, path);
            };

            return container;
        }

        /// Applies rounded corners to buttons.
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

        /// Initializes all controls, styling, layout,
        /// and event handlers for the Admin Accounts page.
        private void InitializeComponent()
        {
            SuspendLayout();

            // Main UserControl settings
            Dock = DockStyle.Fill;
            BackColor = Color.White;

            // Main vertical layout container
            FlowLayoutPanel main = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(30)
            };

            // Page title
            lblTitle = new Label
            {
                Text = "Admin Accounts",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            // Username input
            lblUsername = new Label
            {
                Text = "Username",
                Font = new Font("Segoe UI", 13),
                AutoSize = true
            };

            txtUsername = new TextBox
            {
                Width = 900
            };

            // Password input
            lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 13),
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Width = 900,
                PasswordChar = '*'
            };

            // Confirm password input
            lblConfirm = new Label
            {
                Text = "Confirm Password",
                Font = new Font("Segoe UI", 13),
                AutoSize = true
            };

            txtConfirm = new TextBox
            {
                Width = 900,
                PasswordChar = '*'
            };

            // Button container
            Panel buttonPanel = new Panel
            {
                Width = 950,
                Height = 70
            };

            // Action buttons
            btnAdd = new Button
            {
                Text = "Add",
                Width = 180,
                Height = 50,
                Left = 0,
                Top = 10
            };

            btnUpdate = new Button
            {
                Text = "Update",
                Width = 180,
                Height = 50,
                Left = 210,
                Top = 10
            };

            btnDelete = new Button
            {
                Text = "Delete",
                Width = 180,
                Height = 50,
                Left = 420,
                Top = 10
            };

            // Apply common button styling
            foreach (Button btn in new[]
                     { btnAdd, btnUpdate, btnDelete })
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.ForeColor = Color.White;
                btn.Font =
                    new Font("Segoe UI", 12, FontStyle.Bold);

                RoundButton(btn);

                buttonPanel.Controls.Add(btn);
            }

            // Button colors based on action type
            btnAdd.BackColor =
                Color.FromArgb(94, 60, 255);

            btnUpdate.BackColor =
                Color.FromArgb(59, 130, 246);

            btnDelete.BackColor =
                Color.FromArgb(220, 38, 38);

            // Admin accounts table
            dgvAdmins = new DataGridView
            {
                Width = 1100,
                Height = 380,

                // Prevent direct editing
                ReadOnly = true,

                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,

                // Select complete admin record
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                MultiSelect = false,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,

                RowHeadersVisible = false
            };

            // Back navigation button
            btnBack = new Button
            {
                Text = "Back",
                Width = 180,
                Height = 50,

                BackColor = Color.Gray,
                ForeColor = Color.White,

                FlatStyle = FlatStyle.Flat,

                Font =
                    new Font("Segoe UI", 12, FontStyle.Bold)
            };

            btnBack.FlatAppearance.BorderSize = 0;

            RoundButton(btnBack);

            // Connect UI events with backend logic
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnBack.Click += btnBack_Click;

            dgvAdmins.CellClick += dgvAdmins_CellClick;

            // Add controls to page layout
            main.Controls.Add(lblTitle);

            main.Controls.Add(lblUsername);
            main.Controls.Add(CreateRoundedBox(txtUsername));

            main.Controls.Add(lblPassword);
            main.Controls.Add(CreateRoundedBox(txtPassword));

            main.Controls.Add(lblConfirm);
            main.Controls.Add(CreateRoundedBox(txtConfirm));

            main.Controls.Add(buttonPanel);

            main.Controls.Add(dgvAdmins);

            main.Controls.Add(btnBack);

            // Load final layout into UserControl
            Controls.Add(main);

            ResumeLayout(false);
        }
    }
}