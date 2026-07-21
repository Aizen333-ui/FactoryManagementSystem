using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementAdminTool
{
    public partial class AdminDashboard : Form
    {
        Button activeButton = null;
        Panel card;  // Main UI container
        Panel panelBody;
        Panel panelSideMenu, panelHeader, panelMain;
        Label lblTitle;  // Header title label
        Button btnManageUsers, btnAdminAccounts, btnSystemSettings, btnReports, btnBackup, btnLogout;
        // ===================== INITIALIZE UI COMPONENTS =====================

        private void InitializeComponent()
        {
            panelBody = new Panel();
            panelBody.Dock = DockStyle.Fill;
            panelBody.BackColor = Color.White;
            this.BackColor = Color.White;

            this.panelSideMenu = new Panel();
            this.panelHeader = new Panel();
            this.panelMain = new Panel();
            this.lblTitle = new Label();
            this.panelSideMenu.SuspendLayout();
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
            panelSideMenu.Paint += PanelSideMenu_Paint;
            panelHeader.Dock = DockStyle.Top;
            panelMain.Dock = DockStyle.Fill;

            // Buttons common style
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
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 120, 255);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 220);
                panelSideMenu.Controls.Add(btn);
                top += 70;
            }

            btnManageUsers.Text = "👥 Manage Users";
            btnAdminAccounts.Text = "🔑 Admin Accounts";
            btnSystemSettings.Text = "⚙️ System Settings";
            btnReports.Text = "📄 Reports";
            btnBackup.Text = "💾 Backup";
            btnManageUsers.Tag = "nav";
            btnAdminAccounts.Tag = "nav";
            btnSystemSettings.Tag = "nav";
            btnReports.Tag = "nav";
            btnBackup.Tag = "nav";
            // Wire up button click events to code-behind handlers
            btnManageUsers.Click += btnManageUsers_Click;
            btnAdminAccounts.Click += btnAdminAccounts_Click;
            btnSystemSettings.Click += btnSystemSettings_Click;
            btnReports.Click += btnReports_Click;
            btnBackup.Click += btnBackup_Click;

            // Logout
            btnLogout.Text = "⏻ Logout";
            btnLogout.Width = panelSideMenu.Width - 40;
            btnLogout.Height = 60;
            btnLogout.Left = 10;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;

            btnLogout.ForeColor = Color.White;
            btnLogout.BackColor = Color.FromArgb(220, 38, 38);

            btnLogout.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold);


            btnLogout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            panelSideMenu.Resize += (s, e) =>
            {
                btnLogout.Top = panelSideMenu.Height - btnLogout.Height - 20;
                btnLogout.Left = 20;
            };

            // Wire up logout click
            btnLogout.Click += btnLogout_Click;

            // Make logout button rounded
            RoundButton(btnLogout);

            panelSideMenu.Controls.Add(btnLogout);

            // ================= HEADER =================
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 120;
            panelHeader.Paint += PanelHeader_Paint;

            lblTitle.Text = "Admin Dashboard";
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.Location = new Point(30, 20);
            lblTitle.AutoSize = true;

            panelHeader.Controls.Add(lblTitle);

            // ================= MAIN =================
            // OwnerDashBoard spacing: left 60, top/bottom 40, right ~450 (scaled if narrow)
            panelMain.Dock = DockStyle.Fill;
            panelMain.BackColor = Color.FromArgb(243, 244, 246);

            card = new Panel();
            card.Dock = DockStyle.None;
            card.BackColor = Color.White;
            card.Resize += (s, e) => RoundPanel(card);

            panelMain.Controls.Add(card);
            panelMain.Resize += (s, e) => LayoutMainCard();
            this.Shown += (s, e) => LayoutMainCard();
            this.MinimumSize = new Size(1200, 700);
            this.Size = new Size(1400, 900);
            this.WindowState = FormWindowState.Maximized;
            // ================= FORM =================
            this.Controls.Add(panelBody);
            this.Controls.Add(panelHeader);
            panelBody.Controls.Add(panelMain);
            panelBody.Controls.Add(panelSideMenu);
            
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard";
            this.ResumeLayout(false);
        }

        // Keeps Owner-style right gap without collapsing the card on small widths
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
            int availableForCard = panelMain.ClientSize.Width - leftPad - preferredRight;

            if (availableForCard < minCardWidth)
                rightPad = Math.Max(40, panelMain.ClientSize.Width - leftPad - minCardWidth);

            panelMain.Padding = new Padding(leftPad, topPad, rightPad, bottomPad);

            int horiz = leftPad + rightPad;
            int vert = topPad + bottomPad;

            card.Left = leftPad;
            card.Top = topPad;
            card.Width = Math.Max(200, panelMain.ClientSize.Width - horiz);
            card.Height = Math.Max(200, panelMain.ClientSize.Height - vert);
            RoundPanel(card);
        }

        // ================= ROUNDED PANEL =================
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

        // ================= GRADIENT SIDEBAR =================
        private void PanelSideMenu_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                panelSideMenu.ClientRectangle,
                Color.FromArgb(67, 56, 202),   // deep blue
                Color.FromArgb(99, 102, 241),  // soft purple 
                90F))
            {
                e.Graphics.FillRectangle(brush, panelSideMenu.ClientRectangle);
            }
        }
        private void SetActiveButton(Button btn)
        {
            // reset previous
            if (activeButton != null)
            {
                activeButton.BackColor = Color.Transparent;
                activeButton.ForeColor = Color.White;
            }

            // set new active
            activeButton = btn;

            activeButton.BackColor = Color.White;
            activeButton.ForeColor = Color.Black;
        }
        // ================= GRADIENT HEADER =================
        private void PanelHeader_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                panelHeader.ClientRectangle,
                Color.FromArgb(124, 58, 237),
                Color.FromArgb(168, 85, 247),
                0F))
            {
                e.Graphics.FillRectangle(brush, panelHeader.ClientRectangle);
            }
        }

        // ================= ROUNDED BUTTON =================
        private void RoundButton(Button button)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = 25;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            button.Region = new Region(path);
        }
    }
}
