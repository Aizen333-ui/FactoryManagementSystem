using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    // Main dashboard UI for factory management system
    partial class FactoryDash
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== KPI LABELS =====================
        private Label lblTotalProduction;
        private Label lblRawUsagePercent;

        // ===================== CHARTS =====================
        private Chart pieChart;
        private Chart barChart;

        // ===================== MAIN CONTAINERS =====================
        private FlowLayoutPanel mainFlow;

        private Panel cardProduction;
        private Panel cardRawUsage;
        private FlowLayoutPanel productionFlow;
        private Panel productionPanel;

        // Applies rounded corners + custom border drawing to a control
        private void MakeRounded(Control ctl, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            // Create rounded rectangle region
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);

            path.CloseAllFigures();
            ctl.Region = new Region(path);

            // Custom border rendering
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

            // Reapply rounding when resized
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

        // ===================== UI INITIALIZATION =====================
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // Main scrollable layout container
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

            // ===================== DASHBOARD HEADER =====================
            Label lblTitle = new Label
            {
                Text = "Factory Manager Dashboard",
                Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                AutoSize = true
            };

            Label lblSub = new Label
            {
                Text = "Production and Raw Material Overview",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                AutoSize = true
            };

            mainFlow.Controls.Add(lblTitle);
            mainFlow.Controls.Add(lblSub);

            // ===================== KPI ROW =====================
            FlowLayoutPanel topRow = new FlowLayoutPanel
            {
                Width = 1500,
                Height = 190,
                Margin = new Padding(0, 30, 0, 20)
            };

            // ---------- Production KPI Card ----------
            cardProduction = CreateCard();

            Label titleProduction = CreateCardTitle("Total Production");
            lblTotalProduction = CreateBigValueLabel();

            cardProduction.Controls.Add(titleProduction);
            cardProduction.Controls.Add(lblTotalProduction);

            topRow.Controls.Add(cardProduction);

            // ---------- Raw Material KPI Card ----------
            cardRawUsage = CreateCard();

            Label titleRaw = CreateCardTitle("Raw Material Used %");
            lblRawUsagePercent = CreateBigValueLabel();

            cardRawUsage.Controls.Add(titleRaw);
            cardRawUsage.Controls.Add(lblRawUsagePercent);

            topRow.Controls.Add(cardRawUsage);

            mainFlow.Controls.Add(topRow);

            // ===================== PRODUCTION SECTION =====================
            productionPanel = CreateLargePanel();
            productionPanel.Size = new Size(1450, 320);
            productionPanel.Padding = new Padding(25, 60, 25, 25);
            productionPanel.Margin = new Padding(20, 10, 20, 30);

            Label prodTitle = new Label
            {
                Text = "Production Overview",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(25, 20),
                ForeColor = Color.Black
            };

            productionFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 220,
                Padding = new Padding(10),
                WrapContents = true,
                AutoScroll = true
            };

            productionPanel.Controls.Add(prodTitle);
            productionPanel.Controls.Add(productionFlow);

            mainFlow.Controls.Add(productionPanel);

            // ===================== CHART SECTION =====================

            TableLayoutPanel chartsRow = new TableLayoutPanel
            {
                Width = 1500,
                Height = 480,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.White,
                Padding = new Padding(10),
                Margin = new Padding(0,20,0,20)
            };

            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,48));
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,2));
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,48));
            chartsRow.RowStyles.Add(new RowStyle(SizeType.Percent,100));

            // ================= LEFT CHART =================

            pieChart = new Chart();
            Panel pieHost =CreateChartHost("Material Usage",pieChart);

            // ================= RIGHT CHART =================

            barChart = new Chart();
            Panel barHost =CreateChartHost("Monthly Production",barChart);

            // ================= DIVIDER LINE =================

            Panel divider = new Panel(){
                Width = 2,
                Dock = DockStyle.Fill,
                BackColor =Color.FromArgb(180,190,210),
                Margin =new Padding(0,20,0,20)
            };

            // ADD CONTROLS

            chartsRow.Controls.Add(pieHost,0,0);
            chartsRow.Controls.Add(divider,1,0);
            chartsRow.Controls.Add(barHost,2,0);
            mainFlow.Controls.Add(chartsRow);
        }

        // ===================== BASIC CARD =====================
        private Panel CreateCard()
        {
            Panel panel = new Panel
            {
                Size = new Size(450, 170),
                BackColor = Color.White,
                Margin = new Padding(10),
                Padding = new Padding(10)
            };

            MakeRounded(panel, 16);
            return panel;
        }

        // Large container panel for dashboard sections
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

        // ===================== CHART CONTAINER =====================

        private Panel CreateChartHost(
            string title,
            Chart chart)
        {

            Panel outer = new Panel()
            {
                Dock =DockStyle.Fill,
                BackColor =Color.White,
                Padding =new Padding(5)
            };

            TableLayoutPanel layout =new TableLayoutPanel(){
                    Dock =DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1,
                    BackColor =
                    Color.White
                };

                layout.RowStyles.Add(new RowStyle(SizeType.Absolute,50));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent,100));

                Label header =new Label(){
                    Text = title,
                    Font =new Font("Segoe UI",14,FontStyle.Bold),
                    Dock =DockStyle.Fill,
                    TextAlign =ContentAlignment.MiddleLeft,
                    Padding =new Padding(15,0,0,0)
                };

                chart.Dock =DockStyle.Fill;
                chart.BackColor =Color.White;
                layout.Controls.Add(header,0,0);
                layout.Controls.Add(chart,0,1);
                outer.Controls.Add(layout);
          return outer;
        }

        // ===================== LABEL HELPERS =====================
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
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(20, 70),
                AutoSize = true
            };
        }
    }
}