using System.Drawing.Drawing2D;

namespace FactoryManagementAdminTool
{
    partial class AdminDash
    {
        private Label lblTitle;
        private Label lblSub;
        private Panel userCard;
        private Panel adminCard;
        private Panel activityCard;
        private Panel systemadminCard;
        private Label lblTotalUsers;
        private Label lblActiveUsers;
        private Label lblDisabledUsers;
        private Label lblSystemAdmin;
        private Button btnRefresh;
        private void CenterControlHorizontally(Control control, Control parent, int y)
        {
            control.Location = new Point(
                (parent.ClientSize.Width - control.Width) / 2,
                y);
        }
        private void InitializeComponent()
        {
            SuspendLayout();

            Dock = DockStyle.Fill;
            BackColor = Color.White;

            lblTitle = new Label
            {
                Text = "Admin Overview",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(40, 20)
            };

            lblSub = new Label
            {
                Text = "Monitor users and jump into management tools.",
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(42, 80)
            };

            userCard = CreateCard("👥 Total Users", "All registered users");
            adminCard = CreateCard("🟢 Active Users", "Users currently enabled");
            activityCard = CreateCard("⚠ Disabled Users", "Users currently disabled");
            systemadminCard = CreateCard("👑 System Admins","Administrator accounts");
            userCard.Location = new Point(50, 170);
            adminCard.Location = new Point(430, 170);

            activityCard.Location = new Point(50, 410);
            systemadminCard.Location = new Point(430, 410);

            lblTotalUsers = MakeCountLabel(Color.FromArgb(37, 99, 235));
            lblActiveUsers = MakeCountLabel(Color.FromArgb(22, 163, 74));
            lblDisabledUsers = MakeCountLabel(Color.FromArgb(220, 38, 38));
            lblSystemAdmin = MakeCountLabel(Color.MediumPurple);
            userCard.Controls.Add(lblTotalUsers);
            CenterControlHorizontally(lblTotalUsers, userCard, 60);

            adminCard.Controls.Add(lblActiveUsers);
            CenterControlHorizontally(lblActiveUsers, adminCard, 60);

            activityCard.Controls.Add(lblDisabledUsers);
            CenterControlHorizontally(lblDisabledUsers, activityCard, 60);

            systemadminCard.Controls.Add(lblSystemAdmin);
            CenterControlHorizontally(lblSystemAdmin, systemadminCard, 60);
            btnRefresh = new Button
            {
                Text = "Refresh",
                Size = new Size(150, 50),
                Location = new Point(50, 650),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            RoundButton(btnRefresh);
            btnRefresh.Click += btnRefresh_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblSub);
            Controls.Add(userCard);
            Controls.Add(adminCard);
            Controls.Add(activityCard);
            Controls.Add(systemadminCard);
            Controls.Add(btnRefresh);

            ResumeLayout();
            PerformLayout();
        }

        private Label MakeCountLabel(Color color)
        {
            return new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 32F),
                ForeColor = color,
                AutoSize = true
            };
        }

        private Panel CreateCard(string title, string text)
        {
            Panel p = new Panel
            {
                Size = new Size(330, 180),
                BackColor = Color.FromArgb(248, 250, 252)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 16),
                AutoSize = true
            };

            Label lblDescription = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            p.Controls.Add(lblTitle);
            p.Controls.Add(lblDescription);

            p.Resize += (s, e) =>
            {
                CenterControlHorizontally(lblTitle, p, 18);
                CenterControlHorizontally(lblDescription, p, 145);

                RoundPanel(p);
            };

            // Initial positioning
            CenterControlHorizontally(lblTitle, p, 18);
            CenterControlHorizontally(lblDescription, p, 145);

            RoundPanel(p);

            return p;
        }

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

        private void RoundButton(Button button)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = 18;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            button.Region = new Region(path);
        }
    }
}
