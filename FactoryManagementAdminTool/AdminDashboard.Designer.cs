using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    public partial class AdminDashboard : Form
    {
        // Stores currently selected sidebar button
        Button activeButton = null;

        // Main content container
        Panel card;

        // Main layout panels
        Panel panelBody;
        Panel panelSideMenu;
        Panel panelHeader;
        Panel panelMain;

        // Dashboard header label
        Label lblTitle;

        // Sidebar navigation buttons
        Button btnManageUsers;
        Button btnAdminAccounts;
        Button btnSystemSettings;
        Button btnReports;
        Button btnBackup;
        Button btnLogout;

        // Initializes dashboard UI components
        private void InitializeComponent()
        {
            // Main body panel
            panelBody = new Panel();
            panelBody.Dock = DockStyle.Fill;
            panelBody.BackColor = Color.White;

            this.BackColor = Color.White;


            // Create dashboard panels and controls
            this.panelSideMenu = new Panel();
            this.panelHeader = new Panel();
            this.panelMain = new Panel();
            this.lblTitle = new Label();

            this.btnManageUsers = new Button();
            this.btnAdminAccounts = new Button();
            this.btnSystemSettings = new Button();
            this.btnReports = new Button();
            this.btnBackup = new Button();
            this.btnLogout = new Button();


            this.SuspendLayout();

            // ================= SIDEBAR =================

            panelSideMenu.Dock = DockStyle.Left;
            panelSideMenu.Width = 450;

            // Draw gradient background
            panelSideMenu.Paint += PanelSideMenu_Paint;


            // Header and main content positioning
            panelHeader.Dock = DockStyle.Top;
            panelMain.Dock = DockStyle.Fill;

            // Common navigation button styling
            Button[] buttons =
            {
                btnManageUsers,
                btnAdminAccounts,
                btnSystemSettings,
                btnReports,
                btnBackup
            };

            int top = 100;

            foreach (var btn in buttons)
            {
                btn.Width = panelSideMenu.Width;
                btn.Height = 60;
                btn.Left = 0;
                btn.Top = top;

                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;

                btn.ForeColor = Color.White;
                btn.BackColor = Color.Transparent;

                btn.Font = new Font("Segoe UI Emoji", 16F);

                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(20, 0, 0, 0);

                // Hover and click effects
                btn.FlatAppearance.MouseOverBackColor =
                    Color.FromArgb(120, 120, 255);

                btn.FlatAppearance.MouseDownBackColor =
                    Color.FromArgb(90, 90, 220);

                panelSideMenu.Controls.Add(btn);

                top += 70;
            }

            // Sidebar button text
            btnManageUsers.Text = "👥 Manage Users";
            btnAdminAccounts.Text = "🔑 Admin Accounts";
            btnSystemSettings.Text = "⚙️ System Settings";
            btnReports.Text = "📄 Reports";
            btnBackup.Text = "💾 Backup";

            // Used for identifying navigation buttons
            btnManageUsers.Tag = "nav";
            btnAdminAccounts.Tag = "nav";
            btnSystemSettings.Tag = "nav";
            btnReports.Tag = "nav";
            btnBackup.Tag = "nav";

            // Navigation events
            btnManageUsers.Click += btnManageUsers_Click;
            btnAdminAccounts.Click += btnAdminAccounts_Click;
            btnSystemSettings.Click += btnSystemSettings_Click;
            btnReports.Click += btnReports_Click;
            btnBackup.Click += btnBackup_Click;


            // ================= LOGOUT BUTTON =================

            btnLogout.Text = "⏻ Logout";

            btnLogout.Width = panelSideMenu.Width - 40;
            btnLogout.Height = 60;

            btnLogout.Left = 10;

            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;

            btnLogout.ForeColor = Color.White;
            btnLogout.BackColor = Color.FromArgb(220, 38, 38);

            btnLogout.Font =
                new Font("Segoe UI Emoji", 12F, FontStyle.Bold);

            // Keep logout button at bottom of sidebar
            btnLogout.Anchor =
                AnchorStyles.Bottom | AnchorStyles.Left;

            panelSideMenu.Resize += (s, e) =>
            {
                btnLogout.Top =
                    panelSideMenu.Height - btnLogout.Height - 20;

                btnLogout.Left = 20;
            };

            btnLogout.Click += btnLogout_Click;

            // Apply rounded shape
            RoundButton(btnLogout);

            panelSideMenu.Controls.Add(btnLogout);

            // ================= HEADER =================

            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 120;

            // Draw header gradient
            panelHeader.Paint += PanelHeader_Paint;


            lblTitle.Text = "Admin Dashboard";

            lblTitle.ForeColor = Color.White;

            lblTitle.BackColor = Color.Transparent;

            lblTitle.Font =
                new Font("Segoe UI", 22F, FontStyle.Bold);

            lblTitle.Location = new Point(30, 20);

            lblTitle.AutoSize = true;


            panelHeader.Controls.Add(lblTitle);

            // ================= MAIN CONTENT =================

            panelMain.Dock = DockStyle.Fill;

            panelMain.BackColor =
                Color.FromArgb(243, 244, 246);

            // Main white content card
            card = new Panel();

            card.Dock = DockStyle.None;

            card.BackColor = Color.White;


            // Keep rounded corners after resizing
            card.Resize += (s, e) => RoundPanel(card);


            panelMain.Controls.Add(card);


            // Resize card with dashboard
            panelMain.Resize += (s, e) => LayoutMainCard();

            this.Shown += (s, e) => LayoutMainCard();

            // Form sizing settings
            this.MinimumSize = new Size(1200, 700);

            this.Size = new Size(1400, 900);

            this.WindowState =
                FormWindowState.Maximized;

            // ================= FORM SETTINGS =================

            this.Controls.Add(panelBody);

            this.Controls.Add(panelHeader);


            panelBody.Controls.Add(panelMain);

            panelBody.Controls.Add(panelSideMenu);


            this.FormBorderStyle =
                FormBorderStyle.Sizable;

            this.StartPosition =
                FormStartPosition.CenterScreen;

            this.Text = "Admin Dashboard";


            this.ResumeLayout(false);
        }

        // Adjusts the main content card size and position
        private void LayoutMainCard()
        {
            if (card == null || panelMain == null)
                return;

            const int leftPad = 60;
            const int topPad = 40;
            const int bottomPad = 40;
            const int preferredRight = 450;
            const int minCardWidth = 400;

            int rightPad = preferredRight;

            int availableForCard =
                panelMain.ClientSize.Width -
                leftPad -
                preferredRight;

            // Reduce right spacing on smaller screens
            if (availableForCard < minCardWidth)
            {
                rightPad = Math.Max(40, panelMain.ClientSize.Width - leftPad - minCardWidth);
            }

            panelMain.Padding = new Padding(leftPad, topPad, rightPad, bottomPad);

            int horiz = leftPad + rightPad;
            int vert = topPad + bottomPad;

            card.Left = leftPad;
            card.Top = topPad;

            card.Width = Math.Max(200, panelMain.ClientSize.Width - horiz);
            card.Height = Math.Max(200, panelMain.ClientSize.Height - vert);
            RoundPanel(card);
        }

        // Applies rounded corners to panels
        private void RoundPanel(Panel panel)
        {
            if (panel.Width < 40 || panel.Height < 40)
                return;


            GraphicsPath path = new GraphicsPath();

            int radius = 20;


            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);

            path.CloseAllFigures();


            panel.Region = new Region(path);
        }

        // Draws sidebar gradient background
        private void PanelSideMenu_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush =
                new LinearGradientBrush(
                    panelSideMenu.ClientRectangle,
                    Color.FromArgb(67, 56, 202),
                    Color.FromArgb(99, 102, 241),
                    90F))
            {
                e.Graphics.FillRectangle(
                    brush,
                    panelSideMenu.ClientRectangle);
            }
        }

        // Changes selected sidebar button appearance
        private void SetActiveButton(Button btn)
        {
            // Reset previous active button
            if (activeButton != null)
            {
                activeButton.BackColor =
                    Color.Transparent;

                activeButton.ForeColor =
                    Color.White;
            }

            // Set new active button
            activeButton = btn;

            activeButton.BackColor =
                Color.White;

            activeButton.ForeColor =
                Color.Black;
        }

        // Draws header gradient background
        private void PanelHeader_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush =
                new LinearGradientBrush(
                    panelHeader.ClientRectangle,
                    Color.FromArgb(124, 58, 237),
                    Color.FromArgb(168, 85, 247),
                    0F))
            {
                e.Graphics.FillRectangle(
                    brush,
                    panelHeader.ClientRectangle);
            }
        }

        // Applies rounded corners to buttons
        private void RoundButton(Button button)
        {
            GraphicsPath path = new GraphicsPath();

            int radius = 25;


            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, button.Height - radius, radius, radius, 90, 90);


            path.CloseAllFigures();


            button.Region =
                new Region(path);
        }
    }
}