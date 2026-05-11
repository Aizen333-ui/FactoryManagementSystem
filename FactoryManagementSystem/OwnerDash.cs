using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    public partial class OwnerDash : UserControl
    {
        public OwnerDash()
        {
            InitializeComponent();

            pieChart.TabStop = false;
            barChart.TabStop = false;

            this.HandleCreated += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    LoadPieChart();
                    LoadExpenseChart();
                }));
            };

            LoadWorkers();
            LoadRevenue();
            LoadProduction();
            LoadWorkerCategoryData();
            LoadRawMaterialCards();
        }

        // =========================================================
        // TOTAL WORKERS
        // =========================================================

        private void LoadWorkers()
        {
            string query = "SELECT COUNT(*) FROM Workers";

            int total = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, null)
            );

            lblWorkers.Text = total.ToString();
        }

        // =========================================================
        // TOTAL EXPENSES / REVENUE
        // =========================================================

        private void LoadRevenue()
        {
            string query =
                "SELECT ISNULL(SUM(Amount),0) FROM Payments";

            decimal total = Convert.ToDecimal(
                DBHelper.ExecuteScalar(query, null)
            );

            lblExpenses.Text = "Rs " + total.ToString("N0");
        }

        // =========================================================
        // TOTAL MATERIAL TYPES
        // =========================================================

        private void LoadProduction()
        {
            string query =
                "SELECT ISNULL(SUM(Quantity), 0) AS TotalRawMaterial FROM RawMaterial;";

            int total = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, null)
            );

            lblProduction.Text = total.ToString();
        }

        // =========================================================
        // PIE CHART
        // =========================================================

        private void LoadPieChart()
        {
            pieChart.Series.Clear();
            pieChart.ChartAreas.Clear();
            pieChart.Legends.Clear();

            var area = new ChartArea();
            area.Name = "PieArea";
            pieChart.ChartAreas.Add(area);

            var legend = new Legend();
            legend.Name = "Legend1";
            pieChart.Legends.Add(legend);

            Series series = new Series();
            series.Name = "Materials";
            series.ChartType = SeriesChartType.Pie;
            series.ChartArea = "PieArea";
            series.Legend = "Legend1";

            DataTable dt = DBHelper.ExecuteDataTable(@"
        SELECT 
            UPPER(LTRIM(RTRIM(MaterialName))) AS MaterialName,
            SUM(QuantityUsed) AS TotalUsed
        FROM MaterialUsage
        GROUP BY UPPER(LTRIM(RTRIM(MaterialName)))
    ", null);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data for Pie Chart");
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                series.Points.AddXY(row["MaterialName"], row["TotalUsed"]);
            }

            pieChart.Series.Add(series);

            pieChart.Dock = DockStyle.Fill;
            pieChart.Refresh();
        }

        // =========================================================
        // BAR CHART
        // =========================================================

        private void LoadExpenseChart()
        {
            barChart.Series.Clear();
            barChart.ChartAreas.Clear();
            barChart.Titles.Clear();

            ChartArea area = new ChartArea("MainArea");
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -30;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            barChart.ChartAreas.Add(area);

            Series series = new Series("Expenses");
            series.ChartType = SeriesChartType.Column;

            // 🔥 CRITICAL FIXES (these are what you're missing)
            series.XValueType = ChartValueType.String;
            series.IsXValueIndexed = false;

            // 🔥 THIS PREVENTS STACKING BEHAVIOR
            series["PointWidth"] = "0.6";
            series["DrawSideBySide"] = "true";

            series.IsValueShownAsLabel = true;
            series.ChartArea = "MainArea";

            DataTable dt = DBHelper.ExecuteDataTable(@"
        SELECT 
            UPPER(LTRIM(RTRIM(Reason))) AS Reason,
            SUM(Amount) AS TotalAmount
        FROM Payments
        GROUP BY UPPER(LTRIM(RTRIM(Reason)))
    ", null);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data for Bar Chart");
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                string reason = row["Reason"].ToString().Trim();
                decimal amount = Convert.ToDecimal(row["TotalAmount"]);

                // 🔥 IMPORTANT FIX #2 (force unique X keys)
                DataPoint dp = new DataPoint();
                dp.AxisLabel = reason;
                dp.YValues = new double[] { (double)amount };

                series.Points.Add(dp);
            }

            barChart.Series.Add(series);

            barChart.Dock = DockStyle.Fill;
            barChart.Refresh();
        }

        // =========================================================
        // WORKER CATEGORY DATA
        // =========================================================

        private void LoadWorkerCategoryData()
        {
            LoadWorkerCount(lblLabourers, "Labor");
            LoadWorkerCount(lblDrivers, "Driver");
            LoadWorkerCount(lblLoaders, "Loader");
            LoadWorkerCount(lblOperators, "Machine Operator");
        }

        private void LoadWorkerCount(Label lbl, string role)
        {
            string query =
                "SELECT COUNT(*) FROM Workers WHERE Role=@role";

            var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@role", role)
            };

            int count = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, parameters)
            );

            lbl.Text = count.ToString();
        }

        // =========================================================
        // RAW MATERIAL STOCK CARDS
        // =========================================================

        private void LoadRawMaterialCards()
        {
            LoadMaterial(lblCement, "Cement");
            LoadMaterial(lblSand, "Sand");
            LoadMaterial(lblCrush, "Crush");
            LoadMaterial(lblSteel, "Steel");
            LoadMaterial(lblOil, "Mold Oil");
        }

        private void LoadMaterial(Label lbl, string material)
        {
            string query = @"
                SELECT ISNULL(SUM(Quantity),0)
                FROM RawMaterial
                WHERE UPPER(LTRIM(RTRIM(Name)))
                =
                UPPER(LTRIM(RTRIM(@name)))
            ";

            var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@name", material)
            };

            int qty = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, parameters)
            );

            lbl.Text = qty.ToString();
        }
        
    }
}