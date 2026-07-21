using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    partial class AdminReports
    {
        private Label lblTitle;
        private Label lblTotalUsers;
        private Label lblActiveUsers;
        private Label lblDisabledUsers;
        private Label lblAdmins;
        private DataGridView dgvByRole;
        private DataGridView dgvRecent;
        private Button btnRefresh;
        private Button btnBack;

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

        private Panel CreateStatCard(string title, out Label valueLabel, Color accent)
        {
            Panel p = new Panel
            {
                Size = new Size(220, 120),
                BackColor = Color.FromArgb(248, 250, 252),
                Margin = new Padding(0, 0, 20, 20)
            };

            Label t = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Black,
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.BottomCenter
            };

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

        private void InitializeComponent()
        {
            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.White;

            FlowLayoutPanel main = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(30)
            };

            lblTitle = new Label
            {
                Text = "System Reports",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            FlowLayoutPanel stats = new FlowLayoutPanel
            {
                Width = 1100,
                Height = 150,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            stats.Controls.Add(CreateStatCard("Total Users", out lblTotalUsers, Color.FromArgb(37, 99, 235)));
            stats.Controls.Add(CreateStatCard("Active Users", out lblActiveUsers, Color.FromArgb(22, 163, 74)));
            stats.Controls.Add(CreateStatCard("Disabled Users", out lblDisabledUsers, Color.FromArgb(220, 38, 38)));
            stats.Controls.Add(CreateStatCard("Admins", out lblAdmins, Color.FromArgb(124, 58, 237)));

            Label lblRole = new Label
            {
                Text = "Users by Role",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 8)
            };

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

            Label lblRecent = new Label
            {
                Text = "Recent Users",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 8)
            };

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

            Panel buttons = new Panel { Width = 500, Height = 70, Margin = new Padding(0, 20, 0, 0) };
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

            buttons.Controls.Add(btnRefresh);
            buttons.Controls.Add(btnBack);

            btnRefresh.Click += btnRefresh_Click;
            btnBack.Click += btnBack_Click;

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
