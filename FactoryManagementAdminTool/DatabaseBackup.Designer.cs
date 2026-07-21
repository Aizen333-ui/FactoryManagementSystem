using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    partial class DatabaseBackup
    {
        private Label lblTitle;
        private Label lblPath;
        private Label lblResult;
        private TextBox txtPath;
        private Button btnBrowse;
        private Button btnBackup;
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

            lblTitle = new Label
            {
                Text = "Database Backup",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            Label info = new Label
            {
                Text = "Create a full SQL Server backup of FactoryDB.\n" +
                       "The SQL Server service account must have write access to the destination folder.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 25)
            };

            lblPath = new Label
            {
                Text = "Backup file path",
                Font = new Font("Segoe UI", 13),
                AutoSize = true
            };

            txtPath = new TextBox
            {
                Width = 900,
                Font = new Font("Segoe UI", 12)
            };

            Panel row = new Panel { Width = 950, Height = 70, Margin = new Padding(0, 15, 0, 0) };

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

            row.Controls.Add(btnBrowse);
            row.Controls.Add(btnBackup);
            row.Controls.Add(btnBack);

            lblResult = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                MaximumSize = new Size(900, 0),
                Margin = new Padding(0, 30, 0, 0)
            };

            btnBrowse.Click += btnBrowse_Click;
            btnBackup.Click += btnBackup_Click;
            btnBack.Click += btnBack_Click;

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
