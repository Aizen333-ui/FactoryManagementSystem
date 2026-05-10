using System.Drawing;
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
            mainFlow.BackColor = Color.FromArgb(245, 246, 250);

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
                "Material Types");

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
            workersCategoryPanel.Padding = new Padding(30, 25, 30, 25);
            workersCategoryPanel.Margin = new Padding(20, 30, 20, 30);
            workersCategoryPanel.BackColor = Color.White;

            // ================= TITLE =================
            Label workerTitle = new Label();
            workerTitle.Text = "Worker Categories";
            workerTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            workerTitle.AutoSize = true;
            workerTitle.Margin = new Padding(0, 0, 0, 25); // pushes grid DOWN
            workerTitle.Padding = new Padding(20, 20, 0, 0); 

            // ================= GRID =================
            FlowLayoutPanel workerGrid = new FlowLayoutPanel();
            workerGrid.Dock = DockStyle.Fill;
            workerGrid.AutoScroll = true;
            workerGrid.WrapContents = true;
            workerGrid.FlowDirection = FlowDirection.LeftToRight;
            workerGrid.Padding = new Padding(0, 50, 0, 0);
            workerGrid.Margin = new Padding(0);

            // ================= ADD CARDS =================
            lblLabourers = CreateMiniStatCard(workerGrid, "Labourers");
            lblDrivers = CreateMiniStatCard(workerGrid, "Drivers");
            lblLoaders = CreateMiniStatCard(workerGrid, "Loaders");
            lblOperators = CreateMiniStatCard(workerGrid, "Machine Operators");

            // ================= ORDER FIX =================
            workersCategoryPanel.Controls.Add(workerTitle);
            workersCategoryPanel.Controls.Add(workerGrid);

            mainFlow.Controls.Add(workersCategoryPanel);

            // =====================================================
            // MATERIAL PANEL
            // =====================================================

            materialsPanel = CreateLargePanel();
            materialsPanel.Size = new Size(1350, 320);
            materialsPanel.Padding = new Padding(30, 25, 30, 25);
            materialsPanel.Margin = new Padding(20, 30, 20, 30);

            // ================= TITLE =================
            Label materialTitle = new Label();
            materialTitle.Text = "Raw Material Stock";
            materialTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            materialTitle.AutoSize = true;
            materialTitle.Margin = new Padding(0, 0, 0, 25);
            materialTitle.Padding = new Padding(20, 20, 0, 0);


            // ================= GRID =================
            FlowLayoutPanel materialGrid = new FlowLayoutPanel();
            materialGrid.Dock = DockStyle.Fill;
            
            materialGrid.WrapContents = false;
            materialGrid.FlowDirection = FlowDirection.LeftToRight;
            materialGrid.Padding = new Padding(0, 50, 0, 0);
            materialGrid.Margin = new Padding(0);

            // ================= CARDS =================
            lblCement = CreateMaterialCard(materialGrid, "Cement");
            lblSand = CreateMaterialCard(materialGrid, "Sand");
            lblCrush = CreateMaterialCard(materialGrid, "Crush");
            lblSteel = CreateMaterialCard(materialGrid, "Steel");
            lblOil = CreateMaterialCard(materialGrid, "Mold Oil");

            // ================= ORDER FIX =================
            materialsPanel.Controls.Add(materialTitle);
            materialsPanel.Controls.Add(materialGrid);

            mainFlow.Controls.Add(materialsPanel);


            // =====================================================
            // CHARTS ROW
            // =====================================================

            FlowLayoutPanel chartsRow =
                new FlowLayoutPanel();

            chartsRow.Width = 1500;
            chartsRow.Height = 430;
            chartsRow.Margin = new Padding(0, 20, 0, 20);

            // =====================================================
            // PIE CHART PANEL
            // =====================================================

            Panel piePanel = CreateChartPanel(
                "Material Usage");

            pieChart = new Chart();

            pieChart.Location = new Point(20, 60);
            pieChart.Size = new Size(620, 300);
            piePanel.Controls.Add(pieChart);

            chartsRow.Controls.Add(piePanel);

            // =====================================================
            // BAR CHART PANEL
            // =====================================================

            Panel barPanel = CreateChartPanel(
                "Usage Analytics");

            barChart = new Chart();

            barChart.Location = new Point(20, 60);
            barChart.Size = new Size(620, 300);
            barPanel.Controls.Add(barChart);

            chartsRow.Controls.Add(barPanel);

            mainFlow.Controls.Add(chartsRow);
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

            return panel;
        }

        private Panel CreateLargePanel()
        {
            Panel panel = new Panel();

            panel.Size = new Size(1400, 350);
            panel.BackColor = Color.White;
            panel.Padding = new Padding(20, 20, 20, 20);
            return panel;
        }

        private Panel CreateChartPanel(string title)
        {
            Panel panel = new Panel();

            panel.Size = new Size(680, 450);
            panel.BackColor = Color.White;
            panel.Margin = new Padding(10);

            Label lbl = new Label();

            lbl.Text = title;
            lbl.Font =
                new Font("Segoe UI", 18F, FontStyle.Bold);

            lbl.Location = new Point(20, 20);

            panel.Controls.Add(lbl);

            return panel;
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

            

            

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValue);

            parent.Controls.Add(panel);

            return lblValue;
        }
    }
}