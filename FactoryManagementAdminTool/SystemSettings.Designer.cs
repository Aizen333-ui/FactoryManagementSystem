using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    partial class SystemSettings
    {
        // ============================================================
        // UI Controls
        // Contains all controls used in System Settings page.
        // ============================================================

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

        // ============================================================
        // Creates rounded corners for buttons.
        // Provides consistent modern UI styling.
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
        // Initializes System Settings UI.
        //
        // Creates:
        // - Database information section
        // - Connection testing controls
        // - System statistics
        // - Audit log viewer
        // - Export and navigation buttons
        //
        // Also attaches required event handlers.
        // ============================================================

        private void InitializeComponent()
        {
            SuspendLayout();

            // --------------------------------------------------------
            // Main UserControl settings
            // --------------------------------------------------------

            Dock = DockStyle.Fill;
            BackColor = Color.White;
            // --------------------------------------------------------
            // Main layout container
            // Uses FlowLayoutPanel for automatic vertical arrangement.
            // --------------------------------------------------------

            FlowLayoutPanel main = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(40)
            };

            // --------------------------------------------------------
            // Page Title
            // --------------------------------------------------------

            lblTitle = new Label
            {
                Text = "System Settings",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25)
            };

            // --------------------------------------------------------
            // Database Connection Information
            // Displays current DBHelper connection string.
            // Field is read-only for security.
            // --------------------------------------------------------

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

            // --------------------------------------------------------
            // Database connection status display.
            // Updated when connection test is executed.
            // --------------------------------------------------------

            lblStatus = new Label
            {
                Text = "Database connection: —",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 20, 0, 10)
            };

            // --------------------------------------------------------
            // System statistics display.
            // Shows user and administrator counts.
            // --------------------------------------------------------

            lblStats = new Label
            {
                Text = "Users: —   |   Admins: —",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25)
            };

            // --------------------------------------------------------
            // Database action buttons.
            // --------------------------------------------------------

            Panel buttons = new Panel
            {
                Width = 700,
                Height = 70
            };

            btnTest = MakeButton("Test Connection", Color.FromArgb(94, 60, 255), 0);
            btnRefresh = MakeButton("Refresh", Color.FromArgb(59, 130, 246), 230);

            buttons.Controls.Add(btnTest);
            buttons.Controls.Add(btnRefresh);

            // --------------------------------------------------------
            // Audit Logs Section
            // --------------------------------------------------------

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

            // --------------------------------------------------------
            // Audit log action buttons.
            // Reloads and exports audit records.
            // --------------------------------------------------------

            Panel auditButtons = new Panel
            {
                Width = 700,

                Height = 70
            };
            btnReloadLogs = MakeButton("Reload Logs", Color.FromArgb(59, 130, 246), 0);
            btnExportLogs = MakeButton("Export Logs", Color.FromArgb(94, 60, 255), 230);
            btnBack = MakeButton("Back", Color.Gray, 460);

            // --------------------------------------------------------
            // Connect button events to backend logic.
            // --------------------------------------------------------

            btnTest.Click += btnTest_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnBack.Click += btnBack_Click;
            btnReloadLogs.Click += btnReloadLogs_Click;
            btnExportLogs.Click += btnExportLogs_Click;

            // --------------------------------------------------------
            // Information hint shown at bottom of page.
            // --------------------------------------------------------

            Label hint = new Label
            {
                Text = "Connection settings are configured in FactoryManagementCore (DBHelper).\n" +
                       "Use Backup from the sidebar to create a SQL Server database backup.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = true,
                Margin = new Padding(0, 30, 0, 0)
            };

            // Add audit controls
            auditButtons.Controls.Add(btnReloadLogs);
            auditButtons.Controls.Add(btnExportLogs);
            auditButtons.Controls.Add(btnBack);

            // --------------------------------------------------------
            // Add all controls to main layout.
            // --------------------------------------------------------

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

        // ============================================================
        // Creates reusable styled buttons.
        //
        // Used for:
        // - Test Connection
        // - Refresh
        // - Reload Logs
        // - Export Logs
        // - Back
        // ============================================================

        private Button MakeButton(
            string text,
            Color back,
            int left)
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