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

            ChartArea area = new ChartArea("Main");
            area.BackColor = Color.White;

            pieChart.ChartAreas.Add(area);

            Legend lg = new Legend();
            lg.Docking = Docking.Bottom;
            lg.Font = new Font("Segoe UI", 9F);

            pieChart.Legends.Add(lg);

            Series s = new Series("Materials");

            s.ChartType = SeriesChartType.Pie;

            s.IsValueShownAsLabel = true;

            s.LabelFormat = "0.##'%'";
            s.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            DataTable dt = DBHelper.ExecuteDataTable(@"
                 SELECT 
                    MaterialName,
                    SUM(QuantityUsed) AS TotalUsed
                FROM MaterialUsage
                WHERE 
                    MONTH(Date) = MONTH(GETDATE())
                    AND YEAR(Date) = YEAR(GETDATE())
                GROUP BY MaterialName
    ", null);

            double grandTotal = 0;

            foreach (DataRow r in dt.Rows)
            {
                grandTotal += Convert.ToDouble(r["TotalUsed"]);
            }

            foreach (DataRow r in dt.Rows)
            {
                string material = r["MaterialName"].ToString();

                double used =
                    Convert.ToDouble(r["TotalUsed"]);

                double percent =
                    (used / grandTotal) * 100;

                s.Points.AddXY(material, percent);
            }

            pieChart.Series.Add(s);
        }

        // =========================================================
        // BAR CHART
        // =========================================================

        private void LoadExpenseChart()
        {
            barChart.Series.Clear();
            barChart.ChartAreas.Clear();
            barChart.Legends.Clear();
            barChart.Titles.Clear();

            ChartArea area = new ChartArea("MainArea");

            area.BackColor = Color.White;

            barChart.ChartAreas.Add(area);

            Legend lg = new Legend();

            lg.Docking = Docking.Bottom;
            lg.Font = new Font("Segoe UI", 9F);

            barChart.Legends.Add(lg);

            Series series = new Series("Expenses");

            series.ChartType = SeriesChartType.Pie;

            series.IsValueShownAsLabel = true;

            series.LabelFormat = "0.##'%'";

            series.Font =
                new Font("Segoe UI", 9F, FontStyle.Bold);

            DataTable dt = DBHelper.ExecuteDataTable(@"
        SELECT 
            UPPER(LTRIM(RTRIM(Reason))) AS Reason,
            SUM(Amount) AS TotalAmount
        FROM Payments
        WHERE 
            MONTH(Date) = MONTH(GETDATE())
            AND YEAR(Date) = YEAR(GETDATE())
        GROUP BY UPPER(LTRIM(RTRIM(Reason)))
    ", null);

            double grandTotal = 0;

            foreach (DataRow row in dt.Rows)
            {
                grandTotal +=
                    Convert.ToDouble(row["TotalAmount"]);
            }

            foreach (DataRow row in dt.Rows)
            {
                string reason =
                    row["Reason"].ToString();

                double amount =
                    Convert.ToDouble(row["TotalAmount"]);

                double percent =
                    (amount / grandTotal) * 100;

                series.Points.AddXY(reason, percent);
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