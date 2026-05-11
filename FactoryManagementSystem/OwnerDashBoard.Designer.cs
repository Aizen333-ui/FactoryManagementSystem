using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    public partial class OwnerDashBoard : Form
    {
        Button activeButton = null;
        Panel card;  // Main UI container
        Panel panelBody;
        Panel panelSideMenu, panelHeader, panelMain;
        Label lblTitle;  // Header title label
        Button btnRawMaterial, btnPayments, btnManageWorkers, btnReports, btnLogout;          // Sidebar buttons

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
            this.btnRawMaterial = new Button();
            this.btnPayments = new Button();
            this.btnManageWorkers = new Button();
            this.btnReports = new Button();
            this.btnLogout = new Button();

            this.SuspendLayout();

            // ================= SIDEBAR =================
            panelSideMenu.Dock = DockStyle.Left;
            panelSideMenu.Width = 450;
            panelSideMenu.Paint += PanelSideMenu_Paint;
            panelHeader.Dock = DockStyle.Top;
            panelMain.Dock = DockStyle.Fill;

            // Buttons common style
            Button[] buttons = { btnRawMaterial, btnPayments, btnManageWorkers, btnReports };

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

            btnRawMaterial.Text = "📦 Raw Material";
            btnPayments.Text = "💰 Payments";
            btnManageWorkers.Text = "👷  Manage Workers";
            btnReports.Text = "📄 Reports";
            btnRawMaterial.Tag = "nav";
            btnPayments.Tag = "nav";
            btnManageWorkers.Tag = "nav";
            btnReports.Tag = "nav";
            // Wire up button click events to code-behind handlers
            btnRawMaterial.Click += btnRawMaterial_Click;
            btnPayments.Click += btnPayments_Click;
            btnManageWorkers.Click += btnManageWorkers_Click;
            btnReports.Click += btnReports_Click;

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

            lblTitle.Text = "Owner Dashboard";
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.Location = new Point(30, 20);
            lblTitle.AutoSize = true;

            panelHeader.Controls.Add(lblTitle);

            // ================= MAIN =================
            panelMain.Dock = DockStyle.Fill;
            panelMain.BackColor = Color.FromArgb(243, 244, 246);
            
            panelMain.Padding = new Padding(60, 40, 450, 40);
            panelMain.Resize += (s, e) =>
            {
                int horizMargin = panelMain.Padding.Left + panelMain.Padding.Right;
                int vertMargin = panelMain.Padding.Top + panelMain.Padding.Bottom;

                card.Width = Math.Max(200, panelMain.Width - horizMargin);
                card.Height = Math.Max(200, panelMain.Height - vertMargin);
                card.Left = panelMain.Padding.Left;
                card.Top = panelMain.Padding.Top;
            };
            // Card panel (fills right-side area)
            card = new Panel(); 
            card.Dock = DockStyle.None;
            // initial card size based on panelMain padding
            int _horiz = panelMain.Padding.Left + panelMain.Padding.Right;
            int _vert = panelMain.Padding.Top + panelMain.Padding.Bottom;
            card.Width = Math.Max(200, panelMain.Width - _horiz);
            card.Height = Math.Max(200, panelMain.Height - _vert);
            card.Left = panelMain.Padding.Left;
            card.Top = panelMain.Padding.Top;
            card.BackColor = Color.White;

            // Recalculate rounded region when resized
            card.Resize += (s, e) => RoundPanel(card);

            // Initial rounding (will be recalculated on layout)
            RoundPanel(card);

            panelMain.Controls.Add(card);

            // ================= FORM =================
            this.Controls.Add(panelBody);
            this.Controls.Add(panelHeader);
            panelBody.Controls.Add(panelMain);
            panelBody.Controls.Add(panelSideMenu);

            this.Text = "Owner Dashboard";
            this.ResumeLayout(false);
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

        // ================= ROUNDED PANEL =================
        private void RoundPanel(Panel panel)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = 20;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            panel.Region = new Region(path);
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
