// ======================================================
// FactoryDash.Designer.cs
// ======================================================

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    partial class FactoryDash
    {
        private System.ComponentModel.IContainer components = null;

        // ======================================================
        // LABELS
        // ======================================================

        private Label lblTotalProduction;
        private Label lblRawUsagePercent;

        // ======================================================
        // CHARTS
        // ======================================================

        private Chart pieChart;
        private Chart barChart;

        // ======================================================
        // PANELS
        // ======================================================

        private FlowLayoutPanel mainFlow;

        private Panel cardProduction;
        private Panel cardRawUsage;
        private FlowLayoutPanel productionFlow;
        private Panel productionPanel;

        // ======================================================
        // ROUND PANEL
        // ======================================================

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

                Rectangle rect = new Rectangle(
                    1,
                    1,
                    ctl.Width - 3,
                    ctl.Height - 3);

                using (GraphicsPath borderPath = new GraphicsPath())
                {
                    borderPath.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    borderPath.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    borderPath.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    borderPath.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

                    borderPath.CloseAllFigures();

                    using (Pen pen = new Pen(
                        Color.FromArgb(180, 190, 210),
                        1.5f))
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

        // ======================================================
        // INITIALIZE
        // ======================================================

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            // ======================================================
            // MAIN FLOW
            // ======================================================

            mainFlow = new FlowLayoutPanel();

            mainFlow.Dock = DockStyle.Fill;
            mainFlow.FlowDirection = FlowDirection.TopDown;
            mainFlow.WrapContents = false;
            mainFlow.AutoScroll = true;
            mainFlow.Padding = new Padding(30, 20, 30, 20);
            mainFlow.BackColor = Color.White;

            this.Controls.Add(mainFlow);

            // ======================================================
            // TITLE
            // ======================================================

            Label lblTitle = new Label();

            lblTitle.Text = "Factory Manager Dashboard";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.AutoSize = true;

            Label lblSub = new Label();

            lblSub.Text = "Production and Raw Material Overview";
            lblSub.Font = new Font("Segoe UI", 11F);
            lblSub.ForeColor = Color.Gray;
            lblSub.AutoSize = true;

            mainFlow.Controls.Add(lblTitle);
            mainFlow.Controls.Add(lblSub);

            // ======================================================
            // KPI ROW
            // ======================================================

            FlowLayoutPanel topRow = new FlowLayoutPanel();

            topRow.Width = 1500;
            topRow.Height = 190;
            topRow.Margin = new Padding(0, 30, 0, 20);

            // ======================================================
            // TOTAL PRODUCTION CARD
            // ======================================================

            cardProduction = CreateCard();

            Label titleProduction = CreateCardTitle(
                "Total Production");

            lblTotalProduction = CreateBigValueLabel();

            cardProduction.Controls.Add(titleProduction);
            cardProduction.Controls.Add(lblTotalProduction);

            topRow.Controls.Add(cardProduction);

            // ======================================================
            // RAW MATERIAL CARD
            // ======================================================

            cardRawUsage = CreateCard();

            Label titleRaw = CreateCardTitle(
                "Raw Material Used %");

            lblRawUsagePercent = CreateBigValueLabel();

            cardRawUsage.Controls.Add(titleRaw);
            cardRawUsage.Controls.Add(lblRawUsagePercent);

            topRow.Controls.Add(cardRawUsage);

            mainFlow.Controls.Add(topRow);

            // ======================================================
            // PRODUCTION PANEL
            // ======================================================

            productionPanel = CreateLargePanel();

            productionPanel.Size = new Size(1450, 320);
            productionPanel.Padding = new Padding(25, 60, 25, 25); // 🔥 space for title
            productionPanel.Margin = new Padding(20, 10, 20, 30);

            // TITLE
            Label prodTitle = new Label();
            prodTitle.Text = "Production Overview";
            prodTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            prodTitle.AutoSize = true;
            prodTitle.Location = new Point(25, 20); // 🔥 FIXED POSITION
            prodTitle.ForeColor = Color.Black;

            // FLOW (IMPORTANT)
            productionFlow = new FlowLayoutPanel();
            productionFlow.Dock = DockStyle.Bottom;
            productionFlow.Height = 220;
            productionFlow.Padding = new Padding(10);
            productionFlow.WrapContents = true;
            productionFlow.AutoScroll = true;

            productionPanel.Controls.Add(prodTitle);
            productionPanel.Controls.Add(productionFlow);

            mainFlow.Controls.Add(productionPanel);

            // ======================================================
            // CHARTS ROW
            // ======================================================

            TableLayoutPanel chartsRow = new TableLayoutPanel();

            chartsRow.Width = 1500;
            chartsRow.Height = 480;
            chartsRow.Margin = new Padding(0, 12, 0, 24);

            chartsRow.ColumnCount = 2;
            chartsRow.RowCount = 1;

            chartsRow.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50F));

            chartsRow.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 50F));

            chartsRow.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            chartsRow.BackColor = Color.White;
            chartsRow.Padding = new Padding(10);

            // PIE CHART

            pieChart = new Chart();

            Panel pieHost =
                CreateChartHost("Material Usage", pieChart);

            // BAR CHART

            barChart = new Chart();

            Panel barHost =
                CreateChartHost("Monthly Production", barChart);

            chartsRow.Controls.Add(pieHost, 0, 0);
            chartsRow.Controls.Add(barHost, 1, 0);

            mainFlow.Controls.Add(chartsRow);

            // ======================================================
            // ROUND
            // ======================================================

            MakeRounded(cardProduction, 16);
            MakeRounded(cardRawUsage, 16);
            MakeRounded(productionPanel, 16);

            this.ResumeLayout(false);
        }

        // ======================================================
        // CARD
        // ======================================================

        private Panel CreateCard()
        {
            Panel panel = new Panel();

            panel.Size = new Size(450, 170);
            panel.BackColor = Color.White;
            panel.Margin = new Padding(10);
            panel.Padding = new Padding(10);

            MakeRounded(panel, 16);

            return panel;
        }

        private Panel CreateLargePanel()
        {
            Panel panel = new Panel();

            panel.Size = new Size(1400, 300);
            panel.BackColor = Color.White;
            panel.Padding = new Padding(20);

            MakeRounded(panel, 16);

            return panel;
        }

        // ======================================================
        // CHART HOST
        // ======================================================
        private void ApplyRoundedRegion(Control ctl, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);

            path.CloseAllFigures();

            ctl.Region = new Region(path);
            ctl.Invalidate();
        }
        private Panel CreateChartHost(string title, Chart chart)
        {
            Panel outer = new Panel();

            outer.Dock = DockStyle.Fill;
            outer.BackColor = Color.White;
            outer.Margin = new Padding(10, 8, 10, 16);
            outer.Padding = new Padding(2);

            // IMPORTANT
            outer.Resize += (s, e) =>
            {
                ApplyRoundedRegion(outer, 18);
            };

            ApplyRoundedRegion(outer, 18);
            // BORDER
            outer.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                int radius = 18;

                Rectangle rect = new Rectangle(
                    1,
                    1,
                    outer.Width - 3,
                    outer.Height - 3);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius,
                                rect.Bottom - radius,
                                radius,
                                radius,
                                0,
                                90);

                    path.AddArc(rect.X,
                                rect.Bottom - radius,
                                radius,
                                radius,
                                90,
                                90);

                    path.CloseAllFigures();

                    using (Pen pen =
                        new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            TableLayoutPanel layout = new TableLayoutPanel();

            layout.Dock = DockStyle.Fill;
            layout.RowCount = 2;
            layout.ColumnCount = 1;

            layout.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 50F));

            layout.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            Label hdr = new Label();

            hdr.Text = title;
            hdr.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            hdr.Dock = DockStyle.Fill;
            hdr.TextAlign = ContentAlignment.MiddleLeft;
            hdr.Padding = new Padding(15, 0, 0, 0);

            chart.Dock = DockStyle.Fill;
            chart.Margin = new Padding(10);
            chart.BackColor = Color.White;

            layout.Controls.Add(hdr, 0, 0);
            layout.Controls.Add(chart, 0, 1);

            outer.Controls.Add(layout);

            return outer;
        }

        // ======================================================
        // LABEL HELPERS
        // ======================================================

        private Label CreateCardTitle(string text)
        {
            Label lbl = new Label();

            lbl.Text = text;
            lbl.Font =
                new Font("Segoe UI", 12F, FontStyle.Bold);

            lbl.ForeColor = Color.Gray;
            lbl.Location = new Point(20, 20);
            lbl.AutoSize = true;

            return lbl;
        }

        private Label CreateBigValueLabel()
        {
            Label lbl = new Label();

            lbl.Font =
                new Font("Segoe UI", 28F, FontStyle.Bold);

            lbl.ForeColor = Color.Black;
            lbl.Location = new Point(20, 70);
            lbl.AutoSize = true;

            return lbl;
        }
    }
}