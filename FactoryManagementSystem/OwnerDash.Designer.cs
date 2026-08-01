using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ScottPlot.WinForms;
namespace FactoryManagementSystem
{
    partial class OwnerDash
    {
        private System.ComponentModel.IContainer components = null;


        // ================= KPI LABELS =================

        private Label lblWorkers;
        private Label lblExpenses;
        private Label lblProduction;

        // ================= WORKER LABELS =================

        private Label lblLabourers;
        private Label lblDrivers;
        private Label lblLoaders;
        private Label lblOperators;

        // ================= MATERIAL LABELS =================

        private Label lblCement;
        private Label lblSand;
        private Label lblCrush;
        private Label lblSteel;
        private Label lblOil;

        // ================= CHARTS =================

        private FormsPlot materialChart;
        private FormsPlot expenseChart;

        // ================= MAIN CONTAINERS =================

        private FlowLayoutPanel mainFlow;
        private Panel headerPanel;
        private Panel kpiPanel;
        private Panel workerPanel;
        private Panel materialPanel;
        private Panel chartPanel;

        // Only allowed FlowLayoutPanels
        private FlowLayoutPanel workerFlow;
        private FlowLayoutPanel materialFlow;
        private Label lblTitle;
        private Label lblSub;

        // =====================================================
        // INITIALIZE UI
        // =====================================================

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            SuspendLayout();

            // =================================================
            // MAIN CONTAINER
            // =================================================

            mainFlow = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(30, 20, 30, 20),
                BackColor = Color.White
            };

            Controls.Add(mainFlow);

            // =================================================
            // HEADER
            // =================================================

