using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    partial class ManageUsers
    {
        private System.ComponentModel.IContainer components = null;

        // ============================================================
        // LAYOUT CONSTANTS
        // Controls positions and sizes are centralized here so the UI
        // can be adjusted without modifying individual controls.
        // ============================================================
        private const int LeftMargin = 30;
        private const int FieldWidth = 1000;
        private const int FieldHeight = 50;
        private const int LabelGap = 6;       
        private const int SectionGap = 22;    

        private static readonly Point TitlePos = new Point(LeftMargin, 20);

        private static readonly Point FullNameLabelPos = new Point(LeftMargin, 85);
        private static readonly Point FullNameFieldPos = new Point(LeftMargin, 120);

        private static readonly Point UsernameLabelPos = new Point(LeftMargin, 180);
        private static readonly Point UsernameFieldPos = new Point(LeftMargin, 215);

        private static readonly Point PasswordLabelPos = new Point(LeftMargin, 275);
        private static readonly Point PasswordFieldPos = new Point(LeftMargin, 313);

        private static readonly Point RoleLabelPos = new Point(LeftMargin, 370);
        private static readonly Point RoleFieldPos = new Point(LeftMargin, 410);

        private static readonly Point ButtonRowPos = new Point(LeftMargin, 500);
        private static readonly Point SearchLabelPos = new Point(LeftMargin, 565);
        private static readonly Point SearchFieldPos = new Point(LeftMargin, 605);
        private static readonly Point SearchButtonPos = new Point(750, 605);
        private static readonly int GridTop = 665;

        private const int BackButtonBottomMargin = 20;


        // ============================================================
        // UI CONTROLS
        // Declaration of all controls used in User Management screen.
        // ============================================================
        private Label lblTitle;
        private Label lblFullName;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblRole;
        private Label lblSearch;

        private TextBox txtFullName;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtSearch;

        private ComboBox cmbRole;

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnResetPassword;
        private Button btnToggleStatus;
        private Button btnRefresh;
        private Button btnBack;
        private Button btnSearch;

        private Panel contentPanel;
        private DataGridView dgvUsers;
        // ============================================================
        // Creates modern rounded input fields with custom border style.
        // Used for TextBox and ComboBox containers.
        // ============================================================

        private Panel CreateRoundedBox(Control innerControl, int height = FieldHeight)
        {
            int verticalPadding = 10;
            if (innerControl is TextBox tb)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.Font = new Font("Segoe UI", 12F);
                verticalPadding = (height - tb.PreferredHeight) / 2;
                if (verticalPadding < 0) verticalPadding = 0;
            }

            Panel container = new Panel
            {
                Height = height,
                Width = FieldWidth,
                BackColor = Color.White,
                Padding = new Padding(10, verticalPadding, 10, verticalPadding)
            };

            if (innerControl is ComboBox cb)
            {
                cb.Location = new Point(8, 8);
                cb.Width = container.Width - 16;
                cb.Height = 35;
                cb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            }
            else
            {
                innerControl.Dock = DockStyle.Fill;
            }
            innerControl.BackColor = Color.White;

            container.Controls.Add(innerControl);

            container.Paint += (s, e) =>
            {
                Rectangle rect = new Rectangle(1, 1, container.Width - 2, container.Height - 2);
                using GraphicsPath path = new GraphicsPath();
                int radius = 12;
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseAllFigures();
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using SolidBrush b = new SolidBrush(Color.White);
                e.Graphics.FillPath(b, path);
                using Pen p = new Pen(Color.LightGray, 1.5f);
                e.Graphics.DrawPath(p, path);
            };

            return container;
        }
        // ============================================================
        // Applies rounded corners to buttons for modern UI appearance.
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
        // Creates reusable labels with common font styling.
        // ============================================================
        private Label CreateLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 13F),
                AutoSize = true,
                Location = location
            };
        }
        // ============================================================
        // Creates reusable action buttons.
        // Used for Add, Update, Delete, Search, Back etc.
        // ============================================================

        private Button CreateActionButton(string text, Color backColor, int x, int y, int width = 170)
        {
            Button btn = new Button
            {
                Text = text,
                Width = width,
                Height = 50,
                Left = x,
                Top = y,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            RoundButton(btn);
            return btn;
        }
        // ============================================================
        // Dynamically adjusts DataGridView and Back button position
        // whenever the UserControl size changes.
        // ============================================================

        private void LayoutGridAndBackButton()
        {
            if (contentPanel == null || dgvUsers == null || btnBack == null)
                return;

            int availableWidth = Math.Max(400, contentPanel.ClientSize.Width - LeftMargin * 2);
            int backTop = contentPanel.ClientSize.Height - btnBack.Height - BackButtonBottomMargin;

            dgvUsers.Left = LeftMargin;
            dgvUsers.Top = GridTop;
            dgvUsers.Width = availableWidth;
            dgvUsers.Height = Math.Max(180, backTop - GridTop - 16);

            btnBack.Left = LeftMargin;
            btnBack.Top = Math.Max(GridTop + dgvUsers.Height + 12, backTop);
        }
        // ============================================================
        // INITIALIZES ALL UI COMPONENTS
        // Creates controls, sets properties, attaches events,
        // and adds controls to the main container.
        // ============================================================

        private void InitializeComponent()
        {
            SuspendLayout();

            Dock = DockStyle.Fill;
            BackColor = Color.White;
            // --------------------------------------------------------
            // Main container panel
            // Holds all controls and supports scrolling.
            // --------------------------------------------------------
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(0, 0, 0, 20)
            };
            // --------------------------------------------------------
            // Page title
            // --------------------------------------------------------
            lblTitle = new Label
            {
                Text = "User Management",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Location = TitlePos
            };
            // --------------------------------------------------------
            // User information fields
            // Full Name, Username, Password and Role
            // --------------------------------------------------------
            lblFullName = CreateLabel("Full Name", FullNameLabelPos);
            txtFullName = new TextBox();
            Panel boxFullName = CreateRoundedBox(txtFullName);
            boxFullName.Location = FullNameFieldPos;

            lblUsername = CreateLabel("Username", UsernameLabelPos);
            txtUsername = new TextBox();
            Panel boxUsername = CreateRoundedBox(txtUsername);
            boxUsername.Location = UsernameFieldPos;

            lblPassword = CreateLabel("Password", PasswordLabelPos);
            txtPassword = new TextBox { PasswordChar = '*' };
            Panel boxPassword = CreateRoundedBox(txtPassword);
            boxPassword.Location = PasswordFieldPos;

            lblRole = CreateLabel("Role", RoleLabelPos);
            cmbRole = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new object[] { "Select a Role...", "Owner", "Manager", "Sales Person" });
            cmbRole.Location = RoleFieldPos;
            cmbRole.Width = 1000;
            cmbRole.Height = 45;
            cmbRole.Font = new Font("Segoe UI", 13F);
            cmbRole.SelectedIndex = 0;

            // --------------------------------------------------------
            // User management action buttons
            // --------------------------------------------------------
            btnAdd = CreateActionButton("Add", Color.FromArgb(94, 60, 255), 0, 0);
            btnUpdate = CreateActionButton("Update", Color.FromArgb(59, 130, 246), 180, 0);
            btnDelete = CreateActionButton("Delete", Color.FromArgb(220, 38, 38), 360, 0);
            btnResetPassword = CreateActionButton("Reset Password", Color.FromArgb(245, 158, 11), 540, 0, 190);
            btnToggleStatus = CreateActionButton("Disable User", Color.FromArgb(16, 185, 129), 740, 0, 170);
            btnRefresh = CreateActionButton( "Refresh",Color.Gray,920,0,150);

            Panel buttonPanel = new Panel
            {
                Location = ButtonRowPos,
                Size = new Size(1080, 55),
                BackColor = Color.Transparent
            };
            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnUpdate);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnResetPassword);
            buttonPanel.Controls.Add(btnToggleStatus);
            buttonPanel.Controls.Add(btnRefresh);

            // --------------------------------------------------------
            // Search section
            // --------------------------------------------------------
            lblSearch = CreateLabel("Search users", SearchLabelPos);
            txtSearch = new TextBox();
            Panel boxSearch = CreateRoundedBox(txtSearch, 45);
            boxSearch.Width = 700;
            boxSearch.Location = SearchFieldPos;

            btnSearch = CreateActionButton("Search", Color.FromArgb(99, 102, 241), 0, 0, 150);
            btnSearch.Height = 45;
            RoundButton(btnSearch);
            btnSearch.Location = SearchButtonPos;
            // --------------------------------------------------------
            // Users DataGridView
            // Displays registered system users.
            // --------------------------------------------------------

            dgvUsers = new DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            // --------------------------------------------------------
            // Navigation button
            // Returns to dashboard.
            // --------------------------------------------------------
            btnBack = CreateActionButton("Back", Color.Gray, 0, 0, 180);
            btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            // --------------------------------------------------------
            // Event handlers
            // Connect UI actions with backend logic.
            // --------------------------------------------------------
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnBack.Click += btnBack_Click;
            btnResetPassword.Click += btnResetPassword_Click;
            btnToggleStatus.Click += btnToggleStatus_Click;
            btnSearch.Click += btnSearch_Click;
            dgvUsers.CellClick += dgvUsers_CellClick;
            btnRefresh.Click += btnRefresh_Click;
            // --------------------------------------------------------
            // Add controls to UserControl
            // --------------------------------------------------------

            contentPanel.Controls.Add(lblTitle);
            contentPanel.Controls.Add(lblFullName);
            contentPanel.Controls.Add(boxFullName);
            contentPanel.Controls.Add(lblUsername);
            contentPanel.Controls.Add(boxUsername);
            contentPanel.Controls.Add(lblPassword);
            contentPanel.Controls.Add(boxPassword);
            contentPanel.Controls.Add(lblRole);
            contentPanel.Controls.Add(cmbRole);
            contentPanel.Controls.Add(buttonPanel);
            contentPanel.Controls.Add(lblSearch);
            contentPanel.Controls.Add(boxSearch);
            contentPanel.Controls.Add(btnSearch);
            contentPanel.Controls.Add(dgvUsers);
            contentPanel.Controls.Add(btnBack);
            // Resize handling keeps grid and navigation responsive.

            contentPanel.Resize += (s, e) => LayoutGridAndBackButton();
            Load += (s, e) => LayoutGridAndBackButton();

            Controls.Add(contentPanel);
            ResumeLayout(false);
        }
    }
}