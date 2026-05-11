using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    partial class OwnerDash
    {
        private System.ComponentModel.IContainer components = null;

        // =====================================================
        // KPI LABELS
        // =====================================================

        private Label lblWorkers;
        private Label lblExpenses;
        private Label lblProduction;

        // =====================================================
        // WORKER CATEGORY LABELS
        // =====================================================

        private Label lblLabourers;
        private Label lblDrivers;
        private Label lblLoaders;
        private Label lblOperators;

        // =====================================================
        // MATERIAL LABELS
        // =====================================================

        private Label lblCement;
        private Label lblSand;
        private Label lblCrush;
        private Label lblSteel;
        private Label lblOil;

        // =====================================================
        // CHARTS
        // =====================================================

        private Chart pieChart;
        private Chart barChart;

        // =====================================================
        // PANELS
        // =====================================================

        private FlowLayoutPanel mainFlow;

        private Panel cardWorkers;
        private Panel cardExpenses;
        private Panel cardProduction;

        private Panel workersCategoryPanel;
        private Panel materialsPanel;
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

                    // SAME BORDER COLOR AS RAW MATERIAL
                    using (Pen pen = new Pen(
                        Color.FromArgb(180, 190, 210),
                        1.5f))
                    {
                        e.Graphics.DrawPath(pen, borderPath);
                    }
                }
            };

            // Reapply on resize
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
        // =====================================================
        // INITIALIZE
        // =====================================================

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // =====================================================
            // USER CONTROL
            // =====================================================

            this.BackColor = Color.FromArgb(240, 242, 247);
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // =====================================================
            // MAIN FLOW
            // =====================================================

            mainFlow = new FlowLayoutPanel();

            mainFlow.Dock = DockStyle.Fill;
            mainFlow.FlowDirection = FlowDirection.TopDown;
            mainFlow.WrapContents = false;
            mainFlow.AutoScroll = true;
            mainFlow.AutoScrollPosition = new Point(0, 0);
            mainFlow.Padding = new Padding(30, 20, 30, 20);
            mainFlow.BackColor = Color.White;

            this.Controls.Add(mainFlow);

            // =====================================================
            // DASHBOARD TITLE
            // =====================================================

            Label lblTitle = new Label();

            lblTitle.Text = "Factory Stats";
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.ForeColor = Color.Black;

            Label lblSub = new Label();

            lblSub.Text =
                "Real-time overview of factory operations";

            lblSub.Font = new Font("Segoe UI", 11F);
            lblSub.AutoSize = true;
            lblSub.ForeColor = Color.Gray;

            mainFlow.Controls.Add(lblTitle);
            mainFlow.Controls.Add(lblSub);

            // =====================================================
            // KPI ROW
            // =====================================================

            FlowLayoutPanel topRow = new FlowLayoutPanel();

            topRow.Width = 1500;
            topRow.Height = 190;
            topRow.Margin = new Padding(0, 30, 0, 20);
            


            // =====================================================
            // WORKERS CARD
            // =====================================================

            cardWorkers = CreateCard();

            Label titleWorkers = CreateCardTitle(
                "Total Workers");

            lblWorkers = CreateBigValueLabel();

            cardWorkers.Controls.Add(titleWorkers);
            cardWorkers.Controls.Add(lblWorkers);

            topRow.Controls.Add(cardWorkers);

            // =====================================================
            // EXPENSE CARD
            // =====================================================

            cardExpenses = CreateCard();

            Label titleExpenses = CreateCardTitle(
                "Total Expenses");

            lblExpenses = CreateBigValueLabel();

            cardExpenses.Controls.Add(titleExpenses);
            cardExpenses.Controls.Add(lblExpenses);

            topRow.Controls.Add(cardExpenses);

            // =====================================================
            // MATERIAL CARD
            // =====================================================

            cardProduction = CreateCard();

            Label titleProduction = CreateCardTitle(
                "Total Raw Material");

            lblProduction = CreateBigValueLabel();

            cardProduction.Controls.Add(titleProduction);
            cardProduction.Controls.Add(lblProduction);

            topRow.Controls.Add(cardProduction);

            mainFlow.Controls.Add(topRow);

            // =====================================================
            // WORKER CATEGORY PANEL
            // =====================================================

            workersCategoryPanel = CreateLargePanel();
            workersCategoryPanel.Size = new Size(1350, 300);
            workersCategoryPanel.Padding = new Padding(20);
            workersCategoryPanel.Margin = new Padding(20, 30, 20, 30);

            // INNER LAYOUT
            FlowLayoutPanel wrapper = new FlowLayoutPanel();
            wrapper.Dock = DockStyle.Fill;
            wrapper.FlowDirection = FlowDirection.TopDown;
            wrapper.WrapContents = false;
            wrapper.AutoScroll = false;

            // TITLE (NO PADDING, NO MARGIN HACKS)
            Label workerTitle = new Label();
            workerTitle.Text = "Worker Categories";
            workerTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            workerTitle.AutoSize = true;
            workerTitle.Margin = new Padding(5, 5, 0, 15);

            // GRID
            FlowLayoutPanel workerGrid = new FlowLayoutPanel();
            workerGrid.Width = 1300;
            workerGrid.Height = 200;
            workerGrid.WrapContents = true;
            workerGrid.FlowDirection = FlowDirection.LeftToRight;
            workerGrid.AutoScroll = true;

            // CARDS
            lblLabourers = CreateMiniStatCard(workerGrid, "Labourers");
            lblDrivers = CreateMiniStatCard(workerGrid, "Drivers");
            lblLoaders = CreateMiniStatCard(workerGrid, "Loaders");
            lblOperators = CreateMiniStatCard(workerGrid, "Machine Operators");

            // ADD
            wrapper.Controls.Add(workerTitle);
            wrapper.Controls.Add(workerGrid);

            workersCategoryPanel.Controls.Clear();
            workersCategoryPanel.Controls.Add(wrapper);

            mainFlow.Controls.Add(workersCategoryPanel);

            // =====================================================
            // MATERIAL PANEL
            // =====================================================

            materialsPanel = CreateLargePanel();
            materialsPanel.Size = new Size(1450, 320);
            materialsPanel.Padding = new Padding(20);
            materialsPanel.Margin = new Padding(20, 30, 20, 30);

            // INNER LAYOUT
            FlowLayoutPanel wrapper1 = new FlowLayoutPanel();
            wrapper1.Dock = DockStyle.Fill;
            wrapper1.FlowDirection = FlowDirection.TopDown;
            wrapper1.WrapContents = false;

            // TITLE
            Label materialTitle = new Label();
            materialTitle.Text = "Raw Material Stock";
            materialTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            materialTitle.AutoSize = true;
            materialTitle.Margin = new Padding(5, 5, 0, 15);

            // GRID
            FlowLayoutPanel materialGrid = new FlowLayoutPanel();
            materialGrid.Width = 1450;
            materialGrid.Height = 220;
            materialGrid.WrapContents = false;
            materialGrid.FlowDirection = FlowDirection.LeftToRight;

            // CARDS
            lblCement = CreateMaterialCard(materialGrid, "Cement");
            lblSand = CreateMaterialCard(materialGrid, "Sand");
            lblCrush = CreateMaterialCard(materialGrid, "Crush");
            lblSteel = CreateMaterialCard(materialGrid, "Steel");
            lblOil = CreateMaterialCard(materialGrid, "Mold Oil");

            // ADD
            wrapper1.Controls.Add(materialTitle);
            wrapper1.Controls.Add(materialGrid);

            materialsPanel.Controls.Clear();
            materialsPanel.Controls.Add(wrapper1);

            mainFlow.Controls.Add(materialsPanel);


            // =====================================================
            // CHARTS ROW
            // =====================================================

            TableLayoutPanel chartsRow = new TableLayoutPanel();
            chartsRow.Width = 1500;
            chartsRow.Height = 480;
            chartsRow.Margin = new Padding(0, 12, 0, 24);

            chartsRow.Dock = DockStyle.Fill;   // ✅ IMPORTANT FIX

            chartsRow.ColumnCount = 2;
            chartsRow.RowCount = 1;

            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            chartsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            chartsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            chartsRow.BackColor = Color.White;
            chartsRow.Padding = new Padding(10);

            pieChart = new Chart();
            Panel pieHost = CreateChartHost("Material Usage", pieChart);

            barChart = new Chart();
            Panel barHost = CreateChartHost("Monthly Expenses", barChart);

            chartsRow.Controls.Add(pieHost, 0, 0);
            chartsRow.Controls.Add(barHost, 1, 0);

            mainFlow.Controls.Add(chartsRow);
            MakeRounded(cardWorkers, 16);
            MakeRounded(cardExpenses, 16);
            MakeRounded(cardProduction, 16);

            MakeRounded(workersCategoryPanel, 16);
            MakeRounded(materialsPanel, 16);
            this.ResumeLayout(false);
        }

        // =====================================================
        // CARD HELPERS
        // =====================================================

        private Panel CreateCard()
        {
            Panel panel = new Panel();

            panel.Size = new Size(450, 170);
            panel.BackColor = Color.FromArgb(255, 255, 255);
            panel.Margin = new Padding(10);
            panel.Padding = new Padding(10);
            MakeRounded(panel, 12);
            return panel;
        }

        private Panel CreateLargePanel()
        {
            Panel panel = new Panel();

            panel.Size = new Size(1400, 350);
            panel.BackColor = Color.White;
            panel.Padding = new Padding(20, 20, 20, 20);
            MakeRounded(panel, 12);
            return panel;
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
                GraphicsPath path = new GraphicsPath();

                int radius = 18;

                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(outer.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(outer.Width - radius,
                            outer.Height - radius,
                            radius,
                            radius,
                            0,
                            90);

                path.AddArc(0,
                            outer.Height - radius,
                            radius,
                            radius,
                            90,
                            90);

                path.CloseAllFigures();

                outer.Region = new Region(path);

                outer.Invalidate();
            };

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

        // =====================================================
        // MINI WORKER CARDS
        // =====================================================

        private Label CreateMiniStatCard(
            FlowLayoutPanel parent,
            string title)
        {
            Panel panel = new Panel();

            panel.Size = new Size(300, 100);
            panel.BackColor = Color.White;
            panel.Margin = new Padding(10);

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(10, 10);

            // VALUE
            Label lblValue = new Label();
            lblValue.Text = "0";
            lblValue.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblValue.AutoSize = true;

            // IMPORTANT: place value BELOW title properly
            lblValue.Location = new Point(10, lblTitle.Bottom + 10);

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValue);

            parent.Controls.Add(panel);
            MakeRounded(panel, 12);
            return lblValue;
        }

        // =====================================================
        // MATERIAL CARDS
        // =====================================================

        private Label CreateMaterialCard(
            FlowLayoutPanel parent,
            string material)
        {
            Panel panel = new Panel();

            panel.Size = new Size(240, 180);
            panel.BackColor = Color.White;
            panel.Margin = new Padding(15);
            panel.Padding = new Padding(10);

            Label lblTitle = new Label();

            lblTitle.Text = material;

            lblTitle.Font =
                new Font("Segoe UI", 12F, FontStyle.Bold);

            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoSize = true;

            Label lblValue = new Label();

            lblValue.Text = "0";

            lblValue.Font =
                new Font("Segoe UI", 16F, FontStyle.Bold);

            lblValue.Location = new Point(15, 60);
            lblValue.AutoSize = true;

           


            MakeRounded(panel, 12);

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValue);

            parent.Controls.Add(panel);

            return lblValue;
        }

    }
}