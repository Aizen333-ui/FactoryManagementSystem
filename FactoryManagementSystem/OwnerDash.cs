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

            // LOAD DATABASE DATA
            LoadWorkers();
            LoadRevenue();
            LoadProduction();

            LoadPieChart();
            LoadMaterialUsageChart();

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
                "SELECT COUNT(DISTINCT MaterialName) FROM MaterialUsage";

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

            ChartArea area = new ChartArea("PieArea");
            pieChart.ChartAreas.Add(area);

            Legend legend = new Legend("Legend1");
            pieChart.Legends.Add(legend);

            Series series = new Series("Materials");

            series.ChartType = SeriesChartType.Pie;
            series.ChartArea = "PieArea";
            series.Legend = "Legend1";

            string query = @"
                SELECT 
                    UPPER(LTRIM(RTRIM(MaterialName))) AS MaterialName,
                    SUM(QuantityUsed) AS TotalUsed
                FROM MaterialUsage
                GROUP BY UPPER(LTRIM(RTRIM(MaterialName)))
            ";

            DataTable dt =
                DBHelper.ExecuteDataTable(query, null);

            foreach (DataRow row in dt.Rows)
            {
                series.Points.AddXY(
                    row["MaterialName"].ToString(),
                    Convert.ToInt32(row["TotalUsed"])
                );
            }

            pieChart.Series.Add(series);

            pieChart.Palette = ChartColorPalette.Bright;
        }

        // =========================================================
        // BAR CHART
        // =========================================================

        private void LoadMaterialUsageChart()
        {
            barChart.Series.Clear();
            barChart.ChartAreas.Clear();

            ChartArea area = new ChartArea("BarArea");
            barChart.ChartAreas.Add(area);

            Series series = new Series("Usage");

            series.ChartType = SeriesChartType.Column;
            series.ChartArea = "BarArea";

            string query = @"
                SELECT
                    UPPER(LTRIM(RTRIM(MaterialName))) AS MaterialName,
                    SUM(QuantityUsed) AS TotalUsed
                FROM MaterialUsage
                GROUP BY UPPER(LTRIM(RTRIM(MaterialName)))
            ";

            DataTable dt =
                DBHelper.ExecuteDataTable(query, null);

            foreach (DataRow row in dt.Rows)
            {
                series.Points.AddXY(
                    row["MaterialName"].ToString(),
                    Convert.ToInt32(row["TotalUsed"])
                );
            }

            barChart.Series.Add(series);

            barChart.Palette = ChartColorPalette.Excel;
        }

        // =========================================================
        // WORKER CATEGORY DATA
        // =========================================================

        private void LoadWorkerCategoryData()
        {
            LoadWorkerCount(lblLabourers, "Labourer");
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
                SELECT ISNULL(SUM(QuantityUsed),0)
                FROM MaterialUsage
                WHERE UPPER(LTRIM(RTRIM(MaterialName)))
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