using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace FactoryManagementSystem
{
    public partial class OwnerDash : UserControl
    {
        public OwnerDash()
        {
            InitializeComponent();

            // Prevent chart controls from receiving focus (better UX)
            pieChart.TabStop = false;
            pieChart.TabStop = false;

            // Delay chart loading until control handle is created
            this.HandleCreated += (s, e) =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    LoadPieChart();
                    LoadExpenseChart();
                }));
            };

            // Load all dashboard KPIs and summaries
            LoadWorkers();
            LoadRevenue();
            LoadProduction();
            LoadWorkerCategoryData();
            LoadRawMaterialCards();
        }

        // TOTAL WORKERS KPI
        private void LoadWorkers()
        {
            string query = "SELECT COUNT(*) FROM Workers";

            int total = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, null)
            );

            lblWorkers.Text = total.ToString();
        }

        // TOTAL REVENUE / EXPENSES KPI
        private void LoadRevenue()
        {
            string query = "SELECT ISNULL(SUM(Amount),0) FROM Payments";

            decimal total = Convert.ToDecimal(
                DBHelper.ExecuteScalar(query, null)
            );

            lblExpenses.Text = "Rs " + total.ToString("N0");
        }

        // TOTAL RAW MATERIAL / PRODUCTION KPI
        private void LoadProduction()
        {
            string query =
                "SELECT ISNULL(SUM(Quantity), 0) AS TotalRawMaterial FROM RawMaterial;";

            int total = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, null)
            );

            lblProduction.Text = total.ToString();
        }

        // PIE CHART: MATERIAL USAGE DISTRIBUTION
        private void LoadPieChart()
        {
            pieChart.Series.Clear();
            pieChart.ChartAreas.Clear();
            pieChart.Legends.Clear();

            ChartArea area = new ChartArea("Main")
            {
                BackColor = Color.White
            };

            pieChart.ChartAreas.Add(area);

            Legend lg = new Legend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 9F)
            };

            pieChart.Legends.Add(lg);

            Series s = new Series("Materials")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "0.##'%'",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            // Fetch monthly material usage
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

            // Calculate total usage for percentage conversion
            foreach (DataRow r in dt.Rows)
            {
                grandTotal += Convert.ToDouble(r["TotalUsed"]);
            }

            // Add data points as percentages
            foreach (DataRow r in dt.Rows)
            {
                string material = r["MaterialName"].ToString();
                double used = Convert.ToDouble(r["TotalUsed"]);
                double percent = (used / grandTotal) * 100;

                s.Points.AddXY(material, percent);
            }

            pieChart.Series.Add(s);
        }

        // EXPENSE DISTRIBUTION CHART (Monthly)
        private void LoadExpenseChart()
        {
            pieChart.Series.Clear();
            pieChart.ChartAreas.Clear();
            pieChart.Legends.Clear();
            pieChart.Titles.Clear();

            ChartArea area = new ChartArea("MainArea")
            {
                BackColor = Color.White
            };

            pieChart.ChartAreas.Add(area);

            Legend lg = new Legend
            {
                Docking = Docking.Bottom,
                Font = new Font("Segoe UI", 9F)
            };

            pieChart.Legends.Add(lg);

            Series series = new Series("Expenses")
            {
                ChartType = SeriesChartType.Pie, 
                IsValueShownAsLabel = true,
                LabelFormat = "0.##'%'",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };

            // Fetch payments grouped by reason
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

            // Calculate total expenses
            foreach (DataRow row in dt.Rows)
            {
                grandTotal += Convert.ToDouble(row["TotalAmount"]);
            }

            // Convert each category into percentage
            foreach (DataRow row in dt.Rows)
            {
                string reason = row["Reason"].ToString();
                double amount = Convert.ToDouble(row["TotalAmount"]);
                double percent = (amount / grandTotal) * 100;

                series.Points.AddXY(reason, percent);
            }

            pieChart.Series.Add(series);

            pieChart.Dock = DockStyle.Fill;
            pieChart.Refresh();
        }

        // WORKER CATEGORY COUNTS
        private void LoadWorkerCategoryData()
        {
            LoadWorkerCount(lblLabourers, "Labor");
            LoadWorkerCount(lblDrivers, "Driver");
            LoadWorkerCount(lblLoaders, "Loader");
            LoadWorkerCount(lblOperators, "Machine Operator");
        }

        // Generic method to get worker count by role
        private void LoadWorkerCount(Label lbl, string role)
        {
            string query = "SELECT COUNT(*) FROM Workers WHERE Role=@role";

            var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@role", role)
            };

            int count = Convert.ToInt32(
                DBHelper.ExecuteScalar(query, parameters)
            );

            lbl.Text = count.ToString();
        }

        // RAW MATERIAL STOCK COUNTS
        private void LoadRawMaterialCards()
        {
            LoadMaterial(lblCement, "Cement");
            LoadMaterial(lblSand, "Sand");
            LoadMaterial(lblCrush, "Crush");
            LoadMaterial(lblSteel, "Steel");
            LoadMaterial(lblOil, "Mold Oil");
        }

        // Generic method to fetch stock of a material
        private void LoadMaterial(Label lbl, string material)
        {
            string query = @"
                SELECT ISNULL(SUM(Quantity),0)
                FROM RawMaterial
                WHERE UPPER(LTRIM(RTRIM(Name))) = UPPER(LTRIM(RTRIM(@name)))
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