            headerPanel = new Panel()
            {
                Width = 1500,
                Height = 110,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 10)
            };

            lblTitle = new Label()
            {
                Text = "Factory Owner Dashboard",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                Location = new Point(30, 15),
                AutoSize = true
            };

            lblSub = new Label()
            {
                Text = "Production, workers and expenses overview",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                Location = new Point(35, 80),
                AutoSize = true
            };

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblSub);

            mainFlow.Controls.Add(headerPanel);

            // =================================================
            // KPI SECTION (styled like FactoryDash but keeping OwnerDash names)
            // =================================================

            kpiPanel = new Panel()
            {
                Width = 1500,
                Height = 190,
                BackColor = Color.White,
                Margin = new Padding(0, 30, 0, 20)
            };

            // Create three KPI cards laid out similar to FactoryDash
            Panel card1 = CreateCard();
            Panel card2 = CreateCard();
            Panel card3 = CreateCard();

            // Use existing OwnerDash label fields so backend names stay the same
            lblWorkers = CreateBigValueLabel();
            lblExpenses = CreateBigValueLabel();
            lblProduction = CreateBigValueLabel();

            card1.Controls.Add(CreateCardTitle("Total Workers"));
            card1.Controls.Add(lblWorkers);
            card2.Controls.Add(CreateCardTitle("Expenses"));
            card2.Controls.Add(lblExpenses);
            card3.Controls.Add(CreateCardTitle("Production"));
            card3.Controls.Add(lblProduction);

            // place cards inside kpiPanel using Locations to mimic FactoryDash spacing
            card1.Location = new Point(30, 20);
            card2.Location = new Point(500, 20);
            card3.Location = new Point(970, 20);

            kpiPanel.Controls.Add(card1);
            kpiPanel.Controls.Add(card2);
            kpiPanel.Controls.Add(card3);

            mainFlow.Controls.Add(kpiPanel);

            // =================================================
            // WORKER CATEGORY SECTION
            // =================================================

            workerPanel = new Panel()
            {
                Width = 1450,
                Height = 240,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 25)
            };

            Label workerTitle = new Label()
            {
                Text = "Worker Categories",
                Font = new Font(
                    "Segoe UI",
                    18,
                    FontStyle.Bold
                ),
                Location = new Point(25, 5),
                AutoSize = true
            };

            workerFlow = new FlowLayoutPanel()
            {
                Location = new Point(20, 55),
                Width = 1410,
                Height = 150,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.White
            };

            lblLabourers = CreateMiniStatCard(workerFlow, "Labour");
            lblDrivers = CreateMiniStatCard(workerFlow, "Driver");
            lblLoaders = CreateMiniStatCard(workerFlow, "Loader");
            lblOperators = CreateMiniStatCard(workerFlow, "Machine Operator");

            workerPanel.Controls.Add(workerTitle);
            workerPanel.Controls.Add(workerFlow);
            mainFlow.Controls.Add(workerPanel);

            // Create a large rounded container (like FactoryDash) to hold Worker and Material sections

            materialPanel = new Panel()
            {
                Width = 1450,
                Height = 260,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 0)

            };

            Label materialTitle = new Label()
            {
                Text = "Raw Material Stock",
                Font = new Font(
                    "Segoe UI",
                    18,
                    FontStyle.Bold
                ),
                Location = new Point(25, 15),
                AutoSize = true
            };

            materialFlow = new FlowLayoutPanel()
            {
                Location = new Point(20, 60),
                Width = 1410,
                Height = 170,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.White
            };

            lblCement = CreateMaterialCard(materialFlow, "Cement");
            lblSand = CreateMaterialCard(materialFlow, "Sand");
            lblCrush = CreateMaterialCard(materialFlow, "Crush");
            lblSteel = CreateMaterialCard(materialFlow, "Steel");
            lblOil = CreateMaterialCard(materialFlow, "Mold Oil");

            materialPanel.Controls.Add(materialTitle);
            materialPanel.Controls.Add(materialFlow);
            mainFlow.Controls.Add(materialPanel);
            materialPanel.Margin = new Padding(0, 0, 0, 25);

            // Add rounded style to inner panels as needed
            MakeRounded(workerPanel, 12);
            MakeRounded(materialPanel, 12);

            // =================================================
            // CHART SECTION (LAST)
            // =================================================

            chartPanel = new Panel()
            {
                Width = 1500,
                Height = 500,
                BackColor = Color.White,
                Margin = new Padding(0, 20, 0, 20)
            };

            TableLayoutPanel chartLayout =
                new TableLayoutPanel()
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 3,
                    RowCount = 1,
                    BackColor = Color.White
                };

            chartLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    48
                )
            );

            chartLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    2
                )
            );

            chartLayout.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    48
                )
            );

            materialChart = new FormsPlot();
            expenseChart = new FormsPlot();

            Panel materialChartHost = CreateChartHost("Material Usage", materialChart);
            Panel expenseChartHost = CreateChartHost("Expense Distribution", expenseChart);

            chartLayout.Controls.Add(materialChartHost, 0, 0);
            chartLayout.Controls.Add(expenseChartHost, 2, 0);

            chartPanel.Controls.Add(chartLayout);

            mainFlow.Controls.Add(chartPanel);

            // =================================================
            // FORM SETTINGS
            // =================================================

            BackColor = Color.White;
            Dock = DockStyle.Fill;
            Name = "OwnerDash";

            ResumeLayout(false);
        }
        // =====================================================
        // KPI CARD CREATOR
        // =====================================================

        private Panel CreateCard()
        {
            Panel panel = new Panel()
            {
                Size = new Size(430, 160),
                BackColor = Color.White,
                Margin = new Padding(10),
                Padding = new Padding(10)
            };

            MakeRounded(panel, 16);

            return panel;
        }

        // =====================================================
        // CHART CONTAINER
        // =====================================================

        private Panel CreateChartHost(
            string title,
            FormsPlot chart)
        {

            Panel outer = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            MakeRounded(outer, 18);

            TableLayoutPanel layout =
                new TableLayoutPanel()
                {
                    Dock = DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1,
                    BackColor = Color.White
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
            chart.MinimumSize = new Size(300, 250);

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(chart, 0, 1);

            outer.Controls.Add(layout);

            return outer;
        }

        // =====================================================
        // LABEL HELPERS
        // =====================================================

        private Label CreateCardTitle(string text)
        {
            return new Label()
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black,
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
                ForeColor = Color.Black,
                Location = new Point(20, 70),
                AutoSize = true
            };
        }

        // =====================================================
        // WORKER MINI CARD
        // =====================================================

        private Label CreateMiniStatCard(
            FlowLayoutPanel parent,
            string title)
        {

            Panel panel = new Panel()
            {
                Size = new Size(300, 130),
                BackColor = Color.White,
                Margin = new Padding(10),
                Padding = new Padding(10)

            };


            Label name = new Label()
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            Label value = new Label()
            {
                Text = "0",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(15, 45),
                AutoSize = true
            };

            panel.Controls.Add(name);
            panel.Controls.Add(value);

            MakeRounded(panel, 12);

            parent.Controls.Add(panel);

            return value;
        }

        // =====================================================
        // MATERIAL CARD
        // =====================================================

        private Label CreateMaterialCard(
            FlowLayoutPanel parent,
            string material)
        {

            Panel panel = new Panel()
            {
                Size = new Size(240, 100),
                BackColor = Color.White,
                Margin = new Padding(10)
            };

            Label name = new Label()
            {
                Text = material,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
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

            MakeRounded(panel, 12);

            parent.Controls.Add(panel);

            return value;
        }

        // =====================================================
        // ROUND CORNERS
        // =====================================================

        private void MakeRounded(Control ctl, int radius)
        {
            void ApplyRegion()
            {
                GraphicsPath path = new GraphicsPath();

                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(ctl.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(ctl.Width - radius, ctl.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, ctl.Height - radius, radius, radius, 90, 90);

                path.CloseAllFigures();
                ctl.Region = new Region(path);
            }

            ApplyRegion();

            // ============================
            // DRAW BORDER
            // ============================

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

            // ============================
            // UPDATE WHEN RESIZED
            // ============================

            ctl.Resize += (s, e) =>
            {
                ApplyRegion();
                ctl.Invalidate();
            };
        }
    }
}