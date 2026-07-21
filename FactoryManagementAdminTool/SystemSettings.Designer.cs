using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    partial class SystemSettings
    {
        private Label lblTitle;
        private Label lblConn;
        private Label lblStatus;
        private Label lblStats;
        private Label lblAuditTitle;
        private TextBox txtConnection;
        private Button btnTest;
        private Button btnRefresh;
        private Button btnBack;
        private DataGridView dgvAuditLogs;
        private Button btnReloadLogs;
        private Button btnExportLogs;

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
                Padding = new Padding(40)
            };

            lblAuditTitle = new Label
            {
                Text = "Audit Logs",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 30, 0, 10)
            };

            dgvAuditLogs = new DataGridView
            {
                Width = 1000,
                Height = 300,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            

            lblTitle = new Label
            {
                Text = "System Settings",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25)
            };

            lblConn = new Label
            {
                Text = "Database Connection String (read-only)",
                Font = new Font("Segoe UI", 13),
                AutoSize = true
            };

            txtConnection = new TextBox
            {
                Width = 1000,
                Height = 80,
                Multiline = true,
                ReadOnly = true,
                Font = new Font("Consolas", 11F),
                ScrollBars = ScrollBars.Vertical
            };

            lblStatus = new Label
            {
                Text = "Database connection: —",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 10)
            };

            lblStats = new Label
            {
                Text = "Users: —   |   Admins: —",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25)
            };

            Panel buttons = new Panel
            {
                Width = 700,
                Height = 70
            };

            btnTest = MakeButton("Test Connection", Color.FromArgb(94, 60, 255), 0);
            btnRefresh = MakeButton("Refresh", Color.FromArgb(59, 130, 246), 230);

            buttons.Controls.Add(btnTest);
            buttons.Controls.Add(btnRefresh);


            // Audit buttons row
            Panel auditButtons = new Panel
            {
                Width = 700,
                Height = 70
            };

            btnReloadLogs = MakeButton(
                "Reload Logs",
                Color.FromArgb(59, 130, 246),
                0);

            btnExportLogs = MakeButton(
                "Export Logs",
                Color.FromArgb(94, 60, 255),
                230);


            btnBack = MakeButton(
                "Back",
                Color.Gray,
                460);


            

            btnTest.Click += btnTest_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnBack.Click += btnBack_Click;
            btnReloadLogs.Click += btnReloadLogs_Click;
            btnExportLogs.Click += btnExportLogs_Click;
            Label hint = new Label
            {
                Text = "Connection settings are configured in FactoryManagementCore (DBHelper).\n" +
                       "Use Backup from the sidebar to create a SQL Server database backup.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = true,
                Margin = new Padding(0, 30, 0, 0)
            };
            auditButtons.Controls.Add(btnReloadLogs);
            auditButtons.Controls.Add(btnExportLogs);
            auditButtons.Controls.Add(btnBack);
            main.Controls.Add(lblTitle);
            main.Controls.Add(lblConn);
            main.Controls.Add(txtConnection);
            main.Controls.Add(lblStatus);
            main.Controls.Add(lblStats);
            main.Controls.Add(buttons);
            main.Controls.Add(hint);
            main.Controls.Add(lblAuditTitle);
            main.Controls.Add(dgvAuditLogs);
            main.Controls.Add(auditButtons);

            Controls.Add(main);
            ResumeLayout(false);
        }

        private Button MakeButton(string text, Color back, int left)
        {
            Button btn = new Button
            {
                Text = text,
                Width = 210,
                Height = 50,
                Left = left,
                Top = 10,
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            RoundButton(btn);
            return btn;
        }
    }
}
