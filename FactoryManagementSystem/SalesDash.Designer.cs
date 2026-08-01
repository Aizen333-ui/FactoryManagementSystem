using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    partial class SalesDash
    {
        private System.ComponentModel.IContainer components = null;
        // Dashboard summary labels
        // Values are loaded from database in SalesDash.cs
        private Label lblTodaysSales;
        private Label lblTodaysRevenue;
        private Label lblOrdersToday;
        private Label lblLowStockProducts;
        // Recent sales listing grid

        private DataGridView dgvRecentSales;
        // Main vertical container for dashboard sections

        private FlowLayoutPanel mainFlow;

        private void MakeRounded(Control ctl, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);

            path.CloseAllFigures();
            ctl.Region = new Region(path);

            ctl.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(1, 1, ctl.Width - 3, ctl.Height - 3);

                using (GraphicsPath borderPath = new GraphicsPath())
                {
                    borderPath.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    borderPath.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    borderPath.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    borderPath.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                    borderPath.CloseAllFigures();

                    using (Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };

            ctl.Resize += (s, e) =>
            {
                GraphicsPath p = new GraphicsPath();

                p.AddArc(0, 0, radius, radius, 180, 90);
                p.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
                p.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
                p.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);

                p.CloseAllFigures();
                ctl.Region = new Region(p);
                ctl.Invalidate();
            };
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            mainFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(30, 20, 30, 20),
                BackColor = Color.White
            };

            this.Controls.Add(mainFlow);
            // ==============================
            // DASHBOARD HEADER
            // ==============================
            Label lblWelcome = new Label
            {
                Text = "Welcome to Sales Dashboard",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                AutoSize = true
            };

            Label lblSub = new Label
            {
                Text = "Front desk sales overview for today",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            mainFlow.Controls.Add(lblWelcome);
            mainFlow.Controls.Add(lblSub);
            // ==============================
            // SUMMARY CARDS ROW
            // ==============================
            FlowLayoutPanel topRow = new FlowLayoutPanel
            {
                Width = 1500,
                Height = 190,
                Margin = new Padding(0, 30, 0, 20)
            };
            // Today's sales card

            Panel cardTodaysSales = CreateCard();
            cardTodaysSales.Controls.Add(CreateCardTitle("Today's Sales"));
            lblTodaysSales = CreateBigValueLabel();
            cardTodaysSales.Controls.Add(lblTodaysSales);
            topRow.Controls.Add(cardTodaysSales);
            // Today's revenue card

            Panel cardTodaysRevenue = CreateCard();
            cardTodaysRevenue.Controls.Add(CreateCardTitle("Today's Revenue"));
            lblTodaysRevenue = CreateBigValueLabel();
            cardTodaysRevenue.Controls.Add(lblTodaysRevenue);
            topRow.Controls.Add(cardTodaysRevenue);
            // Orders count card

            Panel cardOrdersToday = CreateCard();
            cardOrdersToday.Controls.Add(CreateCardTitle("Orders Today"));
            lblOrdersToday = CreateBigValueLabel();
            cardOrdersToday.Controls.Add(lblOrdersToday);
            topRow.Controls.Add(cardOrdersToday);
            // Low stock warning card

            Panel cardLowStock = CreateCard();
            cardLowStock.Controls.Add(CreateCardTitle("Low Stock Products"));
            lblLowStockProducts = CreateBigValueLabel();
            cardLowStock.Controls.Add(lblLowStockProducts);
            topRow.Controls.Add(cardLowStock);

            mainFlow.Controls.Add(topRow);

            Panel recentPanel = CreateLargePanel();
            recentPanel.Size = new Size(1450, 420);
            recentPanel.Padding = new Padding(25, 60, 25, 25);
            recentPanel.Margin = new Padding(20, 10, 20, 30);

            Label recentTitle = new Label
            {
                Text = "Recent Sales",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, 20),
                ForeColor = Color.Black
            };

            dgvRecentSales = new DataGridView
            {
                Dock = DockStyle.Bottom,
                Height = 300,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            recentPanel.Controls.Add(recentTitle);
            recentPanel.Controls.Add(dgvRecentSales);

            mainFlow.Controls.Add(recentPanel);
        }

        private Panel CreateCard()
        {
            Panel panel = new Panel
            {
                Size = new Size(340, 170),
                BackColor = Color.White,
                Margin = new Padding(10),
                Padding = new Padding(10)
            };

            MakeRounded(panel, 16);
            return panel;
        }

        private Panel CreateLargePanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1400, 300),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            MakeRounded(panel, 16);
            return panel;
        }

        private Label CreateCardTitle(string text)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.Gray,
                Location = new Point(20, 20),
                AutoSize = true
            };
        }

        private Label CreateBigValueLabel()
        {
            return new Label
            {
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(20, 70),
                Size = new Size(290, 60),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };
        }
    }
}
