using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    partial class DatabaseBackup
    {
        // Database backup page controls
        private Label lblTitle;
        private Label lblPath;
        private Label lblResult;
        private TextBox txtPath;
        private Button btnBrowse;
        private Button btnBackup;
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

        // Initializes database backup page UI
        private void InitializeComponent()
        {
            SuspendLayout();

            // UserControl settings
            Dock = DockStyle.Fill;
            BackColor = Color.White;
                
            // Main layout container
            FlowLayoutPanel main = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(40)
            };

            // Page title
            lblTitle = new Label
            {
                Text = "Database Backup",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            // Backup information message
            Label info = new Label
            {
                Text = "Create a full SQL Server backup of FactoryDB.\n" +
                       "The SQL Server service account must have write access to the destination folder.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25)
            };

            // Backup location label
            lblPath = new Label
            {
                Text = "Backup file path",
                Font = new Font("Segoe UI", 13),
                AutoSize = true
            };

            // Backup path textbox
            txtPath = new TextBox
            {
                Width = 900,
                Font = new Font("Segoe UI", 12)
            };

            // Button row container
            Panel row = new Panel
            {
                Width = 950,
                Height = 70,
                Margin = new Padding(0, 15, 0, 0)
            };

            // Browse button
            btnBrowse = new Button
            {
                Text = "Browse",
                Width = 180,
                Height = 50,
                Left = 0,
                Top = 10,
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            btnBrowse.FlatAppearance.BorderSize = 0;
            RoundButton(btnBrowse);

            // Create backup button
            btnBackup = new Button
            {
                Text = "Create Backup",
                Width = 220,
                Height = 50,
                Left = 210,
                Top = 10,
                BackColor = Color.FromArgb(94, 60, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            btnBackup.FlatAppearance.BorderSize = 0;
            RoundButton(btnBackup);

            // Back navigation button
            btnBack = new Button
            {
                Text = "Back",
                Width = 180,
                Height = 50,
                Left = 460,
                Top = 10,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            btnBack.FlatAppearance.BorderSize = 0;
            RoundButton(btnBack);

            // Add buttons to row
            row.Controls.Add(btnBrowse);
            row.Controls.Add(btnBackup);
            row.Controls.Add(btnBack);

            // Result/status label
            lblResult = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                MaximumSize = new Size(900, 0),
                Margin = new Padding(0, 30, 0, 0)
            };

            // Connect button events
            btnBrowse.Click += btnBrowse_Click;
            btnBackup.Click += btnBackup_Click;
            btnBack.Click += btnBack_Click;

            // Add controls to layout
            main.Controls.Add(lblTitle);
            main.Controls.Add(info);
            main.Controls.Add(lblPath);
            main.Controls.Add(txtPath);
            main.Controls.Add(row);
            main.Controls.Add(lblResult);
            Controls.Add(main);
            ResumeLayout(false);
        }
    }
}