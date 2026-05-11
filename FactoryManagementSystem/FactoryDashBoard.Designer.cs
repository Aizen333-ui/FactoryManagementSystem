using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    // Main dashboard form for factory management system UI
    public partial class FactoryDashBoard : Form
    {
        Button activeButton = null; 
        Panel card;                 // main content card area
        Panel panelBody;            // container panel
        Panel panelSideMenu, panelHeader, panelMain;
        Label lblTitle;
        Button btnRecord, btnRaw, btnReport, btnLogout;

        private void InitializeComponent()
        {
            // ================= MAIN CONTAINER =================
            panelBody = new Panel();
            panelBody.Dock = DockStyle.Fill;

            // initialize main UI sections
            this.panelSideMenu = new Panel();
            this.panelHeader = new Panel();
            this.panelMain = new Panel();
            this.lblTitle = new Label();

            // initialize buttons
            this.panelSideMenu.SuspendLayout();
            this.btnRecord = new Button();
            this.btnRaw = new Button();
            this.btnReport = new Button();
            this.btnLogout = new Button();

            this.SuspendLayout();

            // ================= SIDEBAR =================
            panelSideMenu.Dock = DockStyle.Left;
            panelSideMenu.Width = 450;

            // paint event for gradient sidebar background
            panelSideMenu.Paint += PanelSideMenu_Paint;

            panelHeader.Dock = DockStyle.Top;
            panelMain.Dock = DockStyle.Fill;

            // group main action buttons
            Button[] buttons = { btnRecord, btnRaw, btnReport };

            int top = 100;

            // configure sidebar buttons
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

                // hover effects
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 120, 255);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 220);

                panelSideMenu.Controls.Add(btn);
                top += 70;
            }

            // button labels
            btnRecord.Text = "📊 Record Production";
            btnRaw.Text = "📦 Raw Material Usage";
            btnReport.Text = "📄 Report to Owner";
            btnRecord.Tag = "nav";
            btnRaw.Tag = "nav";
            btnReport.Tag = "nav";
            // attach click events
            btnRecord.Click += btnRecord_Click;
            btnRaw.Click += btnRaw_Click;
            btnReport.Click += btnReport_Click;

            // ================= LOGOUT BUTTON =================
            btnLogout.Text = "⏻ Logout";
            btnLogout.Width = panelSideMenu.Width - 40;
            btnLogout.Height = 60;
            btnLogout.Left = 10;

            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;

            btnLogout.ForeColor = Color.White;
            btnLogout.BackColor = Color.FromArgb(220, 38, 38);
            btnLogout.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Bold);

            // keep logout button at bottom
            btnLogout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

            // reposition on resize
            panelSideMenu.Resize += (s, e) =>
            {
                btnLogout.Top = panelSideMenu.Height - btnLogout.Height - 20;
                btnLogout.Left = 20;
            };

            // logout event
            btnLogout.Click += btnLogout_Click;

            // round logout button corners
            RoundButton(btnLogout);

            panelSideMenu.Controls.Add(btnLogout);

            // ================= HEADER =================
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 120;

            // gradient header background
            panelHeader.Paint += PanelHeader_Paint;

            lblTitle.Text = "Factory Dashboard";
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.Location = new Point(30, 20);
            lblTitle.AutoSize = true;

            panelHeader.Controls.Add(lblTitle);

            // ================= MAIN CONTENT AREA =================
            panelMain.Dock = DockStyle.Fill;
            panelMain.BackColor = Color.FromArgb(243, 244, 246);

            // padding to create spacing for card layout
            panelMain.Padding = new Padding(60, 40, 450, 40);

            // resize logic for card
            panelMain.Resize += (s, e) =>
            {
                int horizMargin = panelMain.Padding.Left + panelMain.Padding.Right;
                int vertMargin = panelMain.Padding.Top + panelMain.Padding.Bottom;

                card.Width = Math.Max(200, panelMain.Width - horizMargin);
                card.Height = Math.Max(200, panelMain.Height - vertMargin);

                card.Left = panelMain.Padding.Left;
                card.Top = panelMain.Padding.Top;
            };

            // ================= CARD PANEL =================
            card = new Panel();
            card.BackColor = Color.White;

            // initial sizing
            int horiz = panelMain.Padding.Left + panelMain.Padding.Right;
            int vert = panelMain.Padding.Top + panelMain.Padding.Bottom;

            card.Width = Math.Max(200, panelMain.Width - horiz);
            card.Height = Math.Max(200, panelMain.Height - vert);

            card.Left = panelMain.Padding.Left;
            card.Top = panelMain.Padding.Top;

            // rounded corners
            card.Resize += (s, e) => RoundPanel(card);
            RoundPanel(card);

            panelMain.Controls.Add(card);

            // ================= FORM STRUCTURE =================
            this.Controls.Add(panelBody);
            this.Controls.Add(panelHeader);

            panelBody.Controls.Add(panelMain);
            panelBody.Controls.Add(panelSideMenu);

            this.Text = "Factory Dashboard";

            this.ResumeLayout(false);
        }

        // ================= SIDEBAR GRADIENT =================
        private void PanelSideMenu_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                panelSideMenu.ClientRectangle,
                Color.FromArgb(67, 56, 202),
                Color.FromArgb(99, 102, 241),
                90F))
            {
                e.Graphics.FillRectangle(brush, panelSideMenu.ClientRectangle);
            }
        }

        // highlight selected button
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

        // ================= HEADER GRADIENT =================
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
