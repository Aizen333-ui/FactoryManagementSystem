
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    partial class OwnerDash
    {
        private System.ComponentModel.IContainer components = null;

        // ===================== KPI LABELS =====================
        private Label lblWorkers;
        private Label lblExpenses;
        private Label lblProduction;

        // ===================== WORKER CATEGORY LABELS =====================
        private Label lblLabourers;
        private Label lblDrivers;
        private Label lblLoaders;
        private Label lblOperators;

        // ===================== MATERIAL LABELS =====================
        private Label lblCement;
        private Label lblSand;
        private Label lblCrush;
        private Label lblSteel;
        private Label lblOil;

        // ===================== CHARTS =====================
        private Chart pieChart;
        private Chart barChart;

        // ===================== MAIN LAYOUT =====================
        private FlowLayoutPanel mainFlow;
        private Panel cardWorkers;
        private Panel cardExpenses;
        private Panel cardProduction;
        private Panel workerPanel;
        private Panel materialPanel;
        private FlowLayoutPanel workerFlow;
        private FlowLayoutPanel materialFlow;
        private Label lblTitle;
        private Label lblSub;

        // ==========================================================
        // ROUND CONTROL WITH BORDER
        // ==========================================================
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
                using (GraphicsPath border = new GraphicsPath())
                {
                    border.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    border.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                    border.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                    border.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                    border.CloseAllFigures();

                    using (Pen pen = new Pen(Color.FromArgb(180, 190, 210), 1.5f))
                    {
                        e.Graphics.DrawPath(pen, border);
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

        // ==========================================================
        // INITIALIZE UI
        // ==========================================================
        private void InitializeComponent()
        {
            mainFlow = new FlowLayoutPanel();
            lblTitle = new Label();
            lblSub = new Label();
            topRow = new FlowLayoutPanel();
            chartsRow = new TableLayoutPanel();
            pieChart = new Chart();
            divider = new Panel();
            barChart = new Chart();
            workerHeader = new Label();
            workerFlow = new FlowLayoutPanel();
            materialHeader = new Label();
            materialFlow = new FlowLayoutPanel();
            mainFlow.SuspendLayout();
            chartsRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pieChart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barChart).BeginInit();
            SuspendLayout();
            // 
            // mainFlow
            // 
            mainFlow.Controls.Add(lblTitle);
            mainFlow.Controls.Add(lblSub);
            mainFlow.Controls.Add(topRow);
            mainFlow.Controls.Add(chartsRow);
            mainFlow.Location = new Point(0, 0);
            mainFlow.Name = "mainFlow";
            mainFlow.Size = new Size(200, 100);
            mainFlow.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 0;
            // 
            // lblSub
            // 
            lblSub.Location = new Point(3, 23);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(100, 23);
            lblSub.TabIndex = 1;
            // 
            // topRow
            // 
            topRow.Location = new Point(3, 49);
            topRow.Name = "topRow";
            topRow.Size = new Size(200, 100);
            topRow.TabIndex = 2;
            // 
            // chartsRow
            // 
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 2F));
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            chartsRow.Controls.Add(divider, 1, 0);
            chartsRow.Location = new Point(3, 155);
            chartsRow.Name = "chartsRow";
            chartsRow.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            chartsRow.Size = new Size(200, 100);
            chartsRow.TabIndex = 3;
            // 
            // pieChart
            // 
            pieChart.Location = new Point(0, 0);
            pieChart.Name = "pieChart";
            pieChart.Size = new Size(300, 300);
            pieChart.TabIndex = 0;
            // 
            // divider
            // 
            divider.Location = new Point(201, 3);
            divider.Name = "divider";
            divider.Size = new Size(1, 94);
            divider.TabIndex = 0;
            // 
            // barChart
            // 
            barChart.Location = new Point(0, 0);
            barChart.Name = "barChart";
            barChart.Size = new Size(300, 300);
            barChart.TabIndex = 0;
            // 
            // workerHeader
            // 
            workerHeader.Location = new Point(0, 0);
            workerHeader.Name = "workerHeader";
            workerHeader.Size = new Size(100, 23);
            workerHeader.TabIndex = 0;
            // 
            // workerFlow
            // 
            workerFlow.Location = new Point(0, 0);
            workerFlow.Name = "workerFlow";
            workerFlow.Size = new Size(200, 100);
            workerFlow.TabIndex = 0;
            // 
            // materialHeader
            // 
            materialHeader.Location = new Point(0, 0);
            materialHeader.Name = "materialHeader";
            materialHeader.Size = new Size(100, 23);
            materialHeader.TabIndex = 0;
            // 
            // materialFlow
            // 
            materialFlow.Location = new Point(0, 0);
            materialFlow.Name = "materialFlow";
            materialFlow.Size = new Size(200, 100);
            materialFlow.TabIndex = 0;
            // 
            // OwnerDash
            // 
            BackColor = Color.White;
            Controls.Add(mainFlow);
            Name = "OwnerDash";
            Size = new Size(1966, 992);
            mainFlow.ResumeLayout(false);
            chartsRow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pieChart).EndInit();
            ((System.ComponentModel.ISupportInitialize)barChart).EndInit();
            ResumeLayout(false);
        }

        // =====================================================
        // KPI CARD CREATOR
        // =====================================================
        private Panel CreateCard()
        {
            Panel panel = new Panel()
            {
                Size = new Size(450, 170),
                BackColor = Color.White,
                Margin = new Padding(10),
                Padding = new Padding(10)
            };
            MakeRounded(panel, 16);
            return panel;
        }

        private Panel CreateLargePanel()
        {
            Panel panel = new Panel()
            {
                Size = new Size(1400, 300),
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            return panel;
        }

        // =====================================================
        // CHART CONTAINER
        // =====================================================
        private Panel CreateChartHost(string title, Chart chart)
        {
            Panel outer = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(10)
            };
            MakeRounded(outer, 18);

            TableLayoutPanel layout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Label header = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Padding = new Padding(15, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.White;

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(chart, 0, 1);
            outer.Controls.Add(layout);

            return outer;
        }

        // =====================================================
        // SMALL LABEL HELPERS
        // =====================================================
        private Label CreateCardTitle(string text)
        {
            return new Label()
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Gray,
                Location = new Point(20, 20),
                AutoSize = true
            };
        }

        private Label CreateBigValueLabel()
        {
            return new Label()
            {
                Text = "0",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                Location = new Point(20, 70),
                AutoSize = true
            };
        }

        // =====================================================
        // WORKER CARD
        // =====================================================
        private Label CreateMiniStatCard(FlowLayoutPanel parent, string title)
        {
            Panel panel = new Panel()
            {
                Size = new Size(300, 100),
                BackColor = Color.White,
                Margin = new Padding(10)
            };

            Label name = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            Label value = new Label()
            {
                Text = "0",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(15, 50),
                AutoSize = true
            };

            panel.Controls.Add(name);
            panel.Controls.Add(value);
            parent.Controls.Add(panel);
            MakeRounded(panel, 12);

            return value;
        }

        // =====================================================
        // MATERIAL CARD
        // =====================================================
        private Label CreateMaterialCard(FlowLayoutPanel parent, string material)
        {
            Panel panel = new Panel()
            {
                Size = new Size(240, 150),
                BackColor = Color.White,
                Margin = new Padding(10)
            };

            Label name = new Label()
            {
                Text = material,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 20),
                AutoSize = true
            };

            Label value = new Label()
            {
                Text = "0",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(15, 70),
                AutoSize = true
            };

            panel.Controls.Add(name);
            panel.Controls.Add(value);
            parent.Controls.Add(panel);
            MakeRounded(panel, 12);

            return value;
        }

        private FlowLayoutPanel topRow;
        private TableLayoutPanel chartsRow;
        private Panel divider;
        private Label workerHeader;
        private Label materialHeader;
    }
}
