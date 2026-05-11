using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    partial class FactoryDash
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== LABELS =====================
        private Label lblTotalProduction;
        private Label lblRawUsagePercent;

        // ===================== FLOW =====================
        private FlowLayoutPanel mainFlow;

        // ===================== CARDS =====================
        private Panel cardProduction;
        private Panel cardRawUsage;

        // ===================== PANELS =====================
        private Panel productionPanel;

        // ===================== CHARTS =====================
        private Chart pieChart;
        private Chart barChart;

        // =====================================================
        // ROUND CORNERS (SAME AS OWNER DASH)
        // =====================================================
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
        }

        // =====================================================
        // INIT
        // =====================================================
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // ================= MAIN FLOW =================
            mainFlow = new FlowLayoutPanel();
            mainFlow.Dock = DockStyle.Fill;
            mainFlow.FlowDirection = FlowDirection.TopDown;
            mainFlow.WrapContents = false;
            mainFlow.AutoScroll = true;
            mainFlow.Padding = new Padding(30, 20, 30, 20);
            mainFlow.BackColor = Color.White;

            this.Controls.Add(mainFlow);

            // ================= TITLE =================
            Label title = new Label();
            title.Text = "Factory Manager Dashboard";
            title.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            title.AutoSize = true;

            Label sub = new Label();
            sub.Text = "Production & Raw Material Analytics";
            sub.Font = new Font("Segoe UI", 11F);
            sub.ForeColor = Color.Gray;
            sub.AutoSize = true;

            mainFlow.Controls.Add(title);
            mainFlow.Controls.Add(sub);

            // ================= KPI ROW =================
            FlowLayoutPanel row = new FlowLayoutPanel();
            row.Width = 1500;
            row.Height = 180;
            row.Margin = new Padding(0, 20, 0, 20);

            // -------- PRODUCTION CARD --------
            cardProduction = CreateCard();

            Label pTitle = CreateCardTitle("Total Production");
            lblTotalProduction = CreateBigLabel();

            cardProduction.Controls.Add(pTitle);
            cardProduction.Controls.Add(lblTotalProduction);

            row.Controls.Add(cardProduction);

            // -------- RAW USAGE CARD --------
            cardRawUsage = CreateCard();

            Label rTitle = CreateCardTitle("Raw Material Used %");
            lblRawUsagePercent = CreateBigLabel();

            cardRawUsage.Controls.Add(rTitle);
            cardRawUsage.Controls.Add(lblRawUsagePercent);

            row.Controls.Add(cardRawUsage);

            mainFlow.Controls.Add(row);

            // ================= PRODUCTION PANEL =================
            productionPanel = CreateLargePanel();

            Label prodTitle = new Label();
            prodTitle.Text = "Record Production";
            prodTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            prodTitle.AutoSize = true;

            productionPanel.Controls.Add(prodTitle);

            mainFlow.Controls.Add(productionPanel);

            // ================= CHARTS ROW (FIXED LIKE OWNER DASH) =================
            TableLayoutPanel chartsRow = new TableLayoutPanel();
            chartsRow.Width = 1500;
            chartsRow.Height = 480;
            chartsRow.ColumnCount = 2;
            chartsRow.RowCount = 1;

            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            chartsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            chartsRow.Padding = new Padding(10);
            chartsRow.BackColor = Color.White;

            // PIE
            pieChart = new Chart();
            Panel pieHost = CreateChartHost("Material Usage", pieChart);

            // BAR
            barChart = new Chart();
            Panel barHost = CreateChartHost("Monthly Production", barChart);

            chartsRow.Controls.Add(pieHost, 0, 0);
            chartsRow.Controls.Add(barHost, 1, 0);

            mainFlow.Controls.Add(chartsRow);

            // ROUND EVERYTHING
            MakeRounded(cardProduction, 16);
            MakeRounded(cardRawUsage, 16);
            MakeRounded(productionPanel, 16);
        }

        // =====================================================
        // CARD
        // =====================================================
        private Panel CreateCard()
        {
            Panel p = new Panel();
            p.Size = new Size(400, 150);
            p.BackColor = Color.White;
            p.Margin = new Padding(10);
            MakeRounded(p, 14);
            return p;
        }

        private Panel CreateLargePanel()
        {
            Panel p = new Panel();
            p.Size = new Size(1400, 350);
            p.BackColor = Color.White;
            p.Margin = new Padding(10);
            MakeRounded(p, 14);
            return p;
        }

        private Label CreateCardTitle(string text)
        {
            return new Label()
            {
                Text = text,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.Gray,
                Location = new Point(20, 20),
                AutoSize = true
            };
        }

        private Label CreateBigLabel()
        {
            return new Label()
            {
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                Location = new Point(20, 60),
                AutoSize = true
            };
        }

        // =====================================================
        // CHART HOST (FIXED LIKE OWNER DASH)
        // =====================================================
        private Panel CreateChartHost(string title, Chart chart)
        {
            Panel outer = new Panel();
            outer.Dock = DockStyle.Fill;
            outer.BackColor = Color.White;

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.RowCount = 2;

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Label lbl = new Label();
            lbl.Text = title;
            lbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lbl.Dock = DockStyle.Fill;

            chart.Dock = DockStyle.Fill;

            layout.Controls.Add(lbl, 0, 0);
            layout.Controls.Add(chart, 0, 1);

            outer.Controls.Add(layout);

            MakeRounded(outer, 16);

            return outer;
        }
    }
}