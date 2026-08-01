using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    partial class AdminReports
    {
        // Report page controls
        private Label lblTitle;

        private Label lblTotalUsers;
        private Label lblActiveUsers;
        private Label lblDisabledUsers;
        private Label lblAdmins;

        private DataGridView dgvByRole;
        private DataGridView dgvRecent;

        private Button btnRefresh;
        private Button btnBack;

        // Applies rounded corners to buttons
        private void RoundButton(Button btn)
        {
            GraphicsPath path = new GraphicsPath();

            int radius = 18;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);

            path.CloseAllFigures();


            btn.Region =
                new Region(path);
        }

        // Creates statistic summary cards with value labels
        private Panel CreateStatCard(string title, out Label valueLabel, Color accent)
        {
            Panel p = new Panel
            {
                Size = new Size(220, 120),
                BackColor = Color.FromArgb(248, 250, 252),
                Margin = new Padding(0, 0, 20, 20)
            };

            // Card heading
            Label t = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Black,
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.BottomCenter
            };

            // Card number display
            valueLabel = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = accent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter
            };

            p.Controls.Add(valueLabel);

            p.Controls.Add(t);


            return p;
        }

        // Initializes report page UI components
        private void InitializeComponent()
        {
            SuspendLayout();

            // UserControl settings
            Dock = DockStyle.Fill;
            BackColor =Color.White;
                
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
                Text = "System Reports",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            // Statistics card container
            FlowLayoutPanel stats = new FlowLayoutPanel
            {
                Width = 1100,
                Height = 150,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            // Add statistic cards
            stats.Controls.Add(
                CreateStatCard(
                    "Total Users",
                    out lblTotalUsers,
                    Color.FromArgb(37, 99, 235)));


            stats.Controls.Add(
                CreateStatCard(
                    "Active Users",
                    out lblActiveUsers,
                    Color.FromArgb(22, 163, 74)));


            stats.Controls.Add(
                CreateStatCard(
                    "Disabled Users",
                    out lblDisabledUsers,
                    Color.FromArgb(220, 38, 38)));


            stats.Controls.Add(
                CreateStatCard(
                    "Admins",
                    out lblAdmins,
                    Color.FromArgb(124, 58, 237)));

            // User role section heading
            Label lblRole = new Label
            {
                Text = "Users by Role",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 8)
            };

            // DataGridView displaying users grouped by role
            dgvByRole = new DataGridView
            {
                Width = 1100,
                Height = 180,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Recent users section heading
            Label lblRecent = new Label
            {
                Text = "Recent Users",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 8)
            };

            // DataGridView displaying latest registered users
            dgvRecent = new DataGridView
            {
                Width = 1100,
                Height = 260,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Action buttons container
            Panel buttons = new Panel
            {
                Width = 500,
                Height = 70,
                Margin = new Padding(0, 20, 0, 0)
            };

            // Refresh button
            btnRefresh = new Button
            {
                Text = "Refresh",
                Width = 180,
                Height = 50,
                Left = 0,
                Top = 10,
                BackColor = Color.FromArgb(94, 60, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            btnRefresh.FlatAppearance.BorderSize = 0;
            RoundButton(btnRefresh);

            // Back button
            btnBack = new Button
            {
                Text = "Back",
                Width = 180,
                Height = 50,
                Left = 210,
                Top = 10,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            btnBack.FlatAppearance.BorderSize = 0;
            RoundButton(btnBack);

            // Add buttons to container
            buttons.Controls.Add(btnRefresh);

            buttons.Controls.Add(btnBack);

            // Connect button events
            btnRefresh.Click += btnRefresh_Click;

            btnBack.Click += btnBack_Click;

            // Add controls to page layout
            main.Controls.Add(lblTitle);
            main.Controls.Add(stats);
            main.Controls.Add(lblRole);
            main.Controls.Add(dgvByRole);
            main.Controls.Add(lblRecent);
            main.Controls.Add(dgvRecent);
            main.Controls.Add(buttons);
            Controls.Add(main);
            ResumeLayout(false);
        }
    }
}