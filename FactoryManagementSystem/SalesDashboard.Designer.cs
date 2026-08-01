using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    public partial class SalesDashboard : Form
    {
        // Currently selected sidebar navigation button

        Button activeButton = null;
        // Main content container where pages are loaded

        Panel card;
        // Main layout containers

        Panel panelBody;
        Panel panelSideMenu, panelHeader, panelMain;
        // Header title

        Label lblTitle;
        // Sidebar navigation buttons

        Button btnNewSale, btnProducts, btnCustomers;
        Button btnSalesHistory, btnReturns,  btnLogout;

        // ==================================================
        // INITIALIZE SALES DASHBOARD UI
        // ==================================================
        // Creates:
        //
        // - Sidebar navigation
        // - Header area
        // - Main content card
        // - Navigation buttons
        // - Logout button
        //
        // Actual page loading is handled in SalesDashboard.cs
        // ==================================================
        private void InitializeComponent()
        {
            panelBody = new Panel();
            panelBody.Dock = DockStyle.Fill;

            this.panelSideMenu = new Panel();
            this.panelHeader = new Panel();
            this.panelMain = new Panel();
            this.lblTitle = new Label();

            this.panelSideMenu.SuspendLayout();
            this.btnNewSale = new Button();
            this.btnProducts = new Button();
            this.btnCustomers = new Button();
            this.btnSalesHistory = new Button();
            this.btnReturns = new Button();
            this.btnLogout = new Button();

            this.SuspendLayout();
            // ==============================
            // SIDEBAR MENU
            // ==============================
            panelSideMenu.Dock = DockStyle.Left;
            panelSideMenu.Width = 450;
            panelSideMenu.Paint += PanelSideMenu_Paint;

            panelHeader.Dock = DockStyle.Top;
            panelMain.Dock = DockStyle.Fill;

            Button[] buttons =
            {
                 btnNewSale,  btnCustomers,btnProducts,
                btnSalesHistory, btnReturns
            };

            int top = 80;

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
                btn.Tag = "nav";

                panelSideMenu.Controls.Add(btn);
                top += 65;
            }

            btnNewSale.Text = "🛒 New Sale";
            btnCustomers.Text = "👥 Customers";
            btnProducts.Text = "📦 Products";
            btnSalesHistory.Text = "📋 Sales History";
            btnReturns.Text = "↩ Returns";
            // Navigation events

            btnNewSale.Click += btnNewSale_Click;
            btnProducts.Click += btnProducts_Click;
            btnCustomers.Click += btnCustomers_Click;
            btnSalesHistory.Click += btnSalesHistory_Click;
            btnReturns.Click += btnReturns_Click;
            // ==============================
            // LOGOUT BUTTON
            // ==============================
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

            btnLogout.Click += btnLogout_Click;
            RoundButton(btnLogout);
            panelSideMenu.Controls.Add(btnLogout);
            // ==============================
            // HEADER
            // ==============================

            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 120;
            panelHeader.Paint += PanelHeader_Paint;

            lblTitle.Text = "Sales Dashboard";
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblTitle.Location = new Point(30, 20);
            lblTitle.AutoSize = true;

            panelHeader.Controls.Add(lblTitle);
            // ==============================
            // MAIN CONTENT AREA
            // ==============================

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

            card = new Panel();
            card.BackColor = Color.White;

            int horiz = panelMain.Padding.Left + panelMain.Padding.Right;
            int vert = panelMain.Padding.Top + panelMain.Padding.Bottom;

            card.Width = Math.Max(200, panelMain.Width - horiz);
            card.Height = Math.Max(200, panelMain.Height - vert);
            card.Left = panelMain.Padding.Left;
            card.Top = panelMain.Padding.Top;

            card.Resize += (s, e) => RoundPanel(card);
            RoundPanel(card);

            panelMain.Controls.Add(card);

            this.Controls.Add(panelBody);
            this.Controls.Add(panelHeader);

            panelBody.Controls.Add(panelMain);
            panelBody.Controls.Add(panelSideMenu);

            this.Text = "Sales Dashboard";

            this.ResumeLayout(false);
        }

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

        private void SetActiveButton(Button btn)
        {
            if (activeButton != null)
            {
                activeButton.BackColor = Color.Transparent;
                activeButton.ForeColor = Color.White;
            }

            activeButton = btn;
            activeButton.BackColor = Color.White;
            activeButton.ForeColor = Color.Black;
        }

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
