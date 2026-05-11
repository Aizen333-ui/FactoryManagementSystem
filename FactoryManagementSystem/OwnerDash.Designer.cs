using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    // Owner dashboard UI showing KPIs, worker stats, materials, and charts
    partial class OwnerDash
    {
        private System.ComponentModel.IContainer components = null;

        // KPI LABELS (Top summary stats)
        private Label lblWorkers;
        private Label lblExpenses;
        private Label lblProduction;

        // WORKER CATEGORY LABELS
        private Label lblLabourers;
        private Label lblDrivers;
        private Label lblLoaders;
        private Label lblOperators;

        // MATERIAL LABELS
        private Label lblCement;
        private Label lblSand;
        private Label lblCrush;
        private Label lblSteel;
        private Label lblOil;

        // CHARTS (Visual analytics section)
        private Chart pieChart;
        private Chart barChart;

        // MAIN CONTAINERS
        private FlowLayoutPanel mainFlow;

        private Panel cardWorkers;
        private Panel cardExpenses;
        private Panel cardProduction;

        private Panel workersCategoryPanel;
        private Panel materialsPanel;

        // Applies rounded corners + border styling to any UI control
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

            // Custom border drawing
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

            // Recalculate rounded region on resize
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

        // INITIALIZE UI LAYOUT
        private void InitializeComponent()
        {
            mainFlow = new FlowLayoutPanel();

            lblTitle = new Label();
            lblSub = new Label();
            topRow = new FlowLayoutPanel();
            chartsRow = new TableLayoutPanel();

            pieChart = new Chart();
            barChart = new Chart();

            wrapper = new FlowLayoutPanel();
            workerTitle = new Label();
            workerGrid = new FlowLayoutPanel();

            wrapper1 = new FlowLayoutPanel();
            materialTitle = new Label();
            materialGrid = new FlowLayoutPanel();

            mainFlow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pieChart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barChart).BeginInit();
            wrapper.SuspendLayout();
            wrapper1.SuspendLayout();
            SuspendLayout();

            // ===================== MAIN LAYOUT =====================
            mainFlow.AutoScroll = true;
            mainFlow.BackColor = Color.White;
            mainFlow.Dock = DockStyle.Fill;
            mainFlow.FlowDirection = FlowDirection.TopDown;
            mainFlow.Padding = new Padding(30, 20, 30, 20);
            mainFlow.WrapContents = false;

            mainFlow.Controls.Add(lblTitle);
            mainFlow.Controls.Add(lblSub);
            mainFlow.Controls.Add(topRow);
            mainFlow.Controls.Add(chartsRow);

            // ===================== HEADER =====================
            lblTitle.Text = "Factory Stats";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Black;

            lblSub.Text = "Real-time overview of factory operations";
            lblSub.Font = new Font("Segoe UI", 11F);
            lblSub.ForeColor = Color.Gray;

            // ===================== KPI ROW =====================
            topRow.Margin = new Padding(0, 30, 0, 20);
            topRow.Size = new Size(1500, 190);

            // ===================== CHART ROW =====================
            chartsRow.BackColor = Color.White;
            chartsRow.ColumnCount = 2;
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            chartsRow.RowCount = 1;
            chartsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            chartsRow.Padding = new Padding(10);
            chartsRow.Dock = DockStyle.Fill;

            // Charts initialization (data loaded in backend file)
            pieChart.Size = new Size(300, 300);
            barChart.Size = new Size(300, 300);

            // ===================== WORKER SECTION WRAPPER =====================
            wrapper.FlowDirection = FlowDirection.TopDown;
            wrapper.WrapContents = false;

            workerTitle.Text = "Worker Categories";
            workerTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);

            workerGrid.AutoScroll = true;

            // ===================== MATERIAL SECTION WRAPPER =====================
            wrapper1.FlowDirection = FlowDirection.TopDown;
            wrapper1.WrapContents = false;

            materialTitle.Text = "Raw Material Stock";
            materialTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);

            materialGrid.WrapContents = false;

            // FINAL ATTACH
            Controls.Add(mainFlow);

            Name = "OwnerDash";
            Size = new Size(2049, 1075);

            mainFlow.ResumeLayout(false);
            mainFlow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pieChart).EndInit();
            ((System.ComponentModel.ISupportInitialize)barChart).EndInit();
            wrapper.ResumeLayout(false);
            wrapper.PerformLayout();
            wrapper1.ResumeLayout(false);
            wrapper1.PerformLayout();
            ResumeLayout(false);
        }

        // CARD HELPERS (Reusable UI blocks)

        // Small KPI card
        private Panel CreateCard()
        {
            Panel panel = new Panel
            {
                Size = new Size(450, 170),
                BackColor = Color.White,
                Margin = new Padding(10),
                Padding = new Padding(10)
            };

            MakeRounded(panel, 12);
            return panel;
        }

        // Large dashboard section panel
        private Panel CreateLargePanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(1400, 350),
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            MakeRounded(panel, 12);
            return panel;
        }

        // Chart container with title + border styling
        private Panel CreateChartHost(string title, Chart chart)
        {
            Panel outer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(10, 8, 10, 16),
                Padding = new Padding(2)
            };

            // Reapply rounded shape on resize
            outer.Resize += (s, e) =>
            {
                GraphicsPath path = new GraphicsPath();

                int radius = 18;

                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(outer.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(outer.Width - radius, outer.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, outer.Height - radius, radius, radius, 90, 90);

                path.CloseAllFigures();
                outer.Region = new Region(path);

                outer.Invalidate();
            };

            // Border drawing
            outer.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(1, 1, outer.Width - 3, outer.Height - 3);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(rect.X, rect.Y, 18, 18, 180, 90);
                    path.AddArc(rect.Right - 18, rect.Y, 18, 18, 270, 90);
                    path.AddArc(rect.Right - 18, rect.Bottom - 18, 18, 18, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - 18, 18, 18, 90, 90);

                    path.CloseAllFigures();

                    using (Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            // Layout: title + chart
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Label hdr = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0)
            };

            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.White;

            layout.Controls.Add(hdr, 0, 0);
            layout.Controls.Add(chart, 0, 1);

            outer.Controls.Add(layout);

            return outer;
        }

        // MINI WORKER CARD FACTORY

        private Label CreateMiniStatCard(
            FlowLayoutPanel parent,
            string title)
        {
            Panel panel = new Panel
            {
                Size = new Size(300, 100),
                BackColor = Color.White,
                Margin = new Padding(10)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Location = new Point(10, lblTitle.Bottom + 10),
                AutoSize = true
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValue);

            parent.Controls.Add(panel);

            MakeRounded(panel, 12);

            return lblValue;
        }

        // MATERIAL CARD FACTORY

        private Label CreateMaterialCard(
            FlowLayoutPanel parent,
            string material)
        {
            Panel panel = new Panel
            {
                Size = new Size(240, 180),
                BackColor = Color.White,
                Margin = new Padding(15),
                Padding = new Padding(10)
            };

            Label lblTitle = new Label
            {
                Text = material,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Location = new Point(15, 60),
                AutoSize = true
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValue);

            parent.Controls.Add(panel);

            MakeRounded(panel, 12);

            return lblValue;
        }

        // EXTRA UI ELEMENTS (declared at bottom for clarity)
        private Label lblTitle;
        private Label lblSub;
        private FlowLayoutPanel topRow;
        private TableLayoutPanel chartsRow;
        private FlowLayoutPanel wrapper;
        private Label workerTitle;
        private FlowLayoutPanel workerGrid;
        private FlowLayoutPanel wrapper1;
        private Label materialTitle;
        private FlowLayoutPanel materialGrid;
    }
}